using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Shared.Dtos;

namespace Infrastructure.Clients
{
    public class UserManagementHttpClient : IUserManagementHttpClient
    {
        private readonly HttpClient _httpClient;

        public UserManagementHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<AuthResult?> ValidateCredentials(string? email, string? password, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return null;

            var requestData = new
            {
                Email = email,
                Password = password
            };

            var response = await _httpClient.PostAsJsonAsync("/api/users/ValidateCredentials", requestData, cancellationToken);


            if(response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return null;

            response.EnsureSuccessStatusCode();

            var authResult = await response.Content.ReadFromJsonAsync<AuthResult>(cancellationToken);
           
            return authResult;

        }
    }
}
