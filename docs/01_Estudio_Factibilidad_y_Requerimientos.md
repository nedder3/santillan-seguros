# 1. ESTUDIO DE FACTIBILIDAD Y ANÁLISIS DE REQUERIMIENTOS

**Proyecto:** Sistema de Gestión de Cartera, Cobranzas y Alertas de Vencimientos — **Federación Santillán Seguros**  
**Entidad Real:** Productor Asesor de Seguros (PAS) operando con **Federación Patronal Seguros S.A.**  
**Institución:** I.S.F.T. N° 191 (Bahía Blanca) — TS en Desarrollo de Software (Ciclo Lectivo 2026)

---

## 1. Entrevista y Relevamiento de Situación Actual (Preguntas de Cátedra)

A continuación se documentan las respuestas relevadas durante la entrevista inicial con el Productor Asesor de Seguros titular de **Federación Santillán Seguros** (operador comercial de **Federación Patronal Seguros S.A.** en Bahía Blanca):

### 1. ¿Qué datos o información mantienen actualmente?
Actualmente mantienen los datos filiatorios y de contacto de los clientes asegurados (Nombre y Apellido / Razón Social, DNI/CUIT, Teléfono móvil para WhatsApp, Correo electrónico y Domicilio legal), el registro de las pólizas contratadas en **Federación Patronal Seguros S.A.** (Número de póliza oficial, Ramo de cobertura —Automotor, Vivienda, Vida, Comercio, Caución—, Bien u objeto asegurado como patente o ubicación del riesgo, Vigencia desde/hasta y Prima total), y el cronograma mensual de cuotas de cobranza (número de cuota, fecha de vencimiento, importe, estado de pago y medio de cobro).

### 2. ¿Cómo se recopila y se almacena esta información en la actualidad?
La información se recopila de manera fragmentada: las altas de clientes y emisiones se transcriben manualmente en **planillas de cálculo dispersas (archivos Excel mensuales)** y cuadernos físicos de caja diaria, mientras que los avisos de vencimiento y comprobantes de pago se buscan de forma manual en el historial de chats de **WhatsApp** del teléfono de la agencia.

### 3. ¿Quiénes utilizan esta información y con qué frecuencia?
La información es utilizada **a diario y de forma continua** por dos perfiles dentro de la oficina:
* **El Productor Asesor de Seguros (`PRODUCTOR` / `ADMIN`):** Consulta la cartera completa todas las mañanas para auditar la morosidad crítica (clientes con riesgo de suspensión de cobertura por falta de pago), emitir nuevas pólizas y realizar respaldos contables.
* **El Operador Administrativo de Cobranzas (`OPERADOR`):** Utiliza el listado durante toda la jornada comercial para atender consultas telefónicas, imputar pagos por caja o transferencia y enviar recordatorios de vencimiento por WhatsApp.

### 4. ¿Existen problemas con la información duplicada o inconsistente?
**Sí, es el principal problema operativo de la oficina.** Al mantener una planilla Excel distinta por cada mes de cobranza:
* Los teléfonos y domicilios de un mismo asegurado aparecen duplicados en múltiples hojas; si un cliente cambia su número de celular y se actualiza en una sola hoja, en los meses siguientes se pierde el contacto.
* Existen inconsistencias frecuentes entre las cuotas marcadas como cobradas en el cuaderno diario y las planillas mensuales, provocando reclamos a clientes que ya habían abonado o, peor aún, omisión de aviso a clientes cuya póliza entra en suspensión de cobertura.

### 5. ¿Qué tipo de consultas o reportes necesitan hacer con los datos?
Necesitan cuatro consultas y reportes fundamentales en tiempo real:
1. **Panel de Alertas de Morosidad (Cuotas Vencidas):** Listado inmediato al abrir el sistema con todas las cuotas cuyo vencimiento ya operó y siguen impagas, indicando días de atraso, monto, teléfono y acceso directo para enviar recordatorio por WhatsApp.
2. **Panel de Prevención (Próximos Vencimientos a 15 días):** Listado de cuotas que vencerán en las próximas dos semanas para gestionar la cobranza preventiva.
3. **Búsqueda Instantánea de Cartera:** Filtrado en tiempo real por DNI/CUIT, Apellido o Número de Póliza, junto con el desglose de todas las cuotas asociadas a cada póliza.
4. **Exportación Contable y Respaldo:** Exportación de reportes tabulares a formato **CSV** (compatible con Excel) y generación de copias de seguridad completas de la base de datos (**SQL DUMP**).

### 6. ¿Qué nivel de seguridad o privacidad requieren para ciertos datos?
Por tratarse de información financiera, patrimonial y datos personales alcanzados por la **Ley N° 25.326 de Protección de Datos Personales** y el **Régimen de Productores Asesores de Seguros (Ley N° 22.400 - SSN)**:
* **Autenticación obligatoria:** Ningún operador puede acceder a la cartera sin iniciar sesión con usuario y contraseña almacenada mediante hash criptográfico irreversible (**SHA-256**).
* **Segregación de roles y auditoría:** Cada cobro imputado o notificación emitida debe registrar qué usuario del sistema ejecutó la operación (`collected_by_user_id`).
* **Doble verificación para operaciones críticas:** La generación y descarga de respaldos completos de la base de datos (**SQL DUMP**) requiere re-ingresar y validar la contraseña del usuario activo mediante un diálogo modal de seguridad (`FormVerificarPassword`).

---

## 2. Requerimientos Funcionales Acordados
1. **RF-01 (Seguridad y Auditoría):** Inicio de sesión con contraseña hasheada (SHA-256), alta de usuarios con roles (`ADMIN`, `PRODUCTOR`, `OPERADOR`) y re-autenticación obligatoria para exportar respaldos SQL DUMP.
2. **RF-02 (Cartera de Asegurados):** ABM completo (Alta, Baja lógica, Modificación y Búsqueda en tiempo real) de asegurados validando unicidad de DNI/CUIT.
3. **RF-03 (Emisión de Pólizas y Cuotas):** Alta de pólizas asociadas a un asegurado y un ramo oficial, generando automáticamente el plan de `N` cuotas mensuales en una transacción SQL atómica (`ACID`).
4. **RF-04 (Panel de Notificaciones y Alertas):** Visualización automática al ingresar al sistema de cuotas **vencidas** y cuotas **por vencer** en los próximos 15 días, con disparo de recordatorio por WhatsApp y registro en historial de notificaciones.
5. **RF-05 (Registro de Pagos y Accesibilidad):** Imputación rápida de pago de cuota desde el panel de alertas o de cartera, exportación a CSV y soporte de modo daltónico / alto contraste (**Okabe-Ito WCAG AA**).

---

## 3. Estudio de Factibilidad

### A. Factibilidad Técnica
* **Lenguaje y Framework:** C# 12 (.NET 8) con Windows Forms (WinForms), nativo para sistemas de escritorio Windows y 100% compatible con Visual Studio Form Designer (`*.Designer.cs`).
* **Base de Datos:** MariaDB 11.4 (SGBD relacional open-source en Tercera Forma Normal - 3FN), administrado con **DBeaver** y desplegable de forma reproducible mediante **Docker Compose**.
* **Arquitectura:** Patrón **MVC en 3 Capas** (`FederacionSantillan.App` Views/Controllers/Advice $\rightarrow$ `FederacionSantillan.Data` DAOs/Factories/Translators $\rightarrow$ `FederacionSantillan.Model` Entities/DTOs/Exceptions/Interfaces).

### B. Factibilidad Operativa
La interfaz está diseñada específicamente para la operativa diaria de una agencia de seguros: al iniciar sesión, la primera pantalla expone inmediatamente los morosos y vencimientos cercanos sin navegación compleja, con navegación fluida por teclado (`TabIndex`), atajos de teclado (`F1`, `F5`, `Ctrl+N`, `Ctrl+E`, `Ctrl+B`, `Ctrl+D`) y paleta accesible para daltonismo.

### C. Factibilidad Económica
Costo de licenciamiento **$0**: tanto .NET 8 SDK, Visual Studio Community, MariaDB, Docker Desktop y DBeaver poseen licencias gratuitas/open-source para desarrollo académico y despliegue local.
