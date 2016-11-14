using IdentityModel.Client;
using System;
using System.Net;
using System.Net.Http;
using Xamarin.Forms;
using XFormsOidcTest.Logic;
using XFormsOidcTest.Pages;

namespace XFormsOidcTest
{
    public partial class MainPage : ContentPage
    {
        /***/
        Settings _settings = new Settings
        {
            IdentityServerUrl = "http://38.89.137.36:85/identity/",
            RedirectUrl = "http://38.89.137.36",
            ServicePath = "GPS/GetAllVehicles",
            ClientId = "EyeRideGPS",
            ResponseType = "id_token token",
            Scope = "openid profile roles",
            ResponseMode = "form_post"
        };
        /***
        Settings _settings = new Settings
        {
            IdentityServerUrl = "https://192.168.1.2:44333/core/",
            RedirectUrl = "http://192.168.1.2:62874",
            ServicePath = "Value",
            ClientId = "mvc",
            ResponseType = "id_token token",
            Scope = "openid profile roles",
            ResponseMode = "form_post"
        };
        /***/

        AuthorizeResponse _authResponse;

        public MainPage()
        {
            InitializeComponent();

            RequestToken();
        }

        private async void RequestToken()
        {
            var authPage = new AuthPage();
            authPage.OnAuth += AuthPage_OnAuth;
            await Navigation.PushAsync(authPage);
            authPage.Start(_settings);
        }

        private void AuthPage_OnAuth(AuthorizeResponse response)
        {
            if (response == null)
                resultsText.Text = "CSRF token doesn't match";
            else
            {
                _authResponse = response;
                resultsText.Text = _authResponse.AccessToken;
                CallService();
            }
        }

        private async void CallService()
        {
            if (_authResponse == null)
            {
                resultsText.Text = "_authResponse == null";
                return;
            }

            HttpResponseMessage response;
            //var baseAddress = new Uri(Constants.GetHost);
            using (var handler = new HttpClientHandler { UseCookies = false })
            using (var client = new HttpClient(handler) { BaseAddress = _settings.RedirectUri })
            {
                var message = new HttpRequestMessage(HttpMethod.Get, _settings.ServicePath);
                client.SetBearerToken(_authResponse.AccessToken);
                try
                {
                    response = await client.SendAsync(message);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        //resultsText.Text = JArray.Parse(json).ToString();
                        resultsText.Text = json;
                    }
                    else
                    {
                        resultsText.Text = response.StatusCode.ToString();
                    }
                }
                catch (Exception ex)
                {
                    resultsText.Text = ex.ToString();
                }
            }
        }

    }
}
