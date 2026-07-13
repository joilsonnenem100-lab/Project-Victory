# Ubiquitous Language

Status: LOCKED

Version: 1.0

---

# Objetivo

Este documento define a linguagem oficial do domínio do Project-Victory.

Todos os documentos, código-fonte e discussões técnicas devem utilizar exclusivamente os termos definidos aqui.

É proibido criar sinônimos para os conceitos oficiais do domínio.

---

# Account

Representa o usuário do jogo.

Responsabilidades:

- autenticação;
- gerenciamento de personagens;
- configurações da conta.

A Account nunca participa do gameplay.

---

# Character

Representa um personagem jogável.

É o principal Aggregate Root do domínio.

Responsabilidades:

- atributos;
- inventário;
- equipamentos;
- progressão;
- skills;
- estado do personagem.

Todo progresso do jogador pertence ao Character.

---

# World

Representa o mundo persistente do jogo.

Responsável por:

- mapas;
- regiões;
- configuração do mundo;
- definição de spawns.

O World não executa gameplay.

---

# Runtime

Representa a simulação do mundo em execução.

Responsável por:

- ticks;
- IA;
- movimentação;
- combate;
- processamento de eventos;
- atualização do mundo.

O Runtime nunca persiste dados.

---

# Monster

Entidade controlada pela Engine.

Pode:

- mover;
- atacar;
- morrer;
- gerar loot.

---

# NPC

Entidade interativa.

Pode oferecer:

- diálogos;
- comércio;
- quests;
- serviços.

---

# Item

Representa um objeto existente no jogo.

Um Item não executa regras complexas.

As regras pertencem ao domínio.

---

# Inventory

Coleção organizada de Items pertencente ao Character.

Inventory nunca existe isoladamente.

---

# Equipment

Representa os equipamentos atualmente utilizados pelo Character.

É parte do Character.

Não possui identidade própria.

---

# Loot

Representa o resultado da geração de recompensas.

Loot não é Item.

Loot gera Items.

---

# Skill

Representa uma habilidade do Character.

Possui evolução própria.

---

# Quest

Representa uma missão.

Possui:

- objetivos;
- progresso;
- estado;
- recompensas.

---

# Region

Representa uma área lógica do World.

Agrupa:

- mapas;
- spawns;
- eventos.

---

# Map

Representa um espaço navegável.

Não contém regras de gameplay.

---

# Spawn

Responsável por criar entidades no Runtime.

Não pertence ao Character.

---

# Combat

Representa o processamento de uma batalha.

Combat pertence ao Runtime.

Nunca ao Character.

---

# Domain Event

Representa acontecimentos importantes do domínio.

Exemplos:

- CharacterCreated
- CharacterDied
- MonsterKilled
- LootDropped
- QuestCompleted

---

# Regras

É obrigatório utilizar exatamente estes termos.

Não criar sinônimos.

Não alterar definições sem uma DEC.

---

# Filosofia

A linguagem do domínio deve permanecer estável durante toda a vida do Project-Victory.

Ela representa a linguagem oficial entre Arquitetura, Desenvolvimento e Produto.