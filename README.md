# 📦 Sistema de Gestión de Inventarios Multi-Sede
## Universidad de Caldas

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=flat&logo=.net)
![SQL Server](https://img.shields.io/badge/SQL%20Server-Express-CC2927?style=flat&logo=microsoft-sql-server)
![Bootstrap](https://img.shields.io/badge/Bootstrap-5.3-7952B3?style=flat&logo=bootstrap)
![License](https://img.shields.io/badge/License-MIT-green?style=flat)

Sistema integral de gestión de inventarios diseñado para instituciones educativas con múltiples sedes y dependencias. Permite el control centralizado de productos, transferencias entre ubicaciones, gestión de pedidos y seguimiento en tiempo real del stock.

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
- 3 roles con permisos granulares
- Asignación de usuarios a sedes/dependencias
- Autenticación con hash de contraseñas
- Sistema dual de sesión (UsuarioLogueado + Session)

### 📦 Pedidos
- Creación de pedidos con múltiples productos
- Generación automática de facturas
- Programación de entregas
- Estados: Pendiente → Procesado

### 📈 Reportes y Estadísticas
- Dashboard con métricas en tiempo real
- Productos con stock bajo
- Pedidos recientes
- Transferencias pendientes
- Consolidados por sede

### 🤖 Integración con Inteligencia Artificial
- Recomendaciones y analisis de ventas en tiempo real
- Evaluación de los productos bajos en stock para recomendar reponer los mas importantes
- En el proceso de ventas se van recomendando productos complementarios
---

## 🛠️ Tecnologías

### Backend
- **Framework:** ASP.NET Core 8.0
- **Lenguaje:** C# 12
- **ORM:** Entity Framework Core 8.0
- **Base de Datos:** SQL Server Express 2022
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
# Abrir y ejecutar: Script proyecto inventarios.sql
```

### 4. Configurar Cadenas de Conexión

**Backend - `appsettings.json`:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=SUSERVIDORSQL;Database=ProyectoInventariosDB;Integrated Security=true;TrustServerCertificate=true;"
  }
}
```

**Frontend - `appsettings.json`:**
```json
{
  "ApiUrls": {
    "BaseUrl": "https://localhost:44387/api"
  }
}
```

### 5. Ejecutar el Proyecto

**Terminal 1 - Backend API:**
```bash
cd ProyectoInventariosWebApi
dotnet run
```
API disponible en: `https://localhost:44387`

**Terminal 2 - Frontend MVC:**
```bash
cd ProyectoInventariosWebApp
dotnet run
```
Aplicación disponible en: `http://localhost:44360`
```

### Configuración de Roles Iniciales

El script de poblado crea automáticamente estos usuarios:

| Usuario | Contraseña | Rol |
|---------|-----------|-----|
| admin@ucaldas.edu.co | admin123 | SuperAdmin |
| coord.principal@ucaldas.edu.co | admin123 | AdminSede |
| maria.gonzalez@ucaldas.edu.co | admin123 | EncargadoDependencia |

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

**Swagger UI:** `http://localhost:44387/swagger`

---

## 🔐 Roles y Permisos

### Jerarquía de Roles

```
SuperAdmin (Acceso Total)
    │
    ├─► AdminSede (Gestiona una sede)
            │
            └─► EncargadoDependencia (Gestiona una dependencia)
```

---

## 👨‍💻 Autores

- **Juan Diego Blandon Toro** - *Desarrollador* - Universidad de Caldas
- **Juan Camilo Salazar Osorio** - *Desarrollador* - Universidad de Caldas
- **Juan Estevan Zapata Correa** - *Desarrollador* - Universidad de Caldas
- **Sebastian Rendon Giraldo** - *Desarrollador* - Universidad de Caldas
- **Cristian David** - *Desarrollador* - Universidad de Caldas
---
