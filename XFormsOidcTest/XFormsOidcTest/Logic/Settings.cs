using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFormsOidcTest.Logic
{
    public class Settings
    {
        public string IdentityServerUrl { get; set; }
        public string AuthorizeEndpoint { get { return IdentityServerUrl + "connect/authorize"; } }

        public string RedirectUrl { get; set; }
        private Uri _redirectUri;
        public Uri RedirectUri
        {
            get
            {
                if (_redirectUri == null || !_redirectUri.OriginalString.Equals(RedirectUrl))
                    _redirectUri = new Uri(RedirectUrl);
                return _redirectUri;
            }
            set
            {
                _redirectUri = value;
            }
        }

        public string ServicePath { get; set; }
        public string ClientId { get; set; }
        public string ResponseType { get; set; }
        public string Scope { get; set; }
        public string ResponseMode { get; set; }
    }
}
