using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDD.Domain.Entities
{
    public class Produto : BaseEntity
    {
        public string Name { get; set; }
        public int? FabricanteId { get;set; }
        [ForeignKey("FabricanteId")]
        public virtual Fabricante Fabricante { get; set; }
        public string Codigo { get; set; }
        public double Preco { get; set; }
        public string Sku { get; set; }

        /* public Produto (string name, int fabricanteId, string codigo, double preco, string sku) {
            this.Name = name;
            this.FabricanteId =  fabricanteId;
            this.Codigo = Codigo;
            this.Preco = preco;
            this.Sku = sku;
        } 
        public Produto (string name, Fabricante fabricante, string codigo, double preco, string sku) {
            this.Name = name;
            this.Fabricante =  fabricante;
            this.Codigo = codigo;
            this.Preco = preco;
            this.Sku = sku;
        }*/
    }
}