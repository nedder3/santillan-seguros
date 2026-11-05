---
type: content
tags: []
created: 2026-10-20
---

# <project-name>

<!-- Optional logo/banner: place it in docs/assets/ and reference it here (supports light/dark on GitHub). -->
<!-- <p align="center"><img src="docs/assets/banner.png" alt="<project>" width="720"/></p> -->

<!-- BADGES: keep minimal and real. Generate at https://shields.io.
     Group 1: project status. Group 2: technology (one per relevant stack). -->
[![standard-readme compliant](https://img.shields.io/badge/readme%20style-standard-brightgreen.svg?style=flat-square)](https://github.com/RichardLitt/standard-readme)
[![license](https://img.shields.io/badge/license-MIT-blue.svg?style=flat-square)](LICENSE)
[![version](https://img.shields.io/badge/version-0.1.0-blue.svg?style=flat-square)](https://github.com/<user>/<repo>/releases)
[![CI](https://img.shields.io/badge/CI-passing-brightgreen.svg?style=flat-square)](https://github.com/<user>/<repo>/actions)
[![deploy](https://img.shields.io/badge/deploy-GitHub%20Pages-2ea44f?style=flat-square)](https://<user>.github.io/<repo>/)

<!-- Technology badges (uncomment/adjust to the project's real stack). -->
<!-- [![language](https://img.shields.io/badge/language-JavaScript-ffca28.svg?style=flat-square)]() -->
<!-- [![runtime](https://img.shields.io/badge/runtime-Node.js%2018%2B-339933.svg?style=flat-square)]() -->
<!-- [![framework](https://img.shields.io/badge/framework-Vanilla%20ESM-000000.svg?style=flat-square)]() -->

> A sentence summarizing what it does and why it exists.

## Table of contents

- [Background](#background)
- [Features](#features)
- [Tech stack](#tech-stack)
- [Quick Start](#quick-start)
- [Installation](#installation)
- [Usage](#usage)
- [Architecture and diagrams](#architecture-and-diagrams)
- [Configuration](#configuration)
- [Contributing](#contributing)
- [License](#license)

## Background

What problem does it solve? Why this implementation and not another? Context and
design decisions. If it competes with or is an alternative to another project, a
short comparison table helps:

| Capability | This project | Alternative |
|---|:---:|:---:|
| Key requirement A | Yes | No |
| Key requirement B | Yes | Yes |

## Features

- **Feature 1:** describe the concrete capability and use case.
- **Feature 2:** ...
- **Feature 3:** ...

## Tech stack

| Layer | Technology | Notes |
|------|-----------|-------|
| Language | JavaScript (ESM) | Native browser modules |
| Runtime | Node.js 18+ | Dev server only |
| Framework | Vanilla / no build | GitHub Pages serves static |
| Tests | Vitest | TDD |
| Deploy | GitHub Pages | No backend |

## Quick Start

The minimum to see it working (happy path, no long explanation):

```bash
git clone https://github.com/<user>/<repo>.git
cd <repo>
# Open index.html directly or serve statically:
python -m http.server 8000
# Then open http://localhost:8000
```

## Installation

Detailed setup steps (dependencies, build if applicable):
```bash
npm install
npm run dev
```

## Usage

How to use the prototype from the browser or the API. Concrete, working examples:
```js
// minimal example
import { algo } from './src/algo.js';
algo();
```

## Architecture and diagrams

Cy generates the flow/mechanism diagrams in [`diagramas/`](diagramas/). They are
viewed in Obsidian (canvas/mermaid) or rendered from the vault. One figure = one
assertion.

Directory tree:

```text
src/
  core/         storage + registry (infra base)
  services/     per-service logic
  ui/           shell + views
index.html      entry point
docs/           design + tutorial
```

```mermaid
flowchart TD
  U[User] -->|acts on| SHELL[UI shell with tabs]
  SHELL -->|registry.list()| REG[ServiceRegistry: single source]
  REG -->|registry.create| SVC[Services: pure logic]
  SVC -->|reads/writes| AD[StorageAdapter]
  AD -->|runtime| LS[(localStorage)]
```

## Configuration

Environment variables / flags (if applicable). Keep the table even if minimal:

| Variable | Default | Description |
|----------|---------|-------------|
| `PORT` | `5173` | Dev server port |
| `MODE` | `memory` | Persistence mode |

## Contributing

Read [CONTRIBUTING.md](CONTRIBUTING.md). Key rules:
- Conversational commits with scope: `feat(proy): ...`, `fix(proy): ...`, `docs(proy): ...`
- TDD: tests before declaring done.
- JSDoc/TSDoc on every public symbol.

See [CHANGELOG.md](CHANGELOG.md) for the change history.

## License

[MIT](LICENSE) © <author>
