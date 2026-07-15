# Hunt Configuration Contract

## Status

Definido

---

# Contexto

O PV-VS-018 — Iniciar Hunt com Configuração Válida define que uma Hunt somente pode ser iniciada quando possuir uma configuração válida de execução.

O fluxo atual possui criação e execução de Hunt, porém ainda não existe uma definição arquitetural para representar os dados necessários antes do início da execução.

Este documento define o contrato arquitetural da configuração de Hunt.

---

# Objetivo

Definir o conceito de Hunt Configuration como a representação dos dados mínimos necessários para iniciar uma Hunt.

A configuração representa a definição da execução.

Ela não representa uma Hunt em andamento.

---

# Responsabilidade

HuntConfiguration é responsável por:

- representar os parâmetros necessários para iniciar uma Hunt;
- garantir que os valores mínimos estejam válidos;
- permitir que uma Hunt seja criada somente com configuração válida.

Não é responsabilidade de HuntConfiguration:

- executar combate;
- controlar progresso;
- processar recompensa;
- aplicar experiência;
- controlar balanceamento.

---

# Localização Arquitetural

HuntConfiguration pertence ao contexto:
