# ADR-002 - Separação entre Engine e Presentation Layer

Status: ACCEPTED

Version: 1.0

---

# Contexto

O Project-Victory é um Idle MMORPG.

O combate é processado exclusivamente pela Engine.

A Interface existe apenas para representar visualmente o estado produzido pela Engine.

---

# Decisão

A Engine será a única fonte de verdade.

A Presentation Layer nunca executará regras de domínio.

Toda representação visual deverá ser baseada exclusivamente no estado produzido pela Engine.

A simulação visual do combate não interfere no resultado da Hunt.

---

# Consequências

Benefícios:

- desacoplamento entre Engine e Interface;
- suporte a múltiplos clientes;
- suporte ao modo Hunt Offline;

  *Nota de emenda (2026-11-06, `PV-GOV-007` / `DEC-20261106-13`, R-013): não existe Hunt Offline. Por decisão do PO (`DEC-20261106-05`), registrada no `ADR-057` (ACCEPTED), a Hunt **termina ao desconectar** e o progresso offline vem **só do Offline Training de skills**. O texto acima foi mantido por histórico e não vale mais.*

- maior segurança;
- menor possibilidade de exploração.

Custos:

- necessidade de sincronização entre Engine e Interface.

---

# Aplicações

Este princípio aplica-se a:

- Combat;
- Monsters;
- Hunts;
- Skills;
- Effects;
- Interface.

---

# Restrições

A Presentation Layer nunca poderá:

- calcular dano;
- gerar Loot;
- criar Monsters;
- controlar Hunts;
- alterar estados da Engine.

Toda lógica pertence exclusivamente à Engine.

---

# Referências

Ver também:

- developer-guide/domain/monsters.md
- developer-guide/domain/combat.md