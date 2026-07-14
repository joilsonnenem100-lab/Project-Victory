# ADR-010 - Hunt Driven Spawn

Status: ACCEPTED

Version: 1.0

---

# Contexto

O conteúdo do Project-Victory deve ser definido pelo Game Design e não pela Engine.

O Spawn System deve permanecer genérico para suportar futuras expansões sem necessidade de alterações estruturais.

---

# Decisão

Cada Hunt será responsável por definir sua própria configuração de Spawn.

A configuração poderá incluir:

- Monster Pool;
- Pack Size;
- Spawn Delay;
- Elite Chance;
- futuras configurações específicas.

O Spawn System apenas interpreta essa configuração.

---

# Consequências

Benefícios:

- separação entre conteúdo e Engine;
- facilidade de balanceamento;
- reutilização de MonsterDefinitions;
- facilidade para criação de novas Hunts;
- escalabilidade.

Custos:

- necessidade de configuração consistente das Hunts.

---

# Aplicações

Este princípio aplica-se a:

- Hunts;
- Spawn System;
- Monster Pool;
- Monster Spawn Service.

---

# Restrições

O Spawn System nunca poderá possuir configurações específicas de uma Hunt.

Toda configuração pertence exclusivamente à Hunt.

---

# Referências

Ver também:

- developer-guide/domain/spawn-system.md
- developer-guide/domain/monsters.md