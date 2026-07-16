# PV-VS-029 — Finalizar Combate

Status: Planned

Fase: FASE 4 — Combat Flow Inicial

Milestone: M3.5 — Ciclo de Combate Consolidado

---

# Objetivo

Adicionar ao Vertical Slice a capacidade de finalizar corretamente um combate após o processamento dos resultados de combate.

---

# Capacidade Entregue

Um Combat Context que atingiu uma condição de encerramento pode finalizar seu ciclo mantendo o estado consistente.

---

# Critério de Aceite

- Um combate ativo pode ser finalizado.
- O estado final do combate é definido corretamente.
- Um combate finalizado não aceita novas ações.
- O resultado final permanece disponível para o fluxo superior.
- A Hunt associada permanece consistente.
- Nenhuma funcionalidade fora do escopo deste contrato é implementada.

---

# Fluxo Esperado
