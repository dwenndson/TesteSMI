using System;
using System.Linq;
using IogServices.Models;
using IogServices.Models.DAO;
using IogServices.Models.DAO.Generic;
using Microsoft.EntityFrameworkCore;

namespace IogServices.Repositories
{
    public class IogContext : DbContext
    {
        public IogContext(DbContextOptions<IogContext> options) : base(options) {}
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<SmcModel> SmcModels { get; set; }
        public DbSet<MeterModel> MeterModels { get; set; }
        public DbSet<RateType> RateTypes { get; set; }
        public DbSet<Meter> Meters { get; set; }
        public DbSet<MeterEnergy> MeterEnergies { get; set; }
        public DbSet<Smc> Smcs { get; set; }
        public DbSet<Modem> Modems { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<MeterKeys> MeterKeys { get; set; }
        public DbSet<CommandTicket> CommandTickets { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<DeviceLog> DeviceLogs { get; set; }
        public DbSet<SmcAlarm> SmcAlarms { get; set; }
        public DbSet<MeterAlarm> MeterAlarms { get; set; }
        public DbSet<SmcUnregistered> SmcUnregistereds { get; set; }
        public DbSet<MeterUnregistered> MeterUnregistereds { get; set; }

        
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Manufacturer>()
                .HasAlternateKey(manufacturer => new {manufacturer.Name});

            modelBuilder.Entity<SmcModel>()
                .HasAlternateKey(smcModel => new {smcModel.Name});
            
            modelBuilder.Entity<MeterModel>()
                .HasAlternateKey(meterModel => new {meterModel.Name});
            
            modelBuilder.Entity<RateType>()
                .HasAlternateKey(rateType => new {rateType.Name});

            modelBuilder.Entity<Meter>()
                .HasAlternateKey(meter => new {meter.Serial});

            modelBuilder.Entity<MeterEnergy>()
                .HasAlternateKey(meterEnergy => new {meterEnergy.Id});

            modelBuilder.Entity<Smc>()
                .HasAlternateKey(smc => new {smc.Serial});

            modelBuilder.Entity<Modem>()
                .HasAlternateKey(modem => new {Eui = modem.DeviceEui});

            modelBuilder.Entity<User>()
                .HasAlternateKey(user => new {user.Email});
            
            modelBuilder.Entity<MeterKeys>()
                .HasAlternateKey(key => new {key.Serial});
            
        }
        
        public override int SaveChanges()
        {
            AddTimestamps();
            return base.SaveChanges();
        }
        
        private void AddTimestamps()
        {
            var entities = ChangeTracker.Entries()
                .Where(x => x.Entity is Base && (x.State == EntityState.Added || x.State == EntityState.Modified));
            
            foreach (var entity in entities)
            {
                var now = DateTime.Now;
                
                if (entity.State == EntityState.Added)
                {
                    ((Base)entity.Entity).CreatedAt = now;
                }
                ((Base)entity.Entity).UpdatedAt = now;
            }
        }
    }
}