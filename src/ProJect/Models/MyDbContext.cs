using Microsoft.EntityFrameworkCore;
using ProJect.Models;

public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<sales>().HasNoKey();
    }
    public DbSet<sales> Sales { get; set; }
    public async Task<List<sales>> ExecuteSqlQueryAsync(string sql)
    {
        return await Sales.FromSqlRaw(sql).ToListAsync();
    }


}