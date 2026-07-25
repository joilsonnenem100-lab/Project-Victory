# PROJECT-VICTORY-CONTEXT-HANDOFF
## Documento de Entrada Oficial

Este é o primeiro documento que deve ser consultado ao iniciar uma nova sessão.

Ordem de leitura:

1. PROJECT-VICTORY-CONTEXT-HANDOFF.md
2. PROJECT-VICTORY-GAME-PHILOSOPHY.md
3. PROJECT-VICTORY-ENGINEERING-HANDOFF.md
4. Documentação específica da tarefa

## Identidade do Projeto

Projeto:

Project-Victory

Tipo:

Jogo online inspirado em MMORPG, com foco em progressão, hunts automáticas, combate, personalização e evolução contínua.

Este documento representa o contexto oficial para continuidade do projeto entre sessões.

O projeto não deve ser tratado como iniciado do zero.

---


# Papel da IA no Projeto

A IA participou desde a fundação do Project-Victory, auxiliando na criação da arquitetura, organização dos processos de engenharia, definição dos contratos e condução técnica do desenvolvimento.

A continuidade deve preservar:

- Decisões arquiteturais.
- Contratos existentes.
- Regras de desenvolvimento.
- Roadmap.
- Princípios definidos.

---

# Funções Exercidas pela IA

## Arquiteto de Software

Responsabilidades:

- Definição da arquitetura inicial.
- Separação das responsabilidades entre camadas.
- Avaliação de modelos de domínio.
- Proteção contra acoplamento indevido.
- Avaliação de decisões técnicas.

Princípio:

Domínio não conhece infraestrutura.

Infraestrutura serve ao domínio.

Aplicação coordena casos de uso.

---

## Technical Director

Responsabilidades:

- Avaliar propostas da Engenharia.
- Autorizar ciclos de implementação.
- Bloquear alterações fora de escopo.
- Garantir alinhamento entre produto e código.

Regra:

Arquitetura define.

Engenharia implementa.

Validação confirma.

---

## Gestor de Contratos

Responsável pelo processo:

Contrato

↓

Implementação

↓

Teste

↓

Validação

↓

Aceite

Nenhuma implementação deve iniciar sem contrato ou autorização equivalente.

---

## Guardião de Escopo

Responsável por evitar expansão prematura.

Decisões preservadas:

- Combat não virou API pública sem contrato específico.
- Item não virou Inventory.
- Monster não foi misturado com Encounter.
- Progressão não foi antecipada.
- Economia não foi criada antes da fundação.
- Sistemas futuros não foram adicionados fora do roadmap.

---

# Organização das Equipes

## Arquitetura

Responsável por:

- Decisões técnicas.
- Contratos.
- Roadmap.
- Aprovações.

---

## Engenharia

Responsável por:

- Implementação.
- Alteração de código.
- Testes.
- Entregas.

---

# Fluxo Oficial do Projeto

Arquitetura

↓

Contrato

↓

Engenharia

↓

Implementação

↓

Build

↓

Teste

↓

Validação

↓

Aceite

---

# Vertical Slice 2.0

Status:

CONCLUÍDO

Objetivo:

Criar a primeira fatia funcional completa do Project-Victory.

---

# Ciclos Concluídos

## PV-VS-2.0-008 — Account Persistence Foundation

Resultado:

- Conta persistente.
- EF Core.
- SQLite.
- Recuperação após reinicialização.

---

## PV-VS-2.0-009 — Authentication Flow

Resultado:

- Login funcional.
- JWT.
- Contexto autenticado.
- Tratamento de credenciais inválidas.

---

## PV-VS-2.0-010 — Character Persistence Foundation

Resultado:

- Personagem persistente.
- Recuperação após reinicialização.

---

## PV-VS-2.0-011 — Hunt Runtime Flow

Resultado:

- Hunt executável.
- Estado criado.
- Resultado retornado.

---

## PV-VS-2.0-012 — Monster Data Foundation

Resultado:

- Monster criado.
- Persistência implementada.
- Dados básicos disponíveis.

---

## PV-VS-2.0-013 — Combat Resolution

Resultado:

- CombatContext criado.
- Resolução de combate.
- Resultado gerado.

---

## PV-VS-2.0-014 — Item Definition

Resultado:

- ItemDefinition.
- ItemCharacteristics.
- Persistência.
- Apresentação funcional.

---

## PV-VS-2.0-015 — Player Loop Validation

Fluxo validado:

Account

↓

Authentication

↓

Character

↓

Game Entry

↓

Hunt

↓

Monster

↓

Combat

↓

Result

---

## PV-VS-2.0-044 — Persistence Integration Validation

Resultado:

Persistência integrada validada.

Confirmado:

- Account.
- Character.
- Monster.
- Item.
- Player Loop após reinicialização.

---

## PV-VS-2.0-045 — Final Acceptance

Resultado:

Vertical Slice 2.0

APROVADO

---

# Estado Atual do Projeto

Após o Vertical Slice 2.0:

O projeto possui:

Conta funcional

↓

Login funcional

↓

Personagem funcional

↓

Entrada no jogo

↓

Hunt funcional

↓

Monster funcional

↓

Combat funcional

↓

Persistência funcional

---

# Update Atual

## PV-VS-2.0-UPDATE-001 — Web Entry Experience Foundation

Objetivo:

Permitir acesso real pelo navegador.

---

## Step 1

Status:

CONCLUÍDO

Resultado:

- ProjectVictory.Client encontrado.
- React + Vite funcionando.
- Browser acessando localhost.
- Primeira experiência web carregando.

Observação:

Existe uma tela visual de Login preparada no frontend.

Essa tela não deve ser removida.

Ela será utilizada em ciclos futuros.

---

# Ciclo Atual

## PV-VS-2.0-UPDATE-002 — Web Account Registration Flow

Status:

ANÁLISE CONCLUÍDA

IMPLEMENTAÇÃO PENDENTE

Objetivo:

Permitir criação de conta pelo navegador.

Fluxo:

Usuário

↓

Browser

↓

Tela de Cadastro

↓

POST /api/account

↓

CreateAccountHandler

↓

Account Persistence

↓

Conta criada

---

# Decisões do UPDATE-002

Não criar novo endpoint.

Utilizar:

POST /api/account

Endpoint existente.

---

# Escopo Autorizado

Criar:

src/ProjectVictory.Client/src/pages/RegisterPage.tsx

src/ProjectVictory.Client/src/services/accountService.ts

Alterar:

src/ProjectVictory.Client/src/App.tsx

---

# Fora do Escopo do UPDATE-002

Não implementar:

- JWT.
- Login funcional completo.
- Sessão.
- Character.
- Game Entry.
- Gameplay.

---

# Próximo Passo Exato

Continuar:

PV-VS-2.0-UPDATE-002

Web Account Registration Implementation

Fluxo:

Contrato

↓

Implementação frontend

↓

Teste criação pelo navegador

↓

Validação

---

# Regras para Continuidade

Sempre:

- Ler este documento antes de decisões.
- Não criar sistemas sem contrato.
- Não alterar arquitetura sem decisão.
- Não assumir funcionalidades futuras.
- Preservar decisões anteriores.

Prioridade:

Estabilidade

↓

Experiência do usuário

↓

Expansão de gameplay