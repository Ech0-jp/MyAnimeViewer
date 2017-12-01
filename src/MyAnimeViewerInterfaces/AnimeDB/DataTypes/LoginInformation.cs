namespace MyAnimeViewerInterfaces.AnimeDB
{
    public class LoginInformation
    {
        public DEFAULT Default;
        public class DEFAULT
        {
            public string username;
            public string password;
        }

        public OAUTH OAuth;
        public class OAUTH
        {
            public string authorization_pin;
            public string refresh_token;
        };
    };

    
}
