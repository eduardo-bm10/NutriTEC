using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Postgre_API;

public partial class NutriTecDbContext : DbContext
{
    public NutriTecDbContext()
    {
    }

    public NutriTecDbContext(DbContextOptions<NutriTecDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Administrador> Administradors { get; set; }

    public virtual DbSet<AsociacionPlanPaciente> AsociacionPlanPacientes { get; set; }

    public virtual DbSet<AsociacionRecetaProducto> AsociacionRecetaProductos { get; set; }

    public virtual DbSet<Consumo> Consumos { get; set; }

    public virtual DbSet<Medida> Medidas { get; set; }

    public virtual DbSet<Nutricionistum> Nutricionista { get; set; }

    public virtual DbSet<Paciente> Pacientes { get; set; }

    public virtual DbSet<Plan> Plans { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<Recetum> Receta { get; set; }

    public virtual DbSet<TiempoComidum> TiempoComida { get; set; }

    public virtual DbSet<TipoCobro> TipoCobros { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=NutriTEC-DB;Username=postgres;Password=1234");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Administrador>(entity =>
        {
            entity.HasKey(e => e.Cedula).HasName("administrador_pkey");

            entity.ToTable("administrador");

            entity.Property(e => e.Cedula)
                .HasMaxLength(9)
                .HasColumnName("cedula");
            entity.Property(e => e.Apellido1)
                .HasMaxLength(50)
                .HasColumnName("apellido1");
            entity.Property(e => e.Apellido2)
                .HasMaxLength(50)
                .HasColumnName("apellido2");
            entity.Property(e => e.Contrasena)
                .HasMaxLength(50)
                .HasColumnName("contrasena");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .HasColumnName("nombre");

            entity.HasMany(d => d.CodigoBarrasProductos).WithMany(p => p.CedulaAdmins)
                .UsingEntity<Dictionary<string, object>>(
                    "AsociacionAdminProducto",
                    r => r.HasOne<Producto>().WithMany()
                        .HasForeignKey("CodigoBarrasProducto")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("llaves1"),
                    l => l.HasOne<Administrador>().WithMany()
                        .HasForeignKey("CedulaAdmin")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("llaves0"),
                    j =>
                    {
                        j.HasKey("CedulaAdmin", "CodigoBarrasProducto").HasName("asociacion_admin_producto_pkey");
                        j.ToTable("asociacion_admin_producto");
                        j.IndexerProperty<string>("CedulaAdmin")
                            .HasMaxLength(9)
                            .HasColumnName("cedula_admin");
                        j.IndexerProperty<int>("CodigoBarrasProducto").HasColumnName("codigo_barras_producto");
                    });
        });

        modelBuilder.Entity<AsociacionPlanPaciente>(entity =>
        {
            entity.HasKey(e => new { e.CedulaPaciente, e.IdPlan }).HasName("asociacion_plan_paciente_pkey");

            entity.ToTable("asociacion_plan_paciente");

            entity.Property(e => e.CedulaPaciente)
                .HasMaxLength(9)
                .HasColumnName("cedula_paciente");
            entity.Property(e => e.IdPlan).HasColumnName("id_plan");
            entity.Property(e => e.FechaFin).HasColumnName("fecha_fin");
            entity.Property(e => e.FechaInicio).HasColumnName("fecha_inicio");

            entity.HasOne(d => d.CedulaPacienteNavigation).WithMany(p => p.AsociacionPlanPacientes)
                .HasForeignKey(d => d.CedulaPaciente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("llaves9");

            entity.HasOne(d => d.IdPlanNavigation).WithMany(p => p.AsociacionPlanPacientes)
                .HasPrincipalKey(p => p.Id)
                .HasForeignKey(d => d.IdPlan)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("llaves10");
        });

        modelBuilder.Entity<AsociacionRecetaProducto>(entity =>
        {
            entity.HasKey(e => new { e.IdReceta, e.CodigoBarrasProducto }).HasName("asociacion_receta_producto_pkey");

            entity.ToTable("asociacion_receta_producto");

            entity.Property(e => e.IdReceta).HasColumnName("id_receta");
            entity.Property(e => e.CodigoBarrasProducto).HasColumnName("codigo_barras_producto");
            entity.Property(e => e.PorcionProducto).HasColumnName("porcion_producto");

            entity.HasOne(d => d.CodigoBarrasProductoNavigation).WithMany(p => p.AsociacionRecetaProductos)
                .HasForeignKey(d => d.CodigoBarrasProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("llaves5");

            entity.HasOne(d => d.IdRecetaNavigation).WithMany(p => p.AsociacionRecetaProductos)
                .HasForeignKey(d => d.IdReceta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("llaves4");
        });

        modelBuilder.Entity<Consumo>(entity =>
        {
            entity.HasKey(e => e.CedulaPaciente).HasName("consumo_pkey");

            entity.ToTable("consumo");

            entity.Property(e => e.CedulaPaciente)
                .HasMaxLength(9)
                .HasColumnName("cedula_paciente");
            entity.Property(e => e.Fecha).HasColumnName("fecha");
            entity.Property(e => e.TiempoComida).HasColumnName("tiempo_comida");

            entity.HasOne(d => d.CedulaPacienteNavigation).WithOne(p => p.Consumo)
                .HasForeignKey<Consumo>(d => d.CedulaPaciente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("llavesa");

            entity.HasOne(d => d.TiempoComidaNavigation).WithMany(p => p.Consumos)
                .HasForeignKey(d => d.TiempoComida)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("llavesb");
        });

        modelBuilder.Entity<Medida>(entity =>
        {
            entity.HasKey(e => e.CedulaPaciente).HasName("medidas_pkey");

            entity.ToTable("medidas");

            entity.Property(e => e.CedulaPaciente)
                .HasMaxLength(9)
                .HasColumnName("cedula_paciente");
            entity.Property(e => e.Caderas).HasColumnName("caderas");
            entity.Property(e => e.Cintura).HasColumnName("cintura");
            entity.Property(e => e.Cuello).HasColumnName("cuello");
            entity.Property(e => e.Fecha).HasColumnName("fecha");
            entity.Property(e => e.PorcentajeGrasa).HasColumnName("porcentaje_grasa");
            entity.Property(e => e.PorcentajeMusculo).HasColumnName("porcentaje_musculo");

            entity.HasOne(d => d.CedulaPacienteNavigation).WithOne(p => p.Medida)
                .HasForeignKey<Medida>(d => d.CedulaPaciente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("llavesc");
        });

        modelBuilder.Entity<Nutricionistum>(entity =>
        {
            entity.HasKey(e => e.Cedula).HasName("nutricionista_pkey");

            entity.ToTable("nutricionista");

            entity.HasIndex(e => e.CodigoNutricionista, "nutricionista_codigo_nutricionista_key").IsUnique();

            entity.Property(e => e.Cedula)
                .HasMaxLength(9)
                .HasColumnName("cedula");
            entity.Property(e => e.Apellido1)
                .HasMaxLength(50)
                .HasColumnName("apellido1");
            entity.Property(e => e.Apellido2)
                .HasMaxLength(50)
                .HasColumnName("apellido2");
            entity.Property(e => e.CodigoNutricionista)
                .HasMaxLength(5)
                .HasColumnName("codigo_nutricionista");
            entity.Property(e => e.Contrasena)
                .HasMaxLength(50)
                .HasColumnName("contrasena");
            entity.Property(e => e.Direccion)
                .HasMaxLength(50)
                .HasColumnName("direccion");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Foto).HasColumnName("foto");
            entity.Property(e => e.Imc).HasColumnName("imc");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .HasColumnName("nombre");
            entity.Property(e => e.Peso).HasColumnName("peso");

            entity.HasMany(d => d.CedulaPacientes).WithMany(p => p.CedulaNutris)
                .UsingEntity<Dictionary<string, object>>(
                    "AsociacionPacienteNutri",
                    r => r.HasOne<Paciente>().WithMany()
                        .HasForeignKey("CedulaPaciente")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("llaves3"),
                    l => l.HasOne<Nutricionistum>().WithMany()
                        .HasForeignKey("CedulaNutri")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("llaves2"),
                    j =>
                    {
                        j.HasKey("CedulaNutri", "CedulaPaciente").HasName("asociacion_paciente_nutri_pkey");
                        j.ToTable("asociacion_paciente_nutri");
                        j.IndexerProperty<string>("CedulaNutri")
                            .HasMaxLength(9)
                            .HasColumnName("cedula_nutri");
                        j.IndexerProperty<string>("CedulaPaciente")
                            .HasMaxLength(9)
                            .HasColumnName("cedula_paciente");
                    });
        });

        modelBuilder.Entity<Paciente>(entity =>
        {
            entity.HasKey(e => e.Cedula).HasName("paciente_pkey");

            entity.ToTable("paciente");

            entity.Property(e => e.Cedula)
                .HasMaxLength(9)
                .HasColumnName("cedula");
            entity.Property(e => e.Apellido1)
                .HasMaxLength(50)
                .HasColumnName("apellido1");
            entity.Property(e => e.Apellido2)
                .HasMaxLength(50)
                .HasColumnName("apellido2");
            entity.Property(e => e.ConsumoMaximo).HasColumnName("consumo_maximo");
            entity.Property(e => e.Contrasena)
                .HasMaxLength(50)
                .HasColumnName("contrasena");
            entity.Property(e => e.Direccion)
                .HasMaxLength(50)
                .HasColumnName("direccion");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.FechaNacimiento).HasColumnName("fecha_nacimiento");
            entity.Property(e => e.Imc).HasColumnName("imc");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .HasColumnName("nombre");
            entity.Property(e => e.Pais)
                .HasMaxLength(20)
                .HasColumnName("pais");
            entity.Property(e => e.Peso).HasColumnName("peso");
        });

        modelBuilder.Entity<Plan>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.CedulaNutri }).HasName("plan_pkey");

            entity.ToTable("plan");

            entity.HasIndex(e => e.CedulaNutri, "plan_cedula_nutri_key").IsUnique();

            entity.HasIndex(e => e.Descripcion, "plan_descripcion_key").IsUnique();

            entity.HasIndex(e => e.Id, "plan_id_key").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.CedulaNutri)
                .HasMaxLength(50)
                .HasColumnName("cedula_nutri");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(50)
                .HasColumnName("descripcion");

            entity.HasOne(d => d.CedulaNutriNavigation).WithOne(p => p.Plan)
                .HasForeignKey<Plan>(d => d.CedulaNutri)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("llavesz");

            entity.HasMany(d => d.IdPlans).WithMany(p => p.IdTiempoComida)
                .UsingEntity<Dictionary<string, object>>(
                    "AsociacionPlanTiempocomidum",
                    r => r.HasOne<Plan>().WithMany()
                        .HasPrincipalKey("Id")
                        .HasForeignKey("IdPlan")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("llaves6"),
                    l => l.HasOne<Plan>().WithMany()
                        .HasPrincipalKey("Id")
                        .HasForeignKey("IdTiempoComida")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("llaves7"),
                    j =>
                    {
                        j.HasKey("IdPlan", "IdTiempoComida").HasName("asociacion_plan_tiempocomida_pkey");
                        j.ToTable("asociacion_plan_tiempocomida");
                        j.IndexerProperty<int>("IdPlan").HasColumnName("id_plan");
                        j.IndexerProperty<int>("IdTiempoComida").HasColumnName("id_tiempo_comida");
                    });

            entity.HasMany(d => d.IdTiempoComida).WithMany(p => p.IdPlans)
                .UsingEntity<Dictionary<string, object>>(
                    "AsociacionPlanTiempocomidum",
                    r => r.HasOne<Plan>().WithMany()
                        .HasPrincipalKey("Id")
                        .HasForeignKey("IdTiempoComida")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("llaves7"),
                    l => l.HasOne<Plan>().WithMany()
                        .HasPrincipalKey("Id")
                        .HasForeignKey("IdPlan")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("llaves6"),
                    j =>
                    {
                        j.HasKey("IdPlan", "IdTiempoComida").HasName("asociacion_plan_tiempocomida_pkey");
                        j.ToTable("asociacion_plan_tiempocomida");
                        j.IndexerProperty<int>("IdPlan").HasColumnName("id_plan");
                        j.IndexerProperty<int>("IdTiempoComida").HasColumnName("id_tiempo_comida");
                    });
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.CodigoBarras).HasName("producto_pkey");

            entity.ToTable("producto");

            entity.Property(e => e.CodigoBarras)
                .ValueGeneratedNever()
                .HasColumnName("codigo_barras");
            entity.Property(e => e.Calcio).HasColumnName("calcio");
            entity.Property(e => e.Carbohidrato).HasColumnName("carbohidrato");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(50)
                .HasColumnName("descripcion");
            entity.Property(e => e.Energia).HasColumnName("energia");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.Grasa).HasColumnName("grasa");
            entity.Property(e => e.Hierro).HasColumnName("hierro");
            entity.Property(e => e.Proteina).HasColumnName("proteina");
            entity.Property(e => e.Sodio).HasColumnName("sodio");
        });

        modelBuilder.Entity<Recetum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("receta_pkey");

            entity.ToTable("receta");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(50)
                .HasColumnName("descripcion");
            entity.Property(e => e.Porciones)
                .HasMaxLength(50)
                .HasColumnName("porciones");
        });

        modelBuilder.Entity<TiempoComidum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tiempo_comida_pkey");

            entity.ToTable("tiempo_comida");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<TipoCobro>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tipo_cobro_pkey");

            entity.ToTable("tipo_cobro");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CedulaNutri)
                .HasMaxLength(9)
                .HasColumnName("cedula_nutri");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(50)
                .HasColumnName("descripcion");

            entity.HasOne(d => d.CedulaNutriNavigation).WithMany(p => p.TipoCobros)
                .HasForeignKey(d => d.CedulaNutri)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("llaves");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
