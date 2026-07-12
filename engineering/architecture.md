# Project-Victory Engineering Architecture

> Status: LOCKED
>
> Última atualização: 2026-07-12

---

# Objetivo

Este documento define como o Project-Victory deve ser desenvolvido.

Ele não contém regras do jogo.

Ele contém apenas regras de engenharia, organização e desenvolvimento do projeto.

---

# Filosofia

O conhecimento do Project-Victory é produzido através de reuniões de engenharia.

Nenhuma regra oficial do jogo deve existir apenas na conversa.

Toda decisão aprovada deverá ser registrada no repositório oficial do projeto.

---

# Estrutura Oficial do Projeto

Project-Victory/

├── docs/

├── VC-LAB/

├── knowledge-base/

├── engineering/

└── source/

---

# Função de cada diretório

## docs/

Documentação auxiliar do projeto.

Não representa a fonte oficial das regras do jogo.

---

## VC-LAB/

Ambiente de implementação e experimentação.

Todo código deverá respeitar as decisões registradas na Knowledge Base.

---

## knowledge-base/

Representa a fonte oficial das regras do jogo.

Todos os sistemas aprovados deverão ser registrados aqui.

Nenhuma informação deste diretório poderá ser baseada em suposição.

---

## engineering/

Contém as decisões de engenharia do projeto.

Também registra arquitetura e histórico das decisões oficiais.

---

## source/

Código-fonte oficial do Project-Victory.

---

# Fluxo Oficial de Desenvolvimento

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

# Regras Oficiais de Engenharia

## ENG-001

Nenhum código poderá implementar uma mecânica que ainda não tenha sido aprovada durante as reuniões de engenharia.

---

## ENG-002

A Knowledge Base representa a fonte oficial das regras do jogo.

---

## ENG-003

O GitHub representa a fonte oficial de conhecimento do projeto.

A conversa serve apenas para produzir conhecimento.

---

## ENG-004

Nenhuma decisão oficial permanecerá apenas na conversa.

Toda decisão aprovada deverá ser registrada no GitHub.

---

## ENG-005

Nenhuma informação será adicionada à Knowledge Base baseada em dedução, criatividade ou conhecimento externo.

Toda regra deverá surgir exclusivamente das reuniões oficiais do Project-Victory.

Caso uma informação ainda não tenha sido discutida, ela deverá permanecer como pendência.

---

## ENG-006

Meu papel (ChatGPT) é transformar as decisões do projeto em engenharia, arquitetura, documentação técnica e implementação.

Meu papel não é criar regras do jogo.

---

# Situação Atual

Projeto em fase de Engenharia.

Objetivo atual:

Construção da base necessária para iniciar o desenvolvimento do VC-LAB.

---

# Status

CharacterService
Em definição avançada.

InventoryService
Em definição avançada.

ItemService
Em definição avançada.

EquipmentService
Em definição avançada.

Demais sistemas permanecem em engenharia.