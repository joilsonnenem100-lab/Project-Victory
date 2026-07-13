# AI WORKFLOW

Status: LOCKED

Version: 1.0

---

# Objetivo

Este documento define como qualquer Inteligência Artificial deve atuar durante o desenvolvimento do Project-Victory.

A IA nunca deve tomar decisões arquiteturais por conta própria.

Sempre deve seguir este fluxo.

---

# Fonte da Verdade

A fonte oficial do projeto é o repositório Git.

As conversas são apenas ferramentas de apoio.

Sempre priorizar a documentação existente.

---

# Ordem de Leitura Obrigatória

Antes de iniciar qualquer Sprint:

1. docs/PROJECT_CONTEXT.md

2. docs/CURRENT_CONTEXT.md

3. developer-guide/

4. engineering/decision-log.md

5. knowledge-base/

6. vc-lab/

---

# Fluxo Oficial

Toda implementação deve seguir obrigatoriamente:

Arquitetura

↓

Documentação

↓

Implementação

↓

Testes

↓

Atualização da Documentação

↓

Commit

---

# Durante a Implementação

A IA deve:

- Respeitar toda a arquitetura do projeto.
- Nunca simplificar uma solução quebrando a arquitetura.
- Priorizar escalabilidade.
- Priorizar compatibilidade futura.
- Priorizar código limpo.
- Seguir o Developer Guide.

---

# Quando encontrar conflitos

Caso exista qualquer conflito entre:

- documentação;
- arquitetura;
- implementação;

A IA deve interromper imediatamente a implementação.

Nunca assumir uma decisão.

Sempre comunicar o conflito ao Arquiteto.

---

# Architecture Safety Check

Sempre que uma limitação do ambiente conversacional puder comprometer:

- arquitetura;
- engenharia;
- documentação;
- consistência;
- escalabilidade;

A implementação deve ser interrompida.

Primeiro documentar.

Depois continuar.

---

# Final de toda Sprint

Toda Sprint deve entregar obrigatoriamente:

- Resumo Executivo
- Arquivos Novos
- Arquivos Alterados
- Arquivos Removidos
- Testes
- Migrations
- Documentação Atualizada
- Checklist
- Status

---

# Atualização dos Documentos

Ao finalizar uma Sprint atualizar:

docs/CURRENT_CONTEXT.md

docs/CHANGELOG.md

Se necessário:

engineering/decision-log.md

developer-guide/

knowledge-base/

vc-lab/

---

# Filosofia Final

O objetivo não é produzir código rapidamente.

O objetivo é produzir um software que possa evoluir por muitos anos preservando sua arquitetura.