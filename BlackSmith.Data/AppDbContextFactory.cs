using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BlackSmith.Data;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args = null!)
    {
        var builder = new DbContextOptionsBuilder<AppDbContext>();

        builder.UseSqlite("Data Source=E:\\Projects\\DotNet\\BlackSmith\\BlackSmith.Data\\BlackSmithDatabase.db");

        return new AppDbContext(builder.Options);
    }
}