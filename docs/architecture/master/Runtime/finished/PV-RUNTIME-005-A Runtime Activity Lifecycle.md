# PV-RUNTIME-005-A

# Runtime Activity Lifecycle


## Objetivo

Criar um ciclo de vida controlado para as atividades executadas pelo jogador dentro do Runtime.

O objetivo é impedir que o Runtime aceite mudanças de atividade sem regras e garantir que a sessão do jogador permaneça em um estado consistente.


## Contexto atual

Após os contratos anteriores:

PV-RUNTIME-003-A

↓

Runtime possui atividade atual


PV-RUNTIME-005

↓

Runtime possui estado temporário de gameplay


Agora é necessário controlar como essas atividades mudam.


## Problema atual

Sem um ciclo de vida definido:

```
CurrentActivity = qualquer valor
```

pode gerar estados inválidos.


Exemplo:

```
Combat

+

Traveling

ao mesmo tempo
```


Ou:

```
Hunting

↓

Exploring

sem finalizar o contexto anterior
```


## Objetivo arquitetural

Criar regras oficiais de transição de atividade.


Modelo esperado:

```
RuntimeActivityLifecycle


Estado atual

↓

Validação da transição

↓

Novo estado
```


## Exemplo de fluxo


```
Idle

↓

Exploring

↓

Hunting

↓

Combat

↓

Reward

↓

Exploring
```


## Responsabilidade

O Runtime deve controlar:


- entrada em uma atividade;
- saída de uma atividade;
- transições permitidas;
- transições inválidas.


## Regras arquiteturais


A mudança de atividade deve passar pelo Runtime.


Sistemas externos não devem alterar diretamente:


```
CurrentActivity
```


Exemplo proibido:


```
Hunt

↓

Runtime.CurrentActivity = Combat
```


A alteração deve seguir o fluxo oficial do Runtime.


## Não faz parte deste contrato


Não implementar:


- Sistema de Combat
- Sistema de Hunt
- Sistema de Travel
- Sistema de Quest
- Sistema de Eventos
- Regras específicas de gameplay


Este contrato cria somente o controle do ciclo de vida das atividades.


## Escopo de implementação


A Engenharia deve analisar:


```
src/ProjectVictory.Application/Runtime/

src/ProjectVictory.Domain/

src/ProjectVictory.Persistence/Runtime/
```


Implementar somente os componentes necessários para controlar transições de atividade.


## Critérios de aceitação


O contrato será considerado concluído quando:


Runtime


↓

possuir regras de transição de atividade


Validar:


- atividade atual é conhecida;
- transições inválidas são bloqueadas;
- mudança de atividade mantém consistência do Runtime.


## Resultado esperado


Antes:


```
Runtime

=

CurrentActivity simples
```


Depois:


```
Runtime

=

CurrentActivity

+

Regras de transição

+

Ciclo de vida controlado
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


PV-RUNTIME-005-A

Runtime Activity Lifecycle


Aguardando implementação da Engenharia.