﻿using MapNotepad.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MapNotepad.Services.Authorization
{
    public interface IAuthorizationService
    {
        bool Status { get; }

        User Profile { get; }

        bool CheckEmailForUse(string login);

        bool EmailMatching(string login);

        bool PasswordMatching(string password);

        bool Registration(User newUser);

        bool Login(string login, string password);

        void LogOut();
    }
}
