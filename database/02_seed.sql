-- ============================================================================
-- DATOS DE PRUEBA (SEED) - Federación Santillán Seguros
-- Fechas relativas a CURDATE() para que las alertas de mora y vencimiento
-- funcionen en vivo sin importar el día que se importe en DBeaver.
-- ============================================================================

USE santillan_seguros;

-- Usuarios iniciales (Contraseña 'admin123' y 'seguros2026' en SHA-256)
INSERT IGNORE INTO usuarios (username, password_hash, nombre_completo, rol, activo) VALUES
('admin', '240be518fabd2724ddb6f04eeb1da5967448d7e831c08c8fa822809f74c720a9', 'Administrador General', 'ADMIN', TRUE),
('santillan', '7cbc7eb7a95fba33419d4abd2357bf17bffdf11e4922000374796bac341faeb7', 'Federación Santillán (PAS)', 'PRODUCTOR', TRUE);

-- Ramos de Seguros (Clasificación SSN / Federación Patronal)
INSERT IGNORE INTO ramos (id_ramo, codigo, nombre, descripcion) VALUES
(1, 'AUTO', 'Automotor', 'Cobertura de Responsabilidad Civil, Terceros Completo y Todo Riesgo'),
(2, 'HOGAR', 'Combinado Familiar', 'Incendio, robo contenido y cristales para vivienda particular'),
(3, 'VIDA', 'Vida Colectivo / Individual', 'Seguro de vida obligatorio y cobertura por fallecimiento e invalidez'),
(4, 'RC', 'Responsabilidad Civil', 'Cobertura de daños a terceros para comercios y profesionales'),
(5, 'CAUCION', 'Caución', 'Garantías contractuales, alquileres y obra pública');

-- Asegurados (Cartera Bahía Blanca y zona)
INSERT IGNORE INTO asegurados (id_asegurado, dni_cuit, apellido_nombre, telefono, email, domicilio) VALUES
(1, '20-34567890-1', 'Gómez, Carlos Alberto', '291-4123456', 'cgomez@email.com', 'Av. Alem 1240, Bahía Blanca'),
(2, '27-29876543-4', 'Fernández, Laura Beatriz', '291-5234567', 'laura.f@email.com', 'Sarmiento 450, Bahía Blanca'),
(3, '20-18765432-9', 'Rodríguez, Martín Ezequiel', '291-6345678', 'mrodriguez@email.com', 'Alsina 820, Bahía Blanca'),
(4, '27-38112233-0', 'Pérez, Sofía Valeria', '291-4987654', 'sofia.perez@email.com', '12 de Octubre 315, Bahía Blanca'),
(5, '20-25667788-5', 'Martínez, Juan Ignacio', '291-5778899', 'jmartinez@email.com', 'Zelarrayán 1510, Bahía Blanca'),
(6, '30-71223344-8', 'Constructora del Sur SRL', '291-4550011', 'admin@constructorasur.com.ar', 'Parque Industrial, Bahía Blanca');

-- Pólizas emitidas con Federación Patronal Seguros
INSERT IGNORE INTO polizas (id_poliza, numero_poliza, id_asegurado, id_ramo, compania, bien_asegurado, prima_total, cantidad_cuotas, fecha_inicio, fecha_fin, estado) VALUES
(1, 900101, 1, 1, 'Federación Patronal Seguros', 'Toyota Hilux 2022 - Patente AF452KL', 360000.00, 6, DATE_SUB(CURDATE(), INTERVAL 60 DAY), DATE_ADD(CURDATE(), INTERVAL 120 DAY), 'ACTIVA'),
(2, 900102, 2, 1, 'Federación Patronal Seguros', 'Volkswagen Golf 2020 - Patente AE118MN', 240000.00, 6, DATE_SUB(CURDATE(), INTERVAL 45 DAY), DATE_ADD(CURDATE(), INTERVAL 135 DAY), 'ACTIVA'),
(3, 900103, 3, 2, 'Federación Patronal Seguros', 'Vivienda Alsina 820 - Suma $45M', 120000.00, 6, DATE_SUB(CURDATE(), INTERVAL 35 DAY), DATE_ADD(CURDATE(), INTERVAL 145 DAY), 'ACTIVA'),
(4, 900104, 4, 1, 'Federación Patronal Seguros', 'Fiat Cronos 2023 - Patente AG221PR', 210000.00, 6, DATE_SUB(CURDATE(), INTERVAL 20 DAY), DATE_ADD(CURDATE(), INTERVAL 160 DAY), 'ACTIVA'),
(5, 900105, 5, 3, 'Federación Patronal Seguros', 'Vida Individual Plan Integral', 90000.00, 6, DATE_SUB(CURDATE(), INTERVAL 25 DAY), DATE_ADD(CURDATE(), INTERVAL 155 DAY), 'ACTIVA'),
(6, 900106, 6, 4, 'Federación Patronal Seguros', 'RC Comprensiva Obra Civil', 480000.00, 6, DATE_SUB(CURDATE(), INTERVAL 50 DAY), DATE_ADD(CURDATE(), INTERVAL 130 DAY), 'ACTIVA');

-- Cuotas estratégicas: PAGADAS, VENCIDAS (Morosos en rojo) y POR VENCER (Próximos 15 días en amarillo)
INSERT IGNORE INTO cuotas (id_poliza, numero_cuota, monto, fecha_vencimiento, estado, fecha_pago, metodo_pago, observaciones) VALUES
-- Póliza 1 (Gómez): Cuota 1 pagada, Cuota 2 VENCIDA hace 12 días, Cuota 3 por vencer en 18 días
(1, 1, 60000.00, DATE_SUB(CURDATE(), INTERVAL 42 DAY), 'PAGADA', DATE_SUB(CURDATE(), INTERVAL 43 DAY), 'Transferencia', 'Abonada en término'),
(1, 2, 60000.00, DATE_SUB(CURDATE(), INTERVAL 12 DAY), 'VENCIDA', NULL, NULL, 'Requiere aviso urgente de mora'),
(1, 3, 60000.00, DATE_ADD(CURDATE(), INTERVAL 18 DAY), 'PENDIENTE', NULL, NULL, NULL),

-- Póliza 2 (Fernández): Cuota 1 pagada, Cuota 2 POR VENCER en 3 días
(2, 1, 40000.00, DATE_SUB(CURDATE(), INTERVAL 27 DAY), 'PAGADA', DATE_SUB(CURDATE(), INTERVAL 28 DAY), 'Débito Automático', NULL),
(2, 2, 40000.00, DATE_ADD(CURDATE(), INTERVAL 3 DAY), 'PENDIENTE', NULL, NULL, 'Próxima a vencer'),

-- Póliza 3 (Rodríguez): Cuota 1 VENCIDA hace 5 días, Cuota 2 por vencer en 25 días
(3, 1, 20000.00, DATE_SUB(CURDATE(), INTERVAL 5 DAY), 'VENCIDA', NULL, NULL, 'Primer vencimiento impago'),
(3, 2, 20000.00, DATE_ADD(CURDATE(), INTERVAL 25 DAY), 'PENDIENTE', NULL, NULL, NULL),

-- Póliza 4 (Pérez): Cuota 1 POR VENCER en 7 días
(4, 1, 35000.00, DATE_ADD(CURDATE(), INTERVAL 7 DAY), 'PENDIENTE', NULL, NULL, 'Enviar recordatorio WhatsApp'),

-- Póliza 5 (Martínez): Cuota 1 pagada, Cuota 2 POR VENCER en 12 días
(5, 1, 15000.00, DATE_SUB(CURDATE(), INTERVAL 18 DAY), 'PAGADA', DATE_SUB(CURDATE(), INTERVAL 19 DAY), 'Efectivo', NULL),
(5, 2, 15000.00, DATE_ADD(CURDATE(), INTERVAL 12 DAY), 'PENDIENTE', NULL, NULL, NULL),

-- Póliza 6 (Constructora del Sur): Cuota 1 pagada, Cuota 2 VENCIDA hace 20 días (mora crítica)
(6, 1, 80000.00, DATE_SUB(CURDATE(), INTERVAL 50 DAY), 'PAGADA', DATE_SUB(CURDATE(), INTERVAL 50 DAY), 'Transferencia', NULL),
(6, 2, 80000.00, DATE_SUB(CURDATE(), INTERVAL 20 DAY), 'VENCIDA', NULL, NULL, 'Riesgo de suspensión de cobertura');
