# ADR-015 - Region World Organization

Status: ACCEPTED

Version: 1.0

---

# Contexto

O crescimento contínuo do Project-Victory exige uma forma consistente de organizar centenas de Hunts sem aumentar a complexidade da Engine.

Era necessário separar organização do mundo da execução do gameplay.

---

# Decisão

Regions passam a representar exclusivamente agrupadores lógicos de Hunts.

Regions não possuem responsabilidade sobre:

- gameplay;
- progressão;
- combate;
- recompensas.

Sua única responsabilidade é organizar o conteúdo do mundo.

---

# Consequências

Benefícios:

- baixo acoplamento;
- expansão ilimitada do mundo;
- facilidade de navegação;
- simplicidade arquitetural;
- organização consistente.

Custos:

- necessidade de manter a configuração das Hunts consistente.

---

# Aplicações

Este princípio aplica-se a:

- Regions;
- Hunts;
- World Module.

---

# Restrições

Regions nunca poderão:

- executar regras de gameplay;
- controlar progressão;
- definir dificuldade;
- conhecer Combat Engine.

---

# Referências

Ver também:

- developer-guide/domain/regions.md
- developer-guide/domain/hunt.md