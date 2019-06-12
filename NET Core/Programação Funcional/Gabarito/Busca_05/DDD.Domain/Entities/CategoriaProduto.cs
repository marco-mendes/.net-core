using System;
using System.Collections.Generic;

namespace DDD.Domain.Entities
{
    public class CategoriaProduto
    {
        public int CategoriaId { get; set; }
        public Categoria Categoria {get; set;}
        public int ProdutoId { get; set; }
        public Produto Produto {get; set;}
    }
}