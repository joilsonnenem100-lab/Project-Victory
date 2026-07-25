PROJECT-VICTORY

PV-RUNTIME-001 - Runtime Creation on Game Entry


Status:

Aprovado para implementação


Objetivo:

Integrar a criação do RuntimeExecutionContext ao fluxo oficial de entrada no jogo.

O objetivo é garantir que todo Character que entra no mundo possua um Runtime ativo antes de executar ações de gameplay.


Contexto:

Durante a validação funcional manual do Vertical Slice foi identificado que:

O jogador consegue:

Login

↓

Character

↓

World Entry


Porém ao iniciar Hunt ocorre falha porque:

RuntimeId não existe.


Erro encontrado:

RuntimeId is required.


Causa identificada:

O EnterGameHandler atualmente carrega o Character e retorna o resultado de entrada no mundo, porém não cria RuntimeExecutionContext.


Fluxo atual:

Login

↓

Character

↓

EnterGameHandler

↓

EnterGameResult

↓

World

↓

Hunt

↓

Erro RuntimeId ausente


Fluxo desejado:

Login

↓

Character

↓

EnterGameHandler

↓

CreateRuntimeExecutionHandler

↓

RuntimeExecutionContext

↓

RuntimeId

↓

World

↓

Hunt

↓

Combat


Escopo autorizado:


1. Integração do Runtime no EnterGame


Alterar o fluxo de entrada para criar RuntimeExecutionContext quando o jogador entrar no mundo.


O Runtime deve utilizar a infraestrutura existente:

CreateRuntimeExecutionHandler

RuntimeExecutionStateStore

CharacterRuntimeState


2. Retorno do RuntimeId


O resultado da entrada no jogo deve disponibilizar o RuntimeId criado para o fluxo de gameplay.


A forma exata deve seguir o padrão existente do projeto.


3. Dependency Injection


Caso necessário:

Registrar dependências adicionais para permitir que EnterGameHandler utilize o fluxo de criação de Runtime.


Utilizar o padrão existente do projeto.


Arquivos a analisar:


src/ProjectVictory.Application/Game/EnterGameHandler.cs


src/ProjectVictory.Application/Game/EnterGameResult.cs


src/ProjectVictory.Application/Runtime/CreateRuntimeExecutionHandler.cs


src/ProjectVictory.Application/Runtime/CreateRuntimeExecutionCommand.cs


src/ProjectVictory.API/Controllers/RuntimeController.cs


Restrições:


Não alterar:

Hunt

Combat

Reward

RuntimeExecutionStateStore

CharacterRuntimeState


Não criar:

Novo sistema de sessão

RuntimeManager

RuntimePersistence

Novo fluxo paralelo de Runtime


Estratégia:


Reutilizar o Runtime existente.


O fluxo deve ser:

Entrada no jogo

↓

Criação do Runtime

↓

Disponibilização do RuntimeId

↓

Gameplay


Critérios de aceite:


Entrada no jogo:


O jogador entra normalmente no mundo.


Runtime:


Um RuntimeExecutionContext é criado.


RuntimeId:


Um RuntimeId válido é retornado.


Hunt:


Após entrar no mundo, iniciar Hunt não deve exigir criação manual de Runtime.


Smoke Test:


PV-TOOLS-001B deve continuar funcionando.


Teste manual:


O fluxo:

Login

Character

World

Hunt

Combat

Reward

Experience


deve executar sem erro de RuntimeId.


Entrega esperada da Engenharia:


Retornar:

1. Arquivos alterados.

2. Explicação da integração realizada.

3. Build executado.

4. Validação manual ou automatizada.

5. Impactos futuros identificados.


Fim do contrato.

PV-RUNTIME-001

Objetivo final:

Garantir que o RuntimeExecutionContext faça parte do ciclo real de entrada do jogador no mundo.