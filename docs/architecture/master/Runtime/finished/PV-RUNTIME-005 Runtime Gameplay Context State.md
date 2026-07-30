# PV-RUNTIME-005

# Runtime Gameplay Context State


## Objetivo

Criar uma separação oficial entre o estado permanente do Character e o estado temporário da sessão do jogador armazenado no Runtime.

O objetivo é garantir que informações temporárias de gameplay não sejam adicionadas ao Aggregate Character.


## Contexto atual

O Project-Victory possui uma separação entre:


Character

=

Estado permanente do jogador


Runtime

=

Estado temporário da sessão


Porém essa separação precisa ser formalizada para suportar novos sistemas de gameplay.


## Modelo arquitetural


Character State:

- Level
- Experience
- Inventory
- Stats
- Progressão permanente


Runtime State:

- Atividade atual
- Hunt atual
- Encounter atual
- Eventos temporários
- Estados de sessão


## Objetivo arquitetural

O Runtime deve representar:

O estado atual da sessão do jogador.


Exemplo:


Character

↓

Runtime

↓

Gameplay State


Onde:


Character:

Level 10

Experience 5000

Inventory


Runtime:

Hunting

Current Hunt

Current Encounter

Temporary State


## Responsabilidade

O Runtime deve armazenar informações que:

- existem somente durante a sessão;
- representam contexto atual;
- podem mudar frequentemente;
- não fazem parte da identidade permanente do Character.


## Regras arquiteturais


Estado permanente pertence ao:

Character Aggregate


Estado temporário pertence ao:

Runtime


Nenhum sistema deve mover dados permanentes para Runtime.


Nenhum sistema deve colocar estado temporário dentro do Character.


## Não faz parte deste contrato


Não implementar:


- Sistema de Combat completo
- Sistema de Inventory
- Progression
- Experience
- Equipment
- Economia
- Quest System


Esses sistemas permanecem independentes.


## Escopo de implementação


A Engenharia deve analisar:


src/ProjectVictory.Application/Runtime/


src/ProjectVictory.Domain/


src/ProjectVictory.Persistence/Runtime/


Implementar somente a fundação necessária para representar Gameplay State no Runtime.


## Critérios de aceitação


O contrato será considerado concluído quando:


Runtime

↓

possuir estrutura oficial para estado temporário


E permitir:


- armazenar contexto atual;
- consultar estado da sessão;
- manter separação com Character.


## Resultado esperado


Antes:


Character

+

Runtime simples


Depois:


Character

=

Estado permanente


Runtime

=

Estado temporário da sessão


## Restrições


Não alterar:


- Combat
- Hunt
- Inventory
- Reward Pipeline
- Character Aggregate


Alterações devem permanecer dentro da evolução do Runtime.


## Status


PV-RUNTIME-005

Runtime Gameplay Context State


Aguardando implementação da Engenharia.