using LibDatabase.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibDatabase
{
    public class MyDataContext : DbContext
    {
        public MyDataContext()
        {
            //this.Database.Migrate();
        }
        public DbSet<UserEntity> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appconfig.json");
            var config = configBuilder.Build();
            var dbConnection = config.GetSection("ConnectionDB").Value;

            optionsBuilder.UseNpgsql(dbConnection);
            //optionsBuilder.UseNpgsql("Server=localhost;Port=5432;Database=wpfdata;User Id=postgres;Password=S97532b3;");
        }
    }
}
