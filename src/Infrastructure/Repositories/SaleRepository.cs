using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Npgsql;
using Domain.Models;
using Domain.ViewModels;
using Infrastructure.Caching;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using NpgsqlTypes;

namespace Infrastructure.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly MyDbContext _context;
        private readonly RedisCacheService _cacheService;

        public SaleRepository(MyDbContext context, IDistributedCache cache)
        {
            _context = context;
            _cacheService = new RedisCacheService(cache);
        }

        public async Task<List<Sales>> GetSalesByDate(DateTime startDate, DateTime endDate, string? customerName,
            string? goodsName)
        {
            string cacheKey = CreateCacheKey(startDate, endDate, customerName, goodsName);
            var cachedResult = await _cacheService.GetAsync<List<Sales>>(cacheKey);

            if (cachedResult != null)
            {
                return cachedResult;
            }

            // Build the SQL query
            string sqlQuery =
                "SELECT DISTINCT sales.*, sales.goods_id as GoodsId, sales.customers_id as CustomersId, sales.sales_date as SalesDate " +
                "FROM sales " +
                "JOIN goods ON sales.goods_id = goods.goods_id " +
                "JOIN customers ON sales.customers_id = customers.customers_id " +
                "WHERE sales.sales_date >= {0} " +
                "  AND sales.sales_date <= {1}";

            // Add conditions for customerName and goodsName if they are not null or empty
            if (!string.IsNullOrEmpty(customerName))
            {
                sqlQuery += " AND customers.customers_name = {2}";
            }

            if (!string.IsNullOrEmpty(goodsName))
            {
                sqlQuery += " AND goods.goods_name = {3}";
            }

            // Execute the SQL query
            var result = await _context.Sales
                .FromSqlRaw(
                    sqlQuery,
                    startDate, endDate,
                    string.IsNullOrEmpty(customerName) ? null : customerName,
                    string.IsNullOrEmpty(goodsName) ? null : goodsName)
                .ToListAsync();

            // Cache the results
            await _cacheService.SetAsync(cacheKey, result, TimeSpan.FromMinutes(10));

            return result;
        }

        public async Task<List<SalesByGoodsViewModel>> GetSalesByGoods(DateTime startDate, DateTime endDate,
            string goodsName)
        {
            string cacheKey = CreateCacheKey(startDate, endDate, goodsName);
            var cachedResult = await _cacheService.GetAsync<List<SalesByGoodsViewModel>>(cacheKey);

            if (cachedResult != null)
            {
                return cachedResult;
            }

            string sqlQuery = "SELECT goods.goods_name, SUM(sales.quantity) as quantity " +
                              "FROM sales " +
                              "JOIN goods ON sales.goods_id = goods.goods_id " +
                              "WHERE sales.sales_date >= @StartDate " +
                              "  AND sales.sales_date <= @EndDate " +
                              "  AND goods.goods_name = @GoodsName " +
                              "GROUP BY goods.goods_name";

            var parameters = new[]
            {
                new NpgsqlParameter("@StartDate", NpgsqlDbType.Date) { Value = startDate },
                new NpgsqlParameter("@EndDate", NpgsqlDbType.Date) { Value = endDate },
                new NpgsqlParameter("@GoodsName", NpgsqlDbType.Text) { Value = goodsName }
            };

            var result = await _context.ExecuteSqlQueryAsync<SalesByGoodsViewModel>(
                sqlQuery,
                reader => new SalesByGoodsViewModel
                {
                    GoodsName = reader["goods_name"].ToString(),
                    Quantity = int.Parse(reader["quantity"].ToString())
                },
                parameters);

            // Cache the results
            await _cacheService.SetAsync(cacheKey, result, TimeSpan.FromMinutes(10));

            return result;
        }

        public async Task<List<SalesByClientsViewModel>> GetSalesByClients(DateTime startDate, DateTime endDate, string clientName)
        {
            string cacheKey = CreateCacheKey(startDate, endDate, clientName);
            var cachedResult = await _cacheService.GetAsync<List<SalesByClientsViewModel>>(cacheKey);

            if (cachedResult != null)
            {
                return cachedResult;
            }

            string sqlQuery = "SELECT customers.customers_name, SUM(sales.quantity * prices.price_value) as total " +
                              "FROM sales " +
                              "JOIN goods ON sales.goods_id = goods.goods_id " +
                              "JOIN customers ON sales.customers_id = customers.customers_id " +
                              "JOIN prices ON goods.goods_id = prices.goods_id " +
                              "WHERE sales.sales_date >= @StartDate " +
                              "  AND sales.sales_date <= @EndDate " +
                              "  AND customers.customers_name = @ClientName " +
                              "GROUP BY customers.customers_name";

            var parameters = new[]
            {
                new NpgsqlParameter("@StartDate", NpgsqlDbType.Date) { Value = startDate },
                new NpgsqlParameter("@EndDate", NpgsqlDbType.Date) { Value = endDate },
                new NpgsqlParameter("@ClientName", NpgsqlDbType.Text) { Value = clientName }
            };

            var result = await _context.ExecuteSqlQueryAsync<SalesByClientsViewModel>(
                sqlQuery,
                reader => new SalesByClientsViewModel
                {
                    ClientName = reader["customers_name"].ToString(),
                    Total = decimal.Parse(reader["total"].ToString())
                },
                parameters);

            // Cache the results
            await _cacheService.SetAsync(cacheKey, result, TimeSpan.FromMinutes(10));

            return result;
        }
        public async Task CreateOrder(PostOrderModel order)
        {
            string sqlQuery = "INSERT INTO sales (sales_date, customers_id, goods_id, quantity) " +
                              "VALUES (@SalesDate, @CustomersId, @GoodsId, @Quantity)";

            var parameters = new[]
            {
                new NpgsqlParameter("@SalesDate", NpgsqlDbType.Date) { Value = DateTime.UtcNow.Date },
                new NpgsqlParameter("@CustomersId", NpgsqlDbType.Integer) { Value = order.CustomersId },
                new NpgsqlParameter("@GoodsId", NpgsqlDbType.Integer) { Value = order.GoodsId },
                new NpgsqlParameter("@Quantity", NpgsqlDbType.Integer) { Value = order.Quantity }
            };

            await _context.Database.ExecuteSqlRawAsync(sqlQuery, parameters);
        }
        public async Task DeleteOrder(DeleteOrderModel order)
        {
            string sqlQuery = "DELETE FROM sales " +
                              "WHERE sales_date = @SalesDate " +
                              "AND customers_id = @CustomersId " +
                              "AND goods_id = @GoodsId";

            var parameters = new[]
            {
                new NpgsqlParameter("@SalesDate", NpgsqlDbType.Date) { Value = order.SalesDate },
                new NpgsqlParameter("@CustomersId", NpgsqlDbType.Integer) { Value = order.CustomersId },
                new NpgsqlParameter("@GoodsId", NpgsqlDbType.Integer) { Value = order.GoodsId }
            };

            await _context.Database.ExecuteSqlRawAsync(sqlQuery, parameters);
        }
        public async Task<List<Sales>> GetSalesByMonth(int year, int month)
        {
            string cacheKey = CreateCacheKeyForSalesByMonth(year, month);
            var cachedResult = await _cacheService.GetAsync<List<Sales>>(cacheKey);

            if (cachedResult != null)
            {
                return cachedResult;
            }

            string sqlQuery = "SELECT * FROM sales " +
                              "WHERE EXTRACT(MONTH FROM sales_date) = @Month " +
                              "AND EXTRACT(YEAR FROM sales_date) = @Year";

            var parameters = new[]
            {
                new NpgsqlParameter("@Month", NpgsqlDbType.Integer) { Value = month },
                new NpgsqlParameter("@Year", NpgsqlDbType.Integer) { Value = year }
            };

            var result = await _context.ExecuteSqlQueryAsync<Sales>(
                sqlQuery,
                reader => new Sales()
                {
                    SalesDate = reader["sales_date"].ToString(),
                    GoodsId = int.Parse(reader["goods_id"].ToString()),
                    CustomersId = int.Parse(reader["customers_id"].ToString()),
                    Quantity = int.Parse(reader["quantity"].ToString())
                },
                parameters);

            // Cache the results
            await _cacheService.SetAsync(cacheKey, result, TimeSpan.FromMinutes(10));

            return result;
            
        }
        private string CreateCacheKeyForSalesByMonth(int year, int month)
        {
            return $"SalesByMonth_{year}_{month}";
        }


        private string CreateCacheKey(DateTime startDate, DateTime endDate, string? name1, string? name2 = null)
        {
            return $"{startDate:yyyy-MM-dd}_{endDate:yyyy-MM-dd}_{name1}_{name2 ?? ""}";
        }
    }
}
