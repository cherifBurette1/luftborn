using Microsoft.Extensions.Configuration;
using System;

namespace luftborn.Ground
{
    public static class Configurations
    {
        public static T Get<T>(string appSettingsKey, T defaultValue)
        {
            string text = ConfigurationManager[appSettingsKey];
            if (string.IsNullOrWhiteSpace(text))
                return defaultValue;

            try
            {
                var value = Convert.ChangeType(text, typeof(T));
                return (T)value;
            }
            catch
            {
                return defaultValue;
            }
        }
        public static IConfiguration ConfigurationManager { get; set; }

        public static bool IsTrackingIfUserAgentChanged
        {
            get
            {
                return Get<bool>("IsTrackingIfUserAgentChanged", true);
            }
        }
        public static string GetConnectionString(string appSettingsKey)
        {
            var cs = ConfigurationManager.GetConnectionString(appSettingsKey);
            if (cs != null)
                return cs;

            return null;
        }
        static string _ApiBaseUrl;
        public static string ApiBaseUrl
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_ApiBaseUrl))
                    _ApiBaseUrl = Get<string>("URLs:ApiBaseUrl", "http://localhost:5001");
                return _ApiBaseUrl;
            }
        }
        static string _APISignalRBackPlaneConnectionString;
        public static string APISignalRBackPlaneConnectionString
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_APISignalRBackPlaneConnectionString))
                    _APISignalRBackPlaneConnectionString = GetConnectionString("APISignalRBackPlaneConnectionString");
                return _APISignalRBackPlaneConnectionString;
            }
        }
        static string _HangfireSignalRBackPlaneConnectionString;
        public static string HangfireSignalRBackPlaneConnectionString
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_HangfireSignalRBackPlaneConnectionString))
                    _HangfireSignalRBackPlaneConnectionString = GetConnectionString("HangfireSignalRBackPlaneConnectionString");
                return _HangfireSignalRBackPlaneConnectionString;
            }
        }
        static string _RedisCacheConnectionString;
        public static string RedisCacheConnectionString
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_RedisCacheConnectionString))
                    _RedisCacheConnectionString = GetConnectionString("RedisCacheConnectionString");
                return _RedisCacheConnectionString;
            }
        }
        static string _HangfireConnectionString;
        public static string HangfireConnectionString
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_HangfireConnectionString))
                    _HangfireConnectionString = GetConnectionString("HangfireConnectionString");
                return _HangfireConnectionString;
            }
        }
        static string _FrontendAngularBaseUrl;
        public static string FrontendAngularBaseUrl
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_FrontendAngularBaseUrl))
                    _FrontendAngularBaseUrl = Get<string>("URLs:FrontendAngularBaseUrl", "http://localhost:4200");
                return _FrontendAngularBaseUrl;
            }
        }
        static string _RedirectURIFrontendAngular;
        public static string RedirectURIFrontendAngular
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_RedirectURIFrontendAngular))
                    _RedirectURIFrontendAngular = Get<string>("URLs:RedirectURIFrontendAngular", "http://localhost:4200");
                return _RedirectURIFrontendAngular;
            }
        }
        static string _luftbornAuditConnectionString;
        public static string luftbornAuditConnectionString
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_luftbornAuditConnectionString))
                    _luftbornAuditConnectionString = GetConnectionString("luftbornAuditConnectionString");
                return _luftbornAuditConnectionString;
            }
        }
        static string _luftbornConnectionString;
        public static string luftbornConnectionString
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_luftbornConnectionString))
                    _luftbornConnectionString = GetConnectionString("luftbornConnectionString");
                return _luftbornConnectionString;
            }
        }
        static string _luftbornDataProtectionKeysConnectionString;
        public static string luftbornDataProtectionKeysConnectionString
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_luftbornDataProtectionKeysConnectionString))
                    _luftbornDataProtectionKeysConnectionString = GetConnectionString("luftbornDataProtectionKeysConnectionString");
                return _luftbornDataProtectionKeysConnectionString;
            }
        }
    }
}