# PV-RUNTIME-006

# Runtime Context Aggregation


## Objetivo

Consolidar o Runtime como a camada oficial responsável pelo contexto temporário de execução do jogador.

O objetivo deste contrato é reunir as evoluções anteriores em uma estrutura única de contexto, permitindo que sistemas de gameplay consultem o estado atual da sessão através do Runtime.


## Contexto atual

Após os contratos anteriores:


PV-RUNTIME-001

↓

Runtime Creation


PV-RUNTIME-002

↓

Runtime Persistence


PV-RUNTIME-002-A

↓

Runtime Recovery


PV-RUNTIME-003-Hunt

↓

Runtime integrado com Hunt


PV-RUNTIME-003-A

↓

Modelo de atividade


PV-RUNTIME-004

↓

Contexto de mundo


PV-RUNTIME-005

↓

Estado temporário de gameplay


Agora o Runtime possui diferentes partes de contexto que precisam ser organizadas.


## Objetivo arquitetural

Criar um contexto consolidado:


```
RuntimeExecutionContext
```


Responsável por representar a sessão atual do jogador.


## Modelo esperado


```
RuntimeExecutionContext


├── CharacterId

├── WorldContext

├── Activity

├── GameplayState

├── CurrentHunt

└── CurrentEncounter
```


## Responsabilidade


O Runtime deve responder:


```
Qual é o contexto atual do jogador?
```


Exemplo:


```
Character

↓

Runtime

↓

World:
FirstWorld


Location:
DarkForest


Activity:
Hunting


Hunt:
Rat Hunt


Encounter:
Active
```


## Regras arquiteturais


Runtime representa:


```
Estado temporário da sessão
```


Character representa:


```
Estado permanente do jogador
```


O Runtime complementa o Character Aggregate.


O Runtime não substitui:

```
Character Aggregate
```


## Não faz parte deste contrato


Não implementar:


- Novo sistema de mundo
- Sistema de combate
- Sistema de inventário
- Sistema de quests
- Sistema de NPC
- Economia
- Progressão


Este contrato somente organiza o contexto existente.


## Escopo de implementação


A Engenharia deve analisar:


```
src/ProjectVictory.Application/Runtime/

src/ProjectVictory.Domain/

src/ProjectVictory.Persistence/Runtime/
```


Implementar somente a agregação do contexto Runtime.


## Critérios de aceitação


O contrato será considerado concluído quando:


```
RuntimeExecutionContext

↓

representar o estado atual completo da sessão
```


Validar:


- Runtime possui CharacterId;
- Runtime possui contexto de atividade;
- Runtime possui contexto de mundo;
- Runtime possui estado temporário;
- Runtime mantém integração com sistemas existentes.


## Resultado esperado


Antes:


```
Runtime

=

Sessão

+

Hunt atual
```


Depois:


```
Runtime

=

Sessão

+

Atividade

+

Mundo

+

Localização

+

Gameplay State

+

Hunt

+

Encounter
```


## Restrições


Não alterar:


- Combat
- Hunt
- Inventory
- Reward Pipeline
- Character Aggregate


Alterações devem permanecer dentro da evolução do Runtime.


## Status


PV-RUNTIME-006

Runtime Context Aggregation


Aguardando implementação da Engenharia.