# PV-RUNTIME-003-A

# Runtime Activity Model Foundation


## Objetivo

Formalizar o conceito de atividade atual do jogador dentro do Runtime.

O Runtime já possui capacidade de manter contexto de execução, porém a atividade atual ainda representa apenas uma informação simples.

Este contrato cria uma fundação oficial para representar o estado atual de atividade do jogador.


## Contexto atual

Estado existente:

RuntimeExecutionContext

↓

RuntimeGameplayContext

↓

CurrentActivity


Problema:

A atividade atual ainda não possui um modelo formal dentro da arquitetura do Runtime.


## Objetivo arquitetural

Criar uma representação oficial:

RuntimeActivity


Responsável por indicar o que o jogador está executando no momento.


## Atividades previstas

Inicialmente suportar:


Idle

Exploring

Traveling

Hunting

Combat

Reward

Dialogue


## Regra de implementação

RuntimeActivity não deve ser representado como texto livre.


Evitar:

"Idle"

"idle"

"Hunting"

"hunting"


A implementação deve utilizar uma representação tipada compatível com a arquitetura existente.


Exemplos possíveis:

- Enum
- Value Object


A decisão final deve respeitar o padrão existente no projeto.


## Responsabilidade do Runtime

Após este contrato, o Runtime deve responder:


Qual atividade o jogador está executando atualmente?


Exemplo:


Character

↓

Runtime

↓

CurrentActivity = Hunting


## Regras arquiteturais

Runtime representa:

Estado temporário da sessão


Não representa:

Estado permanente do Character


## Não faz parte deste contrato

Não implementar:


- Character Progression
- Combat State interno
- Inventory State
- Reward State
- Quest System
- NPC System


Esses sistemas continuam responsáveis pelos próprios estados.


## Arquivos impactados

A Engenharia deve analisar:


src/ProjectVictory.Application/Runtime/

src/ProjectVictory.Domain/

src/ProjectVictory.Persistence/Runtime/


A alteração deve respeitar a arquitetura existente.


## Critérios de aceitação

O contrato será considerado concluído quando:


Runtime

↓

possuir uma representação oficial de atividade atual

↓

permitir consulta consistente pelos sistemas de gameplay


Validar:

- atividade possui representação tipada;
- Runtime consegue armazenar atividade atual;
- sistemas conseguem consultar a atividade.


## Resultado esperado


Antes:


Runtime

=

Sessão

+

Hunt atual


Depois:


Runtime

=

Sessão

+

Atividade atual

+

Contexto preparado para evolução


## Restrições

Não alterar:


- Combat
- Hunt
- Inventory
- Character Aggregate
- Reward Pipeline


A implementação deve ser limitada ao contexto Runtime.


## Status


PV-RUNTIME-003-A

Runtime Activity Model Foundation


Aguardando implementação da Engenharia.