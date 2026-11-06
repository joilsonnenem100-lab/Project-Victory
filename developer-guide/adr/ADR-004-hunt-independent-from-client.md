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

---

# Nota de emenda (2026-11-06)

Ver `ADR-057` (ACCEPTED) e `decision-log.md` (`DEC-20261106-05`, decisão do Product Owner): ao **desconectar**, a Hunt **termina** pelo pipeline único (ADR-012); com o jogador offline a Hunt não rende nada. Por isso, a frase do Contexto "não depende da permanência do jogador conectado" e o "futuro modo Hunt Offline" **deixam de valer para a desconexão do jogador**. Continua valendo: a Hunt pertence exclusivamente à Engine e roda no servidor enquanto o jogador está conectado. "Fechar o cliente não representa o encerramento imediato da Hunt" só se mantém se o PO aprovar uma **janela de reconexão** (dúvida 6 do M6 em `roadmap-to-playable.md`); sem ela, fechar o cliente encerra a sessão do jogador e, com ela, a Hunt.
