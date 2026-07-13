# Aggregate Roots

Status: LOCKED

Version: 1.0

---

# Objetivo

Este documento define oficialmente os Aggregate Roots do domínio do Project-Victory.

Um Aggregate Root representa o único ponto de entrada para alterações em um conjunto consistente de objetos.

Toda modificação em um Aggregate deve ocorrer através do seu Aggregate Root.

---

# Aggregate Root: Account

Responsável pelo gerenciamento da conta do jogador.

Contém:

- informações da conta;
- personagens pertencentes à conta;
- configurações do usuário.

Não contém regras de gameplay.

---

# Aggregate Root: Character

Principal Aggregate Root do domínio.

É responsável por controlar todo o estado do personagem.

Contém:

- Attributes;
- Health;
- Mana;
- Experience;
- Level;
- Inventory;
- Equipment;
- Skills;
- Quest Progress;
- Status Effects;
- Character Flags.

Nenhum desses objetos poderá ser alterado diretamente.

Toda alteração deve ocorrer através do Character.

---

# Aggregate Root: World

Representa o estado persistente do mundo.

Contém:

- Regions;
- Maps;
- Spawn Definitions;
- World Configuration.

O Runtime nunca altera diretamente o World.

---

# Aggregate Root: Quest

Representa uma Quest completa.

Contém:

- Objectives;
- Rewards;
- Requirements;
- States.

---

# Aggregate Root: Guild (Futuro)

Reservado.

Não será implementado no MVP.

---

# Aggregate Root: Marketplace (Futuro)

Reservado.

Não será implementado no MVP.

---

# Objetos que NÃO são Aggregate Roots

Os seguintes objetos pertencem a outros Aggregates:

Inventory

Equipment

Skill

Health

Mana

Experience

Position

Loot

Item

Monster

NPC

Spawn

Combat

Region

Map

---

# Regras

É proibido modificar diretamente qualquer objeto pertencente a um Aggregate.

Toda alteração deve passar obrigatoriamente pelo Aggregate Root.

---

# Filosofia

O Aggregate Root protege a consistência do domínio.

Ele é o único responsável por manter as invariantes do seu conjunto de objetos.

Essa regra deve permanecer válida durante toda a vida do Project-Victory.