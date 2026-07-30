# PV-CORE-GAMEPLAY-LOOP-PRIORITY-IDLE-RPG

# Decisão de Direção do Projeto

## Objetivo atual

O Project-Victory deve priorizar a construção do Core Gameplay Loop de um Idle RPG de navegador.

O objetivo desta fase não é construir um MMORPG complexo.

A prioridade é criar uma experiência simples, repetível e funcional:

Matar inimigo
↓
Receber recompensa
↓
Evoluir personagem
↓
Ficar mais forte
↓
Repetir

---

# Core Gameplay Loop prioritário

Fluxo esperado:

Player
↓
Escolher Hunt
↓
Encounter criado
↓
Combat resolvido
↓
Resultado aplicado
↓
XP recebida
↓
Evolução
↓
Loot recebido
↓
Item no Inventário
↓
Nova Hunt

---

# Modelo de combate

O combate deve ser tratado inicialmente como uma resolução de gameplay.

CombatContext representa:

- dados necessários para resolver a batalha;
- informações temporárias da execução;
- resultado do combate.

CombatContext NÃO representa:

- uma batalha persistente;
- uma sessão independente;
- uma atividade que precisa ser recuperada após logout.

---

# Fluxo completo esperado

Login
↓
Character
↓
Runtime
↓
Location
↓
Escolher Hunt
↓
Criar Encounter
↓
Criar CombatContext temporário
↓
Resolver combate
↓
Vitória ou derrota
↓
Reward
↓
XP
↓
Loot
↓
Inventory
↓
Fim da Hunt

---

# Sistemas fora da prioridade atual

Não devem bloquear o Core Gameplay Loop:

- Guilds
- Marketplace
- Store
- Economia avançada
- Sistemas sociais
- Clans
- Ranking
- PvP
- Sistemas de MMO

---

# Evitar complexidade prematura

Não criar neste momento:

- recuperação de combate após logout;
- persistência de batalha;
- histórico complexo de combate;
- sistemas de sessão de combate;
- infraestrutura para cenários que não fazem parte da experiência atual.

---

# Regra para novos contratos

Antes de criar um contrato, avaliar:

1. Isso aproxima o jogador do loop de matar, evoluir e repetir?

2. Isso melhora a experiência principal do Idle RPG?

3. Isso desbloqueia o primeiro Vertical Slice?

Caso contrário, avaliar adiamento.

---

# Arquitetura desejada

Character:

Estado permanente do jogador.


Runtime:

Estado temporário da sessão.


Encounter:

Situação criada pelo gameplay.


Combat:

Resolução temporária da batalha.


Reward:

Consequência do resultado.

---

# Critério de sucesso desta fase

O Project-Victory deve provar:

"Um jogador consegue iniciar uma Hunt, vencer uma batalha, receber uma recompensa, evoluir e repetir o ciclo."

---

# Local recomendado

docs/architecture/decisions/PV-CORE-GAMEPLAY-LOOP-PRIORITY-IDLE-RPG.md
