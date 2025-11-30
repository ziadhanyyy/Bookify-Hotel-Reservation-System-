using Bookify.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Data
{
    public class BookifyDbContextFactory : IDesignTimeDbContextFactory<BookifyDbContext>
    {
        public BookifyDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BookifyDbContext>();
            optionsBuilder.UseSqlServer("Server=DESKTOP-D9PU7O2;Database=BookifyDB;Trusted_Connection=True;TrustServerCertificate=True;");

            return new BookifyDbContext(optionsBuilder.Options);
        }
    
    
    }
}
