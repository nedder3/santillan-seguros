-- ============================================================================
-- SISTEMA: Federación Santillán Seguros (Productor Asesor - Federación Patronal)
-- MOTOR: MariaDB 11.4+ / MySQL 8+ (InnoDB, UTF8MB4)
-- ESTÁNDAR DE REFERENCIA:
--   * Database Answers Canonical Model (Insurance Policies & Billing)
--   * ACORD Insurance Data Standards (Party -> Policy -> Installment -> Payment)
--   * Ley Nacional N° 22.400 (Productores Asesores de Seguros - SSN Argentina)
-- ============================================================================

CREATE DATABASE IF NOT EXISTS santillan_seguros
  CHARACTER SET utf8mb4
  COLLATE utf8mb4_unicode_ci;

USE santillan_seguros;

-- 1. Tabla de Usuarios del Sistema (Autenticación y Roles - Rúbrica Obligatoria)
CREATE TABLE IF NOT EXISTS usuarios (
    id_usuario INT AUTO_INCREMENT PRIMARY KEY,
    username VARCHAR(50) NOT NULL UNIQUE,
    password_hash VARCHAR(64) NOT NULL COMMENT 'SHA-256 hex digest',
    nombre_completo VARCHAR(120) NOT NULL,
    rol ENUM('ADMIN', 'PRODUCTOR', 'OPERADOR') NOT NULL DEFAULT 'PRODUCTOR',
    activo BOOLEAN NOT NULL DEFAULT TRUE,
    fecha_creacion DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB;

-- 2. Tabla Paramétrica de Ramos de Seguros (Según clasificación SSN / Federación Patronal)
CREATE TABLE IF NOT EXISTS ramos (
    id_ramo INT AUTO_INCREMENT PRIMARY KEY,
    codigo VARCHAR(20) NOT NULL UNIQUE,
    nombre VARCHAR(80) NOT NULL,
    descripcion VARCHAR(255) NULL
) ENGINE=InnoDB;

-- 3. Tabla de Asegurados / Clientes (Entidad Party/Client del estándar ACORD)
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

-- 4. Tabla de Pólizas Emitidas (Entidad Policy - 1 Asegurado posee 1..N Pólizas; 1 Ramo clasifica 1..N Pólizas)
CREATE TABLE IF NOT EXISTS polizas (
    id_poliza INT AUTO_INCREMENT PRIMARY KEY,
    numero_poliza INT NOT NULL UNIQUE,
    id_asegurado INT NOT NULL,
    id_ramo INT NOT NULL,
    compania VARCHAR(80) NOT NULL DEFAULT 'Federación Patronal Seguros',
    bien_asegurado VARCHAR(180) NOT NULL COMMENT 'Ej: Dominio vehículo, dirección inmueble, etc.',
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

-- 5. Tabla de Cuotas / Plan de Cobranzas (Entidad Billing/Installment - 1 Póliza genera 1..N Cuotas)
CREATE TABLE IF NOT EXISTS cuotas (
    id_cuota INT AUTO_INCREMENT PRIMARY KEY,
    id_poliza INT NOT NULL,
    numero_cuota INT NOT NULL,
    monto DECIMAL(12, 2) NOT NULL,
    fecha_vencimiento DATE NOT NULL,
    estado ENUM('PENDIENTE', 'PAGADA', 'VENCIDA') NOT NULL DEFAULT 'PENDIENTE',
    fecha_pago DATETIME NULL,
    metodo_pago VARCHAR(50) NULL COMMENT 'Efectivo, Transferencia, Débito Automático, MercadoPago',
    observaciones VARCHAR(255) NULL,
    CONSTRAINT fk_cuota_poliza FOREIGN KEY (id_poliza)
        REFERENCES polizas(id_poliza) ON UPDATE CASCADE ON DELETE CASCADE,
    CONSTRAINT uq_poliza_cuota UNIQUE (id_poliza, numero_cuota),
    INDEX idx_cuota_vencimiento (fecha_vencimiento, estado)
) ENGINE=InnoDB;

-- 6. Tabla de Registro de Notificaciones de Cobranza (Auditoría de avisos al asegurado)
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
