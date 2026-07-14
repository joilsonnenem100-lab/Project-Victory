# ADR-023 - Absolute Experience Model

Status: ACCEPTED

Version: 1.0

---

# Contexto

O Project-Victory utiliza Ascensions que reinterpretam a progressão do Character sem remover seu progresso permanente.

Era necessário definir uma fonte única e imutável da evolução.

---

# Decisão

Experience passa a representar a única fonte oficial da progressão.

Experience é um contador absoluto.

Ela nunca reinicia.

Ela nunca diminui.

Toda interpretação da Experience pertence à Experience Table.

---

# Consequências

Benefícios:

- única fonte de verdade;
- auditoria simplificada;
- compatibilidade com Ascensions;
- ausência de migrações de Experience;
- arquitetura altamente previsível.

Custos:

- necessidade de uma Experience Table para interpretar a progressão.

---

# Aplicações

Este princípio aplica-se a:

- Character;
- Ascension;
- Experience Table;
- Level.

---

# Restrições

Experience nunca poderá:

- armazenar percentuais;
- armazenar progresso parcial;
- depender da Ascension.

---

# Referências

Ver também:

- developer-guide/domain/experience.md
- developer-guide/domain/character.md