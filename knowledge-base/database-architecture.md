# Database Architecture

Version: 1.0

Status: LOCKED

Last Sprint: SPRINT-028

---

# Objetivo

Definir a organização estrutural do banco de dados do Project-Victory antes da criação das tabelas físicas.

---

# Filosofia

O banco representa a memória permanente do mundo.

A modelagem deve priorizar:

- escalabilidade;
- consistência;
- baixo acoplamento;
- facilidade de evolução.

---

# Domínios

## Account

- Account
- Wallet
- Premium
- Settings

---

## Character

- Character
- Inventory
- Equipment
- Skills
- Spells
- Statistics
- Achievements
- Collections

---

## World

- Continent
- Region
- HuntTemplate
- MonsterTemplate

---

## Items

- ItemTemplate
- ItemInstance

---

## Economy

- Wallet
- Transactions
- Market
- Trade

---

## Guild

- Guild
- GuildMember
- GuildBank
- GuildProgress
- GuildBoss

---

# Princípios

- Template → Factory → Instance
- Event-Driven
- Domain First
- Bounded Context
- Compatibility First
- Stable Core, Expandable Edge