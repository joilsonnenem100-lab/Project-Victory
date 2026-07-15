# Hunt Configuration Contract

## Status

Definido

---

# Contexto

O PV-VS-018 — Iniciar Hunt com Configuração Válida define que uma Hunt somente pode ser iniciada quando possuir uma configuração válida de execução.

O fluxo atual possui criação e execução de Hunt, porém é necessário definir formalmente o modelo responsável por representar os dados necessários antes do início da execução.

Este documento estabelece o contrato arquitetural oficial para a configuração de Hunt.

---

# Objetivo

Definir o conceito de `HuntConfiguration` como a representação dos dados mínimos necessários para permitir a criação de uma Hunt.

`HuntConfiguration` representa uma configuração de execução.

Não representa:

- uma Hunt em andamento;
- uma execução de combate;
- uma recompensa;
- uma progressão.

---

# Modelo Arquitetural

## Nome

```text
HuntConfiguration