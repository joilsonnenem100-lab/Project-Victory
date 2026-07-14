# ADR-009 - Spawn System Stateless

Status: ACCEPTED

Version: 1.0

---

# Contexto

O Project-Victory é um Idle MMORPG onde o combate ocorre continuamente durante uma Hunt.

Os Monsters existem apenas enquanto participam do combate ativo.

Persistir packs entre Hunts aumentaria a complexidade da Engine, criaria estados desnecessários e abriria espaço para inconsistências e exploração de mecânicas.

---

# Decisão

O Spawn System será completamente stateless.

Nenhum pack será persistido.

Cada novo pack será criado exclusivamente sob demanda.

Ao encerrar uma Hunt, todas as instâncias de Monsters serão descartadas.

Ao iniciar uma nova Hunt, um novo Spawn será executado.

---

# Consequências

Benefícios:

- reduz consumo de memória;
- elimina estados persistentes desnecessários;
- simplifica recuperação de falhas;
- reduz acoplamento entre Hunts;
- melhora a escalabilidade.

Custos:

- toda nova Hunt sempre inicia com um novo Spawn.

---

# Aplicações

Este princípio aplica-se a:

- Spawn System;
- Monster Pool;
- Monster Spawn Service;
- Combat Engine.

---

# Restrições

O Spawn System nunca poderá:

- persistir packs;
- manter histórico de packs;
- reutilizar Monsters;
- compartilhar estado entre Hunts.

Cada Spawn representa um novo ciclo de combate.

---

# Referências

Ver também:

- developer-guide/domain/spawn-system.md
- developer-guide/domain/monsters.md