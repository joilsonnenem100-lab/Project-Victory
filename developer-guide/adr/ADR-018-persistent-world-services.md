# ADR-018 - Persistent World Services

Status: ACCEPTED

Version: 1.0

---

# Contexto

O Project-Victory utiliza a Cidade Principal como Hub permanente do mundo.

Os serviços essenciais precisam estar sempre disponíveis para o jogador.

---

# Decisão

Todo serviço permanente do jogo possuirá um NPC correspondente.

Os NPCs permanecerão disponíveis continuamente.

A indisponibilidade de um NPC permanente não faz parte do Game Design.

NPCs temporários poderão existir apenas para conteúdos específicos, como eventos.

---

# Consequências

Benefícios:

- previsibilidade;
- simplicidade;
- maior imersão;
- mundo consistente;
- acesso permanente aos serviços.

Custos:

- necessidade de manter alta disponibilidade dos serviços.

---

# Aplicações

Este princípio aplica-se a:

- Cities;
- NPCs;
- Social Systems;
- Market;
- Craft;
- Guilds;
- Companion System.

---

# Restrições

Serviços permanentes nunca dependerão de horário do servidor para funcionamento.

Toda indisponibilidade será tratada como falha operacional, e não como mecânica do jogo.

---

# Referências

Ver também:

- developer-guide/domain/npcs.md
- developer-guide/domain/cities.md