# PV-VS-021 — Resolver Resultado da Hunt

Status: Planned

Fase: FASE 2 — Combat Flow Inicial

Milestone: M2.6 — Resultado da Hunt Consolidado

---

# Objetivo

Adicionar ao Vertical Slice a capacidade de resolver o resultado final produzido pela execução de uma Hunt.

---

# Capacidade Entregue

Uma Hunt executada corretamente possui seu resultado interpretado e direcionado para o fluxo correspondente.

---

# Critério de Aceite

- Uma execução de Hunt gera um resultado válido.
- O resultado da execução é identificado corretamente.
- O estado final da Hunt é atualizado conforme o resultado.
- Resultados de sucesso seguem para Reward.
- Resultados sem sucesso seguem o fluxo correspondente.
- Cancelamentos permanecem sem Reward.
- O Character mantém estado consistente.
- Nenhuma funcionalidade fora do escopo deste contrato é implementada.

---

# Fluxo Esperado
