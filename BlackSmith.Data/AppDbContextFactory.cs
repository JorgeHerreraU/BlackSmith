using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackSmith.Data;
public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args = null!)
    {
        var builder = new DbContextOptionsBuilder<AppDbContext>();
        builder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=BlackSmith;Trusted_Connection=True;MultipleActiveResultSets=true");

        return new AppDbContext(builder.Options);
    }

    internal object Set<T>()
    {
        throw new NotImplementedException();
    }
}
