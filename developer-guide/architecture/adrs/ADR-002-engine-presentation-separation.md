# ADR-002 - Separação entre Engine e Presentation Layer

Status: ACCEPTED

---

# Contexto

O Project-Victory é um Idle MMORPG.

O combate é totalmente processado pela Engine.

A interface do jogador existe exclusivamente para representar visualmente o estado produzido pela Engine.

Essa decisão foi tomada para permitir que o jogo continue funcionando independentemente do cliente utilizado.

---

# Problema

Misturar lógica de combate com interface gera diversos problemas:

- dependência do cliente;
- dificuldade para criar novos clientes;
- maior acoplamento;
- maior risco de exploração;
- dificuldade para executar Hunts Offline.

Além disso, a apresentação visual deixa de ser livre para evoluir.

---

# Decisão

A Engine e a Presentation Layer são módulos independentes.

A Engine calcula todo o combate.

A Presentation Layer apenas representa o resultado produzido pela Engine.

Nunca haverá lógica de combate na camada de apresentação.

Fluxo:

Engine

↓

Combat Result

↓

Presentation Layer

↓

Visual Simulation

---

# Princípios

A Engine é a única fonte de verdade.

A Presentation Layer nunca altera estados da Engine.

Toda animação exibida ao jogador representa uma simulação visual.

A apresentação não precisa reproduzir exatamente cada cálculo interno da Engine.

O objetivo da camada visual é melhorar a experiência do jogador sem influenciar a lógica do jogo.

---

# Consequências

Benefícios:

- desacoplamento completo;
- possibilidade de múltiplos clientes;
- suporte natural ao modo Offline;
- maior segurança;
- menor risco de exploits;
- liberdade para evolução visual.

Custos:

- necessidade de sincronização entre Engine e Interface;
- camada adicional de comunicação.

Os benefícios justificam plenamente essa separação.

---

# Aplicações

Este princípio deverá ser utilizado por todos os módulos que possuam representação visual.

Exemplos:

- Combat
- Hunts
- Monsters
- Skills
- Effects
- UI

---

# Impacto na arquitetura

Esta decisão permite que:

- Web Client;
- Desktop Client;
- Mobile Client;

compartilhem exatamente a mesma Engine.

Também permite futuramente criar diferentes estilos de apresentação sem alterar qualquer regra de domínio.

---

# Restrições

A Presentation Layer nunca poderá:

- calcular dano;
- alterar atributos;
- controlar Hunts;
- gerar Loot;
- criar Monsters;
- executar Skills.

Toda lógica pertence exclusivamente à Engine.

---

# Status

Esta decisão é permanente.

Qualquer alteração deverá ser aprovada através de novo ADR.