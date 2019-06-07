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
        private readonly FabricanteRepostory<Fabricante> _fabricanteRepository = new FabricanteRepostory<Fabricante>();

        public Fabricante GetWithProdutcs(int fabricanteId)
        {
            return _fabricanteRepository.GetWithProdutcs(fabricanteId);
        }

        public IEnumerable<Fabricante> GetAll()
        {
            return _fabricanteRepository.GetAll();
        }
    }
}