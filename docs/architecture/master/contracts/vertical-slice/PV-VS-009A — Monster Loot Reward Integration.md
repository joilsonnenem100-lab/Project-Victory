# PV-VS-009A — Monster Loot Reward Integration

## Local recomendado

docs/architecture/master/contracts/vertical-slice/


# Objetivo

Adicionar a primeira integração de Loot no ciclo de recompensa existente.

O objetivo é permitir que monstros derrotados possam gerar:

- Experience;
- Gold;
- Item Drop.

Este contrato não implementa armazenamento em inventário.


# Estado Atual Validado

Fluxo atual:

Monster Defeated

↓

Combat Result

↓

Reward

↓

Experience Gain


# Novo Fluxo Esperado

Monster Defeated

↓

Combat Result

↓

Reward Generation

↓

Experience

↓

Gold

↓

Item Drop


# Regra de Implementação

Antes de qualquer alteração:

A Engenharia deve realizar análise de impacto.

Obrigatório informar:

- arquivos que serão alterados;
- dependências encontradas;
- fluxos afetados;
- riscos de regressão;
- estratégia de preservação.


Nenhuma implementação deve iniciar sem essa análise.


# Escopo


## 1. Loot Definition

Adicionar suporte para definição de drops de monstros.

Exemplo:

Monster:

Rat

Reward:

Experience:
50

Gold:
1-5

Drop:

Rat Tail

Chance:
Configurável


---

## 2. Integração com Reward

Após vitória do combate:

O sistema de Reward deve suportar:

- Experience;
- Gold;
- Item Drop quando aplicável.


O fluxo existente de Experience deve continuar funcionando.


---

## 3. Apresentação no Client

O Client deve conseguir receber e apresentar a recompensa gerada.

Exemplo:

Reward:

+50 XP

+3 Gold

+Rat Tail


---

# Fora de Escopo

Não implementar:

- Inventory funcional;
- Item Instance avançado;
- Equipment;
- Affixes;
- Raridade;
- Crafting;
- Loot Tables avançadas.


---

# Restrições Arquiteturais

Não alterar sem necessidade:

- Character Aggregate;
- Combat Resolver;
- Hunt Flow;
- Progression System.


Preservar todos os fluxos já validados.


---

# Critérios de Aceitação


## Gameplay

O contrato será considerado concluído quando:

- Rat continuar sendo derrotado normalmente;
- XP continuar sendo aplicado;
- Gold puder ser gerado;
- Item Drop puder ocorrer baseado em chance.


## Regressão

Continuam funcionando:

- Login;
- Character Selection;
- Enter World;
- Hunt;
- Combat;
- Experience Progression.


---

# Entrega da Engenharia

Retornar:

- arquivos alterados;
- análise de impacto;
- motivo de cada alteração;
- build executado;
- validação funcional;
- evidências.


---

# Dependências

- Snapshot oficial Project-Victory atual;
- PV-VS-006 Combat Loop validado;
- Reward System atual.