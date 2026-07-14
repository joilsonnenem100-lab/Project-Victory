# Character

Status: DRAFT

Version: 1.0

---

# Objetivo

Definir o contrato arquitetural do Aggregate Root Character.

---

# Responsabilidade

O Character representa a identidade permanente e jogável do jogador.

É o principal Aggregate Root do Project-Victory.

Toda progressão permanente pertence ao Character ou a Aggregates subordinados a ele.

---

# Responsabilidades

O Character é responsável por:

- representar a identidade jogável;
- armazenar a progressão permanente;
- possuir Level;
- possuir Attributes;
- possuir Skills;
- possuir Inventory;
- possuir Equipment;
- possuir Companions;
- possuir Pets;
- possuir Cosmetics;
- possuir Achievements;
- possuir Monster Journal;
- possuir Fame.

---

# Dependências

O Character poderá depender apenas de:

- Account;
- Level;
- Attributes;
- Skills;
- Inventory;
- Equipment;
- Fame;
- Monster Journal;
- Companion System;
- Pet System.

---

# Proibições

O Character não poderá:

- executar Combat;
- controlar Hunt Session;
- controlar Spawn;
- conhecer Map;
- conhecer City Channel;
- conhecer Interface;
- conhecer Renderização.

---

# Account

Uma Account poderá possuir múltiplos Characters.

Apenas um Character poderá permanecer ativo simultaneamente.

---

# Classe

Classes oficiais:

- Knight
- Paladin
- Druid
- Sorcerer
- Monk

A classe é permanente.

Nunca poderá ser alterada.

---

# Progressão

Toda progressão pertence ao Character.

Estados temporários pertencem exclusivamente aos módulos de execução.

---

# Alteração de Nome

A alteração de nome ocorre exclusivamente através do Rename Service.

Regras:

- custo progressivo;
- reserva do nome anterior por 6 meses;
- histórico permanente;
- reutilização automática após expiração da reserva.

Toda identidade técnica permanece vinculada ao CharacterId.

---

# Exclusão

Fluxo oficial:

Solicitação

↓

Pending Deletion

↓

30 dias

↓

Exclusão definitiva

Durante o período de segurança:

- exclusão pode ser cancelada;
- Character permanece preservado;
- nome continua reservado.

---

# Filosofia

O Character representa exclusivamente a progressão permanente do jogador.

Todo estado temporário pertence aos módulos de execução.

---

# Referências

Ver também:

- account.md
- level.md
- attributes.md
- skills.md
- inventory.md
- equipment.md
- hunt.md