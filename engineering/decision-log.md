# Project-Victory Decision Log

> Status: ATIVO
>
> Este documento registra todas as decisões oficiais aprovadas durante o desenvolvimento do Project-Victory.
>
> Nenhuma decisão deve ser removida. Caso uma decisão seja substituída, ela deverá permanecer registrada e receber o status SUPERSEDED.

---

# DEC-0001

## Título

Gold não ocupa espaço no inventário.

## Status

LOCKED

## Decisão

Todo gold obtido através de loot será adicionado diretamente ao saldo do jogador.

Não existirá item "Gold Coin".

## Impacto

- Inventory
- Economy
- Loot
- Market

---

# DEC-0002

## Título

Project-Victory não utiliza sistema de Capacity.

## Status

LOCKED

## Decisão

O limite do inventário será definido exclusivamente pela quantidade de slots disponíveis.

Não existirá sistema de peso ou capacidade.

## Impacto

- Inventory
- Character

---

# DEC-0003

## Título

Inventário inicial.

## Status

LOCKED

## Decisão

Todo personagem inicia com 50 slots.

A quantidade poderá ser aumentada futuramente através de VC Points.

## Impacto

- Inventory
- Monetização

---

# DEC-0004

## Título

Mochilas.

## Status

LOCKED

## Decisão

Não existirão mochilas como containers.

O slot Backpack será apenas visual.

Todo loot será armazenado diretamente no inventário principal.

## Impacto

- Inventory
- UI
- Backend

---

# DEC-0005

## Título

Progressão sem perda.

## Status

LOCKED

## Decisão

O personagem nunca perde:

- EXP
- Level

Ao morrer.

## Impacto

- Progression
- Character

---

# DEC-0006

## Título

Ascension.

## Status

LOCKED

## Decisão

Cada Ascension possui limite de Level 500.

Após atingir o limite, o jogador poderá realizar gratuitamente sua Ascension.

Os atributos permanentes serão mantidos.

Novas regiões serão desbloqueadas.

O retorno às regiões antigas continuará permitido.

## Impacto

- Progression
- World

---

# DEC-0007

## Título

Sistema de Food.

## Status

LOCKED

## Decisão

O jogo não possui regeneração natural.

Toda regeneração depende de Food ativa.

## Impacto

- Character
- Combat

---

# DEC-0008

## Título

Boost de EXP.

## Status

LOCKED

## Decisão

O tempo do Boost somente começa a ser consumido após o jogador obter experiência.

O tempo continua correndo normalmente até terminar.

## Impacto

- Progression
- Premium

---

# DEC-0009

## Título

Sistema de raridade.

## Status

LOCKED

## Decisão

Raridades oficiais:

- Normal
- Magic
- Rare

Categoria Especial:

- Unique

## Impacto

- Item
- Loot
- Market

---

# DEC-0010

## Título

Sistema de afixos.

## Status

LOCKED

## Decisão

Normal

- nenhum afixo

Magic

- exatamente 1 afixo

Rare

- exatamente 2 ou 3 afixos

Unique

- atributos fixos

## Impacto

- Item
- Equipment

---

# DEC-0011

## Título

Upgrade.

## Status

LOCKED

## Decisão

Somente equipamentos Rare poderão receber upgrade até +14.

Unique não recebe upgrade.

## Impacto

- Equipment
- Economy

---

# DEC-0012

## Título

Durabilidade.

## Status

LOCKED

## Decisão

Equipamentos possuem durabilidade.

Nunca quebram.

Nunca ficam abaixo de 50%.

A durabilidade diminui apenas ao morrer.

O reparo é realizado através de NPC.

## Impacto

- Equipment

---

# DEC-0013

## Título

Filosofia dos equipamentos.

## Status

LOCKED

## Decisão

Equipamentos representam uma das maiores fontes de poder do personagem.

Itens extremamente raros e totalmente evoluídos devem representar uma conquista de longo prazo.

## Impacto

- Equipment
- Combat
- Economy

---

# DEC-0014

## Título

Filosofia Idle.

## Status

LOCKED

## Decisão

Project-Victory é um Idle MMORPG.

As decisões de engenharia devem priorizar:

- backend
- estabilidade
- progressão
- economia

O desenvolvimento de animações complexas não possui prioridade.

## Impacto

Projeto inteiro.

---

# DEC-0015

## Título

Knowledge Base.

## Status

LOCKED

## Decisão

Nenhuma regra oficial poderá existir apenas na conversa.

Toda decisão deverá ser registrada no GitHub.

## Impacto

Projeto inteiro.

---

# DEC-0016

## Título

Prioridade visual das Hunts.

## Origem

HS-001 — HuntService

## Status

LOCKED

## Decisão

As Hunts possuem prioridade mínima de desenvolvimento visual.

O foco principal do Project-Victory está em:

- Backend
- Progressão
- Economia
- Estabilidade

Elementos visuais servem apenas para contextualizar a Hunt.

---

# DEC-0017

## Título

Pack Size configurável.

## Origem

HS-001 — HuntService

## Status

LOCKED

## Decisão

Toda Hunt possui um Pack Size configurável.

Valor padrão aprovado:

Pack Size = 3

O valor nunca deverá ser fixo no código.

---

# DEC-0018

## Título

Monster Pool.

## Origem

HS-001 — HuntService

## Status

LOCKED

## Decisão

Cada Hunt possui uma Monster Pool própria.

Exemplo:

Dragon

Dragon Elite

Cada entrada possui sua própria Chance de Spawn.

Monstros Elite possuem limite inicial de um exemplar por Pack.

Eventos poderão alterar temporariamente a Monster Pool sem necessidade de criar novas Hunts.

---

# DEC-0019

## Título

Loot pertence ao Monstro.

## Origem

HS-001 — HuntService

## Status

LOCKED

## Decisão

O Loot pertence exclusivamente ao Monstro.

As Hunts não possuem Loot próprio.

---

# DEC-0020

## Título

Respawn das Waves.

## Origem

HS-001 — HuntService

## Status

LOCKED

## Decisão

Após a morte do último monstro do Pack.

O servidor aguarda:

1 segundo

Então gera um novo Pack completo.

O tempo deverá ser configurável.

---

# DEC-0021

## Título

Instâncias de Hunt.

## Origem

HS-001 — HuntService

## Status

LOCKED

## Decisão

Cada jogador possui sua própria instância de Hunt.

A instância é criada ao entrar na Hunt.

A instância é destruída quando:

- jogador sai;
- jogador morre;
- sessão é encerrada.

---

# DEC-0022

## Título

Troca de Hunt.

## Origem

HS-001 — HuntService

## Status

LOCKED

## Decisão

O jogador pode trocar livremente de Hunt.

Cooldown inicial:

5 segundos.

O servidor poderá aumentar temporariamente este tempo como mecanismo de proteção contra abuso.

---

# DEC-0023

## Título

Modo Offline.

## Origem

HS-001 — HuntService

## Status

LOCKED

## Decisão

Ao fechar o navegador ou perder a conexão.

O personagem entra em modo Offline.

Durante o modo Offline:

- Não recebe Loot.
- Recebe EXP reduzida.
- Recebe Gold reduzido.

Os valores deverão permanecer configuráveis pelo servidor.

---

# DEC-0024

## Título

Filosofia de Descoberta.

## Origem

HS-001 — HuntService

## Status

LOCKED

## Decisão

O jogo não deverá informar automaticamente quais Hunts são melhores para:

- EXP
- Loot
- Recursos

Essas informações deverão ser descobertas pela comunidade.

---

# ENG-007

## Título

Escalabilidade acima da implementação imediata.

## Status

LOCKED

## Regra

Sempre que existirem duas soluções que entreguem a mesma experiência ao jogador.

Deverá ser escolhida a arquitetura:

- mais escalável;
- menor custo computacional;
- menor consumo de memória;
- preparada para futuras expansões.

Qualquer alteração de gameplay continua dependendo de aprovação do Game Design.
---

# DEC-0025

## Título

Combat baseado em turnos fixos.

## Origem

CS-001 — CombatService

## Status

LOCKED

## Decisão

O combate utiliza turnos fixos.

Attack Speed e Cast Speed não existem inicialmente.

A velocidade de ataque não influencia o combate.

O balanceamento será realizado através de:

- Level;
- Skills;
- Equipamentos;
- Afixos;
- Imbuements;
- Spells.

---

# DEC-0026

## Título

Ordem do turno de combate.

## Origem

CS-001 — CombatService

## Status

LOCKED

## Decisão

O ciclo de combate segue:

1. Jogador realiza ação.
2. Jogador causa dano.
3. Monstros sobreviventes realizam ataque.
4. Novo turno inicia.

O jogador sempre possui prioridade de ação.

---

# DEC-0027

## Título

Seleção de alvo.

## Origem

CS-001 — CombatService

## Status

LOCKED

## Decisão

O personagem ataca sempre o primeiro monstro da Pack List.

Quando o monstro é derrotado, o próximo monstro da lista torna-se o alvo automaticamente.

---

# DEC-0028

## Título

Área das Skills.

## Origem

CS-001 — CombatService

## Status

LOCKED

## Decisão

Skills de área possuem comportamento definido individualmente.

Cada Skill determina sua quantidade de alvos atingidos.

Uma Skill pode atingir:

- quantidade fixa de monstros;
- todos os monstros do Pack.

---

# DEC-0029

## Título

Estrutura do CombatService.

## Origem

CS-001 — CombatService

## Status

LOCKED

## Decisão

O CombatService será dividido em responsabilidades menores.

Arquitetura:

CombatService

↓

TargetService

↓

DamageCalculator

↓

CriticalCalculator

↓

DeathService

↓

LootService
