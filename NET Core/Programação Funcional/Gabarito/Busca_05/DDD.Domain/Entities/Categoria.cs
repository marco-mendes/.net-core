using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace DDD.Domain.Entities
{
    public class Categoria : BaseEntity
    {
        public string Name { get; set; }
        public string Codigo { get; set; }
        public virtual ICollection<CategoriaProduto> CategoriaProduto {get; set;}
    }
}