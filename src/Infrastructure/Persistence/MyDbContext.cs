using Domain.Models;
using Domain.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sales>().HasNoKey();
            modelBuilder.Ignore<SalesByGoodsViewModel>();
        }

        public DbSet<Sales> Sales { get; set; }
        public DbSet<SalesByGoodsViewModel> SalesByGoodsViewModel { get; set; }

        public async Task<List<Sales>> ExecuteSqlQueryAsync(string sql)
        {
            return await Sales.FromSqlRaw(sql).ToListAsync();
        }
    }
}