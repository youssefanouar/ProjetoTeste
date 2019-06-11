using ProjetoTeste.Core.Domain;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ProjetoTeste.Core.Infrastructure.ORM.Maps
{
    public partial class PessoaMap : EntityTypeConfiguration<Pessoa>
    {
        public PessoaMap()
        {
            this.ToTable("Pessoa");
            this.Property(t => t.PessoaId).HasColumnName("Id");
           

            this.Property(t => t.Nome).HasColumnName("Nome").HasColumnType("varchar").HasMaxLength(50);
            this.Property(t => t.DataNascimento).HasColumnName("DataNascimento");
            this.Property(t => t.Sexo).HasColumnName("Sexo");
            this.Property(t => t.Ativo).HasColumnName("Ativo");
            this.Property(t => t.DataCriacao).HasColumnName("DataCriacao");


            this.HasKey(d => new { d.PessoaId, }); 

			this.CustomConfig();

        }
    }
}