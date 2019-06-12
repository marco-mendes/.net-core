using FluentValidation;
using DDD.Domain.Entities;
using DDD.Domain.Interfaces;
using DDD.Infra.Data.Repository;
using System;
using System.Collections.Generic;

namespace DDD.Service.Services
{
    public class CategoriaService<T> : BaseService<Categoria> where T : Categoria
    {
        private CategoriaRepository<T> _repository = new CategoriaRepository<T>();

        public new IList<Categoria> Get() => _repository.Select();

        public new Categoria Get(int id)
        {
            if (id == 0)
                throw new ArgumentException("The id can't be zero.");

            return _repository.Select(id);
        }

    }
}