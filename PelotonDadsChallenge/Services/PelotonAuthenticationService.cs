﻿using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using PelotonDadsChallenge.Configuration;
using Flurl.Http;
using PelotonDadsChallenge.Models;
using System;

namespace PelotonDadsChallenge.Services
{
    public class PelotonAuthenticationService : IPelotonAuthenticationService
    {
        private PelotonOptions _pelotonOptions;

        public PelotonAuthenticationService(IOptions<PelotonOptions> pelotonOptions)
        {
            _pelotonOptions = pelotonOptions.Value;
        }

        public async Task<PelotonAuthenticationResponse> Authenticate(CookieSession session)
        {
            var uri = $"{_pelotonOptions.BaseUri}/auth/login";

            var result = await session.Request(uri)
                .WithHeader("User-Agent", "PostmanRuntime/7.26.10")
                .WithHeader("Content-Type", "application/json")
                .PostJsonAsync(new
                {
                    username_or_email = _pelotonOptions.Username,
                    password = _pelotonOptions.Password
                }).ReceiveJson<PelotonAuthenticationResponse>();

            return result;
        }
    }
}
