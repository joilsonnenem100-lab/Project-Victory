# VC-LAB — Folder Structure

Status: LOCKED

Version: 1.0

---

# Objetivo

Definir a estrutura física oficial do repositório Project-Victory.

Toda implementação deverá respeitar esta organização.

---

ProjectVictory/

docs/

engineering/

knowledge-base/

developer-guide/

vc-lab/

src/

tests/

tools/

.github/

---

# src/

src/

ProjectVictory.API/

ProjectVictory.Application/

ProjectVictory.Domain/

ProjectVictory.Runtime/

ProjectVictory.Persistence/

ProjectVictory.Infrastructure/

ProjectVictory.Shared/

---

# Domain

ProjectVictory.Domain/

Entities/

ValueObjects/

Events/

Interfaces/

Factories/

Services/

Components/

Systems/

Specifications/

Enums/

Exceptions/

---

# Application

Commands/

Queries/

Handlers/

DTO/

Contracts/

Validators/

Mappings/

Pipelines/

---

# Runtime

Engine/

GameLoop/

Components/

Systems/

Factories/

Instances/

Snapshots/

Recorder/

RuntimeEvents/

Schedulers/

---

# Persistence

Repositories/

Configurations/

Migrations/

Context/

Seed/

Converters/

---

# Infrastructure

Email/

Storage/

Logging/

Cache/

Configuration/

Discord/

Telemetry/

---

# API

Controllers/

Middleware/

Filters/

Authentication/

Authorization/

OpenApi/

Extensions/

---

# Shared

Constants/

Results/

Errors/

Primitives/

Utilities/

---

# Tests

ProjectVictory.Domain.Tests/

ProjectVictory.Runtime.Tests/

ProjectVictory.Application.Tests/

ProjectVictory.Persistence.Tests/

Integration/

Performance/

---

# Ferramentas

tools/

WorldEditor/

DataImporter/

BalanceTool/

LootSimulator/

CombatSimulator/

MigrationTool/

---

# Regra

Novos diretórios somente quando representarem um novo domínio.

Nunca criar pastas genéricas como:

Misc/

Utils/

Helpers/

Temp/

Common/

Caso exista dúvida,

a arquitetura deve ser revisada antes da criação.