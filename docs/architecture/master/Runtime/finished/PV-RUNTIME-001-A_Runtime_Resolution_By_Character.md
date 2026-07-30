# PV-RUNTIME-001-A Runtime Resolution By Character

## Objetivo
Permitir localizar o Runtime ativo usando CharacterId.

## Fluxo
CharacterId
↓
RuntimeExecutionContext
↓
RuntimeId
↓
Gameplay

## Regras
RuntimeExecutionContext continua como fonte oficial.
Não criar RuntimeManager ou sistema paralelo.

## Aceitação
Character entra no jogo e o Runtime ativo pode ser recuperado por CharacterId.
