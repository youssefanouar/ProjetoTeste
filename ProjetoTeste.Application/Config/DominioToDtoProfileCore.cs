using ProjetoTeste.Core.Domain;
using ProjetoTeste.Core.Dto;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace ProjetoTeste.Application.Config
{
    public class DominioToDtoProfileCore : AutoMapper.Profile
    {

		public override string ProfileName
		{
			get { return "DominioToDtoProfileCore"; }
		}


        protected override void Configure()
        {

            AutoMapper.Mapper.CreateMap<Pessoa, PessoaDto>().ReverseMap();
            AutoMapper.Mapper.CreateMap<Pessoa, PessoaDtoSpecialized>().ReverseMap();
            AutoMapper.Mapper.CreateMap<Pessoa, PessoaDtoSpecializedResult>().ReverseMap();
            AutoMapper.Mapper.CreateMap<Pessoa, PessoaDtoSpecializedReport>().ReverseMap();
            AutoMapper.Mapper.CreateMap<Pessoa, PessoaDtoSpecializedDetails>().ReverseMap();



        }
    }
}
