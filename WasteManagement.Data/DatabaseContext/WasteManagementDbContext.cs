using Microsoft.EntityFrameworkCore;
using WasteManagement.Models.Interfaces;
using WasteManagement.Models.Models;
using System.Configuration;

namespace WasteManagement.Data.DatabaseContext
{
    public class WasteManagementDbContext : DbContext, IWasteManagementDbContext
    {
        public DbSet<WasteContainerDtoModel> Containers { get; set; }

        public WasteManagementDbContext(){}

        public WasteManagementDbContext(DbContextOptions<WasteManagementDbContext> options)
      : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=db;User=sa;Password=P@ssw0rd123!;");
        }
    }
}