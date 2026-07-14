# Project-Victory

# Chat Transition Guide

Versão: 1.0

Status: Oficial

---

# Objetivo

Este documento define o procedimento oficial para continuar o desenvolvimento do Project-Victory em uma nova conversa do ChatGPT.

O objetivo é garantir continuidade do projeto sem depender do histórico da conversa anterior.

---

# Fonte de Verdade

A documentação oficial do Project-Victory é a única fonte de verdade.

Conversas anteriores nunca devem ser consideradas fonte oficial.

Caso exista conflito entre uma conversa e a documentação:

A documentação prevalece.

---

# Ordem Obrigatória de Leitura

Toda nova conversa deverá ler obrigatoriamente os seguintes documentos nesta ordem:

1.

developer-guide/roadmap.md

2.

developer-guide/project-state.md

3.

developer-guide/architecture-index.md

4.

developer-guide/decision-log.md

5.

developer-guide/glossary.md

6.

Toda a documentação indicada pelo Architecture Index.

---

# Fluxo Obrigatório

Após concluir a leitura da documentação:

1.

Produzir um resumo da arquitetura compreendida.

2.

Informar em qual módulo o projeto se encontra.

3.

Confirmar qual é o próximo módulo do Roadmap.

4.

Aguardar instruções.

Nenhuma implementação deverá começar antes dessa confirmação.

---

# Regras

Durante toda a conversa:

- Nunca assumir regras de Game Design.
- Nunca inventar comportamento.
- Nunca alterar módulos LOCKED.
- Nunca iniciar módulos fora do Roadmap.
- Nunca implementar código antes da documentação oficial.

---

# Questionários

Sempre que faltar informação relacionada ao comportamento do jogo:

O arquiteto deverá interromper a documentação e produzir um questionário objetivo.

O Product Owner responderá apenas sobre:

- mecânicas;
- experiência do jogador;
- comportamento esperado.

Toda decisão arquitetural permanece responsabilidade do arquiteto.

---

# Mudanças Arquiteturais

Caso uma mudança estrutural seja identificada:

1.

Registrar a necessidade.

2.

Avaliar impacto.

3.

Atualizar a documentação.

4.

Registrar uma nova ADR.

Somente após isso o desenvolvimento poderá continuar.

---

# Objetivo Final

Toda conversa nova deverá ser capaz de continuar o Project-Victory consultando apenas a documentação oficial.