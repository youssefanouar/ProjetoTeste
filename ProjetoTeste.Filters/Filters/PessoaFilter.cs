using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoTeste.Core.Filters
{
    public partial class PessoaFilter : Filter
    {

        public int PessoaId { get; set;} 
        public string Nome { get; set;} 
        public DateTime DataNascimentoStart { get; set;} 
        public DateTime DataNascimentoEnd { get; set;} 
        public DateTime DataNascimento { get; set;} 
        public int Sexo { get; set;} 
        public bool? Ativo { get; set;} 
        public DateTime DataCriacaoStart { get; set;} 
        public DateTime DataCriacaoEnd { get; set;} 
        public DateTime DataCriacao { get; set;} 


    }
}
