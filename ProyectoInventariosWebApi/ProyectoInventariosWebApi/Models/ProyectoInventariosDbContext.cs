using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ProyectoInventariosWebApi.Models;

public partial class ProyectoInventariosDbContext : DbContext
{
    public ProyectoInventariosDbContext()
    {
    }

    public ProyectoInventariosDbContext(DbContextOptions<ProyectoInventariosDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Clientes> Clientes { get; set; }
    public virtual DbSet<DetallesPedido> DetallesPedido { get; set; }
    public virtual DbSet<Empresas> Empresas { get; set; }
    public virtual DbSet<Entregas> Entregas { get; set; }
    public virtual DbSet<Facturas> Facturas { get; set; }
    public virtual DbSet<Pedidos> Pedidos { get; set; }
    public virtual DbSet<Productos> Productos { get; set; }
    public virtual DbSet<Usuarios> Usuarios { get; set; }

    public virtual DbSet<Sedes> Sedes { get; set; }
    public virtual DbSet<Dependencias> Dependencias { get; set; }
    public virtual DbSet<InventarioDependencia> InventarioDependencia { get; set; }
    public virtual DbSet<TransferenciaStock> TransferenciaStock { get; set; }
    public virtual DbSet<MovimientoInventario> MovimientoInventario { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-C29PHED\\SQLEXPRESS;Database=ProyectoInventariosDB;Trusted_Connection=True;TrustServerCertificate=True;");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Empresas>(entity =>
        {
            entity.HasKey(e => e.IdEmpresa).HasName("PK__Empresas__5EF4033E696756D0");

            entity.Property(e => e.Ciudad).HasMaxLength(100);
            entity.Property(e => e.Direccion).HasMaxLength(150);
            entity.Property(e => e.EmailContacto).HasMaxLength(100);
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Nit).HasMaxLength(20);
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.PaginaWeb).HasMaxLength(150);
            entity.Property(e => e.RepresentanteLegal).HasMaxLength(100);
            entity.Property(e => e.Telefono).HasMaxLength(20);
            entity.Property(e => e.TipoEmpresa).HasMaxLength(50);
        });

        modelBuilder.Entity<Sedes>(entity =>
        {
            entity.HasKey(e => e.IdSede);

            entity.HasIndex(e => e.Codigo).IsUnique();

            entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Codigo).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Direccion).IsRequired().HasMaxLength(150);
            entity.Property(e => e.Telefono).HasMaxLength(20);
            entity.Property(e => e.HorarioLaboral).HasMaxLength(500);
            entity.Property(e => e.EsSedePrincipal).HasDefaultValue(false);
            entity.Property(e => e.Estado).HasDefaultValue(true);
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.IdEmpresaNavigation)
                .WithMany(p => p.Sedes)
                .HasForeignKey(d => d.IdEmpresa)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Dependencias>(entity =>
        {
            entity.HasKey(e => e.IdDependencia);

            entity.Property(e => e.Nombre).IsRequired().HasMaxLength(150);
            entity.Property(e => e.TipoDependencia).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Ubicacion).HasMaxLength(200);
            entity.Property(e => e.Responsable).HasMaxLength(100);
            entity.Property(e => e.TelefonoContacto).HasMaxLength(20);
            entity.Property(e => e.Estado).HasDefaultValue(true);
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.IdSedeNavigation)
                .WithMany(p => p.Dependencias)
                .HasForeignKey(d => d.IdSede)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Productos>(entity =>
        {
            entity.HasKey(e => e.IdProducto).HasName("PK__Producto__098892101FC72D21");

            entity.HasIndex(e => e.Codigo).IsUnique();

            entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Descripcion).HasMaxLength(200);
            entity.Property(e => e.Precio).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Codigo).HasMaxLength(50);
            entity.Property(e => e.UnidadMedida).HasMaxLength(50);
            entity.Property(e => e.Categoria).HasMaxLength(50);
            entity.Property(e => e.EsCompartible).HasDefaultValue(true);
            entity.Property(e => e.Imagen).HasMaxLength(500);
            entity.Property(e => e.Estado).HasDefaultValue(true);
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });


        modelBuilder.Entity<InventarioDependencia>(entity =>
        {
            entity.HasKey(e => e.IdInventario);

            entity.HasIndex(e => new { e.IdProducto, e.IdDependencia }).IsUnique();

            entity.Property(e => e.StockActual).HasDefaultValue(0);
            entity.Property(e => e.StockMinimo).HasDefaultValue(0);
            entity.Property(e => e.StockMaximo).HasDefaultValue(1000);
            entity.Property(e => e.PuntoReorden).HasDefaultValue(10);
            entity.Property(e => e.CostoPromedio).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Ubicacion).HasMaxLength(100);
            entity.Property(e => e.UltimaActualizacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.EstadoInventario)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue("Disponible");

            entity.HasOne(d => d.IdProductoNavigation)
                .WithMany(p => p.InventarioDependencia)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(d => d.IdDependenciaNavigation)
                .WithMany(p => p.InventarioDependencia)
                .HasForeignKey(d => d.IdDependencia)
                .OnDelete(DeleteBehavior.Restrict);
        });


        modelBuilder.Entity<TransferenciaStock>(entity =>
        {
            entity.HasKey(e => e.IdTransferencia);

            entity.Property(e => e.Motivo).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Estado)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue("Pendiente");
            entity.Property(e => e.Observaciones).HasMaxLength(500);
            entity.Property(e => e.CostoTransporte).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.FechaSolicitud)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaAprobacion).HasColumnType("datetime");
            entity.Property(e => e.FechaEjecucion).HasColumnType("datetime");

            entity.HasOne(d => d.IdProductoNavigation)
                .WithMany(p => p.TransferenciaStock)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(d => d.IdDependenciaOrigenNavigation)
                .WithMany(p => p.TransferenciasOrigen)
                .HasForeignKey(d => d.IdDependenciaOrigen)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(d => d.IdDependenciaDestinoNavigation)
                .WithMany(p => p.TransferenciasDestino)
                .HasForeignKey(d => d.IdDependenciaDestino)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(d => d.IdUsuarioSolicitaNavigation)
                .WithMany(p => p.TransferenciasSolicitadas)
                .HasForeignKey(d => d.IdUsuarioSolicita)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(d => d.IdUsuarioApruebaNavigation)
                .WithMany(p => p.TransferenciasAprobadas)
                .HasForeignKey(d => d.IdUsuarioAprueba)
                .OnDelete(DeleteBehavior.Restrict);
        });


        modelBuilder.Entity<MovimientoInventario>(entity =>
        {
            entity.HasKey(e => e.IdMovimiento);

            entity.Property(e => e.TipoMovimiento).IsRequired().HasMaxLength(50);
            entity.Property(e => e.TipoReferencia).HasMaxLength(50);
            entity.Property(e => e.Observaciones).HasMaxLength(500);
            entity.Property(e => e.CostoUnitario).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasIndex(e => e.Fecha);

            entity.HasOne(d => d.IdInventarioNavigation)
                .WithMany(p => p.MovimientoInventario)
                .HasForeignKey(d => d.IdInventario)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(d => d.IdUsuarioNavigation)
                .WithMany(p => p.MovimientoInventario)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.Restrict);
        });


        modelBuilder.Entity<Usuarios>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuarios__5B65BF9734D3DB92");

            entity.HasIndex(e => e.Correo, "UQ__Usuarios__60695A19316662C9").IsUnique();

            entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Correo).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Contrasena).HasMaxLength(255);
            entity.Property(e => e.Rol).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Estado).HasDefaultValue(true);
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UltimoAcceso).HasColumnType("datetime");

            entity.HasOne(d => d.IdSedeNavigation)
                .WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdSede)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(d => d.IdDependenciaNavigation)
                .WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdDependencia)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Clientes>(entity =>
        {
            entity.HasKey(e => e.IdCliente).HasName("PK__Clientes__D59466424E4DA08A");

            entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Telefono).HasMaxLength(20);
            entity.Property(e => e.Direccion).HasMaxLength(150);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.TipoCliente).HasMaxLength(50);
            entity.Property(e => e.DocumentoIdentidad).HasMaxLength(50);
            entity.Property(e => e.Estado).HasDefaultValue(true);
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.IdSedePredeterminadaNavigation)
                .WithMany(p => p.Clientes)
                .HasForeignKey(d => d.IdSedePredeterminada)
                .OnDelete(DeleteBehavior.SetNull);
        });


        modelBuilder.Entity<Pedidos>(entity =>
        {
            entity.HasKey(e => e.IdPedido).HasName("PK__Pedidos__9D335DC3684D74B6");

            entity.Property(e => e.Estado)
                .HasMaxLength(50)
                .HasDefaultValue("Pendiente");
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Total).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TipoEntrega).HasMaxLength(50);
            entity.Property(e => e.MetodoPago).HasMaxLength(50);
            entity.Property(e => e.Observaciones).HasMaxLength(500);
            entity.Property(e => e.FechaEstimadaEntrega).HasColumnType("datetime");

            entity.HasOne(d => d.IdClienteNavigation)
                .WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.IdCliente)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(d => d.IdUsuarioNavigation)
                .WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(d => d.IdSedeNavigation)
                .WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.IdSede)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(d => d.IdDependenciaNavigation)
                .WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.IdDependencia)
                .OnDelete(DeleteBehavior.SetNull);
        });


        modelBuilder.Entity<DetallesPedido>(entity =>
        {
            entity.HasKey(e => e.IdDetalle).HasName("PK__Detalles__E43646A566B314D3");

            entity.Property(e => e.Subtotal).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.PrecioUnitario).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Descuento).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Observaciones).HasMaxLength(200);

            entity.HasOne(d => d.IdPedidoNavigation)
                .WithMany(p => p.DetallesPedido)
                .HasForeignKey(d => d.IdPedido)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.IdProductoNavigation)
                .WithMany(p => p.DetallesPedido)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(d => d.IdInventarioNavigation)
                .WithMany(p => p.DetallesPedido)
                .HasForeignKey(d => d.IdInventario)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Entregas>(entity =>
        {
            entity.HasKey(e => e.IdEntrega).HasName("PK__Entregas__C852F553AFBB5046");

            entity.Property(e => e.DireccionEntrega).HasMaxLength(200);
            entity.Property(e => e.Estado)
                .HasMaxLength(50)
                .HasDefaultValue("Pendiente");
            entity.Property(e => e.FechaEntrega).HasColumnType("datetime");
            entity.Property(e => e.Transportista).HasMaxLength(100);
            entity.Property(e => e.Observaciones).HasMaxLength(500);
            entity.Property(e => e.CostoEnvio).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.IdPedidoNavigation)
                .WithMany(p => p.Entregas)
                .HasForeignKey(d => d.IdPedido)
                .OnDelete(DeleteBehavior.Cascade);
        });


        modelBuilder.Entity<Facturas>(entity =>
        {
            entity.HasKey(e => e.IdFactura).HasName("PK__Facturas__50E7BAF197F5B7DF");

            entity.HasIndex(e => e.NumeroFactura).IsUnique();

            entity.Property(e => e.NumeroFactura).HasMaxLength(50);
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Total).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Subtotal).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Iva).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Descuentos).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.MetodoPago).HasMaxLength(50);
            entity.Property(e => e.EstadoPago)
                .HasMaxLength(50)
                .HasDefaultValue("Pendiente");
            entity.Property(e => e.Observaciones).HasMaxLength(500);

            entity.HasOne(d => d.IdPedidoNavigation)
                .WithMany(p => p.Facturas)
                .HasForeignKey(d => d.IdPedido)
                .OnDelete(DeleteBehavior.Cascade);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}