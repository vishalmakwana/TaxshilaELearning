using Acr.UserDialogs;
using TaxshilaMobile.DataTypesApp.Default;
using TaxshilaMobile.Models;
using TaxshilaMobile.ServiceBus.OfflineSync.Models;
using TaxshilaMobile.ServiceBus.OnlineSync;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TaxshilaMobile.Helpers
{
    public class RestApiHelper : RestHelperBase
    {
        private readonly HttpClient _httpClient;


        #region Services

        private IUserDialogs UserDialogsService { get; set; }

        #endregion

        #region Constructor

        public RestApiHelper()
        {
            UserDialogsService = UserDialogs.Instance;
            //_httpClient = new HttpClient(new NativeMessageHandler())
            _httpClient = new HttpClient()
            {
                // for testing timeouts
                Timeout = new TimeSpan(0, 20, 0)

            };
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
        }

        static RestApiHelper()
        {

        }

        #endregion

        #region Methods

        /// <summary>
        /// Sends a GET request to the API endpoint specified and returns the response value       
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="endpoint">a string</param>
        /// <param name="json">a json string containing the parameters to send to the API</param>
        /// <param name="baseUrl">[Optional] the base URL string for the API. If this is not set, the default baseUrl will be used.</param>
        /// <param name="isLocalhost">[Optional] a flag to determine if the baseUrl should be overridden with App.LocalhostUrl </param>        
        /// <returns>
        /// The deserialized response from the API of type T.
        /// <para>
        /// If it cannot be deserialized as type T, or an exception occurs, a new instance of T will be created with the following properties attemped to be set:
        ///     Success: false
        ///     Message: a detailed message about the error
        ///     StackTrace: the stack trace from the exception
        /// </para>
        /// </returns>
        public async Task<T> GetAsync<T>(string endpoint, bool isLocalhost = false, bool isLoggingIn = false) where T : new()
        {
            T objResponse;
            string uriRequest = "";

            try
            {
                //Configures the Request
                uriRequest = FormatUriRequest(endpoint);
                Debug.WriteLine("GET ASYNC: " + uriRequest);
                HttpRequestMessage httpContent = new HttpRequestMessage(HttpMethod.Get, uriRequest);
                if (!string.IsNullOrEmpty(_settings.Token))
                    httpContent.Headers.Authorization = new AuthenticationHeaderValue("bearer", _settings.Token);
                httpContent.Headers.ExpectContinue = false;

                //Sends the request and reads the response
                HttpResponseMessage response = await _httpClient.SendAsync(httpContent).ConfigureAwait(false);
                objResponse = await GetResponseAsync<T>(response, uriRequest).ConfigureAwait(false);
            }
            catch (Exception ex)
            {

                Debug.WriteLine($"ERROR RestApiHelper.GetAsync: url'{uriRequest}' {ex}");

                //Send Error to App Center
                dynamic errorContext = new { uriRequest };
                // AppCenterHelper.TrackError(ex, errorContext);

                objResponse = GetErrorResponse<T>(ex);
            }

            return objResponse;
        }

        /// <summary>
        /// Sends a POST request to the API endpoint specified and returns the response value       
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="endpoint">a string</param>
        /// <param name="json">a json string containing the parameters to send to the API</param>
        /// <param name="baseUrl">[Optional] the base URL string for the API. If this is not set, the default baseUrl will be used.</param>
        /// <param name="isLocalhost">[Optional] a flag to determine if the baseUrl should be overridden with App.LocalhostUrl </param>        
        /// <returns>
        /// The deserialized response from the API of type T.
        /// <para>
        /// If it cannot be deserialized as type T, or an exception occurs, a new instance of T will be created with the following properties attemped to be set:
        ///     Success: false
        ///     Message: a detailed message about the error
        ///     StackTrace: the stack trace from the exception
        /// </para>
        /// </returns>
        public async Task<T> PostAsync<T>(string endpoint, string json, string baseUrl = null, bool isLocalhost = false, bool isLoggingIn = false) where T : new()
        {
            T objResponse;
            string uriRequest = "";
            HttpResponseMessage response = null;
            try
            {
                //Configures the Request
                uriRequest = FormatUriRequest(endpoint, baseUrl);
                Debug.WriteLine("POST ASYNC: " + uriRequest);
                HttpRequestMessage httpContent = new HttpRequestMessage(HttpMethod.Post, uriRequest);
                HttpContent contentPost = new StringContent(json, Encoding.UTF8, "application/json");

                if (!string.IsNullOrEmpty(_settings.Token))
                    httpContent.Headers.Authorization = new AuthenticationHeaderValue("bearer", _settings.Token);

                httpContent.Headers.ExpectContinue = false;
                httpContent.Content = contentPost;

                //Sends the request and reads the response
                response = await _httpClient.SendAsync(httpContent).ConfigureAwait(false);
                objResponse = await GetResponseAsync<T>(response, uriRequest).ConfigureAwait(false);
                }
            catch (Exception ex)
            {
                Debug.WriteLine($"ERROR RestApiHelper.GetAsync: url'{uriRequest}' {ex}");
                //logs the exception in the App Center
                dynamic errorContext = new { uriRequest };
                string textAttachment = $"Request: {json}";
                if (response != null)
                {
                    var statusCode = response.StatusCode;

                    errorContext = new { uriRequest, statusCode };
                }
              //  AppCenterHelper.TrackError(ex, errorContext, textAttachment);


                objResponse = GetErrorResponse<T>(ex);
            }

            return objResponse;
        }

        public async Task<bool> RequestTokenAsync(string url)
        {
            RequestToken rt = new RequestToken();
            string accessToken = string.Empty;

            try
            {
                //var body = new List<KeyValuePair<string, string>>
                //{
                //    new KeyValuePair<string, string>("grant_type", rt.grant_type),
                //    new KeyValuePair<string, string>("username", rt.username),
                //    new KeyValuePair<string, string>("password", rt.password)
                //};
                //url = "http://localhost/mobileapi/api/auth/token";
                var httpContent = new HttpRequestMessage(HttpMethod.Get, url);
                httpContent.Headers.ExpectContinue = false;
                //httpContent.Content = new FormUrlEncodedContent(body);

                var response = await _httpClient.SendAsync(httpContent).ConfigureAwait(false);
                var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                JObject results = JObject.Parse(result);
                if (results != null)
                {
                    ResponseToken responsejson = JsonConvert.DeserializeObject<ResponseToken>(result);
                    accessToken = responsejson.access_token;
                }

                _settings.Token = accessToken;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        /// <summary>
        /// Creates a instance of T and attempts to populate fields that are usually present (ApiResponse is normally the base type of T)
        /// </summary>
        /// <remarks>
        /// The assumption is that the base type T = ApiResponse.  Since the property Message, StackTrace and Success aren't guaranteed to exist, reflection is used to attempt to set them.
        /// </remarks>
        /// <param name="ex">an Exception</param>
        /// <param name="message">a string containing an error message</param>
        /// <returns>
        /// A new instance of T, with the exception data attempted to be added. 
        /// </returns>
        private T GetErrorResponse<T>(Exception ex = null, string message = null) where T : new()
        {
            T obj = new T();
            Type type = obj.GetType();
            var errorResponse = (T)Activator.CreateInstance(type);

            if (ex != null)
            {
                message = message != null ? $"{message}; {ex.Message}" : ex.Message;
                TrySetProperty(errorResponse, "StackTrace", ex.StackTrace);
            }
            if (message != null)
            {
                TrySetProperty(errorResponse, "Message", message);
            }

            TrySetProperty(errorResponse, "Success", false);

            return errorResponse;
        }

        /// <summary>
        /// Reads the response from the HttpResponseMessage, parses it and returns the object
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="response">an HttpResponseMessage instance</param>
        /// <param name="uriRequest">a string containing the uri of the request. Used for debugging purposes</param>
        /// <returns>
        /// an instance of type T containing the parsed response content
        /// </returns>
        private async Task<T> GetResponseAsync<T>(HttpResponseMessage response, string uriRequest) where T : new()
        {
            T objResponse = default;
            if (response != null)
            {
                if (response.IsSuccessStatusCode)
                {
                    //using (var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                    //using (var decompressed = new GZipStream(stream, CompressionMode.Decompress))
                    //using (var reader = new StreamReader(decompressed))
                    //{
                    //    string strResponse = reader.ReadToEnd();
                    //    var jobj = JObject.Parse(strResponse);

                    //    objResponse = JsonConvert.DeserializeObject<T>(jobj.ToString());
                    //}

                    using (var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                    using (var reader = new StreamReader(stream))
                    {
                        string strResponse = reader.ReadToEnd();
                        var jobj = JObject.Parse(strResponse);

                        objResponse = JsonConvert.DeserializeObject<T>(jobj.ToString());
                    }
                }
                else
                {
                    Debug.WriteLine($"ERROR RestApiHelper.GetResponseAsync: url'{uriRequest}' {response.ReasonPhrase}");
                    objResponse = GetErrorResponse<T>(message: $"url'{uriRequest}' {response.ReasonPhrase}");
                }
            }
            else
            {
                objResponse = GetErrorResponse<T>(message: "Invalid Response.");
            }

            return objResponse;
        }


        //public async Task<string> RequestTokenAsync()
        //{
        //    RequestToken rt = new RequestToken();
        //    string accessToken = string.Empty;

        //    try
        //    {
        //        var body = new List<KeyValuePair<string, string>>
        //        {
        //            new KeyValuePair<string, string>("grant_type", rt.grant_type),
        //            new KeyValuePair<string, string>("username", rt.username),
        //            new KeyValuePair<string, string>("password", rt.password)
        //        };

        //        var httpContent = new HttpRequestMessage(HttpMethod.Post, _settings.BaseUrl + Constants.Get_Token);
        //        httpContent.Headers.ExpectContinue = false;
        //        httpContent.Content = new FormUrlEncodedContent(body);

        //        var response = await _httpClient.SendAsync(httpContent).ConfigureAwait(false);
        //        var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        //        JObject results = JObject.Parse(result);
        //        if (results != null)
        //        {
        //            ResponseToken responsejson = JsonConvert.DeserializeObject<ResponseToken>(result);
        //            accessToken = responsejson.access_token;
        //        }

        //        _settings.Token = accessToken;
        //        return accessToken;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        #endregion

    }


    public class RestApiResponseHelper : RestHelperBase
    {
        private readonly HttpClient _httpClient;
        private string errorResponse;

        #region Services

        #endregion

        #region Constructor

        public RestApiResponseHelper()
        {
            //_httpClient = new HttpClient(new NativeMessageHandler())
            _httpClient = new HttpClient()
            {
                // for testing timeouts
                Timeout = new TimeSpan(0, 20, 0)

            };
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));

        }

        static RestApiResponseHelper()
        {

        }
        #endregion

        #region Methods

        /// <summary>
        /// Sends an GET request to the API endpoint specified, unwraps the content from the ApiResponse and returns the result.         
        /// </summary>
        /// <remarks>
        ///  
        /// </remarks>
        /// <param name="endpoint">a string</param>
        /// <param name="json">a json string containing the parameters to send to the API</param>
        /// <param name="baseUrl">[Optional] the base URL string for the API. If this is not set, the default baseUrl will be used.</param>
        /// <param name="isLocalhost">[Optional] a flag to determine if the baseUrl should be overridden with App.LocalhostUrl </param>        
        /// <returns>
        /// The ResponseContent of the ApiResponse. If the returned response from the API was malformed, null, or an exception occurred, the returned value will depend on the type of T
        /// <para>If T is newable, a new instance will be created and returned. If present, the property IsEmptyModel will be set to true</para>
        /// <para>If T is not newable, the default value will be returned.</para>
        /// </returns>
        public async Task<T> GetAsync<T>(string endpoint, [CallerMemberName] string callerName = "")
        {
            Response<T> apiResponse = null;
            string uriRequest = "";
            HttpResponseMessage response = null;

            errorResponse = null;
            try
            {
                //Configures the Request
                uriRequest = FormatUriRequest(endpoint);
                Debug.WriteLine("GET ASYNC: " + uriRequest);
                HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Get, uriRequest);
                if (!string.IsNullOrEmpty(_settings.Token))
                    httpRequest.Headers.Authorization = new AuthenticationHeaderValue("bearer", _settings.Token);
                httpRequest.Headers.ExpectContinue = false;

                //Sends the request and reads the response
                response = await _httpClient.SendAsync(httpRequest).ConfigureAwait(false);
                apiResponse = await GetApiResponseAsync<T>(response, uriRequest).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ERROR RestApiResponseHelper.GetAsync: url'{uriRequest}' {ex}");

                LogError(ex, uriRequest, callerName, response);

                apiResponse = GetErrorApiResponse<T>(ex);
            }
            finally
            {
                errorResponse = null;
            }

            return GetResultFromApiResponse(apiResponse);
        }

        /// <summary>
        /// Sends a POST request to the API endpoint specified, unwraps the content from the ApiResponse and returns the result.         
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="endpoint">a string</param>
        /// <param name="json">a json string containing the parameters to send to the API</param>
        /// <param name="baseUrl">[Optional] the base URL string for the API. If this is not set, the default baseUrl will be used.</param>
        /// <param name="isLocalhost">[Optional] a flag to determine if the baseUrl should be overridden with App.LocalhostUrl </param>        
        /// <returns>
        /// The ResponseContent of the ApiResponse. If the returned response from the API was malformed, null, or an exception occurred, the returned value will depend on the type of T
        /// <para>If T is newable, a new instance will be created and returned. If present, the property IsEmptyModel will be set to true</para>
        /// <para>If T is not newable, the default value will be returned.</para>
        /// </returns>
        public async Task<T> PostAsync<T>(string endpoint, string json, string baseUrl = null, [CallerMemberName] string callerName = "")
        {
            Response<T> apiResponse = null;
            string uriRequest = "";
            HttpResponseMessage response = null;

            errorResponse = null;
            try
            {
                //Configures the Request
                uriRequest = FormatUriRequest(endpoint, baseUrl);
                Debug.WriteLine("POST ASYNC: " + uriRequest);

                HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Post, uriRequest);
                HttpContent contentPost = new StringContent(json, Encoding.UTF8, "application/json");
                if (!string.IsNullOrEmpty(_settings.Token))
                    httpRequest.Headers.Authorization = new AuthenticationHeaderValue("bearer", _settings.Token);
                httpRequest.Headers.ExpectContinue = false;
                httpRequest.Content = contentPost;

                //Sends the request and reads the response
                response = await _httpClient.SendAsync(httpRequest).ConfigureAwait(false);
                apiResponse = await GetApiResponseAsync<T>(response, uriRequest).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ERROR RestApiResponseHelper.PostAsync: url'{uriRequest}' {ex}");
                LogError(ex, uriRequest, callerName, response, json);

                //Get a default response
                apiResponse = GetErrorApiResponse<T>(ex);
            }
            finally
            {
                errorResponse = null;
            }

            //Log results and return output
            T output = GetResultFromApiResponse(apiResponse);

            if (typeof(IEnumerable).IsAssignableFrom(typeof(T)))  //All List types
            {
                //Do logic when T is a list
                var count = TryGetProperty(output, "Count");
                Debug.WriteLine($"POST ASYNC {uriRequest} response count: {count}");
            }

            return output;
        }


        public async Task<bool> RequestTokenAsync(string url)
        {
            RequestToken rt = new RequestToken();
            string accessToken = string.Empty;

            try
            {
                //var body = new List<KeyValuePair<string, string>>
                //{
                //    new KeyValuePair<string, string>("grant_type", rt.grant_type),
                //    new KeyValuePair<string, string>("username", rt.username),
                //    new KeyValuePair<string, string>("password", rt.password)
                //};
                //url = "http://localhost/mobileapi/api/auth/token";
                var httpContent = new HttpRequestMessage(HttpMethod.Get, url);
                httpContent.Headers.ExpectContinue = false;
                //httpContent.Content = new FormUrlEncodedContent(body);

                var response = await _httpClient.SendAsync(httpContent).ConfigureAwait(false);
                var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                JObject results = JObject.Parse(result);
                if (results != null)
                {
                    ResponseToken responsejson = JsonConvert.DeserializeObject<ResponseToken>(result);
                    accessToken = responsejson.access_token;
                }

                _settings.Token = accessToken;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }



        /// <summary>
        /// Reads the response from the HttpResponseMessage, parses it and returns the ApiResponse<typeparamref name="T"/> object
        /// </summary>
        /// <remarks>
        /// An exception will be thrown for the following reasons:
        ///     if the response is null
        ///     if the response.IsSuccessStatusCode is false
        ///     if the response stream cannot be parsed into ApiResponse<typeparamref name="T"/> or T
        ///     if any other exceptions occur while attempting to read the response stream
        ///     
        /// These exceptions should be handled in the method that called this.
        /// </remarks>
        /// <param name="response">an HttpResponseMessage instance</param>
        /// <param name="uriRequest">a string containing the uri of the request. Used for debugging purposes</param>
        /// <returns>
        /// an ApiResponse<typeparamref name="T"/> instance containing the parsed response content
        /// </returns>
        private async Task<Response<T>> GetApiResponseAsync<T>(HttpResponseMessage response, string uriRequest)
        {
            Response<T> objResponse;
            string strResponse = "";

            if (response != null)
            {
                using (var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                using (var reader = new StreamReader(stream))
                {
                    strResponse = reader.ReadToEnd();
                }

                if (response.IsSuccessStatusCode)
                {
                    var jobj = JObject.Parse(strResponse);

                    try
                    {
                        objResponse = JsonConvert.DeserializeObject<Response<T>>(jobj.ToString());
                    }
                    catch
                    {
                        //If the response fails to deserialize as ApiResponse<T>, try just T 
                        T content = JsonConvert.DeserializeObject<T>(jobj.ToString());
                        objResponse = new Response<T>
                        {
                            Success = true,
                            ResponseContent = content
                        };
                    }
                }
                else
                {
                    Debug.WriteLine($"ERROR RestApiResponseHelper.GetApiResponseAsync: url'{uriRequest}' {response.ReasonPhrase}");

                    errorResponse = strResponse;
                    throw new Exception($"url'{uriRequest}' {response.ReasonPhrase}");
                }
            }
            else
            {
                throw new Exception("Invalid Response");
            }

            return objResponse;
        }

        /// <summary>
        /// Creates an instance of ApiResponse<typeparamref name="T"/> and sets the properties regarding the exception
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="ex">an Exception</param>
        /// <param name="message">a string containing an error message</param>
        /// <returns>
        /// A new instance of ApiResponse<typeparamref name="T"/>, with the exception data added. 
        /// </returns>
        private Response<T> GetErrorApiResponse<T>(Exception ex = null, string message = null)
        {
            Response<T> response = new Response<T>();

            if (ex != null)
            {
                message = message != null ? $"{message}; {ex.Message}" : ex.Message;
                response.StackTrace = ex.StackTrace;
            }
            if (message != null)
            {
                response.Message = message;
            }

            response.Success = false;

            return response;
        }

        /// <summary>
        /// Gets the ResponseContent from the ApiResponse<typeparamref name="T"/> object
        /// </summary>
        /// <remarks>
        /// If the apiResponse was not successful, creates a new instance of type T, attempts to set IsEmptyModel
        /// <para>if T is not newable, sets result to its default value</para>
        /// </remarks>
        /// <param name="apiResponse">an ApiResponse<typeparamref name="T"/> instance</param>
        /// <returns>
        /// A new instance of type T, or its default value if not newable
        /// </returns>
        private T GetResultFromApiResponse<T>(Response<T> apiResponse)
        {
            T result = default;


            if (apiResponse != null && apiResponse.Success)
            {
                result = apiResponse.ResponseContent;
            }
            if (result == null || apiResponse == null || !apiResponse.Success)
            {
                var constructorInfo = typeof(T).GetConstructor(Type.EmptyTypes);
                if (constructorInfo != null)
                {
                    result = (T)constructorInfo.Invoke(null);
                    TrySetProperty(result, "IsEmptyModel", true);
                    if (typeof(IEnumerable).IsAssignableFrom(typeof(T)))  //All List types
                    {
                        //Do logic when T is a list

                    }
                    if (typeof(ModelDTOBase).IsAssignableFrom(typeof(T)))  //All inherited ModelDTOBase types
                    {
                        //Do logic when T is a ModelDTOBase type
                    }
                }
                else
                {
                    result = default;
                }
            }


            return result;
        }

        /// <summary>
        /// Sends an Error Report to the App Center. Data sent includes the request URI, the calling method, and the request and response data if available
        /// </summary>        
        /// <param name="ex">the caught exception thrown</param>
        /// <param name="uriRequest">a string containing the URI endpoint to make the request</param>
        /// <param name="callerName">a string containing the calling method</param>
        /// <param name="response">the optional HttpResponseMessage</param>
        /// <param name="requestContent">a string containing the request json</param>        
        private void LogError(Exception ex, string uriRequest, string callerName, HttpResponseMessage response = null, string requestContent = null)
        {

            try
            {
                dynamic errorContext = new { uriRequest, callerName };
                string textAttachment = (requestContent != null) ? $"Request: {requestContent}" : "";
                if (response != null)
                {
                    var statusCode = response.StatusCode;

                    errorContext = new { uriRequest, callerName, statusCode };
                    if (errorResponse != null)
                    {
                        string responseMsg = $"Response: {errorResponse}";
                        textAttachment = (textAttachment != null) ? $"{textAttachment}; {responseMsg}" : responseMsg;
                    }
                }
               // AppCenterHelper.TrackError(ex, errorContext, textAttachment);
            }
            catch (Exception e)
            {
                Debug.WriteLine($"ERROR RestApiResponseHelper.LogError: {e}");
            }

        }


        #endregion
    }

    public class RestHelperBase
    {

        protected readonly IAppSettings _settings;

        public RestHelperBase()
        {
            _settings = DependencyService.Get<IAppSettings>();
        }


        /// <summary>
        /// Contructs the uriRequest string from the parameters given
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="endpoint">a string containing the API endpoint to access</param>
        /// <param name="baseUrl">[Optional] a string contiaining the url root to the API</param>
        /// <returns>
        /// a string containing the uri request
        /// </returns>
        protected string FormatUriRequest(string endpoint, string baseUrl = null)
        {
            bool isLocalhost = App.UseLocalService;
            string url = isLocalhost ? App.LocalhostUrl : !string.IsNullOrEmpty(baseUrl) ? baseUrl : _settings.BaseUrl;
            string uriRequest = $"{url}{endpoint}";

            return uriRequest;
        }



        /// <summary>
        /// Attempts to set an object's property (by the name of the property) to <paramref name="value"/> using reflection
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="obj">an object</param>
        /// <param name="property">the property name string</param>
        /// <param name="value">an object</param>
        protected void TrySetProperty(object obj, string property, object value)
        {
            var prop = obj?.GetType().GetProperty(property, BindingFlags.Public | BindingFlags.Instance);
            if (prop != null && prop.CanWrite)
                prop.SetValue(obj, value, null);
        }

        protected object TryGetProperty(object obj, string property)
        {
            var prop = obj.GetType().GetProperty(property, BindingFlags.Public | BindingFlags.Instance);
            if (prop != null && prop.CanRead)
                return prop.GetValue(obj);

            return null;
        }

    }

}
