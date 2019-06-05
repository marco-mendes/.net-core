using System;
using DDD.Infra.Data.Repository;
using DDD.Domain.Interfaces;
using DDD.Domain.Entities;
using DDD.Service;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using DDD.Infra.Data.Context;
using DDD.Service.Services;
using DDD.Infra.Data.Mapping;
using FluentValidation;

namespace DDD.Tests
{
    public class TestContext : DbContext
    {
        
        public DbSet<User> User { get; set; }
        public DbSet<Produto> Produto { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Utilizando um servidor SQLite local. Aqui poderíamos configurar qualquer outro banco de dados.
            if (!optionsBuilder.IsConfigured)
                 optionsBuilder.UseInMemoryDatabase(databaseName: "test1");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(new UserMap().Configure);
            modelBuilder.Entity<Produto>(new ProdutoMap().Configure);
        }

    }
    public class TestRepository<T> : BaseRepository<T> where T : BaseEntity
    {
       private TestContext context = new TestContext();
        new public void Insert(T obj)
        {
            context.Set<T>().Add(obj);
            context.SaveChanges();
        }

        new public void Update(T obj)
        {
            context.Entry(obj).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
        }

        new public void Delete(int id)
        {
            context.Set<T>().Remove(Select(id));
            context.SaveChanges();
        }

        new public IList<T> Select()
        {
            return context.Set<T>().ToList();
        }

        new public T Select(int id)
        {
            return context.Set<T>().Find(id);
        }

    }
    public class TestService<T> : IService<T> where T : BaseEntity
    {
        private TestRepository<T> repository = new TestRepository<T>();

        public T Post<V>(T obj) where V : AbstractValidator<T>
        {
            Validate(obj, Activator.CreateInstance<V>());

            repository.Insert(obj);
            return obj;
        }

        public T Put<V>(T obj) where V : AbstractValidator<T>
        {
            Validate(obj, Activator.CreateInstance<V>());

            repository.Update(obj);
            return obj;
        }

        public void Delete(int id)
        {
            if (id == 0)
                throw new ArgumentException("The id can't be zero.");

            repository.Delete(id);
        }

        public IList<T> Get() => repository.Select();

        public T Get(int id)
        {
            if (id == 0)
                throw new ArgumentException("The id can't be zero.");

            return repository.Select(id);
        }

        private void Validate(T obj, AbstractValidator<T> validator)
        {
            if (obj == null)
                throw new Exception("Registros não detectados!");

            validator.ValidateAndThrow(obj);
        }
    }

}