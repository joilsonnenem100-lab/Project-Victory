# ADR-028 - Permanent State vs Derived State

Status: ACCEPTED

Version: 1.0

---

# Contexto

A arquitetura do Project-Victory diferencia informações permanentes de informações derivadas.

Era necessário formalizar essa separação para orientar toda a evolução futura da Engine.

---

# Decisão

A arquitetura passa a reconhecer oficialmente dois grupos de informações.

## Estado Permanente

Representa a fonte oficial da verdade.

Exemplos:

- Experience;
- Ascension;
- Inventory;
- Equipment;
- Skills;
- Attributes.

## Estado Derivado

Representa informações calculadas.

Exemplos:

- Level;
- Progressão do Level;
- Experience restante;
- Experience para o próximo Level;
- atributos efetivos;
- estatísticas efetivas.

---

# Consequências

Benefícios:

- menor redundância;
- menor risco de inconsistências;
- maior previsibilidade;
- facilidade de reconstrução.

Custos:

- necessidade de componentes interpretadores.

---

# Aplicações

Este princípio aplica-se a toda a arquitetura do Project-Victory.

---

# Restrições

Informações derivadas nunca poderão tornar-se fonte oficial da verdade.

Toda persistência deverá priorizar estados permanentes.

---

# Referências

Ver também:

- developer-guide/domain/level.md
- developer-guide/domain/experience.md