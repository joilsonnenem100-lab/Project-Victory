# ADR-055 - Event-Driven Quests

Status: PROPOSED

Version: 1.0

Data: 2026-11-06 · Preenchido em `PV-GOV-007` (proposta do `PV-GOV-005`). O arquivo existia vazio desde o commit `9f54994`.

---

# Contexto

Quests precisam reagir a acontecimentos do jogo (por exemplo, monstro derrotado ou item obtido) sem que Combat, Loot ou Reward conheçam o módulo de Quest.

Já estão decididos:

- ADR-049: a Engine usa um Event Bus fortemente tipado; todos os Events derivam de `EngineEvent`; o despacho nunca é por nome em texto.
- ADR-050: o Event Bus transporta exclusivamente Domain Events.
- ADR-056: cada Quest Definition possui uma Completion Policy; a decisão de conclusão pertence exclusivamente a ela.
- ADR-024: todo ganho permanente passa pelo Reward Pipeline.

---

# Decisão (proposta)

- O progresso de uma Quest é atualizado **somente** pela assinatura de Domain Events (ADR-050) no Typed Event Bus (ADR-049).
- Nenhum sistema chama o módulo de Quest diretamente.
- A conclusão é decidida pela Completion Policy da Quest Definition (ADR-056).
- As recompensas de Quest passam pelo Reward Pipeline (ADR-024).

---

# Consequências

Benefícios:

- Combat, Loot e Reward não dependem de Quest;
- novas Quests não exigem mudança nos sistemas que geram os eventos.

Custos:

- depende do Event System (M7: `PV-EVENT-010`, `PV-EVENT-011`), que ainda não existe no código.

---

# Restrições

- Quest não altera estado de outro agregado (Aggregate Ownership — `CLAUDE.md` e `developer-guide/architecture/architecture-rules.md`).
- Eventos são tipados, nunca string (ADR-049).

---

# Pendências para ACCEPTED

- Aceite do Arquiteto depois do M7 (Event System implementado).
- O conteúdo das Quests (quais existem, objetivos, recompensas) é decisão do PO.

---

# Referências

- ADR-024, ADR-049, ADR-050, ADR-056
- `developer-guide/architecture/roadmap-to-playable.md` (M7)
- `docs/architecture/PV-GOV-005-RELATORIO-2026-11-06.md` (seção a)
