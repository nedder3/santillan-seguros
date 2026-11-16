-- ============================================================================
-- SISTEMA: FederaciÃ³n SantillÃ¡n Seguros (Productor Asesor - FederaciÃ³n Patronal)
-- MOTOR: MariaDB 11.4+ / MySQL 8+ (InnoDB, UTF8MB4)
-- ESTÃNDAR DE REFERENCIA:
--   * Database Answers Canonical Model (Insurance Policies & Billing)
--   * ACORD Insurance Data Standards (Party -> Policy -> Installment -> Payment)
--   * Ley Nacional NÂ° 22.400 (Productores Asesores de Seguros - SSN Argentina)
-- ============================================================================

CREATE DATABASE IF NOT EXISTS santillan_seguros
  CHARACTER SET utf8mb4
  COLLATE utf8mb4_unicode_ci;

USE santillan_seguros;

-- 1. Tabla de Usuarios del Sistema (AutenticaciÃ³n y Roles - RÃºbrica Obligatoria)
CREATE TABLE IF NOT EXISTS usuarios (
    id_usuario INT AUTO_INCREMENT PRIMARY KEY,
    username VARCHAR(50) NOT NULL UNIQUE,
    password_hash VARCHAR(64) NOT NULL COMMENT 'SHA-256 hex digest',
    nombre_completo VARCHAR(120) NOT NULL,
    rol ENUM('ADMIN', 'PRODUCTOR', 'OPERADOR') NOT NULL DEFAULT 'PRODUCTOR',
    activo BOOLEAN NOT NULL DEFAULT TRUE,
    fecha_creacion DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB;

-- 2. Tabla ParamÃ©trica de Ramos de Seguros (SegÃºn clasificaciÃ³n SSN / FederaciÃ³n Patronal)
CREATE TABLE IF NOT EXISTS ramos (
    id_ramo INT AUTO_INCREMENT PRIMARY KEY,
    codigo VARCHAR(20) NOT NULL UNIQUE,
    nombre VARCHAR(80) NOT NULL,
    descripcion VARCHAR(255) NULL
) ENGINE=InnoDB;

-- 3. Tabla de Asegurados / Clientes (Entidad Party/Client del estÃ¡ndar ACORD)
CREATE TABLE IF NOT EXISTS asegurados (
    id_asegurado INT AUTO_INCREMENT PRIMARY KEY,
    dni_cuit VARCHAR(20) NOT NULL UNIQUE,
    apellido_nombre VARCHAR(120) NOT NULL,
    telefono VARCHAR(30) NOT NULL,
    email VARCHAR(120) NULL,
    domicilio VARCHAR(180) NULL,
    activo BOOLEAN NOT NULL DEFAULT TRUE,
    fecha_alta DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    INDEX idx_asegurado_nombre (apellido_nombre)
) ENGINE=InnoDB;

-- 4. Tabla de PÃ³lizas Emitidas (Entidad Policy - 1 Asegurado posee 1..N PÃ³lizas; 1 Ramo clasifica 1..N PÃ³lizas)
CREATE TABLE IF NOT EXISTS polizas (
    id_poliza INT AUTO_INCREMENT PRIMARY KEY,
    numero_poliza INT NOT NULL UNIQUE,
    id_asegurado INT NOT NULL,
    id_ramo INT NOT NULL,
    compania VARCHAR(80) NOT NULL DEFAULT 'FederaciÃ³n Patronal Seguros',
    bien_asegurado VARCHAR(180) NOT NULL COMMENT 'Ej: Dominio vehÃ­culo, direcciÃ³n inmueble, etc.',
    prima_total DECIMAL(12, 2) NOT NULL,
    cantidad_cuotas INT NOT NULL DEFAULT 6,
    fecha_inicio DATE NOT NULL,
    fecha_fin DATE NOT NULL,
    estado ENUM('ACTIVA', 'SUSPENDIDA', 'ANULADA', 'VENCIDA') NOT NULL DEFAULT 'ACTIVA',
    CONSTRAINT fk_poliza_asegurado FOREIGN KEY (id_asegurado)
        REFERENCES asegurados(id_asegurado) ON UPDATE CASCADE ON DELETE RESTRICT,
    CONSTRAINT fk_poliza_ramo FOREIGN KEY (id_ramo)
        REFERENCES ramos(id_ramo) ON UPDATE CASCADE ON DELETE RESTRICT,
    INDEX idx_poliza_estado (estado)
) ENGINE=InnoDB;

-- 5. Tabla de Cuotas / Plan de Cobranzas (Entidad Billing/Installment - 1 PÃ³liza genera 1..N Cuotas)
CREATE TABLE IF NOT EXISTS cuotas (
    id_cuota INT AUTO_INCREMENT PRIMARY KEY,
    id_poliza INT NOT NULL,
    numero_cuota INT NOT NULL,
    monto DECIMAL(12, 2) NOT NULL,
    fecha_vencimiento DATE NOT NULL,
    estado ENUM('PENDIENTE', 'PAGADA', 'VENCIDA') NOT NULL DEFAULT 'PENDIENTE',
    fecha_pago DATETIME NULL,
    metodo_pago VARCHAR(50) NULL COMMENT 'Efectivo, Transferencia, DÃ©bito AutomÃ¡tico, MercadoPago',
    observaciones VARCHAR(255) NULL,
    CONSTRAINT fk_cuota_poliza FOREIGN KEY (id_poliza)
        REFERENCES polizas(id_poliza) ON UPDATE CASCADE ON DELETE CASCADE,
    CONSTRAINT uq_poliza_cuota UNIQUE (id_poliza, numero_cuota),
    INDEX idx_cuota_vencimiento (fecha_vencimiento, estado)
) ENGINE=InnoDB;

-- 6. Tabla de Registro de Notificaciones de Cobranza (AuditorÃ­a de avisos al asegurado)
CREATE TABLE IF NOT EXISTS notificaciones (
    id_notificacion INT AUTO_INCREMENT PRIMARY KEY,
    id_cuota INT NOT NULL,
    tipo_alerta ENUM('POR_VENCER', 'VENCIDA') NOT NULL,
    mensaje VARCHAR(255) NOT NULL,
    fecha_generacion DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    leida BOOLEAN NOT NULL DEFAULT FALSE,
    CONSTRAINT fk_notificacion_cuota FOREIGN KEY (id_cuota)
        REFERENCES cuotas(id_cuota) ON UPDATE CASCADE ON DELETE CASCADE
) ENGINE=InnoDB;
-- ============================================================================
-- DATOS DE PRUEBA (SEED) - FederaciÃ³n SantillÃ¡n Seguros
-- Fechas relativas a CURDATE() para que las alertas de mora y vencimiento
-- funcionen en vivo sin importar el dÃ­a que se importe en DBeaver.
-- ============================================================================

USE santillan_seguros;

-- Usuarios iniciales (ContraseÃ±a 'admin123' y 'seguros2026' en SHA-256)
INSERT IGNORE INTO usuarios (username, password_hash, nombre_completo, rol, activo) VALUES
('admin', '240be518fabd2724ddb6f04eeb1da5967448d7e831c08c8fa822809f74c720a9', 'Administrador General', 'ADMIN', TRUE),
('santillan', '7cbc7eb7a95fba33419d4abd2357bf17bffdf11e4922000374796bac341faeb7', 'FederaciÃ³n SantillÃ¡n (PAS)', 'PRODUCTOR', TRUE);

-- Ramos de Seguros (ClasificaciÃ³n SSN / FederaciÃ³n Patronal)
INSERT IGNORE INTO ramos (id_ramo, codigo, nombre, descripcion) VALUES
(1, 'AUTO', 'Automotor', 'Cobertura de Responsabilidad Civil, Terceros Completo y Todo Riesgo'),
(2, 'HOGAR', 'Combinado Familiar', 'Incendio, robo contenido y cristales para vivienda particular'),
(3, 'VIDA', 'Vida Colectivo / Individual', 'Seguro de vida obligatorio y cobertura por fallecimiento e invalidez'),
(4, 'RC', 'Responsabilidad Civil', 'Cobertura de daÃ±os a terceros para comercios y profesionales'),
(5, 'CAUCION', 'CauciÃ³n', 'GarantÃ­as contractuales, alquileres y obra pÃºblica');

-- Asegurados (Cartera BahÃ­a Blanca y zona)
INSERT IGNORE INTO asegurados (id_asegurado, dni_cuit, apellido_nombre, telefono, email, domicilio) VALUES
(1, '20-34567890-1', 'GÃ³mez, Carlos Alberto', '291-4123456', 'cgomez@email.com', 'Av. Alem 1240, BahÃ­a Blanca'),
(2, '27-29876543-4', 'FernÃ¡ndez, Laura Beatriz', '291-5234567', 'laura.f@email.com', 'Sarmiento 450, BahÃ­a Blanca'),
(3, '20-18765432-9', 'RodrÃ­guez, MartÃ­n Ezequiel', '291-6345678', 'mrodriguez@email.com', 'Alsina 820, BahÃ­a Blanca'),
(4, '27-38112233-0', 'PÃ©rez, SofÃ­a Valeria', '291-4987654', 'sofia.perez@email.com', '12 de Octubre 315, BahÃ­a Blanca'),
(5, '20-25667788-5', 'MartÃ­nez, Juan Ignacio', '291-5778899', 'jmartinez@email.com', 'ZelarrayÃ¡n 1510, BahÃ­a Blanca'),
(6, '30-71223344-8', 'Constructora del Sur SRL', '291-4550011', 'admin@constructorasur.com.ar', 'Parque Industrial, BahÃ­a Blanca');

-- PÃ³lizas emitidas con FederaciÃ³n Patronal Seguros
INSERT IGNORE INTO polizas (id_poliza, numero_poliza, id_asegurado, id_ramo, compania, bien_asegurado, prima_total, cantidad_cuotas, fecha_inicio, fecha_fin, estado) VALUES
(1, 900101, 1, 1, 'FederaciÃ³n Patronal Seguros', 'Toyota Hilux 2022 - Patente AF452KL', 360000.00, 6, DATE_SUB(CURDATE(), INTERVAL 60 DAY), DATE_ADD(CURDATE(), INTERVAL 120 DAY), 'ACTIVA'),
(2, 900102, 2, 1, 'FederaciÃ³n Patronal Seguros', 'Volkswagen Golf 2020 - Patente AE118MN', 240000.00, 6, DATE_SUB(CURDATE(), INTERVAL 45 DAY), DATE_ADD(CURDATE(), INTERVAL 135 DAY), 'ACTIVA'),
(3, 900103, 3, 2, 'FederaciÃ³n Patronal Seguros', 'Vivienda Alsina 820 - Suma $45M', 120000.00, 6, DATE_SUB(CURDATE(), INTERVAL 35 DAY), DATE_ADD(CURDATE(), INTERVAL 145 DAY), 'ACTIVA'),
(4, 900104, 4, 1, 'FederaciÃ³n Patronal Seguros', 'Fiat Cronos 2023 - Patente AG221PR', 210000.00, 6, DATE_SUB(CURDATE(), INTERVAL 20 DAY), DATE_ADD(CURDATE(), INTERVAL 160 DAY), 'ACTIVA'),
(5, 900105, 5, 3, 'FederaciÃ³n Patronal Seguros', 'Vida Individual Plan Integral', 90000.00, 6, DATE_SUB(CURDATE(), INTERVAL 25 DAY), DATE_ADD(CURDATE(), INTERVAL 155 DAY), 'ACTIVA'),
(6, 900106, 6, 4, 'FederaciÃ³n Patronal Seguros', 'RC Comprensiva Obra Civil', 480000.00, 6, DATE_SUB(CURDATE(), INTERVAL 50 DAY), DATE_ADD(CURDATE(), INTERVAL 130 DAY), 'ACTIVA');

-- Cuotas estratÃ©gicas: PAGADAS, VENCIDAS (Morosos en rojo) y POR VENCER (PrÃ³ximos 15 dÃ­as en amarillo)
INSERT IGNORE INTO cuotas (id_poliza, numero_cuota, monto, fecha_vencimiento, estado, fecha_pago, metodo_pago, observaciones) VALUES
-- PÃ³liza 1 (GÃ³mez): Cuota 1 pagada, Cuota 2 VENCIDA hace 12 dÃ­as, Cuota 3 por vencer en 18 dÃ­as
(1, 1, 60000.00, DATE_SUB(CURDATE(), INTERVAL 42 DAY), 'PAGADA', DATE_SUB(CURDATE(), INTERVAL 43 DAY), 'Transferencia', 'Abonada en tÃ©rmino'),
(1, 2, 60000.00, DATE_SUB(CURDATE(), INTERVAL 12 DAY), 'VENCIDA', NULL, NULL, 'Requiere aviso urgente de mora'),
(1, 3, 60000.00, DATE_ADD(CURDATE(), INTERVAL 18 DAY), 'PENDIENTE', NULL, NULL, NULL),

-- PÃ³liza 2 (FernÃ¡ndez): Cuota 1 pagada, Cuota 2 POR VENCER en 3 dÃ­as
(2, 1, 40000.00, DATE_SUB(CURDATE(), INTERVAL 27 DAY), 'PAGADA', DATE_SUB(CURDATE(), INTERVAL 28 DAY), 'DÃ©bito AutomÃ¡tico', NULL),
(2, 2, 40000.00, DATE_ADD(CURDATE(), INTERVAL 3 DAY), 'PENDIENTE', NULL, NULL, 'PrÃ³xima a vencer'),

-- PÃ³liza 3 (RodrÃ­guez): Cuota 1 VENCIDA hace 5 dÃ­as, Cuota 2 por vencer en 25 dÃ­as
(3, 1, 20000.00, DATE_SUB(CURDATE(), INTERVAL 5 DAY), 'VENCIDA', NULL, NULL, 'Primer vencimiento impago'),
(3, 2, 20000.00, DATE_ADD(CURDATE(), INTERVAL 25 DAY), 'PENDIENTE', NULL, NULL, NULL),

-- PÃ³liza 4 (PÃ©rez): Cuota 1 POR VENCER en 7 dÃ­as
(4, 1, 35000.00, DATE_ADD(CURDATE(), INTERVAL 7 DAY), 'PENDIENTE', NULL, NULL, 'Enviar recordatorio WhatsApp'),

-- PÃ³liza 5 (MartÃ­nez): Cuota 1 pagada, Cuota 2 POR VENCER en 12 dÃ­as
(5, 1, 15000.00, DATE_SUB(CURDATE(), INTERVAL 18 DAY), 'PAGADA', DATE_SUB(CURDATE(), INTERVAL 19 DAY), 'Efectivo', NULL),
(5, 2, 15000.00, DATE_ADD(CURDATE(), INTERVAL 12 DAY), 'PENDIENTE', NULL, NULL, NULL),

-- PÃ³liza 6 (Constructora del Sur): Cuota 1 pagada, Cuota 2 VENCIDA hace 20 dÃ­as (mora crÃ­tica)
(6, 1, 80000.00, DATE_SUB(CURDATE(), INTERVAL 50 DAY), 'PAGADA', DATE_SUB(CURDATE(), INTERVAL 50 DAY), 'Transferencia', NULL),
(6, 2, 80000.00, DATE_SUB(CURDATE(), INTERVAL 20 DAY), 'VENCIDA', NULL, NULL, 'Riesgo de suspensiÃ³n de cobertura');
