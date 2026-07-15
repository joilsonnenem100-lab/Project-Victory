# Skills

Status: DRAFT

Version: 1.0

---

# Objetivo

Definir o contrato arquitetural do módulo Skills.

---

# Responsabilidade

O módulo Skills é responsável exclusivamente por representar as habilidades permanentes do Character.

Skills representam as capacidades disponíveis do Character.

A execução dessas capacidades pertence ao módulo Effects.

---

# Responsabilidades

O módulo Skills é responsável por:

- representar Skills;
- representar Skill Tree;
- representar Skill Progression;
- representar Skill Loadout;
- validar requisitos;
- disponibilizar Skills ao Combat.

---

# Dependências

Skills poderá depender apenas de:

- Character;
- Experience;
- Ascension;
- Skill Tree;
- Skill Progression.

---

# Proibições

Skills não poderá:

- executar dano;
- executar cura;
- aplicar Buffs;
- aplicar Debuffs;
- conhecer Combat;
- conhecer Equipment;
- conhecer Inventory.

---

# Estrutura

Cada Skill possui:

- Id;
- Version;
- Definition;
- Requirements;
- Progression;
- Cooldown;
- Cost;
- Tags;
- Effects.

---

# Progressão

A evolução das Skills é automática.

O jogador nunca distribui pontos.

A Engine resolve o nível efetivo utilizando:

Experience

↓

Ascension

↓

Skill Progression

↓

Effective Skill Level

---

# Loadout

O Character pode possuir inúmeras Skills desbloqueadas.

Entretanto apenas o Skill Loadout participa do combate.

O Loadout pode ser alterado apenas fora do estado de combate.

---

# Effects

Skills nunca executam lógica diretamente.

Cada Skill apenas referencia uma coleção de Effects.

---

# Filosofia

Progressão desbloqueia possibilidades.

O jogador escolhe quais possibilidades permanecerão ativas.

---

# Fluxo

Class

↓

Skill Tree

↓

Unlocked Skills

↓

Skill Loadout

↓

Effects

↓

Combat

---

# Referências

Ver também:

- character.md
- attributes.md
- combat.md