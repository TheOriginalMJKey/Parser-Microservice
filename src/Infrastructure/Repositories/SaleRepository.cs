using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Models;
using Application.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly MyDbContext _context;

        public SaleRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<List<Sales>> GetSalesByDate(DateTime startDate, DateTime endDate, string? customerName,
            string? goodsName)
        {
            // Build the SQL query
            string sqlQuery = "SELECT DISTINCT sales.*, sales.goods_id as GoodsId, sales.customers_id as CustomersId, sales.sales_date as SalesDate " +
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

            return result;
        }
    }
}