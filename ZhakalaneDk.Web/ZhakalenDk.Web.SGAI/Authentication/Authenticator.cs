using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;

namespace ZhakalenDk.Web.SGAI.Authentication
{
    /// <summary>
    /// Represents a Google Auth 2.0 authenticator
    /// </summary>
    public class Authenticator
    {
        /// <summary>
        /// The associated credentials for the authenticated user
        /// </summary>
        public UserCredential UserCreds { get; private set; }

        /// <summary>
        /// The scope of which the authenticator should request access to
        /// </summary>
        public string[] Scope { get; set; } = { DriveService.Scope.Drive };

        /// <summary>
        /// The name of the application that wants access. (<i>As default this will return <see cref="Assembly.GetName()"/></i>)
        /// </summary>
        public string AppName { get; set; } = Assembly.GetExecutingAssembly().GetName().Name;

        /// <summary>
        /// The name of the file that contains the secret credentials. (See: <see href="https://github.com/ZhakalenDk/Simple_Google_API_Intergration/blob/Development/README.md#client-id--client-secret">Client ID and Client Secret</see> )
        /// </summary>
        public string SecretFileName { get; }

        /// <summary>
        /// The gateway service that allows for the access of the Google API functionality
        /// </summary>
        public DriveService GetService { get; private set; }

        /// <summary>
        /// The path to where the Google Response token was stored
        /// </summary>
        public string TokenFolder { get; private set; }

        protected bool InitiateService ()
        {
            if ( UserCreds != null )
            {
                GetService = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = UserCreds,
                    ApplicationName = AppName
                });

                return true;
            }

            return false;

        }

        /// <summary>
        /// This will ask a given user for their permission to access their information on their Google Drive, defined by the given <see cref="Scope"/>.
        /// <br/>
        /// If the authentication is successful it will store the auth 2.0 respons token from Google at the end of the specified subfolder-structure.
        /// <br/>
        /// If the authentication was not succesful this will return false.
        /// </summary>
        /// <param name="_storeAt">The subfolder structure where the token is stored upong validation</param>
        /// <returns></returns>
        public bool AuthAndStoreToken (string _storeAt)
        {
            using ( var stream = new FileStream(SecretFileName, FileMode.Open, FileAccess.Read) )
            {
                UserCreds = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scope,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(_storeAt, true)).Result;
            }

            TokenFolder = _storeAt;

            return InitiateService();
        }

        /// <summary>
        /// Clears the subfolder specified by the <see cref="TokenFolder"/> where the Google Response Token was stored
        /// </summary>
        public void EraseTokens ()
        {
            new FileDataStore(TokenFolder, true).ClearAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_secretFileName">The name of the Google Credentials file that contains the <see href="https://github.com/ZhakalenDk/Simple_Google_API_Intergration/blob/Development/README.md#client-id--client-secret">Client ID and Client Secret</see> </param>
        public Authenticator (string _secretFileName)
        {
            SecretFileName = _secretFileName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_secretFileName">The name of the Google Credentials file that contains the <see href="https://github.com/ZhakalenDk/Simple_Google_API_Intergration/blob/Development/README.md#client-id--client-secret">Client ID and Client Secret</see> </param>
        /// <param name="_appName">The name of the application that wants access to the Drive API</param>
        public Authenticator (string _secretFileName, string _appName)
        {
            SecretFileName = _secretFileName;
            AppName = _appName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_secretFileName">The name of the Google Credentials file that contains the <see href="https://github.com/ZhakalenDk/Simple_Google_API_Intergration/blob/Development/README.md#client-id--client-secret">Client ID and Client Secret</see> </param>
        /// <param name="_appName">The name of the application that wants access to the Drive API</param>
        /// <param name="_scope">The scope that defines what kind of access this application is requesting</param>
        public Authenticator (string _secretFileName, string _appName, string[] _scope)
        {
            SecretFileName = _secretFileName;
            AppName = _appName;
            Scope = _scope;
        }
    }
}
