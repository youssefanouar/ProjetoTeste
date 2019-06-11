using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;

namespace ProjetoTeste.Core.Domain
{	
	public partial class PessoaValidation
	{
        [Required(ErrorMessage="Pessoa - Campo Nome é Obrigatório")]
        [MaxLength(50, ErrorMessage = "Pessoa - Quantidade de caracteres maior que o permitido para o campo Nome")]
        public virtual object Nome {get; set;}

        [Required(ErrorMessage="Pessoa - Campo DataNascimento é Obrigatório")]
        public virtual object DataNascimento {get; set;}

        [Required(ErrorMessage="Pessoa - Campo Sexo é Obrigatório")]
        public virtual object Sexo {get; set;}

        [Required(ErrorMessage="Pessoa - Campo Ativo é Obrigatório")]
        public virtual object Ativo {get; set;}

        [Required(ErrorMessage="Pessoa - Campo DataCriacao é Obrigatório")]
        public virtual object DataCriacao {get; set;}


	}
}