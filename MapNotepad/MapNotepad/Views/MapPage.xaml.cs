﻿using MapNotepad.ViewModels;
using System;
using System.IO;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace MapNotepad.Views
{
    public partial class MapPage : BaseContentPage
    {
        public MapPage()
        {
            MessagingCenter.Subscribe<PinsPageViewModel, Position>(
                this,
                "MoveToPosition",
                (sender, position) => {
                    map.MoveCamera(CameraUpdateFactory.NewPosition(position));
                });

            MessagingCenter.Subscribe<MapPageViewModel, Position>(
                this,
                "MoveToPosition",
                (sender, position) => {
                    map.MoveCamera(CameraUpdateFactory.NewPosition(position));
                });

            MessagingCenter.Subscribe<MapPageViewModel, Position>(
                this,
                "MoveToMyLocation",
                async (sender, position) => {
                    await map.MoveCamera(CameraUpdateFactory.NewPosition(position));
                });

            InitializeComponent();

            try
            {
                var assembly = typeof(MapPage).Assembly;

                Stream stream = assembly.GetManifestResourceStream("MapNotepad.Themes.DarkMapTheme.txt");
                string Json = "";
                using (var reader = new StreamReader(stream))
                {
                    Json = reader.ReadToEnd();
                }

                map.MapStyle = MapStyle.FromJson(Json);
            }
            catch (Exception e)
            {
            }

        }
    }
}