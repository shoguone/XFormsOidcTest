using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFormsOidcTest.Logic
{
    public class ConnectionSettings
    {
        public static string IdentityServerUrl = "http://38.89.137.36:85/identity/";
        public static string AuthorizeEndpoint { get { return IdentityServerUrl + "connect/authorize"; } }

        public static string RedirectUrl = "http://38.89.137.36";
        public static Uri RedirectUri
        {
            get { return new Uri(RedirectUrl); }
        }
        public static string ServicePath = "GPS/GetAllVehicles";
        public static string ClientId = "EyeRideGPS";
        public static string ResponseType = "id_token token";
        public static string Scope = "openid profile roles";
        public static string ResponseMode = "form_post";

        public static string accessToken;
        public static string workingAccessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiYWRtaW4iOnRydWUsInJvbGUiOiJNb2JpbGUgVXNlcnMiLCJDb21wYW55IjoibXlfY29tcGFueSJ9.-y3ebp9OzM82C8384dp51mEnFVa_gb6q4CJldbSWy_I";
    }
}
