using FluentValidation;
using DDD.Domain.Entities;
using DDD.Domain.Interfaces;
using DDD.Infra.Data.Repository;
using System;
using System.Collections.Generic;

namespace DDD.Service.Services
{
    public class FabricanteService<T> : BaseService<Fabricante> where T : Fabricante
    {
        private FabricanteRepository<T> repository = new FabricanteRepository<T>();

        public new IList<Fabricante> Get() => repository.Select();

        public new Fabricante Get(int id)
        {
            if (id == 0)
                throw new ArgumentException("The id can't be zero.");

            return repository.Select(id);
        }

    }
}