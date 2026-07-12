# Project-Victory Engineering Architecture

Version: 1.1

Status: LOCKED

Last Sprint: SPRINT-004

---

# Objetivo

Este documento define como o Project-Victory deve ser desenvolvido.

Ele não define regras do jogo.

Ele define regras de engenharia, organização e desenvolvimento.

---

# Filosofia

O conhecimento do Project-Victory nasce durante reuniões de engenharia.

Nenhuma regra oficial do jogo deverá existir apenas na conversa.

Toda decisão aprovada deverá ser registrada no GitHub.

---

# Estrutura Oficial

Project-Victory/

├── docs/

├── VC-LAB/

├── knowledge-base/

├── engineering/

└── source/

---

# Responsabilidade de cada diretório

## docs/

Documentação auxiliar.

Não representa a fonte oficial das regras do jogo.

---

## VC-LAB/

Ambiente de implementação.

Todo código deverá respeitar a Knowledge Base.

---

## knowledge-base/

Fonte oficial das regras do jogo.

Nenhuma informação poderá ser baseada em hipótese ou dedução.

---

## engineering/

Arquitetura do projeto.

Decisões oficiais.

Histórico de engenharia.

---

## source/

Código-fonte oficial.

---

# Fluxo Oficial

Reunião

↓

Discussão

↓

Decisão

↓

Knowledge Base

↓

VC-LAB

↓

Validação

↓

Nova reunião

---

# Regras de Engenharia

## ENG-001

Nenhum código poderá implementar uma mecânica que ainda não tenha sido aprovada.

---

## ENG-002

A Knowledge Base representa a fonte oficial das regras do jogo.

---

## ENG-003

O GitHub representa a fonte oficial de conhecimento do projeto.

A conversa existe para produzir conhecimento.

---

## ENG-004

Nenhuma decisão oficial permanecerá apenas na conversa.

Toda decisão deverá ser registrada no GitHub.

---

## ENG-005

Nenhuma regra do jogo poderá ser criada por dedução, criatividade ou conhecimento externo.

Toda regra deverá surgir exclusivamente das reuniões oficiais do Project-Victory.

Caso uma informação ainda não tenha sido discutida, ela deverá permanecer como pendência.

---

## ENG-006

Função do ChatGPT.

Transformar decisões aprovadas em:

- Engenharia
- Arquitetura
- Modelagem
- Banco de Dados
- Documentação
- Código

O ChatGPT não define gameplay por iniciativa própria.

---

## ENG-007

Sempre que existirem duas soluções que entreguem exatamente a mesma experiência ao jogador, deverá ser escolhida a solução que apresentar:

- menor custo computacional;
- menor consumo de memória;
- maior facilidade de manutenção;
- maior escalabilidade;
- maior facilidade de expansão.

Alterações de gameplay continuam dependendo de aprovação do Game Design.

---

# Filosofia Idle

O Project-Victory é um Idle MMORPG.

As prioridades de desenvolvimento são:

1. Backend
2. Progressão
3. Economia
4. Estabilidade
5. Escalabilidade

O desenvolvimento visual deve permanecer simples sempre que possível.

---

# Princípios do Projeto

## O jogador deve descobrir.

O sistema não deverá revelar automaticamente todas as melhores estratégias.

A descoberta faz parte da experiência.

---

## O tempo do jogador possui valor.

Nenhuma mecânica deverá desperdiçar tempo do jogador sem oferecer algum tipo de progresso.

---

## Backend acima do Visual.

Sempre que houver conflito entre investimento visual e investimento em sistemas, deverá ser priorizado o desenvolvimento dos sistemas.

---

## Escalabilidade.

Toda arquitetura deverá ser preparada para futuras expansões.

Valores nunca devem ser fixados diretamente no código quando puderem ser parametrizados.

Exemplos:

- Pack Size
- Respawn
- Chances
- Multiplicadores
- Cooldowns

---

# Estado Atual

O projeto encontra-se na fase de Engenharia.

Objetivo atual:

Construção da base necessária para iniciar o desenvolvimento do VC-LAB.

---

# Serviços em Engenharia

CharacterService

InventoryService

ItemService

EquipmentService

WorldService

HuntService

MonsterService

CombatService

LootService

EconomyService

MarketService

SessionService