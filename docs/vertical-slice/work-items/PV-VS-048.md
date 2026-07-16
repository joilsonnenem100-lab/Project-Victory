# PV-VS-048 — Aplicar Evolução Controlada no Character

Status: Planned

Fase: FASE 7 — Character Evolution

Bloco: Character Evolution Foundation

---

# Objetivo

Permitir que o Aggregate Character aplique uma evolução previamente validada.

---

# Capacidade Entregue

Character consegue alterar seu próprio estado através de uma operação controlada.

---

# Critérios de Aceite

- Apenas Character altera seu estado interno.
- Alterações passam pelas invariantes.
- Progression não modifica Character diretamente.
- Application não possui regra de domínio.
- Estado final permanece consistente.

---

# Fluxo

Progression Result

↓

Evolution Request

↓

Character Validation

↓

Character Apply Evolution

↓

Updated Character State

---

# Fora do Escopo

Não implementar:

- Experience.
- Level.
- Level Up.
- Skills.
- Classes.
- Balanceamento.
- Sistemas futuros.