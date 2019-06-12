using FluentValidation;
using DDD.Domain.Entities;
using DDD.Domain.Interfaces;
using DDD.Infra.Data.Repository;
using System;
using System.Collections.Generic;

namespace DDD.Service.Services
{
    public class ProdutoService<T> : BaseService<Produto> where T : Produto
    {
        private ProdutoRepository<T> _repository = new ProdutoRepository<T>();

        public T Post<V>(T obj) where V : AbstractValidator<T>
        {
            Validate(obj, Activator.CreateInstance<V>());

            _repository.Insert(obj);
            return obj;
        }

        public T Put<V>(T obj) where V : AbstractValidator<T>
        {
            Validate(obj, Activator.CreateInstance<V>());

            _repository.Update(obj);
            return obj;
        }

        public new IList<Produto> Get() => _repository.Select();

        public IEnumerable<Produto> Search(Func<Produto, bool> condition)
        {
            return _repository.Search(condition);
        }
        public new Produto Get(int id)
        {
            if (id == 0)
                throw new ArgumentException("The id can't be zero.");

            return _repository.Select(id);
        }

        private void Validate(T obj, AbstractValidator<T> validator)
        {
            if (obj == null)
                throw new Exception("Registros não detectados!");

            validator.ValidateAndThrow(obj);
        }
    }
}