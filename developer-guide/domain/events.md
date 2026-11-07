# Events

Status: LOCKED (2026-11-06, `PV-GOV-013`, com a emenda do `ADR-058`; antes: DRAFT)

Version: 1.0

---

# Objetivo

Definir o contrato arquitetural do módulo Events.

---

# Responsabilidade

O módulo Events é responsável exclusivamente pela publicação e distribuição de Domain Events entre os módulos da Engine.

---

# Responsabilidades

- publicar Domain Events;
- distribuir Domain Events;
- registrar Subscribers;
- executar Subscribers;
- manter contratos tipados.

---

# Dependências

O módulo Events não depende de módulos de domínio.

---

# Proibições

O módulo Events nunca poderá:

- conhecer Character;
- conhecer Combat;
- conhecer Hunt;
- conhecer Inventory;
- conhecer Loot;
- conhecer Quest;
- conhecer qualquer regra de negócio.

---

# Engine Event

Todo Domain Event:

- deriva de EngineEvent;
- possui EventId;
- possui Timestamp;
- pode possuir CorrelationId;
- pode possuir CausationId;
- é imutável.

---

> **Emenda (2026-11-06, `ADR-058`, `PV-EVENT-012`):** "síncrono" = o publisher aguarda todos os assinantes (handlers assíncronos **aguardados** cumprem a regra; sem fire-and-forget). "Nunca são obrigatórios" vale para o **publisher** (não depende de assinante); um assinante pode ser obrigatório para a **composição** (ex.: Reward), garantido por teste de composição. Notificações pós-commit (`PublishNotificationAsync`) isolam a falha do assinante.

# Event Bus

O Event Bus:

- é síncrono;
- fortemente tipado;
- stateless;
- executa Subscribers registrados.

---

# Subscribers

Subscribers:

- registram interesse por tipos específicos;
- nunca são obrigatórios;
- nunca são conhecidos pelos Publishers.

---

# Publicação

Toda publicação ocorre de forma síncrona.

O Publisher somente continua sua execução após todos os Subscribers concluírem.

---

# Granularidade

Cada Event representa exatamente um fato de negócio.

---

# Escopo

O Event Bus transporta exclusivamente Domain Events.

Eventos de infraestrutura utilizam mecanismos próprios.

---

# Filosofia

Publishers publicam.

Subscribers reagem.

O Event Bus conecta ambos.

---

# Fluxo

Publisher

↓

Event Bus

↓

Subscribers

---

# Referências

Ver também:

- hunt.md
- combat.md
- spawn.md