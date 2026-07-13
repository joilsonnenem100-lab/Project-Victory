# VC-LAB — Solution Architecture

Status: Draft

Sprint: 037

## Objetivo

Definir a estrutura física da Solution do Project-Victory.

## Estrutura

ProjectVictory.sln

src/
    ProjectVictory.API
    ProjectVictory.Application
    ProjectVictory.Domain
    ProjectVictory.Infrastructure
    ProjectVictory.Persistence
    ProjectVictory.Runtime
    ProjectVictory.Shared

tests/
    ProjectVictory.Domain.Tests
    ProjectVictory.Runtime.Tests
    ProjectVictory.Application.Tests

## Princípios

- Monorepo
- Clean Architecture
- Domain First
- Runtime Separado
- Dependências apontam para o núcleo
- Testes desde o início