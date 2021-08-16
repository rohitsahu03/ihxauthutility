using AuthUtility.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace AuthUtility.Common
{
    public class RestClient : IRestClient
    {
        private readonly ILogger _logger;
        private readonly IHttpClientFactory _clientFactory;
        public RestClient(IHttpClientFactory clientFactory, ILogger<RestClient> logger)
        {
            _clientFactory = clientFactory;
            _logger = logger;
        }
        private HttpClient CreateClient(string Uri, bool KeepAlive = true)
        {
            HttpClient client = _clientFactory.CreateClient();
            client.BaseAddress = new Uri(Uri);
            client.DefaultRequestHeaders.ConnectionClose = !KeepAlive;
            return client;
        }

        public V MakePostRestCall<K, V>(K request, string absolutepath, string serviceUri, bool isElastic)
        {
            V returnval = default(V);
            try
            {
                var restClient = CreateClient(serviceUri);
                var stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

                var restTasks = restClient.PostAsync(absolutepath, stringContent);
                restTasks.Wait();
                HttpResponseMessage response = restTasks.Result;

                if (response != null && response.IsSuccessStatusCode)
                {
                    var taskResponseAsString = response.Content.ReadAsStringAsync();
                    taskResponseAsString.Wait();
                    returnval = JsonConvert.DeserializeObject<V>(taskResponseAsString.Result);
                }
            }
            catch (Exception Ex)
            {
                _logger.LogError($"{DateTime.Now} RestService Exception Occurred in MakePostRestCall : {Ex.Message}" + Environment.NewLine + $"For URL: {serviceUri + absolutepath}" + Environment.NewLine + $" For Request {JsonConvert.SerializeObject(request)}" + Environment.NewLine);
                if (isElastic)
                    throw Ex;
            }
            return returnval;
        }

        public V MakeGetRestCall<V>(string absolutepath, string serviceUri)
        {
            V returnval = default(V);
            try
            {
                var restClient = CreateClient(serviceUri);
                MediaTypeWithQualityHeaderValue contentType = new MediaTypeWithQualityHeaderValue("application/json");
                restClient.DefaultRequestHeaders.Accept.Add(contentType);
                var taskResponse = restClient.GetAsync(absolutepath);
                taskResponse.Wait();
                HttpResponseMessage response = taskResponse.Result;
                if (response != null && response.IsSuccessStatusCode)
                {
                    var taskResponseAsString = response.Content.ReadAsStringAsync();
                    taskResponseAsString.Wait();
                    returnval = JsonConvert.DeserializeObject<V>(taskResponseAsString.Result);
                }
                return returnval;
            }
            catch (Exception Ex)
            {
                _logger.LogError($"{DateTime.Now} RestService Exception Occurred in MakeGetRestCall : {Ex.Message}" + Environment.NewLine + $"For URL: {serviceUri + absolutepath}" + Environment.NewLine);
            }
            return returnval;
        }

        public V MakeGetCallWithAbsoluteUri<V>(string url)
        {
            V returnval = default(V);
            try
            {
                var client = CreateClient(url);
                var taskResponse = client.GetAsync(url);
                taskResponse.Wait();
                HttpResponseMessage response = taskResponse.Result;
                if (response != null && response.IsSuccessStatusCode)
                {
                    var taskResponseAsString = response.Content.ReadAsStringAsync();
                    taskResponseAsString.Wait();
                    returnval = JsonConvert.DeserializeObject<V>(taskResponseAsString.Result);
                }
            }
            catch (Exception Ex)
            {
                _logger.LogError($"{DateTime.Now} RestService Exception Occurred in MakeGetCallWithAbsoluteUri : {Ex.Message}" + Environment.NewLine + $"For URL: {url}");
            }
            return returnval;
        }


    }
}