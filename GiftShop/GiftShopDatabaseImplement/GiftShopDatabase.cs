﻿using GiftShopDatabaseImplement.Models;
using Microsoft.EntityFrameworkCore;

namespace GiftShopDatabaseImplement
{
    public class GiftShopDatabase : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured == false)
            {
                optionsBuilder.UseSqlServer(@"Data Source=DESKTOP-NSNKSRN;Initial Catalog=GiftShopDatabase;Integrated Security=True;MultipleActiveResultSets=True;");
            }
            base.OnConfiguring(optionsBuilder);
        }
        public virtual DbSet<Material> Materials { set; get; }
        public virtual DbSet<Gift> Gifts { set; get; }
        public virtual DbSet<GiftMaterial> GiftMaterials { set; get; }
        public virtual DbSet<Order> Orders { set; get; }
    }
}
