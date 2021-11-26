﻿using Acr.UserDialogs;
using MapNotepad.Helpers;
using MapNotepad.Models;
using MapNotepad.Services.Authorization;
using MapNotepad.Services.PermissionsService;
using MapNotepad.Services.Pins;
using MapNotepad.Services.SettingsManager;
using Plugin.Geolocator;
using Prism.Navigation;
using Prism.Unity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace MapNotepad.ViewModels
{
    class AddPinsPageViewModel : BaseViewModel
    {
        private Pin _currentPin;

        private ResourceDictionary _resourceDictionary;

        private IPinService _pinService;

        private IAuthorizationService _authorizationService;

        private IPermissionsService _permissionsService;

        private ISettingsManagerService _settingsManagerService;

        public AddPinsPageViewModel(
            INavigationService navigationService,
            IPinService pinService,
            IAuthorizationService authorizationService,
            IPermissionsService permissionsService,
            ISettingsManagerService settingsManagerService) 
            : base(navigationService)
        {
            _pinService = pinService;
            _authorizationService = authorizationService;
            _permissionsService = permissionsService;
            _settingsManagerService = settingsManagerService;

            _currentPin = new Pin();
            _currentPin.Label = "Point";

            InitialCameraUpdate = CameraUpdateFactory.NewPositionZoom(new Position(35.71d, 139.81d), 12d);

            ICollection<ResourceDictionary> mergedDictionaries = PrismApplication.Current.Resources.MergedDictionaries;
            _resourceDictionary = mergedDictionaries.FirstOrDefault();

            BorderColorLabel = (Color)_resourceDictionary["LightGray"];
            BorderColorLongitude = (Color)_resourceDictionary["LightGray"];
            BorderColorLatitude = (Color)_resourceDictionary["LightGray"];
        }

        #region -- Public properties --

        private Color _borderColorLabel;
        public Color BorderColorLabel
        {
            get => _borderColorLabel;
            set => SetProperty(ref _borderColorLabel, value);
        }

        private Color _borderColorLongitude;
        public Color BorderColorLongitude
        {
            get => _borderColorLongitude;
            set => SetProperty(ref _borderColorLongitude, value);
        }

        private Color _borderColorLatitude;
        public Color BorderColorLatitude
        {
            get => _borderColorLatitude;
            set => SetProperty(ref _borderColorLatitude, value);
        }

        private string _label;
        public string Label
        {
            get => _label;
            set => SetProperty(ref _label, value);
        }

        private string _description;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private string _latitude;
        public string Latitude
        {
            get => _latitude;
            set => SetProperty(ref _latitude, value);
        }

        private string _longitude;
        public string Longitude
        {
            get => _longitude;
            set => SetProperty(ref _longitude, value);
        }

        private bool _isEnableSaveButton;
        public bool IsEnableSaveButton
        {
            get => _isEnableSaveButton;
            set => SetProperty(ref _isEnableSaveButton, value);
        }

        private bool _isShowingUser;
        public bool IsShowingUser
        {
            get => _isShowingUser;
            set => SetProperty(ref _isShowingUser, value);
        }

        private CameraUpdate _initialCameraUpdate;
        public CameraUpdate InitialCameraUpdate
        {
            get => _initialCameraUpdate;
            set => SetProperty(ref _initialCameraUpdate, value);
        }

        private ICommand _goBackCommand;
        public ICommand GoBackCommand => _goBackCommand ??= SingleExecutionCommand.FromFunc(OnGoBackCommandAsync);

        private ICommand _saveCommand;
        public ICommand SaveCommand => _saveCommand ??= SingleExecutionCommand.FromFunc(OnSaveCommandAsync);

        private ICommand _moveToMyLocationCommand;
        public ICommand MoveToMyLocationCommand => _moveToMyLocationCommand ??= SingleExecutionCommand.FromFunc(OnMoveToMyLocationCommandAsync);

        private ICommand _mapClickedCommand;
        public ICommand MapClickedCommand => _mapClickedCommand ??= SingleExecutionCommand.FromFunc<Position>(OnMapClickedCommandAsync);

        #endregion

        #region -- IInitializeAsync implementation --

        public async override Task InitializeAsync(INavigationParameters parameters)
        {
            if (_settingsManagerService.IsNightThemeEnabled)
            {
                MessagingCenter.Send<AddPinsPageViewModel, string>(this, "ThemeChange", "MapNotepad.Themes.DarkMapTheme.txt");
            }
            else
            {
                MessagingCenter.Send<AddPinsPageViewModel, string>(this, "ThemeChange", "MapNotepad.Themes.LightMapTheme.txt");
            }

            await CheckPermissions();
        }

        #endregion

        #region -- Overrides --

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            double sample;

            base.OnPropertyChanged(args);

            bool updatePin = false;
            bool truePosition = double.TryParse(Longitude, out sample)
                && double.TryParse(Latitude, out sample);

            if (truePosition)
            {
                var lng = double.Parse(Longitude);
                var lat = double.Parse(Latitude);

                truePosition &= (-180 <= lng) && (lng <= 180) && (-90 <= lat) && (lat <= 90);
            }

            IsEnableSaveButton = !string.IsNullOrEmpty(Label) && truePosition;

            switch (args.PropertyName)
            {
                case nameof(Label):

                    if (string.IsNullOrEmpty(Label))
                    {
                        BorderColorLabel = (Color)_resourceDictionary["Error"];
                    }
                    else
                    {
                        BorderColorLabel = (Color)_resourceDictionary["LightGray"];
                    }

                    break;
                case nameof(Longitude):

                    if (double.TryParse(Longitude, out sample))
                    {
                        var lng = double.Parse(Longitude);

                        if(-180 <= lng && lng <= 180)
                        {
                            BorderColorLongitude = (Color)_resourceDictionary["LightGray"];
                        }
                        else
                        {
                            BorderColorLongitude = (Color)_resourceDictionary["Error"];
                        }
                    }
                    else
                    {
                        BorderColorLongitude = (Color)_resourceDictionary["Error"];
                    }

                    updatePin = true;

                    break;
                case nameof(Latitude):

                    if (double.TryParse(Latitude, out sample))
                    {
                        var lat = double.Parse(Latitude);

                        if (-90 <= lat && lat <= 90)
                        {
                            BorderColorLatitude = (Color)_resourceDictionary["LightGray"];
                        }
                        else
                        {
                            BorderColorLatitude = (Color)_resourceDictionary["Error"];
                        }
                    }
                    else
                    {
                        BorderColorLatitude = (Color)_resourceDictionary["Error"];
                    }

                    updatePin = true;

                    break;
            }

            if (truePosition && updatePin)
            {
                MessagingCenter.Send<AddPinsPageViewModel, Pin>(this, "DeletePin", _currentPin);

                _currentPin.Position = new Position(double.Parse(Latitude), double.Parse(Longitude));

                MessagingCenter.Send<AddPinsPageViewModel, Pin>(this, "AddPin", _currentPin);

                MessagingCenter.Send<AddPinsPageViewModel, Position>(
                    this,
                    "MoveToLocation",
                    _currentPin.Position);
            }
        }

        #endregion

        #region -- Private helpers --

        private async Task CheckPermissions()
        {
            IsShowingUser = await _permissionsService.RequestAsync<Permissions.LocationWhenInUse>() == Xamarin.Essentials.PermissionStatus.Granted;
        }

        #endregion

        #region -- Private methods --

        private async Task OnGoBackCommandAsync()
        {
            await _navigationService.GoBackAsync();
        }

        private async Task OnSaveCommandAsync()
        {
            var geoCoder = new Geocoder();

            var position = new Position(double.Parse(Latitude), double.Parse(Longitude));
            var possibleAddresses = await geoCoder.GetAddressesForPositionAsync(position);

            if (possibleAddresses != null)
            {
                var userPin = new UserPin()
                {
                    Autor = _authorizationService.Profile.Id,
                    Label = Label,
                    Address = possibleAddresses.FirstOrDefault(),
                    Description = Description,
                    Latitude = double.Parse(Latitude),
                    Longitude = double.Parse(Longitude),
                    Favorites = true,
                    CreationDate = DateTime.Now
                };

                var result = _pinService.AddPinAsync(userPin);

                if (result.Result.IsSuccess)
                {
                    userPin.Id = result.Result.Result;

                    MessagingCenter.Send<AddPinsPageViewModel, UserPin>(this, "AddPin", userPin);

                    await _navigationService.GoBackAsync();
                }
                else
                {
                    await UserDialogs.Instance.AlertAsync(new AlertConfig()
                    {
                        OkText = Resource.ResourceManager.GetString("Ok", Resource.Culture),
                        Message = Resource.ResourceManager.GetString("ErrorAddPin", Resource.Culture)
                    });
                }
            }
            else
            {
                await UserDialogs.Instance.AlertAsync(new AlertConfig()
                {
                    OkText = Resource.ResourceManager.GetString("Ok", Resource.Culture),
                    Message = Resource.ResourceManager.GetString("ErrorAddPin", Resource.Culture)
                });
            }
        }

        private async Task OnMoveToMyLocationCommandAsync()
        {
            await CheckPermissions();

            if (IsShowingUser)
            {
                try
                {
                    var locator = CrossGeolocator.Current;
                    
                    if (locator.IsGeolocationEnabled && locator.IsGeolocationAvailable)
                    {
                        var position = await locator.GetPositionAsync();

                        MessagingCenter.Send<AddPinsPageViewModel, Position>(
                            this,
                            "MoveToLocation",
                            new Position(position.Latitude, position.Longitude));
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }

        private Task OnMapClickedCommandAsync(Position position)
        {
            Longitude = position.Longitude.ToString();
            Latitude = position.Latitude.ToString();

            return Task.CompletedTask;
        }

        #endregion

    }
}
