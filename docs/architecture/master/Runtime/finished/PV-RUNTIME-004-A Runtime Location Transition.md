# PV-RUNTIME-004-A

# Runtime Location Transition


## Objetivo

Criar uma fundação para controlar mudanças de localização do jogador através do Runtime.

O objetivo é evitar alterações diretas de localização sem validação e permitir que futuras transições de mundo sejam controladas pelo contexto do Runtime.


## Contexto atual

Após PV-RUNTIME-004:

Runtime

↓

RuntimeWorldContext

↓

WorldId

↓

RegionId

↓

LocationId


O Runtime passa a conhecer a localização atual do jogador.


O próximo passo é controlar como essa localização muda.


## Problema atual

Sem uma camada de transição, sistemas podem alterar localização diretamente:

Runtime

↓

Location = novo valor


Isso cria risco de:

- estados inválidos;
- mudanças sem validação;
- sistemas externos alterando contexto do jogador.


## Objetivo arquitetural

Criar um fluxo oficial para mudança de localização.


Modelo esperado:

Solicitação de mudança

↓

Runtime valida transição

↓

Novo Location Context

↓

Atualização do Runtime


## Responsabilidade

O Runtime deve controlar:

- entrada em uma nova localização;
- saída da localização atual;
- mudança de contexto mundial.


Exemplo:

Localização atual:

ForestEntrance


Solicitação:

DarkForest


Fluxo:

ForestEntrance

↓

Runtime valida

↓

DarkForest


## Regras arquiteturais

Toda mudança de localização deve passar pelo Runtime.


Sistemas de gameplay devem solicitar alteração.

Eles não devem alterar diretamente:

- LocationId
- RegionId
- WorldId


## Não faz parte deste contrato

Não implementar:

- Sistema de mapa
- Pathfinding
- Movimento visual
- Controle de personagem
- Teleporte
- Viagem completa


Este contrato cria somente o controle da transição de contexto.


## Escopo de implementação

A Engenharia deve analisar:

src/ProjectVictory.Application/Runtime/

src/ProjectVictory.Domain/

src/ProjectVictory.Persistence/Runtime/


Implementar somente os componentes necessários para controlar mudança de localização.


## Critérios de aceitação

O contrato será considerado concluído quando:


Runtime

↓

receber solicitação de mudança

↓

validar transição

↓

atualizar novo contexto de localização


Validar:

- localização atual permanece consistente;
- mudança passa pelo Runtime;
- novo contexto pode ser recuperado.


## Resultado esperado


Antes:

Runtime

=
Localização alterada diretamente


Depois:

Runtime

=
Controlador oficial de transição de localização


## Restrições

Não alterar:

- Combat
- Hunt
- Inventory
- Reward Pipeline
- Character Aggregate


Alterações devem permanecer dentro da evolução do Runtime.


## Status

PV-RUNTIME-004-A

Runtime Location Transition


Aguardando implementação da Engenharia.