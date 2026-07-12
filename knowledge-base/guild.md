# Guild System

Version: 1.0

Status: LOCKED

Last Sprint: SPRINT-011

---

# Objetivo

Definir o funcionamento oficial do sistema de Guilds do Project-Victory.

GuildService representa a camada social e coletiva do jogo.

---

# Filosofia

No Project-Victory, uma Guild representa união e conquista.

A Guild existe para criar:

- interação social;
- objetivos coletivos;
- reconhecimento;
- progressão em grupo.

---

# Criação de Guild

Requisitos:

- Level mínimo 250;
- Ascension 1.

---

# Participação

Cada personagem pode participar de apenas uma Guild.

---

# Hierarquia

Estrutura oficial:

```
Leader

↓

Vice Leader

↓

Members
```

---

# Leader

Quantidade:

```
1
```

Responsabilidades:

- gerenciamento da Guild;
- decisões administrativas;
- controle de membros.

---

# Vice Leader

Quantidade:

```
5
```

Responsabilidades:

- auxiliar o Leader;
- analisar aplicações;
- administrar atividades da Guild.

---

# Members

Quantidade:

Variável.

O limite depende do Guild Level.

---

# Guild Progression

Guilds possuem Level próprio.

O progresso acontece através de:

- Doações;
- Quests;
- Bosses;
- Eventos;
- Region War.

---

# Doações

Jogadores podem contribuir com:

- Gold;
- Resources;
- Materiais;
- Itens.

---

# Guild Level

O Guild Level influencia:

- quantidade máxima de membros;
- acesso a sistemas;
- benefícios coletivos.

---

# Benefícios

Guild Level pode desbloquear:

- Mais membros;
- Loja própria;
- Bosses exclusivos.

---

# Guild Bank

A Guild possui um banco coletivo.

Pode armazenar:

- Gold;
- Recursos;
- Materiais;
- Itens.

A capacidade aumenta conforme o Guild Level.

---

# Aplicação em Guild

Entrada não ocorre por convite automático.

Fluxo:

```
Jogador

↓

Pesquisa Guild

↓

Visualiza Perfil

↓

Envia aplicação

↓

Leader/Vice Leader analisam

↓

Aceitar ou recusar
```

---

# Perfil da Guild

O jogador poderá visualizar informações públicas.

Exemplos:

- Nome;
- Level;
- Membros;
- Conquistas;
- Histórico;
- Informações relevantes.

---

# Controle Territorial

Sistema futuro.

Guilds poderão controlar:

- Cidades;
- Regiões;
- Fortalezas.

---

# Region War

Sistema futuro de disputa territorial.

Poderá influenciar:

- Progressão da Guild;
- Ranking;
- Reconhecimento.

---

# Guild Boss

Guilds poderão desbloquear Bosses exclusivos.

---

# Arquitetura

Modelo:

```
GuildService

↓

GuildEntity

↓

GuildMemberComponent

↓

GuildBankComponent

↓

GuildLevelComponent

↓

GuildLogComponent

↓

GuildQuestComponent

↓

GuildWarComponent
```

---

# Integrações

GuildService comunica com:

- CharacterService;
- EconomyService;
- InventoryService;
- EventService;
- WorldService.

---

# Pendências futuras

Definir:

- Fórmula de Guild EXP;
- Quantidade máxima final de membros;
- Regras de Leader/Vice Leader;
- Loja da Guild;
- Bosses de Guild;
- Region War.