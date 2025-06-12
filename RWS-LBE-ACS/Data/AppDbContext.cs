using Microsoft.EntityFrameworkCore;
using RWS_LBE_ACS.Models;

namespace RWS_LBE_ACS.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }
         
        public DbSet<SysChannel> SysChannel { get; set; } 
    }
}
