# ADR-004 - Hunt Independente do Cliente

Status: ACCEPTED

---

# Contexto

O Project-Victory foi concebido como um Idle MMORPG.

A Hunt representa uma atividade contínua executada pela Engine.

O jogador poderá fechar o navegador ou cliente sem interromper imediatamente a Hunt.

Essa decisão garante que o progresso do jogador dependa exclusivamente da Engine e não do estado da aplicação cliente.

---

# Problema

Caso a Hunt dependesse do cliente conectado, ocorreriam diversos problemas:

- interrupção da Hunt ao fechar o navegador;
- diferença de comportamento entre plataformas;
- maior possibilidade de exploração através do cliente;
- maior acoplamento entre Interface e Engine.

Essa abordagem também inviabilizaria o futuro modo Hunt Offline.

---

# Decisão

A Hunt pertence exclusivamente à Engine.

O cliente apenas acompanha seu estado.

Fluxo:

Character

↓

Start Hunt

↓

Engine

↓

Hunt Runtime

↓

Combat

↓

Loot

↓

Experience

↓

Engine State

↓

Client

O cliente nunca controla a execução da Hunt.

---

# Princípios

A Hunt continua existindo independentemente da conexão do jogador.

Fechar o cliente não encerra automaticamente uma Hunt.

A Interface apenas representa o estado atual da Hunt.

Toda lógica permanece no servidor.

---

# Estados

Uma Hunt poderá existir nos seguintes estados conceituais:

Stopped

↓

Starting

↓

Running

↓

Offline

↓

Finished

O estado Offline representa apenas ausência de cliente conectado.

A Hunt continua sendo executada pela Engine conforme as regras definidas para o modo Offline.

---

# Consequências

Benefícios:

- arquitetura desacoplada;
- suporte natural ao modo Offline;
- funcionamento idêntico entre Web e Desktop;
- maior segurança;
- menor possibilidade de exploração.

Custos:

- necessidade de gerenciamento de sessões no servidor;
- sincronização entre cliente e Engine.

Os benefícios justificam plenamente essa decisão.

---

# Impacto

Esta decisão influencia diretamente:

- Hunt
- Combat
- Monsters
- Loot
- Skills
- Character
- Session
- Future Offline Hunt

Todos esses módulos deverão considerar a Hunt como um processo pertencente à Engine.

---

# Restrições

O cliente nunca poderá:

- executar combate;
- gerar Loot;
- criar Monsters;
- controlar Spawn;
- calcular Experience;
- decidir resultados da Hunt.

Essas responsabilidades pertencem exclusivamente à Engine.

---

# Observações

O comportamento específico da Hunt Offline será documentado em módulo próprio quando previsto pelo Roadmap.

Este ADR apenas estabelece que a Hunt é independente do cliente.

---

# Status

Esta decisão é permanente.

Qualquer alteração deverá ser formalizada através de um novo ADR.