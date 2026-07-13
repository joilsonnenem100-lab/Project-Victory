# Architecture Rules

Status: LOCKED

Version: 1.0

---

# Objetivo

Este documento define as regras obrigatórias de arquitetura do Project-Victory.

Nenhum código poderá violar estas regras.

---

# Regra 1

Engine First

Toda regra do jogo pertence à Engine.

Nunca à API.

Nunca ao Banco.

Nunca ao Cliente.

---

# Regra 2

Domain First

Toda regra nasce primeiro na Domain.

Depois é utilizada pela Application.

Nunca o contrário.

---

# Regra 3

Template → Factory → Instance

Toda entidade dinâmica do jogo deve seguir obrigatoriamente este fluxo.

Template

↓

Factory

↓

Instance

---

# Regra 4

Event Driven

Sempre que possível, sistemas comunicam-se através de eventos.

Nunca diretamente.

---

# Regra 5

Commands

Toda alteração no jogo ocorre através de Commands.

Nunca através de Controllers.

---

# Regra 6

Runtime

Toda simulação existe apenas em memória.

Nada relacionado ao Runtime deve depender diretamente do banco.

---

# Regra 7

Persistence

Persistimos apenas os resultados.

Nunca persistimos a simulação.

---

# Regra 8

Composition Over Inheritance

Sempre preferir composição.

Herança apenas quando realmente representar uma relação "é um".

---

# Regra 9

Small Modules

Criar novos módulos.

Nunca aumentar indefinidamente módulos existentes.

---

# Regra 10

Bounded Context

Cada domínio deve possuir responsabilidade única.

Inventory nunca conhece Market.

Combat nunca conhece Guild.

Guild nunca conhece Combat.

---

# Regra 11

Compatibility First

Toda decisão arquitetural deve responder:

"Isso permite expansão futura?"

Se não permitir,

a solução deve ser reavaliada.

---

# Regra 12

Stable Core

Expandimos o sistema adicionando novos módulos.

Nunca alterando o núcleo.

---

# Regra 13

Simulation First

A Engine calcula.

A Interface interpreta.

O servidor nunca renderiza animações.

---

# Regra 14

Single Responsibility

Cada classe resolve apenas um problema.

---

# Regra 15

SOLID

Todos os módulos devem respeitar SOLID.

Quando houver conflito entre praticidade e SOLID,

priorizar a arquitetura do Project-Victory.

---

# Regra 16

Clean Architecture

Dependências sempre apontam para dentro.

API

↓

Application

↓

Domain

Nunca o inverso.

---

# Regra 17

Zero Framework Domain

A Domain nunca depende de:

- ASP.NET
- EF Core
- Redis
- PostgreSQL
- Blazor

---

# Regra 18

Scalability

Toda implementação deve considerar expansão futura.

Mesmo que inicialmente exista apenas um servidor.

---

# Regra 19

Performance

O banco nunca participa da simulação.

Todo combate ocorre em memória.

---

# Regra 20

Arquitetura acima da facilidade

Nunca escolher uma solução apenas porque exige menos código.

Sempre escolher a solução que melhor preserva a arquitetura do projeto.