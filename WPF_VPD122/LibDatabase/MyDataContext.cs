using LibDatabase.Entities;
using LibDatabase.Helpers;
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
            this.Database.Migrate();
        }
        public DbSet<UserEntity> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var dbConnection = MyAppConfig.GetSectionValue("ConnectionDB");

            optionsBuilder.UseNpgsql(dbConnection);
            //optionsBuilder.UseNpgsql("Server=localhost;Port=5432;Database=wpfdata;User Id=postgres;Password=S97532b3;");
        }
    }
}
