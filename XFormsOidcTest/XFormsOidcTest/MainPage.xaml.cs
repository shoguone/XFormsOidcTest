using IdentityModel.Client;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using XFormsOidcTest.Logic;
using XFormsOidcTest.Pages;

namespace XFormsOidcTest
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            IdentityServerUrlLabel.Text = ConnectionSettings.IdentityServerUrl;
        }

        async Task Init()
        {
            var authenticator = new Authenticator();
            authenticator.OnAuth += OnAuth;
            var page = authenticator.InitAuth();

            await Navigation.PushAsync(page);

        }

        async void OnAuth(string result)
        {
            resultsText.Text = result;

            await Navigation.PopAsync();
            //await Navigation.PushAsync(new Results());
        }

        async void Start_Clicked(object sender, EventArgs e)
        {
            await Init();

        }

        void Call_Clicked(object sender, EventArgs e)
        {
            //CallService();
        }
    }
}
