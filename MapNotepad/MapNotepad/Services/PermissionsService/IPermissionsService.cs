﻿using MapNotepad.Helpers.ProcessHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace MapNotepad.Services.PermissionsService
{
    public interface IPermissionsService
    {
        Task<PermissionStatus> CheckStatusAsync<T>() where T : Permissions.BasePermission, new();

        Task<PermissionStatus> RequestAsync<T>() where T : Permissions.BasePermission, new();
    }
}