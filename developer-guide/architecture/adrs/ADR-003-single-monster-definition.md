# ADR-003 - MonsterDefinition Única por Espécie

Status: ACCEPTED

---

# Contexto

Durante a definição do módulo Monsters foi necessário decidir como representar diferentes níveis de dificuldade de uma mesma criatura.

As alternativas avaliadas foram:

- criar múltiplas MonsterDefinitions;
- manter uma única MonsterDefinition e aplicar modificadores durante o Spawn.

---

# Problema

Criar múltiplas MonsterDefinitions gera crescimento exponencial da base de dados.

Exemplo:

Rat

Rat Forest

Rat Cave

Rat Volcano

Rat Lv 10

Rat Lv 30

Elite Rat

Elite Rat Event

Elite Rat Dungeon

...

Essa abordagem aumenta significativamente:

- manutenção;
- documentação;
- testes;
- balanceamento.

---

# Decisão

Cada espécie do jogo possuirá exatamente uma MonsterDefinition.

Toda diferença de comportamento será produzida durante a criação da instância através de modificadores.

Fluxo oficial:

MonsterDefinition

↓

Hunt Modifier

↓

Monster Modifiers

↓

Monster Instance

---

# Princípios

MonsterDefinition representa apenas a espécie.

MonsterDefinition nunca representa:

- dificuldade;
- região;
- evento;
- Elite;
- Boss;
- temporada.

Essas diferenças pertencem exclusivamente aos modificadores.

---

# Consequências

Benefícios:

- balanceamento centralizado;
- menor quantidade de dados;
- reutilização máxima;
- menor custo de manutenção;
- documentação mais simples.

Custos:

- necessidade de um pipeline de modificadores.

O benefício supera amplamente o custo.

---

# Exemplos

Correto:

Rat Definition

↓

Forest Hunt Modifier

↓

Elite Modifier

↓

Monster Instance

Também:

Rat Definition

↓

Volcano Hunt Modifier

↓

Champion Modifier

↓

Monster Instance

---

# Incorreto

Rat Forest

Rat Cave

Rat Lv 20

Elite Rat

Champion Rat

Cada uma dessas representações exigiria uma nova MonsterDefinition e viola este ADR.

---

# Impacto

Esta decisão afeta diretamente os módulos:

- Monsters
- Hunts
- Combat
- Loot
- Progression
- Future Events

Todos esses módulos deverão respeitar este princípio.

---

# Exceções

Somente uma nova espécie poderá originar uma nova MonsterDefinition.

Exemplos:

Rat

Spider

Dragon

Demon

Cada espécie continua possuindo apenas uma Definition.

---

# Status

Esta decisão é permanente.

Toda exceção deverá possuir ADR específico aprovado pela Arquitetura.