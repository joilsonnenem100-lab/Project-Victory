# PV-WORLD-004

# Player Location Binding


## Objetivo

Criar a ligação oficial entre o Runtime do jogador e uma localização válida existente no mundo do Project-Victory.

Este contrato garante que o contexto atual do jogador utilize referências reais de mundo.


## Contexto atual

Após os contratos anteriores:

PV-WORLD-001

World Definition Foundation


↓

PV-WORLD-002

Region Foundation


↓

PV-WORLD-003

Location Definition Foundation


O sistema possui:

WorldDefinition

↓

RegionDefinition

↓

LocationDefinition


Porém ainda falta validar a relação:

Runtime

↓

LocationDefinition


## Problema atual

O Runtime possui:

RuntimeWorldContext

↓

LocationId


Mas o sistema ainda não garante que:

- o LocationId existe;
- pertence à Region correta;
- pertence ao World correto.


## Objetivo arquitetural

Criar a validação oficial de localização do jogador.


Fluxo esperado:

Runtime

↓

WorldContext

↓

LocationId

↓

LocationDefinition


## Responsabilidade

Este contrato deve garantir:

- jogador sempre está em uma localização válida;
- localização pertence ao mundo correto;
- localização pertence à região correta.


## Regras arquiteturais

O Runtime mantém:

Estado atual da sessão.


O World System mantém:

Definições válidas do mundo.


A validação deve ocorrer entre essas camadas.


## Não faz parte deste contrato

Não implementar:

- Movimento do jogador
- Sistema de exploração
- Viagem
- Teleporte
- Spawn
- Encontros
- NPC
- Quests
- Eventos


## Critérios de aceitação

O contrato será considerado concluído quando:

Runtime

↓

possuir localização válida


Validar:

- LocationId existe;
- Location pertence à Region correta;
- Region pertence ao World correto;
- Runtime mantém referência consistente.


## Restrições

Não alterar:

- Character Aggregate
- Combat
- Hunt
- Inventory
- Reward Pipeline
- Runtime Recovery existente


## Status

PV-WORLD-004

Player Location Binding

Aguardando implementação da Engenharia.
