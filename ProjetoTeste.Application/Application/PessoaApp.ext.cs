using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Domain.Interfaces;
using Common.Infrastructure.Cache;
using System.Collections.Generic;
using Common.Models;
using ProjetoTeste.Core.Filters;
using Common.Interfaces;
using ProjetoTeste.Core.Dto;
using ProjetoTeste.Core.Domain;
using Common.Dto;

namespace ProjetoTeste.Core.Application
{
    public partial class PessoaApp
    {

		public void Init(string token)
        {
			this.cache = ConfigContainer.Container().GetInstance<ICache>();
            this.repPessoa = ConfigContainer.Container().GetInstance<IRepository<Pessoa>>();
            this.Pessoa = new Pessoa(this.repPessoa, cache);
            this.Pessoa.SetToken(token);
		}

		private IEnumerable<PessoaDto> MapperDomainToResult(PessoaFilter filter, PaginateResult<Pessoa> dataList)
        {
            var result = filter.IsOnlySummary ? null : AutoMapper.Mapper.Map<IEnumerable<Pessoa>, IEnumerable<PessoaDtoSpecializedResult>>(dataList.ResultPaginatedData);
            return result;
        }

		private IEnumerable<PessoaDto> MapperDomainToReport(PessoaFilter filter, PaginateResult<Pessoa> dataList)
        {
            var result = filter.IsOnlySummary ? null : AutoMapper.Mapper.Map<IEnumerable<Pessoa>, IEnumerable<PessoaDtoSpecializedReport>>(dataList.ResultPaginatedData);
            return result;
        }

		
		private PessoaDto MapperDomainToDtoDetails(Pessoa data,  Common.Dto.DtoBase dto)
        {
            var result =  AutoMapper.Mapper.Map<Pessoa, PessoaDtoSpecializedDetails>(data);
            return result;
        }

		private DtoBase MapperDomainToDtoSpecialized(Pessoa data,  Common.Dto.DtoBase dto)
        {
            var result =  AutoMapper.Mapper.Map<Pessoa, PessoaDtoSpecialized>(data);
            return result;
        }
	}
}
