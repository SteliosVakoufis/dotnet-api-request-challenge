using ipstack_lib.dto;
using ipstack_lib.exceptions;
using ipstack_lib.interfaces;
using Newtonsoft.Json.Linq;

namespace ipstack_lib
{
    public class IPInfoProvider : IIPInfoProvider
    {
        private readonly string _api = "http://api.ipstack.com/";
        private readonly string _apiKey = "?access_key=5c13a8e073be3aab6e5ab59bb0f701b7";

        async Task<IPDetails> IIPInfoProvider.GetDetails(string ip)
        {
            var uri = _api + ip + _apiKey;
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            var client = new HttpClient();

            var response = await client.SendAsync(request);
            var responseString = response.Content.ReadAsStringAsync().Result;
            var jsonObj = JObject.Parse(responseString);

            if (jsonObj.ContainsKey("success") && !(bool)jsonObj["success"])
            {
                throw new IPServiceNotAvailableException("IP service not available, Please try again.");
            }

            return ComposeIPDetails(jsonObj);
        }

        private IPDetails ComposeIPDetails(JObject jsonObj)
        {
            try
            {
                return new()
                {
                    City = (string)jsonObj["city"]!,
                    Country = (string)jsonObj["country_name"]!,
                    Continent = (string)jsonObj["continent_name"]!,
                    Latitude = (double)jsonObj["latitude"]!,
                    Longitude = (double)jsonObj["longitude"]!,
                };
            }
            catch (Exception)
            {
                throw new IPServiceNotAvailableException("Service not available, Please try again.");
            }
        }
    }
}