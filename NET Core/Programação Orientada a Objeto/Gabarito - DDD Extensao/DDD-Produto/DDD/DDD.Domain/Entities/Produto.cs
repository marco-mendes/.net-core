using System;

namespace DDD.Domain.Entities
{
    public class Produto : BaseEntity
    {
        public string Name { get; set; }
        public string Fabricante { get; set; }
        public string Codigo { get; set; }
        public double Preco { get; set; }
        public string SKU { get; set; }
    }
}