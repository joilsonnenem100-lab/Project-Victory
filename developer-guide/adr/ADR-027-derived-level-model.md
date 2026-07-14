# ADR-027 - Derived Level Model

Status: ACCEPTED

Version: 1.0

---

# Contexto

Durante a evolução da arquitetura do Project-Victory, verificou-se que o Level não representa a verdadeira progressão do Character.

A Experience absoluta tornou-se a única fonte oficial da evolução.

Era necessário redefinir o papel do Level.

---

# Decisão

Level passa a representar exclusivamente um valor derivado.

Ele será calculado utilizando:

- Experience;
- Ascension;
- Experience Table.

Level deixa de ser uma fonte de verdade.

---

# Consequências

Benefícios:

- ausência de inconsistências;
- eliminação de sincronizações;
- compatibilidade com Ascensions;
- simplicidade arquitetural;
- facilidade para auditoria.

Custos:

- necessidade de resolução através da Experience Table.

---

# Aplicações

Este princípio aplica-se a:

- Character;
- Experience;
- Ascension;
- Interface.

---

# Restrições

Level nunca poderá:

- modificar Experience;
- armazenar progresso;
- controlar desbloqueios.

---

# Referências

Ver também:

- developer-guide/domain/level.md
- developer-guide/domain/experience-table.md