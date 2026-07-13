# VC-LAB — Project Dependencies

Status: LOCKED

Sprint: 049

## Objetivo

Definir as dependências oficiais entre os projetos da Solution.

## Grafo

API
↓
Application
↓
Domain

Runtime
↓
Domain

Persistence
↓
Domain

Infrastructure
↓
Domain

Shared

## Regras

- Dependências são unidirecionais.
- Não existem referências circulares.
- Domain é o núcleo do sistema.
- Runtime é independente.
- Shared não depende de nenhum projeto interno.