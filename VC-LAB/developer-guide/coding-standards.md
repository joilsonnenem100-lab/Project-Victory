# Coding Standards

Status: LOCKED

Version: 1.0

---

# Objetivo

Definir o padrão oficial de desenvolvimento do Project-Victory.

Todo código novo deve seguir estas regras.

A consistência do código é mais importante do que preferências individuais.

---

# Filosofia

Código deve ser:

- Simples
- Legível
- Determinístico
- Testável
- Escalável
- Fácil de manter

Sempre escrever código pensando que outro desenvolvedor irá mantê-lo daqui a cinco anos.

---

# Regra 1

Código deve ser escrito em inglês.

Classes

Métodos

Variáveis

Namespaces

Comentários técnicos

Tudo em inglês.

Documentação pode permanecer em português.

---

# Regra 2

Nomes devem representar exatamente o comportamento.

Bom

CombatService

LootGenerator

ItemFactory

HealthComponent

Ruim

Manager

Helper

Utils

Common

Data

Processor

Nunca utilizar nomes genéricos.

---

# Regra 3

Uma classe possui uma responsabilidade.

Sempre.

Se uma classe começa a resolver dois problemas,

ela deve ser dividida.

---

# Regra 4

Métodos pequenos.

Preferencialmente:

20~40 linhas.

Se passar muito disso,

avaliar extração.

---

# Regra 5

Evitar comentários.

Código deve explicar sozinho.

Comentários apenas quando explicarem regras de negócio complexas.

Nunca comentar código óbvio.

---

# Regra 6

Não utilizar regiões (#region).

Arquivos pequenos tornam regiões desnecessárias.

---

# Regra 7

Evitar métodos estáticos.

Somente quando forem realmente funções puras.

---

# Regra 8

Nunca utilizar números mágicos.

Errado

Damage *= 1.35

Correto

Damage *= CriticalMultiplier

---

# Regra 9

Constantes possuem local definido.

Nunca espalhar constantes pelo projeto.

---

# Regra 10

Null deve ser tratado explicitamente.

Nunca assumir que algo existe.

---

# Regra 11

Nunca utilizar Exception como fluxo de execução.

Exceptions representam falhas.

Não regras de negócio.

---

# Regra 12

Preferir composição.

Nunca criar grandes cadeias de herança.

---

# Regra 13

Toda classe deve ser facilmente testável.

Se um teste for difícil,

a arquitetura provavelmente está errada.

---

# Regra 14

Nunca acessar banco diretamente.

Sempre através dos Repositories.

---

# Regra 15

Nunca acessar Runtime diretamente.

Sempre através dos Services apropriados.

---

# Regra 16

Não duplicar código.

Antes de copiar,

avaliar abstração.

---

# Regra 17

Métodos devem possuir apenas um verbo.

Exemplo

CalculateDamage()

GenerateLoot()

EquipItem()

Nunca:

CalculateDamageAndApplyLoot()

---

# Regra 18

Sempre preferir código explícito.

Código curto nem sempre é código melhor.

---

# Regra 19

Evitar otimizações prematuras.

Primeiro:

clareza.

Depois:

performance.

---

# Regra 20

Qualquer dúvida deve preservar a arquitetura.

Nunca sacrificar a arquitetura para ganhar velocidade de desenvolvimento.