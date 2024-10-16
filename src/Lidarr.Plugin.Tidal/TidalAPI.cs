using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using NLog;
using TidalSharp;
using TidalSharp.Data;

namespace NzbDrone.Plugin.Tidal
{
    public class TidalAPI
    {
        public static TidalAPI Instance { get; private set; }

        public static void Initialize(AudioQuality quality, string configDir, Logger logger)
        {
            if (Instance != null)
                return;
            Instance = new TidalAPI(quality, configDir);
            logger.Info("Tidal URL; use this to login: " + Instance.Client.GetPkceLoginUrl());
        }

        private TidalAPI(AudioQuality quality, string configDir)
        {
            Instance = this;
            _client = new(quality, VideoQuality.HIGH, configDir);
        }

        public TidalClient Client => _client;

        private TidalClient _client;

        public string GetAPIUrl(string method, Dictionary<string, string> parameters = null)
        {
            parameters ??= new();
            parameters["sessionId"] = _client.ActiveUser?.SessionID ?? "";
            parameters["countryCode"] = _client.ActiveUser?.CountryCode ?? "";
            if (!parameters.ContainsKey("limit"))
                parameters["limit"] = "1000";

            StringBuilder stringBuilder = new("https://api.tidal.com/v1/");
            stringBuilder.Append(method);
            for (var i = 0; i < parameters.Count; i++)
            {
                var start = i == 0 ? "?" : "&";
                var key = WebUtility.UrlEncode(parameters.ElementAt(i).Key);
                var value = WebUtility.UrlEncode(parameters.ElementAt(i).Value);
                stringBuilder.Append(start + key + "=" + value);
            }
            return stringBuilder.ToString();
        }
    }
}
