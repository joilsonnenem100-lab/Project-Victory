# PV-VS-017 — Preparar Próxima Hunt

Status: Planned

Fase: FASE 1 — First Fun

Milestone: M2.2 — Continuidade do Loop Validada

---

# Objetivo

Adicionar ao Vertical Slice a capacidade de preparar uma nova Hunt após a finalização de uma execução anterior.

---

# Capacidade Entregue

Um Character que concluiu uma Hunt pode iniciar uma nova execução utilizando o estado atual mantido pelo sistema.

---

# Critério de Aceite

- Um Character válido pode iniciar uma nova Hunt após uma Hunt anterior.
- O estado atualizado do Character é utilizado.
- O contexto necessário para nova execução é criado corretamente.
- O sistema não reutiliza estado inválido da Hunt anterior.
- Resultados anteriores não interferem incorretamente na nova execução.
- O jogador consegue continuar o loop de gameplay.
- Nenhuma funcionalidade fora do escopo deste contrato é implementada.

---

# Fluxo Esperado
