using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SportsBetting.Data.Models;

public partial class SportsBettingContext : DbContext
{
    public SportsBettingContext()
    {
    }

    public SportsBettingContext(DbContextOptions<SportsBettingContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BetOffer> BetOffers { get; set; }

    public virtual DbSet<BetOfferType> BetOfferTypes { get; set; }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<League> Leagues { get; set; }

    public virtual DbSet<Result> Results { get; set; }

    public virtual DbSet<Sport> Sports { get; set; }

    public virtual DbSet<Team> Teams { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BetOffer>(entity =>
        {
            entity.HasKey(e => e.Id);
        });

        modelBuilder.Entity<BetOfferType>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.EndDateTime);
            entity.Property(e => e.IsBetsOpened).IsRequired();
            entity.Property(e => e.StartDateTime);
        });

        modelBuilder.Entity<League>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Result>(entity =>
        {
            entity.HasKey(e => e.Id);
        });

        modelBuilder.Entity<Sport>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
               
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
            
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
