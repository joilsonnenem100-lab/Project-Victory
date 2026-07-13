# Event Guidelines

Status: LOCKED

Version: 1.0

---

# Objetivo

Definir o padrão oficial para utilização de Events dentro do Project-Victory.

Todo Event criado no projeto deve seguir obrigatoriamente estas regras.

---

# Filosofia

Um Event representa algo que JÁ aconteceu.

Nunca uma intenção.

Nunca uma solicitação.

Sempre um fato consumado.

---

# Commands vs Events

Commands

↓

"Faça isso."

Exemplo

StartHuntCommand

EquipItemCommand

UpgradeSpellCommand

TradeItemCommand

---

Events

↓

"Isso aconteceu."

Exemplo

HuntStartedEvent

ItemEquippedEvent

SpellUpgradedEvent

TradeCompletedEvent

---

# Regra 1

Commands alteram o jogo.

Events informam o jogo.

Nunca inverter.

---

# Regra 2

Events são imutáveis.

Após criados,

nunca podem ser modificados.

---

# Regra 3

Um Event possui apenas informações necessárias.

Nunca enviar objetos enormes.

Exemplo

Errado

Character

Inventory

Equipment

Combat

Correto

CharacterId

ItemInstanceId

Timestamp

---

# Regra 4

Events nunca executam lógica.

Eles apenas representam um fato.

---

# Regra 5

Events nunca conhecem quem irá consumi-los.

O produtor nunca conhece os consumidores.

---

# Regra 6

Todo Event deve possuir nome no passado.

Exemplos

MonsterKilledEvent

LootGeneratedEvent

CharacterLeveledUpEvent

GuildCreatedEvent

---

# Regra 7

Events pertencem ao Domain.

A publicação é responsabilidade da Application ou Runtime.

---

# Regra 8

Não criar Events genéricos.

Errado

DataChangedEvent

ObjectUpdatedEvent

Correto

EquipmentChangedEvent

InventoryExpandedEvent

SpellResetEvent

---

# Regra 9

Um Event representa apenas um acontecimento.

Nunca vários acontecimentos juntos.

---

# Regra 10

Events nunca acessam banco.

Nunca acessam Runtime.

Nunca acessam API.

---

# Regra 11

Handlers podem gerar novos Events.

Exemplo

MonsterKilledEvent

↓

LootGeneratedEvent

↓

AchievementUnlockedEvent

↓

CollectionProgressedEvent

---

# Regra 12

Nunca criar dependência entre Handlers.

Todos são independentes.

---

# Regra 13

Handlers devem ser idempotentes sempre que possível.

Processar o mesmo Event duas vezes não deve gerar inconsistências.

---

# Regra 14

Nunca utilizar Events para comunicação síncrona obrigatória.

Quando houver dependência imediata,

utilizar Services.

---

# Regra 15

Events representam domínio.

Nunca detalhes técnicos.

---

# Regra 16

Todo Event deve possuir Timestamp.

---

# Regra 17

Todo Event deve possuir um EventId único.

Facilita auditoria,

Replay,

Debug.

---

# Regra 18

Events devem ser facilmente serializáveis.

---

# Regra 19

Toda alteração importante da Domain gera Event.

---

# Regra 20

Quando houver dúvida,

preferir criar um novo Event específico.

Nunca reutilizar Events genéricos.