# ADR-025 - Experience Table Interpretation Model

Status: ACCEPTED

Version: 1.0

---

# Contexto

O Project-Victory utiliza Experience absoluta e Ascensions para representar a progressão do Character.

Era necessário separar definitivamente os dados permanentes da interpretação da progressão.

---

# Decisão

Experience Table passa a representar exclusivamente o interpretador da Experience.

Experience permanece como única fonte de verdade.

Level torna-se um valor derivado.

Cada Ascension referencia sua própria Experience Table.

---

# Consequências

Benefícios:

- separação clara de responsabilidades;
- rebalanceamentos independentes;
- ausência de migração de Experience;
- suporte natural às Ascensions.

Custos:

- necessidade de manter tabelas consistentes.

---

# Aplicações

Este princípio aplica-se a:

- Experience;
- Ascension;
- Level;
- Progressão.

---

# Restrições

Experience Table nunca poderá:

- alterar Experience;
- armazenar Experience;
- executar regras de gameplay.

---

# Referências

Ver também:

- developer-guide/domain/experience-table.md
- developer-guide/domain/experience.md