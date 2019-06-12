using System;
using System.Collections.Generic;

namespace DDD.Domain.Entities
{
    public class Produto : BaseEntity
    {
        public string Name { get; set; }
        public virtual Fabricante Fabricante { get; set; }
        public virtual ICollection<CategoriaProduto> CategoriaProduto {get; set;}
        public double Preco { get; set; }
        public string Sku { get; set; }
    }
}