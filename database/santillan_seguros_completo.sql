-- ============================================================================
-- SYSTEM: Federacion Santillan Seguros (Insurance Producer - Federacion Patronal)
-- ENGINE: MariaDB 11.4+ / MySQL 8+ (InnoDB, UTF8MB4)
-- REFERENCE STANDARDS:
--   * Database Answers Canonical Insurance Model (Policies & Billing)
--   * ACORD Insurance Data Standards (Party -> Policy -> Installment -> Payment)
--   * Argentine Law N° 22.400 (Insurance Producer Advisors - SSN)
-- ============================================================================

SET NAMES utf8mb4;

CREATE DATABASE IF NOT EXISTS santillan_seguros
  CHARACTER SET utf8mb4
  COLLATE utf8mb4_unicode_ci;

USE santillan_seguros;

-- 1. Users (Authentication & Access Control)
CREATE TABLE IF NOT EXISTS users (
    user_id INT AUTO_INCREMENT PRIMARY KEY,
    username VARCHAR(50) NOT NULL UNIQUE,
    password_hash VARCHAR(64) NOT NULL COMMENT 'SHA-256 hex digest',
    full_name VARCHAR(120) NOT NULL,
    role ENUM('ADMIN', 'PRODUCTOR', 'OPERADOR') NOT NULL DEFAULT 'PRODUCTOR',
    active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB;

-- 2. Insurance Branches (SSN / Federacion Patronal Catalog)
CREATE TABLE IF NOT EXISTS branches (
    branch_id INT AUTO_INCREMENT PRIMARY KEY,
    code VARCHAR(20) NOT NULL UNIQUE,
    name VARCHAR(80) NOT NULL,
    description VARCHAR(255) NULL
) ENGINE=InnoDB;

-- 3. Insureds / Clients (ACORD Party/Client Entity)
CREATE TABLE IF NOT EXISTS insureds (
    insured_id INT AUTO_INCREMENT PRIMARY KEY,
    tax_id VARCHAR(20) NOT NULL UNIQUE COMMENT 'DNI or CUIT',
    full_name VARCHAR(120) NOT NULL,
    phone VARCHAR(30) NOT NULL,
    email VARCHAR(120) NULL,
    address VARCHAR(180) NULL,
    active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    INDEX idx_insured_name (full_name)
) ENGINE=InnoDB;

-- 4. Issued Policies (ACORD Policy Entity - 3FN)
CREATE TABLE IF NOT EXISTS policies (
    policy_id INT AUTO_INCREMENT PRIMARY KEY,
    policy_number INT NOT NULL UNIQUE,
    insured_id INT NOT NULL,
    branch_id INT NOT NULL,
    company VARCHAR(80) NOT NULL DEFAULT 'Federación Patronal Seguros',
    insured_asset VARCHAR(180) NOT NULL,
    total_premium DECIMAL(12, 2) NOT NULL,
    installments_count INT NOT NULL DEFAULT 6,
    start_date DATE NOT NULL,
    end_date DATE NOT NULL,
    status ENUM('ACTIVE', 'SUSPENDED', 'CANCELLED', 'EXPIRED') NOT NULL DEFAULT 'ACTIVE',
    CONSTRAINT fk_policy_insured FOREIGN KEY (insured_id)
        REFERENCES insureds(insured_id) ON UPDATE CASCADE ON DELETE RESTRICT,
    CONSTRAINT fk_policy_branch FOREIGN KEY (branch_id)
        REFERENCES branches(branch_id) ON UPDATE CASCADE ON DELETE RESTRICT,
    INDEX idx_policy_status (status)
) ENGINE=InnoDB;

-- 5. Installments / Billing Schedule (ACORD Billing/Installment Entity)
CREATE TABLE IF NOT EXISTS installments (
    installment_id INT AUTO_INCREMENT PRIMARY KEY,
    policy_id INT NOT NULL,
    installment_number INT NOT NULL,
    amount DECIMAL(12, 2) NOT NULL,
    due_date DATE NOT NULL,
    status ENUM('PENDING', 'PAID', 'OVERDUE') NOT NULL DEFAULT 'PENDING',
    payment_date DATETIME NULL,
    payment_method VARCHAR(50) NULL,
    notes VARCHAR(255) NULL,
    CONSTRAINT fk_installment_policy FOREIGN KEY (policy_id)
        REFERENCES policies(policy_id) ON UPDATE CASCADE ON DELETE CASCADE,
    CONSTRAINT uq_policy_installment UNIQUE (policy_id, installment_number),
    INDEX idx_installment_due_status (due_date, status)
) ENGINE=InnoDB;

-- 6. Collection Notifications Log (Audit trail of client reminders)
CREATE TABLE IF NOT EXISTS notifications (
    notification_id INT AUTO_INCREMENT PRIMARY KEY,
    installment_id INT NOT NULL,
    alert_type ENUM('UPCOMING', 'OVERDUE') NOT NULL,
    message VARCHAR(255) NOT NULL,
    generated_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    sent BOOLEAN NOT NULL DEFAULT TRUE,
    CONSTRAINT fk_notification_installment FOREIGN KEY (installment_id)
        REFERENCES installments(installment_id) ON UPDATE CASCADE ON DELETE CASCADE,
    INDEX idx_notification_date (generated_at)
) ENGINE=InnoDB;

-- ============================================================================
-- SEED DATA - Federacion Santillan Seguros (Relative dates to CURDATE())
-- ============================================================================

INSERT IGNORE INTO users (username, password_hash, full_name, role, active) VALUES
('admin', '240be518fabd2724ddb6f04eeb1da5967448d7e831c08c8fa822809f74c720a9', 'Administrador General', 'ADMIN', TRUE),
('santillan', '7cbc7eb7a95fba33419d4abd2357bf17bffdf11e4922000374796bac341faeb7', 'Federación Santillán (PAS)', 'PRODUCTOR', TRUE);

INSERT IGNORE INTO branches (branch_id, code, name, description) VALUES
(1, 'AUTO', 'Automotor', 'Responsabilidad Civil, Terceros Completo y Todo Riesgo'),
(2, 'HOME', 'Combinado Familiar', 'Incendio, robo contenido y cristales para vivienda'),
(3, 'LIFE', 'Vida Colectivo / Individual', 'Seguro de vida obligatorio y cobertura integral'),
(4, 'RC', 'Responsabilidad Civil', 'Cobertura de daños a terceros para comercios y obras'),
(5, 'SURETY', 'Caución', 'Garantías contractuales y de alquiler');

INSERT IGNORE INTO insureds (insured_id, tax_id, full_name, phone, email, address) VALUES
(1, '20-34567890-1', 'Gómez, Carlos Alberto', '291-4123456', 'cgomez@email.com', 'Av. Alem 1240, Bahía Blanca'),
(2, '27-29876543-4', 'Fernández, Laura Beatriz', '291-5234567', 'laura.f@email.com', 'Sarmiento 450, Bahía Blanca'),
(3, '20-18765432-9', 'Rodríguez, Martín Ezequiel', '291-6345678', 'mrodriguez@email.com', 'Alsina 820, Bahía Blanca'),
(4, '27-38112233-0', 'Pérez, Sofía Valeria', '291-4987654', 'sofia.perez@email.com', '12 de Octubre 315, Bahía Blanca'),
(5, '20-25667788-5', 'Martínez, Juan Ignacio', '291-5778899', 'jmartinez@email.com', 'Zelarrayán 1510, Bahía Blanca'),
(6, '30-71223344-8', 'Constructora del Sur SRL', '291-4550011', 'admin@constructorasur.com.ar', 'Parque Industrial, Bahía Blanca');

INSERT IGNORE INTO policies (policy_id, policy_number, insured_id, branch_id, company, insured_asset, total_premium, installments_count, start_date, end_date, status) VALUES
(1, 900101, 1, 1, 'Federación Patronal Seguros', 'Toyota Hilux 2022 - Patente AF452KL', 360000.00, 6, DATE_SUB(CURDATE(), INTERVAL 60 DAY), DATE_ADD(CURDATE(), INTERVAL 120 DAY), 'ACTIVE'),
(2, 900102, 2, 1, 'Federación Patronal Seguros', 'Volkswagen Golf 2020 - Patente AE118MN', 240000.00, 6, DATE_SUB(CURDATE(), INTERVAL 45 DAY), DATE_ADD(CURDATE(), INTERVAL 135 DAY), 'ACTIVE'),
(3, 900103, 3, 2, 'Federación Patronal Seguros', 'Vivienda Alsina 820 - Suma $45M', 120000.00, 6, DATE_SUB(CURDATE(), INTERVAL 35 DAY), DATE_ADD(CURDATE(), INTERVAL 145 DAY), 'ACTIVE'),
(4, 900104, 4, 1, 'Federación Patronal Seguros', 'Fiat Cronos 2023 - Patente AG221PR', 210000.00, 6, DATE_SUB(CURDATE(), INTERVAL 20 DAY), DATE_ADD(CURDATE(), INTERVAL 160 DAY), 'ACTIVE'),
(5, 900105, 5, 3, 'Federación Patronal Seguros', 'Vida Individual Plan Integral', 90000.00, 6, DATE_SUB(CURDATE(), INTERVAL 25 DAY), DATE_ADD(CURDATE(), INTERVAL 155 DAY), 'ACTIVE'),
(6, 900106, 6, 4, 'Federación Patronal Seguros', 'RC Comprensiva Obra Civil', 480000.00, 6, DATE_SUB(CURDATE(), INTERVAL 50 DAY), DATE_ADD(CURDATE(), INTERVAL 130 DAY), 'ACTIVE');

INSERT IGNORE INTO installments (policy_id, installment_number, amount, due_date, status, payment_date, payment_method, notes) VALUES
(1, 1, 60000.00, DATE_SUB(CURDATE(), INTERVAL 42 DAY), 'PAID', DATE_SUB(CURDATE(), INTERVAL 43 DAY), 'Transferencia', 'Abonada en término'),
(1, 2, 60000.00, DATE_SUB(CURDATE(), INTERVAL 12 DAY), 'OVERDUE', NULL, NULL, 'Requiere aviso urgente de mora'),
(1, 3, 60000.00, DATE_ADD(CURDATE(), INTERVAL 18 DAY), 'PENDING', NULL, NULL, NULL),
(2, 1, 40000.00, DATE_SUB(CURDATE(), INTERVAL 27 DAY), 'PAID', DATE_SUB(CURDATE(), INTERVAL 28 DAY), 'Débito Automático', NULL),
(2, 2, 40000.00, DATE_ADD(CURDATE(), INTERVAL 3 DAY), 'PENDING', NULL, NULL, 'Próxima a vencer'),
(3, 1, 20000.00, DATE_SUB(CURDATE(), INTERVAL 5 DAY), 'OVERDUE', NULL, NULL, 'Primer vencimiento impago'),
(3, 2, 20000.00, DATE_ADD(CURDATE(), INTERVAL 25 DAY), 'PENDING', NULL, NULL, NULL),
(4, 1, 35000.00, DATE_ADD(CURDATE(), INTERVAL 7 DAY), 'PENDING', NULL, NULL, 'Enviar recordatorio WhatsApp'),
(5, 1, 15000.00, DATE_SUB(CURDATE(), INTERVAL 18 DAY), 'PAID', DATE_SUB(CURDATE(), INTERVAL 19 DAY), 'Efectivo', NULL),
(5, 2, 15000.00, DATE_ADD(CURDATE(), INTERVAL 12 DAY), 'PENDING', NULL, NULL, NULL),
(6, 1, 80000.00, DATE_SUB(CURDATE(), INTERVAL 50 DAY), 'PAID', DATE_SUB(CURDATE(), INTERVAL 50 DAY), 'Transferencia', NULL),
(6, 2, 80000.00, DATE_SUB(CURDATE(), INTERVAL 20 DAY), 'OVERDUE', NULL, NULL, 'Riesgo de suspensión de cobertura');
