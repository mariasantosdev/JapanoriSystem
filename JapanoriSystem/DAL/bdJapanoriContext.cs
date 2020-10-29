﻿using JapanoriSystem.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace JapanoriSystem.DAL
{
    public class bdJapanoriContext : DbContext
    {

        public bdJapanoriContext() : base()
        {
        }

        public DbSet<Comanda> tbComanda { get; set; }
        public DbSet<ComandaProduto> tbComandaProduto { get; set; }
        public DbSet<Produto> tbProduto { get; set; }
        public DbSet<ProdutoEstoque> tbProdutoEstoque { get; set; }
        public DbSet<Estoque> tbEstoque { get; set; }



        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

    }
}