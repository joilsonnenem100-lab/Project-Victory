# PV-VS-027 — Technical Contract

## Nome

Aplicar Resultado de Combate

---

# Objetivo

Adicionar ao Vertical Slice a capacidade de aplicar um resultado de combate válido ao contexto de combate ativo.

---

# Escopo

Implementar exclusivamente:

- receber resultado de combate;
- validar CombatContext associado;
- validar resultado recebido;
- aplicar resultado ao fluxo Combat;
- atualizar estado permitido do CombatContext;
- retornar confirmação da aplicação.

---

# Fluxo esperado
