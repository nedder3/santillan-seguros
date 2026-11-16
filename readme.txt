================================================================================
CONTEXTO COMPLETO DE SESION (HANDOFF PARA ANTIGRAVITY EN MAC)
Proyecto: Federacion Santillan Seguros (git@github.com:nedder3/santillan-seguros.git)
Fecha limite de entrega / promocion: Jueves 19 de noviembre de 2026
================================================================================

1. PERFIL DEL USUARIO Y CONTEXTO ACADEMICO
--------------------------------------------------------------------------------
- Estudiante: Ariel (10 anos de experiencia profesional en Java/software).
- Diagnostico: Dislexia (por eso se prioriza codigo limpio, sin anidamientos
  innecesarios y con expression bodies `=>` en C#).
- Institucion: I.S.F.T. N° 191 (Bahia Blanca) - TS en Desarrollo de Software 2026.
- Profesor de la catedra: Docente estricto de escuela clasica UNS / Bertrand
  Meyer. Evalua al detalle cada convencion formal y busca errores menores.

2. CONVENCIONES OBLIGATORIAS DEL PROFESOR (NO ROMPER)
--------------------------------------------------------------------------------
- Lenguaje: C# (.NET 8). Nunca usar tipos de Java (`boolean` -> usar `bool`).
- Estilo de Clases (Core):
  * Atributos `private`.
  * Constructores explicitos inicializando el 100% del estado del objeto.
  * Getters y Setters explicitos con expression bodies (`=>`), ej:
    `public int obtenerCodigo() => codigo;`
    `public void establecerNombre(string n) => nombre = n;`
  * Metodos `copy()`, `clone()` y `equals()` implementados EN PROFUNDIDAD cuando
    hay objetos asociados (delegando en `.copy()`, `.clone()` y `.equals()` del
    objeto hijo).
  * IMPORTANTE: Usar siempre terminologia academica sobria y profesional en
    comentarios y docs (NO usar frases informales ni sensacionalistas).
- Arquitectura: Patron MVC en 3 Capas estrictas:
  * Vista: Windows Forms (.NET 8 WinForms) con archivos `*.Designer.cs` para
    que abran en el Form Designer de Visual Studio (NO WPF).
  * Controlador: `AuthController`, `CarteraController`, `NotificacionController`.
  * Modelo: Entidades puras en `FederacionSantillan.Core` + Acceso a Datos
    totalmente desacoplado en `FederacionSantillan.Data` (DAOs con SQL
    parametrizado; las consultas SQL NUNCA van en las Vistas ni en las Entidades).
- Base de Datos: MariaDB administrada con DBeaver (corriendo en Docker en Mac).
- Diagramas: Todos en Draw.io (`.drawio`), NADA de Mermaid en la entrega.
- Documentacion: Lista para exportar a PDF (Factibilidad, Requerimientos,
  DER + Diccionario + DEPs, Manual de Usuario y Guia Docker).

3. NEGOCIO REAL Y RESPALDO PUBLICO DEL DER
--------------------------------------------------------------------------------
- Cliente real: Federacion Santillan, Productor Asesor de Seguros (PAS) que
  opera con Federacion Patronal Seguros S.A.
- Necesidad principal del cliente: Ver alertas/notificaciones de cuotas VENCIDAS
  (morosos) y cuotas POR VENCER (proximos 15 dias), ademas de gestionar
  Asegurados, emitir Polizas con plan de cuotas automatico y registrar pagos.
- Fuentes publicas citadas para blindar el DER ante el profesor:
  1) Ley Nacional N° 22.400 (Regimen de Productores Asesores de Seguros - SSN / InfoLEG).
  2) Database Answers Canonical Insurance Model (Car, Home and Life).
  3) Estandar internacional ACORD (Party/Client -> Policy -> Billing/Installment).

4. LO QUE YA ESTA HECHO Y COMMITEADO EN `main`
--------------------------------------------------------------------------------
- `docker/docker-compose.yml`: Contenedor MariaDB 11.4 en puerto 3306 con
  auto-inicializacion (`01_schema.sql` y `02_seed.sql`).
- `database/01_schema.sql`, `02_seed.sql` y `santillan_seguros_completo.sql`:
  Esquema 3FN (`usuarios`, `ramos`, `asegurados`, `polizas`, `cuotas`,
  `notificaciones`) con fechas relativas a `CURDATE()` para que siempre haya
  morosos y vencimientos en vivo al importar en DBeaver.
  * Usuarios de prueba: `santillan` / `seguros2026` y `admin` / `admin123`.
- `src/FederacionSantillan.Core/`: Entidades `Usuario`, `Ramo`, `Asegurado`,
  `Poliza`, `Cuota`, `Notificacion`, `Fecha` con convenciones de catedra.
- `src/FederacionSantillan.Data/`: `ConnectionFactory` (lee `appsettings.json`)
  + `UsuarioDAO`, `RamoDAO`, `AseguradoDAO`, `PolizaDAO`, `CuotaDAO` usando
  `MySqlConnector` 2.3.7 y transacciones SQL.
- `src/FederacionSantillan.App/`: Migrado de WPF a WinForms `.Designer.cs`:
  * `FormLogin` (enmascaramiento `*`, `TabIndex`, estado DB, iconos).
  * `FormRegistroUsuario` (alta de nuevos usuarios en MariaDB).
  * `FormPrincipal` (Tab Alertas con KPIs + cobro rapido, Tab Asegurados ABM,
    Tab Polizas con generacion de cuotas, manejo global `try/catch`).
- `diagramas/`: `01_DER_santillan_seguros.drawio`, `02_Diagrama_Clases_MVC.drawio`,
  `03_Casos_de_Uso.drawio`, `04_Diagrama_Secuencia.drawio`, `05_Diagrama_Contexto.drawio`.
- `docs/`: `01_Estudio_Factibilidad_y_Requerimientos.md`,
  `02_Fundamentacion_DER_y_Diccionario.md`, `03_Manual_Usuario_y_Guia_Docker.md`.

5. PLAN DE LO QUE QUEDA POR HACER EN MAC + VM WINDOWS (ANTES DEL JUEVES 19/11)
--------------------------------------------------------------------------------
1) En Mac: Instalar/abrir Docker Desktop y levantar MariaDB:
   `cd docker && docker compose up -d`
   Verificar logs y conectar DBeaver a `localhost:3306` (user `santillan` / pass `santillan123`).
2) En VM Windows: Configurar `src/FederacionSantillan.App/appsettings.json`
   apuntando `Server=` a la IP del host Mac (o localhost si hay port-forward).
3) En Visual Studio (VM Windows): Abrir `FederacionSantillan.sln`, restaurar NuGet
   (`MySqlConnector`), comprobar que el Form Designer abre los 3 Forms sin errores,
   compilar y correr (`F5`).
4) Probar cada punto de la Rubrica de Cotejo:
   - Login con `santillan` / `seguros2026` y enmascaramiento `*`.
   - Registro de un usuario nuevo desde `FormRegistroUsuario`.
   - Navegacion fluida con tecla `TAB` en todos los formularios.
   - Alta/edicion de Asegurado, emision de Poliza con cuotas, y cobro desde Alertas.
   - Ejecutar `dotnet test` sobre `FederacionSantillan.Tests` (verificar que
     compilen y pasen todos los tests unitarios tras la actualizacion de modelos).
5) Exportacion a PDF:
   - Abrir los 5 archivos `.drawio` de `diagramas/` en Draw.io y exportarlos a PDF.
   - Exportar los documentos de `docs/` a PDF para entregar la documentacion digital.
6) Entrega final:
   - Verificar que el docente este agregado como colaborador en GitHub
     (`git@github.com:nedder3/santillan-seguros.git`).
   - Hacer push final si se ajusto `appsettings.json` o tests.
================================================================================
