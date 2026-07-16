# PV-VS-023 — Iniciar Combate da Hunt

Status: Planned

Fase: FASE 4 — Combat Flow Inicial

Milestone: M3.1 — Combat Context Validado

---

# Objetivo

Adicionar ao Vertical Slice a capacidade de iniciar um contexto de combate associado a uma Hunt ativa.

---

# Capacidade Entregue

Uma Hunt em execução pode criar e manter um contexto de combate válido para processamento futuro.

---

# Critério de Aceite

- Uma Hunt em estado Running pode iniciar combate.
- O combate possui um estado inicial válido.
- O contexto de combate está associado corretamente à Hunt.
- Uma Hunt inválida não pode iniciar combate.
- O encerramento da Hunt encerra o contexto de combate associado.
- Nenhuma regra de dano ou resultado de combate é implementada neste contrato.

---

# Fluxo Esperado
