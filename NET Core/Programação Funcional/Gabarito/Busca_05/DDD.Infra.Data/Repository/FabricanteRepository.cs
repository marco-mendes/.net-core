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
    public class FabricanteRepository<T> : BaseRepository<Fabricante> where T : Fabricante
    {
        public new IList<Fabricante> Select()
        {
            return context.Fabricante.AsNoTracking().Include("Produtos").ToList();
        }

        public new Fabricante Select(int id)
        {
            return context.Fabricante.AsNoTracking().Include("Produtos").Where(fab => fab.Id.Equals(id)).First();
        }
    }
}