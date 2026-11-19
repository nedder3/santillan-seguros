<div align="center">

# Federación Santillán Seguros
### Sistema de Gestión de Cartera, Cobranzas y Alertas de Vencimientos

![C# 12 .NET 8](https://img.shields.io/badge/C%23%2012-.NET%208.0%20WinForms-512BD4?style=for-the-badge&logo=dotnet)
![MariaDB 11.4](https://img.shields.io/badge/MariaDB-11.4%20(3FN%20InnoDB)-003545?style=for-the-badge&logo=mariadb)
![Architecture](https://img.shields.io/badge/Architecture-3--Layer%20MVC%20%2B%20SOLID-0F4C81?style=for-the-badge)
![Docker & noVNC](https://img.shields.io/badge/Docker-Wine%20%2B%20noVNC%20(Mac%2FWeb)-2496ED?style=for-the-badge&logo=docker)
![Tests](https://img.shields.io/badge/Tests-29%2F29%20Passing%20(FIRST%20%2B%20Gherkin)-15803D?style=for-the-badge)
![Big-O Guarantee](https://img.shields.io/badge/Complexity-O(1)%20Lookup%20%7C%20O(log%20N)%20B--Tree-F59E0B?style=for-the-badge)
![Conventional Commits](https://img.shields.io/badge/Conventional%20Commits-1.0.0-FE5196?style=for-the-badge)

**Proyecto Final 2026 — I.S.F.T. N° 191 (Bahía Blanca) — TS en Desarrollo de Software**  
*Desarrollado para Productor Asesor de Seguros (PAS) operando con **Federación Patronal Seguros S.A.***

</div>

---

## 1. Resumen Ejecutivo y Capacidades

**Federación Santillán Seguros** es un sistema de escritorio desarrollado en **C# 12 (.NET 8 Windows Forms)** compatible con **Visual Studio Form Designer (`*.Designer.cs`)** y respaldado por **MariaDB 11.4** en **Tercera Forma Normal (3FN)**. Resuelve la problemática operativa real de una oficina de seguros que gestionaba su cartera en planillas de cálculo dispersas, eliminando la duplicación de datos de contacto y automatizando la detección temprana de morosidad y vencimientos.

### Capacidades Técnicas Destacadas
* **Arquitectura MVC en 3 Capas Estrictas + SOLID:**
  * `FederacionSantillan.Model`: Entidades de dominio con encapsulamiento estricto (`getX()` / `setX()` con cuerpos de expresión `=>`, `copy()`, `clone()` en profundidad, `equals()`), contratos en `Interfaces/`, DTOs segregados en `Requests/` (`DataAnnotations` + `IValidatableObject`) y `Responses/`, y jerarquía de excepciones de dominio (`AppException`, `SystemMessages`).
  * `FederacionSantillan.Data`: Repositorios SQL (`DAOs/`) desacoplados con `MySqlConnector`, `Factories/ConnectionFactory` con resolución dinámica de variables de entorno para Docker (`ConnectionStrings__MariaDb` / `DB_CONNECTION_STRING`), `Translators/DbExceptionTranslator` (basado en `MySqlErrorCode` tipado, cero *magic numbers*) y generador autónomo de respaldos `DatabaseBackupDao`.
  * `FederacionSantillan.App`: Controladores *pass-through* con constructores primarios C# 12 (`Controllers/`), pipeline transversal de validación y manejo centralizado de errores estilo Spring Boot `@ControllerAdvice` (`Advice/ControllerPipeline` y `ControllerAdvice`), y vistas `WinForms` (`Views/`).
* **Dockerización Completa para Mac y Linux (Wine + noVNC Web Visual):** Arquitectura de contenedor multi-stage que compila la solución en .NET 8 SDK `win-x64` y la ejecuta sobre Wine embutido con servidor de visualización virtual X11 (`Xvfb`), gestor `fluxbox`, `x11vnc` y puente WebSocket `noVNC` en el puerto `8080`. Permite usar la aplicación WinForms 100% interactiva desde Safari, Chrome o Firefox en macOS (Apple Silicon M1/M2/M3/M4 o Intel) sin requerir máquinas virtuales ni emuladores Windows de terceros.
* **Emisión Transaccional ACID de Pólizas y Cuotas:** Al emitir una póliza, `PolicyDao` inserta la póliza y calcula/genera automáticamente sus $N$ cuotas mensuales consecutivas dentro de una única transacción SQL (`COMMIT` / `ROLLBACK`).
* **Motor de Alertas $O(1)$ e Integración con WhatsApp y CSV:** Sincronización automática de cuotas vencidas (`OVERDUE`) y próximas a vencer (`PENDING` a 7, 15 o 30 días), indexación en memoria `Dictionary<int, Installment>` en $O(1)$ y consultas SQL aceleradas mediante índices compuestos B-Tree InnoDB (`idx_installment_status_due` y `idx_installment_due_status`) en $O(\log M + K)$.
* **Validaciones Estrictas de Entrada:** DNI argentino de 7-8 dígitos, CUIT de 11 dígitos con algoritmo Módulo 11 oficial de AFIP, ingreso 100% numérico puro (filtrado de teclado `KeyPress` en tiempo real), restricción de dígitos en nombres de personas físicas y prevención total de fuga de información en autenticación (*"Credenciales inválidas"*).
* **Seguridad SHA-256 y Respaldo SQL DUMP Protegido:** Autenticación con hash criptográfico irreversible SHA-256 y doble verificación modal de contraseña (`FormVerificarPassword`) antes de exportar copias de seguridad completas (`.sql`).
* **Accesibilidad Visual Científica (WCAG 2.1 AA / Okabe-Ito):** Soporte dinámico de paleta para daltonismo (*Okabe-Ito*), modo Alto Contraste y escalado tipográfico en `FormOpciones`.

### Fundamentación Normativa y Estándares Públicos del DER (3FN)
1. **Marco Legal Argentino — [Ley Nacional N° 22.400](http://servicios.infoleg.gob.ar/infolegInternet/anexos/20000-24999/20968/texact.htm):** Régimen de Productores Asesores de Seguros (Superintendencia de Seguros de la Nación - SSN).
2. **Database Answers Canonical Insurance Model:** Estructura canónica `Party/Insured` $\rightarrow$ `Policy` $\rightarrow$ `Installments` $\rightarrow$ `Notifications`.
3. **ACORD Insurance Data Standards:** Separación en 3FN entre la entidad contractual (`policies`), el catálogo oficial de ramos (`branches`) y el cronograma financiero de facturación (`installments`).

---

## 2. Arquitectura Visual (Mermaid)

### A. Flujo de Datos y Arquitectura MVC en 3 Capas (`flowchart`)

```mermaid
flowchart TD
    subgraph AppLayer["Capa de Presentación y Control — FederacionSantillan.App"]
        Views["Views (WinForms Designer *.Designer.cs)<br/>FormLogin • FormRegistroUsuario • FormPrincipal<br/>FormVerificarPassword • FormOpciones • FormAyuda"]
        Pipeline["Advice / Pipeline Transversal<br/>ControllerPipeline (@Valid DTO)<br/>ControllerAdvice (Centralized Exception Handler)"]
        Controllers["Pass-Through Controllers (C# 12 Primary Ctor)<br/>AuthController • PortfolioController<br/>NotificationController • SettingsController"]
    end

    subgraph ModelLayer["Capa de Dominio — FederacionSantillan.Model"]
        DTOs["DTOs (DataAnnotations + IValidatableObject)<br/>Requests/*RequestDto • Responses/*ResponseDto"]
        Entities["Entities & Value Objects<br/>User • Branch • Insured • Policy<br/>Installment • Notification • CustomDate"]
        Contracts["Interfaces & Exceptions<br/>ICopiable • IClonable • IEquatableDomain<br/>AppException • SystemMessages"]
    end

    subgraph DataLayer["Capa de Persistencia — FederacionSantillan.Data"]
        Translator["Translators / Factories<br/>DbExceptionTranslator (MySqlErrorCode)<br/>ConnectionFactory (EnvVar / appsettings.json)"]
        DAOs["SQL Repositories (DAOs)<br/>UserDao • BranchDao • InsuredDao • PolicyDao<br/>InstallmentDao • NotificationDao • DatabaseBackupDao"]
    end

    DB[("MariaDB 11.4 (InnoDB / 3FN)<br/>santillan_seguros<br/>Port 3306 (Docker / Local)")]

    Views -->|"1. Envía Request DTO"| Pipeline
    Pipeline -->|"2. Validator.TryValidateObject"| DTOs
    Pipeline -->|"3. Despacha DTO válido"| Controllers
    Controllers -->|"4. Invoca operación"| DAOs
    DAOs -->|"5. Envuelve ejecución SQL"| Translator
    Translator -->|"6. Abre MySqlConnection"| DB
    DAOs -->|"7. Hidrata / Clona"| Entities
    Entities -.->|"Implementa"| Contracts
    Pipeline -.->|"Captura AppException -> ErrorProvider / MessageBox"| Views
```

### B. Arquitectura de Despliegue en Contenedores para Mac y Linux (Wine + noVNC Web Visual)

```mermaid
flowchart LR
    subgraph Host["Host Machine (macOS Sonoma/Sequoia / Linux / Windows)"]
        Browser["Navegador Web (Safari / Chrome / Firefox)<br/>http://localhost:8080"]
    end

    subgraph DockerNetwork["Docker Network: santillan-seguros_default"]
        subgraph AppContainer["Contenedor: santillan_seguros_app (Debian Slim)"]
            noVNC["noVNC Web Server & WebSockets<br/>Port 8080 (HTTP / WS)"]
            x11vnc["Servidor x11vnc<br/>Port 5900 (RFB Protocol)"]
            Xvfb["Xvfb Display :0<br/>Virtual Framebuffer (1366x768x24)"]
            Fluxbox["Fluxbox Window Manager"]
            Wine["Wine 64-bit Subsystem"]
            WinFormsApp["FederacionSantillan.App.exe<br/>(.NET 8 Windows Forms)"]

            noVNC -->|"Proxy WS -> RFB"| x11vnc
            x11vnc -->|"Captura Framebuffer"| Xvfb
            Fluxbox -->|"Gestiona Ventanas"| Xvfb
            WinFormsApp -->|"Renderiza GDI+/WinForms"| Wine
            Wine -->|"GDI+ -> X11"| Xvfb
        end

        subgraph DbContainer["Contenedor: santillan_seguros_db (MariaDB 11.4)"]
            Engine["MariaDB InnoDB Engine<br/>Port 3306 (TCP)"]
            DataVol[("Volumen Persistente<br/>mariadb_data")]
            Engine --- DataVol
        end
    end

    Browser <-->|"HTTP / WebSocket :8080"| noVNC
    WinFormsApp -->|"TCP 3306 (ConnectionStrings__MariaDb)"| Engine
```

### C. Modelo de Concurrencia, Transaccionalidad ACID y Fronteras de Hilo UI (`sequenceDiagram`)

```mermaid
sequenceDiagram
    autonumber
    actor PAS as Productor Asesor (UI Thread)
    participant View as FormPrincipal (WinForms STA)
    participant Pipe as ControllerPipeline / ControllerAdvice
    participant Ctrl as PortfolioController
    participant Dao as PolicyDao / DbExceptionTranslator
    participant Pool as MySqlConnector ConnectionPool
    participant DB as MariaDB 11.4 (InnoDB Engine)

    PAS->>View: Click "Emitir Póliza y Cuotas" (STA UI Thread)
    View->>Pipe: Execute(PolicyRequestDto, action, txtPolNum)
    Pipe->>Pipe: Validator.TryValidateObject(dto, validateAllProperties: true)
    alt DTO Inválido (Regla de Dominio o Rango de Fechas)
        Pipe-->>View: Lanza ValidationException -> ErrorProvider.SetError(txtPolNum)
    else DTO Válido
        Pipe->>Ctrl: IssuePolicy(PolicyRequestDto)
        Ctrl->>Dao: IssuePolicyWithInstallments(PolicyRequestDto)
        Dao->>Pool: ConnectionFactory.CreateConnection() & Open()
        Pool->>DB: BEGIN TRANSACTION (Isolation: RepeatableRead)
        Dao->>DB: INSERT INTO policies (...) VALUES (@num, @insured, @branch, ...)
        DB-->>Dao: LAST_INSERT_ID() -> newPolicyId
        loop Para i = 1 hasta InstallmentsCount (N cuotas mensuales)
            Dao->>DB: INSERT INTO installments (policy_id, installment_number, amount, due_date, status)
        end
        alt Violación UNIQUE (MySqlErrorCode.DuplicateKeyEntry = 1062) o Caída de Red (1042)
            DB-->>Dao: MySqlException (Number: 1062 / 1042)
            Dao->>DB: ROLLBACK TRANSACTION
            Dao-->>Pipe: DbExceptionTranslator traduce a DuplicateResourceException / DatabaseUnavailableException
            Pipe-->>View: ControllerAdvice.Handle() muestra MessageBox en STA Thread
        else Transacción Exitosa
            Dao->>DB: COMMIT TRANSACTION
            Dao->>Pool: Dispose() -> Devuelve conexión al Pool
            Ctrl-->>Pipe: Retorna SystemMessages.PolicyIssuedSuccess
            Pipe-->>View: ReloadAll() -> Refresca DataGridViews y KPIs en STA UI Thread
        end
    end
```

---

## 3. Auditoría y Garantías de Complejidad Algorítmica Big-O

Todas las operaciones críticas del sistema han sido auditadas matemáticamente para garantizar un rendimiento óptimo $O$ tanto a nivel de memoria RAM (.NET 8 C#) como a nivel del motor relacional MariaDB (árboles B-Tree en almacenamiento InnoDB).

### A. Operaciones en Memoria (.NET 8 C#)

| Componente / Algoritmo | Operación / Método | Complejidad Temporal $O$ | Complejidad Espacial $O$ | Justificación Técnica |
| :--- | :--- | :---: | :---: | :--- |
| **Indexación de Tablero** (`InstallmentDao`) | `LoadAlertsDashboard` | $\mathcal{O}(K_1 + K_2)$ | $\mathcal{O}(K_1 + K_2)$ | Construcción de tabla hash predimensionada `Dictionary<int, Installment>` para $K_1$ cuotas vencidas y $K_2$ próximas. |
| **Acceso a Cuota por ID** | `AlertsDashboardResponseDto.InstallmentIndex[id]` | $\mathcal{O}(1)$ promedio | $\mathcal{O}(1)$ | Acceso instantáneo por clave primaria en la tabla hash en memoria sin iterar la lista. |
| **Validador CUIT AFIP** (`ArgentineDocumentValidator`) | `ValidateCuit(string cuit)` | $\mathcal{O}(1)$ | $\mathcal{O}(1)$ | Longitud fija $L = 11$. Producto escalar con vector de ponderaciones $[5,4,3,2,7,6,5,4,3,2]$ y cálculo de residuo módulo 11 en tiempo constante. |
| **Validador DNI** (`ArgentineDocumentValidator`) | `ValidateDni(string dni)` | $\mathcal{O}(1)$ | $\mathcal{O}(1)$ | Longitud $7 \le L \le 8$. Validación de caracteres numéricos puros en tiempo acotado $\le 8$ pasos. |
| **Validador Nombre Persona** (`ArgentineDocumentValidator`) | `ValidateFullName(string name)` | $\mathcal{O}(L)$ | $\mathcal{O}(1)$ | Escaneo lineal de caracteres para asegurar ausencia de dígitos. Con $L \le 120$ caracteres, costo temporal despreciable. |
| **Hashing de Contraseñas** (`AuthController`) | `ComputeSha256(string password)` | $\mathcal{O}(M)$ | $\mathcal{O}(1)$ | Transformación criptográfica SHA-256 (bloques de 512 bits Merkle–Damgård). Con $M \le 64$ caracteres, opera en $\mathcal{O}(1)$ práctico. |
| **Plan de Cuotas** (`Policy` / `Installment`) | `CalculateInstallments(total, count)` | $\mathcal{O}(N)$ | $\mathcal{O}(N)$ | Generación lineal de $N$ cuotas ($1 \le N \le 12$) distribuyendo el monto base y compensando el redondeo de centavos en la primera cuota. |
| **Clonación Profunda de Entidades** | `Insured.clone()`, `Policy.clone()` | $\mathcal{O}(1)$ | $\mathcal{O}(1)$ | Copia inmutable de campos escalares y de valor en nuevas instancias de memoria. |

### B. Persistencia e Índices B-Tree (MariaDB 11.4 InnoDB)

| Tabla | Operación SQL | Índice B-Tree Utilizado | Complejidad Temporal $O$ | Tipo de Plan de Ejecución (`EXPLAIN`) |
| :--- | :--- | :--- | :---: | :--- |
| `users` | Autenticación (`GetByUsername`) | `UNIQUE (username)` | $\mathcal{O}(\log U)$ | `type: const / eq_ref` — Búsqueda en árbol B+ por clave única. |
| `insureds` | Verificación de CUIT/DNI (`ExistsTaxId`) | `UNIQUE (tax_id)` | $\mathcal{O}(\log N)$ | `type: const / eq_ref` — Acceso logarítmico directo sin escaneo de tabla. |
| `insureds` | Búsqueda por Nombre (`ORDER BY full_name`) | `INDEX idx_insured_name (full_name)` | $\mathcal{O}(\log N + S)$ | `type: range / index` — Lectura ordenada sin requerir `filesort`. |
| `policies` | Verificación de Póliza (`ExistsPolicyNumber`) | `UNIQUE (policy_number)` | $\mathcal{O}(\log P)$ | `type: const / eq_ref` — Index seek inmediato. |
| `policies` | Pólizas por Asegurado (`fk_policy_insured`) | `FOREIGN KEY (insured_id)` | $\mathcal{O}(\log P + R)$ | `type: ref` — Búsqueda logarítmica de las $R$ pólizas del cliente. |
| `installments` | Cuotas Vencidas (`GetOverdue`) | `INDEX idx_installment_status_due (status, due_date)` | $\mathcal{O}(\log M + K)$ | `type: ref` — Búsqueda directa por `status = 'OVERDUE'` y lectura secuencial de cuotas vencidas. |
| `installments` | Cuotas Próximas (`GetUpcoming`) | `INDEX idx_installment_due_status (due_date, status)` | $\mathcal{O}(\log M + K)$ | `type: range` — Index range scan sobre el intervalo de fechas sin tocar registros ajenos. |
| `notifications`| Auditoría Histórica | `INDEX idx_notification_date (generated_at)` | $\mathcal{O}(\log A + K)$ | `type: range` — Filtro cronológico rápido sobre el registro de alertas. |

---

## 4. Especificación de Contratos, Esquema JSON y Payloads SQL

### A. Esquema de Configuración (`src/FederacionSantillan.App/appsettings.json`)
```json
{
  "ConnectionStrings": {
    "MariaDb": "Server=localhost;Port=3306;Database=santillan_seguros;User ID=santillan;Password=santillan123;Connection Timeout=5;"
  }
}
```
> **Nota de Contenedor:** `ConnectionFactory` prioriza de forma transparente las variables de entorno `ConnectionStrings__MariaDb` o `DB_CONNECTION_STRING` provistas por Docker Compose, manteniendo compatibilidad total con ejecuciones locales nativas.

### B. Contratos DTO de Entrada / Salida y Códigos de Estado de Persistencia

#### Ejemplo de Payload de Entrada (`PolicyRequestDto`)
```json
{
  "PolicyNumber": 900107,
  "InsuredId": 1,
  "BranchId": 1,
  "Company": "Federación Patronal Seguros",
  "InsuredAsset": "Toyota Corolla Cross SEG 2025 - Patente AH102ZZ",
  "TotalPremium": 420000.00,
  "InstallmentsCount": 6,
  "StartDate": "2026-11-18",
  "EndDate": "2027-11-18",
  "FirstDueDate": "2026-12-10"
}
```

#### Ejemplo de Payload de Respuesta del Tablero (`AlertsDashboardResponseDto`)
```json
{
  "OverdueTotal": 160000.00,
  "UpcomingTotal": 90000.00,
  "Overdue": [
    {
      "InstallmentId": 12,
      "PolicyNumberRef": 900106,
      "InsuredNameRef": "Constructora del Sur SRL",
      "InsuredPhoneRef": "291-4550011",
      "BranchNameRef": "Responsabilidad Civil",
      "InstallmentNumber": 2,
      "Amount": 80000.00,
      "DueDate": "29/10/2026",
      "Status": "OVERDUE"
    }
  ]
}
```

#### Mapeo Determinístico de Códigos de Estado y Excepciones (`DbExceptionTranslator`)
| Código Motor / Validación | Enum / Origen | Excepción de Dominio Lanzada | Comportamiento en UI (`ControllerAdvice`) |
| :---: | :--- | :--- | :--- |
| **`200 / OK`** | Operación SQL / Validación exitosa | *(Ninguna)* | Actualiza grillas, KPIs y confirma acción. |
| **`400 / Validation`** | `Validator.TryValidateObject` / `IValidatableObject` | `ValidationException` | Marca el campo con `ErrorProvider` + `MessageBoxIcon.Warning`. |
| **`401 / Unauthorized`** | Usuario inexistente o contraseña incorrecta | `InvalidCredentialsException` | Alerta genérica *"Credenciales inválidas."* sin revelar la existencia del usuario (`MessageBoxIcon.Warning`). |
| **`404 / Not Found`** | Registro de negocio inexistente | `ResourceNotFoundException` | Informa recurso no encontrado (`MessageBoxIcon.Warning`). |
| **`409 / Conflict (1062)`** | `MySqlErrorCode.DuplicateKeyEntry` (`1062`) | `DuplicateResourceException` | Informa DNI/CUIT, usuario o N° de póliza duplicado. |
| **`503 / Unavailable (1042)`** | `MySqlErrorCode.UnableToConnectToHost` (`1042` / `0`) | `DatabaseUnavailableException` | Alerta caída o falta de conexión con el servidor MariaDB (`MessageBoxIcon.Error`). |

---

## 5. Galería Visual de la Aplicación (Capturas Reales en Vivo)

| Inicio de Sesión (`FormLogin`) | Alta de Usuario (`FormRegistroUsuario`) |
| :---: | :---: |
| ![FormLogin](docs/screenshots/01_form_login.png) | ![FormRegistroUsuario](docs/screenshots/02_form_registro_usuario.png) |

| Tablero Principal de Alertas y Morosos (`FormPrincipal`) | ABM de Cartera de Asegurados (`FormPrincipal`) |
| :---: | :---: |
| ![Alertas](docs/screenshots/03_form_principal_alertas.png) | ![Asegurados](docs/screenshots/04_form_principal_asegurados.png) |

| Emisión de Pólizas y Plan de Cuotas (`FormPrincipal`) | Opciones WCAG AA / Okabe-Ito y Exportación SQL DUMP |
| :---: | :---: |
| ![Polizas](docs/screenshots/05_form_principal_polizas.png) | ![Opciones](docs/screenshots/07_form_opciones.png) |

---

## 6. Guía de Despliegue en Docker para Mac / Linux / Windows y Ejecución

### Opción 1: Ejecución Completa con Docker Compose (Recomendada para Mac / Linux)
Esta opción levanta automáticamente tanto **MariaDB 11.4** como la aplicación **WinForms en Wine + noVNC** accesible directamente desde el navegador web sin emuladores de Windows.

```bash
# 1. Clonar el repositorio
git clone --recursive git@github.com:nedder3/santillan-seguros.git
cd santillan-seguros

# 2. Construir la imagen y levantar los contenedores
docker compose up --build -d
```

Una vez levantado:
* Abrir el navegador web en Mac o Linux: **`http://localhost:8080`**
* La aplicación se conectará automáticamente a la pantalla de Windows Forms en alta definición.
* **Credenciales de Acceso:**
  * **Rol Productor:** Usuario `santillan` | Contraseña `seguros2026`
  * **Rol Administrador:** Usuario `admin` | Contraseña `admin123`
* Para detener los servicios: `docker compose down`

---

### Opción 2: Ejecución Nativa en Windows (.NET SDK Local)

#### Paso 1: Levantar únicamente MariaDB en Docker
```bash
docker compose up -d mariadb
```

#### Paso 2: Compilar y Ejecutar la Suite de Pruebas Automatizadas (29 Tests)
```bash
dotnet restore FederacionSantillan.sln
dotnet build FederacionSantillan.sln --configuration Release
dotnet test FederacionSantillan.sln --logger "console;verbosity=normal"
```
> **Resultado verificado:** `29/29 tests passing` (resolución dinámica de conexión por variables de entorno, clonación profunda `clone()` / `copy()` / `equals()`, facturación y cálculo de mora en `Installment`, validaciones declarativas en DTOs, validación oficial de DNI y CUIT argentino 100% numérico puro mediante algoritmo Módulo 11, restricción estricta de caracteres en nombres y usuarios, números telefónicos numéricos, autenticación segura sin fuga de información e integración en vivo contra MariaDB).

#### Paso 3: Ejecutar la Aplicación Desktop
```bash
dotnet run --project src/FederacionSantillan.App/FederacionSantillan.App.csproj
```

---

## 7. Entregables Oficiales en PDF y Diagramas Nativos de Visual Studio

Todos los documentos requeridos por la cátedra (**Proyecto Final 2026** y **Rúbrica de Cotejo**) se encuentran generados y listos para entregar en `docs/pdf/`:

| Archivo PDF Entregable | Contenido Académico |
| :--- | :--- |
| **[docs/pdf/01_Estudio_Factibilidad_y_Entrevista.pdf](docs/pdf/01_Estudio_Factibilidad_y_Entrevista.pdf)** | Respuestas completas a las **6 preguntas oficiales de relevamiento**, requerimientos funcionales (`RF-01`..`RF-05`) y estudio de factibilidad técnica, operativa y económica. |
| **[docs/pdf/02_DER_Diccionario_Datos_y_DEPs.pdf](docs/pdf/02_DER_Diccionario_Datos_y_DEPs.pdf)** | **Modelo Entidad-Relación (DER en 3FN)** fundamentado en Ley N° 22.400 (SSN) y ACORD, **Diccionario de Datos** de las 6 tablas y **Especificación Estructurada de Procesos (DEP)**. |
| **[docs/pdf/03_Diagramas_UML_y_DFD.pdf](docs/pdf/03_Diagramas_UML_y_DFD.pdf)** | **Diagrama de Clases UML** en 3 capas, **Diagrama de Casos de Uso**, **Diagrama de Secuencia UML**, **Diagrama de Contexto (DFD Nivel 0)** y **DFD Nivel 1**. |
| **[docs/pdf/04_Manual_de_Usuario.pdf](docs/pdf/04_Manual_de_Usuario.pdf)** | **Manual de Usuario** paso a paso ilustrado con las **8 capturas reales** de la aplicación WinForms conectada a MariaDB (`docs/screenshots/`). |

### Diagramas de Clases Nativos para Visual Studio Class Designer (`.cd`)
* `src/FederacionSantillan.Model/ModelClassDiagram.cd`
* `src/FederacionSantillan.Data/DataLayerDiagram.cd`
* `src/FederacionSantillan.App/MvcArchitectureDiagram.cd`
