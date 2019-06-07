using System.Collections.Generic;
using System;
using System.Linq;
using System.Collections.ObjectModel;

namespace DDD.Domain.Entities
{
    public class Fabricante : BaseEntity
    {
        public string Name { get; set; }
        public string Codigo { get; set; }
        public virtual ICollection<Produto> Produtos {get; set;}

        public bool removeProduto (Produto pdt) {
            var itemToRemove = Produtos.SingleOrDefault(r => r.Id == pdt.Id);
            
            if (itemToRemove != null) {
                Produtos.Remove(itemToRemove);
                return true;
            }
            return false;
        }
        public bool adicionaProduto (Produto pdt) {
            var existe = Produtos.SingleOrDefault(r => r.Id == pdt.Id);
            if (existe == null) {
                Produtos.Add(pdt);
                return true;
            }
            return false;
        }
    }
}