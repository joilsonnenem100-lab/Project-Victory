# PV-VS-047 — Validar Evolução no Character

Status: Planned

Fase: FASE 7 — Character Evolution

Bloco: Character Evolution Foundation

---

# Objetivo

Adicionar ao Aggregate Character a capacidade de validar uma solicitação de evolução.

---

# Capacidade Entregue

Character consegue decidir se uma evolução solicitada respeita suas próprias invariantes.

---

# Critérios de Aceite

- Character valida solicitações recebidas.
- Character continua dono das invariantes.
- Solicitações inválidas são rejeitadas.
- Nenhuma regra de Progression é criada no Character.
- Nenhuma alteração ocorre neste PV-VS.

---

# Fluxo

Evolution Request

↓

Character Validation

↓

Accepted / Rejected

---

# Fora do Escopo

Não implementar:

- Alteração de atributos.
- Experience.
- Level.
- Skills.
- Classes.