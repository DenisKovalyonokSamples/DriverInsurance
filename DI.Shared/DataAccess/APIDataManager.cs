using DI.Shared.Entities.API;
using DI.Shared.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace DI.Shared.DataAccess
{
    public delegate void ConnectionStatusHandler(bool status);
    public delegate void AuthenticationStatusHandler(bool status);

    public class APIDataManager
    {
        private static object _synchObj = new object();
        private static Timer _t;

        public static event ConnectionStatusHandler ConnectionStatusChanged;
        public static event AuthenticationStatusHandler AuthenticationStatusChanged;

        #region Connection

        private static TokenResponseModel tokenResponseModel;
        public static TokenResponseModel TokenResponseModel
        {
            get
            {
                lock (_synchObj)
                    return tokenResponseModel;
            }
            protected set
            {
                bool changed = false;

                lock (_synchObj)
                {
                    changed = tokenResponseModel != value;
                    tokenResponseModel = value;
                    AuthenticationStatusChanged?.Invoke(tokenResponseModel != null);
                }
            }
        }

        public static void SetUserToken(string token)
        {
            TokenResponseModel = new TokenResponseModel();
            TokenResponseModel.Token = token;
        }

        public static bool IsAuthenticated
        {
            get
            {
                lock (_synchObj)
                    return (tokenResponseModel != null);
            }
        }

        private static bool _isConnected;
        public static bool IsConnected
        {
            get
            {
                lock (_synchObj)
                    return _isConnected;
            }
            protected set
            {
                bool needReconnect = false;
                bool changed = false;
                lock (_synchObj)
                {
                    changed = _isConnected != value;
                    needReconnect = _isConnected != value && !value;
                    _isConnected = value;
                }

                if (needReconnect)
                {
                    Connect(null);
                }

                if (changed)
                    ConnectionStatusChanged?.Invoke(value);
            }
        }

        public static void Connect(object data)
        {
            Ping();

            if (IsConnected)
                return;

            if (_t == null)
                _t = new Timer(Connect, null, 20000, Timeout.Infinite);
            else
                _t.Change(20000, Timeout.Infinite);
        }

        public static bool Ping()
        {
            HttpWebRequest request = new HttpWebRequest(new Uri(String.Format("{0}api/Account/Ping", Constants.WebAPIBaseAddress)));
            request.Method = "GET";
            request.Timeout = 10000;
            try
            {
                request.GetResponse();
                IsConnected = true;
                return true;
            }
            catch (WebException ex)
            {
                IsConnected = IsConnectionAlive(ex.Status);
                return false;
            }
            catch (Exception ex)
            {
                IsConnected = true;
                return false;
            }
        }

        private static bool IsConnectionAlive(WebExceptionStatus stastus)
        {
            switch (stastus)
            {
                case WebExceptionStatus.Timeout:
                case WebExceptionStatus.ConnectFailure:
                case WebExceptionStatus.ConnectionClosed:
                case WebExceptionStatus.ReceiveFailure:
                case WebExceptionStatus.SendFailure:
                    return false;
                default:
                    return true;
            }
        }
        #endregion

        #region Accessors

        #region Account 

        public static async Task<TokenResponseModel> Login(string username, string password)
        {
            try
            {
                HttpWebRequest request = new HttpWebRequest(new Uri(String.Format("{0}signin", Constants.WebAPIBaseAddress)));
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                string postString = String.Format("username={0}&password={1}", username, password);

                byte[] bytes = Encoding.UTF8.GetBytes(postString);
                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(bytes, 0, bytes.Length);
                }


                var httpResponse = await request.GetResponseAsync();
                string jsonResult;
                using (Stream responseStream = httpResponse.GetResponseStream())
                {
                    jsonResult = new StreamReader(responseStream).ReadToEnd();
                }

                IsConnected = true;

                TokenResponseModel = JsonConvert.DeserializeObject<TokenResponseModel>(jsonResult);
                if (TokenResponseModel != null)
                {
                    return TokenResponseModel;
                }

            }
            catch (WebException ex)
            {
                try
                {
                    var answer = CreateException((HttpWebResponse)((WebException)ex).Response);


                }
                catch(Exception e)
                {
                    //Log! The remote server returned an error: (503) Service Temporarily Unavailable.
                }
                IsConnected = IsConnectionAlive(ex.Status);
                return null;
            }
            catch (Exception ex)
            {
                IsConnected = true;
                return null;
            }
            IsConnected = true;
            return null;
        }

        public static async Task<ResponseResultWithCode> SendSMSCode(string phone)
        {
            try
            {
                HttpWebRequest request = new HttpWebRequest(new Uri(String.Format("{0}verify-user", Constants.WebAPIBaseAddress)));
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                string postString = String.Format("phone={0}", phone.Replace("+", "%2B"));

                byte[] bytes = Encoding.UTF8.GetBytes(postString);
                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(bytes, 0, bytes.Length);
                }


                var httpResponse = await request.GetResponseAsync();
                string jsonResult;
                using (Stream responseStream = httpResponse.GetResponseStream())
                {
                    jsonResult = new StreamReader(responseStream).ReadToEnd();
                }

                IsConnected = true;

                var value = JsonConvert.DeserializeObject<ResponseResultWithCode>(jsonResult);
                if (value != null)
                {
                    return value;
                }

                return null;
            }
            catch (WebException e)
            {
                IsConnected = IsConnectionAlive(e.Status);
                return null;
            }
            catch (Exception ex)
            {
                IsConnected = true;
                return null;
            }
        }

        public static async Task<ResponseResult> VerifySMSCode(string phone, string code)
        {
            try
            {
                HttpWebRequest request = new HttpWebRequest(new Uri(String.Format("{0}verify-user", Constants.WebAPIBaseAddress)));
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                string postString = String.Format("phone={0}&code={1}", phone.Replace("+", "%2B"), code);

                byte[] bytes = Encoding.UTF8.GetBytes(postString);
                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(bytes, 0, bytes.Length);
                }


                var httpResponse = await request.GetResponseAsync();
                string jsonResult;
                using (Stream responseStream = httpResponse.GetResponseStream())
                {
                    jsonResult = new StreamReader(responseStream).ReadToEnd();
                }

                IsConnected = true;

                var value = JsonConvert.DeserializeObject<ResponseResult>(jsonResult);
                if (value != null)
                {
                    return value;
                }

                return null;
            }
            catch (WebException ex)
            {
                IsConnected = IsConnectionAlive(ex.Status);
                return null;
            }
            catch (Exception ex)
            {
                IsConnected = true;
                return null;
            }
        }

        public static async Task<ResponseResult> SendSMSForPasswordReset(string phone)
        {
            try
            {
                HttpWebRequest request = new HttpWebRequest(new Uri(String.Format("{0}reset", Constants.WebAPIBaseAddress)));
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                string postString = String.Format("phone={0}", phone.Replace("+", "%2B"));

                byte[] bytes = Encoding.UTF8.GetBytes(postString);
                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(bytes, 0, bytes.Length);
                }


                var httpResponse = await request.GetResponseAsync();
                string jsonResult;
                using (Stream responseStream = httpResponse.GetResponseStream())
                {
                    jsonResult = new StreamReader(responseStream).ReadToEnd();
                }

                IsConnected = true;

                var value = JsonConvert.DeserializeObject<ResponseResult>(jsonResult);
                if (value != null)
                {
                    return value;
                }

                return null;
            }
            catch (WebException ex)
            {
                var answer = CreateException((HttpWebResponse)((WebException)ex).Response);
                IsConnected = IsConnectionAlive(ex.Status);
                return null;
            }
            catch (Exception ex)
            {
                IsConnected = true;
                return null;
            }
        }

        public static async Task<ResponseResult> ResetPassword(string phone, string code, string password)
        {
            try
            {
                HttpWebRequest request = new HttpWebRequest(new Uri(String.Format("{0}reset", Constants.WebAPIBaseAddress)));
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                string postString = String.Format("phone={0}&code={1}&password={2}", phone.Replace("+", "%2B"), code, password);

                byte[] bytes = Encoding.UTF8.GetBytes(postString);
                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(bytes, 0, bytes.Length);
                }


                var httpResponse = await request.GetResponseAsync();
                string jsonResult;
                using (Stream responseStream = httpResponse.GetResponseStream())
                {
                    jsonResult = new StreamReader(responseStream).ReadToEnd();
                }

                IsConnected = true;

                var value = JsonConvert.DeserializeObject<ResponseResult>(jsonResult);
                if (value != null)
                {
                    return value;
                }

                return null;
            }
            catch (WebException ex)
            {
                IsConnected = IsConnectionAlive(ex.Status);
                return null;
            }
            catch (Exception ex)
            {
                IsConnected = true;
                return null;
            }
        }

        #endregion

        #region Companies 

        public static async Task<Companies> GetCompanies()
        {
            if (TokenResponseModel == null)
                return null;

            try
            {
                HttpWebRequest request = new HttpWebRequest(new Uri(String.Format("{0}companies", Constants.WebAPIBaseAddress)));
                request.Headers.Set(HttpRequestHeader.Authorization, "Token " + TokenResponseModel.Token);
                request.Method = "GET";
                request.ContentType = "application/json";

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

                return JsonConvert.DeserializeObject<Companies>(json, settings);
            }
            catch (WebException ex)
            {
                IsConnected = IsConnectionAlive(ex.Status);
                return null;
            }
            catch (Exception ex)
            {
                IsConnected = true;
                return null;
            }
        }

        public static async Task<Company> GetCompany(int id)
        {
            if (TokenResponseModel == null)
                return null;

            try
            {
                HttpWebRequest request = new HttpWebRequest(new Uri(String.Format("{0}companies/{1}", Constants.WebAPIBaseAddress, id)));
                request.Headers.Set(HttpRequestHeader.Authorization, "Token " + TokenResponseModel.Token);
                request.Method = "GET";
                request.ContentType = "application/json";

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

                var entity = JsonConvert.DeserializeObject<CompanyContainer>(json, settings);
                if (entity != null && entity.Data != null)
                {
                    return entity.Data;
                }

                return null;
            }
            catch (WebException ex)
            {
                IsConnected = IsConnectionAlive(ex.Status);
                return null;
            }
            catch (Exception ex)
            {
                IsConnected = true;
                return null;
            }
        }

        public static async Task<Company> CreateCompany(Company entity)
        {
            if (TokenResponseModel == null)
                return null;

            try
            {
                var settings = new JsonSerializerSettings() { ContractResolver = new EmptyStringToNullResolver() };
                settings.Converters.Add(new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd" });

                string jsonObj = JsonConvert.SerializeObject(entity, settings);
                byte[] body = Encoding.UTF8.GetBytes(jsonObj);

                HttpWebRequest request = new HttpWebRequest(new Uri(String.Format("{0}companies", Constants.WebAPIBaseAddress)));
                request.Headers.Set(HttpRequestHeader.Authorization, "Token " + TokenResponseModel.Token);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.ContentLength = body.Length;

                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(body, 0, body.Length);
                    stream.Close();
                }

                var httpResponse = await request.GetResponseAsync();

                string jsonResult;
                using (Stream responseStream = httpResponse.GetResponseStream())
                {
                    jsonResult = new StreamReader(responseStream).ReadToEnd();
                }

                IsConnected = true;

                var result = JsonConvert.DeserializeObject<CompanyContainer>(jsonResult);

                IsConnected = true;
                return result.Data;

            }
            catch (WebException ex)
            {
                IsConnected = IsConnectionAlive(ex.Status);
                return null;
            }
            catch (Exception ex)
            {
                IsConnected = true;
                return null;
            }
        }

        public static async Task<Company> UpdateCompany(Company entity)
        {
            if (TokenResponseModel == null)
                return null;

            try
            {
                var settings = new JsonSerializerSettings() { ContractResolver = new EmptyStringToNullResolver() };
                settings.Converters.Add(new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd" });

                string jsonObj = JsonConvert.SerializeObject(entity, settings);
                byte[] body = Encoding.UTF8.GetBytes(jsonObj);

                HttpWebRequest request = new HttpWebRequest(new Uri(String.Format("{0}companies", Constants.WebAPIBaseAddress)));
                request.Headers.Set(HttpRequestHeader.Authorization, "Token " + TokenResponseModel.Token);
                request.Method = "PUT";
                request.ContentType = "application/json";
                request.ContentLength = body.Length;

                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(body, 0, body.Length);
                    stream.Close();
                }

                var httpResponse = await request.GetResponseAsync();

                string jsonResult;
                using (Stream responseStream = httpResponse.GetResponseStream())
                {
                    jsonResult = new StreamReader(responseStream).ReadToEnd();
                }

                var result = JsonConvert.DeserializeObject<CompanyContainer>(jsonResult);

                IsConnected = true;
                return result.Data;

            }
            catch (WebException ex)
            {
                var answer = CreateException((HttpWebResponse)((WebException)ex).Response);
                IsConnected = IsConnectionAlive(ex.Status);
                return null;
            }
            catch (Exception ex)
            {
                IsConnected = true;
                return null;
            }
        }

        #endregion

        #region Bonus Transaction

        public static async Task<List<BonusTransaction>> GetBonusTransactions(string contractId)
        {
            if (TokenResponseModel == null)
                return null;

            try
            {
                HttpWebRequest request = new HttpWebRequest(new Uri(String.Format("{0}bonus-transactions/?contract=" + contractId, Constants.WebAPIBaseAddress)));
                request.Headers.Set(HttpRequestHeader.Authorization, "Token " + TokenResponseModel.Token);
                request.Method = "GET";
                request.ContentType = "application/json";

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

                var value = JsonConvert.DeserializeObject<BonusTransactionsContainer>(json, settings);
                if (value != null && value.Data != null && value.Data.Count > 0)
                {
                    return value.Data;
                }

                return null;
            }
            catch (WebException ex)
            {
                var answer = CreateException((HttpWebResponse)((WebException)ex).Response);
                IsConnected = IsConnectionAlive(ex.Status);
                return null;
            }
            catch (Exception ex)
            {
                IsConnected = true;
                return null;
            }
        }

        public static async Task<BonusTransaction> CreateBonusTransaction(BonusTransaction entity)
        {
            if (TokenResponseModel == null)
                return null;

            try
            {
                var settings = new JsonSerializerSettings() { ContractResolver = new EmptyStringToNullResolver() };
                settings.Converters.Add(new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd" });

                string jsonObj = JsonConvert.SerializeObject(entity, settings);
                byte[] body = Encoding.UTF8.GetBytes(jsonObj);

                HttpWebRequest request = new HttpWebRequest(new Uri(String.Format("{0}bonus-transactions", Constants.WebAPIBaseAddress)));
                request.Headers.Set(HttpRequestHeader.Authorization, "Token " + TokenResponseModel.Token);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.ContentLength = body.Length;

                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(body, 0, body.Length);
                    stream.Close();
                }

                var httpResponse = await request.GetResponseAsync();

                string jsonResult;
                using (Stream responseStream = httpResponse.GetResponseStream())
                {
                    jsonResult = new StreamReader(responseStream).ReadToEnd();
                }

                IsConnected = true;

                var result = JsonConvert.DeserializeObject<BonusTransactionContainer>(jsonResult);

                IsConnected = true;
                return result.Data;

            }
            catch (WebException ex)
            {
                var answer = CreateException((HttpWebResponse)((WebException)ex).Response);
                IsConnected = IsConnectionAlive(ex.Status);
                return null;
            }
            catch (Exception ex)
            {
                IsConnected = true;
                return null;
            }
        }

        #endregion

        #region Quotation

        public static async Task<Quotation> CreateQuotation(Quotation entity)
        {
            if (TokenResponseModel == null)
                return null;

            try
            {
                string jsonObj = JsonConvert.SerializeObject(entity);

                byte[] body = Encoding.UTF8.GetBytes(jsonObj);

                HttpWebRequest request = new HttpWebRequest(new Uri(String.Format("{0}quotations", Constants.WebAPIBaseAddress)));
                request.Headers.Set(HttpRequestHeader.Authorization, "Token " + TokenResponseModel.Token);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.ContentLength = body.Length;

                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(body, 0, body.Length);
                    stream.Close();
                }

                var httpResponse = await request.GetResponseAsync();

                string jsonResult;
                using (Stream responseStream = httpResponse.GetResponseStream())
                {
                    jsonResult = new StreamReader(responseStream).ReadToEnd();
                }

                IsConnected = true;

                var result = JsonConvert.DeserializeObject<QuotationContainer>(jsonResult);

                IsConnected = true;
                return result.Data;

            }
            catch (WebException ex)
            {
                var answer = CreateException((HttpWebResponse)((WebException)ex).Response);
                IsConnected = IsConnectionAlive(ex.Status);
                return null;
            }
            catch (Exception ex)
            {
                IsConnected = true;
                return null;
            }
        }

        public static async Task<Quotation> GetQuotation(string companyId)
        {
            if (TokenResponseModel == null)
                return null;

            try
            {
                HttpWebRequest request = new HttpWebRequest(new Uri(String.Format("{0}quotations/?client=" + companyId, Constants.WebAPIBaseAddress)));
                request.Headers.Set(HttpRequestHeader.Authorization, "Token " + TokenResponseModel.Token);
                request.Method = "GET";
                request.ContentType = "application/json";

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

                var entity = JsonConvert.DeserializeObject<QuotationsContainer>(json, settings);
                if (entity != null && entity.Data != null && entity.Data.Count > 0)
                {
                    return entity.Data.Where(e => e.Status != "declined").FirstOrDefault();
                }

                return null;
            }
            catch (WebException ex)
            {
                var answer = CreateException((HttpWebResponse)((WebException)ex).Response);
                IsConnected = IsConnectionAlive(ex.Status);
                return null;
            }
            catch (Exception ex)
            {
                IsConnected = true;
                return null;
            }
        }

        public static async Task<Quotation> GetQuotationByStatus(string companyId, string status)
        {
            if (TokenResponseModel == null)
                return null;

            try
            {
                HttpWebRequest request = new HttpWebRequest(new Uri(String.Format("{0}quotations/?client=" + companyId + "&status=" + status, Constants.WebAPIBaseAddress)));
                request.Headers.Set(HttpRequestHeader.Authorization, "Token " + TokenResponseModel.Token);
                request.Method = "GET";
                request.ContentType = "application/json";

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

                var entity = JsonConvert.DeserializeObject<QuotationsContainer>(json, settings);
                if (entity != null && entity.Data != null && entity.Data.Count > 0)
                {
                    return entity.Data.FirstOrDefault();
                }

                return null;
            }
            catch (WebException ex)
            {
                IsConnected = IsConnectionAlive(ex.Status);
                return null;
            }
            catch (Exception ex)
            {
                IsConnected = true;
                return null;
            }
        }

        #endregion

        #region Car

        public static async Task<Car> CreateCar(Car entity)
        {
            if (TokenResponseModel == null)
                return null;

            try
            {
                string jsonObj = JsonConvert.SerializeObject(entity);

                byte[] body = Encoding.UTF8.GetBytes(jsonObj);

                HttpWebRequest request = new HttpWebRequest(new Uri(String.Format("{0}cars", Constants.WebAPIBaseAddress)));
                request.Headers.Set(HttpRequestHeader.Authorization, "Token " + TokenResponseModel.Token);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.ContentLength = body.Length;

                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(body, 0, body.Length);
                    stream.Close();
                }

                var httpResponse = await request.GetResponseAsync();

                string jsonResult;
                using (Stream responseStream = httpResponse.GetResponseStream())
                {
                    jsonResult = new StreamReader(responseStream).ReadToEnd();
                }

                IsConnected = true;

                var result = JsonConvert.DeserializeObject<CarContainer>(jsonResult);

                IsConnected = true;
                return result.Data;

            }
            catch (WebException ex)
            {
                IsConnected = IsConnectionAlive(ex.Status);
                return null;
            }
            catch (Exception ex)
            {
                IsConnected = true;
                return null;
            }
        }

        public static async Task<Car> UpdateCar(Car entity)
        {
            if (TokenResponseModel == null)
                return null;

            try
            {
                var settings = new JsonSerializerSettings() { ContractResolver = new EmptyStringToNullResolver() };
                settings.Converters.Add(new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd" });

                string jsonObj = JsonConvert.SerializeObject(entity, settings);
                byte[] body = Encoding.UTF8.GetBytes(jsonObj);

                HttpWebRequest request = new HttpWebRequest(new Uri(String.Format("{0}cars", Constants.WebAPIBaseAddress)));
                request.Headers.Set(HttpRequestHeader.Authorization, "Token " + TokenResponseModel.Token);
                request.Method = "PUT";
                request.ContentType = "application/json";
                request.ContentLength = body.Length;

                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(body, 0, body.Length);
                    stream.Close();
                }

                var httpResponse = await request.GetResponseAsync();

                string jsonResult;
                using (Stream responseStream = httpResponse.GetResponseStream())
                {
                    jsonResult = new StreamReader(responseStream).ReadToEnd();
                }

                var result = JsonConvert.DeserializeObject<CarContainer>(jsonResult);

                IsConnected = true;
                return result.Data;
            }
            catch (WebException ex)
            {
                var answer = CreateException((HttpWebResponse)((WebException)ex).Response);
                IsConnected = IsConnectionAlive(ex.Status);
                return null;
            }
            catch (Exception ex)
            {
                IsConnected = true;
                return null;
            }
        }

        public static async Task<Car> GetCar(string id)
        {
            if (TokenResponseModel == null)
                return null;

            try
            {
                HttpWebRequest request = new HttpWebRequest(new Uri(String.Format("{0}cars/?id=" + id, Constants.WebAPIBaseAddress)));
                request.Headers.Set(HttpRequestHeader.Authorization, "Token " + TokenResponseModel.Token);
                request.Method = "GET";
                request.ContentType = "application/json";

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

                var entity = JsonConvert.DeserializeObject<CarsContainer>(json, settings);
                if (entity != null && entity.Data != null && entity.Data.Count > 0)
                {
                    return entity.Data.FirstOrDefault();
                }

                return null;
            }
            catch (WebException ex)
            {
                IsConnected = IsConnectionAlive(ex.Status);
                return null;
            }
            catch (Exception ex)
            {
                IsConnected = true;
                return null;
            }
        }

        public static async Task<Car> GetCarByCompany(string companyId)
        {
            if (TokenResponseModel == null)
                return null;

            try
            {
                HttpWebRequest request = new HttpWebRequest(new Uri(String.Format("{0}cars/?owner=" + companyId, Constants.WebAPIBaseAddress)));
                request.Headers.Set(HttpRequestHeader.Authorization, "Token " + TokenResponseModel.Token);
                request.Method = "GET";
                request.ContentType = "application/json";

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

                var entity = JsonConvert.DeserializeObject<CarsContainer>(json, settings);
                if (entity != null && entity.Data != null && entity.Data.Count > 0)
                {
                    return entity.Data.FirstOrDefault();
                }

                return null;
            }
            catch (WebException ex)
            {
                IsConnected = IsConnectionAlive(ex.Status);
                return null;
            }
            catch (Exception ex)
            {
                IsConnected = true;
                return null;
            }
        }

        public static async Task<Car> GetCarDetails(string carId)
        {
            if (TokenResponseModel == null)
                return null;

            try
            {
                HttpWebRequest request = new HttpWebRequest(new Uri(String.Format("{0}cars/" + carId, Constants.WebAPIBaseAddress)));
                request.Headers.Set(HttpRequestHeader.Authorization, "Token " + TokenResponseModel.Token);
                request.Method = "GET";
                request.ContentType = "application/json";

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

                var entity = JsonConvert.DeserializeObject<CarContainer>(json, settings);
                if (entity != null && entity.Data != null)
                {
                    return entity.Data;
                }

                return null;
            }
            catch (WebException ex)
            {
                IsConnected = IsConnectionAlive(ex.Status);
                return null;
            }
            catch (Exception ex)
            {
                IsConnected = true;
                return null;
            }
        }

        public static async Task<Car> GetCarByRegNumber(string regNumber)
        {
            if (TokenResponseModel == null)
                return null;

            try
            {
                HttpWebRequest request = new HttpWebRequest(new Uri(String.Format("{0}cars/?regnum=" + regNumber, Constants.WebAPIBaseAddress)));
                request.Headers.Set(HttpRequestHeader.Authorization, "Token " + TokenResponseModel.Token);
                request.Method = "GET";
                request.ContentType = "application/json";

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

                var entity = JsonConvert.DeserializeObject<CarsContainer>(json, settings);
                if (entity != null && entity.Data != null && entity.Data.Count > 0)
                {
                    return entity.Data.FirstOrDefault();
                }

                return null;
            }
            catch (WebException ex)
            {
                IsConnected = IsConnectionAlive(ex.Status);
                return null;
            }
            catch (Exception ex)
            {
                IsConnected = true;
                return null;
            }
        }

        #endregion

        #region Contracts

        public static async Task<Contract> GetContract(string id)
        {
            if (TokenResponseModel == null)
                return null;

            try
            {
                HttpWebRequest request = new HttpWebRequest(new Uri(String.Format("{0}contracts/" + id, Constants.WebAPIBaseAddress)));
                request.Headers.Set(HttpRequestHeader.Authorization, "Token " + TokenResponseModel.Token);
                request.Method = "GET";
                request.ContentType = "application/json";

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

                var value = JsonConvert.DeserializeObject<ContractContainer>(json, settings);
                if (value != null && value.Data != null)
                {
                    return value.Data;
                }

                return null;
            }
            catch (WebException ex)
            {
                IsConnected = IsConnectionAlive(ex.Status);
                return null;
            }
            catch (Exception ex)
            {
                IsConnected = true;
                return null;
            }
        }

        public static async Task<Contract> GetCurrentContract(string companyId)
        {
            if (TokenResponseModel == null)
                return null;

            try
            {
                HttpWebRequest request = new HttpWebRequest(new Uri(String.Format("{0}contracts/?status=accept&client=" + companyId + "&contract_end__gte=" + DateTime.UtcNow.ToString("yyyy-MM-dd"), Constants.WebAPIBaseAddress)));
                request.Headers.Set(HttpRequestHeader.Authorization, "Token " + TokenResponseModel.Token);
                request.Method = "GET";
                request.ContentType = "application/json";

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

                var value = JsonConvert.DeserializeObject<ContractsContainer>(json, settings);
                if (value != null && value.Data != null)
                {
                    return value.Data.FirstOrDefault();
                }

                return null;
            }
            catch (WebException ex)
            {
                IsConnected = IsConnectionAlive(ex.Status);
                return null;
            }
            catch (Exception ex)
            {
                IsConnected = true;
                return null;
            }
        }

        public static async Task<List<Contract>> GetContractOffers(string companyId, string status)
        {
            if (TokenResponseModel == null)
                return null;

            try
            {
                HttpWebRequest request = new HttpWebRequest(new Uri(String.Format("{0}contracts/?status=" + status + "&client=" + companyId, Constants.WebAPIBaseAddress)));
                request.Headers.Set(HttpRequestHeader.Authorization, "Token " + TokenResponseModel.Token);
                request.Method = "GET";
                request.ContentType = "application/json";

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

                var value = JsonConvert.DeserializeObject<ContractsContainer>(json, settings);
                if (value != null && value.Data != null && value.Data.Count > 0)
                {
                    return value.Data;
                }

                return null;
            }
            catch (WebException ex)
            {
                IsConnected = IsConnectionAlive(ex.Status);
                return null;
            }
            catch (Exception ex)
            {
                IsConnected = true;
                return null;
            }
        }

        public static async Task<Contract> GetContractOffer(int id)
        {
            if (TokenResponseModel == null)
                return null;

            try
            {
                HttpWebRequest request = new HttpWebRequest(new Uri(String.Format("{0}contracts/{1}", Constants.WebAPIBaseAddress, id)));
                request.Headers.Set(HttpRequestHeader.Authorization, "Token " + TokenResponseModel.Token);
                request.Method = "GET";
                request.ContentType = "application/json";

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

                var entity = JsonConvert.DeserializeObject<ContractContainer>(json, settings);
                if (entity != null && entity.Data != null)
                {
                    return entity.Data;
                }

                return null;
            }
            catch (WebException ex)
            {
                IsConnected = IsConnectionAlive(ex.Status);
                return null;
            }
            catch (Exception ex)
            {
                IsConnected = true;
                return null;
            }
        }

        public static async Task<Contract> UpdateContract(Contract entity, StatusValue status)
        {
            if (TokenResponseModel == null)
                return null;

            try
            {
                string jsonObj = JsonConvert.SerializeObject(status);
                byte[] body = Encoding.UTF8.GetBytes(jsonObj);

                HttpWebRequest request = new HttpWebRequest(new Uri(String.Format("{0}contracts/" + entity.Id, Constants.WebAPIBaseAddress)));
                request.Headers.Set(HttpRequestHeader.Authorization, "Token " + TokenResponseModel.Token);
                request.Method = "PUT";
                request.ContentType = "application/json";
                request.ContentLength = body.Length;

                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(body, 0, body.Length);
                    stream.Close();
                }

                var httpResponse = await request.GetResponseAsync();

                string jsonResult;
                using (Stream responseStream = httpResponse.GetResponseStream())
                {
                    jsonResult = new StreamReader(responseStream).ReadToEnd();
                }

                var result = JsonConvert.DeserializeObject<ContractContainer>(jsonResult);

                IsConnected = true;
                return result.Data;
            }
            catch (WebException ex)
            {
                IsConnected = IsConnectionAlive(ex.Status);
                return null;
            }
            catch (Exception ex)
            {
                IsConnected = true;
                return null;
            }
        }

        #endregion

        #region Users

        public static async Task<User> GetUserByPhone(string phone)
        {
            if (TokenResponseModel == null)
                return null;

            try
            {
                HttpWebRequest request = new HttpWebRequest(new Uri(String.Format("{0}users/?phone=" + phone, Constants.WebAPIBaseAddress)));
                request.Headers.Set(HttpRequestHeader.Authorization, "Token " + TokenResponseModel.Token);
                request.Method = "GET";
                request.ContentType = "application/json";

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

                var value = JsonConvert.DeserializeObject<Users>(json, settings);
                if (value != null && value.Data != null && value.Data.Count > 0)
                {
                    return value.Data.FirstOrDefault();
                }
                else
                {
                    return null;
                }
            }
            catch (WebException ex)
            {
                IsConnected = IsConnectionAlive(ex.Status);
                return null;
            }
            catch (Exception ex)
            {
                IsConnected = true;
                return null;
            }
        }

        public static async Task<Users> GetUsers()
        {
            if (TokenResponseModel == null)
                return null;

            try
            {
                HttpWebRequest request = new HttpWebRequest(new Uri(String.Format("{0}users", Constants.WebAPIBaseAddress)));
                request.Headers.Set(HttpRequestHeader.Authorization, "Token " + TokenResponseModel.Token);
                request.Method = "GET";
                request.ContentType = "application/json";

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

                return JsonConvert.DeserializeObject<Users>(json, settings);
            }
            catch (WebException ex)
            {
                IsConnected = IsConnectionAlive(ex.Status);
                return null;
            }
            catch (Exception ex)
            {
                IsConnected = true;
                return null;
            }
        }

        public static async Task<User> GetUser(int id)
        {
            if (TokenResponseModel == null)
                return null;

            try
            {
                HttpWebRequest request = new HttpWebRequest(new Uri(String.Format("{0}users/{1}", Constants.WebAPIBaseAddress, id)));
                request.Headers.Set(HttpRequestHeader.Authorization, "Token " + TokenResponseModel.Token);
                request.Method = "GET";
                request.ContentType = "application/json";

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

                var entity = JsonConvert.DeserializeObject<UserContainer>(json, settings);
                if (entity != null && entity.Data != null)
                {
                    return entity.Data;
                }

                return null;
            }
            catch (WebException ex)
            {
                IsConnected = IsConnectionAlive(ex.Status);
                return null;
            }
            catch (Exception ex)
            {
                IsConnected = true;
                return null;
            }
        }

        public static async Task<User> GetUserByName(string name)
        {
            if (TokenResponseModel == null)
                return null;

            try
            {
                HttpWebRequest request = new HttpWebRequest(new Uri(String.Format("{0}users/?username=" + name, Constants.WebAPIBaseAddress)));
                request.Headers.Set(HttpRequestHeader.Authorization, "Token " + TokenResponseModel.Token);
                request.Method = "GET";
                request.ContentType = "application/json";

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

                var entity = JsonConvert.DeserializeObject<UsersContainer>(json, settings);
                if (entity != null && entity.Data != null && entity.Data.Count > 0)
                {
                    return entity.Data.FirstOrDefault();
                }

                return null;
            }
            catch (WebException ex)
            {
                IsConnected = IsConnectionAlive(ex.Status);
                return null;
            }
            catch (Exception ex)
            {
                IsConnected = true;
                return null;
            }
        }

        public static async Task<User> CreateUser(UserForRegistration entity)
        {
            if (TokenResponseModel == null)
                return null;

            try
            {
                string jsonObj = JsonConvert.SerializeObject(entity);
                if (entity.ReferCode == string.Empty)
                {
                    jsonObj = jsonObj.Replace(@"""refer_code"":"""",", "");
                }

                byte[] body = Encoding.UTF8.GetBytes(jsonObj);

                HttpWebRequest request = new HttpWebRequest(new Uri(String.Format("{0}users", Constants.WebAPIBaseAddress)));
                request.Headers.Set(HttpRequestHeader.Authorization, "Token " + TokenResponseModel.Token);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.ContentLength = body.Length;

                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(body, 0, body.Length);
                    stream.Close();
                }

                var httpResponse = await request.GetResponseAsync();

                string jsonResult;
                using (Stream responseStream = httpResponse.GetResponseStream())
                {
                    jsonResult = new StreamReader(responseStream).ReadToEnd();
                }

                IsConnected = true;

                var result = JsonConvert.DeserializeObject<UserContainer>(jsonResult);

                IsConnected = true;
                return result.Data;

            }
            catch (WebException ex)
            {
                //var answer = CreateException((HttpWebResponse)((WebException)ex).Response);
                IsConnected = IsConnectionAlive(ex.Status);
                return null;
            }
            catch (Exception ex)
            {
                IsConnected = true;
                return null;
            }
        }

        public static async Task<bool> UpdateUser(UserForUpdate entity, string userId)
        {
            if (TokenResponseModel == null)
                return false;

            try
            {
                string jsonObj = JsonConvert.SerializeObject(entity);
                byte[] body = Encoding.UTF8.GetBytes(jsonObj);

                HttpWebRequest request = new HttpWebRequest(new Uri(String.Format("{0}users/" + userId, Constants.WebAPIBaseAddress)));
                request.Headers.Set(HttpRequestHeader.Authorization, "Token " + TokenResponseModel.Token);
                request.Method = "PUT";
                request.ContentType = "application/json";
                request.ContentLength = body.Length;

                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(body, 0, body.Length);
                    stream.Close();
                }

                await request.GetResponseAsync();
                IsConnected = true;
                return true;

            }
            catch (WebException ex)
            {
                //var answer = CreateException((HttpWebResponse)((WebException)ex).Response);
                IsConnected = IsConnectionAlive(ex.Status);
                return false;
            }
            catch (Exception ex)
            {
                IsConnected = true;
                return false;
            }
        }

        #endregion

        #region Incident

        public static async Task<List<Incident>> GetIncidents(string contractId)
        {
            if (TokenResponseModel == null)
                return null;

            try
            {
                HttpWebRequest request = new HttpWebRequest(new Uri(String.Format("{0}incidents/?contract=" + contractId, Constants.WebAPIBaseAddress)));
                request.Headers.Set(HttpRequestHeader.Authorization, "Token " + TokenResponseModel.Token);
                request.Method = "GET";
                request.ContentType = "application/json";

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

                var value = JsonConvert.DeserializeObject<IncidentsContainer>(json, settings);
                if (value != null && value.Data != null && value.Data.Count > 0)
                {
                    return value.Data;
                }

                return null;
            }
            catch (WebException ex)
            {
                IsConnected = IsConnectionAlive(ex.Status);
                return null;
            }
            catch (Exception ex)
            {
                IsConnected = true;
                return null;
            }
        }

        public static async Task<Incident> CreateIncident(Incident entity)
        {
            if (TokenResponseModel == null)
                return null;

            try
            {
                string jsonObj = JsonConvert.SerializeObject(entity);

                byte[] body = Encoding.UTF8.GetBytes(jsonObj);

                HttpWebRequest request = new HttpWebRequest(new Uri(String.Format("{0}incidents", Constants.WebAPIBaseAddress)));
                request.Headers.Set(HttpRequestHeader.Authorization, "Token " + TokenResponseModel.Token);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.ContentLength = body.Length;

                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(body, 0, body.Length);
                    stream.Close();
                }

                var httpResponse = await request.GetResponseAsync();

                string jsonResult;
                using (Stream responseStream = httpResponse.GetResponseStream())
                {
                    jsonResult = new StreamReader(responseStream).ReadToEnd();
                }

                IsConnected = true;

                var result = JsonConvert.DeserializeObject<IncidentContainer>(jsonResult);

                IsConnected = true;
                return result.Data;

            }
            catch (WebException ex)
            {
                IsConnected = IsConnectionAlive(ex.Status);
                return null;
            }
            catch (Exception ex)
            {
                IsConnected = true;
                return null;
            }
        }

        #endregion

        #region Loss

        public static async Task<List<Loss>> GetLosses(string contractId)
        {
            if (TokenResponseModel == null)
                return null;

            try
            {
                HttpWebRequest request = new HttpWebRequest(new Uri(String.Format("{0}losses/?contract=" + contractId, Constants.WebAPIBaseAddress)));
                request.Headers.Set(HttpRequestHeader.Authorization, "Token " + TokenResponseModel.Token);
                request.Method = "GET";
                request.ContentType = "application/json";

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

                var value = JsonConvert.DeserializeObject<LossesContainer>(json, settings);
                if (value != null && value.Data != null && value.Data.Count > 0)
                {
                    return value.Data;
                }

                return null;
            }
            catch (WebException ex)
            {
                IsConnected = IsConnectionAlive(ex.Status);
                return null;
            }
            catch (Exception ex)
            {
                IsConnected = true;
                return null;
            }
        }

        public static async Task<Loss> GetLoss(string incidentId)
        {
            if (TokenResponseModel == null)
                return null;

            try
            {
                HttpWebRequest request = new HttpWebRequest(new Uri(String.Format("{0}losses/?incident=" + incidentId, Constants.WebAPIBaseAddress)));
                request.Headers.Set(HttpRequestHeader.Authorization, "Token " + TokenResponseModel.Token);
                request.Method = "GET";
                request.ContentType = "application/json";

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

                var value = JsonConvert.DeserializeObject<LossesContainer>(json, settings);
                if (value != null && value.Data != null && value.Data.Count > 0)
                {
                    return value.Data.FirstOrDefault();
                }

                return null;
            }
            catch (WebException ex)
            {
                IsConnected = IsConnectionAlive(ex.Status);
                return null;
            }
            catch (Exception ex)
            {
                IsConnected = true;
                return null;
            }
        }

        #endregion

        #region Dictionary 

        public static async Task<List<Dictionary>> GetDictionaries()
        {
            if (TokenResponseModel == null)
                return null;

            try
            {
                HttpWebRequest request = new HttpWebRequest(new Uri(String.Format("{0}dictionaries", Constants.WebAPIBaseAddress)));
                request.Headers.Set(HttpRequestHeader.Authorization, "Token " + TokenResponseModel.Token);
                request.Method = "GET";
                request.ContentType = "application/json";

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

                var value = JsonConvert.DeserializeObject<DictionariesContainer>(json, settings);
                if (value != null && value.Data != null && value.Data.Count > 0)
                {
                    return value.Data;
                }

                return null;
            }
            catch (WebException ex)
            {
                IsConnected = IsConnectionAlive(ex.Status);
                return null;
            }
            catch (Exception ex)
            {
                IsConnected = true;
                return null;
            }
        }

        public static async Task<Dictionary> GetDictionary(int id)
        {
            if (TokenResponseModel == null)
                return null;

            try
            {
                HttpWebRequest request = new HttpWebRequest(new Uri(String.Format("{0}dictionaries/{1}", Constants.WebAPIBaseAddress, id)));
                request.Headers.Set(HttpRequestHeader.Authorization, "Token " + TokenResponseModel.Token);
                request.Method = "GET";
                request.ContentType = "application/json";

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

                var entity = JsonConvert.DeserializeObject<DictionaryContainer>(json, settings);
                if (entity != null && entity.Data != null)
                {
                    return entity.Data;
                }

                return null;
            }
            catch (WebException ex)
            {
                IsConnected = IsConnectionAlive(ex.Status);
                return null;
            }
            catch (Exception ex)
            {
                IsConnected = true;
                return null;
            }
        }

        public static async Task<Dictionary> GetDictionaryByCode(string code)
        {
            if (TokenResponseModel == null)
                return null;

            try
            {
                HttpWebRequest request = new HttpWebRequest(new Uri(String.Format("{0}dictionaries/?code=" + code, Constants.WebAPIBaseAddress)));
                request.Headers.Set(HttpRequestHeader.Authorization, "Token " + TokenResponseModel.Token);
                request.Method = "GET";
                request.ContentType = "application/json";

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

                var entity = JsonConvert.DeserializeObject<DictionaryContainer>(json, settings);
                if (entity != null && entity.Data != null)
                {
                    return entity.Data;
                }

                return null;
            }
            catch (WebException ex)
            {
                IsConnected = IsConnectionAlive(ex.Status);
                return null;
            }
            catch (Exception ex)
            {
                IsConnected = true;
                return null;
            }
        }

        #endregion

        #region Dictionary Items 

        public static async Task<List<DictionaryItem>> GetDictionaryItems(string dictionaryCode)
        {
            if (TokenResponseModel == null)
                return null;

            try
            {
                HttpWebRequest request = new HttpWebRequest(new Uri(String.Format("{0}dictionary-items/?dictionary_code=" + dictionaryCode, Constants.WebAPIBaseAddress)));
                request.Headers.Set(HttpRequestHeader.Authorization, "Token " + TokenResponseModel.Token);
                request.Method = "GET";
                request.ContentType = "application/json";

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

                var value = JsonConvert.DeserializeObject<DictionaryItemsContainer>(json, settings);
                if (value != null && value.Data != null && value.Data.Count > 0)
                {
                    return value.Data;
                }

                return null;
            }
            catch (WebException ex)
            {
                IsConnected = IsConnectionAlive(ex.Status);
                return null;
            }
            catch (Exception ex)
            {
                IsConnected = true;
                return null;
            }
        }

        #endregion

        #region Device

        public static async Task<Device> GetDevice(string id)
        {
            if (TokenResponseModel == null)
                return null;

            try
            {
                HttpWebRequest request = new HttpWebRequest(new Uri(String.Format("{0}devices/?id=" + id, Constants.WebAPIBaseAddress)));
                request.Headers.Set(HttpRequestHeader.Authorization, "Token " + TokenResponseModel.Token);
                request.Method = "GET";
                request.ContentType = "application/json";

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

                var entity = JsonConvert.DeserializeObject<DevicesContainer>(json, settings);
                if (entity != null && entity.Data != null && entity.Data.Count > 0)
                {
                    return entity.Data.FirstOrDefault();
                }

                return null;
            }
            catch (WebException ex)
            {
                IsConnected = IsConnectionAlive(ex.Status);
                return null;
            }
            catch (Exception ex)
            {
                IsConnected = true;
                return null;
            }
        }

        #endregion

        #region Scoring Rounds

        public static async Task<List<ScoringRound>> GetScoringRounds(string contractId)
        {
            if (TokenResponseModel == null)
                return null;

            try
            {
                HttpWebRequest request = new HttpWebRequest(new Uri(String.Format("{0}scoring-rounds/?contract=" + contractId, Constants.WebAPIBaseAddress)));
                request.Headers.Set(HttpRequestHeader.Authorization, "Token " + TokenResponseModel.Token);
                request.Method = "GET";
                request.ContentType = "application/json";

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

                var value = JsonConvert.DeserializeObject<ScoringRoundsContainer>(json, settings);
                if (value != null && value.Data != null && value.Data.Count > 0)
                {
                    return value.Data;
                }

                return null;
            }
            catch (WebException ex)
            {
                IsConnected = IsConnectionAlive(ex.Status);
                return null;
            }
            catch (Exception ex)
            {
                IsConnected = true;
                return null;
            }
        }

        public static async Task<List<ScoringRound>> GetScoringRoundsForPeriod(string contractId, int limit)
        {
            if (TokenResponseModel == null)
                return null;

            try
            {
                HttpWebRequest request = new HttpWebRequest(new Uri(String.Format("{0}scoring-rounds/?contract=" + contractId + "&limit=" + limit.ToString() + "&ordering=-score_day&format_code=day", Constants.WebAPIBaseAddress))); //&format_code=DI_day
                request.Headers.Set(HttpRequestHeader.Authorization, "Token " + TokenResponseModel.Token);
                request.Method = "GET";
                request.ContentType = "application/json";

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

                var value = JsonConvert.DeserializeObject<ScoringRoundsContainer>(json, settings);
                if (value != null && value.Data != null && value.Data.Count > 0)
                {
                    return value.Data;
                }

                return null;
            }
            catch (WebException ex)
            {
                //var answer = CreateException((HttpWebResponse)((WebException)ex).Response);
                IsConnected = IsConnectionAlive(ex.Status);
                return null;
            }
            catch (Exception ex)
            {
                IsConnected = true;
                return null;
            }
        }

        public static async Task<List<ScoringRound>> GetMarkForDay(string contractId, int limit)
        {
            if (TokenResponseModel == null)
                return null;

            try
            {
                HttpWebRequest request = new HttpWebRequest(new Uri(String.Format("{0}scoring-rounds/?contract=" + contractId + "&limit=" + limit.ToString() + "&ordering=-score_day&format_code=day", Constants.WebAPIBaseAddress)));
                request.Headers.Set(HttpRequestHeader.Authorization, "Token " + TokenResponseModel.Token);
                request.Method = "GET";
                request.ContentType = "application/json";

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

                var value = JsonConvert.DeserializeObject<ScoringRoundsContainer>(json, settings);
                if (value != null && value.Data != null && value.Data.Count > 0)
                {
                    return value.Data;
                }

                return null;
            }
            catch (WebException ex)
            {
                //var answer = CreateException((HttpWebResponse)((WebException)ex).Response);
                IsConnected = IsConnectionAlive(ex.Status);
                return null;
            }
            catch (Exception ex)
            {
                IsConnected = true;
                return null;
            }
        }

        public static async Task<List<ScoringRound>> GetMarkForPeriod(string contractId)
        {
            if (TokenResponseModel == null)
                return null;

            try
            {
                HttpWebRequest request = new HttpWebRequest(new Uri(String.Format("{0}scoring-rounds/?contract=" + contractId + "&format_code=DI_day&limit=1&ordering=-score_day", Constants.WebAPIBaseAddress)));
                request.Headers.Set(HttpRequestHeader.Authorization, "Token " + TokenResponseModel.Token);
                request.Method = "GET";
                request.ContentType = "application/json";

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

                var value = JsonConvert.DeserializeObject<ScoringRoundsContainer>(json, settings);
                if (value != null && value.Data != null && value.Data.Count > 0)
                {
                    return value.Data;
                }

                return null;
            }
            catch (WebException ex)
            {
                //var answer = CreateException((HttpWebResponse)((WebException)ex).Response);
                IsConnected = IsConnectionAlive(ex.Status);
                return null;
            }
            catch (Exception ex)
            {
                IsConnected = true;
                return null;
            }
        }

        #endregion

        #region Scoring Round Parameters

        public static async Task<List<ScoringRoundParameter>> GetScoringRoundParameters(string roundId)
        {
            if (TokenResponseModel == null)
                return null;

            try
            {
                HttpWebRequest request = new HttpWebRequest(new Uri(String.Format("{0}scoring-round-parameters/?round=" + roundId, Constants.WebAPIBaseAddress)));
                request.Headers.Set(HttpRequestHeader.Authorization, "Token " + TokenResponseModel.Token);
                request.Method = "GET";
                request.ContentType = "application/json";

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

                var value = JsonConvert.DeserializeObject<ScoringRoundParametersContainer>(json, settings);
                if (value != null && value.Data != null && value.Data.Count > 0)
                {
                    return value.Data;
                }

                return null;
            }
            catch (WebException ex)
            {
                IsConnected = IsConnectionAlive(ex.Status);
                return null;
            }
            catch (Exception ex)
            {
                IsConnected = true;
                return null;
            }
        }

        #endregion

        #region Tracking

        public static async Task<Tracking> GetTracking()
        {
            if (TokenResponseModel == null)
                return null;

            try
            {
                HttpWebRequest request = new HttpWebRequest(new Uri(String.Format("{0}tracking", Constants.WebAPIBaseAddress)));
                request.Headers.Set(HttpRequestHeader.Authorization, "Token " + TokenResponseModel.Token);
                request.Method = "GET";
                request.ContentType = "application/json";

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

                var value = JsonConvert.DeserializeObject<TrackingsContainer>(json, settings);
                if (value != null && value.Data != null && value.Data.Count > 0)
                {
                    return value.Data.FirstOrDefault();
                }

                return null;
            }
            catch (WebException ex)
            {
                var answer = CreateException((HttpWebResponse)((WebException)ex).Response);
                IsConnected = IsConnectionAlive(ex.Status);
                return null;
            }
            catch (Exception ex)
            {
                IsConnected = true;
                return null;
            }
        }

        public static async Task<List<Tracking>> GetTripTrackings(string carId, string timeStart, string timeEnd)
        {
            if (TokenResponseModel == null)
                return null;

            try
            {
                HttpWebRequest request = new HttpWebRequest(new Uri(String.Format("{0}tracking?car=" + carId + "&distance__gt=0&ordering=time_end&time_end__lte=" + timeEnd + "&time_start__gte=" + timeStart, Constants.WebAPIBaseAddress)));
                request.Headers.Set(HttpRequestHeader.Authorization, "Token " + TokenResponseModel.Token);
                request.Method = "GET";
                request.ContentType = "application/json";

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

                var value = JsonConvert.DeserializeObject<TrackingsContainer>(json, settings);
                if (value != null && value.Data != null && value.Data.Count > 0)
                {
                    return value.Data;
                }

                return null;
            }
            catch (WebException ex)
            {
                IsConnected = IsConnectionAlive(ex.Status);
                return null;
            }
            catch (Exception ex)
            {
                IsConnected = true;
                return null;
            }
        }

        #endregion

        #region Tracking Day

        public static async Task<List<TrackingDay>> GetTrackingDaysForPeriod(string carId, int limit, string dayStart)
        {
            if (TokenResponseModel == null)
                return null;

            try
            {
                HttpWebRequest request = new HttpWebRequest(new Uri(String.Format("{0}trackingday/?car=" + carId + "&limit=" + limit.ToString() + "&trackparam_code=distance&day__gte=" + dayStart, Constants.WebAPIBaseAddress)));
                request.Headers.Set(HttpRequestHeader.Authorization, "Token " + TokenResponseModel.Token);
                request.Method = "GET";
                request.ContentType = "application/json";

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

                var value = JsonConvert.DeserializeObject<TrackingDaysContainer>(json, settings);
                if (value != null && value.Data != null && value.Data.Count > 0)
                {
                    return value.Data;
                }

                return null;
            }
            catch (WebException ex)
            {
                var answer = CreateException((HttpWebResponse)((WebException)ex).Response);
                IsConnected = IsConnectionAlive(ex.Status);
                return null;
            }
            catch (Exception ex)
            {
                IsConnected = true;
                return null;
            }
        }

        #endregion

        #region Attachments

        public static async Task<FileUploadResponse> UploadAttachment(string fileName, string fileContentType, byte[] file)
        {
            if (TokenResponseModel == null)
                return null;

            try
            {
                HttpWebRequest request = new HttpWebRequest(new Uri(String.Format("{0}storage", Constants.WebAPIBaseAddress)));
                request.Headers.Set(HttpRequestHeader.Authorization, "Token " + TokenResponseModel.Token);
                request.Method = "POST";
                string boundary = DateTime.Now.Ticks.ToString("x");
                request.ContentType = "multipart/form-data; boundary=------------------------" + boundary;

                string fileHeader = string.Format("------------------------{0}\r\nContent-Disposition: form-data; name=\"{1}\"; filename*=\"{2}\";\r\nContent-Type: {3}\r\n\r\n", boundary, "file", fileName, fileContentType);
                string boundaryClose = "\r\n--------------------------" + boundary + "--\r\n";
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(Encoding.UTF8.GetBytes(fileHeader), 0, fileHeader.Length);
                    stream.Write(file, 0, file.Length);
                    stream.Write(Encoding.UTF8.GetBytes(boundaryClose), 0, boundaryClose.Length);
                    stream.Close();
                }

                var httpResponse = await request.GetResponseAsync();

                string jsonResult;
                using (Stream responseStream = httpResponse.GetResponseStream())
                {
                    jsonResult = new StreamReader(responseStream).ReadToEnd();
                }

                IsConnected = true;

                var result = JsonConvert.DeserializeObject<FileUploadResponse>(jsonResult);

                return result;

            }
            catch (WebException ex)
            {
                var answer = CreateException((HttpWebResponse)((WebException)ex).Response);
                IsConnected = IsConnectionAlive(ex.Status);
                return null;
            }
            catch (Exception ex)
            {
                IsConnected = true;
                return null;
            }
        }

        #endregion

        #region Exceptions

        private static Exception CreateException(HttpWebResponse response)
        {
            if ((int)response.StatusCode >= 400)
            {
                string defaultMessage = $"HTTP status { (int)response.StatusCode }. { Enum.GetName(typeof(HttpStatusCode), response.StatusCode)}";
                string rawResponseContent = null;
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    rawResponseContent = streamReader.ReadToEnd();
                }
                var responseContent = JsonConvert.DeserializeObject<HeliumApiResponseContent<JObject>>(rawResponseContent);
                return new HeliumApiException(responseContent != null ? !String.IsNullOrEmpty(responseContent.Message) ? responseContent.Message : defaultMessage : defaultMessage);
            }
            else
            {
                return new HeliumApiException($"Can't process the response with HTTP status { (int)response.StatusCode }");
            }
        }

        #endregion

        #region Traking Trips

        public static async Task<List<TrackingTrip>> GetUserTripsWithPaging(string carId, string timeEnd)
        {
            if (TokenResponseModel == null)
                return null;

            try
            {
                HttpWebRequest request = new HttpWebRequest(new Uri(String.Format("{0}tracking-trip?car=" + carId + "&offset=0&trip_type=motion&ordering=-time_end&time_end__lte=" + timeEnd + "&limit=" + 7, Constants.WebAPIBaseAddress)));
                request.Headers.Set(HttpRequestHeader.Authorization, "Token " + TokenResponseModel.Token);
                request.Method = "GET";
                request.ContentType = "application/json";

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

                var value = JsonConvert.DeserializeObject<TrackingTripsContainer>(json, settings);
                if (value != null && value.Data != null && value.Data.Count > 0)
                {
                    return value.Data;
                }

                return null;
            }
            catch (WebException ex)
            {
                IsConnected = IsConnectionAlive(ex.Status);
                return null;
            }
            catch (Exception ex)
            {
                IsConnected = true;
                return null;
            }
        }

        public static async Task<List<TrackingTrip>> GetUserTrips(string carId, string timeStart, string timeEnd)
        {
            if (TokenResponseModel == null)
                return null;

            try
            {
                HttpWebRequest request = new HttpWebRequest(new Uri(String.Format("{0}tracking-trip?car=" + carId + "&offset=0&trip_type=motion&ordering=-time_end&time_end__lte=" + timeEnd + "&time_start__gte=" + timeStart, Constants.WebAPIBaseAddress)));
                request.Headers.Set(HttpRequestHeader.Authorization, "Token " + TokenResponseModel.Token);
                request.Method = "GET";
                request.ContentType = "application/json";

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

                var value = JsonConvert.DeserializeObject<TrackingTripsContainer>(json, settings);
                if (value != null && value.Data != null && value.Data.Count > 0)
                {
                    return value.Data;
                }

                return null;
            }
            catch (WebException ex)
            {
                IsConnected = IsConnectionAlive(ex.Status);
                return null;
            }
            catch (Exception ex)
            {
                IsConnected = true;
                return null;
            }
        }

        public static async Task<TrackingTrip> GetTrackingTrip(int id)
        {
            if (TokenResponseModel == null)
                return null;

            try
            {
                HttpWebRequest request = new HttpWebRequest(new Uri(String.Format("{0}tracking-trip/{1}", Constants.WebAPIBaseAddress, id)));
                request.Headers.Set(HttpRequestHeader.Authorization, "Token " + TokenResponseModel.Token);
                request.Method = "GET";
                request.ContentType = "application/json";

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

                var entity = JsonConvert.DeserializeObject<TrackingTripContainer>(json, settings);
                if (entity != null && entity.Data != null)
                {
                    return entity.Data;
                }

                return null;
            }
            catch (WebException ex)
            {
                IsConnected = IsConnectionAlive(ex.Status);
                return null;
            }
            catch (Exception ex)
            {
                IsConnected = true;
                return null;
            }
        }

        public static string Convert(string str)
        {
            byte[] utf8Bytes = Encoding.UTF8.GetBytes(str);
            str = Encoding.UTF8.GetString(utf8Bytes);
            return str;
        }

        #endregion

        #endregion
    }

    public class HeliumApiException : Exception
    {
        public HeliumApiException(string message)
            : base(message)
        {

        }

        public HeliumApiException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
