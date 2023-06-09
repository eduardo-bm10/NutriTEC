﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Postgre_API.Models;

public partial class NutritecDbContext : DbContext
{
    public NutritecDbContext()
    {
    }

    public NutritecDbContext(DbContextOptions<NutritecDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AdminProductAssociation> AdminProductAssociations { get; set; }

    public virtual DbSet<Administrator> Administrators { get; set; }

    public virtual DbSet<Caloriespermealtimeonplan> Caloriespermealtimeonplans { get; set; }

    public virtual DbSet<Consumption> Consumptions { get; set; }

    public virtual DbSet<MealTime> MealTimes { get; set; }

    public virtual DbSet<Measurement> Measurements { get; set; }

    public virtual DbSet<Nonassociatedclient> Nonassociatedclients { get; set; }

    public virtual DbSet<Nutritionist> Nutritionists { get; set; }

    public virtual DbSet<Patient> Patients { get; set; }

    public virtual DbSet<PatientNutritionistAssociation> PatientNutritionistAssociations { get; set; }

    public virtual DbSet<PaymentType> PaymentTypes { get; set; }

    public virtual DbSet<Plan> Plans { get; set; }

    public virtual DbSet<PlanMealtimeAssociation> PlanMealtimeAssociations { get; set; }

    public virtual DbSet<PlanPatientAssociation> PlanPatientAssociations { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Recipe> Recipes { get; set; }

    public virtual DbSet<RecipeProductAssociation> RecipeProductAssociations { get; set; }

    public virtual DbSet<Totalrecipecalory> Totalrecipecalories { get; set; }

    public virtual DbSet<Vitamin> Vitamins { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Server=server-nutritec.postgres.database.azure.com;Database=nutritec-db;Port=5432;User Id=jimena;Password=Nutri_TEC;Ssl Mode=VerifyFull;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AdminProductAssociation>(entity =>
        {
            entity.HasKey(e => new { e.Adminid, e.Productbarcode }).HasName("admin_product_association_pkey");

            entity.ToTable("admin_product_association");

            entity.Property(e => e.Adminid)
                .HasMaxLength(9)
                .HasColumnName("adminid");
            entity.Property(e => e.Productbarcode).HasColumnName("productbarcode");
            entity.Property(e => e.Filler)
                .ValueGeneratedOnAdd()
                .HasColumnName("filler");

            entity.HasOne(d => d.Admin).WithMany(p => p.AdminProductAssociations)
                .HasForeignKey(d => d.Adminid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("keys6");

            entity.HasOne(d => d.ProductbarcodeNavigation).WithMany(p => p.AdminProductAssociations)
                .HasForeignKey(d => d.Productbarcode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("keys7");
        });

        modelBuilder.Entity<Administrator>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("administrator_pkey");

            entity.ToTable("administrator");

            entity.Property(e => e.Id)
                .HasMaxLength(9)
                .HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Firstname)
                .HasMaxLength(50)
                .HasColumnName("firstname");
            entity.Property(e => e.Lastname1)
                .HasMaxLength(50)
                .HasColumnName("lastname1");
            entity.Property(e => e.Lastname2)
                .HasMaxLength(50)
                .HasColumnName("lastname2");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .HasColumnName("password");
        });

        modelBuilder.Entity<Caloriespermealtimeonplan>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("caloriespermealtimeonplan");

            entity.Property(e => e.Mealtime)
                .HasMaxLength(50)
                .HasColumnName("mealtime");
            entity.Property(e => e.Planid).HasColumnName("planid");
            entity.Property(e => e.Totalcalories).HasColumnName("totalcalories");
        });

        modelBuilder.Entity<Consumption>(entity =>
        {
            entity.HasKey(e => new { e.Patientid, e.Date, e.Mealtime }).HasName("consumption_pkey");

            entity.ToTable("consumption");

            entity.Property(e => e.Patientid)
                .HasMaxLength(9)
                .HasColumnName("patientid");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.Mealtime).HasColumnName("mealtime");
            entity.Property(e => e.Productbarcode).HasColumnName("productbarcode");

            entity.HasOne(d => d.MealtimeNavigation).WithMany(p => p.Consumptions)
                .HasForeignKey(d => d.Mealtime)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("keys4");

            entity.HasOne(d => d.Patient).WithMany(p => p.Consumptions)
                .HasForeignKey(d => d.Patientid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("keys3");

            entity.HasOne(d => d.ProductbarcodeNavigation).WithMany(p => p.Consumptions)
                .HasForeignKey(d => d.Productbarcode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("keys15");
        });

        modelBuilder.Entity<MealTime>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("meal_time_pkey");

            entity.ToTable("meal_time");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Measurement>(entity =>
        {
            entity.HasKey(e => new { e.Patientid, e.Date }).HasName("measurement_pkey");

            entity.ToTable("measurement");

            entity.Property(e => e.Patientid)
                .HasMaxLength(9)
                .HasColumnName("patientid");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.Fatpercentage).HasColumnName("fatpercentage");
            entity.Property(e => e.Hips).HasColumnName("hips");
            entity.Property(e => e.Musclepercentage).HasColumnName("musclepercentage");
            entity.Property(e => e.Neck).HasColumnName("neck");
            entity.Property(e => e.Waist).HasColumnName("waist");

            entity.HasOne(d => d.Patient).WithMany(p => p.Measurements)
                .HasForeignKey(d => d.Patientid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("keys5");
        });

        modelBuilder.Entity<Nonassociatedclient>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("nonassociatedclients");

            entity.Property(e => e.Patientname).HasColumnName("patientname");
            entity.Property(e => e.Patientssn)
                .HasMaxLength(9)
                .HasColumnName("patientssn");
        });

        modelBuilder.Entity<Nutritionist>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("nutritionist_pkey");

            entity.ToTable("nutritionist");

            entity.HasIndex(e => e.Nutritionistcode, "nutritionist_nutritionistcode_key").IsUnique();

            entity.Property(e => e.Id)
                .HasMaxLength(9)
                .HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(50)
                .HasColumnName("address");
            entity.Property(e => e.Bmi).HasColumnName("bmi");
            entity.Property(e => e.CardNumber)
                .HasMaxLength(16)
                .HasColumnName("card_number");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Firstname)
                .HasMaxLength(50)
                .HasColumnName("firstname");
            entity.Property(e => e.Lastname1)
                .HasMaxLength(50)
                .HasColumnName("lastname1");
            entity.Property(e => e.Lastname2)
                .HasMaxLength(50)
                .HasColumnName("lastname2");
            entity.Property(e => e.Nutritionistcode)
                .HasMaxLength(5)
                .HasColumnName("nutritionistcode");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .HasColumnName("password");
            entity.Property(e => e.Paymentid).HasColumnName("paymentid");
            entity.Property(e => e.Photo)
                .HasMaxLength(50)
                .HasColumnName("photo");
            entity.Property(e => e.Weight).HasColumnName("weight");

            entity.HasOne(d => d.Payment).WithMany(p => p.Nutritionists)
                .HasForeignKey(d => d.Paymentid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("keys1");
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("patient_pkey");

            entity.ToTable("patient");

            entity.Property(e => e.Id)
                .HasMaxLength(9)
                .HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(50)
                .HasColumnName("address");
            entity.Property(e => e.Birthdate).HasColumnName("birthdate");
            entity.Property(e => e.Bmi).HasColumnName("bmi");
            entity.Property(e => e.Country)
                .HasMaxLength(20)
                .HasColumnName("country");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Firstname)
                .HasMaxLength(50)
                .HasColumnName("firstname");
            entity.Property(e => e.Lastname1)
                .HasMaxLength(50)
                .HasColumnName("lastname1");
            entity.Property(e => e.Lastname2)
                .HasMaxLength(50)
                .HasColumnName("lastname2");
            entity.Property(e => e.Maxconsumption).HasColumnName("maxconsumption");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .HasColumnName("password");
            entity.Property(e => e.Weight).HasColumnName("weight");
        });

        modelBuilder.Entity<PatientNutritionistAssociation>(entity =>
        {
            entity.HasKey(e => new { e.Nutritionistid, e.Patientid }).HasName("patient_nutritionist_association_pkey");

            entity.ToTable("patient_nutritionist_association");

            entity.Property(e => e.Nutritionistid)
                .HasMaxLength(9)
                .HasColumnName("nutritionistid");
            entity.Property(e => e.Patientid)
                .HasMaxLength(9)
                .HasColumnName("patientid");
            entity.Property(e => e.Filler)
                .ValueGeneratedOnAdd()
                .HasColumnName("filler");

            entity.HasOne(d => d.Nutritionist).WithMany(p => p.PatientNutritionistAssociations)
                .HasForeignKey(d => d.Nutritionistid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("keys8");

            entity.HasOne(d => d.Patient).WithMany(p => p.PatientNutritionistAssociations)
                .HasForeignKey(d => d.Patientid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("keys9");
        });

        modelBuilder.Entity<PaymentType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("payment_type_pkey");

            entity.ToTable("payment_type");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .HasColumnName("description");
        });

        modelBuilder.Entity<Plan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("plan_pkey");

            entity.ToTable("plan");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .HasColumnName("description");
            entity.Property(e => e.Nutritionistid)
                .HasMaxLength(50)
                .HasColumnName("nutritionistid");

            entity.HasOne(d => d.Nutritionist).WithMany(p => p.Plans)
                .HasForeignKey(d => d.Nutritionistid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("keys2");
        });

        modelBuilder.Entity<PlanMealtimeAssociation>(entity =>
        {
            entity.HasKey(e => new { e.Planid, e.Mealtimeid, e.Productbarcode }).HasName("plan_mealtime_association_pkey");

            entity.ToTable("plan_mealtime_association");

            entity.Property(e => e.Planid).HasColumnName("planid");
            entity.Property(e => e.Mealtimeid).HasColumnName("mealtimeid");
            entity.Property(e => e.Productbarcode).HasColumnName("productbarcode");
            entity.Property(e => e.Filler)
                .ValueGeneratedOnAdd()
                .HasColumnName("filler");

            entity.HasOne(d => d.Mealtime).WithMany(p => p.PlanMealtimeAssociationMealtimes)
                .HasForeignKey(d => d.Mealtimeid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("keys13");

            entity.HasOne(d => d.Plan).WithMany(p => p.PlanMealtimeAssociationPlans)
                .HasForeignKey(d => d.Planid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("keys12");

            entity.HasOne(d => d.ProductbarcodeNavigation).WithMany(p => p.PlanMealtimeAssociations)
                .HasForeignKey(d => d.Productbarcode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("keys19");
        });

        modelBuilder.Entity<PlanPatientAssociation>(entity =>
        {
            entity.HasKey(e => new { e.Patientid, e.Planid }).HasName("plan_patient_association_pkey");

            entity.ToTable("plan_patient_association");

            entity.Property(e => e.Patientid)
                .HasMaxLength(9)
                .HasColumnName("patientid");
            entity.Property(e => e.Planid).HasColumnName("planid");
            entity.Property(e => e.Enddate).HasColumnName("enddate");
            entity.Property(e => e.Filler)
                .ValueGeneratedOnAdd()
                .HasColumnName("filler");
            entity.Property(e => e.Startdate).HasColumnName("startdate");

            entity.HasOne(d => d.Patient).WithMany(p => p.PlanPatientAssociations)
                .HasForeignKey(d => d.Patientid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("keys16");

            entity.HasOne(d => d.Plan).WithMany(p => p.PlanPatientAssociations)
                .HasForeignKey(d => d.Planid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("keys17");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Barcode).HasName("product_pkey");

            entity.ToTable("product");

            entity.Property(e => e.Barcode)
                .ValueGeneratedNever()
                .HasColumnName("barcode");
            entity.Property(e => e.Calcium).HasColumnName("calcium");
            entity.Property(e => e.Carbohydrate).HasColumnName("carbohydrate");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .HasColumnName("description");
            entity.Property(e => e.Energy).HasColumnName("energy");
            entity.Property(e => e.Fat).HasColumnName("fat");
            entity.Property(e => e.Iron).HasColumnName("iron");
            entity.Property(e => e.Protein).HasColumnName("protein");
            entity.Property(e => e.Sodium).HasColumnName("sodium");
            entity.Property(e => e.Status).HasColumnName("status");
        });

        modelBuilder.Entity<Recipe>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("recipe_pkey");

            entity.ToTable("recipe");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .HasColumnName("description");
        });

        modelBuilder.Entity<RecipeProductAssociation>(entity =>
        {
            entity.HasKey(e => new { e.Recipeid, e.Productbarcode }).HasName("recipe_product_association_pkey");

            entity.ToTable("recipe_product_association");

            entity.Property(e => e.Recipeid).HasColumnName("recipeid");
            entity.Property(e => e.Productbarcode).HasColumnName("productbarcode");
            entity.Property(e => e.Filler)
                .ValueGeneratedOnAdd()
                .HasColumnName("filler");
            entity.Property(e => e.Productportion).HasColumnName("productportion");

            entity.HasOne(d => d.ProductbarcodeNavigation).WithMany(p => p.RecipeProductAssociations)
                .HasForeignKey(d => d.Productbarcode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("keys11");

            entity.HasOne(d => d.Recipe).WithMany(p => p.RecipeProductAssociations)
                .HasForeignKey(d => d.Recipeid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("keys10");
        });

        modelBuilder.Entity<Totalrecipecalory>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("totalrecipecalories");

            entity.Property(e => e.Product)
                .HasMaxLength(50)
                .HasColumnName("product");
            entity.Property(e => e.Productcalories).HasColumnName("productcalories");
            entity.Property(e => e.Productportion).HasColumnName("productportion");
            entity.Property(e => e.Recipedescription)
                .HasMaxLength(50)
                .HasColumnName("recipedescription");
            entity.Property(e => e.Recipeid).HasColumnName("recipeid");
            entity.Property(e => e.Totalproductcalories).HasColumnName("totalproductcalories");
            entity.Property(e => e.Totalrecipecalories).HasColumnName("totalrecipecalories");
        });

        modelBuilder.Entity<Vitamin>(entity =>
        {
            entity.HasKey(e => new { e.ProductBarcode, e.Vitamin1 }).HasName("vitamins_pkey");

            entity.ToTable("vitamins");

            entity.Property(e => e.ProductBarcode).HasColumnName("product_barcode");
            entity.Property(e => e.Vitamin1)
                .HasMaxLength(20)
                .HasColumnName("vitamin");

            entity.HasOne(d => d.ProductBarcodeNavigation).WithMany(p => p.Vitamins)
                .HasForeignKey(d => d.ProductBarcode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("keys18");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
