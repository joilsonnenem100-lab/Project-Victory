# EXP-VS-205 — Player State Presentation

## Tipo

Client Experience Contract

---

# Referência arquitetural

Este contrato pertence ao:

PV-MILESTONE-002 — Player Experience Vertical Slice

---

# Dependências

Este contrato depende de:

EXP-VS-200 — Client Foundation

↓

EXP-VS-203 — Game Entry Experience

↓

EXP-VS-204 — First World Scene Foundation

---

# Objetivo

Definir a apresentação visual do estado atual do jogador dentro do Client.

O objetivo é permitir que o jogador visualize informações básicas do seu Character utilizando exclusivamente dados oficiais fornecidos pelo Backend.

A responsabilidade do Client é somente apresentar o estado recebido.

O Client não possui autoridade sobre qualquer informação de gameplay.

---

# Escopo

Este contrato define:

- dados apresentados ao jogador;
- origem dos dados;
- fluxo de consulta;
- responsabilidade do HUD;
- estados visuais;
- critérios de aceite.

---

# Fluxo arquitetural

O fluxo oficial será:

ProjectVictory.Client

↓

HTTP REST

↓

ProjectVictory.API

↓

Application

↓

Domain

↓

Response DTO

↓

Player State Presentation

---

# Dados apresentados ao jogador

O HUD inicial deve apresentar somente informações básicas do Character.

Dados obrigatórios:

Character Name

Level

Experience

---

# Modelo de dados esperado

Exemplo:

{
  "characterId": "guid",
  "name": "Knight",
  "level": 1,
  "experience": 0
}

---

# Origem dos dados

Os dados apresentados pelo Client devem possuir origem oficial no Backend.

Fluxo:

Character State

↓

Application Query

↓

API Response

↓

Client HUD

---

# Responsabilidade do Client

O Client será responsável por:

✅ Consumir os dados disponibilizados pela API

✅ Apresentar informações visuais

✅ Atualizar o HUD conforme novos dados recebidos

✅ Integrar a apresentação com a World Scene

---

# Responsabilidade do Backend

O Backend será responsável por:

✅ Manter o estado oficial do Character

✅ Fornecer dados válidos

✅ Aplicar regras de domínio

✅ Controlar persistência

✅ Controlar Progression

---

# O Client não deve possuir

❌ Regras de Level

❌ Regras de Experience

❌ Cálculo de Progression

❌ Alteração de estado do Character

❌ Persistência local como fonte oficial

❌ Regras de domínio

---

# Responsabilidade do HUD

O HUD será responsável por:

✅ Apresentar nome do Character

✅ Apresentar Level atual

✅ Apresentar Experience atual

✅ Atualizar informações recebidas

✅ Informar estados visuais ao jogador

---

# O HUD não deve:

❌ Calcular valores

❌ Alterar valores recebidos

❌ Criar Progression

❌ Simular estado do servidor

❌ Aplicar regras de jogo

---

# Estados visuais

## Loading

Estado apresentado enquanto:

Player State está sendo carregado.

Exemplo:

Carregando informações do jogador...

---

## Ready

Estado apresentado quando:

Dados recebidos com sucesso.

HUD atualizado.

---

## Error

Estado apresentado quando:

Falha de comunicação.

ou

Dados indisponíveis.

---

# Atualização do estado

O fluxo permitido é:

Backend

↓

Novo estado do Character

↓

API Response

↓

Client

↓

Atualização visual

---

O Client não deve:

Criar valores temporários.

Modificar dados recebidos.

Assumir valores de Progression.

---

# Limites do primeiro Vertical Slice

Permitido:

✅ Nome do Character

✅ Level

✅ Experience

✅ Atualização visual básica

---

Não incluir:

❌ Inventário

❌ Equipamentos

❌ Skills

❌ Buffs

❌ Debuffs

❌ Status avançados

❌ Sistemas futuros

---

# Critérios de aceite

O contrato será considerado atendido quando:

✅ Player State é carregado após Game Entry.

✅ HUD apresenta dados reais do Character.

✅ Dados apresentados correspondem ao Backend.

✅ Client não calcula valores.

✅ Client não altera estado do jogador.

✅ Estados Loading e Error estão definidos.

✅ Separação Client/API/Application/Domain permanece preservada.

---

# Restrições

Não criar:

❌ Novo sistema de Character

❌ Novo sistema de Progression

❌ Novo armazenamento de Player State

❌ Regras de gameplay no Client

---

# Status

EXP-VS-205 — Player State Presentation

🟢 Liberado para implementação após aprovação arquitetural.