# Cities

Status: DRAFT

Version: 1.0

---

# Objetivo

Definir o contrato arquitetural do módulo Cities.

---

# Responsabilidade

O módulo Cities é responsável exclusivamente por representar as cidades permanentes do mundo do Project-Victory.

As cidades representam ambientes sociais e de preparação para o gameplay.

---

# Responsabilidades

O módulo Cities é responsável por:

- representar a Cidade Principal;
- disponibilizar os serviços permanentes do jogo;
- hospedar os jogadores quando não estiverem em uma Hunt;
- coordenar a distribuição transparente entre City Channels.

---

# Dependências

O módulo Cities poderá depender apenas de:

- Character;
- City Channel;
- Social Services;
- Hunt.

---

# Proibições

O módulo Cities não poderá:

- executar combate;
- controlar Spawn;
- controlar Monster AI;
- distribuir Loot;
- distribuir Experience;
- conhecer Combat Engine.

---

# Cidade Principal

Inicialmente existirá apenas uma Cidade Principal.

Ela representa o Hub permanente do jogo.

Todo Character que não estiver em uma Hunt ativa permanecerá na Cidade Principal.

---

# City Channels

A Cidade Principal poderá possuir múltiplas instâncias de execução (City Channels).

Essas instâncias existem exclusivamente para fins de escalabilidade.

A infraestrutura é responsável por:

- criar novas instâncias quando necessário;
- destruir instâncias vazias;
- distribuir jogadores automaticamente.

O jogador nunca escolhe manualmente um City Channel.

---

# Capacidade

Configuração inicial:

Max Players Per City Channel = 50

Quando uma instância atingir sua capacidade máxima, uma nova instância poderá ser criada automaticamente.

---

# Movimentação Social

O jogador poderá utilizar funcionalidades sociais para reunir-se com outros jogadores.

Primeira funcionalidade oficial:

Ir até o amigo

Fluxo:

VIP List

↓

Selecionar amigo

↓

Ir até o amigo

↓

Infraestrutura verifica disponibilidade

↓

Transferência transparente

Caso não exista capacidade disponível, a movimentação será recusada.

---

# Serviços

A Cidade Principal representa o Hub Central do Project-Victory.

Os serviços permanentes do jogo deverão estar acessíveis a partir dela.

Exemplos:

- Seleção de Hunts;
- NPCs;
- Market;
- Crafting;
- Guilds;
- Party;
- Pets;
- Companions;
- Diário de Monstros;
- Eventos;
- Sistemas futuros.

---

# Regras

Cada Character pertence a apenas um City Channel por vez.

Todo login ocorre na Cidade Principal.

Todo encerramento de Hunt retorna para a Cidade Principal.

Os City Channels são invisíveis para o jogador.

Toda movimentação entre City Channels ocorre através da infraestrutura.

---

# Filosofia

A Cidade Principal representa o centro social do jogo.

A infraestrutura trabalha de forma transparente.

O jogador percebe apenas um único mundo compartilhado.

A Cidade concentra preparação, interação social, economia e customização.

---

# Referências

Ver também:

- hunt.md
- character.md
- market.md
- guilds.md
- party.md