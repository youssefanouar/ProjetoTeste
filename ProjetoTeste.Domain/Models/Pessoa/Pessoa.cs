using System;
using System.Linq;
using Common.Domain;
using Common.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq.Expressions;
using ProjetoTeste.Core.Filters;
using Common.Models;
using System.Threading.Tasks;

namespace ProjetoTeste.Core.Domain
{
	public partial class Pessoa : DomainBase, IDisposable, IDomainCrud<Pessoa>
	{
        protected IRepository<Pessoa> rep;
		protected ICache cache;
        public Pessoa(IRepository<Pessoa> rep, ICache cache):this()
        {
            this.rep = rep;
            this.cache = cache;
			this.Init();
        }

        public int PessoaId { get; set;} 
        public string Nome { get; set;} 
        public DateTime DataNascimento { get; set;} 
        public int Sexo { get; set;} 
        public bool Ativo { get; set;} 
        public DateTime DataCriacao { get; set;} 

		
		public virtual void Warnings(PessoaFilter filters)
        {
            ValidationHelper.AddDomainWarning<Pessoa>("");
        }

        public IQueryable<Pessoa> GetAll(params Expression<Func<Pessoa, object>>[] includes)
        {
            return this.rep.GetAll(includes);
        }
		
		public Pessoa GetFromContext(Pessoa model)
        {
			return this.rep.Get(model.PessoaId);
        }

		public int Total()
        {
            return this.rep.GetAll().Count();
        }

		protected IQueryable<Pessoa> SimpleFilters(PessoaFilter filters,IQueryable<Pessoa> queryBase)
        {

			var queryFilter = queryBase;

            if (filters.PessoaId.IsSent()) 
			{ 
				
				queryFilter = queryFilter.Where(_=>_.PessoaId == filters.PessoaId);
			};
            if (filters.Nome.IsSent()) 
			{ 
				
				queryFilter = queryFilter.Where(_=>_.Nome.Contains(filters.Nome));
			};
            if (filters.DataNascimentoStart.IsSent()) 
			{ 
				
				queryFilter = queryFilter.Where(_=>_.DataNascimento >= filters.DataNascimentoStart );
			};
            if (filters.DataNascimentoEnd.IsSent()) 
			{ 
				filters.DataNascimentoEnd = filters.DataNascimentoEnd.AddDays(1).AddMilliseconds(-1);
				queryFilter = queryFilter.Where(_=>_.DataNascimento  <= filters.DataNascimentoEnd);
			};

            if (filters.Sexo.IsSent()) 
			{ 
				
				queryFilter = queryFilter.Where(_=>_.Sexo == filters.Sexo);
			};
            if (filters.Ativo.IsSent()) 
			{ 
				
				queryFilter = queryFilter.Where(_=>_.Ativo == filters.Ativo);
			};
            if (filters.DataCriacaoStart.IsSent()) 
			{ 
				
				queryFilter = queryFilter.Where(_=>_.DataCriacao >= filters.DataCriacaoStart );
			};
            if (filters.DataCriacaoEnd.IsSent()) 
			{ 
				filters.DataCriacaoEnd = filters.DataCriacaoEnd.AddDays(1).AddMilliseconds(-1);
				queryFilter = queryFilter.Where(_=>_.DataCriacao  <= filters.DataCriacaoEnd);
			};



            return queryFilter;
        }

		public virtual IEnumerable<dynamic> GetDataListCustom(PessoaFilter filters)
        {
            var result = this.GetByFilters(filters);

            return result.Select(_ => new
            {
				CustomFieldOrder =_.PessoaId,
				PessoaId = _.PessoaId	
            });
        }
		
		public virtual dynamic GetDataCustom(PessoaFilter filters)
        {
            var result = this.GetByFilters(filters);

            return result.Select(_ => new
            {
				PessoaId = _.PessoaId	
            }).SingleOrDefault();
        }

		public virtual Summary GetSummaryDataListCustom(IEnumerable<dynamic> result)
        {
            return new Summary
            {
                Total = result.Count()
            };
        }

		protected virtual bool Continue(Pessoa model, Pessoa modelOld)
        {
            return true;
        }

		protected virtual void ConfigMessageDomainConfirm(Pessoa model)
        {
            ValidationHelper.AddDomainConfirm<Pessoa>("Realmente deseja realizar essa operação.");
        }

		protected virtual Pessoa UpdateDefault(Pessoa model,Pessoa modelOld)
		{
			var alvo = this.GetFromContext(model);
            model.TransferTo(alvo);
            this.rep.Update(alvo, modelOld);
			return model;
		}

		protected Pessoa SaveDefault(Pessoa model, bool validation = true, bool questionToContinue = true)
        {
            var modelOld = this.Get(model);
            var isNew = modelOld.IsNull();

			if (questionToContinue)
            {
                if (Continue(model, modelOld) == false)
                {
                    ConfigMessageDomainConfirm(model);
                    return model;
                }
            }

			this.SetInitialValues(model);
			
            if (validation) ValidationHelper.ValidateAll();

            this.DeleteCollectionsOnSave(model);

            if (isNew)
                this.rep.Add(model);
            else
				this.UpdateDefault(model, modelOld);
           
		    this.ClearCache();
            return model;
        }

		public virtual Pessoa SavePartial(Pessoa model)
        {
  		    model = SaveDefault(model, false);
			this.rep.Commit();
			return model;
        }
		public virtual void DeleteFromRepository(Pessoa alvo)
        {
            this.rep.Delete(alvo);
			this.ClearCache();
        }

		public virtual void ClearCache()
        {
			if (this.cache.IsNotNull())
            {
                var tag = this.cache.GetAndCast<List<string>>("Pessoa");
				if (tag.IsNull()) return;
                foreach (var item in tag)
                {
                    this.cache.Remove(item);    
                }
                this.cache.Remove("Pessoa");
            }
            
        }

		private Expression<Func<Pessoa, object>>[] DataAgregationBehaviorDefault(Expression<Func<Pessoa, object>>[] includes)
        { 
            return includes;
        }


	
	}
}