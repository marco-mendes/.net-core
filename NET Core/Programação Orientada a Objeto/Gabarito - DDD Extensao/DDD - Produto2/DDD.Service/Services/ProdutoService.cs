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
        private readonly ProdutoRepository<Produto> _produtoRepository = new ProdutoRepository<Produto>();

        public IEnumerable<Produto> GetAllByFabricante(int fabricanteId)
        {
            return _produtoRepository.GetAllByFabricante(fabricanteId);
        }

        public IEnumerable<Produto> GetAll()
        {
            return _produtoRepository.GetAll();
        }
    }
}