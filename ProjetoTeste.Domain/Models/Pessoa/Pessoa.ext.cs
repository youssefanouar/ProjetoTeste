using Common.Domain;
using Common.Domain.CustomExceptions;
using Common.Domain.Interfaces;
using Common.Models;
using ProjetoTeste.Core.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;

namespace ProjetoTeste.Core.Domain
{
    [MetadataType(typeof(PessoaValidation))]
    public partial class Pessoa : IDataAgregation<Pessoa>
    {
        public Pessoa()
        {
        }
        public ValidationHelper ValidationHelper = new ValidationHelper();



        public Pessoa Get(Pessoa model)
        {
            return this.rep.GetAll(DataAgregation(new PessoaFilter
            {
                QueryOptimizerBehavior = model.QueryOptimizerBehavior

            })).Where(_ => _.PessoaId == model.PessoaId).SingleOrDefault();
        }

        public IQueryable<Pessoa> GetByFilters(PessoaFilter filters, params Expression<Func<Pessoa, object>>[] includes)
        {
            var queryBase = this.rep.GetAllAsTracking(includes);
            var queryFilter = queryBase;

            queryFilter = this.SimpleFilters(filters, queryBase);

            //Filter Customizados

            return queryFilter;
        }

        public IEnumerable<DataItem> GetDataItem(PessoaFilter filters)
        {
            var dataList = this.GetByFilters(filters)
                .Select(_ => new DataItem
                {
                    Id = _.PessoaId.ToString(),
                });

            return dataList.ToList();
        }

        public Common.Models.Summary GetSummary(IQueryable<Pessoa> result)
        {
            return new Common.Models.Summary
            {
                Total = 0
            };
        }

        public Pessoa Save()
        {
            return Save(this);
        }

        public Pessoa Save(Pessoa model)
        {
            model = SaveDefault(model);
            this.rep.Commit();
            return model;
        }

        public IEnumerable<Pessoa> Save(IEnumerable<Pessoa> models)
        {
            var modelsInserted = new List<Pessoa>();
            foreach (var item in models)
            {
                modelsInserted.Add(SaveDefault(item));
            }

            this.rep.Commit();
            return modelsInserted;
        }

        public void Delete(Pessoa model)
        {
            if (model.IsNull())
                throw new CustomBadRequestException("Delete sem parametros");

            var alvo = this.Get(model);
            this.DeleteFromRepository(alvo);
            this.rep.Commit();
        }

        private void SetInitialValues(Pessoa model)
        {
        }

        private void ValidationReletedClasses(Pessoa model, CurrentUser user, Pessoa modelOld)
        {

        }

        private void DeleteCollectionsOnSave(Pessoa model)
        {

        }

        public Expression<Func<Pessoa, object>>[] DataAgregation(Filter filter)
        {
            return DataAgregation(new Expression<Func<Pessoa, object>>[] { }, filter);
        }

        public Expression<Func<Pessoa, object>>[] DataAgregation(Expression<Func<Pessoa, object>>[] includes, Filter filter)
        {
            return this.DataAgregationBehaviorDefault(includes);

        }


        public override void Dispose()
        {
            if (this.rep != null)
                this.rep.Dispose();
        }

        ~Pessoa()
        {

        }

    }
}