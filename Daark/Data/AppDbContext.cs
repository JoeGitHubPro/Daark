using System;
using System.Collections.Generic;
using Daark.Entities.Identity.Models;
using Daark.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Daark.Entities.Identity.Models;
using Microsoft.EntityFrameworkCore.Metadata;


namespace Daark.Data;

public partial class AppDbContext : IdentityDbContext<ApplicationUser>
{

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }



    public virtual DbSet<Bayut> Bayuts { get; set; }

    public virtual DbSet<DaarkRealEstate> DaarkRealEstates { get; set; }

    public virtual DbSet<Lead> Leads { get; set; }

    public virtual DbSet<Portal> Portals { get; set; }

    public virtual DbSet<PropertyFinder> PropertyFinders { get; set; }

    public virtual DbSet<Team> Teams { get; set; }

    public virtual DbSet<UserTeam> UserTeams { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {


        modelBuilder.Entity<Bayut>(entity =>
        {
            entity.ToTable("Bayut");
        });

        modelBuilder.Entity<DaarkRealEstate>(entity =>
        {
            entity.ToTable("DaarkRealEstate");

            entity.HasIndex(e => e.LeadsId, "IX_DaarkRealEstate_LeadsId");

            entity.HasIndex(e => e.PortalsId, "IX_DaarkRealEstate_PortalsId");

            entity.HasIndex(e => e.UserId, "IX_DaarkRealEstate_UserId");

            //entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Id)
     .ValueGeneratedOnAdd()
     .UseIdentityColumn(seed: 1000);





            entity.Property(e => e.Date).HasColumnType("date");

            entity.HasOne(d => d.Leads).WithMany(p => p.DaarkRealEstates)
                .HasForeignKey(d => d.LeadsId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_DaarkRealEstate_Leads");

            entity.HasOne(d => d.Portals).WithMany(p => p.DaarkRealEstates)
                .HasForeignKey(d => d.PortalsId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_DaarkRealEstate_Portals");

            entity.HasOne(d => d.User).WithMany(p => p.DaarkRealEstates)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_DaarkRealEstate_AspNetUsers");

        });

        modelBuilder.Entity<Portal>(entity =>
        {
            entity.HasIndex(e => e.BayutId, "IX_Portals_BayutId");

            entity.HasIndex(e => e.PropertyFinderId, "IX_Portals_PropertyFinderId");

            entity.HasOne(d => d.Bayut).WithMany(p => p.Portals)
                .HasForeignKey(d => d.BayutId)
                .HasConstraintName("FK_Portals_Bayut");

            entity.HasOne(d => d.PropertyFinder).WithMany(p => p.Portals)
                .HasForeignKey(d => d.PropertyFinderId)
                .HasConstraintName("FK_Portals_PropertyFinder");
        });

        modelBuilder.Entity<PropertyFinder>(entity =>
        {
            entity.ToTable("PropertyFinder");
        });



        modelBuilder.Entity<Team>(entity =>
        {
            entity.Property(e => e.Id)
            .UseIdentityColumn(seed: 1)
            .ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<UserTeam>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("UserTeam");

            entity.HasKey(e => e.UserId);

            entity.Property(e => e.UserTeamId)
                        .UseIdentityColumn(seed: 1)
                        .ValueGeneratedOnAdd();


            entity.Property(e => e.UserId).HasMaxLength(450);

            entity.HasOne(d => d.Team).WithMany()
                .HasForeignKey(d => d.TeamId)
                .HasConstraintName("FK_UserTeam_Teams");

            entity.HasOne(d => d.User).WithMany()
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_UserTeam_AspNetUsers");
        });

        modelBuilder.Entity<ApplicationUser>()
       .Property(e => e.UserId)
       .UseIdentityColumn(seed: 1000)
       .ValueGeneratedOnAdd();

        modelBuilder.Entity<ApplicationUser>()
      .HasIndex(e => e.PhoneNumber)
      .IsUnique(true);

        OnModelCreatingPartial(modelBuilder);
        base.OnModelCreating(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
