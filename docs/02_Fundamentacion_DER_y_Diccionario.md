# 2. FUNDAMENTACIÓN DEL MODELO ENTIDAD-RELACIÓN (DER) Y DICCIONARIO DE DATOS

## 1. Fuentes Públicas y Estándares de Referencia (Respaldo Académico e Industrial)

El diseño de la base de datos `santillan_seguros` (normalizada en **Tercera Forma Normal - 3FN**) se fundamenta en tres fuentes públicas verificables del dominio asegurador:

1. **Marco Legal Argentino — Ley N° 22.400 (Régimen de Productores Asesores de Seguros - SSN):**
   * Fuente oficial: [InfoLEG - Ministerio de Justicia de la Nación](http://servicios.infoleg.gob.ar/infolegInternet/anexos/20000-24999/20968/texact.htm)
   * Regula la intermediación del Productor Asesor de Seguros (PAS) ante la **Superintendencia de Seguros de la Nación (SSN)** y las compañías aseguradoras (ej. **Federación Patronal Seguros S.A.**), exigiendo el registro trazable de asegurados, pólizas emitidas por ramo y cobranzas.
2. **Database Answers Canonical Insurance Model (*Insurance Policies - Car, Home and Life*):**
   * Modelo de referencia público para sistemas de seguros que establece la estructura jerárquica estándar: `Customer/Party` $\rightarrow$ `Policy` $\rightarrow$ `Policy_Installments/Billing` $\rightarrow$ `Payments`.
3. **Estándar Internacional ACORD (*Insurance Data Model*):**
   * Define la separación estricta entre la entidad **Póliza** (`policies`) y las entidades de **Facturación/Cuotas** (`installments`), evitando almacenar montos o estados de pago repetidos dentro de la tabla maestra de pólizas.

---

## 2. Justificación de Cada Relación del DER (3FN e Integridad Referencial)

| Relación | Cardinalidad | Fundamentación Técnica y de Negocio |
| :--- | :---: | :--- |
| **`insureds` $\rightarrow$ `policies`** | **1 : N** | Un asegurado (`tax_id` único) puede contratar múltiples pólizas vigentes o históricas (ej. auto + hogar + vida). Cada póliza pertenece a un único titular (`FK insured_id`, `ON DELETE RESTRICT` para preservar integridad histórica). |
| **`branches` $\rightarrow$ `policies`** | **1 : N** | Normalización 3FN según clasificación oficial de la SSN (Automotor, Combinado Familiar, Vida, RC, Caución). Evita redundancia textual en `policies` mediante `FK branch_id`. |
| **`policies` $\rightarrow$ `installments`** | **1 : N** | Según estándar ACORD Billing, una póliza se fracciona en `N` cuotas mensuales independientes (`installment_number`, `due_date`, `status`). Restricción `UNIQUE (policy_id, installment_number)` e índice compuesto `(due_date, status)` para consultas en $O(\log N)$. |
| **`installments` $\rightarrow$ `notifications`** | **1 : N** | Una cuota próxima a vencer o en mora dispara registros de auditoría (`UPCOMING`, `OVERDUE`) para registrar cada aviso enviado al asegurado. |
| **`users`** | **Independiente** | Entidad de control de acceso y auditoría de operadores (`ADMIN`, `PRODUCTOR`, `OPERADOR`), almacenando contraseñas con hash SHA-256 (`password_hash`). |

---

## 3. Diccionario de Datos (Tablas MariaDB)

* **`users`:** `user_id` (PK, INT AI), `username` (VARCHAR 50, UQ), `password_hash` (VARCHAR 64), `full_name` (VARCHAR 120), `role` (ENUM), `active` (BOOLEAN), `created_at` (DATETIME).
* **`branches`:** `branch_id` (PK, INT AI), `code` (VARCHAR 20, UQ), `name` (VARCHAR 80), `description` (VARCHAR 255).
* **`insureds`:** `insured_id` (PK, INT AI), `tax_id` (VARCHAR 20, UQ), `full_name` (VARCHAR 120), `phone` (VARCHAR 30), `email` (VARCHAR 120), `address` (VARCHAR 180), `active` (BOOLEAN).
* **`policies`:** `policy_id` (PK, INT AI), `policy_number` (INT, UQ), `insured_id` (FK), `branch_id` (FK), `company` (VARCHAR 80), `insured_asset` (VARCHAR 180), `total_premium` (DECIMAL 12,2), `installments_count` (INT), `start_date` (DATE), `end_date` (DATE), `status` (ENUM).
* **`installments`:** `installment_id` (PK, INT AI), `policy_id` (FK), `installment_number` (INT), `amount` (DECIMAL 12,2), `due_date` (DATE), `status` (ENUM: `PENDING`, `PAID`, `OVERDUE`), `payment_date` (DATETIME), `payment_method` (VARCHAR 50), `notes` (VARCHAR 255).
* **`notifications`:** `notification_id` (PK, INT AI), `installment_id` (FK), `alert_type` (ENUM: `UPCOMING`, `OVERDUE`), `message` (VARCHAR 255), `generated_at` (DATETIME), `sent` (BOOLEAN).

---

## 4. Especificación Estructurada de Procesos (DEP)

* **DEP-01 (Emisión de Póliza):** Validar DTO (`PolicyInputDto`) $\rightarrow$ Iniciar transacción SQL $\rightarrow$ Insertar en `policies` $\rightarrow$ Bucle `i = 1..installments_count` calculando `amount = total_premium / installments_count` y `due_date = first_due_date + (i-1) meses` $\rightarrow$ Insertar en `installments` $\rightarrow$ Commit.
* **DEP-02 (Motor de Alertas):** Sincronizar cuotas `PENDING` con `due_date < CURDATE()` a `OVERDUE` $\rightarrow$ Consultar `OVERDUE` y `PENDING` dentro de la ventana de días $\rightarrow$ Calcular KPIs en una pasada $O(N)$ e indexar en `Dictionary` $O(1)$.
* **DEP-03 (Imputación de Pago y Respaldo SQL):** Registrar pago (`UPDATE installments SET status='PAID', payment_date=NOW()`) o exportar DUMP SQL completo (`DatabaseBackupDao`) con `SET FOREIGN_KEY_CHECKS` desde `FormOpciones` protegido por contraseña.
