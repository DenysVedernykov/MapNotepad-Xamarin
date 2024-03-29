﻿using MapNotepad.Helpers.ProcessHelpers;
using MapNotepad.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MapNotepad.Services.Repository
{
    public interface IRepositoryService
    {
        Task<int> InsertAsync<T>(T entity) where T : IEntityBase, new();

        Task<int> UpdateAsync<T>(T entity) where T : IEntityBase, new();

        Task<int> DeleteAsync<T>(T entity) where T : IEntityBase, new();

        Task<List<T>> GetAllRowsAsync<T>() where T : IEntityBase, new();
    }
}
