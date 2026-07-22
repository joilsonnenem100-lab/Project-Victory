# PROJECT-VICTORY --- ARCHITECTURE DECISION LOG

## Objetivo

Registrar decisões arquiteturais importantes, contexto e consequências.

Este documento não substitui contratos oficiais. Ele preserva o
raciocínio arquitetural.

------------------------------------------------------------------------

# Decision: Separação Character State e Runtime State

Status:

Approved Concept

Contexto:

O personagem possui estado permanente, enquanto atividades possuem
estado temporário.

Decisão:

Separar identidade/progressão do contexto de execução.

Consequência:

Character não deve armazenar Hunt ou Combat atual.

------------------------------------------------------------------------

# Decision: Content Definition separado de Runtime Instance

Status:

Architectural Principle

Contexto:

Conteúdo criado pelo design não deve ser confundido com entidades
existentes durante execução.

Decisão:

Usar separação:

CONTENT

↓

RUNTIME INSTANCE

↓

PERSISTENCE
