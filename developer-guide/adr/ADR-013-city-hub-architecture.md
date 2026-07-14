# ADR-013 - City Hub Architecture

Status: ACCEPTED

Version: 1.0

---

# Contexto

O Project-Victory concentra toda a interação social fora das Hunts.

Era necessário definir um ponto central para preparação, economia e interação entre jogadores.

---

# Decisão

A Cidade Principal será o Hub permanente do jogo.

Todo Character que não estiver em uma Hunt ativa permanecerá nela.

A Cidade concentra todos os sistemas permanentes da Engine.

---

# Consequências

Benefícios:

- simplificação da navegação;
- fortalecimento da interação social;
- valorização dos cosméticos;
- redução de deslocamentos desnecessários;
- centralização dos serviços.

Custos:

- necessidade de suportar grande concentração de jogadores.

---

# Aplicações

Este princípio aplica-se a:

- Cities;
- Hunt;
- Character;
- Market;
- Guilds;
- Party;
- Sistemas sociais futuros.

---

# Restrições

A Cidade nunca executará mecânicas de combate.

Toda atividade de gameplay ocorre exclusivamente durante uma Hunt.

---

# Referências

Ver também:

- developer-guide/domain/cities.md
- developer-guide/domain/hunt.md