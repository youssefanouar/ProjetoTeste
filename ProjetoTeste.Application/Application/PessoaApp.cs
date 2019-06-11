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
using Common.Domain;
using System.Transactions;

namespace ProjetoTeste.Core.Application
{
    public partial class PessoaApp : IDisposable
    {
        private IRepository<Pessoa> repPessoa;
        private ICache cache;
        private Pessoa Pessoa;
		public ValidationHelper ValidationHelper;

        public PessoaApp(string token)
        {
			this.Init(token);
			this.ValidationHelper = Pessoa.ValidationHelper;
        }

		public void GetWarnings(PessoaFilter filters)
        {
            this.Pessoa.Warnings(filters);
        }
		
		public IEnumerable<PessoaDto> GetAll()
        {
			var result = default(IEnumerable<PessoaDto>);
			using (var transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
            {
				var dataList = this.Pessoa.GetAll();
				result =  AutoMapper.Mapper.Map<IQueryable<Pessoa>, IEnumerable<PessoaDto>>(dataList);
			}
			return result;
        }

		public SearchResult<PessoaDto> GetByFilters(PessoaFilter filter)
        {
			var result = default(SearchResult<PessoaDto>);
			using (var transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
            {
				result = GetByFiltersWithCache(filter, MapperDomainToResult);
			}
			return result;
        }

		public SearchResult<PessoaDto> GetReport(PessoaFilter filter)
        {	
			var result = default(SearchResult<PessoaDto>);
			using (var transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
            {
				result = GetByFiltersWithCache(filter, MapperDomainToReport);
			}
			return result;
        }
		
		public SearchResult<dynamic> GetDataListCustom(PessoaFilter filters)
        {
			var result = default(SearchResult<dynamic>);
			using (var transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
            {
				var queryBase = this.Pessoa.GetDataListCustom(filters);
				var dataList = PagingAndSorting<dynamic>(filters, queryBase.AsQueryable(), PaginationDynamic);
				result = new SearchResult<dynamic>
				{
					DataList = dataList.ResultPaginatedData.ToList(),
					Summary = this.Pessoa.GetSummaryDataListCustom(queryBase)
				};
			}
			return result;
        }

		
		public dynamic GetDataCustom(PessoaFilter filters)
        {
			var result = default(dynamic);
			using (var transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
            {
				result = this.Pessoa.GetDataCustom(filters);
			}
			return result;
        }

		public PessoaDto Get(PessoaDto dto)
        {
			var result = default(PessoaDto);
			using (var transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
            {
				var model =  AutoMapper.Mapper.Map<PessoaDto, Pessoa>(dto);
				var data = this.Pessoa.Get(model);
				result =  this.MapperDomainToDtoSpecialized(data);
			}
			return result;
        }

		public PessoaDto GetDetails(PessoaDto dto)
        {
			var result = default(PessoaDto);
			using (var transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))            
			{
				var model =  AutoMapper.Mapper.Map<PessoaDto, Pessoa>(dto);
				var data = this.Pessoa.Get(model);
				result =  this.MapperDomainToDtoDetails(data, dto);
			}

			return result;
        }

		public IEnumerable<DataItem> GetDataItem(IFilter filters)
		{
			var result = default(IEnumerable<DataItem>);
			using (var transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
            {
				var filter = (filters as PessoaFilter).IsNotNull() ? filters as PessoaFilter : new PessoaFilter { };
				var filterKey = filter.CompositeKey(); 

				if (filter.ByCache)
				{
					if (this.cache.ExistsKey<IEnumerable<DataItemCache>>(filterKey))
					{
						var resultCache = this.cache.GetAndCast<IEnumerable<DataItemCache>>(filterKey);
						return resultCache.Select(_ => new DataItem
						{
							Id = _.Id,
							Name = _.Name
						});
					}
				}

				result = this.Pessoa.GetDataItem(filter);
				if (filter.ByCache)
				{
					this.cache.Add(filterKey, result.Select(_ => new DataItemCache
					{
						Id = _.Id,
						Name = _.Name
					}).ToList());
					this.AddTagCache(filterKey);
				}
			}
			return result;
		}

		public int GetTotalByFilters(PessoaFilter filter)
        {
			var result = default(int);
			using (var transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
            {
				result = this.Pessoa.GetByFilters(filter).Count();
			}
			return result;
        }

		public PessoaDto Save(PessoaDto dto)
        {
			var result = default(PessoaDto);
			using (var transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
            {
				var model =  AutoMapper.Mapper.Map<PessoaDto, Pessoa>(dto);
				var data = this.Pessoa.Save(model);
				result =  AutoMapper.Mapper.Map<Pessoa, PessoaDto>(data);
				transaction.Complete();
			}
			return result;
        }

		public PessoaDto Save(PessoaDtoSpecialized dto)
        {
			var result = default(PessoaDto);
			using (var transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
            {
				var model =  AutoMapper.Mapper.Map<PessoaDtoSpecialized, Pessoa>(dto);
				var data = this.Pessoa.Save(model);
				result =  AutoMapper.Mapper.Map<Pessoa, PessoaDto>(data);
				transaction.Complete();
			}
			return result;
        }

		public PessoaDto SavePartial(PessoaDtoSpecialized dto)
        {
			var result = default(PessoaDto);
			using (var transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))            
			{
				var model =  AutoMapper.Mapper.Map<PessoaDtoSpecialized, Pessoa>(dto);
				var data = this.Pessoa.SavePartial(model);
				result =  AutoMapper.Mapper.Map<Pessoa, PessoaDto>(data);
				transaction.Complete();
			}
			return result;
        }

		public IEnumerable<PessoaDto> Save(IEnumerable<PessoaDtoSpecialized> dtos)
        {
			var result = default(IEnumerable<PessoaDto>);
			using (var transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
            {
				var models = AutoMapper.Mapper.Map<IEnumerable<PessoaDtoSpecialized>, IEnumerable<Pessoa>>(dtos);
				var data = this.Pessoa.Save(models);
				result = AutoMapper.Mapper.Map<IEnumerable<Pessoa>, IEnumerable<PessoaDto>>(data);
				transaction.Complete();
			}
			return result;
        }

		public void Delete(PessoaDto dto)
        {
			using (var transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
            {
				var model =  AutoMapper.Mapper.Map<PessoaDto, Pessoa>(dto);
				this.Pessoa.Delete(model);
				transaction.Complete();
			}
        }
				
		private Common.Models.Summary Summary(IQueryable<Pessoa> queryBase, PaginateResult<Pessoa> dataList)
        {
			var domainSummary = this.Pessoa.GetSummary(queryBase);
			var summary = new Common.Models.Summary
			{
				Total = dataList.TotalCount,
				AdditionalSummary = domainSummary.AdditionalSummary.IsNotNull() ? domainSummary.AdditionalSummary : null
			};
			return summary;
        }

		private PaginateResult<T> PagingAndSorting<T>(PessoaFilter filter, IQueryable<T> queryBase, Func<PessoaFilter, IQueryable<T>, PaginateResult<T>, PaginateResult<T>> PaginationDefault) where T: class
        {
           var querySorting = queryBase;

            if (filter.IsOrderByDynamic)
                querySorting = queryBase.OrderByDynamic(filter);

            var dataList = new PaginateResult<T>
            {
                ResultPaginatedData = querySorting,
                TotalCount = 0
            };

            if (filter.IsPagination)
            {
                if (filter.IsOrderByDynamic || filter.IsOrderByDomain)
                    return dataList = querySorting.PaginateNew(filter);

                dataList = PaginationDefault(filter, querySorting, dataList);
            }

			return dataList;
        }

		private PaginateResult<Pessoa> PaginationDefault(PessoaFilter filter, IQueryable<Pessoa> querySorting, PaginateResult<Pessoa> dataList)
        {
            dataList = querySorting.OrderByDescending(_ => _.PessoaId).PaginateNew(filter);
            return dataList;
        }

        private PaginateResult<dynamic> PaginationDynamic(PessoaFilter filter, IQueryable<dynamic> querySorting, PaginateResult<dynamic> dataList)
        {
            if (filter.OrderFields.IsNull())
            {
                filter.OrderFields = new[] { "CustomFieldOrder" };
                filter.orderByType = Common.Enum.OrderByType.OrderByDescending;
            }
            dataList = querySorting.OrderByDynamic(filter).PaginateNew(filter);
            return dataList;
        }

		private SearchResult<PessoaDto> GetByFiltersWithCache(PessoaFilter filter, Func<PessoaFilter, PaginateResult<Pessoa>, IEnumerable<PessoaDto>> MapperDomainToDto)
        {
            var filterKey = filter.CompositeKey();
            if (filter.ByCache)
                if (this.cache.ExistsKey<SearchResult<PessoaDto>>(filterKey))
                    return this.cache.GetAndCast<SearchResult<PessoaDto>>(filterKey);

            var queryBase = this.Pessoa.GetByFilters(filter);
            var dataList = PagingAndSorting(filter, queryBase,PaginationDefault);
            var result = MapperDomainToDto(filter, dataList);
            var summary = Summary(queryBase, dataList);

            var searchResult = new SearchResult<PessoaDto>
            {
                DataList = result,
                Summary = summary,
            };

            if (filter.ByCache)
			{
                this.cache.Add(filterKey, searchResult);
				this.AddTagCache(filterKey);
			}

            return searchResult;
        }
		
        private void AddTagCache(string filterKey)
        {
            var tags = this.cache.GetAndCast<List<string>>("Pessoa") as List<string>;
            if (tags.IsNull()) tags = new List<string>();
            tags.Add(filterKey);
            this.cache.Add("Pessoa", tags);
        }
		
		private PessoaDto MapperDomainToDtoSpecialized(Pessoa data)
        {
            var result =  AutoMapper.Mapper.Map<Pessoa, PessoaDtoSpecialized>(data);
            return result;
        }

        public void Dispose()
        {
            this.Pessoa.Dispose();
        }
    }
}
