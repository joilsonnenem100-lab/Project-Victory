# PV-VS-046 — Criar Comando de Evolução do Character

Status: Planned

Fase: FASE 7 — Character Evolution

Bloco: Character Evolution Foundation

---

# Objetivo

Criar a estrutura de comando responsável por solicitar uma evolução ao Aggregate Character.

---

# Capacidade Entregue

O domínio Character recebe uma solicitação formal de evolução originada pelo fluxo de Progression.

---

# Critérios de Aceite

- Existe uma solicitação formal de evolução.
- A solicitação pertence ao fluxo Character.
- Progression não altera Character diretamente.
- Application apenas coordena.
- Nenhuma evolução é aplicada neste PV-VS.

---

# Fluxo

Progression Result

↓

Application

↓

Character Evolution Command

↓

Await Validation

---

# Fora do Escopo

Não implementar:

- Experience.
- Level.
- Level Up.
- Skills.
- Classes.
- Atributos.
- Aplicação de evolução.