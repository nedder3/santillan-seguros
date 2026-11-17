<div align="center">

# Federación Santillán Seguros
### Sistema de Gestión de Cartera, Cobranzas y Alertas de Vencimientos

![C# .NET 8](https://img.shields.io/badge/C%23-.NET%208%20WinForms-512BD4?style=for-the-badge&logo=dotnet)
![MariaDB](https://img.shields.io/badge/MariaDB-11.4%20(3FN)-003545?style=for-the-badge&logo=mariadb)
![Architecture](https://img.shields.io/badge/Arquitectura-MVC%203%20Capas-0F4C81?style=for-the-badge)
![Docker](https://img.shields.io/badge/Docker-Compose-2496ED?style=for-the-badge&logo=docker)
![Conventional Commits](https://img.shields.io/badge/Conventional%20Commits-1.0.0-FE5196?style=for-the-badge)

**Proyecto Final 2026 — I.S.F.T. N° 191 (Bahía Blanca)**  
*Desarrollado para Productor Asesor de Seguros (PAS) operando con Federación Patronal Seguros S.A.*

</div>

---

## Resumen Ejecutivo

Sistema de escritorio desarrollado en **C# (.NET 8 Windows Forms)** con persistencia en **MariaDB** bajo arquitectura **MVC en 3 Capas** (`Views WinForms` $\rightarrow$ `Controllers` $\rightarrow$ `Data DAOs` + `Core Entidades`). Resuelve la gestión diaria de cartera de asegurados, emisión transaccional de pólizas con generación automática de plan de cuotas y un **tablero en vivo de alertas de mora (cuotas vencidas) y vencimientos próximos (15 días)**.

### Fundamentación de Negocio y Estándares Públicos
* **Marco Legal Argentino:** [Ley Nacional N° 22.400](http://servicios.infoleg.gob.ar/infolegInternet/anexos/20000-24999/20968/texact.htm) (Régimen de Productores Asesores de Seguros - SSN).
* **Modelo de Datos (DER 3FN):** Basado en **Database Answers Canonical Insurance Model** y estándar internacional **ACORD** (`Party/Asegurado` 1:N `Policy/Poliza` 1:N `Installment/Cuota` 1:N `Notificacion`).

---

## Arquitectura 3 Capas + MVC

```text
┌─────────────────────────────────────────────────────────────────────────┐
│ CAPA VISTA (FederacionSantillan.App/Views - WinForms Designer)          │
│ FormLogin • FormRegistroUsuario • FormPrincipal (Alertas/ABM/Pólizas)   │
└───────────────────────────────────┬─────────────────────────────────────┘
                                    │ Invoca acciones
                                    ▼
┌─────────────────────────────────────────────────────────────────────────┐
│ CAPA CONTROLADOR (FederacionSantillan.App/Controllers - MVC)            │
│ AuthController • CarteraController • NotificacionController             │
└───────────────────┬───────────────────────────────────┬─────────────────┘
                    │ Orquesta Entidades                │ Consulta SQL
                    ▼                                   ▼
┌───────────────────────────────────┐   ┌─────────────────────────────────┐
│ MODELO DOMINIO (.Core)            │   │ ACCESO A DATOS (.Data/DAOs)     │
│ Usuario, Asegurado, Ramo, Poliza, │   │ ConnectionFactory (MySqlConn.)  │
│ Cuota, Notificacion, Fecha        │   │ UsuarioDAO, AseguradoDAO,       │
│ (copy / clone / equals prof.)     │   │ RamoDAO, PolizaDAO, CuotaDAO    │
└───────────────────────────────────┘   └───────────────┬─────────────────┘
                                                        ▼
                                            ┌───────────────────────┐
                                            │ MariaDB 11.4 (Docker) │
                                            └───────────────────────┘
```

---

## Guía de Inicio Rápido (Docker + DBeaver + Visual Studio)

### 1. Levantar MariaDB con Docker
```bash
cd docker
docker compose up -d
```
*(O importar `database/santillan_seguros_completo.sql` directamente en **DBeaver**).*

### 2. Credenciales de Acceso de Prueba
* **Usuario Productor:** `santillan` / **Contraseña:** `seguros2026`
* **Usuario Admin:** `admin` / **Contraseña:** `admin123`

### 3. Compilar y Ejecutar en Visual Studio (Windows)
1. Abrir `FederacionSantillan.sln` en Visual Studio 2022+.
2. Establecer `FederacionSantillan.App` como proyecto de inicio (*Startup Project*).
3. Restaurar paquetes NuGet (`MySqlConnector`) y presionar `F5`.

---

## Documentación y Diagramas (Listos para PDF)
* `diagramas/*.drawio`: DER (3FN), Diagrama de Clases UML (MVC), Casos de Uso, Secuencia y Contexto.
* `docs/01_Estudio_Factibilidad_y_Requerimientos.md`
* `docs/02_Fundamentacion_DER_y_Diccionario.md`
* `docs/03_Manual_Usuario_y_Guia_Docker.md`
