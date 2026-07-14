# ADR-005 - Monster AI Data Driven

Status: ACCEPTED

Version: 1.0

---

# Contexto

O módulo Monster AI deverá suportar milhares de decisões por segundo sem aumento significativo da complexidade da Engine.

Cada espécie possui comportamentos diferentes, porém a implementação da IA deve permanecer única.

---

# Decisão

A Monster AI será totalmente orientada por configuração (Data Driven).

A Engine possuirá apenas uma implementação de IA.

O comportamento de cada espécie será definido exclusivamente através da configuração de suas habilidades.

---

# Consequências

Benefícios:

- reduz código;
- reduz manutenção;
- simplifica balanceamento;
- facilita testes;
- aumenta escalabilidade.

Custos:

- necessidade de configuração consistente das espécies.

---

# Aplicações

Este princípio aplica-se a:

- Monsters;
- Elites;
- Bosses;
- Eventos;
- Expansões futuras.

---

# Restrições

Não será permitido criar implementações específicas para espécies.

Exemplos proibidos:

- RatAI;
- SpiderAI;
- DragonAI;
- DemonAI.

Toda diferença de comportamento deverá surgir exclusivamente através da configuração.

---

# Referências

Ver também:

- developer-guide/domain/monster-ai.md
- developer-guide/domain/monsters.md