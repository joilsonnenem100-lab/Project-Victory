# ADR-016 - Progressive World Discovery

Status: ACCEPTED

Version: 1.0

---

# Contexto

Um dos pilares do Project-Victory é permitir que o jogador descubra o mundo de forma gradual, incentivando exploração, experimentação e interação social.

Exibir todo o conteúdo desde o início reduziria o senso de descoberta e sobrecarregaria novos jogadores.

---

# Decisão

Regions serão reveladas progressivamente.

Cada Character visualizará apenas as Regions já descobertas.

A arquitetura apenas fornece o mecanismo de visibilidade.

As regras de desbloqueio pertencem aos módulos de Progressão e Game Design.

---

# Consequências

Benefícios:

- preserva o senso de descoberta;
- reduz sobrecarga de informação;
- incentiva exploração;
- fortalece a interação social;
- permite expansão contínua do mundo.

Custos:

- necessidade de configuração dos critérios de descoberta.

---

# Aplicações

Este princípio aplica-se a:

- Regions;
- Hunts;
- World Module;
- Sistemas de Progressão.

---

# Restrições

A Engine nunca revelará automaticamente Regions não descobertas.

Toda visibilidade deverá respeitar o progresso do Character.

---

# Referências

Ver também:

- developer-guide/domain/regions.md
- developer-guide/domain/hunt.md
- developer-guide/domain/cities.md