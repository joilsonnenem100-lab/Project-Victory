# Project Workflow

Status: LOCKED

Version: 1.0

---

# Objetivo

Este documento define o fluxo oficial de desenvolvimento do Project-Victory.

Nenhuma funcionalidade deve ser implementada fora deste processo.

---

# Filosofia

Toda funcionalidade nasce primeiro na arquitetura.

Nunca diretamente no código.

Fluxo:

Pensar

↓

Projetar

↓

Documentar

↓

Implementar

↓

Testar

↓

Commit

---

# Fluxo Oficial

## Etapa 1

Necessidade

↓

Nova funcionalidade identificada.

---

## Etapa 2

Análise

Responder:

Essa funcionalidade afeta:

- Gameplay?
- Arquitetura?
- Runtime?
- Banco?
- API?
- Interface?

---

## Etapa 3

Decisão

Caso gere uma decisão arquitetural:

Criar uma DEC.

Registrar no:

engineering/decision-log.md

---

## Etapa 4

Documentação

Atualizar apenas os documentos necessários.

Exemplos:

knowledge-base/

developer-guide/

vc-lab/

Nunca atualizar arquivos sem necessidade.

---

## Etapa 5

Modelagem

Se necessário:

- Banco
- Runtime
- Domain
- API

---

## Etapa 6

Implementação

Somente após:

Arquitetura aprovada.

---

## Etapa 7

Testes

Criar testes.

Validar comportamento.

---

## Etapa 8

Commit

Somente após:

Código

↓

Testes

↓

Documentação

↓

Atualizados.

---

# Checklist

Antes de iniciar qualquer implementação perguntar:

Esta funcionalidade já existe?

↓

Existe documentação?

↓

Existe DEC relacionada?

↓

Existe Template?

↓

Existe Factory?

↓

Existe Runtime?

↓

Existe Teste?

Se qualquer resposta for NÃO,

avaliar antes de programar.

---

# Atualização da documentação

Atualizar somente quando necessário.

## knowledge-base

Sempre que alterar regras do jogo.

---

## engineering

Sempre que surgir uma nova decisão arquitetural.

---

## vc-lab

Sempre que alterar a implementação planejada.

---

## developer-guide

Somente quando mudar o padrão de desenvolvimento.

Nunca por detalhes de gameplay.

---

# Commits

Todo Commit deve responder:

O que foi alterado?

Por quê?

Que documentos foram atualizados?

Existe nova DEC?

---

# Pull Request

Todo PR deve responder:

Qual problema resolve?

Que arquitetura respeita?

Que testes foram criados?

Que documentação foi alterada?

---

# Regra de Ouro

Nunca implementar primeiro.

Sempre:

Arquitetura

↓

Documentação

↓

Implementação

---

# Architecture Safety Check

Sempre que existir risco de perda de contexto devido às limitações do ambiente conversacional:

Interromper o roadmap.

↓

Criar documentação.

↓

Continuar.

Esta regra é obrigatória.