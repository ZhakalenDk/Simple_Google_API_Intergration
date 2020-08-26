using System;
using System.IO;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZhakalenDk.Web.SGAI.Authentication;

namespace ZhakalenDk.Web.SGAI.Tests.Auth
{
    [TestClass]
    public class AuthenticationTests
    {
        Authenticator auth = new Authenticator("credentials.json", "Unit Test Application");
        string tokenFolder = "Tokens";
        string tokenName = "Google.Apis.Auth.OAuth2.Responses.TokenResponse-user";

        [TestMethod]
        public void AuthAndStore ()
        {
            if ( auth == null )
            {
                Assert.Fail("Authenticator was null");
            }

            Assert.IsTrue(auth.AuthAndStoreToken(tokenFolder), $"Used [{auth.SecretFileName}] and tried to store Token in folder [{tokenFolder}]");
        }

        [TestMethod]
        public void DeleteResponseTokens ()
        {
            string fullPath = $"{ Assembly.GetExecutingAssembly().Location }/{ tokenFolder}";
            if ( auth == null || !auth.AuthAndStoreToken(tokenFolder) )
            {
                Assert.Fail("Authenticator was null");
            }

            auth.EraseTokens();

            Assert.IsFalse(File.Exists($"{fullPath}/{tokenName}"), $"File didn't exist at [{fullPath}]");
        }

    }
}
