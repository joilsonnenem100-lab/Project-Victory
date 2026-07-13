# Testing Guidelines

Status: LOCKED

Version: 1.0

---

# Objetivo

Definir a estratégia oficial de testes do Project-Victory.

Todo código novo deve nascer preparado para ser testado.

Testes não são opcionais.

São parte da arquitetura.

---

# Filosofia

O objetivo dos testes não é encontrar bugs.

O objetivo é impedir regressões.

---

# Pirâmide de Testes

Project-Victory utilizará a seguinte distribuição.

70%

↓

Unit Tests

20%

↓

Integration Tests

10%

↓

End-to-End Tests

---

# Regra 1

Toda regra de negócio importante deve possuir Unit Test.

---

# Regra 2

Toda correção de bug deve gerar um teste.

Primeiro o teste.

Depois a correção.

---

# Regra 3

Não testar Frameworks.

Testar apenas comportamento do projeto.

---

# Regra 4

Runtime deve ser testável sem banco.

---

# Regra 5

Domain deve ser testável sem Runtime.

---

# Regra 6

Tests nunca dependem da ordem de execução.

---

# Regra 7

Tests devem ser determinísticos.

Mesmo input.

Mesmo resultado.

---

# Regra 8

Utilizar Mocks apenas quando necessário.

Preferir objetos reais quando possível.

---

# Regra 9

Não utilizar banco real em Unit Tests.

---

# Regra 10

Cada teste valida apenas um comportamento.

---

# Regra 11

Nome do teste deve explicar o comportamento esperado.

Exemplo

GainExperience_ShouldLevelUp_WhenExperienceReachesRequirement

---

# Regra 12

Não compartilhar estado entre testes.

---

# Regra 13

Runtime Tests nunca dependem de tempo real.

Utilizar Tick controlado.

---

# Regra 14

Combat Tests devem utilizar Seed fixa.

---

# Regra 15

Loot Tests devem utilizar Seed fixa.

---

# Regra 16

Todo bug crítico gera um teste permanente.

---

# Regra 17

Performance Tests não substituem Unit Tests.

---

# Regra 18

Toda Entity deve possuir cobertura mínima dos seus comportamentos.

---

# Regra 19

Factories devem possuir testes próprios.

---

# Regra 20

Quando houver dúvida,

escrever o teste antes da implementação.