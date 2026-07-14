# ADR-030 - Deterministic Modifier Pipeline

Status: ACCEPTED

Version: 1.0

---

# Contexto

Diversos sistemas modificam o desempenho do Character.

Era necessário estabelecer uma ordem oficial para aplicação desses modificadores.

---

# Decisão

A Engine passa a utilizar um Pipeline Determinístico.

Ordem oficial:

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

# Consequências

Benefícios:

- previsibilidade;
- reprodução de resultados;
- facilidade para testes;
- isolamento de responsabilidades;
- baixo acoplamento.

Custos:

- necessidade de respeitar a ordem oficial de processamento.

---

# Aplicações

Este princípio aplica-se a:

- Combat;
- Equipment;
- Buffs;
- Passive Skills;
- Attributes;
- modificadores futuros.

---

# Restrições

Nenhum sistema poderá alterar a ordem oficial de aplicação dos modificadores.

Toda alteração deverá passar pelo Pipeline Determinístico.

---

# Referências

Ver também:

- developer-guide/domain/attributes.md
- developer-guide/domain/combat.md