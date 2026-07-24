PROJECT-VICTORY

PV-COMBAT-INFRA-001 - Combat Infrastructure Foundation


Status:

Proposto para implementação


Objetivo:

Completar a infraestrutura mínima necessária do módulo Combat para permitir que o fluxo real do jogo seja executado pelo PV-TOOLS-001B - Core Gameplay Flow Test.


Contexto:

Durante a execução do PV-TOOLS-001B - Core Gameplay Flow Test foi identificada uma lacuna de infraestrutura.

O fluxo esperado:

CombatContext

↓

Combat Handler

↓

Combat Resolution

↓

Reward


não consegue ser executado pelo Smoke Test porque existe uma dependência sem implementação concreta.


Encontrado:

Existe:

ICombatContextRepository


Porém:

Não foi encontrada implementação concreta registrada na infraestrutura.


Objetivo do contrato:

Implementar somente a infraestrutura necessária para que o Combat existente possa executar seu fluxo atual.

Este contrato não cria um novo sistema de combate.


Escopo autorizado:


1. Implementação do Repository de Combat


Criar a implementação concreta de:

ICombatContextRepository


Local esperado:

src/ProjectVictory.Persistence/Combat/CombatContextRepository.cs


A implementação deve seguir o padrão utilizado pelos repositórios existentes do projeto.


Referências obrigatórias:

src/ProjectVictory.Persistence/Character/CharacterRepository.cs

src/ProjectVictory.Persistence/Hunt/HuntRepository.cs


2. Registro no Dependency Injection


Registrar:

ICombatContextRepository

utilizando o padrão existente de configuração da aplicação.


3. Garantir resolução pelo fluxo atual


Após implementação:

CreateCombatContextHandler

↓

ICombatContextRepository

↓

CombatContextRepository


deve ser resolvido corretamente pelo DI.


Arquivos que devem ser analisados antes da implementação:


src/ProjectVictory.Application/Combat/ICombatContextRepository.cs


src/ProjectVictory.Persistence/Character/CharacterRepository.cs


src/ProjectVictory.Persistence/Hunt/HuntRepository.cs


Arquivo de registro DI:

src/ProjectVictory.API/Program.cs

ou equivalente encontrado no snapshot.


Dependências afetadas:


Possíveis áreas:


ProjectVictory.Persistence


ProjectVictory.API


ProjectVictory.Application.Combat



Não alterar:


CombatResolver

CombatResult

CombatContext

Damage System

Reward

Hunt

Runtime

Character

Progression



Restrições:


Não criar:

Novo Combat System

CombatManager

CombatService paralelo

Novo Aggregate

Nova regra de dano

Persistência permanente de combate sem necessidade


Estratégia:


Seguir o padrão existente:


Repository existente

↓

Implementação equivalente

↓

Registro DI

↓

Build

↓

Validação do Smoke Test



Critérios de aceite:


Infraestrutura:


ICombatContextRepository possui implementação concreta.


Dependency Injection resolve a implementação.


Projeto compila.



Integração:


PV-TOOLS-001B consegue avançar além do ponto:

CombatContextRepository resolution


Sem:

Mocks

Bypass

Código específico de teste



Entrega esperada da Engenharia:


Retornar:


1. Arquivos alterados ou criados.


2. Explicação curta da implementação.


3. Build executado.


4. Validação do fluxo.


5. Impactos futuros identificados.



Fim do contrato.


PV-COMBAT-INFRA-001

Objetivo final:

Fechar a lacuna de infraestrutura encontrada pelo Smoke Test e permitir continuidade do PV-TOOLS-001B.