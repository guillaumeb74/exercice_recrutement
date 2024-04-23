using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace ExerciceRecrutementRestaurant.Models;

public partial class RestaurantsContext : DbContext
{
    public RestaurantsContext()
    {

    }

    public RestaurantsContext(DbContextOptions<RestaurantsContext> options)
        : base(options)
    {
        
    }

    public virtual DbSet<Meal> Meals { get; set; }

    public virtual DbSet<Restaurant> Restaurants { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        #warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        optionsBuilder.UseSqlite("Data Source=.\\Database\\restaurants.db");
        //optionsBuilder.UseLazyLoadingProxies();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

    }

}
