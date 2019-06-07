using DDD.Domain.Entities;
using DDD.Domain.Interfaces;
using DDD.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace DDD.Infra.Data.Repository
{
    public class ProdutoRepository<T> : BaseRepository<T> where T : Produto
    {
        public IEnumerable<Produto> GetAllByFabricante(int fabricanteId)
        {
            return context.Produto.AsNoTracking().Where(c => c.FabricanteId.Equals(fabricanteId)).Include("Fabricante").ToList();
        }

        public IEnumerable<Produto> GetAll()
        {
            return context.Produto.AsNoTracking().Include(p => p.Fabricante).ToList();
        }

        public IEnumerable<Produto> Search(Func<Produto, bool> condition) {
            return context.Produto.AsNoTracking().Include("Fabricante").Where(condition).ToList();
        }

    }
}