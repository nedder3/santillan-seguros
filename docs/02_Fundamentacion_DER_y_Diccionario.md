# 2. FUNDAMENTACIÓN DEL MODELO ENTIDAD-RELACIÓN (DER) Y DICCIONARIO DE DATOS

## 1. Fuentes Públicas y Estándares de Referencia (Respaldo Académico e Industrial)

El diseño de la base de datos `santillan_seguros` (normalizada en **Tercera Forma Normal - 3FN**) no es arbitrario; se fundamenta en tres fuentes públicas verificables del dominio asegurador:

1. **Marco Legal Argentino — Ley N° 22.400 (Régimen de Productores Asesores de Seguros - SSN):**
   * Fuente oficial: [InfoLEG - Ministerio de Justicia de la Nación](http://servicios.infoleg.gob.ar/infolegInternet/anexos/20000-24999/20968/texact.htm)
   * Regula la intermediación del Productor Asesor de Seguros (PAS) ante la **Superintendencia de Seguros de la Nación (SSN)** y las compañías aseguradoras (ej. **Federación Patronal Seguros S.A.**), exigiendo el registro trazable de asegurados, pólizas emitidas por ramo y cobranzas.
2. **Database Answers Canonical Insurance Model (*Insurance Policies - Car, Home and Life*):**
   * Modelo de referencia público para sistemas de seguros que establece la estructura jerárquica estándar: `Customer/Party` $\rightarrow$ `Policy` $\rightarrow$ `Policy_Installments/Billing` $\rightarrow$ `Payments`.
3. **Estándar Internacional ACORD (*Insurance Data Model*):**
   * Define la separación estricta entre la entidad **Póliza** (contrato de cobertura) y las entidades de **Facturación/Cuotas** (`Billing/Installment`), evitando almacenar montos o estados de pago repetidos dentro de la tabla maestra de pólizas.

---

## 2. Justificación de Cada Relación del DER

| Relación | Cardinalidad | Fundamentación Técnica y de Negocio |
| :--- | :---: | :--- |
| **`asegurados` $\rightarrow$ `polizas`** | **1 : N** | Un asegurado (persona física o jurídica identificada por `dni_cuit`) puede contratar múltiples pólizas vigentes o históricas (ej. auto + hogar + vida). Cada póliza pertenece a un único titular (`FK id_asegurado`, `ON DELETE RESTRICT` para preservar integridad histórica). |
| **`ramos` $\rightarrow$ `polizas`** | **1 : N** | Normalización 3FN según clasificación oficial de la SSN (Automotor, Combinado Familiar, Vida, RC, Caución). Evita redundancia textual en la tabla `polizas` mediante `FK id_ramo`. |
| **`polizas` $\rightarrow$ `cuotas`** | **1 : N** | Según estándar ACORD Billing, una póliza anual o semestral se fracciona en `N` cuotas mensuales independientes (`numero_cuota`, `fecha_vencimiento`, `estado`). Restricción `UNIQUE (id_poliza, numero_cuota)` impide cuotas duplicadas. |
| **`cuotas` $\rightarrow$ `notificaciones`** | **1 : N** | Una cuota próxima a vencer o en mora puede disparar uno o más registros de alerta (`POR_VENCER`, `VENCIDA`) para auditar cuándo fue notificado el cliente. |
| **`usuarios`** | **Independiente** | Entidad de control de acceso y auditoría de operadores (`ADMIN`, `PRODUCTOR`, `OPERADOR`), almacenando contraseñas con hash SHA-256 (`password_hash`). |

---

## 3. Diccionario de Datos (Tablas MariaDB)

* **`usuarios`:** `id_usuario` (PK, INT AI), `username` (VARCHAR 50, UQ), `password_hash` (VARCHAR 64), `nombre_completo` (VARCHAR 120), `rol` (ENUM), `activo` (BOOLEAN), `fecha_creacion` (DATETIME).
* **`ramos`:** `id_ramo` (PK, INT AI), `codigo` (VARCHAR 20, UQ), `nombre` (VARCHAR 80), `descripcion` (VARCHAR 255).
* **`asegurados`:** `id_asegurado` (PK, INT AI), `dni_cuit` (VARCHAR 20, UQ), `apellido_nombre` (VARCHAR 120), `telefono` (VARCHAR 30), `email` (VARCHAR 120), `domicilio` (VARCHAR 180), `activo` (BOOLEAN).
* **`polizas`:** `id_poliza` (PK, INT AI), `numero_poliza` (INT, UQ), `id_asegurado` (FK), `id_ramo` (FK), `compania` (VARCHAR 80), `bien_asegurado` (VARCHAR 180), `prima_total` (DECIMAL 12,2), `cantidad_cuotas` (INT), `fecha_inicio` (DATE), `fecha_fin` (DATE), `estado` (ENUM).
* **`cuotas`:** `id_cuota` (PK, INT AI), `id_poliza` (FK), `numero_cuota` (INT), `monto` (DECIMAL 12,2), `fecha_vencimiento` (DATE), `estado` (ENUM), `fecha_pago` (DATETIME), `metodo_pago` (VARCHAR 50), `observaciones` (VARCHAR 255).
* **`notificaciones`:** `id_notificacion` (PK, INT AI), `id_cuota` (FK), `tipo_alerta` (ENUM), `mensaje` (VARCHAR 255), `fecha_generacion` (DATETIME), `leida` (BOOLEAN).

---

## 4. Especificación Estructurada de Procesos (DEP)

* **DEP-01 (Emisión de Póliza):** Validar número único $\rightarrow$ Iniciar transacción SQL $\rightarrow$ Insertar en `polizas` $\rightarrow$ Bucle `i = 1..cantidad_cuotas` calculando `monto = prima_total / cantidad_cuotas` y `fecha_vencimiento = primer_vto + (i-1) meses` $\rightarrow$ Insertar en `cuotas` $\rightarrow$ Commit.
* **DEP-02 (Motor de Alertas):** Sincronizar cuotas `PENDIENTE` con `fecha_vencimiento < CURDATE()` a `VENCIDA` $\rightarrow$ Consultar `VENCIDA` (rojo) y `PENDIENTE` dentro de los próximos 15 días (amarillo) $\rightarrow$ Mostrar en panel principal.
* **DEP-03 (Imputación de Pago):** Seleccionar cuota $\rightarrow$ Confirmar medio de pago $\rightarrow$ Ejecutar `UPDATE cuotas SET estado='PAGADA', fecha_pago=NOW()` $\rightarrow$ Refrescar KPIs y grillas.
