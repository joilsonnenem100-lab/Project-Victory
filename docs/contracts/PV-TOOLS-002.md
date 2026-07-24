PROJECT-VICTORY

PV-TOOLS-002 — Smoke Test Observability Upgrade


Status:

Aprovado para implementação


Objetivo:

Melhorar a visualização e rastreabilidade da execução automática do Smoke Test do Project-Victory.

O objetivo não é alterar o comportamento do jogo, mas tornar a execução do teste funcional mais clara para desenvolvimento, validação e diagnóstico.


Contexto:

O PV-TOOLS-001B validou com sucesso o Vertical Slice completo:

Account

↓

Authentication

↓

Character

↓

Enter Game

↓

RuntimeExecutionContext

↓

Hunt

↓

Encounter

↓

Combat

↓

Reward

↓

Experience


Porém a saída atual do teste apresenta poucas informações:

Exemplo atual:

STORE Runtime=xxxx
Character=xxxx

Encounter criado

PASS


A equipe precisa visualizar claramente cada etapa executada.


Escopo:


Criar uma camada simples de relatório para o Smoke Test.


O teste deve informar:

- etapa executada;
- resultado da etapa;
- identificadores importantes;
- resultado final da execução.


Fluxo esperado de saída:


========================================

PROJECT VICTORY SMOKE TEST

========================================


[1] Account Creation

PASS


[2] Authentication

PASS


[3] Character Creation

PASS


[4] Enter Game

PASS


[5] Runtime Execution

PASS

RuntimeId: xxxx


[6] Hunt

PASS

HuntId: xxxx


[7] Combat

PASS

EncounterId: xxxx


[8] Reward

PASS


[9] Experience

PASS


========================================

RESULT:

VERTICAL SLICE HEALTHY

========================================



Arquivos envolvidos:


Projeto:

tests/ProjectVictory.SmokeTests


Arquivos existentes a analisar:


CoreGameplayFlowSmokeTest.cs


TestCurrentPlayerContext.cs


TestServiceCollectionExtensions.cs


Possível novo arquivo:


SmokeTestReporter.cs


A criação de novos arquivos deve ocorrer somente se melhorar organização.


Regras:


Alterar somente o projeto de testes.


Não alterar:


Domain

Application

API

Infrastructure

Persistence


Não alterar lógica de gameplay.


Não criar:


Novo sistema de testes paralelo.

Nova infraestrutura.

Novo mecanismo de execução.


Estratégia:


Manter exatamente o fluxo atual aprovado.


Apenas substituir saídas técnicas dispersas por um relatório organizado.


Exemplo:


Antes:

Console.WriteLine("STORE Runtime=" + runtimeId);


Depois:


SmokeTestReporter.StepPassed(
    "Runtime Execution",
    $"RuntimeId: {runtimeId}"
);



Critérios de aceite:


Execução:

O comando deve continuar funcionando:


dotnet test .\tests\ProjectVictory.SmokeTests\ProjectVictory.SmokeTests.csproj


Resultado:

✅ Teste continua passando.


Saída:

Deve apresentar todas as etapas do Vertical Slice.


Falhas:

Caso alguma etapa falhe:

- identificar a etapa;
- mostrar erro;
- manter falha do teste.


Compatibilidade:

PV-TOOLS-001B continua aprovado.


Entrega esperada:


Retornar:

1. Arquivos alterados.

2. Arquivos criados.

3. Código completo dos arquivos modificados.

4. Build/test executado.

5. Exemplo da nova saída do Smoke Test.



Fim do contrato.

PV-TOOLS-002

Objetivo final:

Transformar o Smoke Test em uma ferramenta clara de validação do Vertical Slice.