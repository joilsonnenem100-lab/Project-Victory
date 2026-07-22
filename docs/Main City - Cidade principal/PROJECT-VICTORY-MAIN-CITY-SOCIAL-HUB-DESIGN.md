# PROJECT-VICTORY-MAIN-CITY-SOCIAL-HUB-DESIGN

## Visão da Cidade Principal e Camada Social

Status:
- Visão de produto registrada
- Arquitetura futura
- Não representa implementação concluída

---

## 1. Conceito

A Cidade Principal é o hub central permanente do jogador.

Ela representa:

- retorno do personagem;
- preparação;
- interação social;
- comércio;
- comunidade;
- ligação entre atividades.

Fluxo:

Account
↓
Character
↓
Main City
↓
Preparação
↓
Exploração
↓
Hunts
↓
Rewards
↓
Retorno à Cidade

---

## 2. Papel na Arquitetura

Relação futura:

Account
↓
Game Session
↓
Character State
↓
Runtime Environment
↓
City Runtime
↓
Social Systems

---

## 3. City Channels

A cidade deve utilizar instâncias transparentes.

Fluxo:

Entrada do jogador
↓
Channel Manager
↓
Encontrar instância disponível
↓
Carregar ambiente
↓
Jogador entra na cidade

O jogador não escolhe canais manualmente.

---

## 4. Sistemas Sociais

A cidade será base para:

- Party System
- Guild System
- Market
- Eventos
- Interação entre jogadores

---

## 5. Distritos Conceituais

Possíveis áreas:

- Central Plaza
- Market District
- Guild District
- Training District
- Crafting District
- Residential District
- Portal District

---

## 6. NPCs

Modelo futuro:

NPC State

- Identidade
- Localização
- Serviços
- Agenda
- Relacionamentos

---

## 7. Relação com Gameplay Loop

Cidade

↓
Preparação

↓
Conteúdo

↓
Hunt

↓
Combat

↓
Reward

↓
Progressão

↓
Retorno

---

## 8. Estado Atual do Projeto

Fundação existente:

- Account
- Authentication
- Character
- Hunt Foundation
- Monster Foundation
- Combat Foundation
- Persistence

Ainda futuro:

- Game Session
- Runtime Environment
- Runtime State
- City Runtime
- Social Systems

---

## 9. Direção

A Cidade Principal deve ser o centro de identidade do jogador e representar a transição entre executar sistemas e viver em um mundo persistente.

