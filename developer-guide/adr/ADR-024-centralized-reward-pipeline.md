# ADR-024 - Centralized Reward Pipeline

Status: ACCEPTED

Version: 1.0

---

# Contexto

Diversos sistemas concedem recompensas permanentes ao Character.

Era necessário estabelecer um único ponto oficial para aplicação dessas recompensas.

---

# Decisão

Todo ganho permanente será aplicado exclusivamente através do Reward Pipeline.

O Reward Pipeline torna-se o único responsável por modificar Experience e demais progressões permanentes.

---

# Consequências

Benefícios:

- consistência;
- atomicidade;
- auditoria;
- extensibilidade;
- desacoplamento.

Custos:

- necessidade de integração de novos sistemas ao Reward Pipeline.

---

# Aplicações

Este princípio aplica-se a:

- Experience;
- Loot;
- Fame;
- Monster Journal;
- Achievements;
- Sistemas futuros de progressão.

---

# Restrições

Nenhum módulo poderá modificar Experience diretamente.

Toda alteração permanente deverá passar obrigatoriamente pelo Reward Pipeline.

---

# Referências

Ver também:

- developer-guide/domain/experience.md
- developer-guide/domain/character.md
- developer-guide/domain/hunt.md