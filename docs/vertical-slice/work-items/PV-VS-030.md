# PV-VS-030 — Retornar Controle para Hunt após Combate

Status: Planned

Fase: FASE 4 — Combat Flow Inicial

Milestone: M3.6 — Integração Combat → Hunt Validada

---

# Objetivo

Adicionar ao Vertical Slice a capacidade de retornar corretamente o controle da execução para a Hunt após o encerramento do combate.

---

# Capacidade Entregue

Um combate finalizado libera o fluxo da Hunt para continuar sua execução conforme o estado atual.

---

# Critério de Aceite

- Um Combat Context finalizado pode retornar controle para a Hunt associada.
- A Hunt permanece em estado consistente.
- O combate não pode continuar após encerramento.
- A transição Combat → Hunt ocorre somente em estados válidos.
- O Character permanece consistente.
- Nenhuma funcionalidade fora do escopo deste contrato é implementada.

---

# Fluxo Esperado
