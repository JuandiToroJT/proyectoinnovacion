using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProyectoInventariosWebApi.Models;

#nullable disable

namespace ProyectoInventariosWebApi.Migrations
{
    [DbContext(typeof(ProyectoInventariosDbContext))]
    partial class ProyectoInventariosDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ProyectoInventariosWebApi.Models.Clientes", b =>
                {
                    b.Property<int>("IdCliente")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdCliente"));

                    b.Property<string>("Direccion")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("DocumentoIdentidad")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Email")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("Estado")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<DateTime?>("FechaRegistro")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<int?>("IdSedePredeterminada")
                        .HasColumnType("int");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Telefono")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("TipoCliente")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("IdCliente")
                        .HasName("PK__Clientes__D59466424E4DA08A");

                    b.HasIndex("IdSedePredeterminada");

                    b.ToTable("Clientes");
                });

            modelBuilder.Entity("ProyectoInventariosWebApi.Models.Dependencias", b =>
                {
                    b.Property<int>("IdDependencia")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdDependencia"));

                    b.Property<bool>("Estado")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<DateTime>("FechaCreacion")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<int>("IdSede")
                        .HasColumnType("int");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("Responsable")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("TelefonoContacto")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("TipoDependencia")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Ubicacion")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("IdDependencia");

                    b.HasIndex("IdSede");

                    b.ToTable("Dependencias");
                });

            modelBuilder.Entity("ProyectoInventariosWebApi.Models.DetallesPedido", b =>
                {
                    b.Property<int>("IdDetalle")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdDetalle"));

                    b.Property<int>("Cantidad")
                        .HasColumnType("int");

                    b.Property<decimal?>("Descuento")
                        .HasColumnType("decimal(10, 2)");

                    b.Property<int?>("IdInventario")
                        .HasColumnType("int");

                    b.Property<int>("IdPedido")
                        .HasColumnType("int");

                    b.Property<int>("IdProducto")
                        .HasColumnType("int");

                    b.Property<string>("Observaciones")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<decimal?>("PrecioUnitario")
                        .HasColumnType("decimal(10, 2)");

                    b.Property<decimal?>("Subtotal")
                        .HasColumnType("decimal(10, 2)");

                    b.HasKey("IdDetalle")
                        .HasName("PK__Detalles__E43646A566B314D3");

                    b.HasIndex("IdInventario");

                    b.HasIndex("IdPedido");

                    b.HasIndex("IdProducto");

                    b.ToTable("DetallesPedido");
                });

            modelBuilder.Entity("ProyectoInventariosWebApi.Models.Empresas", b =>
                {
                    b.Property<int>("IdEmpresa")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdEmpresa"));

                    b.Property<string>("Ciudad")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Direccion")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("EmailContacto")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime?>("FechaCreacion")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<string>("Nit")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("PaginaWeb")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("RepresentanteLegal")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Telefono")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("TipoEmpresa")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("IdEmpresa")
                        .HasName("PK__Empresas__5EF4033E696756D0");

                    b.ToTable("Empresas");
                });

            modelBuilder.Entity("ProyectoInventariosWebApi.Models.Entregas", b =>
                {
                    b.Property<int>("IdEntrega")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdEntrega"));

                    b.Property<decimal?>("CostoEnvio")
                        .HasColumnType("decimal(10, 2)");

                    b.Property<string>("DireccionEntrega")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Estado")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasDefaultValue("Pendiente");

                    b.Property<DateTime?>("FechaEntrega")
                        .HasColumnType("datetime");

                    b.Property<int>("IdPedido")
                        .HasColumnType("int");

                    b.Property<string>("Observaciones")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Transportista")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("IdEntrega")
                        .HasName("PK__Entregas__C852F553AFBB5046");

                    b.HasIndex("IdPedido");

                    b.ToTable("Entregas");
                });

            modelBuilder.Entity("ProyectoInventariosWebApi.Models.Facturas", b =>
                {
                    b.Property<int>("IdFactura")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdFactura"));

                    b.Property<decimal?>("Descuentos")
                        .HasColumnType("decimal(10, 2)");

                    b.Property<string>("EstadoPago")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasDefaultValue("Pendiente");

                    b.Property<DateTime?>("Fecha")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<int>("IdPedido")
                        .HasColumnType("int");

                    b.Property<decimal?>("Iva")
                        .HasColumnType("decimal(10, 2)");

                    b.Property<string>("MetodoPago")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("NumeroFactura")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Observaciones")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<decimal?>("Subtotal")
                        .HasColumnType("decimal(10, 2)");

                    b.Property<decimal?>("Total")
                        .HasColumnType("decimal(10, 2)");

                    b.HasKey("IdFactura")
                        .HasName("PK__Facturas__50E7BAF197F5B7DF");

                    b.HasIndex("IdPedido");

                    b.HasIndex("NumeroFactura")
                        .IsUnique()
                        .HasFilter("[NumeroFactura] IS NOT NULL");

                    b.ToTable("Facturas");
                });

            modelBuilder.Entity("ProyectoInventariosWebApi.Models.InventarioDependencia", b =>
                {
                    b.Property<int>("IdInventario")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdInventario"));

                    b.Property<decimal>("CostoPromedio")
                        .HasColumnType("decimal(10, 2)");

                    b.Property<string>("EstadoInventario")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasDefaultValue("Disponible");

                    b.Property<int>("IdDependencia")
                        .HasColumnType("int");

                    b.Property<int>("IdProducto")
                        .HasColumnType("int");

                    b.Property<int>("PuntoReorden")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(10);

                    b.Property<int>("StockActual")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<int>("StockMaximo")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(1000);

                    b.Property<int>("StockMinimo")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<string>("Ubicacion")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("UltimaActualizacion")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.HasKey("IdInventario");

                    b.HasIndex("IdDependencia");

                    b.HasIndex("IdProducto", "IdDependencia")
                        .IsUnique();

                    b.ToTable("InventarioDependencia");
                });

            modelBuilder.Entity("ProyectoInventariosWebApi.Models.MovimientoInventario", b =>
                {
                    b.Property<int>("IdMovimiento")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdMovimiento"));

                    b.Property<int>("Cantidad")
                        .HasColumnType("int");

                    b.Property<decimal?>("CostoUnitario")
                        .HasColumnType("decimal(10, 2)");

                    b.Property<DateTime>("Fecha")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<int>("IdInventario")
                        .HasColumnType("int");

                    b.Property<int?>("IdReferencia")
                        .HasColumnType("int");

                    b.Property<int>("IdUsuario")
                        .HasColumnType("int");

                    b.Property<string>("Observaciones")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<int>("StockAnterior")
                        .HasColumnType("int");

                    b.Property<int>("StockNuevo")
                        .HasColumnType("int");

                    b.Property<string>("TipoMovimiento")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("TipoReferencia")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("IdMovimiento");

                    b.HasIndex("Fecha");

                    b.HasIndex("IdInventario");

                    b.HasIndex("IdUsuario");

                    b.ToTable("MovimientoInventario");
                });

            modelBuilder.Entity("ProyectoInventariosWebApi.Models.Pedidos", b =>
                {
                    b.Property<int>("IdPedido")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdPedido"));

                    b.Property<string>("Estado")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasDefaultValue("Pendiente");

                    b.Property<DateTime?>("Fecha")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<DateTime?>("FechaEstimadaEntrega")
                        .HasColumnType("datetime");

                    b.Property<int>("IdCliente")
                        .HasColumnType("int");

                    b.Property<int?>("IdDependencia")
                        .HasColumnType("int");

                    b.Property<int?>("IdSede")
                        .HasColumnType("int");

                    b.Property<int?>("IdUsuario")
                        .HasColumnType("int");

                    b.Property<string>("MetodoPago")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Observaciones")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("TipoEntrega")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<decimal?>("Total")
                        .HasColumnType("decimal(10, 2)");

                    b.HasKey("IdPedido")
                        .HasName("PK__Pedidos__9D335DC3684D74B6");

                    b.HasIndex("IdCliente");

                    b.HasIndex("IdDependencia");

                    b.HasIndex("IdSede");

                    b.HasIndex("IdUsuario");

                    b.ToTable("Pedidos");
                });

            modelBuilder.Entity("ProyectoInventariosWebApi.Models.Productos", b =>
                {
                    b.Property<int>("IdProducto")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdProducto"));

                    b.Property<string>("Categoria")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Codigo")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int?>("DiasVidaUtil")
                        .HasColumnType("int");

                    b.Property<bool>("EsCompartible")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<bool>("Estado")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<DateTime?>("FechaCreacion")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<string>("Imagen")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<decimal>("Precio")
                        .HasColumnType("decimal(10, 2)");

                    b.Property<bool?>("RequiereRefrigeracion")
                        .HasColumnType("bit");

                    b.Property<int>("Stock")
                        .HasColumnType("int");

                    b.Property<int?>("StockMinimoGlobal")
                        .HasColumnType("int");

                    b.Property<string>("UnidadMedida")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("IdProducto")
                        .HasName("PK__Producto__098892101FC72D21");

                    b.HasIndex("Codigo")
                        .IsUnique()
                        .HasFilter("[Codigo] IS NOT NULL");

                    b.ToTable("Productos");
                });

            modelBuilder.Entity("ProyectoInventariosWebApi.Models.Sedes", b =>
                {
                    b.Property<int>("IdSede")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdSede"));

                    b.Property<string>("Codigo")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Direccion")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<bool>("EsSedePrincipal")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<bool>("Estado")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<DateTime>("FechaCreacion")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<string>("HorarioLaboral")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<int>("IdEmpresa")
                        .HasColumnType("int");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Telefono")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("IdSede");

                    b.HasIndex("Codigo")
                        .IsUnique();

                    b.HasIndex("IdEmpresa");

                    b.ToTable("Sedes");
                });

            modelBuilder.Entity("ProyectoInventariosWebApi.Models.TransferenciaStock", b =>
                {
                    b.Property<int>("IdTransferencia")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdTransferencia"));

                    b.Property<int>("Cantidad")
                        .HasColumnType("int");

                    b.Property<decimal?>("CostoTransporte")
                        .HasColumnType("decimal(10, 2)");

                    b.Property<string>("Estado")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasDefaultValue("Pendiente");

                    b.Property<DateTime?>("FechaAprobacion")
                        .HasColumnType("datetime");

                    b.Property<DateTime?>("FechaEjecucion")
                        .HasColumnType("datetime");

                    b.Property<DateTime>("FechaSolicitud")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<int>("IdDependenciaDestino")
                        .HasColumnType("int");

                    b.Property<int>("IdDependenciaOrigen")
                        .HasColumnType("int");

                    b.Property<int>("IdProducto")
                        .HasColumnType("int");

                    b.Property<int?>("IdUsuarioAprueba")
                        .HasColumnType("int");

                    b.Property<int>("IdUsuarioSolicita")
                        .HasColumnType("int");

                    b.Property<string>("Motivo")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Observaciones")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.HasKey("IdTransferencia");

                    b.HasIndex("IdDependenciaDestino");

                    b.HasIndex("IdDependenciaOrigen");

                    b.HasIndex("IdProducto");

                    b.HasIndex("IdUsuarioAprueba");

                    b.HasIndex("IdUsuarioSolicita");

                    b.ToTable("TransferenciaStock");
                });

            modelBuilder.Entity("ProyectoInventariosWebApi.Models.Usuarios", b =>
                {
                    b.Property<int>("IdUsuario")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdUsuario"));

                    b.Property<string>("Contrasena")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Correo")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("Estado")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<DateTime?>("FechaCreacion")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<int?>("IdDependencia")
                        .HasColumnType("int");

                    b.Property<int?>("IdSede")
                        .HasColumnType("int");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Rol")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime?>("UltimoAcceso")
                        .HasColumnType("datetime");

                    b.HasKey("IdUsuario")
                        .HasName("PK__Usuarios__5B65BF9734D3DB92");

                    b.HasIndex("IdDependencia");

                    b.HasIndex("IdSede");

                    b.HasIndex(new[] { "Correo" }, "UQ__Usuarios__60695A19316662C9")
                        .IsUnique();

                    b.ToTable("Usuarios");
                });

            modelBuilder.Entity("ProyectoInventariosWebApi.Models.Clientes", b =>
                {
                    b.HasOne("ProyectoInventariosWebApi.Models.Sedes", "IdSedePredeterminadaNavigation")
                        .WithMany("Clientes")
                        .HasForeignKey("IdSedePredeterminada")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("IdSedePredeterminadaNavigation");
                });

            modelBuilder.Entity("ProyectoInventariosWebApi.Models.Dependencias", b =>
                {
                    b.HasOne("ProyectoInventariosWebApi.Models.Sedes", "IdSedeNavigation")
                        .WithMany("Dependencias")
                        .HasForeignKey("IdSede")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("IdSedeNavigation");
                });

            modelBuilder.Entity("ProyectoInventariosWebApi.Models.DetallesPedido", b =>
                {
                    b.HasOne("ProyectoInventariosWebApi.Models.InventarioDependencia", "IdInventarioNavigation")
                        .WithMany("DetallesPedido")
                        .HasForeignKey("IdInventario")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("ProyectoInventariosWebApi.Models.Pedidos", "IdPedidoNavigation")
                        .WithMany("DetallesPedido")
                        .HasForeignKey("IdPedido")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProyectoInventariosWebApi.Models.Productos", "IdProductoNavigation")
                        .WithMany("DetallesPedido")
                        .HasForeignKey("IdProducto")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("IdInventarioNavigation");

                    b.Navigation("IdPedidoNavigation");

                    b.Navigation("IdProductoNavigation");
                });

            modelBuilder.Entity("ProyectoInventariosWebApi.Models.Entregas", b =>
                {
                    b.HasOne("ProyectoInventariosWebApi.Models.Pedidos", "IdPedidoNavigation")
                        .WithMany("Entregas")
                        .HasForeignKey("IdPedido")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("IdPedidoNavigation");
                });

            modelBuilder.Entity("ProyectoInventariosWebApi.Models.Facturas", b =>
                {
                    b.HasOne("ProyectoInventariosWebApi.Models.Pedidos", "IdPedidoNavigation")
                        .WithMany("Facturas")
                        .HasForeignKey("IdPedido")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("IdPedidoNavigation");
                });

            modelBuilder.Entity("ProyectoInventariosWebApi.Models.InventarioDependencia", b =>
                {
                    b.HasOne("ProyectoInventariosWebApi.Models.Dependencias", "IdDependenciaNavigation")
                        .WithMany("InventarioDependencia")
                        .HasForeignKey("IdDependencia")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("ProyectoInventariosWebApi.Models.Productos", "IdProductoNavigation")
                        .WithMany("InventarioDependencia")
                        .HasForeignKey("IdProducto")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("IdDependenciaNavigation");

                    b.Navigation("IdProductoNavigation");
                });

            modelBuilder.Entity("ProyectoInventariosWebApi.Models.MovimientoInventario", b =>
                {
                    b.HasOne("ProyectoInventariosWebApi.Models.InventarioDependencia", "IdInventarioNavigation")
                        .WithMany("MovimientoInventario")
                        .HasForeignKey("IdInventario")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("ProyectoInventariosWebApi.Models.Usuarios", "IdUsuarioNavigation")
                        .WithMany("MovimientoInventario")
                        .HasForeignKey("IdUsuario")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("IdInventarioNavigation");

                    b.Navigation("IdUsuarioNavigation");
                });

            modelBuilder.Entity("ProyectoInventariosWebApi.Models.Pedidos", b =>
                {
                    b.HasOne("ProyectoInventariosWebApi.Models.Clientes", "IdClienteNavigation")
                        .WithMany("Pedidos")
                        .HasForeignKey("IdCliente")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("ProyectoInventariosWebApi.Models.Dependencias", "IdDependenciaNavigation")
                        .WithMany("Pedidos")
                        .HasForeignKey("IdDependencia")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("ProyectoInventariosWebApi.Models.Sedes", "IdSedeNavigation")
                        .WithMany("Pedidos")
                        .HasForeignKey("IdSede")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("ProyectoInventariosWebApi.Models.Usuarios", "IdUsuarioNavigation")
                        .WithMany("Pedidos")
                        .HasForeignKey("IdUsuario")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("IdClienteNavigation");

                    b.Navigation("IdDependenciaNavigation");

                    b.Navigation("IdSedeNavigation");

                    b.Navigation("IdUsuarioNavigation");
                });

            modelBuilder.Entity("ProyectoInventariosWebApi.Models.Sedes", b =>
                {
                    b.HasOne("ProyectoInventariosWebApi.Models.Empresas", "IdEmpresaNavigation")
                        .WithMany("Sedes")
                        .HasForeignKey("IdEmpresa")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("IdEmpresaNavigation");
                });

            modelBuilder.Entity("ProyectoInventariosWebApi.Models.TransferenciaStock", b =>
                {
                    b.HasOne("ProyectoInventariosWebApi.Models.Dependencias", "IdDependenciaDestinoNavigation")
                        .WithMany("TransferenciasDestino")
                        .HasForeignKey("IdDependenciaDestino")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("ProyectoInventariosWebApi.Models.Dependencias", "IdDependenciaOrigenNavigation")
                        .WithMany("TransferenciasOrigen")
                        .HasForeignKey("IdDependenciaOrigen")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("ProyectoInventariosWebApi.Models.Productos", "IdProductoNavigation")
                        .WithMany("TransferenciaStock")
                        .HasForeignKey("IdProducto")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("ProyectoInventariosWebApi.Models.Usuarios", "IdUsuarioApruebaNavigation")
                        .WithMany("TransferenciasAprobadas")
                        .HasForeignKey("IdUsuarioAprueba")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("ProyectoInventariosWebApi.Models.Usuarios", "IdUsuarioSolicitaNavigation")
                        .WithMany("TransferenciasSolicitadas")
                        .HasForeignKey("IdUsuarioSolicita")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("IdDependenciaDestinoNavigation");

                    b.Navigation("IdDependenciaOrigenNavigation");

                    b.Navigation("IdProductoNavigation");

                    b.Navigation("IdUsuarioApruebaNavigation");

                    b.Navigation("IdUsuarioSolicitaNavigation");
                });

            modelBuilder.Entity("ProyectoInventariosWebApi.Models.Usuarios", b =>
                {
                    b.HasOne("ProyectoInventariosWebApi.Models.Dependencias", "IdDependenciaNavigation")
                        .WithMany("Usuarios")
                        .HasForeignKey("IdDependencia")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("ProyectoInventariosWebApi.Models.Sedes", "IdSedeNavigation")
                        .WithMany("Usuarios")
                        .HasForeignKey("IdSede")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("IdDependenciaNavigation");

                    b.Navigation("IdSedeNavigation");
                });

            modelBuilder.Entity("ProyectoInventariosWebApi.Models.Clientes", b =>
                {
                    b.Navigation("Pedidos");
                });

            modelBuilder.Entity("ProyectoInventariosWebApi.Models.Dependencias", b =>
                {
                    b.Navigation("InventarioDependencia");

                    b.Navigation("Pedidos");

                    b.Navigation("TransferenciasDestino");

                    b.Navigation("TransferenciasOrigen");

                    b.Navigation("Usuarios");
                });

            modelBuilder.Entity("ProyectoInventariosWebApi.Models.Empresas", b =>
                {
                    b.Navigation("Sedes");
                });

            modelBuilder.Entity("ProyectoInventariosWebApi.Models.InventarioDependencia", b =>
                {
                    b.Navigation("DetallesPedido");

                    b.Navigation("MovimientoInventario");
                });

            modelBuilder.Entity("ProyectoInventariosWebApi.Models.Pedidos", b =>
                {
                    b.Navigation("DetallesPedido");

                    b.Navigation("Entregas");

                    b.Navigation("Facturas");
                });

            modelBuilder.Entity("ProyectoInventariosWebApi.Models.Productos", b =>
                {
                    b.Navigation("DetallesPedido");

                    b.Navigation("InventarioDependencia");

                    b.Navigation("TransferenciaStock");
                });

            modelBuilder.Entity("ProyectoInventariosWebApi.Models.Sedes", b =>
                {
                    b.Navigation("Clientes");

                    b.Navigation("Dependencias");

                    b.Navigation("Pedidos");

                    b.Navigation("Usuarios");
                });

            modelBuilder.Entity("ProyectoInventariosWebApi.Models.Usuarios", b =>
                {
                    b.Navigation("MovimientoInventario");

                    b.Navigation("Pedidos");

                    b.Navigation("TransferenciasAprobadas");

                    b.Navigation("TransferenciasSolicitadas");
                });
#pragma warning restore 612, 618
        }
    }
}
