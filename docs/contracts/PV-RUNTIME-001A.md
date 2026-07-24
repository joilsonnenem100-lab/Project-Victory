PROJECT-VICTORY

PV-RUNTIME-001A - Runtime Lookup by Character


Status:

Aprovado para implementação


Objetivo:

Complementar a infraestrutura do RuntimeExecutionStateStore para permitir recuperar o Runtime ativo de um Character sem exigir que o Client envie RuntimeId.


Contexto:

O PV-RUNTIME-001 definiu que o RuntimeExecutionContext deve ser criado durante o fluxo de entrada no mundo.

Fluxo aprovado:

EnterGame

↓

RuntimeExecutionContext

↓

Gameplay


Durante a implementação foi identificado que o armazenamento atual permite:

RuntimeId

↓

RuntimeExecutionContext


Porém o fluxo de gameplay necessita localizar o Runtime usando:

CharacterId


Problema atual:

RuntimeExecutionStateStore possui apenas:

Get(Guid runtimeId)


Não existe:

GetByCharacterId(Guid characterId)


Resultado:

ExecuteHuntHandler não consegue recuperar o Runtime ativo sem receber RuntimeId externamente.


Escopo:


Implementar capacidade de consulta do Runtime ativo por CharacterId.


Alterações autorizadas:


Adicionar ao RuntimeExecutionStateStore uma forma oficial de localizar:

CharacterId

↓

RuntimeExecutionContext


A implementação deve preservar o comportamento existente.


Manter:

Get(Guid runtimeId)

Store(RuntimeExecutionContext)


Adicionar somente a capacidade necessária.


Arquivos a analisar:


src/ProjectVictory.Application/VerticalSlice/RuntimeExecutionStateStore.cs


src/ProjectVictory.Application/VerticalSlice/IRuntimeExecutionStateStore.cs


Possíveis consumidores:


src/ProjectVictory.Application/Hunt/ExecuteHuntHandler.cs


Restrições:


Não criar:

RuntimeManager

RuntimePersistence

Novo sistema de sessão


Não alterar:

CharacterRuntimeState

RuntimeExecutionContext

Hunt

Combat


Estratégia esperada:


Fluxo futuro:

EnterGame

↓

CreateRuntimeExecutionHandler

↓

RuntimeExecutionContext

↓

RuntimeExecutionStateStore

↓

CharacterId lookup

↓

RuntimeId recuperado

↓

Hunt/Combat


Critérios de aceite:


Runtime:

✅ Runtime continua sendo criado pelo EnterGame.


Store:

✅ Continua permitindo Get por RuntimeId.

✅ Permite recuperar Runtime por CharacterId.


Gameplay:

✅ Client não precisa enviar RuntimeId para iniciar Hunt.


Smoke Test:

✅ PV-TOOLS-001B continua aprovado.


Entrega esperada:


- Arquivos alterados.
- Explicação da solução.
- Build executado.
- Validação realizada.
- Impactos futuros identificados.


Fim do contrato.

PV-RUNTIME-001A