using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public static class DbContextExtensions
    {
        public static async Task<List<T>> ExecuteSqlQueryAsync<T>(this DbContext context, string sql, Func<DbDataReader, T> map, params object[] parameters)
        {
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = sql;

                if (parameters != null)
                {
                    foreach (var param in parameters)
                    {
                        command.Parameters.Add(param);
                    }
                }

                await context.Database.OpenConnectionAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    var results = new List<T>();

                    while (await reader.ReadAsync())
                    {
                        results.Add(map(reader));
                    }

                    return results;
                }
            }
        }
    }
}