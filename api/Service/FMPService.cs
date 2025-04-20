using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Stocks;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;

namespace api.Service
{
    public class FMPService : IFMPService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        public FMPService(HttpClient client, IConfiguration configuration)
        {
            _httpClient = client;
            _config = configuration;
        }
        public async Task<Stocks?> FindStockBySymbolAsync(string symbol)
        {
            
            try{
                var result = await _httpClient.GetAsync($"https://financialmodelingprep.com/api/v3/profile/{symbol}/?apikey={_config["FMPKey"]}");
                if(result.IsSuccessStatusCode){
                    var content = await result.Content.ReadAsStringAsync();
                    var task = JsonConvert.DeserializeObject<FMPStock[]>(content);
                    var stock = task[0];

                    if(stock != null){
                        return stock.ToStockFromFMPStock();
                    }
                    
                }
                return null;

            }catch(Exception ex){
                return null;
            }
        }
    }
}