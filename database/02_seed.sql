-- ============================================================================
-- SEED DATA - Federacion Santillan Seguros (Relative dates to CURDATE())
-- ============================================================================

SET NAMES utf8mb4;
USE santillan_seguros;

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
