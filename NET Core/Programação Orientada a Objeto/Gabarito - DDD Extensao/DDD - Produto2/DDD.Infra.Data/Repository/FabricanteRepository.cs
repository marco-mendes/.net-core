using DDD.Domain.Entities;
using DDD.Domain.Interfaces;
using DDD.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace DDD.Infra.Data.Repository
{
    public class FabricanteRepostory<T> : BaseRepository<T> where T : Fabricante
    {
        public Fabricante GetWithProdutcs(int id)
        {
            return context.Fabricante.Where(c => c.Id == id).Include("Produtos").FirstOrDefault();
        }

        public IEnumerable<Fabricante> GetAll()
        {
            return context.Fabricante.ToList();
        }

    }
}