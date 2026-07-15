# Equipment

Status: DRAFT

Version: 1.0

---

# Objetivo

Definir o contrato arquitetural do módulo Equipment.

---

# Responsabilidade

O módulo Equipment representa exclusivamente os equipamentos atualmente utilizados pelo Character.

Equipment representa poder externo e reutilizável.

Ele nunca representa progressão permanente.

---

# Responsabilidades

O módulo Equipment é responsável por:

- representar Equipment Definitions;
- representar Equipment Instances;
- representar equipamentos equipados;
- disponibilizar Modifiers;
- validar Requirements;
- validar Slots;
- validar Occupation Policy.

---

# Dependências

Equipment poderá depender apenas de:

- Character;
- Equipment Definitions;
- Equipment Instances;
- Validators;
- Policies.

---

# Proibições

Equipment não poderá:

- modificar Experience;
- modificar Base Attributes;
- executar Combat;
- consumir recursos;
- executar Effects;
- conhecer Inventory.

---

# Definition x Instance

Equipment Definition descreve o equipamento.

Equipment Instance representa o equipamento pertencente ao Character.

---

# Modifiers

Equipment nunca contém lógica.

Cada equipamento declara uma coleção de Modifiers.

A Engine interpreta esses Modifiers.

---

# Requirements

Cada equipamento declara seus próprios Requirements.

A Engine apenas valida.

---

# Slots

Cada equipamento declara:

- Compatible Slots;
- Occupation Policy.

A Engine nunca possui regras fixas para tipos de equipamentos.

---

# Ownership

A arquitetura suporta Ownership Policies.

O comportamento padrão do jogo é:

Tradeable.

Ownership Policies representam uma capacidade da Engine.

Não representam uma regra obrigatória.

---

# Visual

A identidade visual permanece desacoplada da identidade mecânica.

---

# Filosofia

Equipment representa poder externo.

Progressão permanente pertence exclusivamente ao Character.

---

# Fluxo

Equipment Definition

↓

Equipment Instance

↓

Requirements

↓

Slot Validation

↓

Occupation Policy

↓

Modifiers

↓

Attribute Pipeline

↓

Combat Formula

↓

Effective Combat Stats

---

# Referências

Ver também:

- character.md
- attributes.md
- combat.md
- inventory.md