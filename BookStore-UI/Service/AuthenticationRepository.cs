using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using BookStore_UI.Contracts;
using BookStore_UI.Models;
using BookStore_UI.Providers;
using BookStore_UI.Static;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;

namespace BookStore_UI.Service {
    public class AuthenticationRepository:IAuthenticationRepository {
        private readonly IHttpClientFactory _client;
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public AuthenticationRepository(IHttpClientFactory client, ILocalStorageService localStorage, AuthenticationStateProvider authenticationStateProvider) {
            _client = client;
            _localStorage = localStorage;
            _authenticationStateProvider = authenticationStateProvider;
        }
        public async Task<bool> Register(RegistrationModel user) {
            try {
                var request = new HttpRequestMessage(HttpMethod.Post, Endpoints.RegisterEndpoint);
                request.Content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

                var client = _client.CreateClient();
                var response = await client.SendAsync(request);

                return response.IsSuccessStatusCode;
            } catch (Exception e) {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<bool> Login(LoginModel user) {
            try {
                var request = new HttpRequestMessage(HttpMethod.Post, Endpoints.LoginEndpoint);
                request.Content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

                var client = _client.CreateClient();
                var response = await client.SendAsync(request);

                if (!response.IsSuccessStatusCode) return false;

                var content = await response.Content.ReadAsStringAsync();

                var token = JsonConvert.DeserializeObject<TokenResponse>(content);

                //store token
                await _localStorage.SetItemAsync("authToken", token.Token);

                //change auth state of the app
                await ((ApiAuthenticationStateProvider)_authenticationStateProvider).LoggedIn();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token.Token);

                return true;
            } catch (Exception e) {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task Logout() {
            await _localStorage.RemoveItemAsync("authToken");
            ((ApiAuthenticationStateProvider)_authenticationStateProvider).LoggedOut();
        }
    }
}