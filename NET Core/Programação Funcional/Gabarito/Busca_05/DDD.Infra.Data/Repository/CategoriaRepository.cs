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
    public class CategoriaRepository<T> : BaseRepository<Categoria> where T : Categoria
    {
        public new IList<Categoria> Select()
        {
            return context.Categoria.AsNoTracking().Include("CategoriaProduto").ToList();
        }

        public new Categoria Select(int id)
        {
            return context.Categoria.AsNoTracking().Include("CategoriaProduto").Where(cat => cat.Id.Equals(id)).First();
        }
    }
}