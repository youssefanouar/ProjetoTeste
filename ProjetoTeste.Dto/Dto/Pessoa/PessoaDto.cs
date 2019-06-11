using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using Common.Dto;

namespace ProjetoTeste.Core.Dto
{
	public class PessoaDto  : DtoBase
	{
	
        public int PessoaId { get; set;} 
        public string Nome { get; set;} 
        public DateTime DataNascimento { get; set;} 
        public int Sexo { get; set;} 
        public bool Ativo { get; set;} 
        public DateTime DataCriacao { get; set;} 

		
	}
}