# Monsters

Status: DRAFT

Version: 1.0

---

# Objetivo

Definir o contrato arquitetural do módulo Monsters.

---

# Responsabilidade

O módulo Monsters representa todas as criaturas controladas pela Engine durante uma Hunt.

É responsável por representar espécies, criar instâncias e preservar a consistência do domínio dos Monsters.

---

# Responsabilidades

O módulo Monsters é responsável por:

- representar espécies através de MonsterDefinition;
- criar instâncias de Monster;
- controlar o ciclo de vida dos Monsters;
- aplicar Monster Modifiers durante a criação;
- disponibilizar Monsters para os módulos consumidores.

---

# Dependências

O módulo Monsters poderá depender apenas de:

- Entity;
- AggregateRoot;
- Value Objects;
- Resources.

---

# Proibições

O módulo Monsters não poderá:

- acessar banco de dados;
- conhecer infraestrutura;
- conhecer Runtime;
- conhecer Application;
- conhecer UI;
- controlar Hunts;
- executar Combat;
- calcular Loot.

---

# Regras

Existe apenas uma MonsterDefinition para cada espécie.

MonsterDefinition é imutável.

Monster é um Aggregate Root.

Toda instância de Monster possui um identificador único.

Toda criação de Monster deverá ocorrer através do MonsterFactory.

Toda lógica de Spawn pertence exclusivamente ao MonsterSpawnService.

A MonsterPool poderá conter apenas MonsterDefinitions válidas.

A seleção dos Monsters ocorre através de sorteio ponderado.

Repetições são permitidas durante o Spawn.

Elite não é uma MonsterDefinition.

Elite é um Monster Modifier.

Cada grupo poderá possuir no máximo um Elite.

Toda diferença de dificuldade deverá ser aplicada através de modificadores.

---

# Filosofia

MonsterDefinition representa apenas a espécie.

Toda variação de dificuldade pertence à instância criada.

O módulo privilegia composição ao invés de duplicação.

Monster representa exclusivamente o domínio das criaturas.

O módulo permanece desacoplado de Combat, Loot e Hunt.