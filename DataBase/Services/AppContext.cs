using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using DataBase.Models;
using Microsoft.Data.Sqlite;

namespace DataBase.Services
{
    public class AppContext : DbContext, IDatabaseContext
    {
        public const string DEFAULTDBFILE = "Almutal.db3";
        private readonly string _dbPath = DEFAULTDBFILE;
        private SqliteConnection connection;

        #region Tables
        public DbSet<Process> Processes { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Details> Details { get; set; }
        public DbSet<ClientService> ClientServices { get; set; }
        #endregion
        public AppContext()
        {

            SQLitePCL.Batteries_V2.Init();

            this.Database.EnsureCreated();
        }

        public AppContext(SqliteConnection sqliteConnection)
        {
            if (!string.IsNullOrEmpty(sqliteConnection?.DataSource)) _dbPath = sqliteConnection.DataSource;
            //SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlcipher());
            connection = sqliteConnection;
            SQLitePCL.Batteries_V2.Init();

            this.Database.EnsureCreated();

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppContext).Assembly);

            modelBuilder.Entity<ClientService>()
                .HasKey(x => new { x.ClientId, x.ServiceId });

            // The assembly where we have our configurations

            if (Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
            {
                foreach (var entityType in modelBuilder.Model.GetEntityTypes())
                {
                    var properties = entityType.ClrType.GetProperties().Where(p => p.PropertyType == typeof(decimal));

                    var dateTimeProperties = entityType.ClrType.GetProperties().Where(p => p.PropertyType == typeof(DateTimeOffset) || p.PropertyType == typeof(DateTimeOffset?));

                    foreach (var property in properties)
                    {
                        modelBuilder.Entity(entityType.Name).Property(property.Name).HasConversion<double>();
                    }

                    foreach (var property in dateTimeProperties)
                    {
                        modelBuilder.Entity(entityType.Name).Property(property.Name).HasConversion(new DateTimeToBinaryConverter());
                    }
                }
            }

        }

        private SqliteConnection InitializeSQLiteConnection()
        {
            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), _dbPath);
            var connectionString = new SqliteConnectionStringBuilder
            {
                Mode = SqliteOpenMode.ReadWriteCreate,
                DataSource = dbPath,
            };
            return new SqliteConnection(connectionString.ToString());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            connection ??= InitializeSQLiteConnection();
            connection.Open();
            optionsBuilder.UseSqlite(connection);

        }
    }
}
