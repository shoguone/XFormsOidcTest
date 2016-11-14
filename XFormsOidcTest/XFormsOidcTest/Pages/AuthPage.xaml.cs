using IdentityModel.Client;
using System;
using Xamarin.Forms;
using XFormsOidcTest.Logic;

namespace XFormsOidcTest.Pages
{
    public partial class AuthPage : ContentPage
    {
        public delegate void AuthResult(AuthorizeResponse authResponse);
        public AuthResult OnAuth;

        Settings _settings;
        AuthorizeResponse _authResponse;

        string _UniqId { get { return Guid.NewGuid().ToString("N"); } }
        string _currentCSRFToken;

        public AuthPage()
        {
            InitializeComponent();

            loginWebView.Navigating += LoginWebView_Navigating;
        }

        public void Start(Settings settings)
        {
            _settings = settings;

            var request = new AuthorizeRequest(_settings.AuthorizeEndpoint);
            _currentCSRFToken = _UniqId;
            var startUrl = request.CreateAuthorizeUrl(
                clientId: _settings.ClientId,
                responseType: _settings.ResponseType,
                //responseMode: _settings.ResponseMode,
                scope: _settings.Scope,
                redirectUri: _settings.RedirectUrl,
                state: _currentCSRFToken,
                nonce: _UniqId);

            loginWebView.Source = new Uri(startUrl);
        }

        private void LoginWebView_Navigating(object sender, WebNavigatingEventArgs e)
        {
            var uri = new Uri(e.Url);
            if (uri.Authority.Equals(_settings.RedirectUri.Authority) && e.Url.StartsWith(_settings.RedirectUrl))
            {
                // parse response
                _authResponse = new AuthorizeResponse(e.Url);

                // CSRF check
                if (_authResponse.Values.ContainsKey("state"))
                {
                    var state = _authResponse.Values["state"];
                    if (state.Equals(_currentCSRFToken))
                    {
                        ReturnResult(_authResponse, e);
                        return;
                    }
                }
                ReturnResult(null, e);
            }
        }

        async void ReturnResult(AuthorizeResponse authResponse, WebNavigatingEventArgs e)
        {
            OnAuth?.Invoke(authResponse);
            await Navigation.PopAsync(true);
        }

    }
}
