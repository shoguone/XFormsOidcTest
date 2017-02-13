using IdentityModel.Client;
using System;
using System.Net;
using System.Net.Http;
using Xamarin.Forms;
using XFormsOidcTest.Pages;

namespace XFormsOidcTest.Logic
{
    public class Authenticator
    {
        public delegate void AuthResult(string result);
        public event AuthResult OnAuth;

        string GenerateUniqId { get { return Guid.NewGuid().ToString("N"); } }
        string currentCSRFToken;

        AuthorizeResponse authResponse;

        AuthPage authPage;

        string startUrl;
        string GenerateStartUrl
        {
            get
            {
                var request = new AuthorizeRequest(ConnectionSettings.AuthorizeEndpoint);
                currentCSRFToken = GenerateUniqId;
                startUrl = request.CreateAuthorizeUrl(
                    clientId: ConnectionSettings.ClientId,
                    responseType: ConnectionSettings.ResponseType,
                    //responseMode: ConnectionSettings.ResponseMode,
                    scope: ConnectionSettings.Scope,
                    redirectUri: ConnectionSettings.RedirectUrl,
                    state: currentCSRFToken,
                    nonce: GenerateUniqId);
                return startUrl;
            }
        }

        public Authenticator()
        {
        }

        public Page InitAuth()
        {
            authPage = new AuthPage();
            authPage.OnLogin += AuthPage_OnLogin;

            authPage.Init(GenerateStartUrl, ConnectionSettings.RedirectUri);

            return authPage;
        }

        void AuthPage_OnLogin(string url)
        {
            // parse response
            authResponse = new AuthorizeResponse(url);

            // CSRF check
            if (authResponse.Values.ContainsKey("state"))
            {
                var state = authResponse.Values["state"];
                if (state.Equals(currentCSRFToken))
                {
                    OnAuth?.Invoke(authResponse.AccessToken);
                    return;
                }
            }
            OnAuth?.Invoke("CSRF!");
        }

        public async void CallService()
        {
            string results = "";
            if (authResponse == null)
            {
                results = "_authResponse == null";
                OnAuth?.Invoke(results);
                return;
            }

            HttpResponseMessage response;
            //var baseAddress = new Uri(Constants.GetHost);
            using (var handler = new HttpClientHandler { UseCookies = false })
            using (var client = new HttpClient(handler) { BaseAddress = ConnectionSettings.RedirectUri })
            {
                var message = new HttpRequestMessage(HttpMethod.Get, ConnectionSettings.ServicePath);
                client.SetBearerToken(authResponse.AccessToken);
                try
                {
                    response = await client.SendAsync(message);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        //resultsText.Text = JArray.Parse(json).ToString();
                        results = json;
                    }
                    else
                    {
                        results = response.StatusCode.ToString();
                    }
                }
                catch (Exception ex)
                {
                    results = ex.ToString();
                }
            }
            OnAuth?.Invoke(results);
        }

        //void Start()
        //{
        //    var request = new AuthorizeRequest(ConnectionSettings.AuthorizeEndpoint);
        //    currentCSRFToken = GenerateUniqId;
        //    var startUrl = request.CreateAuthorizeUrl(
        //        clientId: ConnectionSettings.ClientId,
        //        responseType: ConnectionSettings.ResponseType,
        //        //responseMode: ConnectionSettings.ResponseMode,
        //        scope: ConnectionSettings.Scope,
        //        redirectUri: ConnectionSettings.RedirectUrl,
        //        state: currentCSRFToken,
        //        nonce: GenerateUniqId);

        //}

    }
}
