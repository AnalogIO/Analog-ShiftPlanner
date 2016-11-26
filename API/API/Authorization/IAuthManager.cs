﻿using System.Net.Http.Headers;
using Data.Models;

namespace API.Authorization
{
    public interface IAuthManager
    {
        bool AuthenticateToken(string apikey);
        Organization GetOrganizationByApiKey(string apiKey);
        Organization GetOrganizationByHeader(HttpRequestHeaders headers);
        Manager GetManagerByHeader(HttpRequestHeaders headers);
        bool ValidateOrganizationApiKey(string apiKey);
    }
}