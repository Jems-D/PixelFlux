using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stocks;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Azure;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace api.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDBContext _context;

        public StockRepository(ApplicationDBContext dBContext)
        {
            _context = dBContext;
        }

        public async Task<Stocks> CreateStockAsync(Stocks stockModel)
        {
            //await _context.Stock.AddAsync(stockModel);
            //await _context.SaveChangesAsync();
            var stock = await _context.AddOneStockAsync(stockModel);
            if(stock.isSuccess == true && stock.Payload > 0){
                stockModel.Id = stock.Payload;
            }

            return stockModel;
        }

        public async Task<Stocks?> DeleteStockAsync(int id)
        {
            var stockModel = await _context.Stock.FirstOrDefaultAsync(x => x.Id == id);

            if(stockModel == null){
                return null;
            }

            _context.Stock.Remove(stockModel);
            await _context.SaveChangesAsync();

            return stockModel;

        }

        public async Task<APIResult<bool>> DoesStockExist(int id)
        {
             //await _context.Stock.AnyAsync(s => s.Id == id);   - ef core counterpart
            return await _context.DoesAStockExist(id);
        }

        public async Task<APIResult<List<Stocks>>> GetAllAsync(QueryObject query)
        {
            //for normal ef without query or filter
            //return await _context.Stock.Include(c=> c.Comments).ToListAsync();
           /*     var stocks =  _context.Stock.Include(c => c.Comments).ThenInclude(a => a.AppUser).AsQueryable();
            if(!string.IsNullOrWhiteSpace(query.CompanyName)){
                stocks = stocks.Where(c => c.CompanyName.Contains(query.CompanyName));
            }

            if(!string.IsNullOrWhiteSpace(query.Symbol)){
                stocks = stocks.Where(c => c.Symbol.Contains(query.Symbol));
            } 
            
            if(!string.IsNullOrWhiteSpace(query.SortBy)){
                if(query.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase)){
                    stocks = query.isDescending ? stocks.OrderByDescending(s => s.Symbol) : stocks.OrderBy(s => s.Symbol);
                }
            }   */
            var stocks = await _context.GetAllStocksAsync(query);
           //var skipNumber = (query.PageNumber - 1) * query.PageSize;

            //return await stocks.Skip(skipNumber).Take(query.PageSize).ToListAsync();
            return stocks;
        }

        public async Task<Stocks?> GetBySymbol(string Symbol)
        {
            return await _context.Stock.FirstOrDefaultAsync(s => s.Symbol == Symbol);
        }

        public async Task<APIResult<Stocks?>> GetOneByIdAsync(int id)
        {
            //var stock = await _context.Stock.Include(c=> c.Comments).FirstOrDefaultAsync(i => i.Id == id);
            var stock = await _context.GetOneStockAsync(id);
    
            return new APIResult<Stocks?>{
                statusCode = stock.statusCode,
                isSuccess = stock.isSuccess,
                Message = stock.Message,
                Payload = stock.Payload
            }; 
            //return stock;
        }

        public async Task<APIResult<Stocks?>> UpdateStockAsync(int id, UpdateStockRequestDTO stockRequestDTO)
        {
            //var existingStock = await _context.Stock.FirstOrDefaultAsync(x => x.Id == id);
            var existingStock = await _context.UpdateStockAsync(id, stockRequestDTO.ToStockFromUpdate());
            if(existingStock == null){
                return  null;
            }

            //Map manually,
/*             existingStock.Symbol = stockRequestDTO.Symbol;
            existingStock.CompanyName = stockRequestDTO.CompanyName;
            existingStock.Purchase = stockRequestDTO.Purchase;
            existingStock.LastDiv = stockRequestDTO.LastDiv;
            existingStock.Industry = stockRequestDTO.Industry;
            existingStock.MarketCap = stockRequestDTO.MarketCap; */

            //Map automatically
            //_context.Entry(existingStock).CurrentValues.SetValues(stockRequestDTO);

            //await _context.SaveChangesAsync();

            
            return new APIResult<Stocks?> {
                statusCode = existingStock.statusCode,
                Message = existingStock.Message,
                isSuccess = existingStock.isSuccess,
                Payload = existingStock.Payload
            };
        }


    }
}