using DI.Shared.Entities.Smooch;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DI.Shared.DataAccess
{
    public class SmoochManager
    {
        private static object _synchObj = new object();

#if __IOS__
        private static HockeyApp.iOS.BITHockeyManager HockeyManager = HockeyApp.iOS.BITHockeyManager.SharedHockeyManager;
#endif

        #region Initialization

        public async static Task<InitResponceModel> Init(InitRequestModel model)
        {
            try
            {
                string jsonObj = JsonConvert.SerializeObject(model);
                byte[] body = Encoding.UTF8.GetBytes(jsonObj);
                string url = Constants.SMOOCH_API_ADDRESS;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url + "init"));
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers.Set("app-token", Constants.SMOOCH_APP_TOKEN);
                request.ContentLength = body.Length;

                using (Stream requestStream = request.GetRequestStream())

                {
                    requestStream.Write(body, 0, body.Length);
                    requestStream.Close();

                }

                var httpResponce = await request.GetResponseAsync().WithTimeout(10000);
                string jsonResult;
                using (Stream responceStream = httpResponce.GetResponseStream())
                {
                    jsonResult = new StreamReader(responceStream).ReadToEnd();
                }

                var result = JsonConvert.DeserializeObject<InitResponceModel>(jsonResult);
                return result;

            }
            catch (Exception ex)
            {
                if (ex != null)
                {
#if __ANDROID__
                    HockeyApp.Android.Metrics.MetricsManager.TrackEvent("Smooch API Init ERROR: " + ex.ToString());
#endif
#if __IOS__
                    HockeyManager.MetricsManager.TrackEvent("Smooch API Init ERROR: " + ex.ToString());
#endif
                }

                return null;
            }
        }

        public async static Task<bool> ConnectWithFirebase(IntegrationRequest model)
        {
            try
            {
                string jsonObj = JsonConvert.SerializeObject(model);
                byte[] body = Encoding.UTF8.GetBytes(jsonObj);
                string url = Constants.SMOOCH_API_ADDRESS;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url + "apps/" + Constants.SMOOCH_APP_ID + "/integrations"));
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers.Set("app-token", Constants.SMOOCH_APP_TOKEN);
                request.ContentLength = body.Length;

                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(body, 0, body.Length);
                    requestStream.Close();
                }

                var httpResponce = await request.GetResponseAsync().WithTimeout(10000);
                string jsonResult;
                using (Stream responceStream = httpResponce.GetResponseStream())
                {
                    jsonResult = new StreamReader(responceStream).ReadToEnd();
                }

                return true;

            }
            catch (WebException ex)
            {
                if (ex != null)
                {
#if __ANDROID__
                    HockeyApp.Android.Metrics.MetricsManager.TrackEvent("Smooch API ConnectWithFirebase ERROR: " + ex.ToString());
#endif
#if __IOS__
                    HockeyManager.MetricsManager.TrackEvent("Smooch API ConnectWithFirebase ERROR: " + ex.ToString());
#endif
                }
                return false;
            }
            catch (Exception ex)
            {
                if (ex != null)
                {
#if __ANDROID__
                    HockeyApp.Android.Metrics.MetricsManager.TrackEvent("Smooch API ConnectWithFirebase ERROR: " + ex.ToString());
#endif
#if __IOS__
                    HockeyManager.MetricsManager.TrackEvent("Smooch API ConnectWithFirebase ERROR: " + ex.ToString());
#endif
                }
                return false;
            }
        }

        #endregion

        #region App User

        public async static Task<AppUser> UpdateAppUser(string userId, AppUserData model)
        {
            try
            {
                string jsonObj = JsonConvert.SerializeObject(model);
                byte[] body = Encoding.UTF8.GetBytes(jsonObj);
                string url = Constants.SMOOCH_API_ADDRESS;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url + "appusers/" + userId));
                request.Method = "PUT";
                request.ContentType = "application/json";
                request.Headers.Set("app-token", Constants.SMOOCH_APP_TOKEN);
                request.ContentLength = body.Length;

                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(body, 0, body.Length);
                    requestStream.Close();
                }

                var httpResponce = await request.GetResponseAsync().WithTimeout(10000);
                string jsonResult;
                using (Stream responceStream = httpResponce.GetResponseStream())
                {
                    jsonResult = new StreamReader(responceStream).ReadToEnd();
                }

                var result = JsonConvert.DeserializeObject<AppUser>(jsonResult);
                return result;

            }
            catch (Exception ex)
            {
                if (ex != null)
                {
#if __ANDROID__
                    HockeyApp.Android.Metrics.MetricsManager.TrackEvent("Smooch API UpdateAppUser ERROR: " + ex.ToString());
#endif
#if __IOS__
                    HockeyManager.MetricsManager.TrackEvent("Smooch API UpdateAppUser ERROR: " + ex.ToString());
#endif
                }

                return null;
            }
        }

        public async static Task<Device> UpdateDevice(string userId, string deviceId, Info model)
        {
            try
            {
                string jsonObj = JsonConvert.SerializeObject(model);
                byte[] body = Encoding.UTF8.GetBytes(jsonObj);
                string url = Constants.SMOOCH_API_ADDRESS;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url + "appusers/" + userId + "/devices/" + deviceId));
                request.Method = "PUT";
                request.ContentType = "application/json";
                request.Headers.Set("app-token", Constants.SMOOCH_APP_TOKEN);
                request.ContentLength = body.Length;

                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(body, 0, body.Length);
                    requestStream.Close();
                }

                var httpResponce = await request.GetResponseAsync().WithTimeout(10000);
                string jsonResult;
                using (Stream responceStream = httpResponce.GetResponseStream())
                {
                    jsonResult = new StreamReader(responceStream).ReadToEnd();
                }

                var result = JsonConvert.DeserializeObject<Device>(jsonResult);
                return result;

            }
            catch (Exception ex)
            {
                if (ex != null)
                {
#if __ANDROID__
                    HockeyApp.Android.Metrics.MetricsManager.TrackEvent("Smooch API UpdateDevice ERROR: " + ex.ToString());
#endif
#if __IOS__
                    HockeyManager.MetricsManager.TrackEvent("Smooch API UpdateDevice ERROR: " + ex.ToString());
#endif
                }

                return null;
            }
        }

        #endregion

        #region Conversations

        public static async Task<MessagesResponse> GetMessages(string userId, string timespan)
        {
            try
            {
                string url = Constants.SMOOCH_API_ADDRESS;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url + "appusers/" + userId + "/messages?before=" + timespan));
                request.Method = "GET";
                request.ContentType = "application/json";
                request.Headers.Set("app-token", Constants.SMOOCH_APP_TOKEN);

                var response = await request.GetResponseAsync();
                string json = string.Empty;
                using (Stream responseStream = response.GetResponseStream())
                {
                    json = new StreamReader(responseStream).ReadToEnd();
                }
                JsonSerializerSettings settings = new JsonSerializerSettings()
                {
                    TypeNameHandling = Newtonsoft.Json.TypeNameHandling.All
                };
                response.Close();

                return JsonConvert.DeserializeObject<MessagesResponse>(json, settings);
            }
            catch (WebException ex)
            {
                if (ex != null)
                {
#if __ANDROID__
                    HockeyApp.Android.Metrics.MetricsManager.TrackEvent("Smooch API GetMessages ERROR: " + ex.ToString());
#endif
#if __IOS__
                    HockeyManager.MetricsManager.TrackEvent("Smooch API GetMessages ERROR: " + ex.ToString());
#endif
                }

                return null;
            }
            catch (Exception ex)
            {
#if __ANDROID__
                HockeyApp.Android.Metrics.MetricsManager.TrackEvent("Smooch API GetMessages ERROR: " + ex.ToString());
#endif
#if __IOS__
                    HockeyManager.MetricsManager.TrackEvent("Smooch API GetMessages ERROR: " + ex.ToString());
#endif

                return null;
            }
        }

        public async static Task<MessageResponse> SendMessage(string userId, MessageModel model)
        {
            try
            {
                string jsonObj = JsonConvert.SerializeObject(model);
                byte[] body = Encoding.UTF8.GetBytes(jsonObj);
                string url = Constants.SMOOCH_API_ADDRESS;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url + "appusers/" + userId + "/messages"));
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers.Set("app-token", Constants.SMOOCH_APP_TOKEN);
                request.ContentLength = body.Length;

                using (Stream requestStream = request.GetRequestStream())

                {
                    requestStream.Write(body, 0, body.Length);
                    requestStream.Close();

                }

                var httpResponce = await request.GetResponseAsync().WithTimeout(10000);
                string jsonResult;
                using (Stream responceStream = httpResponce.GetResponseStream())
                {
                    jsonResult = new StreamReader(responceStream).ReadToEnd();
                }

                var result = JsonConvert.DeserializeObject<MessageResponse>(jsonResult);
                return result;

            }
            catch (Exception ex)
            {
                if (ex != null)
                {
#if __ANDROID__
                    HockeyApp.Android.Metrics.MetricsManager.TrackEvent("Smooch API SendMessage ERROR: " + ex.ToString());
#endif
#if __IOS__
                    HockeyManager.MetricsManager.TrackEvent("Smooch API SendMessage ERROR: " + ex.ToString());
#endif
                }

                return null;
            }
        }

        #endregion

        #region Attachments

        public async static Task<MessageResponse> UploadFile(string userId, MessageModel model)
        {
            try
            {
                string jsonObj = JsonConvert.SerializeObject(model);
                byte[] body = Encoding.UTF8.GetBytes(jsonObj);
                string url = Constants.SMOOCH_API_ADDRESS;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url + "appusers/" + userId + "/images"));
                request.Method = "POST";
                request.ContentType = "multipart/form-data";
                request.Headers.Set("app-token", Constants.SMOOCH_APP_TOKEN);
                request.Headers.Set("role", "appUser");
                request.Headers.Set("name", "TestName");
                request.ContentLength = body.Length;

                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(body, 0, body.Length);
                    requestStream.Close();
                }

                var httpResponce = await request.GetResponseAsync().WithTimeout(10000);
                string jsonResult;
                using (Stream responceStream = httpResponce.GetResponseStream())
                {
                    jsonResult = new StreamReader(responceStream).ReadToEnd();
                }

                var result = JsonConvert.DeserializeObject<MessageResponse>(jsonResult);
                return result;

            }
            catch (Exception ex)
            {
                if (ex != null)
                {
#if __ANDROID__
                    HockeyApp.Android.Metrics.MetricsManager.TrackEvent("Smooch API UploadFile ERROR: " + ex.ToString());
#endif
#if __IOS__
                    HockeyManager.MetricsManager.TrackEvent("Smooch API UploadFile ERROR: " + ex.ToString());
#endif
                }

                return null;
            }
        }

        #endregion

        #region Integrations

        public static async Task<JWTModel> GetJWT()
        {
            try
            {
                string url = Constants.SMOOCH_API_ADDRESS;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url + "apps/" + Constants.SMOOCH_APP_ID + "/keys/" + Constants.SMOOCH_APP_KEY_ID + "/jwt"));
                request.Method = "GET";
                request.ContentType = "application/json";
                request.Headers.Set(HttpRequestHeader.Authorization, "Bearer 89185006088Ss");

                var response = await request.GetResponseAsync();
                string json;
                using (Stream responseStream = response.GetResponseStream())
                {
                    json = new StreamReader(responseStream).ReadToEnd();
                }
                JsonSerializerSettings settings = new JsonSerializerSettings()
                {
                    TypeNameHandling = Newtonsoft.Json.TypeNameHandling.All
                };
                response.Close();

                return JsonConvert.DeserializeObject<JWTModel>(json, settings);
            }
            catch (WebException ex)
            {
                if (ex != null)
                {
#if __ANDROID__
                    HockeyApp.Android.Metrics.MetricsManager.TrackEvent("Smooch API GetJWT ERROR: " + ex.ToString());
#endif
#if __IOS__
                    HockeyManager.MetricsManager.TrackEvent("Smooch API GetJWT ERROR: " + ex.ToString());
#endif
                }

                return null;
            }
            catch (Exception ex)
            {
                if (ex != null)
                {
#if __ANDROID__
                    HockeyApp.Android.Metrics.MetricsManager.TrackEvent("Smooch API GetJWT ERROR: " + ex.ToString());
#endif
#if __IOS__
                    HockeyManager.MetricsManager.TrackEvent("Smooch API GetJWT ERROR: " + ex.ToString());
#endif
                }

                return null;
            }
        }

        public static async Task<MessagesResponse> GetKeys()
        {
            try
            {
                string url = Constants.SMOOCH_API_ADDRESS;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url + "apps/" + Constants.SMOOCH_APP_ID + "/keys"));
                request.Method = "GET";
                request.ContentType = "application/json";
                request.Headers.Set(HttpRequestHeader.Authorization, "Bearer " + Constants.SMOOCH_ACCOUNT_TOKEN);

                var response = await request.GetResponseAsync();
                string json;
                using (Stream responseStream = response.GetResponseStream())
                {
                    json = new StreamReader(responseStream).ReadToEnd();
                }
                JsonSerializerSettings settings = new JsonSerializerSettings()
                {
                    TypeNameHandling = Newtonsoft.Json.TypeNameHandling.All
                };
                response.Close();

                return JsonConvert.DeserializeObject<MessagesResponse>(json, settings);
            }
            catch (WebException ex)
            {
                if (ex != null)
                {
#if __ANDROID__
                    HockeyApp.Android.Metrics.MetricsManager.TrackEvent("Smooch API GetKeys ERROR: " + ex.ToString());
#endif
#if __IOS__
                    HockeyManager.MetricsManager.TrackEvent("Smooch API GetKeys ERROR: " + ex.ToString());
#endif
                }

                return null;
            }
            catch (Exception ex)
            {
                if (ex != null)
                {
#if __ANDROID__
                    HockeyApp.Android.Metrics.MetricsManager.TrackEvent("Smooch API GetKeys ERROR: " + ex.ToString());
#endif
#if __IOS__
                    HockeyManager.MetricsManager.TrackEvent("Smooch API GetKeys ERROR: " + ex.ToString());
#endif
                }

                return null;
            }
        }

        public static async Task<MessagesResponse> GetIntegrations()
        {
            try
            {
                string url = Constants.SMOOCH_API_ADDRESS;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url + "apps/" + Constants.SMOOCH_APP_ID + "/integrations"));
                request.Method = "GET";
                request.ContentType = "application/json";
                request.Headers.Set(HttpRequestHeader.Authorization, "Bearer " + Constants.SMOOCH_ACCOUNT_TOKEN); 

                var response = await request.GetResponseAsync();
                string json;
                using (Stream responseStream = response.GetResponseStream())
                {
                    json = new StreamReader(responseStream).ReadToEnd();
                }
                JsonSerializerSettings settings = new JsonSerializerSettings()
                {
                    TypeNameHandling = Newtonsoft.Json.TypeNameHandling.All
                };
                response.Close();

                return JsonConvert.DeserializeObject<MessagesResponse>(json, settings);
            }
            catch (WebException ex)
            {
                if (ex != null)
                {
#if __ANDROID__
                    HockeyApp.Android.Metrics.MetricsManager.TrackEvent("Smooch API GetIntegrations ERROR: " + ex.ToString());
#endif
#if __IOS__
                    HockeyManager.MetricsManager.TrackEvent("Smooch API GetIntegrations ERROR: " + ex.ToString());
#endif
                }

                return null;
            }
            catch (Exception ex)
            {
                if (ex != null)
                {
#if __ANDROID__
                    HockeyApp.Android.Metrics.MetricsManager.TrackEvent("Smooch API GetIntegrations ERROR: " + ex.ToString());
#endif
#if __IOS__
                    HockeyManager.MetricsManager.TrackEvent("Smooch API GetIntegrations ERROR: " + ex.ToString());
#endif
                }

                return null;
            }
        }

        #endregion

    }

    public static class RequestHelper
    {
#if __IOS__
        private static HockeyApp.iOS.BITHockeyManager HockeyManager = HockeyApp.iOS.BITHockeyManager.SharedHockeyManager;
#endif
        public async static Task<T> WithTimeout<T>(this Task<T> task, int duration)
        {
            try
            {
                var retTask = await Task.WhenAny(task, Task.Delay(duration))
                                    .ConfigureAwait(false);

                if (retTask is Task<T>) return task.Result;
                return default(T);
            }
            catch (Exception ex)
            {
#if __ANDROID__
                HockeyApp.Android.Metrics.MetricsManager.TrackEvent("Smooch API WithTimeout ERROR: " + ex.ToString());
#endif
#if __IOS__
                    HockeyManager.MetricsManager.TrackEvent("Smooch API WithTimeout ERROR: " + ex.ToString());
#endif

                return default(T);
            }
        }
    }
}
