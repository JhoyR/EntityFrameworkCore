using Microsoft.EntityFrameworkCore;
using CursoEFCore.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CursoEFCore.Data.Configurations
{
    public class PedidoConfiguration : IEntityTypeConfiguration<Pedido>
    {
        public void Configure(EntityTypeBuilder<Pedido> builder) //Padrão
        {
            builder.ToTable("Pedidos");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.IniciadoEm).HasDefaultValueSql("GETDATE()").ValueGeneratedOnAdd();
            builder.Property(p => p.Status).HasConversion<string>();
            builder.Property(p => p.TipoFrete).HasConversion<int>();
            builder.Property(p => p.Observacao).HasColumnType("VARCHAR(512)");

            builder.HasMany(p => p.Items) //Configura o relacionamento "Muitos para um" ou N,1
                .WithOne(p => p.Pedido)
                .OnDelete(DeleteBehavior.Cascade); //Permite configurar o tipo de restrição para o relacionamento (Quando deletar um pedido, seus items serão deletados automaticamente)
        }
    } 
}