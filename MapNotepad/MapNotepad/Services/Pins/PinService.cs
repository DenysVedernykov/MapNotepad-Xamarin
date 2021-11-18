﻿using MapNotepad.Helpers.ProcessHelpers;
using MapNotepad.Models;
using MapNotepad.Services.Repository;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapNotepad.Services.Pins
{
    public class PinService : IPinService
    {
        private IRepositoryService _repositoryService;

        public PinService(IRepositoryService repositoryService)
        {
            _repositoryService = repositoryService;
        }

        public async Task<AOResult<int>> AddPinAsync(UserPin pin)
        {
            var result = new AOResult<int>();

            try
            {
                var response = _repositoryService.InsertAsync(pin);

                if(response == null)
                {
                    result.SetFailure();
                }
                else
                {
                    result.SetSuccess(response.Result);
                }
            }
            catch (Exception ex)
            {
                result.SetError("0", "Exception PinsService AddPin", ex);
            }

            return result;
        }

        public async Task<AOResult<int>> UpdatePinAsync(UserPin pin)
        {
            var result = new AOResult<int>();

            try
            {
                var response = _repositoryService.UpdateAsync(pin);

                if (response == null)
                {
                    result.SetFailure();
                }
                else
                {
                    result.SetSuccess(response.Result);
                }
            }
            catch (Exception ex)
            {
                result.SetError("0", "Exception PinsService UpdatePin", ex);
            }

            return result;
        }

        public async Task<AOResult<int>> DeletePinAsync(UserPin pin)
        {
            var result = new AOResult<int>();

            try
            {
                var response = _repositoryService.DeleteAsync(pin);

                if (response == null)
                {
                    result.SetFailure();
                }
                else
                {
                    result.SetSuccess(response.Result);
                }
            }
            catch (Exception ex)
            {
                result.SetError("0", "Exception PinsService DeletePin", ex);
            }

            return result;
        }

        public async Task<AOResult<List<UserPin>>> AllPinsAsync()
        {
            var result = new AOResult<List<UserPin>>();

            try
            {
                var response = _repositoryService.GetAllRowsAsync<UserPin>();

                if (response == null)
                {
                    result.SetFailure();
                }
                else
                {
                    result.SetSuccess(response.Result);
                }
            }
            catch (Exception ex)
            {
                result.SetError("0", "Exception PinsService AllPins", ex);
            }

            return result;
        }

        public async Task<AOResult<UserPin>> GetByIdAsync(int id)
        {
            var result = new AOResult<UserPin>();

            try
            {
                var response = _repositoryService.GetAllRowsAsync<UserPin>();

                if (response == null)
                {
                    result.SetFailure();
                }
                else
                {
                    result.SetSuccess(response.Result.Where(row => row.Id == id).FirstOrDefault());
                }
            }
            catch (Exception ex)
            {
                result.SetError("0", "Exception PinsService AllPins", ex);
            }

            return result;
        }

        public async Task<AOResult<List<UserPin>>> SearchPinsAsync(string text)
        {
            text = text.ToLower();

            var result = new AOResult<List<UserPin>>();

            try
            {
                var response = _repositoryService.GetAllRowsAsync<UserPin>();

                if (response == null)
                {
                    result.SetFailure();
                }
                else
                {
                    var list = response.Result.Where(
                        (row) => {
                            var result = row.Longitude.ToString().ToLower().IndexOf(text) != -1
                            | row.Latitude.ToString().ToLower().IndexOf(text) != -1;

                            if (!string.IsNullOrEmpty(row.Label))
                            {
                                result |= row.Label.ToLower().IndexOf(text) != -1;
                            }

                            if (!string.IsNullOrEmpty(row.Address))
                            {
                                result |= row.Address.ToLower().IndexOf(text) != -1;
                            }

                            if (!string.IsNullOrEmpty(row.Description))
                            {
                                result |= row.Description.ToLower().IndexOf(text) != -1;
                            }

                            return result;
                        }
                    ).ToList();

                    result.SetSuccess(list);
                }
            }
            catch (Exception ex)
            {
                result.SetError("0", "Exception PinsService AllPins", ex);
            }

            return result;
        }
    }
}