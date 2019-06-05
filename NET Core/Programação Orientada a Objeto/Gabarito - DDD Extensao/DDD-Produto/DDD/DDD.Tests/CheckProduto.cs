using System;
using Xunit;
using DDD.Infra.Data.Repository;
using DDD.Domain.Interfaces;
using DDD.Domain.Entities;
using DDD.Service;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using DDD.Service.Validators;
using FluentValidation;

namespace DDD.Tests
{
    public class CheckProduto
    {
        [Theory]
        [InlineData("pdt1", "2", "22", 0, "teste")]
        [InlineData("pdt2", "3", "33", 1, "teste")]
        [InlineData("pdt3", "4", "44", -1, "teste")]
        public void CanCreateAndSet(string name, string codigo, string sku, double preco, string fabricante="")
        {
            // Somente teste de criação, sem salvar em banco.
            var pdt = new Produto();
            Assert.NotNull(pdt);
            pdt.Name = name;
            pdt.Codigo = codigo;
            pdt.SKU = sku;
            pdt.Preco = preco;
            pdt.Fabricante = fabricante;
            
            Assert.Equal(pdt.Name, name);
            Assert.Equal(pdt.Codigo, codigo);
            Assert.Equal(pdt.SKU, sku);
            Assert.Equal(pdt.Preco, preco);
            Assert.Equal(pdt.Fabricante, fabricante);
            
        }

        [Theory]
        [InlineData("pdt1", "2", "22", 4, "teste")]
        [InlineData("pdt2", "3", "33", 1, "teste")]
        [InlineData("pdt3", "4", "44", -1, "teste")]
        public void CanCreateAndSave(string name, string codigo, string sku, double preco, string fabricante="")
        {
            TestService<Produto> service = new TestService<Produto>();

            var pdt = new Produto();
            Assert.NotNull(pdt);
            pdt.Name = name;
            pdt.Codigo = codigo;
            pdt.SKU = sku;
            pdt.Preco = preco;
            pdt.Fabricante = fabricante;

            // Se houver validações, deve dar erro ao adicionar no banco.
            if (preco < 0 || name.Length < 1 || sku.Length < 1 || codigo.Length < 1 ) {
                Assert.Throws<FluentValidation.ValidationException>(() => service.Post<ProdutoValidator>(pdt));
                return;
            }
            // Agora criando, resgatando, comparando e deletando.
            service.Post<ProdutoValidator>(pdt);
            Assert.True(pdt.Id > -1);
            var pdt2 = service.Get(pdt.Id);

            Assert.Equal(pdt2.Name, pdt.Name);
            Assert.Equal(pdt2.SKU, pdt.SKU);
            Assert.Equal(pdt2.Fabricante, pdt.Fabricante);

            var id = pdt.Id;
            service.Delete(pdt.Id);

            // Tentando deletar novamente e verificando se Exception é gerada.
            Assert.Throws<System.ArgumentNullException>(() => service.Delete(id));
        }

    }
}

