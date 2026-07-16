# PV-VS-035 — Encaminhar Reward para Progression

Status: Planned

Fase: FASE 6 — Progression Integration

Milestone: M5.1 — Integração Reward → Progression Validada

---

# Objetivo

Adicionar ao Vertical Slice a capacidade de encaminhar o resultado de uma Reward válida para o fluxo de Progression.

---

# Capacidade Entregue

Uma recompensa processada pelo Reward Domain pode disponibilizar seus efeitos para o domínio de Progression sem transferir responsabilidades entre domínios.

---

# Critério de Aceite

- Uma Reward processada pode gerar uma solicitação para Progression.
- Progression recebe somente dados necessários para processar evolução.
- Reward continua responsável pelo processamento da recompensa.
- Progression continua responsável pelas regras de evolução.
- Character continua dono das próprias invariantes.
- Nenhuma evolução é aplicada neste contrato.

---

# Responsabilidade Arquitetural

Fluxo obrigatório:
