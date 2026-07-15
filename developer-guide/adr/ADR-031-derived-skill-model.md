# ADR-031 - Derived Skill Model

Status: ACCEPTED

Version: 1.0

---

# Contexto

As Skills fazem parte da progressão permanente do Character.

Era necessário definir como essa evolução ocorreria.

---

# Decisão

As Skills são desbloqueadas automaticamente.

Seu nível efetivo é determinado utilizando:

- Experience;
- Ascension;
- Skill Progression.

O jogador nunca distribui pontos.

---

# Consequências

Benefícios:

- ausência de builds permanentes;
- simplicidade;
- balanceamento isolado;
- progressão determinística.

Custos:

- necessidade da Skill Progression.

---

# Aplicações

Este princípio aplica-se a:

- Character;
- Skills;
- Combat.

---

# Restrições

Skills nunca poderão:

- executar Effects;
- modificar Experience;
- armazenar Skill Levels permanentes.

---

# Referências

Ver também:

- developer-guide/domain/skills.md