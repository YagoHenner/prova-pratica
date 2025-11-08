using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class ProdutoConfiguration : IEntityTypeConfiguration<Produto>
{
    public void Configure(EntityTypeBuilder<Produto> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Codigo)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(p => p.Codigo)
            .IsUnique();

        builder.Property(p => p.Nome)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Descricao)
            .HasMaxLength(1000);

        builder.Property(p => p.Preco)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(p => p.Categoria)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Status)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(p => p.UrlFoto)
            .IsRequired(false)
            .HasMaxLength(500);

        builder.Property(p => p.GeradoEm)
            .IsRequired();

        builder.Property(p => p.AtualizadoEm)
            .IsRequired();
    }
}