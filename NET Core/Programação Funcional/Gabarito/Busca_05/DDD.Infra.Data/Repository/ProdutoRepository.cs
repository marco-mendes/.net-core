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
    public class ProdutoRepository<T> : BaseRepository<Produto> where T : Produto
    {
        public new void Insert(Produto obj)
        {

            var fab = obj.Fabricante;

            obj.Fabricante = null;
            
            
            context.Produto.Add(obj);
            context.SaveChanges();
            if (fab != null) {
                obj.Fabricante = context.Fabricante.Find(fab.Id);
                Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(context.Fabricante.Find(obj.Fabricante.Id)));
            }           
            if (obj.CategoriaProduto != null) {
                foreach(CategoriaProduto cat in obj.CategoriaProduto) {
                    cat.ProdutoId = obj.Id;
                }
            }
            context.SaveChanges();

        }
        public IEnumerable<Produto> Search(Func<Produto, bool> condition) {
            return context.Produto.AsNoTracking().Include("Fabricante").Include(pdt => pdt.CategoriaProduto).Where(condition).ToList();
        }
        public new void Update(Produto obj)
        {
            if (obj.Fabricante != null) {
                obj.Fabricante = context.Fabricante.Find(obj.Fabricante.Id);
            }
            context.Entry(obj).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            
            
            if (obj.CategoriaProduto != null) {
                // Limpando categorias anteriores
                IList<CategoriaProduto> anteriores = context.CategoriaProduto.Where(cp => cp.ProdutoId == obj.Id).ToList();
                foreach (var ant in anteriores)
                {
                    context.CategoriaProduto.Remove(ant);
                }
                // Adicionando categorias novas
                foreach(CategoriaProduto cat in obj.CategoriaProduto) {
                    cat.ProdutoId = obj.Id;
                }
            }
            context.SaveChanges();
        }

        public new void Delete(int id)
        {
            context.Produto.Remove(Select(id));
            context.SaveChanges();
        }

        public new IList<Produto> Select()
        {
            return context.Produto.AsNoTracking().Include(f => f.Fabricante).Include(a => a.CategoriaProduto).ThenInclude(c => c.Categoria).ToList();
        }

        public new Produto Select(int id)
        {
            return context.Produto.Include(f => f.Fabricante).Include(a => a.CategoriaProduto).ThenInclude(c => c.Categoria).Where(pdt => pdt.Id.Equals(id)).First();
        }
    }
}