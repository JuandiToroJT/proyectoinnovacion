using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoInventariosWebApi.Migrations
{
    /// <inheritdoc />
    public partial class AgregarSistemaMultiSede : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__DetallesP__IdPed__49C3F6B7",
                table: "DetallesPedido");

            migrationBuilder.DropForeignKey(
                name: "FK__DetallesP__IdPro__4AB81AF0",
                table: "DetallesPedido");

            migrationBuilder.DropForeignKey(
                name: "FK__Entregas__IdPedi__4E88ABD4",
                table: "Entregas");

            migrationBuilder.DropForeignKey(
                name: "FK__Facturas__IdPedi__52593CB8",
                table: "Facturas");

            migrationBuilder.DropForeignKey(
                name: "FK__Pedidos__IdClien__45F365D3",
                table: "Pedidos");

            migrationBuilder.DropForeignKey(
                name: "FK__Pedidos__IdUsuar__46E78A0C",
                table: "Pedidos");

            migrationBuilder.AlterColumn<string>(
                name: "Rol",
                table: "Usuarios",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCreacion",
                table: "Usuarios",
                type: "datetime",
                nullable: true,
                defaultValueSql: "(getdate())");

            migrationBuilder.AddColumn<int>(
                name: "IdDependencia",
                table: "Usuarios",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdSede",
                table: "Usuarios",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UltimoAcceso",
                table: "Usuarios",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Categoria",
                table: "Productos",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Codigo",
                table: "Productos",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DiasVidaUtil",
                table: "Productos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EsCompartible",
                table: "Productos",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "Estado",
                table: "Productos",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCreacion",
                table: "Productos",
                type: "datetime",
                nullable: true,
                defaultValueSql: "(getdate())");

            migrationBuilder.AddColumn<string>(
                name: "Imagen",
                table: "Productos",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "RequiereRefrigeracion",
                table: "Productos",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StockMinimoGlobal",
                table: "Productos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UnidadMedida",
                table: "Productos",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Estado",
                table: "Pedidos",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                defaultValue: "Pendiente",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true,
                oldDefaultValue: "Pendiente");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaEstimadaEntrega",
                table: "Pedidos",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdDependencia",
                table: "Pedidos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdSede",
                table: "Pedidos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MetodoPago",
                table: "Pedidos",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Observaciones",
                table: "Pedidos",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TipoEntrega",
                table: "Pedidos",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Total",
                table: "Pedidos",
                type: "decimal(10,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Descuentos",
                table: "Facturas",
                type: "decimal(10,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EstadoPago",
                table: "Facturas",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                defaultValue: "Pendiente");

            migrationBuilder.AddColumn<decimal>(
                name: "Iva",
                table: "Facturas",
                type: "decimal(10,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MetodoPago",
                table: "Facturas",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NumeroFactura",
                table: "Facturas",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Observaciones",
                table: "Facturas",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Subtotal",
                table: "Facturas",
                type: "decimal(10,2)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Estado",
                table: "Entregas",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                defaultValue: "Pendiente",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true,
                oldDefaultValue: "Programado");

            migrationBuilder.AddColumn<decimal>(
                name: "CostoEnvio",
                table: "Entregas",
                type: "decimal(10,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Observaciones",
                table: "Entregas",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Transportista",
                table: "Entregas",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Descuento",
                table: "DetallesPedido",
                type: "decimal(10,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdInventario",
                table: "DetallesPedido",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Observaciones",
                table: "DetallesPedido",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PrecioUnitario",
                table: "DetallesPedido",
                type: "decimal(10,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DocumentoIdentidad",
                table: "Clientes",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Clientes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Estado",
                table: "Clientes",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaRegistro",
                table: "Clientes",
                type: "datetime",
                nullable: true,
                defaultValueSql: "(getdate())");

            migrationBuilder.AddColumn<int>(
                name: "IdSedePredeterminada",
                table: "Clientes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TipoCliente",
                table: "Clientes",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Sedes",
                columns: table => new
                {
                    IdSede = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdEmpresa = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Codigo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Direccion = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    HorarioLaboral = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    EsSedePrincipal = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Estado = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sedes", x => x.IdSede);
                    table.ForeignKey(
                        name: "FK_Sedes_Empresas_IdEmpresa",
                        column: x => x.IdEmpresa,
                        principalTable: "Empresas",
                        principalColumn: "IdEmpresa",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Dependencias",
                columns: table => new
                {
                    IdDependencia = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdSede = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    TipoDependencia = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Ubicacion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Responsable = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TelefonoContacto = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Estado = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dependencias", x => x.IdDependencia);
                    table.ForeignKey(
                        name: "FK_Dependencias_Sedes_IdSede",
                        column: x => x.IdSede,
                        principalTable: "Sedes",
                        principalColumn: "IdSede",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InventarioDependencia",
                columns: table => new
                {
                    IdInventario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdProducto = table.Column<int>(type: "int", nullable: false),
                    IdDependencia = table.Column<int>(type: "int", nullable: false),
                    StockActual = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    StockMinimo = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    StockMaximo = table.Column<int>(type: "int", nullable: false, defaultValue: 1000),
                    PuntoReorden = table.Column<int>(type: "int", nullable: false, defaultValue: 10),
                    CostoPromedio = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Ubicacion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UltimaActualizacion = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    EstadoInventario = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Disponible")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventarioDependencia", x => x.IdInventario);
                    table.ForeignKey(
                        name: "FK_InventarioDependencia_Dependencias_IdDependencia",
                        column: x => x.IdDependencia,
                        principalTable: "Dependencias",
                        principalColumn: "IdDependencia",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InventarioDependencia_Productos_IdProducto",
                        column: x => x.IdProducto,
                        principalTable: "Productos",
                        principalColumn: "IdProducto",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransferenciaStock",
                columns: table => new
                {
                    IdTransferencia = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdProducto = table.Column<int>(type: "int", nullable: false),
                    IdDependenciaOrigen = table.Column<int>(type: "int", nullable: false),
                    IdDependenciaDestino = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    Motivo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FechaSolicitud = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    FechaAprobacion = table.Column<DateTime>(type: "datetime", nullable: true),
                    FechaEjecucion = table.Column<DateTime>(type: "datetime", nullable: true),
                    IdUsuarioSolicita = table.Column<int>(type: "int", nullable: false),
                    IdUsuarioAprueba = table.Column<int>(type: "int", nullable: true),
                    Estado = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Pendiente"),
                    Observaciones = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CostoTransporte = table.Column<decimal>(type: "decimal(10,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransferenciaStock", x => x.IdTransferencia);
                    table.ForeignKey(
                        name: "FK_TransferenciaStock_Dependencias_IdDependenciaDestino",
                        column: x => x.IdDependenciaDestino,
                        principalTable: "Dependencias",
                        principalColumn: "IdDependencia",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransferenciaStock_Dependencias_IdDependenciaOrigen",
                        column: x => x.IdDependenciaOrigen,
                        principalTable: "Dependencias",
                        principalColumn: "IdDependencia",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransferenciaStock_Productos_IdProducto",
                        column: x => x.IdProducto,
                        principalTable: "Productos",
                        principalColumn: "IdProducto",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransferenciaStock_Usuarios_IdUsuarioAprueba",
                        column: x => x.IdUsuarioAprueba,
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransferenciaStock_Usuarios_IdUsuarioSolicita",
                        column: x => x.IdUsuarioSolicita,
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MovimientoInventario",
                columns: table => new
                {
                    IdMovimiento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdInventario = table.Column<int>(type: "int", nullable: false),
                    TipoMovimiento = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    StockAnterior = table.Column<int>(type: "int", nullable: false),
                    StockNuevo = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    TipoReferencia = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IdReferencia = table.Column<int>(type: "int", nullable: true),
                    Observaciones = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CostoUnitario = table.Column<decimal>(type: "decimal(10,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovimientoInventario", x => x.IdMovimiento);
                    table.ForeignKey(
                        name: "FK_MovimientoInventario_InventarioDependencia_IdInventario",
                        column: x => x.IdInventario,
                        principalTable: "InventarioDependencia",
                        principalColumn: "IdInventario",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MovimientoInventario_Usuarios_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_IdDependencia",
                table: "Usuarios",
                column: "IdDependencia");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_IdSede",
                table: "Usuarios",
                column: "IdSede");

            migrationBuilder.CreateIndex(
                name: "IX_Productos_Codigo",
                table: "Productos",
                column: "Codigo",
                unique: true,
                filter: "[Codigo] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_IdDependencia",
                table: "Pedidos",
                column: "IdDependencia");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_IdSede",
                table: "Pedidos",
                column: "IdSede");

            migrationBuilder.CreateIndex(
                name: "IX_Facturas_NumeroFactura",
                table: "Facturas",
                column: "NumeroFactura",
                unique: true,
                filter: "[NumeroFactura] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesPedido_IdInventario",
                table: "DetallesPedido",
                column: "IdInventario");

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_IdSedePredeterminada",
                table: "Clientes",
                column: "IdSedePredeterminada");

            migrationBuilder.CreateIndex(
                name: "IX_Dependencias_IdSede",
                table: "Dependencias",
                column: "IdSede");

            migrationBuilder.CreateIndex(
                name: "IX_InventarioDependencia_IdDependencia",
                table: "InventarioDependencia",
                column: "IdDependencia");

            migrationBuilder.CreateIndex(
                name: "IX_InventarioDependencia_IdProducto_IdDependencia",
                table: "InventarioDependencia",
                columns: new[] { "IdProducto", "IdDependencia" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MovimientoInventario_Fecha",
                table: "MovimientoInventario",
                column: "Fecha");

            migrationBuilder.CreateIndex(
                name: "IX_MovimientoInventario_IdInventario",
                table: "MovimientoInventario",
                column: "IdInventario");

            migrationBuilder.CreateIndex(
                name: "IX_MovimientoInventario_IdUsuario",
                table: "MovimientoInventario",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Sedes_Codigo",
                table: "Sedes",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sedes_IdEmpresa",
                table: "Sedes",
                column: "IdEmpresa");

            migrationBuilder.CreateIndex(
                name: "IX_TransferenciaStock_IdDependenciaDestino",
                table: "TransferenciaStock",
                column: "IdDependenciaDestino");

            migrationBuilder.CreateIndex(
                name: "IX_TransferenciaStock_IdDependenciaOrigen",
                table: "TransferenciaStock",
                column: "IdDependenciaOrigen");

            migrationBuilder.CreateIndex(
                name: "IX_TransferenciaStock_IdProducto",
                table: "TransferenciaStock",
                column: "IdProducto");

            migrationBuilder.CreateIndex(
                name: "IX_TransferenciaStock_IdUsuarioAprueba",
                table: "TransferenciaStock",
                column: "IdUsuarioAprueba");

            migrationBuilder.CreateIndex(
                name: "IX_TransferenciaStock_IdUsuarioSolicita",
                table: "TransferenciaStock",
                column: "IdUsuarioSolicita");

            migrationBuilder.AddForeignKey(
                name: "FK_Clientes_Sedes_IdSedePredeterminada",
                table: "Clientes",
                column: "IdSedePredeterminada",
                principalTable: "Sedes",
                principalColumn: "IdSede",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_DetallesPedido_InventarioDependencia_IdInventario",
                table: "DetallesPedido",
                column: "IdInventario",
                principalTable: "InventarioDependencia",
                principalColumn: "IdInventario",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_DetallesPedido_Pedidos_IdPedido",
                table: "DetallesPedido",
                column: "IdPedido",
                principalTable: "Pedidos",
                principalColumn: "IdPedido",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DetallesPedido_Productos_IdProducto",
                table: "DetallesPedido",
                column: "IdProducto",
                principalTable: "Productos",
                principalColumn: "IdProducto",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Entregas_Pedidos_IdPedido",
                table: "Entregas",
                column: "IdPedido",
                principalTable: "Pedidos",
                principalColumn: "IdPedido",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Facturas_Pedidos_IdPedido",
                table: "Facturas",
                column: "IdPedido",
                principalTable: "Pedidos",
                principalColumn: "IdPedido",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_Clientes_IdCliente",
                table: "Pedidos",
                column: "IdCliente",
                principalTable: "Clientes",
                principalColumn: "IdCliente",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_Dependencias_IdDependencia",
                table: "Pedidos",
                column: "IdDependencia",
                principalTable: "Dependencias",
                principalColumn: "IdDependencia",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_Sedes_IdSede",
                table: "Pedidos",
                column: "IdSede",
                principalTable: "Sedes",
                principalColumn: "IdSede",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_Usuarios_IdUsuario",
                table: "Pedidos",
                column: "IdUsuario",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Dependencias_IdDependencia",
                table: "Usuarios",
                column: "IdDependencia",
                principalTable: "Dependencias",
                principalColumn: "IdDependencia",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Sedes_IdSede",
                table: "Usuarios",
                column: "IdSede",
                principalTable: "Sedes",
                principalColumn: "IdSede",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clientes_Sedes_IdSedePredeterminada",
                table: "Clientes");

            migrationBuilder.DropForeignKey(
                name: "FK_DetallesPedido_InventarioDependencia_IdInventario",
                table: "DetallesPedido");

            migrationBuilder.DropForeignKey(
                name: "FK_DetallesPedido_Pedidos_IdPedido",
                table: "DetallesPedido");

            migrationBuilder.DropForeignKey(
                name: "FK_DetallesPedido_Productos_IdProducto",
                table: "DetallesPedido");

            migrationBuilder.DropForeignKey(
                name: "FK_Entregas_Pedidos_IdPedido",
                table: "Entregas");

            migrationBuilder.DropForeignKey(
                name: "FK_Facturas_Pedidos_IdPedido",
                table: "Facturas");

            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_Clientes_IdCliente",
                table: "Pedidos");

            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_Dependencias_IdDependencia",
                table: "Pedidos");

            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_Sedes_IdSede",
                table: "Pedidos");

            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_Usuarios_IdUsuario",
                table: "Pedidos");

            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Dependencias_IdDependencia",
                table: "Usuarios");

            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Sedes_IdSede",
                table: "Usuarios");

            migrationBuilder.DropTable(
                name: "MovimientoInventario");

            migrationBuilder.DropTable(
                name: "TransferenciaStock");

            migrationBuilder.DropTable(
                name: "InventarioDependencia");

            migrationBuilder.DropTable(
                name: "Dependencias");

            migrationBuilder.DropTable(
                name: "Sedes");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_IdDependencia",
                table: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_IdSede",
                table: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Productos_Codigo",
                table: "Productos");

            migrationBuilder.DropIndex(
                name: "IX_Pedidos_IdDependencia",
                table: "Pedidos");

            migrationBuilder.DropIndex(
                name: "IX_Pedidos_IdSede",
                table: "Pedidos");

            migrationBuilder.DropIndex(
                name: "IX_Facturas_NumeroFactura",
                table: "Facturas");

            migrationBuilder.DropIndex(
                name: "IX_DetallesPedido_IdInventario",
                table: "DetallesPedido");

            migrationBuilder.DropIndex(
                name: "IX_Clientes_IdSedePredeterminada",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "FechaCreacion",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "IdDependencia",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "IdSede",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "UltimoAcceso",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "Categoria",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "Codigo",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "DiasVidaUtil",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "EsCompartible",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "FechaCreacion",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "Imagen",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "RequiereRefrigeracion",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "StockMinimoGlobal",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "UnidadMedida",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "FechaEstimadaEntrega",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "IdDependencia",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "IdSede",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "MetodoPago",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "Observaciones",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "TipoEntrega",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "Total",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "Descuentos",
                table: "Facturas");

            migrationBuilder.DropColumn(
                name: "EstadoPago",
                table: "Facturas");

            migrationBuilder.DropColumn(
                name: "Iva",
                table: "Facturas");

            migrationBuilder.DropColumn(
                name: "MetodoPago",
                table: "Facturas");

            migrationBuilder.DropColumn(
                name: "NumeroFactura",
                table: "Facturas");

            migrationBuilder.DropColumn(
                name: "Observaciones",
                table: "Facturas");

            migrationBuilder.DropColumn(
                name: "Subtotal",
                table: "Facturas");

            migrationBuilder.DropColumn(
                name: "CostoEnvio",
                table: "Entregas");

            migrationBuilder.DropColumn(
                name: "Observaciones",
                table: "Entregas");

            migrationBuilder.DropColumn(
                name: "Transportista",
                table: "Entregas");

            migrationBuilder.DropColumn(
                name: "Descuento",
                table: "DetallesPedido");

            migrationBuilder.DropColumn(
                name: "IdInventario",
                table: "DetallesPedido");

            migrationBuilder.DropColumn(
                name: "Observaciones",
                table: "DetallesPedido");

            migrationBuilder.DropColumn(
                name: "PrecioUnitario",
                table: "DetallesPedido");

            migrationBuilder.DropColumn(
                name: "DocumentoIdentidad",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "FechaRegistro",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "IdSedePredeterminada",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "TipoCliente",
                table: "Clientes");

            migrationBuilder.AlterColumn<string>(
                name: "Rol",
                table: "Usuarios",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Estado",
                table: "Pedidos",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                defaultValue: "Pendiente",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true,
                oldDefaultValue: "Pendiente");

            migrationBuilder.AlterColumn<string>(
                name: "Estado",
                table: "Entregas",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                defaultValue: "Programado",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true,
                oldDefaultValue: "Pendiente");

            migrationBuilder.AddForeignKey(
                name: "FK__DetallesP__IdPed__49C3F6B7",
                table: "DetallesPedido",
                column: "IdPedido",
                principalTable: "Pedidos",
                principalColumn: "IdPedido");

            migrationBuilder.AddForeignKey(
                name: "FK__DetallesP__IdPro__4AB81AF0",
                table: "DetallesPedido",
                column: "IdProducto",
                principalTable: "Productos",
                principalColumn: "IdProducto");

            migrationBuilder.AddForeignKey(
                name: "FK__Entregas__IdPedi__4E88ABD4",
                table: "Entregas",
                column: "IdPedido",
                principalTable: "Pedidos",
                principalColumn: "IdPedido");

            migrationBuilder.AddForeignKey(
                name: "FK__Facturas__IdPedi__52593CB8",
                table: "Facturas",
                column: "IdPedido",
                principalTable: "Pedidos",
                principalColumn: "IdPedido");

            migrationBuilder.AddForeignKey(
                name: "FK__Pedidos__IdClien__45F365D3",
                table: "Pedidos",
                column: "IdCliente",
                principalTable: "Clientes",
                principalColumn: "IdCliente");

            migrationBuilder.AddForeignKey(
                name: "FK__Pedidos__IdUsuar__46E78A0C",
                table: "Pedidos",
                column: "IdUsuario",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario");
        }
    }
}
