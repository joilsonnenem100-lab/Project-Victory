# Naming Conventions

Status: LOCKED

Version: 1.0

---

# Objetivo

Definir o padrão oficial de nomenclatura do Project-Victory.

Todos os nomes do projeto devem seguir este documento.

Consistência é mais importante do que preferência pessoal.

---

# Idioma

Todo código deve ser escrito em inglês.

Nunca utilizar português.

---

# Namespaces

Seguir sempre:

ProjectVictory.Domain

ProjectVictory.Application

ProjectVictory.Runtime

ProjectVictory.Persistence

ProjectVictory.Infrastructure

ProjectVictory.API

ProjectVictory.Shared

---

# Classes

PascalCase

Exemplos

Character

ItemInstance

CombatService

MonsterFactory

LootGenerator

---

# Interfaces

Sempre iniciar com I

Exemplos

ICharacterRepository

ICombatSystem

IEventDispatcher

IContainer

---

# Métodos

Sempre iniciar com verbo.

Exemplos

CalculateDamage()

GenerateLoot()

EquipItem()

GainExperience()

Never

Damage()

Loot()

Equip()

---

# Variáveis

camelCase

Exemplos

currentHealth

criticalChance

inventorySlot

---

# Propriedades

PascalCase

Exemplos

Level

Experience

CurrentRegion

CreatedAt

---

# Campos privados

camelCase com "_"

Exemplos

_repository

_dispatcher

_random

---

# Constantes

PascalCase

Exemplos

MaxStackSize

CriticalMultiplier

MaximumUpgradeLevel

---

# Enums

Sempre no singular.

Exemplo

ItemRarity

MonsterType

EquipmentSlot

---

# Eventos

Sempre terminar com Event.

Sempre no passado.

Exemplos

CharacterLeveledUpEvent

LootGeneratedEvent

MonsterKilledEvent

TradeCompletedEvent

---

# Commands

Sempre terminar com Command.

Representam intenção.

Exemplos

EquipItemCommand

StartHuntCommand

UpgradeSpellCommand

---

# Queries

Sempre terminar com Query.

Exemplos

GetCharacterQuery

GetInventoryQuery

GetGuildQuery

---

# Handlers

Sempre terminar com Handler.

Exemplos

EquipItemHandler

MonsterKilledHandler

---

# Services

Sempre terminar com Service.

Exemplos

CombatService

LootService

TrainingService

---

# Factories

Sempre terminar com Factory.

Exemplos

ItemFactory

MonsterFactory

HuntFactory

---

# Components

Sempre terminar com Component.

Exemplos

HealthComponent

BuffComponent

CooldownComponent

---

# Systems

Sempre terminar com System.

Exemplos

CombatSystem

BuffSystem

TrainingSystem

---

# Runtime Objects

Sempre terminar com Runtime.

Exemplos

CharacterRuntime

CombatRuntime

MonsterRuntime

---

# Value Objects

Representam conceito.

Nunca utilizar sufixos desnecessários.

Exemplos

Health

Mana

Gold

Experience

Damage

Level

Duration

---

# Repositories

Interface

ICharacterRepository

Implementação

CharacterRepository

---

# Arquivos

Nome do arquivo deve ser exatamente igual ao nome da classe.

Nunca múltiplas classes públicas por arquivo.

---

# Testes

Nome

Metodo

↓

Condição

↓

Resultado esperado

Exemplo

GainExperience_ShouldLevelUp_WhenExperienceReachesRequirement

---

# Abreviações

Evitar.

Errado

InvMgr

CfgSvc

HPMgr

Correto

InventoryManager

ConfigurationService

HealthService

---

# Comentários

Explicar o motivo.

Nunca explicar o código.

---

# Acrônimos

Utilizar PascalCase.

Exemplo

Npc

Api

Dto

Uuid

Não utilizar

NPC

API

DTO

UUID

(exceto quando a convenção oficial da linguagem exigir outro formato)

---

# Regra Final

Quando houver dúvida,

escolher o nome mais explícito.