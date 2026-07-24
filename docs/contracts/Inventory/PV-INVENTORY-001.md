# PV-INVENTORY-001 — Sistema de Inventário

Status:

APROVADO PARA IMPLEMENTAÇÃO


## Objetivo

Implementar a primeira versão oficial do Sistema de Inventário do Project-Victory.

O objetivo é permitir que o personagem possua e gerencie itens recebidos através dos sistemas de gameplay.


## Contexto atual

Vertical Slice validado:

Account

↓

Authentication

↓

Character

↓

Enter Game

↓

Runtime

↓

Hunt

↓

Encounter

↓

Combat

↓

Reward

↓

Experience


Estado atual:

O personagem consegue receber recompensas.

Este contrato adiciona a capacidade de armazenar itens.


---

# Objetivo funcional

Criar o fluxo:


Combat concluído

↓

Reward gerada

↓

Item recebido

↓

Inventory atualizado

↓

Item disponível para consulta


---

# Princípios arquiteturais


## Item Definition

Representa a definição do item no jogo.


Exemplos:

- Sword Iron
- Health Potion


Responsabilidade:

Descrever o tipo do item.


---

## Item Instance

Representa uma ocorrência pertencente ao jogador.


Exemplo:

Personagem recebeu:

- Sword Iron
- Health Potion x3


Responsabilidade:

Representar posse.


---

# Escopo


## 1. Inventário do Character


O Inventário pertence ao Character.


Regra:

Inventory faz parte do estado permanente do personagem.


Não criar:

- Inventory como Aggregate Root independente;
- sistema separado de jogador.


---

## 2. Operações mínimas


O sistema deve permitir:


Adicionar item.


Consultar itens.


Remover item.


Validar quantidade quando aplicável.


---

## 3. Integração com Reward


A Reward deve poder entregar itens.


Fluxo:


Combat

↓

Reward

↓

Inventory


---

# Fora do escopo


Não implementar:


❌ Equipment

❌ Slots de equipamento

❌ Crafting

❌ Trading

❌ Marketplace

❌ Economia

❌ Durabilidade

❌ Enchant

❌ Drop Tables


Esses sistemas serão contratos futuros.


---

# Regras de implementação


Antes da implementação:


Consultar snapshot oficial.


Identificar:


- estrutura atual do Character;
- padrão de entidades/value objects;
- padrão de persistência;
- fluxo atual de Reward.


Não criar arquitetura paralela.


---

# Alterações permitidas


Podem ser necessários:


Domain:

- Item;
- Inventory;
- extensão do Character.


Application:

- comandos;
- handlers;
- integração Reward.


Persistence:

- armazenamento necessário.


Testes:

- validação do fluxo.


A Engenharia deve alterar somente o necessário.


---

# Critérios de aceite


## Domínio


Deve existir uma representação clara de:


Item

Inventory


seguindo os padrões atuais.


---

## Gameplay


Fluxo validado:


Criar Character

↓

Executar Gameplay

↓

Receber Reward

↓

Adicionar Item

↓

Consultar Inventory


---

## Testes


Adicionar validação automática:


Personagem recebe item.


Inventário contém item.


---

# Não alterar


Sem necessidade:


CombatResolver

Runtime

Hunt

Encounter


---

# Entrega da Engenharia


Informar:


1. Arquivos analisados.

2. Arquivos alterados.

3. Arquivos novos.

4. Código completo.

5. Build/test executado.

6. Resultado.


---

# Resultado esperado


Após este contrato:


O personagem possuirá estado de inventário persistente.


O Project-Victory terá a base necessária para:


Loot

↓

Equipment

↓

Progression


Fim do contrato.

PV-INVENTORY-001