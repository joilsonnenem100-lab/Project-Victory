# Attributes

Status: DRAFT

Version: 1.0

> **Emenda (2026-11-06, Fila 5, `DEC-20261106-26/27`):** não existem "atributos base" além dos do Tibia. Os atributos do personagem são as **Skills** (Level, Magic Level, Fist, Club, Sword, Axe, Distance Fighting, Shielding, Fishing) e o **Status** (HP e Mana, derivados por Attribute Curve, ADR-029). A classe `Attributes` (Strength, Dexterity, Intelligence, Vitality) foi removida do código no `PV-CHAR-012`. Onde este documento diz "Base Attributes", leia HP/Mana derivados.

---

# Objetivo

Definir o contrato arquitetural do módulo Attributes.

---

# Responsabilidade

O módulo Attributes é responsável exclusivamente por representar os atributos base permanentes do Character.

Os Attributes representam o potencial natural do Character.

Eles nunca representam os atributos efetivos utilizados pelo Combat.

---

# Responsabilidades

O módulo Attributes é responsável por:

- representar os atributos base;
- fornecer a entrada para Combat Formula;
- representar a evolução natural do Character.

---

# Dependências

O módulo Attributes poderá depender apenas de:

- Character;
- Experience;
- Ascension;
- Attribute Curves.

---

# Proibições

O módulo Attributes não poderá:

- conhecer Combat;
- conhecer Equipment;
- conhecer Buffs;
- conhecer Debuffs;
- conhecer Passive Skills;
- calcular Combat Stats.

---

# Definição

Attributes representam um estado derivado.

São determinados por:

Class

↓

Experience

↓

Ascension

↓

Attribute Curve

↓

Base Attributes

---

# Evolução

Enquanto a Experience aumentar:

- os Base Attributes continuam evoluindo;
- não existe congelamento da progressão;
- o último Level não interrompe a evolução.

---

# Equipment

Equipamentos nunca alteram os Base Attributes.

Equipamentos representam modificadores externos.

---

# Combat

Combat nunca utiliza diretamente os Base Attributes.

Combat utiliza exclusivamente os Effective Combat Stats.

---

# Pipeline

Class

↓

Experience

↓

Ascension

↓

Attribute Curve

↓

Base Attributes

↓

Equipment

↓

Passive Skills

↓

Temporary Buffs

↓

Temporary Debuffs

↓

Global Modifiers

↓

Combat Formula

↓

Effective Combat Stats

---

# Filosofia

Base Attributes representam o potencial natural do Character.

Combat Stats representam a capacidade efetiva de combate.

Os dois conceitos permanecem permanentemente desacoplados.

---

# Referências

Ver também:

- character.md
- experience.md
- experience-table.md
- level.md
- combat.md