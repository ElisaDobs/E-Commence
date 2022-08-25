using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net.Http.Headers;

namespace E_Commence.Common
{
    public class ConsumeApi
    {
        public static async Task<object> PostAsync<T>(string url, T objParam, string token)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["ECommenceAPIURL"]);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = await client.PostAsync(url, new StringContent(JsonConvert.SerializeObject(objParam), Encoding.Unicode, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    return result;
                }
                return default;
            }
        }

        public static async Task<object> PostTokenAsync<T>(string url, T objParam)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["ECommenceAPIURL"]);
                HttpResponseMessage response = await client.PostAsync(url, new StringContent(JsonConvert.SerializeObject(objParam), Encoding.Unicode, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    return result;
                }
                return default;
            }
        }


        public static async Task<object> GetAsync(string url, string token)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["ECommenceAPIURL"]);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;

                    return result;
                }
                return default;
            }    
        }
    }
}