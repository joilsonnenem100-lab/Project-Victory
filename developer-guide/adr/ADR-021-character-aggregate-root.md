# ADR-021 - Character Aggregate Root

Status: ACCEPTED

Version: 1.0

---

# Contexto

O Character representa a entidade central da progressão permanente do jogador.

Diversos sistemas do Project-Victory dependem diretamente dele, tornando necessária uma definição clara de responsabilidades.

---

# Decisão

O Character será o principal Aggregate Root da Engine.

Toda progressão permanente pertence ao Character ou aos Aggregates subordinados a ele.

O Character nunca armazenará estados temporários da execução do jogo.

---

# Consequências

Benefícios:

- responsabilidade única;
- baixo acoplamento;
- evolução independente dos sistemas;
- alta consistência da progressão;
- facilidade para testes.

Custos:

- necessidade de Aggregates especializados para funcionalidades complexas.

---

# Aplicações

Este princípio aplica-se a:

- Level;
- Attributes;
- Skills;
- Equipment;
- Inventory;
- Companion System;
- Pet System;
- Achievements;
- Monster Journal;
- Fame.

---

# Restrições

O Character nunca poderá:

- executar Combat;
- controlar Hunt Session;
- controlar Spawn;
- conhecer Renderização;
- conhecer Interface.

Toda lógica temporária pertence aos módulos de execução.

---

# Referências

Ver também:

- developer-guide/domain/character.md
- developer-guide/domain/hunt.md