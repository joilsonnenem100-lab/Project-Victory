# Persistence Model

Version: 1.0

Status: LOCKED

Last Sprint: SPRINT-027

---

# Objetivo

Definir oficialmente quais dados são persistidos e quais existem apenas em memória durante a execução do servidor.

---

# Filosofia

No Project-Victory, persistência representa continuidade.

Todo progresso importante do jogador deve sobreviver a reinicializações do servidor.

---

# Dados Persistentes

Entidades persistidas:

- Account
- Character
- Wallet
- Inventory
- Equipment
- Skills
- Spells
- Spell Upgrades
- Achievements
- Collections
- Guild
- Guild Progress
- Guild Bank
- Market Listings
- Trade History
- Quest Progress
- Premium
- VC Points
- Player Settings
- Character Statistics

---

# Dados Temporários

Existem apenas em memória:

- HuntInstance
- CombatInstance
- PackInstance
- MonsterInstance
- Buffs
- Debuffs
- Cooldowns
- Damage Queue
- Current Target
- AI State

Ao terminar a Hunt:

Destroy()

---

# Salvamento

Persistência híbrida.

## Salvamento imediato

- Gold
- Loot
- Upgrade
- Trade
- Market
- Guild Donation
- Ascension
- Character Creation
- Level Up

## Salvamento periódico

- Interface
- Preferências
- Estado visual

Intervalo inicial:

60 segundos

Configurável.

---

# IDs

Todas as entidades utilizam:

UUID v7

Nunca reutilizados.

---

# Soft Delete

Entidades importantes utilizam:

deleted_at

A remoção física poderá ocorrer posteriormente.

---

# Auditoria

Registrar:

- Trade
- Market
- Premium
- VC Points
- Guild Donations
- Ascensions
- Character Delete

Não registrar:

- Turnos
- Ataques
- Dano
- Buffs
- IA

---

# Cache

Jogador Online:

Todos os dados ficam em memória.

Jogador Offline:

Persistência completa.

Remoção da memória.

---

# Banco Oficial

PostgreSQL.

---

# Escalabilidade

Arquitetura preparada para:

- múltiplos servidores;
- particionamento;
- cache distribuído;
- microsserviços futuros.

---

# Arquitetura Event-Driven

Eventos oficiais:

MonsterDeathEvent

CharacterDeathEvent

LootCreatedEvent

ItemEquippedEvent

LevelUpEvent

AscensionEvent

QuestCompletedEvent

TradeCompletedEvent

MarketPurchaseEvent

GuildDonationEvent

CollectionCompletedEvent

AchievementUnlockedEvent

Cada sistema responde apenas aos eventos que lhe interessam.

Não existe chamada direta entre sistemas quando um evento pode resolver o fluxo.

---

# Arquitetura

```
Game Event

↓

Event Bus

↓

Services

↓

Persistence
```

Essa arquitetura é o padrão oficial do Project-Victory.