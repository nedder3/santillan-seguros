# 1. ESTUDIO DE FACTIBILIDAD Y ANÁLISIS DE REQUERIMIENTOS

**Proyecto:** Sistema de Gestión de Cartera, Cobranzas y Alertas de Vencimientos — **Federación Santillán Seguros**  
**Entidad Real:** Productor Asesor de Seguros (PAS) operando con **Federación Patronal Seguros S.A.**  
**Institución:** I.S.F.T. N° 191 (Bahía Blanca) — TS en Desarrollo de Software (Ciclo Lectivo 2026)

---

## 1. Entrevista y Relevamiento de Situación Actual

### A. ¿Qué datos e información mantiene actualmente y cómo?
Actualmente, el productor asesor mantiene el seguimiento de su cartera mediante **planillas de cálculo dispersas (Excel)** y anotaciones manuales/mensajes de WhatsApp para recordar qué clientes tienen cuotas próximas a vencer o pólizas impagas.

### B. Problemas detectados (Información duplicada / inconsistente)
* Duplicación de datos de contacto de asegurados en múltiples hojas de cálculo por mes.
* Falta de visibilidad consolidada de cuotas **vencidas** (morosos en riesgo de suspensión de cobertura) vs cuotas **próximas a vencer** (15 días).
* Imposibilidad de auditar rápidamente qué usuario registró un cobro o cuándo se emitió una póliza.

### C. Requerimientos Funcionales Acordados
1. **RF-01 (Seguridad):** Inicio de sesión con usuario y contraseña enmascarada (SHA-256) y alta de nuevos usuarios con roles (`ADMIN`, `PRODUCTOR`, `OPERADOR`).
2. **RF-02 (Cartera de Asegurados):** ABM completo (Alta, Baja lógica, Modificación y Búsqueda en tiempo real) de asegurados.
3. **RF-03 (Emisión de Pólizas y Cuotas):** Alta de pólizas asociadas a un asegurado y un ramo, generando automáticamente el plan de `N` cuotas mensuales en una transacción atómica.
4. **RF-04 (Panel de Notificaciones y Alertas):** Visualización automática al ingresar al sistema de:
   * Cuotas **vencidas** (días de atraso, teléfono y monto adeudado).
   * Cuotas **por vencer** en los próximos 15 días.
5. **RF-05 (Registro de Pagos):** Imputación rápida de pago de cuota desde el panel de alertas, actualizando estado, fecha y medio de pago.

---

## 2. Estudio de Factibilidad

### A. Factibilidad Técnica
* **Lenguaje y Framework:** C# (.NET 8) con Windows Forms (WinForms), nativo para sistemas de escritorio Windows y compatible con Visual Studio Form Designer.
* **Base de Datos:** MariaDB 11.4 (SGBD relacional open-source), administrado con **DBeaver** y desplegable de forma reproducible mediante **Docker**.
* **Arquitectura:** Patrón **MVC en 3 Capas** (`Views` WinForms $\rightarrow$ `Controllers` $\rightarrow$ `Data` DAOs + `Core` Entidades de Dominio).

### B. Factibilidad Operativa
La interfaz está diseñada específicamente para la operativa diaria de una oficina de seguros: al iniciar sesión, la primera pantalla expone inmediatamente los morosos y vencimientos cercanos sin navegación compleja, con navegación fluida por tabulador (`TabIndex`).

### C. Factibilidad Económica
Costo de licenciamiento **$0**: tanto .NET 8 SDK, MariaDB, Docker Desktop y DBeaver poseen licencias gratuitas/open-source para desarrollo y despliegue local.
