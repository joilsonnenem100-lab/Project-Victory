# ADR-017 - NPC Service Facade

Status: ACCEPTED

Version: 1.0

---

# Contexto

Os NPCs representam a principal forma de interação do jogador com os sistemas permanentes do mundo.

Era necessário impedir que regras de negócio fossem implementadas diretamente nos NPCs.

---

# Decisão

Todo NPC atuará exclusivamente como fachada para um serviço da aplicação.

Fluxo oficial:

Character

↓

NPC

↓

Application Service

↓

Domain

Nenhuma regra de negócio será implementada no NPC.

---

# Consequências

Benefícios:

- baixo acoplamento;
- reutilização dos serviços;
- facilidade para testes;
- expansão simples;
- manutenção facilitada.

Custos:

- necessidade de serviços bem definidos.

---

# Aplicações

Este princípio aplica-se a:

- NPCs;
- Cities;
- Application Services;
- Domain Services.

---

# Restrições

NPCs nunca poderão:

- executar regras de negócio;
- acessar persistência diretamente;
- conhecer detalhes da Combat Engine.

---

# Referências

Ver também:

- developer-guide/domain/npcs.md
- developer-guide/domain/cities.md