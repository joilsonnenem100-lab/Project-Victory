# PV-TOOLS-001A --- Smoke Test Project Foundation

## Local recomendado

docs/tools/smoke-tests/

## Objetivo

Criar a fundação inicial do sistema de Smoke Test do Project-Victory.

Esta ferramenta deve permitir validação automática rápida do ciclo
principal do jogo sem alterar sistemas de produção.

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

Incluído:

-   criação do projeto de teste;
-   configuração inicial;
-   estrutura preparada para testes automáticos;
-   primeiro teste executável.

## Fora de escopo

Não implementar:

-   teste frontend;
-   navegador;
-   Playwright;
-   screenshots;
-   relatórios avançados;
-   CI/CD.

## Princípios

-   simples;
-   rápido;
-   baixa manutenção;
-   sem dependência visual;
-   focado no fluxo crítico.

## Critérios de Aceitação

-   projeto Smoke Test criado;
-   solução compila;
-   teste inicial executa;
-   estrutura pronta para o Core Gameplay Flow Test.

## Entrega da Engenharia

Retornar:

-   arquivos criados;
-   configurações adicionadas;
-   build executado;
-   teste executado;
-   resultado obtido.

## Próxima etapa

PV-TOOLS-001B --- Core Gameplay Flow Test

Adicionar validação automática do fluxo completo:

Login → Character → World → Hunt → Combat → Reward → Experience
