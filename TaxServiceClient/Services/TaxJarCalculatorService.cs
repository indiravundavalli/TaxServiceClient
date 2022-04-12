using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TaxServiceClient.Models;

namespace TaxServiceClient.Services
{
    public class TaxJarCalculatorService  : ITaxJarCalculatorService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly string _accessToken;
        public TaxJarCalculatorService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _accessToken = _configuration["TaxJar:Token"];

        }
        public async Task<RateResult> GetRatesByLocation(string zip)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",_accessToken);

            var response = await _httpClient.GetAsync($"rates/{zip}");

            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            var responseObject = await JsonSerializer.DeserializeAsync<RateResult>(responseStream);

            return responseObject;

        }

        public async Task<OrderResponse> CalculateSalesTax(OrderRequest request)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
            StringContent content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"taxes",content);

            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            var responseObject = await JsonSerializer.DeserializeAsync<OrderResponse>(responseStream);

            return responseObject;

        }
    }
}
