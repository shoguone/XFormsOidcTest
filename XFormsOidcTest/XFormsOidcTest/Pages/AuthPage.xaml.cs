using IdentityModel.Client;
using System;
using Xamarin.Forms;
using XFormsOidcTest.Logic;

namespace XFormsOidcTest.Pages
{
    public partial class AuthPage : ContentPage
    {
        public delegate void LoginResult(string url);
        public LoginResult OnLogin;

        string startUrl;
        Uri redirectUri;

        public AuthPage()
        {
            InitializeComponent();

            loginWebView.Navigating += LoginWebView_Navigating;
        }

        public void Init(string startUrl, Uri redirectUri)
        {
            this.startUrl = startUrl;
            this.redirectUri = redirectUri;

            loginWebView.Source = new Uri(this.startUrl);
        }

        private void LoginWebView_Navigating(object sender, WebNavigatingEventArgs e)
        {
            var uri = new Uri(e.Url);
            //if (uri.Authority.Equals(ConnectionSettings.RedirectUri.Authority) && e.Url.StartsWith(ConnectionSettings.RedirectUrl))
            if (uri.Authority.Equals(redirectUri.Authority) && e.Url.StartsWith(redirectUri.OriginalString))
            {
                // parse response
                var _authResponse = new AuthorizeResponse(e.Url);

                OnLogin?.Invoke(e.Url);
            }
        }

    }
}
