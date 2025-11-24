-- =====================================================
-- SCRIPT DE POBLADO COMPLETO
-- Sistema de Inventario Multi-Sede
-- Universidad de Caldas
-- =====================================================

USE ProyectoInventariosDB;
GO

-- =====================================================
-- 1. EMPRESAS
-- =====================================================
SET IDENTITY_INSERT Empresas ON;

IF NOT EXISTS (SELECT 1 FROM Empresas WHERE IdEmpresa = 1)
BEGIN
    INSERT INTO Empresas (IdEmpresa, Nombre, Nit, Ciudad, Direccion, Telefono, EmailContacto, PaginaWeb, RepresentanteLegal, TipoEmpresa, FechaCreacion)
    VALUES (
        1,
        'Universidad de Caldas',
        '890800640-1',
        'Manizales',
        'Calle 65 No 26-10',
        '6068781500',
        'rectoría@ucaldas.edu.co',
        'https://www.ucaldas.edu.co',
        'Dr. Fabio Hernando Arias Orozco',
        'Universidad Pública',
        GETDATE()
    );
    PRINT '✓ Empresa creada: Universidad de Caldas';
END

SET IDENTITY_INSERT Empresas OFF;
GO

-- =====================================================
-- 2. SEDES (5 sedes físicas)
-- =====================================================
SET IDENTITY_INSERT Sedes ON;

IF NOT EXISTS (SELECT 1 FROM Sedes)
BEGIN
    INSERT INTO Sedes (IdSede, IdEmpresa, Nombre, Codigo, Direccion, Telefono, HorarioLaboral, EsSedePrincipal, Estado, FechaCreacion)
    VALUES
    -- Sede Principal
    (1, 1, 'Sede Principal', 'PRINCIPAL', 'Calle 65 No 26-10', '6068781500', 'Lunes a Jueves: 7:45am-11:45am / 1:45pm-5:45pm | Viernes: 7:00am-3:30pm', 1, 1, GETDATE()),
    
    -- Sede Bellas Artes
    (2, 1, 'Sede Bellas Artes', 'BELLAS', 'Carrera 21 No 13-02', '6068781510', 'Lunes a Jueves: 7:45am-11:45am / 1:45pm-5:45pm | Viernes: 7:00am-3:30pm', 0, 1, GETDATE()),
    
    -- Sede Palogrande
    (3, 1, 'Sede Palogrande', 'PALO', 'Carrera 23 No 58-65', '6068781520', 'Lunes a Jueves: 7:45am-11:45am / 1:45pm-5:45pm | Viernes: 7:00am-3:30pm', 0, 1, GETDATE()),
    
    -- Sede Sancancio
    (4, 1, 'Sede Sancancio', 'SANCAN', 'Calle 65 No 30-65', '6068781530', 'Lunes a Jueves: 7:45am-11:45am / 1:45pm-5:45pm | Viernes: 7:00am-3:30pm', 0, 1, GETDATE()),
    
    -- Sede Versalles
    (5, 1, 'Sede Versalles', 'VERSAL', 'Carrera 25 No 48-57', '6068781540', 'Lunes a Jueves: 7:45am-11:45am / 1:45pm-5:45pm | Viernes: 7:00am-3:30pm', 0, 1, GETDATE());

    PRINT '✓ 5 Sedes creadas exitosamente';
END

SET IDENTITY_INSERT Sedes OFF;
GO

-- =====================================================
-- 3. DEPENDENCIAS (15 dependencias en total)
-- =====================================================
SET IDENTITY_INSERT Dependencias ON;

IF NOT EXISTS (SELECT 1 FROM Dependencias)
BEGIN
    INSERT INTO Dependencias (IdDependencia, IdSede, Nombre, TipoDependencia, Ubicacion, Responsable, TelefonoContacto, Estado, FechaCreacion)
    VALUES
    -- SEDE PRINCIPAL (5 dependencias)
    (1, 1, 'Papelería Sede Principal', 'Papelería', 'Edificio Administrativo - Piso 1', 'María González', '3001234567', 1, GETDATE()),
    (2, 1, 'Cafetería Central', 'Cafetería', 'Edificio Central - Piso 1', 'Carlos Ramírez', '3001234568', 1, GETDATE()),
    (3, 1, 'Almacén General', 'Almacén', 'Edificio de Servicios - Bodega', 'Jorge Pérez', '3001234569', 1, GETDATE()),
    (4, 1, 'Laboratorio de Química', 'Laboratorio', 'Edificio de Ciencias - Piso 3', 'Dra. Ana López', '3001234570', 1, GETDATE()),
    (5, 1, 'Biblioteca Principal', 'Otros', 'Edificio de Biblioteca - Todos los pisos', 'Lic. Pedro Sánchez', '3001234571', 1, GETDATE()),
    
    -- SEDE BELLAS ARTES (3 dependencias)
    (6, 2, 'Papelería Bellas Artes', 'Papelería', 'Entrada Principal', 'Laura Martínez', '3002234567', 1, GETDATE()),
    (7, 2, 'Cafetería Artes', 'Cafetería', 'Patio Central', 'Diego Torres', '3002234568', 1, GETDATE()),
    (8, 2, 'Almacén de Materiales Artísticos', 'Almacén', 'Edificio B - Bodega', 'Sofía Ruiz', '3002234569', 1, GETDATE()),
    
    -- SEDE PALOGRANDE (3 dependencias)
    (9, 3, 'Papelería Palogrande', 'Papelería', 'Bloque A - Piso 1', 'Andrés Moreno', '3003234567', 1, GETDATE()),
    (10, 3, 'Cafetería Palogrande', 'Cafetería', 'Zona Social', 'Camila Vargas', '3003234568', 1, GETDATE()),
    (11, 3, 'Laboratorio de Agronomía', 'Laboratorio', 'Bloque C - Invernaderos', 'Ing. Roberto Gómez', '3003234569', 1, GETDATE()),
    
    -- SEDE SANCANCIO (2 dependencias)
    (12, 4, 'Papelería Sancancio', 'Papelería', 'Edificio Principal', 'Patricia Díaz', '3004234567', 1, GETDATE()),
    (13, 4, 'Cafetería Sancancio', 'Cafetería', 'Zona de Descanso', 'Miguel Ángel Castro', '3004234568', 1, GETDATE()),
    
    -- SEDE VERSALLES (2 dependencias)
    (14, 5, 'Papelería Versalles', 'Papelería', 'Recepción', 'Valentina Herrera', '3005234567', 1, GETDATE()),
    (15, 5, 'Cafetería Versalles', 'Cafetería', 'Terraza', 'Fernando Ospina', '3005234568', 1, GETDATE());

    PRINT '✓ 15 Dependencias creadas exitosamente';
END

SET IDENTITY_INSERT Dependencias OFF;
GO

-- =====================================================
-- 5. CLIENTES (10 clientes)
-- =====================================================
SET IDENTITY_INSERT Clientes ON;

IF NOT EXISTS (SELECT 1 FROM Clientes)
BEGIN
    INSERT INTO Clientes (IdCliente, Nombre, Telefono, Direccion, Email, TipoCliente, DocumentoIdentidad, IdSedePredeterminada, FechaRegistro, Estado)
    VALUES
    (1, 'Juan Pérez Estudiante', '3101234567', 'Calle 50 #20-15', 'juan.perez@ucaldas.edu.co', 'Estudiante', '1234567890', 1, GETDATE(), 1),
    (2, 'María García Docente', '3102234567', 'Carrera 30 #45-20', 'maria.garcia@ucaldas.edu.co', 'Docente', '2345678901', 1, GETDATE(), 1),
    (3, 'Carlos López Administrativo', '3103234567', 'Calle 60 #15-30', 'carlos.lopez@ucaldas.edu.co', 'Administrativo', '3456789012', 1, GETDATE(), 1),
    (4, 'Ana Martínez Estudiante', '3104234567', 'Carrera 25 #40-10', 'ana.martinez@ucaldas.edu.co', 'Estudiante', '4567890123', 2, GETDATE(), 1),
    (5, 'Pedro Rodríguez Docente', '3105234567', 'Calle 45 #30-25', 'pedro.rodriguez@ucaldas.edu.co', 'Docente', '5678901234', 2, GETDATE(), 1),
    (6, 'Laura Sánchez Estudiante', '3106234567', 'Carrera 20 #50-15', 'laura.sanchez@ucaldas.edu.co', 'Estudiante', '6789012345', 3, GETDATE(), 1),
    (7, 'Diego Torres Externo', '3107234567', 'Calle 55 #25-20', 'diego.torres@gmail.com', 'Externo', '7890123456', 1, GETDATE(), 1),
    (8, 'Sofía Ramírez Administrativo', '3108234567', 'Carrera 35 #35-30', 'sofia.ramirez@ucaldas.edu.co', 'Administrativo', '8901234567', 1, GETDATE(), 1),
    (9, 'Andrés Gómez Estudiante', '3109234567', 'Calle 40 #20-40', 'andres.gomez@ucaldas.edu.co', 'Estudiante', '9012345678', 4, GETDATE(), 1),
    (10, 'Valentina Díaz Docente', '3110234567', 'Carrera 40 #55-25', 'valentina.diaz@ucaldas.edu.co', 'Docente', '0123456789', 5, GETDATE(), 1);

    PRINT '✓ 10 Clientes creados exitosamente';
END

SET IDENTITY_INSERT Clientes OFF;
GO

-- =====================================================
-- 6. PRODUCTOS (30 productos variados)
-- =====================================================
SET IDENTITY_INSERT Productos ON;

IF NOT EXISTS (SELECT 1 FROM Productos)
BEGIN
    INSERT INTO Productos (IdProducto, Codigo, Nombre, Descripcion, Precio, Stock, UnidadMedida, Categoria, EsCompartible, StockMinimoGlobal, RequiereRefrigeracion, Estado, FechaCreacion)
    VALUES
    -- PAPELERÍA (10 productos compartibles)
    (1, 'PAP-001', 'Papel Bond Carta', 'Resma de 500 hojas papel bond carta 75g', 15000, 0, 'Resma', 'Papelería', 1, 50, 0, 1, GETDATE()),
    (2, 'PAP-002', 'Papel Bond Oficio', 'Resma de 500 hojas papel bond oficio 75g', 17000, 0, 'Resma', 'Papelería', 1, 30, 0, 1, GETDATE()),
    (3, 'PAP-003', 'Bolígrafos Azul', 'Caja x12 bolígrafos punta fina azul', 8000, 0, 'Caja', 'Papelería', 1, 20, 0, 1, GETDATE()),
    (4, 'PAP-004', 'Bolígrafos Negro', 'Caja x12 bolígrafos punta fina negro', 8000, 0, 'Caja', 'Papelería', 1, 20, 0, 1, GETDATE()),
    (5, 'PAP-005', 'Lápices HB', 'Caja x12 lápices grafito HB', 6000, 0, 'Caja', 'Papelería', 1, 15, 0, 1, GETDATE()),
    (6, 'PAP-006', 'Cuadernos 100 hojas', 'Cuaderno cuadriculado 100 hojas', 5500, 0, 'Unidad', 'Papelería', 1, 30, 0, 1, GETDATE()),
    (7, 'PAP-007', 'Carpetas de Cartón', 'Carpeta carta con ganchos', 2500, 0, 'Unidad', 'Papelería', 1, 40, 0, 1, GETDATE()),
    (8, 'PAP-008', 'Marcadores Permanentes', 'Marcador permanente punta fina negro', 3000, 0, 'Unidad', 'Papelería', 1, 25, 0, 1, GETDATE()),
    (9, 'PAP-009', 'Resaltadores', 'Set x4 resaltadores colores variados', 9000, 0, 'Set', 'Papelería', 1, 15, 0, 1, GETDATE()),
    (10, 'PAP-010', 'Grapadora Metálica', 'Grapadora metálica para 20 hojas', 12000, 0, 'Unidad', 'Papelería', 1, 10, 0, 1, GETDATE()),
    
    -- CAFETERÍA (10 productos compartibles)
    (11, 'CAF-001', 'Café Molido Premium', 'Café molido 500g origen colombiano', 18000, 0, 'Paquete', 'Cafetería', 1, 30, 0, 1, GETDATE()),
    (12, 'CAF-002', 'Agua Embotellada', 'Agua purificada 600ml', 1500, 0, 'Unidad', 'Cafetería', 1, 100, 0, 1, GETDATE()),
    (13, 'CAF-003', 'Jugo Natural Naranja', 'Jugo natural de naranja 300ml', 3500, 0, 'Unidad', 'Cafetería', 1, 50, 1, 1, GETDATE()),
    (14, 'CAF-004', 'Empanada de Carne', 'Empanada tradicional de carne', 2000, 0, 'Unidad', 'Cafetería', 1, 40, 1, 1, GETDATE()),
    (15, 'CAF-005', 'Sándwich Mixto', 'Sándwich de jamón y queso', 5000, 0, 'Unidad', 'Cafetería', 1, 30, 1, 1, GETDATE()),
    (16, 'CAF-006', 'Galletas Integrales', 'Paquete de galletas integrales', 3000, 0, 'Paquete', 'Cafetería', 1, 40, 0, 1, GETDATE()),
    (17, 'CAF-007', 'Chocolate Caliente', 'Sobre de chocolate en polvo', 2500, 0, 'Unidad', 'Cafetería', 1, 50, 0, 1, GETDATE()),
    (18, 'CAF-008', 'Té Verde', 'Caja x20 sobres de té verde', 8000, 0, 'Caja', 'Cafetería', 1, 20, 0, 1, GETDATE()),
    (19, 'CAF-009', 'Yogurt Natural', 'Yogurt natural 200ml', 2800, 0, 'Unidad', 'Cafetería', 1, 50, 1, 1, GETDATE()),
    (20, 'CAF-010', 'Frutas Frescas Mix', 'Paquete de frutas picadas variadas', 4500, 0, 'Unidad', 'Cafetería', 1, 20, 1, 1, GETDATE()),
    
    -- LABORATORIO (5 productos NO compartibles)
    (21, 'LAB-001', 'Microscopio Óptico', 'Microscopio óptico binocular 1000x', 2500000, 0, 'Unidad', 'Laboratorio', 0, 2, 0, 1, GETDATE()),
    (22, 'LAB-002', 'Reactivo Químico Especial', 'Reactivo exclusivo Lab. Química Sede Principal', 150000, 0, 'Frasco', 'Laboratorio', 0, 5, 0, 1, GETDATE()),
    (23, 'LAB-003', 'Kit Disección Completo', 'Kit completo de herramientas de disección', 85000, 0, 'Kit', 'Laboratorio', 0, 3, 0, 1, GETDATE()),
    (24, 'LAB-004', 'Balanza Analítica Digital', 'Balanza de precisión 0.001g', 1800000, 0, 'Unidad', 'Laboratorio', 0, 2, 0, 1, GETDATE()),
    (25, 'LAB-005', 'Centrífuga de Laboratorio', 'Centrífuga 6 tubos 4000rpm', 3200000, 0, 'Unidad', 'Laboratorio', 0, 1, 0, 1, GETDATE()),
    
    -- MATERIALES ARTÍSTICOS (5 productos compartibles)
    (26, 'ART-001', 'Témperas Profesionales', 'Set x12 témperas 100ml colores variados', 45000, 0, 'Set', 'Arte', 1, 10, 0, 1, GETDATE()),
    (27, 'ART-002', 'Pinceles Variados', 'Set x10 pinceles de diferentes tamaños', 35000, 0, 'Set', 'Arte', 1, 15, 0, 1, GETDATE()),
    (28, 'ART-003', 'Lienzo 50x70cm', 'Lienzo pre-tensado 50x70cm', 28000, 0, 'Unidad', 'Arte', 1, 20, 0, 1, GETDATE()),
    (29, 'ART-004', 'Arcilla para Modelar', 'Arcilla blanca para modelado 1kg', 12000, 0, 'Kilogramo', 'Arte', 1, 25, 0, 1, GETDATE()),
    (30, 'ART-005', 'Block de Dibujo A3', 'Block de papel para dibujo A3 50 hojas', 18000, 0, 'Unidad', 'Arte', 1, 15, 0, 1, GETDATE());

    PRINT '✓ 30 Productos creados exitosamente';
END

SET IDENTITY_INSERT Productos OFF;
GO

-- =====================================================
-- 7. INVENTARIO POR DEPENDENCIA (distribución realista)
-- =====================================================
SET IDENTITY_INSERT InventarioDependencia ON;

IF NOT EXISTS (SELECT 1 FROM InventarioDependencia)
BEGIN
    -- PAPELERÍA SEDE PRINCIPAL (productos de papelería)
    INSERT INTO InventarioDependencia (IdInventario, IdProducto, IdDependencia, StockActual, StockMinimo, StockMaximo, PuntoReorden, CostoPromedio, Ubicacion, EstadoInventario, UltimaActualizacion)
    VALUES
    (1, 1, 1, 150, 20, 300, 30, 12000, 'Estante A1', 'Disponible', GETDATE()),
    (2, 2, 1, 80, 15, 200, 25, 13600, 'Estante A2', 'Disponible', GETDATE()),
    (3, 3, 1, 45, 10, 100, 15, 6400, 'Estante B1', 'Disponible', GETDATE()),
    (4, 4, 1, 50, 10, 100, 15, 6400, 'Estante B1', 'Disponible', GETDATE()),
    (5, 5, 1, 30, 8, 80, 12, 4800, 'Estante B2', 'Disponible', GETDATE()),
    (6, 6, 1, 120, 20, 250, 35, 4400, 'Estante C1', 'Disponible', GETDATE()),
    (7, 7, 1, 200, 30, 400, 50, 2000, 'Estante C2', 'Disponible', GETDATE()),
    (8, 8, 1, 80, 15, 150, 25, 2400, 'Estante D1', 'Disponible', GETDATE()),
    (9, 9, 1, 40, 10, 80, 15, 7200, 'Estante D2', 'Disponible', GETDATE()),
    (10, 10, 1, 25, 5, 50, 8, 9600, 'Estante E1', 'Disponible', GETDATE());

    -- ALMACÉN GENERAL (stock grande de productos compartibles)
    INSERT INTO InventarioDependencia (IdInventario, IdProducto, IdDependencia, StockActual, StockMinimo, StockMaximo, PuntoReorden, CostoPromedio, Ubicacion, EstadoInventario, UltimaActualizacion)
    VALUES
    (11, 1, 3, 500, 50, 1000, 100, 12000, 'Bodega A - Zona 1', 'Disponible', GETDATE()),
    (12, 2, 3, 300, 30, 600, 60, 13600, 'Bodega A - Zona 1', 'Disponible', GETDATE()),
    (13, 3, 3, 200, 20, 400, 40, 6400, 'Bodega A - Zona 2', 'Disponible', GETDATE()),
    (14, 4, 3, 200, 20, 400, 40, 6400, 'Bodega A - Zona 2', 'Disponible', GETDATE()),
    (15, 11, 3, 150, 30, 300, 50, 14400, 'Bodega B - Zona 1', 'Disponible', GETDATE()),
    (16, 16, 3, 250, 40, 500, 70, 2400, 'Bodega B - Zona 2', 'Disponible', GETDATE()),
    (17, 26, 3, 50, 10, 100, 15, 36000, 'Bodega C - Zona 1', 'Disponible', GETDATE()),
    (18, 27, 3, 60, 15, 120, 20, 28000, 'Bodega C - Zona 1', 'Disponible', GETDATE());

    -- CAFETERÍA CENTRAL
    INSERT INTO InventarioDependencia (IdInventario, IdProducto, IdDependencia, StockActual, StockMinimo, StockMaximo, PuntoReorden, CostoPromedio, Ubicacion, EstadoInventario, UltimaActualizacion)
    VALUES
    (19, 11, 2, 80, 20, 150, 30, 14400, 'Despensa Superior', 'Disponible', GETDATE()),
    (20, 12, 2, 200, 50, 400, 80, 1200, 'Refrigerador 1', 'Disponible', GETDATE()),
    (21, 13, 2, 50, 20, 100, 30, 2800, 'Refrigerador 2', 'Disponible', GETDATE()),
    (22, 14, 2, 40, 15, 80, 25, 1600, 'Congelador', 'Disponible', GETDATE()),
    (23, 15, 2, 30, 10, 60, 15, 4000, 'Refrigerador 3', 'Disponible', GETDATE()),
    (24, 19, 2, 60, 20, 120, 35, 2240, 'Refrigerador 4', 'Disponible', GETDATE());

    -- LABORATORIO DE QUÍMICA (equipos NO compartibles)
    INSERT INTO InventarioDependencia (IdInventario, IdProducto, IdDependencia, StockActual, StockMinimo, StockMaximo, PuntoReorden, CostoPromedio, Ubicacion, EstadoInventario, UltimaActualizacion)
    VALUES
    (25, 21, 4, 3, 1, 5, 2, 2000000, 'Gabinete Principal', 'Disponible', GETDATE()),
    (26, 22, 4, 15, 5, 30, 8, 120000, 'Estante Reactivos - Vitrina', 'Disponible', GETDATE()),
    (27, 24, 4, 2, 1, 3, 1, 1440000, 'Mesa de Trabajo 1', 'Disponible', GETDATE()),
    (28, 25, 4, 1, 1, 2, 1, 2560000, 'Mesa de Trabajo 2', 'Disponible', GETDATE());

    -- PAPELERÍA BELLAS ARTES
    INSERT INTO InventarioDependencia (IdInventario, IdProducto, IdDependencia, StockActual, StockMinimo, StockMaximo, PuntoReorden, CostoPromedio, Ubicacion, EstadoInventario, UltimaActualizacion)
    VALUES
    (29, 1, 6, 5, 15, 200, 25, 12000, 'Mostrador Principal', 'Disponible', GETDATE()), -- Stock BAJO
    (30, 3, 6, 30, 10, 80, 15, 6400, 'Estante Lateral', 'Disponible', GETDATE()),
    (31, 6, 6, 80, 15, 150, 25, 4400, 'Estante Central', 'Disponible', GETDATE()),
    (32, 26, 6, 40, 10, 80, 15, 36000, 'Zona Arte 1', 'Disponible', GETDATE()),
    (33, 27, 6, 35, 10, 70, 15, 28000, 'Zona Arte 2', 'Disponible', GETDATE()),
    (34, 28, 6, 50, 15, 100, 20, 22400, 'Zona Arte 3', 'Disponible', GETDATE());

    -- ALMACÉN MATERIALES ARTÍSTICOS
    INSERT INTO InventarioDependencia (IdInventario, IdProducto, IdDependencia, StockActual, StockMinimo, StockMaximo, PuntoReorden, CostoPromedio, Ubicacion, EstadoInventario, UltimaActualizacion)
    VALUES
    (35, 26, 8, 80, 15, 150, 25, 36000, 'Estante Principal A', 'Disponible', GETDATE()),
    (36, 27, 8, 90, 20, 180, 30, 28000, 'Estante Principal B', 'Disponible', GETDATE()),
    (37, 28, 8, 100, 20, 200, 35, 22400, 'Estante Principal C', 'Disponible', GETDATE()),
    (38, 29, 8, 120, 25, 250, 40, 9600, 'Bodega Arcilla', 'Disponible', GETDATE()),
    (39, 30, 8, 70, 15, 140, 25, 14400, 'Estante Papel', 'Disponible', GETDATE());

    -- PAPELERÍA PALOGRANDE
    INSERT INTO InventarioDependencia (IdInventario, IdProducto, IdDependencia, StockActual, StockMinimo, StockMaximo, PuntoReorden, CostoPromedio, Ubicacion, EstadoInventario, UltimaActualizacion)
    VALUES
    (40, 1, 9, 100, 20, 200, 30, 12000, 'Vitrina 1', 'Disponible', GETDATE()),
    (41, 3, 9, 8, 10, 80, 15, 6400, 'Vitrina 2', 'Disponible', GETDATE()), -- Stock BAJO
    (42, 5, 9, 40, 10, 80, 15, 4800, 'Vitrina 3', 'Disponible', GETDATE());

    PRINT '✓ 42 Registros de inventario creados en diferentes dependencias';
END

SET IDENTITY_INSERT InventarioDependencia OFF;
GO

-- =====================================================
-- 8. PEDIDOS DE EJEMPLO (5 pedidos)
-- =====================================================
SET IDENTITY_INSERT Pedidos ON;

IF NOT EXISTS (SELECT 1 FROM Pedidos)
BEGIN
    INSERT INTO Pedidos (IdPedido, IdCliente, IdUsuario, IdSede, IdDependencia, Fecha, Estado, Total, TipoEntrega, MetodoPago, FechaEstimadaEntrega)
    VALUES
    (1, 1, 4, 1, 1, DATEADD(DAY, -5, GETDATE()), 'Entregado', 45000, 'Retiro', 'Efectivo', DATEADD(DAY, -4, GETDATE())),
    (2, 2, 4, 1, 1, DATEADD(DAY, -3, GETDATE()), 'Entregado', 72000, 'Retiro', 'Tarjeta', DATEADD(DAY, -2, GETDATE())),
    (3, 4, 6, 2, 6, DATEADD(DAY, -2, GETDATE()), 'Procesado', 38500, 'Retiro', 'Efectivo', DATEADD(DAY, -1, GETDATE())),
    (4, 6, 4, 1, 1, DATEADD(DAY, -1, GETDATE()), 'Pendiente', 91000, 'Domicilio', 'Transferencia', DATEADD(DAY, 1, GETDATE())),
    (5, 9, 4, 1, 2, GETDATE(), 'Pendiente', 24000, 'Retiro', 'Efectivo', DATEADD(DAY, 1, GETDATE()));

    PRINT '✓ 5 Pedidos de ejemplo creados';
END

SET IDENTITY_INSERT Pedidos OFF;
GO

-- =====================================================
-- 9. DETALLES DE PEDIDOS
-- =====================================================
SET IDENTITY_INSERT DetallesPedido ON;

IF NOT EXISTS (SELECT 1 FROM DetallesPedido)
BEGIN
    -- Pedido 1
    INSERT INTO DetallesPedido (IdDetalle, IdPedido, IdProducto, IdInventario, Cantidad, PrecioUnitario, Descuento, Subtotal)
    VALUES
    (1, 1, 1, 1, 3, 15000, 0, 45000);

    -- Pedido 2
    INSERT INTO DetallesPedido (IdDetalle, IdPedido, IdProducto, IdInventario, Cantidad, PrecioUnitario, Descuento, Subtotal)
    VALUES
    (2, 2, 3, 3, 2, 8000, 0, 16000),
    (3, 2, 6, 6, 4, 5500, 0, 22000),
    (4, 2, 10, 10, 2, 12000, 0, 24000),
    (5, 2, 7, 7, 4, 2500, 0, 10000);

    -- Pedido 3
    INSERT INTO DetallesPedido (IdDetalle, IdPedido, IdProducto, IdInventario, Cantidad, PrecioUnitario, Descuento, Subtotal)
    VALUES
    (6, 3, 26, 32, 1, 45000, 6500, 38500);

    -- Pedido 4
    INSERT INTO DetallesPedido (IdDetalle, IdPedido, IdProducto, IdInventario, Cantidad, PrecioUnitario, Descuento, Subtotal)
    VALUES
    (7, 4, 1, 1, 5, 15000, 0, 75000),
    (8, 4, 3, 3, 2, 8000, 0, 16000);

    -- Pedido 5
    INSERT INTO DetallesPedido (IdDetalle, IdPedido, IdProducto, IdInventario, Cantidad, PrecioUnitario, Descuento, Subtotal)
    VALUES
    (9, 5, 11, 19, 2, 18000, 0, 36000),
    (10, 5, 14, 22, 4, 2000, 0, 8000);

    PRINT '✓ 10 Detalles de pedidos creados';
END

SET IDENTITY_INSERT DetallesPedido OFF;
GO

-- =====================================================
-- 10. MOVIMIENTOS DE INVENTARIO (historial)
-- =====================================================
SET IDENTITY_INSERT MovimientoInventario ON;

IF NOT EXISTS (SELECT 1 FROM MovimientoInventario)
BEGIN
    -- Movimientos iniciales (entrada de inventario)
    INSERT INTO MovimientoInventario (IdMovimiento, IdInventario, TipoMovimiento, Cantidad, StockAnterior, StockNuevo, Fecha, IdUsuario, TipoReferencia, IdReferencia, Observaciones, CostoUnitario)
    VALUES
    (1, 1, 'Entrada', 150, 0, 150, DATEADD(DAY, -30, GETDATE()), 4, 'Compra', NULL, 'Stock inicial - Compra a proveedor', 12000),
    (2, 11, 'Entrada', 500, 0, 500, DATEADD(DAY, -30, GETDATE()), 5, 'Compra', NULL, 'Stock inicial - Almacén General', 12000),
    (3, 29, 'Entrada', 100, 0, 100, DATEADD(DAY, -25, GETDATE()), 6, 'Compra', NULL, 'Stock inicial - Papelería Bellas Artes', 12000);

    -- Transferencia de ejemplo
    INSERT INTO MovimientoInventario (IdMovimiento, IdInventario, TipoMovimiento, Cantidad, StockAnterior, StockNuevo, Fecha, IdUsuario, TipoReferencia, IdReferencia, Observaciones, CostoUnitario)
    VALUES
    (4, 11, 'Transferencia', -50, 550, 500, DATEADD(DAY, -20, GETDATE()), 5, 'Transferencia', 1, 'Transferencia a Papelería Principal', 12000),
    (5, 1, 'Transferencia', 50, 100, 150, DATEADD(DAY, -20, GETDATE()), 4, 'Transferencia', 1, 'Recepción desde Almacén General', 12000);

    -- Salidas por ventas (pedidos entregados)
    INSERT INTO MovimientoInventario (IdMovimiento, IdInventario, TipoMovimiento, Cantidad, StockAnterior, StockNuevo, Fecha, IdUsuario, TipoReferencia, IdReferencia, Observaciones, CostoUnitario)
    VALUES
    (6, 1, 'Salida', -3, 153, 150, DATEADD(DAY, -4, GETDATE()), 4, 'Pedido', 1, 'Venta - Pedido #1', 12000),
    (7, 3, 'Salida', -2, 47, 45, DATEADD(DAY, -2, GETDATE()), 4, 'Pedido', 2, 'Venta - Pedido #2', 6400);

    -- Transferencia reciente (la que hicimos con stock bajo)
    INSERT INTO MovimientoInventario (IdMovimiento, IdInventario, TipoMovimiento, Cantidad, StockAnterior, StockNuevo, Fecha, IdUsuario, TipoReferencia, IdReferencia, Observaciones, CostoUnitario)
    VALUES
    (8, 11, 'Transferencia', -95, 595, 500, DATEADD(DAY, -2, GETDATE()), 5, 'Transferencia', 2, 'Transferencia a Papelería Bellas Artes - Reabastecimiento', 12000),
    (9, 29, 'Transferencia', 95, 5, 100, DATEADD(DAY, -2, GETDATE()), 6, 'Transferencia', 2, 'Recepción desde Almacén General - Reabastecimiento', 12000);

    -- Ajuste de inventario
    INSERT INTO MovimientoInventario (IdMovimiento, IdInventario, TipoMovimiento, Cantidad, StockAnterior, StockNuevo, Fecha, IdUsuario, TipoReferencia, IdReferencia, Observaciones, CostoUnitario)
    VALUES
    (10, 41, 'Ajuste', -2, 10, 8, DATEADD(DAY, -1, GETDATE()), 4, 'Ajuste Manual', NULL, 'Ajuste por inventario físico - Producto dañado', 6400);

    PRINT '✓ 10 Movimientos de inventario registrados';
END

SET IDENTITY_INSERT MovimientoInventario OFF;
GO

-- =====================================================
-- 11. TRANSFERENCIAS DE STOCK (ejemplos de flujo completo)
-- =====================================================
SET IDENTITY_INSERT TransferenciaStock ON;

IF NOT EXISTS (SELECT 1 FROM TransferenciaStock)
BEGIN
    INSERT INTO TransferenciaStock (IdTransferencia, IdProducto, IdDependenciaOrigen, IdDependenciaDestino, Cantidad, Motivo, FechaSolicitud, FechaAprobacion, FechaEjecucion, IdUsuarioSolicita, IdUsuarioAprueba, Estado, Observaciones, CostoTransporte)
    VALUES
    -- Transferencia ejecutada (reabastecimiento exitoso)
    (1, 1, 3, 1, 50, 'Reabastecimiento', DATEADD(DAY, -20, GETDATE()), DATEADD(DAY, -20, GETDATE()), DATEADD(DAY, -20, GETDATE()), 4, 2, 'Ejecutada', 'Transferencia normal - Stock bajo en Papelería Principal', NULL),
    
    -- Transferencia ejecutada reciente (la del stock bajo)
    (2, 1, 3, 6, 95, 'Stock bajo', DATEADD(DAY, -2, GETDATE()), DATEADD(DAY, -2, GETDATE()), DATEADD(DAY, -2, GETDATE()), 6, 3, 'Ejecutada', 'Reabastecimiento urgente - Papelería Bellas Artes', NULL),
    
    -- Transferencia aprobada (lista para ejecutar)
    (3, 3, 3, 9, 30, 'Reabastecimiento', DATEADD(DAY, -1, GETDATE()), DATEADD(DAY, -1, GETDATE()), NULL, 4, 2, 'Aprobada', 'Pendiente de ejecución', NULL),
    
    -- Transferencia pendiente (esperando aprobación)
    (4, 11, 3, 2, 50, 'Reabastecimiento', GETDATE(), NULL, NULL, 4, NULL, 'Pendiente', 'Solicitud de café para cafetería central', NULL),
    
    -- Transferencia rechazada
    (5, 21, 4, 11, 1, 'Apertura nueva dependencia', DATEADD(DAY, -5, GETDATE()), NULL, NULL, 4, 2, 'Rechazada', 'Producto no compartible - No se puede transferir equipos especializados', NULL);

    PRINT '✓ 5 Transferencias de stock creadas (diferentes estados)';
END

SET IDENTITY_INSERT TransferenciaStock OFF;
GO

-- =====================================================
-- 12. ENTREGAS (para pedidos entregados)
-- =====================================================
SET IDENTITY_INSERT Entregas ON;

IF NOT EXISTS (SELECT 1 FROM Entregas)
BEGIN
    INSERT INTO Entregas (IdEntrega, IdPedido, DireccionEntrega, FechaEntrega, Estado, Transportista, CostoEnvio)
    VALUES
    (1, 1, 'Calle 50 #20-15 - Manizales', DATEADD(DAY, -4, GETDATE()), 'Entregado', 'Retiro en sede', 0),
    (2, 2, 'Carrera 30 #45-20 - Manizales', DATEADD(DAY, -2, GETDATE()), 'Entregado', 'Retiro en sede', 0),
    (3, 3, 'Carrera 21 No 13-02 - Manizales', DATEADD(DAY, -1, GETDATE()), 'Entregado', 'Retiro en sede', 0);

    PRINT '✓ 3 Entregas registradas';
END

SET IDENTITY_INSERT Entregas OFF;
GO

-- =====================================================
-- 13. FACTURAS (para pedidos entregados)
-- =====================================================
SET IDENTITY_INSERT Facturas ON;

IF NOT EXISTS (SELECT 1 FROM Facturas)
BEGIN
    INSERT INTO Facturas (IdFactura, IdPedido, NumeroFactura, Fecha, Subtotal, Iva, Descuentos, Total, MetodoPago, EstadoPago)
    VALUES
    (1, 1, 'FACT-2024-001', DATEADD(DAY, -4, GETDATE()), 45000, 0, 0, 45000, 'Efectivo', 'Pagado'),
    (2, 2, 'FACT-2024-002', DATEADD(DAY, -2, GETDATE()), 72000, 0, 0, 72000, 'Tarjeta', 'Pagado'),
    (3, 3, 'FACT-2024-003', DATEADD(DAY, -1, GETDATE()), 38500, 0, 6500, 32000, 'Efectivo', 'Pagado');

    PRINT '✓ 3 Facturas generadas';
END

SET IDENTITY_INSERT Facturas OFF;
GO

-- =====================================================
-- RESUMEN FINAL
-- =====================================================
PRINT '';
PRINT '========================================';
PRINT 'RESUMEN DEL POBLADO DE DATOS';
PRINT '========================================';
PRINT '';

SELECT 'Empresas' AS Tabla, COUNT(*) AS Registros FROM Empresas
UNION ALL
SELECT 'Sedes', COUNT(*) FROM Sedes
UNION ALL
SELECT 'Dependencias', COUNT(*) FROM Dependencias
UNION ALL
SELECT 'Usuarios', COUNT(*) FROM Usuarios
UNION ALL
SELECT 'Clientes', COUNT(*) FROM Clientes
UNION ALL
SELECT 'Productos', COUNT(*) FROM Productos
UNION ALL
SELECT 'InventarioDependencia', COUNT(*) FROM InventarioDependencia
UNION ALL
SELECT 'Pedidos', COUNT(*) FROM Pedidos
UNION ALL
SELECT 'DetallesPedido', COUNT(*) FROM DetallesPedido
UNION ALL
SELECT 'MovimientoInventario', COUNT(*) FROM MovimientoInventario
UNION ALL
SELECT 'TransferenciaStock', COUNT(*) FROM TransferenciaStock
UNION ALL
SELECT 'Entregas', COUNT(*) FROM Entregas
UNION ALL
SELECT 'Facturas', COUNT(*) FROM Facturas;

PRINT '';
PRINT '========================================';
PRINT 'DATOS DE ACCESO';
PRINT '========================================';
PRINT 'Usuario: admin@ucaldas.edu.co';
PRINT 'Contraseña: admin123';
PRINT 'Rol: SuperAdmin';
PRINT '';
PRINT 'Otros usuarios disponibles:';
PRINT '- coord.principal@ucaldas.edu.co (AdminSede)';
PRINT '- coord.bellas@ucaldas.edu.co (AdminSede)';
PRINT '- maria.gonzalez@ucaldas.edu.co (EncargadoDependencia)';
PRINT '- Todos con contraseña: admin123';
PRINT '';
PRINT '✅ POBLADO COMPLETO EXITOSO';
PRINT '';
