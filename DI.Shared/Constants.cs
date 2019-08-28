namespace DI.Shared
{
    public static class Constants
    {
#if PRODUCTION
#warning Using PRODUCTION configuration: YOUR API URL
        public const string WebAPIBaseAddress = "YOUR API URL";
        public const string DEFAULT_USER_LOGIN = "mobiletestuser";
        public const string DEFAULT_USER_PASSWORD = "skMQathY";
        public const string HOCKEY_APP_ANDROID = "YOUR APP KEY";
#else
#warning Using DEVELOPMENT configuration: YOUR API URL
        public const string WebAPIBaseAddress = "YOUR API URL";
        public const string DEFAULT_USER_LOGIN = "mobilebinder";
        public const string DEFAULT_USER_PASSWORD = "Lq9Nuw53Udg0";
        public const string HOCKEY_APP_ANDROID = "YOUR APP KEY";
#endif

        public const string ANDROID_APP_LINK = "";

        //Smooch API
        public const string SMOOCH_API_ADDRESS = "https://api.smooch.io/v1/";
        public const string SMOOCH_APP_TOKEN = "YOUR APP TOKEN";
        public const string SMOOCH_APP_ID = "YOUR APP ID";
        public const string SMOOCH_ACCOUNT_TOKEN = "YOUR ACCOUNT TOKEN";
        public const string SMOOCH_APP_KEY_ID = "YOUR APP KEY ID";
        public const int SMOOCH_SYNC_PERIOD = 60;



        //Intent Extras
        public const string ID = "ID";
        public const string AGE = "AGE";
        public const string PHONE = "PHONE";
        public const string WEBURL = "WEBURL";
        public const string DRIVINGEXPERIENCE = "DRIVINGEXPERIENCE";
        public const string PARENT = "PARENT";

        //Acceleration Limits
        public const int NormalAccelLimit = 306;
        public const int NormalBrakeLimit = 408;
    }
}
