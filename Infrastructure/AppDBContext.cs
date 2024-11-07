using Business;
using Business.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class AppDBContext : DbContext
    {
        public string DBPath { get; }
        public DbSet<DatabaseInfo> DatabaseInfoes { get; set; }
        public DbSet<BackupLog> BackupLogs { get; set; }
        public DbSet<Config> Configs { get; set; }

        public AppDBContext()
        {
  

            var directoryPath = GeneralInfo.AppDataPath;
            if (!Directory.Exists(GeneralInfo.AppDataPath))
                Directory.CreateDirectory(directoryPath);
            DBPath = Path.Join(directoryPath, "BackupAppDB.db");
            this.Database.EnsureCreated();
        }

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DBPath}");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            modelBuilder.Entity<DatabaseInfo>(builder =>
            {

                builder.ToTable(nameof(DatabaseInfo));
                builder.HasKey(item => item.ID);
                builder.Property(item => item.ID).HasColumnName(nameof(DatabaseInfo.ID)).ValueGeneratedOnAdd();
                builder.Property(item => item.Name).HasColumnName(nameof(DatabaseInfo.Name));
                builder.Property(item => item.ConnectionString).HasColumnName(nameof(DatabaseInfo.ConnectionString)).HasMaxLength(1024);
                
            });

            modelBuilder.Entity<BackupLog>(builder =>
            {
                builder.ToTable(nameof(BackupLog));
                builder.HasKey(item => item.ID);
                builder.Property(item => item.ID).HasColumnName(nameof(DatabaseInfo.ID)).ValueGeneratedOnAdd();
                builder.Property(item => item.DatabaseInfoID).HasColumnName(nameof(BackupLog.DatabaseInfoID));
                builder.Property(item => item.Date).HasColumnName(nameof(BackupLog.Date)).HasDefaultValue(DateTime.Now);
                builder.Property(item => item.IsSuccessful).HasColumnName(nameof(BackupLog.IsSuccessful));
                builder.Property(item => item.Message).HasColumnName(nameof(BackupLog.Message));
                builder.HasOne(item=>item.DatabaseInfo).WithMany(item=>item.BackupLogs).HasForeignKey(item=>item.DatabaseInfoID)
                .HasConstraintName("FK_BackupLog_DatabaseInfo"); 


            });
            modelBuilder.Entity<Config>(builder =>
            {

                builder.ToTable(nameof(Config));
                builder.HasKey(item => item.ID);
                builder.Property(item => item.ID).HasColumnName(nameof(Config.ID)).ValueGeneratedOnAdd();
                builder.Property(item => item.Interval).HasColumnName(nameof(Config.Interval));
                builder.Property(item => item.HostSize).HasColumnName(nameof(Config.HostSize));
                builder.Property(item => item.FTPEncodedUrl).HasColumnName(nameof(Config.FTPEncodedUrl));
                builder.Property(item => item.FTPEncodedUsername).HasColumnName(nameof(Config.FTPEncodedUsername));
                builder.Property(item => item.FTPEncodedPassword).HasColumnName(nameof(Config.FTPEncodedPassword));


            });

        }

    }
}
