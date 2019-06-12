using System.Collections.Generic;
using System;

namespace DDD.Domain.Entities
{
    public class Fabricante : BaseEntity
    {
        public string Name { get; set; }
        public string Codigo { get; set; }
        public virtual ICollection<Produto> Produtos {get; set;}

    }
}