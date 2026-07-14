# ADR-014 - Transparent City Channels

Status: ACCEPTED

Version: 1.0

---

# Contexto

A Cidade Principal deverá suportar crescimento sem comprometer desempenho ou experiência do jogador.

Ao mesmo tempo, a infraestrutura não deve expor conceitos técnicos como canais ou shards.

---

# Decisão

A Cidade Principal utilizará City Channels transparentes.

Características:

- capacidade inicial de 50 jogadores por instância;
- criação sob demanda;
- destruição automática quando vazios;
- distribuição automática de jogadores;
- ausência de seleção manual de canais.

A movimentação entre instâncias ocorrerá apenas através de funcionalidades sociais autorizadas, como:

- Ir até o amigo;
- Party;
- Guilds;
- Eventos futuros.

---

# Consequências

Benefícios:

- excelente escalabilidade;
- infraestrutura invisível ao jogador;
- fortalecimento da interação social;
- otimização de recursos do servidor.

Custos:

- necessidade de gerenciamento automático de capacidade.

---

# Aplicações

Este princípio aplica-se a:

- Cities;
- Social Systems;
- Party;
- Guilds;
- Infraestrutura.

---

# Restrições

O jogador nunca visualizará uma lista de City Channels.

Toda movimentação deverá respeitar a capacidade máxima da instância de destino.

---

# Referências

Ver também:

- developer-guide/domain/cities.md
- developer-guide/domain/hunt.md