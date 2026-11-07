# World System

Version: 1.1

Status: EM DESENVOLVIMENTO

Last Sprint: SPRINT-004

---

# Objetivo

Definir a arquitetura oficial do mundo do Project-Victory.

Este documento contém apenas decisões aprovadas.

---

# Filosofia

O Project-Victory é um Idle MMORPG.

O mundo existe para suportar a progressão do jogador.

As Hunts representam o principal conteúdo do jogo.

O desenvolvimento do mundo prioriza:

- simplicidade;
- escalabilidade;
- desempenho;
- expansão futura.

---

# Estrutura Oficial

Servidor

↓

Continentes

↓

Regiões

↓

Hunts

↓

Packs

↓

Monstros

---

# Continentes

> *Emenda (2026-11-06, Fila 5, `DEC-20261106-30`; revisão do PV-GOV-010): além do bloqueio por Continente, cada **Hunt area** passa a exigir um **tier de título** mínimo (0 = Novice … 5 = Legend, derivado do Level; Spider Cave = 0). "Tier de título" (`TitleTier`, `RequiredTitleTier` no código) é **outro conceito** do "Tier" de item: o Tier de item continua pertencendo só aos itens. Onde abaixo se lê "Ascension mínima/recomendada", leia o tier de título.*

> *Nota de superação (2026-11-07, `DEC-20261107-02`, PO-09/PO-55 em `docs/product/PO-DECISOES-BASE.md`): a Hunt area **não exige** tier de título: `RequiredTitleTier` passa a ser **nível recomendado** (informativo), sem bloquear a entrada. O bloqueio por Continente fica pendente de confirmação do PO (PO-09 diz "regiões livres"; ver relatório da Fila 9).* *(Confirmado em 2026-11-07, PO-56: Continente livre.)*

Cada Continente possui uma Ascension mínima para acesso.

> *Nota de superação (2026-11-07, `DEC-20261107-05`, PO-56 em `docs/product/PO-DECISOES-BASE.md`): o Continente é **livre**: não há Ascension mínima para acesso nem bloqueio no Continente; o nível/tier recomendado é **só informativo**, no Continente e na Hunt.*

O bloqueio acontece no Continente.

As Hunts apenas possuem uma Ascension recomendada.

---

# Regiões

Cada Continente é composto por diversas Regiões.

Cada Região possui:

- Hunts;
- Boss relacionado;
- recursos próprios;
- identidade temática.

---

# Hunts

Uma Hunt representa uma experiência contínua de evolução.

Cada Hunt possui:

- ID interno;
- Nome interno;
- Nome exibido;
- Ascension recomendada;
- Monster Pool;
- Loot obtido através dos monstros;
- Boss relacionado;
- Eventos possíveis.

A Hunt não possui Tier.

Tier pertence exclusivamente aos itens.

> *Emenda (Fila 5, `DEC-20261106-30`): a Hunt continua sem Tier **de item**, mas exige um **tier de título** mínimo (`RequiredTitleTier`).*

> *Nota de superação (2026-11-07, `DEC-20261107-02`, PO-55 em `docs/product/PO-DECISOES-BASE.md`): o `RequiredTitleTier` é informativo, não exigência.*

---

# Instâncias

Cada jogador possui sua própria instância de Hunt.

A instância é criada quando o jogador entra.

A instância é destruída quando:

- o jogador sai;
- o jogador morre;
- a sessão termina.

---

# Sistema de Waves

Cada Hunt gera Packs continuamente.

O funcionamento oficial é:

Pack

↓

Todos os monstros derrotados

↓

Espera de 1 segundo

↓

Novo Pack

↓

Repetição infinita

O tempo de respawn deverá permanecer configurável.

---

# Packs

Valor padrão aprovado:

Pack Size = 3

O tamanho do Pack nunca deverá ser fixado no código.

Cada Hunt poderá possuir configurações próprias no futuro.

---

# Monster Pool

Cada Hunt possui sua própria Monster Pool.

Exemplo:

Dragon

Dragon Elite

Cada monstro possui sua própria chance de Spawn.

Monstros Elite possuem limite inicial de um exemplar por Pack.

Eventos poderão alterar temporariamente a Monster Pool.

---

# Bosses

Bosses nunca aparecem dentro das Hunts.

Cada Boss possui sua própria Boss Room.

O acesso ocorre através da Boss Key correspondente.

---

# Objetivo das Hunts

Cada Hunt pode possuir um ou mais objetivos.

Exemplos:

- Evolução
- Recursos
- Craft
- Itens específicos

O sistema não informa automaticamente qual Hunt é a mais eficiente.

A descoberta faz parte da experiência do jogador.

---

# Interface

A interface da Hunt possui prioridade mínima de desenvolvimento.

O foco principal está nas informações úteis ao jogador.

Representações visuais servem apenas como contextualização da região.

---

# Troca de Hunt

O jogador pode trocar livremente de Hunt.

Cooldown inicial:

5 segundos.

O servidor poderá aumentar temporariamente este tempo como mecanismo de proteção.

---

# Pendências

Ainda precisam ser definidos:

- Estrutura completa dos Continentes.
- Estrutura das Regiões.
- Boss Rooms.
- Eventos de Hunt.
- Interface visual das Hunts.
- Algoritmo de Spawn Controller.

---

# Decisões Relacionadas

DEC-0006

DEC-0014

DEC-0016

DEC-0017

DEC-0018

DEC-0020

DEC-0021

DEC-0022

DEC-0024