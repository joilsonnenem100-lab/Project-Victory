# Entities

Status: LOCKED

Version: 1.1

---

# Objetivo

Este documento define todas as Entities oficiais do domínio do Project-Victory.

Uma Entity representa um objeto que possui identidade própria durante todo o seu ciclo de vida.

Sua identidade permanece a mesma, independentemente das alterações em seu estado interno.

---

# Filosofia

Entities representam conceitos que possuem existência própria dentro do domínio.

Toda Entity deve possuir identidade estável.

Toda Entity pode sofrer alterações de estado.

Toda Entity pertence a um único Aggregate Root.

---

# Account

Entity

Aggregate Root.

Representa a conta do jogador.

Responsável por:

- autenticação;
- gerenciamento de personagens;
- preferências da conta.

Não participa do gameplay.

---

# Character

Entity

Aggregate Root principal do domínio.

Representa um personagem jogável.

É responsável por proteger toda a consistência do personagem.

Todo estado do personagem pertence ao Character.

---

# World

Entity

Aggregate Root.

Representa o estado persistente do mundo.

Responsável por:

- regiões;
- mapas;
- definições de spawn;
- configuração global.

---

# Quest

Entity

Aggregate Root.

Representa uma missão completa.

Controla:

- objetivos;
- progresso;
- recompensas;
- estado da missão.

---

# Monster

Entity.

Representa um monstro existente no Runtime.

Não é Aggregate Root.

Sua existência depende da simulação.

---

# NPC

Entity.

Representa um personagem não jogável.

Pode fornecer:

- diálogos;
- comércio;
- quests;
- serviços.

---

# Item

Entity.

Representa um objeto individual do jogo.

Cada Item possui identidade própria.

Dois itens iguais continuam sendo objetos diferentes.

Exemplos:

- durabilidade;
- encantamentos;
- sockets;
- melhorias;
- histórico.

---

# Spawn

Entity.

Representa um ponto de geração de entidades.

Pertence ao World.

---

# Region

Entity.

Representa uma região lógica do World.

Agrupa mapas e definições de spawn.

---

# Map

Entity.

Representa um mapa pertencente ao World.

Não contém regras de gameplay.

---

# Objetos pertencentes ao Character

Os objetos abaixo fazem parte do Aggregate Character.

Não possuem identidade independente.

Não podem ser manipulados fora do Character.

- Inventory
- Equipment
- Skill Collection
- Quest Progress
- Status Effects
- Character Flags

---

# Objetos que NÃO são Entities

Os seguintes conceitos serão implementados como Value Objects:

- Health
- Mana
- Level
- Experience
- Position
- Gold
- Damage
- Defense
- AttackSpeed
- MovementSpeed
- CriticalChance
- CriticalDamage
- DropChance
- Weight
- Capacity
- RespawnTime
- Duration
- Percentage
- Range

---

# Identidade

Toda Entity deve possuir uma identidade única e estável.

A identidade nunca poderá depender de atributos mutáveis.

---

# Mutabilidade

Entities são mutáveis.

Seu estado poderá evoluir durante toda a execução do jogo.

---

# Persistência

A identidade da Entity deve permanecer válida após salvar e carregar o jogo.

---

# Regras

É proibido transformar Value Objects em Entities.

É proibido manipular objetos internos do Character sem passar pelo próprio Character.

É proibido criar novas Entities sem aprovação arquitetural.

---

# Filosofia Final

Entities representam identidade.

Value Objects representam conceitos.

Aggregate Roots representam consistência.

Essa separação deve permanecer estável durante toda a vida do Project-Victory.