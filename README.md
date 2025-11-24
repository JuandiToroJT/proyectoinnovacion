# 📦 Sistema de Gestión de Inventarios Multi-Sede
## Universidad de Caldas

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=flat&logo=.net)
![SQL Server](https://img.shields.io/badge/SQL%20Server-Express-CC2927?style=flat&logo=microsoft-sql-server)
![Bootstrap](https://img.shields.io/badge/Bootstrap-5.3-7952B3?style=flat&logo=bootstrap)
![License](https://img.shields.io/badge/License-MIT-green?style=flat)

Sistema integral de gestión de inventarios diseñado para instituciones educativas con múltiples sedes y dependencias. Permite el control centralizado de productos, transferencias entre ubicaciones, gestión de pedidos y seguimiento en tiempo real del stock.

---

## 📋 Tabla de Contenidos

- [Características](#-características)
- [Tecnologías](#-tecnologías)
- [Arquitectura](#-arquitectura)
- [Requisitos Previos](#-requisitos-previos)
- [Instalación](#-instalación)
- [Configuración](#-configuración)
- [Estructura del Proyecto](#-estructura-del-proyecto)
- [Modelo de Datos](#-modelo-de-datos)
- [API Endpoints](#-api-endpoints)
- [Roles y Permisos](#-roles-y-permisos)
- [Funcionalidades Principales](#-funcionalidades-principales)
- [Capturas de Pantalla](#-capturas-de-pantalla)
- [Contribuir](#-contribuir)
- [Licencia](#-licencia)
- [Autores](#-autores)

---

## 🚀 Características

### ✨ Gestión Multi-Sede
- **5 sedes físicas** de la Universidad de Caldas
- **15+ dependencias** distribuidas entre sedes
- Control jerárquico de permisos por ubicación
- Dashboard contextual según sede/dependencia

### 📊 Control de Inventario
- Seguimiento en tiempo real de stock por ubicación
- Alertas automáticas de stock bajo
- Histórico completo de movimientos
- Consolidación de inventario por sede
- Gestión de productos compartibles y exclusivos

### 🔄 Transferencias de Stock
- Solicitud, aprobación y ejecución de transferencias
- Validación automática de disponibilidad
- Timeline del proceso de transferencia
- Restricciones según tipo de producto

### 👥 Gestión de Usuarios
- 6 roles con permisos granulares
- Asignación de usuarios a sedes/dependencias
- Autenticación con hash de contraseñas
- Sistema dual de sesión (UsuarioLogueado + Session)

### 📦 Pedidos y Entregas
- Creación de pedidos con múltiples productos
- Generación automática de facturas
- Programación de entregas
- Estados: Pendiente → Procesado → Entregado

### 📈 Reportes y Estadísticas
- Dashboard con métricas en tiempo real
- Productos con stock bajo
- Pedidos recientes
- Transferencias pendientes
- Consolidados por sede

---

## 🛠️ Tecnologías

### Backend
- **Framework:** ASP.NET Core 8.0
- **Lenguaje:** C# 12
- **ORM:** Entity Framework Core 8.0
- **Base de Datos:** SQL Server Express 2022
- **Autenticación:** ASP.NET Identity
- **API:** RESTful con Swagger/OpenAPI

### Frontend
- **Framework:** ASP.NET MVC (Razor Views)
- **CSS:** Bootstrap 5.3
- **JavaScript:** jQuery 3.7
- **Iconos:** Bootstrap Icons 1.11
- **AJAX:** Fetch API

### Herramientas de Desarrollo
- **IDE:** Visual Studio 2022 / Visual Studio Code
- **Control de Versiones:** Git
- **Package Manager:** NuGet
- **Migrations:** EF Core Migrations

---

## 🏗️ Arquitectura

```
┌─────────────────────────────────────────────────────────┐
│                    CLIENTE (Navegador)                   │
│  ┌──────────┐  ┌──────────┐  ┌──────────┐             │
│  │  Razor   │  │   AJAX   │  │Bootstrap │             │
│  │  Views   │  │  jQuery  │  │   UI     │             │
│  └──────────┘  └──────────┘  └──────────┘             │
└─────────────────────────────────────────────────────────┘
                         │ HTTP/HTTPS
                         ↓
┌─────────────────────────────────────────────────────────┐
│              ASP.NET Core MVC (Frontend)                 │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐ │
│  │ Controllers  │  │    Views     │  │   Filters    │ │
│  │   (MVC)      │  │   (Razor)    │  │(Auth/Roles)  │ │
│  └──────────────┘  └──────────────┘  └──────────────┘ │
└─────────────────────────────────────────────────────────┘
                         │ HTTP Client
                         ↓
┌─────────────────────────────────────────────────────────┐
│            ASP.NET Core Web API (Backend)                │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐ │
│  │ Controllers  │  │    DTOs      │  │   Services   │ │
│  │    (API)     │  │  (Models)    │  │  (Business)  │ │
│  └──────────────┘  └──────────────┘  └──────────────┘ │
└─────────────────────────────────────────────────────────┘
                         │ Entity Framework Core
                         ↓
┌─────────────────────────────────────────────────────────┐
│                  SQL Server Express                      │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐ │
│  │   Productos  │  │  Inventario  │  │   Pedidos    │ │
│  │    Sedes     │  │Dependencias  │  │  Usuarios    │ │
│  └──────────────┘  └──────────────┘  └──────────────┘ │
└─────────────────────────────────────────────────────────┘
```

---

## 📋 Requisitos Previos

- **.NET SDK 8.0** o superior ([Descargar](https://dotnet.microsoft.com/download))
- **SQL Server Express 2022** ([Descargar](https://www.microsoft.com/sql-server/sql-server-downloads))
- **Visual Studio 2022** (opcional, recomendado) o **VS Code**
- **Git** para clonar el repositorio

### Verificar instalación:
```bash
dotnet --version  # Debe mostrar 8.0.x
```

---

## 📥 Instalación

### 1. Clonar el Repositorio
```bash
git clone https://github.com/usuario/proyecto-inventarios-ucaldas.git
cd proyecto-inventarios-ucaldas
```

### 2. Restaurar Dependencias
```bash
# Backend API
cd ProyectoInventariosWebApi
dotnet restore

# Frontend MVC
cd ../ProyectoInventariosWebApp
dotnet restore
```

### 3. Configurar Base de Datos

#### Opción A: Crear base de datos y aplicar migraciones
```bash
cd ProyectoInventariosWebApi
dotnet ef database update
```

#### Opción B: Ejecutar script SQL completo
```bash
# Usar SQL Server Management Studio (SSMS)
# Abrir y ejecutar: Scripts/SCRIPT-POBLADO-COMPLETO.sql
```

### 4. Configurar Cadenas de Conexión

**Backend - `appsettings.json`:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=DESKTOP-C29PHED\\SQLEXPRESS;Database=ProyectoInventariosDB;Integrated Security=true;TrustServerCertificate=true;"
  }
}
```

**Frontend - `appsettings.json`:**
```json
{
  "ApiUrls": {
    "BaseUrl": "http://localhost:5192/"
  }
}
```

### 5. Ejecutar el Proyecto

**Terminal 1 - Backend API:**
```bash
cd ProyectoInventariosWebApi
dotnet run
```
API disponible en: `http://localhost:5192`

**Terminal 2 - Frontend MVC:**
```bash
cd ProyectoInventariosWebApp
dotnet run
```
Aplicación disponible en: `http://localhost:5082`

---

## ⚙️ Configuración

### Variables de Entorno (Opcional)

```bash
# Backend
export ASPNETCORE_ENVIRONMENT=Development
export ConnectionStrings__DefaultConnection="Server=...;Database=...;"

# Frontend
export ApiUrls__BaseUrl="http://localhost:5192/"
```

### Configuración de Roles Iniciales

El script de poblado crea automáticamente estos usuarios:

| Usuario | Contraseña | Rol |
|---------|-----------|-----|
| admin@ucaldas.edu.co | admin123 | SuperAdmin |
| coord.principal@ucaldas.edu.co | admin123 | AdminSede |
| maria.gonzalez@ucaldas.edu.co | admin123 | EncargadoDependencia |

---

## 📁 Estructura del Proyecto

```
proyecto-inventarios-ucaldas/
│
├── ProyectoInventariosWebApi/          # Backend API
│   ├── Controllers/                     # 8 controladores API
│   │   ├── ProductosController.cs
│   │   ├── SedesController.cs
│   │   ├── DependenciasController.cs
│   │   ├── InventarioDependenciaController.cs
│   │   ├── TransferenciaStockController.cs
│   │   ├── PedidosController.cs
│   │   ├── UsuariosController.cs
│   │   └── ClientesController.cs
│   ├── Models/                          # 14 entidades
│   │   ├── Empresas.cs
│   │   ├── Sedes.cs
│   │   ├── Dependencias.cs
│   │   ├── Productos.cs
│   │   ├── InventarioDependencia.cs
│   │   ├── TransferenciaStock.cs
│   │   ├── MovimientoInventario.cs
│   │   ├── Pedidos.cs
│   │   ├── DetallesPedido.cs
│   │   ├── Usuarios.cs
│   │   ├── Clientes.cs
│   │   ├── Entregas.cs
│   │   └── Facturas.cs
│   ├── Data/
│   │   └── ProyectoInventariosContext.cs
│   ├── Migrations/                      # Migraciones EF Core
│   ├── Program.cs                       # Configuración API
│   └── appsettings.json
│
├── ProyectoInventariosWebApp/          # Frontend MVC
│   ├── Controllers/                     # Controladores MVC
│   │   ├── HomeController.cs
│   │   ├── AccountController.cs
│   │   ├── ProductosController.cs
│   │   ├── SedesController.cs
│   │   ├── DependenciasController.cs
│   │   ├── TransferenciasController.cs
│   │   ├── PedidosController.cs
│   │   └── UsuariosController.cs
│   ├── Views/                           # Vistas Razor
│   │   ├── Home/
│   │   ├── Productos/
│   │   ├── Sedes/
│   │   ├── Dependencias/
│   │   ├── Transferencias/
│   │   ├── Pedidos/
│   │   └── Usuarios/
│   ├── Models/                          # Modelos de vista
│   │   ├── UsuarioLogueado.cs
│   │   ├── ProductoConInventario.cs
│   │   └── InventarioUbicacion.cs
│   ├── Filtro/                          # Filtros de autenticación
│   │   └── AutenticadoAttribute.cs
│   ├── Helpers/
│   │   └── ModelStateExtensions.cs
│   ├── wwwroot/                         # Archivos estáticos
│   │   ├── css/
│   │   ├── js/
│   │   └── lib/
│   ├── Program.cs
│   └── appsettings.json
│
├── Scripts/                             # Scripts SQL
│   ├── SCRIPT-POBLADO-COMPLETO.sql
│   └── Migrations/
│
├── Docs/                                # Documentación
│   ├── GUIA-INSTALACION.md
│   ├── API-ENDPOINTS.md
│   └── ROLES-PERMISOS.md
│
├── .gitignore
├── README.md
└── LICENSE
```

---

## 🗄️ Modelo de Datos

### Entidades Principales

```
┌─────────────┐       ┌─────────────┐       ┌─────────────┐
│  Empresas   │───┬───│   Sedes     │───┬───│Dependencias │
└─────────────┘   │   └─────────────┘   │   └─────────────┘
                  │                      │
                  │   ┌─────────────┐   │
                  └───│  Usuarios   │───┘
                      └─────────────┘
                      
┌─────────────┐       ┌─────────────────────┐       ┌─────────────┐
│  Productos  │───────│InventarioDependencia│───────│Dependencias │
└─────────────┘       └─────────────────────┘       └─────────────┘
                                │
                                │
                      ┌─────────────────────┐
                      │MovimientoInventario │
                      └─────────────────────┘
                      
┌─────────────┐       ┌─────────────┐       ┌─────────────┐
│  Clientes   │───────│   Pedidos   │───────│   Usuarios  │
└─────────────┘       └─────────────┘       └─────────────┘
                              │
                              │
                      ┌───────┴────────┐
                      │                │
              ┌───────────────┐ ┌──────────┐
              │DetallesPedido │ │ Entregas │
              └───────────────┘ └──────────┘
                      │              │
                      │       ┌──────────┐
                      └───────│ Facturas │
                              └──────────┘
```

### Relaciones Clave

- **Sedes** tienen múltiples **Dependencias**
- **Productos** pueden estar en múltiples **Dependencias** (InventarioDependencia)
- **Usuarios** pueden pertenecer a una **Sede** y/o **Dependencia**
- **TransferenciaStock** mueve productos entre **Dependencias**
- **Pedidos** generan **Entregas** y **Facturas**

---

## 🔌 API Endpoints

### Base URL
```
http://localhost:5192/api
```

### Productos
```http
GET    /api/Productos                    # Listar todos
GET    /api/Productos/{id}               # Obtener por ID
POST   /api/Productos                    # Crear
PUT    /api/Productos/{id}               # Actualizar
DELETE /api/Productos/{id}               # Eliminar
GET    /api/Productos/StockBajo          # Productos con stock bajo
GET    /api/Productos/{id}/Disponibilidad # Ver disponibilidad
```

### Sedes
```http
GET    /api/Sedes                        # Listar todas
GET    /api/Sedes/{id}                   # Obtener por ID
POST   /api/Sedes                        # Crear
PUT    /api/Sedes/{id}                   # Actualizar
GET    /api/Sedes/{id}/Dependencias      # Dependencias de una sede
GET    /api/Sedes/{id}/Inventario        # Inventario consolidado
```

### Dependencias
```http
GET    /api/Dependencias                 # Listar todas
GET    /api/Dependencias/{id}            # Obtener por ID
POST   /api/Dependencias                 # Crear
PUT    /api/Dependencias/{id}            # Actualizar
GET    /api/Dependencias/{id}/Inventario # Inventario de la dependencia
GET    /api/Dependencias/{id}/StockBajo  # Productos con stock bajo
```

### Inventario
```http
GET    /api/InventarioDependencia                          # Listar todo
GET    /api/InventarioDependencia/{id}                     # Obtener por ID
POST   /api/InventarioDependencia                          # Crear
PUT    /api/InventarioDependencia/{id}/AjustarStock        # Ajustar stock
GET    /api/InventarioDependencia/Consolidado              # Vista consolidada
```

### Transferencias
```http
GET    /api/TransferenciaStock                   # Listar todas
GET    /api/TransferenciaStock/{id}              # Obtener por ID
POST   /api/TransferenciaStock/Solicitar         # Solicitar transferencia
PUT    /api/TransferenciaStock/{id}/Aprobar      # Aprobar
PUT    /api/TransferenciaStock/{id}/Rechazar     # Rechazar
PUT    /api/TransferenciaStock/{id}/Ejecutar     # Ejecutar
DELETE /api/TransferenciaStock/{id}/Cancelar     # Cancelar
```

### Pedidos
```http
GET    /api/Pedidos                      # Listar todos
GET    /api/Pedidos/{id}                 # Obtener por ID
POST   /api/Pedidos                      # Crear
PUT    /api/Pedidos/{id}                 # Actualizar
PUT    /api/Pedidos/{id}/Estado          # Cambiar estado
```

### Usuarios
```http
GET    /api/Usuarios                     # Listar todos
GET    /api/Usuarios/{id}                # Obtener por ID
POST   /api/Usuarios                     # Crear
PUT    /api/Usuarios/{id}                # Actualizar
DELETE /api/Usuarios/{id}                # Eliminar
POST   /api/Usuarios/Login               # Autenticar
PUT    /api/Usuarios/{id}/AsignarSede    # Asignar a sede
GET    /api/Usuarios/Roles               # Listar roles
```

**Swagger UI:** `http://localhost:5192/swagger`

---

## 🔐 Roles y Permisos

### Jerarquía de Roles

```
SuperAdmin (Acceso Total)
    │
    ├─► AdminSede (Gestiona una sede)
    │       │
    │       └─► EncargadoDependencia (Gestiona una dependencia)
    │
    ├─► Cliente (Realiza pedidos)
    │
    └─► Legacy Roles:
            ├─► Administrador (Sistema anterior)
            └─► Empleado (Sistema anterior)
```

### Permisos por Rol

| Funcionalidad | SuperAdmin | AdminSede | Encargado | Cliente | Admin | Empleado |
|---------------|:----------:|:---------:|:---------:|:-------:|:-----:|:--------:|
| Ver Productos | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ |
| Crear Productos | ✅ | ❌ | ❌ | ❌ | ✅ | ❌ |
| Ver Sedes | ✅ | ✅ | ❌ | ❌ | ✅ | ❌ |
| Gestionar Sedes | ✅ | ❌ | ❌ | ❌ | ❌ | ❌ |
| Ver Dependencias | ✅ | ✅ (su sede) | ✅ (su dep) | ❌ | ✅ | ❌ |
| Crear Dependencias | ✅ | ✅ | ❌ | ❌ | ✅ | ❌ |
| Solicitar Transferencia | ✅ | ✅ | ✅ | ❌ | ✅ | ❌ |
| Aprobar Transferencia | ✅ | ✅ | ❌ | ❌ | ✅ | ❌ |
| Ver Pedidos | ✅ | ✅ | ✅ | ✅ (suyos) | ✅ | ✅ |
| Crear Pedidos | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ |
| Gestionar Usuarios | ✅ | ❌ | ❌ | ❌ | ✅ | ❌ |

---

## 🎯 Funcionalidades Principales

### 1. Dashboard Contextual

El dashboard se adapta según el rol y ubicación del usuario:

- **SuperAdmin:** Ve estadísticas globales de todas las sedes
- **AdminSede:** Ve solo su sede asignada
- **EncargadoDependencia:** Ve solo su dependencia

**Métricas mostradas:**
- Total de productos en inventario
- Stock total en unidades
- Pedidos del día actual
- Productos con stock bajo

### 2. Gestión de Productos

**Tipos de Productos:**
- **Compartibles:** Pueden transferirse entre sedes (ej: Papelería, Cafetería)
- **Exclusivos:** Permanecen en su ubicación (ej: Equipos de Laboratorio)

**Características:**
- Código autogenerado (PAP-001, CAF-002, etc.)
- Categorización por tipo
- Control de precio y unidad de medida
- Stock mínimo configurable por ubicación

### 3. Sistema de Transferencias

**Flujo completo:**

```
1. SOLICITUD
   ├─ Encargado de origen solicita transferencia
   ├─ Sistema valida disponibilidad de stock
   └─ Transferencia queda en estado "Pendiente"

2. APROBACIÓN
   ├─ AdminSede o SuperAdmin revisa solicitud
   ├─ Puede aprobar o rechazar
   └─ Si aprueba, pasa a estado "Aprobada"

3. EJECUCIÓN
   ├─ AdminSede ejecuta la transferencia
   ├─ Stock se descuenta de origen
   ├─ Stock se incrementa en destino
   └─ Estado final: "Ejecutada"
```

**Validaciones:**
- Solo productos compartibles pueden transferirse
- Verificación de stock disponible
- Historial completo de movimientos
- Timeline del proceso

### 4. Alertas de Stock Bajo

El sistema monitorea constantemente el inventario:

- **Comparación:** `Stock Actual < Stock Mínimo`
- **Alertas visuales:** Badges rojos en listados
- **Notificaciones:** Dashboard muestra productos críticos
- **Filtros:** Vista específica de stock bajo por dependencia

### 5. Gestión de Pedidos

**Estados de Pedido:**
1. **Pendiente:** Recién creado, esperando procesamiento
2. **Procesado:** Aprobado y listo para entrega
3. **Entregado:** Completado con entrega confirmada

**Funciones:**
- Agregar múltiples productos al pedido
- Calcular subtotales automáticamente
- Generar factura con total
- Programar fecha de entrega
- Tracking de estados

### 6. Reportes y Consultas

**Inventario Consolidado:**
```sql
-- Ejemplo: Ver stock total de un producto en todas las ubicaciones
GET /api/Productos/{id}/Disponibilidad
```

**Movimientos de Inventario:**
```sql
-- Tipos de movimiento:
- Entrada Inicial
- Entrada por Compra
- Salida por Venta
- Transferencia Enviada
- Transferencia Recibida
- Ajuste de Inventario
```

**Historial Completo:**
- Fecha y hora de cada movimiento
- Usuario responsable
- Cantidad y ubicación
- Motivo del movimiento

---

## 📸 Capturas de Pantalla

### Dashboard Principal
```
┌─────────────────────────────────────────────────┐
│  Total Productos    Stock Total    Pedidos Hoy  │
│       30              2,990            5         │
└─────────────────────────────────────────────────┘
```

### Lista de Productos
```
┌────────────────────────────────────────────────┐
│ PAP-001 │ Papel Bond Carta  │ Stock: 650 │ ✓  │
│ CAF-001 │ Café Premium      │ Stock: 230 │ ✓  │
│ LAB-005 │ Centrífuga        │ Stock:   1 │ ⚠️  │
└────────────────────────────────────────────────┘
```

### Transferencias
```
┌─────────────────────────────────────────────────┐
│ Producto: Bolígrafos Azul                       │
│ Origen: Almacén General (200 disponibles)       │
│ Destino: Papelería Palogrande                   │
│ Cantidad: [___] unidades                        │
│ [ Solicitar Transferencia ]                     │
└─────────────────────────────────────────────────┘
```

---

## 🧪 Testing

### Pruebas Manuales

**1. Probar Login:**
```bash
curl -X POST http://localhost:5192/api/Usuarios/Login \
  -H "Content-Type: application/json" \
  -d '{
    "correo": "admin@ucaldas.edu.co",
    "contrasena": "admin123"
  }'
```

**2. Obtener Productos:**
```bash
curl -X GET http://localhost:5192/api/Productos
```

**3. Ver Inventario de Sede:**
```bash
curl -X GET http://localhost:5192/api/Sedes/1/Inventario
```

### Datos de Prueba

El script `SCRIPT-POBLADO-COMPLETO.sql` incluye:
- 5 Sedes con información real de Universidad de Caldas
- 15 Dependencias distribuidas
- 30 Productos de diferentes categorías
- 10 Usuarios con diferentes roles
- 42 Registros de inventario
- 5 Pedidos de ejemplo
- 5 Transferencias en diferentes estados

---

## 🤝 Contribuir

¡Las contribuciones son bienvenidas! Por favor sigue estos pasos:

1. **Fork** el repositorio
2. Crea una **rama** para tu feature (`git checkout -b feature/AmazingFeature`)
3. **Commit** tus cambios (`git commit -m 'Add some AmazingFeature'`)
4. **Push** a la rama (`git push origin feature/AmazingFeature`)
5. Abre un **Pull Request**

### Guías de Estilo

- **C#:** Seguir convenciones de [Microsoft C# Coding Conventions](https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)
- **Commits:** Usar [Conventional Commits](https://www.conventionalcommits.org/)
- **Nombres:** PascalCase para clases, camelCase para variables

### Reportar Bugs

Usa el sistema de [Issues](https://github.com/usuario/proyecto/issues) con:
- Descripción clara del problema
- Pasos para reproducir
- Comportamiento esperado vs actual
- Screenshots si aplica
- Información del entorno (.NET version, SO, etc.)

---

## 📄 Licencia

Este proyecto está bajo la Licencia MIT - ver el archivo [LICENSE](LICENSE) para más detalles.

```
MIT License

Copyright (c) 2025 Universidad de Caldas

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction...
```

---

## 👨‍💻 Autores

- **Juan Diego Blandon Toro** - *Desarrollador* - Universidad de Caldas
- **Juan Camilo Salazar Osorio** - *Desarrollador* - Universidad de Caldas
- **Juan Estevan Zapata Correa** - *Desarrollador* - Universidad de Caldas
- **Sebastian Rendon Giraldo** - *Desarrollador* - Universidad de Caldas
---

## 🙏 Agradecimientos

- Universidad de Caldas por el apoyo al proyecto
- Comunidad .NET por las excelentes herramientas
- Bootstrap team por el framework de UI
- Stack Overflow por resolver todas las dudas 😄

---

## 📚 Recursos Adicionales

- [Documentación .NET](https://docs.microsoft.com/dotnet/)
- [Entity Framework Core](https://docs.microsoft.com/ef/core/)
- [ASP.NET Core](https://docs.microsoft.com/aspnet/core/)
- [Bootstrap 5](https://getbootstrap.com/docs/5.3/)
- [SQL Server](https://docs.microsoft.com/sql/)

---

## 🗺️ Roadmap

### Versión 2.0 (Q1 2025)
- [ ] Aplicación móvil (React Native)
- [ ] Reportes en PDF
- [ ] Integración con código de barras
- [ ] Notificaciones por email
- [ ] Dashboard con gráficos (Chart.js)

### Versión 2.1 (Q2 2025)
- [ ] API GraphQL
- [ ] Integración con proveedores externos
- [ ] Sistema de órdenes de compra automáticas
- [ ] Predicción de demanda con ML
- [ ] Auditoría completa de cambios

### Versión 3.0 (Q3 2025)
- [ ] Arquitectura de microservicios
- [ ] Contenedores Docker
- [ ] CI/CD con GitHub Actions
- [ ] Deploy en Azure
- [ ] Multi-tenancy para otras universidades

---

## ❓ FAQ

### ¿Cómo cambio el puerto del backend?
Edita `Properties/launchSettings.json` y cambia el puerto en `applicationUrl`.

### ¿Puedo usar otra base de datos?
Sí, Entity Framework Core soporta PostgreSQL, MySQL, SQLite. Cambia el proveedor en `Program.cs`.

### ¿Cómo agrego una nueva sede?
Desde la interfaz con rol SuperAdmin, o ejecutando:
```sql
INSERT INTO Sedes (Nombre, Codigo, Direccion, Estado) 
VALUES ('Nueva Sede', 'NUEVA', 'Dirección', 1);
```

### ¿Cómo reseteo las migraciones?
```bash
dotnet ef database drop
dotnet ef migrations remove
dotnet ef migrations add InitialCreate
dotnet ef database update
```

---