-- ============================================================================
-- SYSTEM: Federacion Santillan Seguros (Insurance Producer - Federacion Patronal)
-- ENGINE: MariaDB 11.4+ / MySQL 8+ (InnoDB, UTF8MB4)
-- REFERENCE STANDARDS:
--   * Database Answers Canonical Insurance Model (Policies & Billing)
--   * ACORD Insurance Data Standards (Party -> Policy -> Installment -> Payment)
--   * Argentine Law N° 22.400 (Insurance Producer Advisors - SSN)
-- ============================================================================

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
