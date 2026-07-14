# Weapons

# Objetivo

Weapons representam equipamentos ofensivos utilizados pelo Character durante o combate.

Uma Weapon participa diretamente do cálculo de dano e define qual Skill será utilizada durante ataques físicos e spells ofensivas compatíveis.

Weapons não conhecem Character, Skills, Vocation ou Combat.

Elas apenas descrevem suas próprias características.

---

# Weapon Category

Toda Weapon pertence obrigatoriamente a uma única categoria.

Categorias oficiais:

- Fist
- Club
- Sword
- Axe
- Distance

Weapon Category define exclusivamente qual Skill será utilizada durante os cálculos de combate.

Uma Weapon nunca pertence a mais de uma categoria.

---

# Weapon Type

Weapon Type representa a forma física de utilização da arma.

Exemplos:

- One Handed
- Two Handed
- Bow
- Crossbow
- Spear
- Throwing Weapon

Weapon Type não influencia qual Skill será utilizada.

Sua responsabilidade é apenas representar o comportamento físico da arma.

---

# Attack

Toda Weapon possui um valor base de Attack.

Attack representa o poder ofensivo base da arma.

O dano final é determinado posteriormente pelo sistema de combate.

---

# Defense

Weapons podem possuir Defense.

Defense representa a capacidade defensiva fornecida pela arma durante o combate.

---

# Requirements

Weapons conhecem seus próprios requisitos.

Exemplos:

- Required Level
- Required Vocation
- Required Quest
- Outros requisitos específicos definidos futuramente.

Esses requisitos fazem parte da identidade permanente do Template.

---

# Weapon Modifiers

Weapons podem fornecer modificadores temporários.

Exemplos:

- +Magic Level
- +Sword Fighting
- +Axe Fighting
- +Shielding

Esses modificadores nunca alteram permanentemente as Skills do Character.

---

# Relação com Skills

Toda Weapon utiliza exatamente uma Skill.

Exemplos:

- Fist → Fist Fighting
- Club → Club Fighting
- Sword → Sword Fighting
- Axe → Axe Fighting
- Distance → Distance Fighting

O sistema de combate determina automaticamente qual Skill utilizar a partir da Weapon Category.

---

# Relação com Spells

Spells ofensivas nunca conhecem Skills específicas.

Quando necessário, a Spell consulta apenas a Weapon Category atualmente equipada.

A Skill correspondente é determinada automaticamente pelo sistema.

---

# Ataques Automáticos

Ataques automáticos evoluem exclusivamente a Skill correspondente à Weapon Category utilizada.

---

# Spells

Spells ofensivas utilizam a Skill correspondente à Weapon Category equipada para cálculo de dano.

A evolução gerada pela Spell ocorre exclusivamente em Magic Level.

Skills físicas não evoluem através da utilização de Spells.

---

# Durabilidade

Weapons não possuem sistema de durabilidade.

---

# Attack Speed

Weapons não possuem velocidade de ataque.

O combate do Project-Victory é baseado em turnos.

---

# Princípios Arquiteturais

- Toda Weapon pertence a exatamente uma Weapon Category.
- Weapon Category determina a Skill utilizada.
- Weapon Type representa apenas o comportamento físico da arma.
- Weapons nunca modificam permanentemente Skills.
- Spells consultam Weapon Category, nunca Skills específicas.
- O cálculo de dano pertence exclusivamente ao sistema de combate.