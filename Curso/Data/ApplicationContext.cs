using System.Linq;
using CursoEFCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CursoEFCore.Data
{
    public class ApplicationContext : DbContext
    {
        private static readonly ILoggerFactory _logger = LoggerFactory.Create(p => p.AddConsole());

        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLoggerFactory(_logger) //Define o log que deseja utilizar para exibir
                .EnableSensitiveDataLogging() //Permite visualizar o valor de cada parametro no console
                .UseSqlServer("Data source=(localdb)\\mssqllocaldb;Initial Catalog=CursoEFCore;Integrated Security=true",
                p=>p.EnableRetryOnFailure(maxRetryCount: 2, //Define quantas vezes vai tentar conectar 
                maxRetryDelay: 
                TimeSpan.FromSeconds(5),//Define o tempo a aguardar entre cada tentativa
                errorNumbersToAdd: null).MigrationsHistoryTable("curso_ef_core"));
        }

        //Melhor forma
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);
            MapearPropriedadesEsquecidas(modelBuilder);
        }

        // protected override void OnModelCreating(ModelBuilder modelBuilder)
        // {
        //    modelBuilder.ApplyConfiguration(new ClienteConfiguration());
        //    modelBuilder.ApplyConfiguration(new PedidoConfiguration());
        //    modelBuilder.ApplyConfiguration(new PedidoItemConfiguration());
        //    modelBuilder.ApplyConfiguration(new ProdutoConfiguration());
        // }

        private void MapearPropriedadesEsquecidas(ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                var properties = entity.GetProperties().Where(p => p.ClrType == typeof(string));

                foreach (var property in properties)
                {
                    if(string.IsNullOrEmpty(property.GetColumnType()) &&  !property.GetMaxLength().HasValue){
                        property.SetMaxLength(100);
                        property.SetColumnType("VARCHAR(100)");
                    }
                }
            }
        }
    }
}