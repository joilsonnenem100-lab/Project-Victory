# ADR-004 - Hunt Independente do Cliente

Status: ACCEPTED

Version: 1.0

---

# Contexto

O Project-Victory foi projetado para funcionar como um Idle MMORPG.

A Hunt é executada pela Engine e não depende da permanência do jogador conectado.

Esta decisão garante funcionamento consistente entre diferentes clientes e prepara a arquitetura para o futuro modo Hunt Offline.

---

# Decisão

A Hunt pertence exclusivamente à Engine.

O cliente apenas inicia, acompanha e apresenta seu estado.

Fechar o cliente não representa o encerramento imediato da Hunt.

Toda lógica de execução permanece na Engine.

---

# Consequências

Benefícios:

- desacoplamento entre cliente e servidor;
- suporte ao futuro Hunt Offline;
- maior segurança;
- funcionamento consistente entre plataformas;
- menor possibilidade de exploração.

Custos:

- necessidade de sincronização entre cliente e Engine.

---

# Aplicações

Este princípio aplica-se aos módulos:

- Hunt;
- Combat;
- Monsters;
- Loot;
- Character;
- Session.

---

# Restrições

O cliente nunca poderá:

- executar Combat;
- controlar Spawn;
- criar Monsters;
- calcular Loot;
- calcular Experience;
- decidir resultados da Hunt.

Toda regra de negócio pertence exclusivamente à Engine.

---

# Referências

Ver também:

- developer-guide/domain/monsters.md
- developer-guide/domain/combat.md