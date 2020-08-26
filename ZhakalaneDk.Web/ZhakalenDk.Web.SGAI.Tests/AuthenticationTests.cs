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
        string tokenPath = "Tokens";

        [TestMethod]
        public void AuthAndStore ()
        {
            if ( auth == null )
            {
                Assert.Fail("Authenticator was null");
            }

            Assert.IsTrue(auth.AuthAndStoreToken(tokenPath));
        }

        [TestMethod]
        public void DeleteResponseTokens ()
        {
            if ( auth == null || !auth.AuthAndStoreToken(tokenPath) )
            {
                Assert.Fail("Authenticator was null");
            }

            auth.EraseTokens();

            Assert.IsFalse(File.Exists($"{Assembly.GetExecutingAssembly().Location}/{tokenPath}/Google.Apis.Auth.OAuth2.Responses.TokenResponse-user"));

        }

    }
}
