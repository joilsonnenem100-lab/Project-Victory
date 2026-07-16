# PV-VS-028 — Technical Contract

## Nome

Finalizar Combate Após Aplicação do Resultado

---

# Objetivo

Permitir que um CombatContext com resultado aplicado finalize corretamente seu ciclo de combate.

---

# Escopo

Implementar exclusivamente:

- receber solicitação de finalização do combate;
- validar CombatContext existente;
- validar condição de finalização;
- alterar estado permitido;
- retornar confirmação da finalização.

---

# Fluxo esperado
