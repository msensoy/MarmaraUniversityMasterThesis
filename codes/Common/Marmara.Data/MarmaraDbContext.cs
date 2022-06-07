using Marmara.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace Marmara.Data
{
    public class MarmaraDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Data Source=SQL8003.site4now.net;Initial Catalog=db_a86e66_marmara;User Id=db_a86e66_marmara_admin;Password=Marmara123");
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<MQ2Data> MQ2Datas { get; set; }
        public DbSet<MQ135Data> MQ135Datas { get; set; }
        public DbSet<DHT11Data> DHT11Datas { get; set; }
        public DbSet<LDRData> LDRDatas { get; set; }
        public DbSet<LEDData> LEDDatas { get; set; }
        public DbSet<CFLData> CFLDatas { get; set; }
        public DbSet<AlarmData> AlarmDatas { get; set; }
        public DbSet<FanData> FanDatas { get; set; }
        public DbSet<SchTask> SchTasks { get; set; }
        public DbSet<Rule> Rules { get; set; }
        public DbSet<ControlUser> Users { get; set; }
    }
}
