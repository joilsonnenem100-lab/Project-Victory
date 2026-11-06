# PROJECT CONTEXT

Status: LOCKED

> **Emenda (2026-11-06, `PV-GOV-007` / `DEC-20261106-13`):** revisão arquitetural formal autorizada pelo Arquiteto-Chefe. As referências a `docs/CURRENT_CONTEXT.md` (documento histórico) foram trocadas por `developer-guide/architecture/project-state.md`, estado oficial (`CLAUDE.md`). O texto antigo foi mantido tachado.

Version: 1.0

---

# Projeto

Nome: Project-Victory

Objetivo:

Desenvolver um MMORPG Idle inspirado em Tibia, focado em progressão automática através de Hunts, arquitetura escalável e alta manutenibilidade.

---

# Filosofia

O Project-Victory segue os princípios:

- Simplicidade
- Escalabilidade
- Engine First
- Domain First
- Clean Architecture
- Event Driven
- Template → Factory → Instance
- Runtime Isolation

Toda decisão deve preservar esses princípios.

---

# Estrutura Oficial

A documentação oficial do projeto está organizada em:

developer-guide/

engineering/

knowledge-base/

vc-lab/

docs/

---

# Fonte da Verdade

A fonte oficial do projeto é o repositório Git.

As conversas com IA auxiliam o desenvolvimento, mas não substituem a documentação versionada.

---

# Fluxo Oficial

Toda implementação segue obrigatoriamente:

Arquitetura

↓

Documentação

↓

Implementação

↓

Testes

↓

Commit

---

# Regras

Nunca alterar a arquitetura sem criar uma nova DEC.

Nunca implementar regras de negócio sem documentação suficiente.

Sempre respeitar o Developer Guide.

---

# Documentos Obrigatórios

Antes de iniciar qualquer Sprint:

1. PROJECT_CONTEXT.md
2. developer-guide/architecture/project-state.md *(antes: ~~CURRENT_CONTEXT.md~~ — `PV-GOV-007`)*
3. developer-guide/
4. engineering/decision-log.md

---

# Objetivo Final

Construir um MMORPG escalável, organizado e de fácil manutenção, permitindo evolução por muitos anos sem comprometer a arquitetura.