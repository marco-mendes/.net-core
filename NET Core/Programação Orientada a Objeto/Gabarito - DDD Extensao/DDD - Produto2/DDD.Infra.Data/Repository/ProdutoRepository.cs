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
            return context.Produto.Where(c => c.FabricanteId.Equals(fabricanteId)).Include("Fabricante").ToList();
        }

        public IEnumerable<Produto> GetAll()
        {
            return context.Produto.Include("Fabricante").ToList();
        }

    }
}