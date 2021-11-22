﻿using MapNotepad.Helpers;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MapNotepad.ViewModels
{
    public class ScanCodePageViewModel : BaseViewModel
    {
        public ScanCodePageViewModel(INavigationService navigationService)
               : base(navigationService)
        {

        }

        #region -- Public properties --

        private ICommand _goBackCommand;
        public ICommand GoBackCommand => _goBackCommand ??= SingleExecutionCommand.FromFunc(OnGoBackCommandAsync);

        #endregion

        #region -- Private methods --

        private Task OnGoBackCommandAsync()
        {
            return _navigationService.GoBackAsync();
        }

        #endregion

    }
}