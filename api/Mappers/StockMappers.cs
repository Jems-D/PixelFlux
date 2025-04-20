using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Stocks;
using api.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace api.Mappers
{
    public static class StockMappers
    {
        public static StockDTO ToStockDto(this Stocks stockModel){
            return new StockDTO
            {
                Id = stockModel.Id,
                Symbol = stockModel.Symbol,
                CompanyName = stockModel.CompanyName,
                Purchase = stockModel.Purchase,
                LastDiv = stockModel.LastDiv,
                Industry = stockModel.Industry,
                MarketCap = stockModel.MarketCap,
                Comments = stockModel.Comments.Select(c => c.ToCommentDto()).ToList()
            };
        }
        //When inserting a data, it cannot be in the form of the DTO, so the return type should be the original
        public static Stocks ToStockFromCreateDto(this CreateStockRequestDTO stockDto){
            return new Stocks
            {
                Symbol = stockDto.Symbol,
                CompanyName = stockDto.CompanyName,
                Purchase = stockDto.Purchase,
                LastDiv = stockDto.LastDiv,
                Industry = stockDto.Industry,
                MarketCap = stockDto.MarketCap
            };
        }

        public static Stocks ToStockFromUpdate(this UpdateStockRequestDTO stockDto){
            return new Stocks{
                Symbol = stockDto.Symbol,
                CompanyName = stockDto.CompanyName,
                Purchase = stockDto.Purchase,
                LastDiv = stockDto.LastDiv,
                Industry = stockDto.Industry,
                MarketCap = stockDto.MarketCap
            };
        }

        public static Stocks ToStockFromFMPStock(this FMPStock stock)
        {
            return new Stocks{
                Symbol = stock.symbol,
                CompanyName = stock.companyName,
                Purchase = (decimal)stock.price,
                LastDiv = (decimal)stock.lastDiv,
                Industry = stock.industry,
                MarketCap = stock.mktCap
            };
        }
    }
}