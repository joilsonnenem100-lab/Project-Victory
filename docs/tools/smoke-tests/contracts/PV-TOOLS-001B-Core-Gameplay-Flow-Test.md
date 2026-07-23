# PV-TOOLS-001B — Core Gameplay Flow Test

## Objetivo

Criar o primeiro Smoke Test funcional do Project-Victory validando automaticamente o ciclo principal do jogo.

O objetivo é proteger os fluxos já validados contra regressões futuras sem alterar sistemas de produção.


## Fluxo protegido

Login

↓

Character

↓

World

↓

Hunt

↓

Combat

↓

Reward

↓

Experience


## Escopo

Implementar:

- execução automática do fluxo principal do Vertical Slice;
- validação de login;
- validação de acesso ao Character;
- validação de entrada no World;
- validação de criação de Hunt;
- validação de geração de Encounter;
- validação de execução de Combat;
- validação de Reward;
- validação de ganho de Experience.


## Princípios

O Smoke Test deve ser:

- simples;
- rápido;
- baixa manutenção;
- independente de interface visual;
- focado no fluxo crítico do jogo.


## Não implementar

Não criar:

- testes frontend;
- navegador;
- Playwright;
- screenshots;
- relatórios avançados;
- CI/CD;
- testes de performance;
- cobertura completa do sistema.


## Regras arquiteturais

O Smoke Test deve consumir os fluxos reais existentes.

Não criar:

- endpoints exclusivos para teste;
- lógica paralela;
- mocks que escondam problemas reais.


O objetivo é validar o comportamento real do Vertical Slice.


## Sistemas protegidos

Validar sem alterar:

- Account;
- Character;
- World Entry;
- Hunt;
- Combat;
- Reward;
- Experience Progression.


## Critérios de Aceitação

O Smoke Test deve validar:

Login realizado

↓

Character disponível

↓

Entrada no World confirmada

↓

Hunt criada

↓

Encounter gerado

↓

Combat executado

↓

Monster derrotado

↓

Reward recebido

↓

Experience atualizada


## Dependências esperadas

Utilizar:

- estrutura criada pelo PV-TOOLS-001A;
- APIs existentes;
- handlers existentes;
- fluxos já validados do Vertical Slice.


## Fora de escopo

Não implementar:

- validação visual;
- testes de UI;
- automação de navegador;
- alteração nos sistemas de gameplay;
- novas APIs apenas para testes.


## Entrega da Engenharia

Retornar:

- arquivos criados;
- estrutura do projeto de teste;
- configurações adicionadas;
- build executado;
- teste executado;
- resultado completo da execução;
- tempo aproximado de execução.


## Próxima etapa

Após conclusão:

PV-TOOLS-001C — Evolução do Smoke Test

Avaliar:

- integração contínua;
- execução automática em ambiente controlado;
- relatórios simples.