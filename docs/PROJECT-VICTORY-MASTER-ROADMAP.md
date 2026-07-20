# PROJECT-VICTORY-MASTER-ROADMAP

## Identidade do Projeto

Project-Victory é um jogo online inspirado em MMORPGs, construído com evolução incremental, arquitetura modular e desenvolvimento baseado em contratos.

O objetivo é criar uma experiência completa onde o jogador possa:

- Criar uma conta.
- Criar um personagem.
- Entrar no mundo.
- Executar hunts.
- Evoluir.
- Desbloquear conteúdo.
- Participar de uma progressão contínua.

---

# Filosofia de Desenvolvimento

O projeto segue:

Arquitetura

↓

Contrato

↓

Engenharia

↓

Implementação

↓

Teste

↓

Validação

↓

Aceite

---

Nenhum sistema deve ser criado sem:

- definição arquitetural;
- escopo;
- critérios de validação.

---

# Organização do Projeto

## Arquitetura

Responsável por:

- visão técnica;
- decisões;
- contratos;
- roadmap;
- controle de escopo.

---

## Engenharia

Responsável por:

- implementação;
- código;
- testes;
- entregas.

---

# Estado Geral do Projeto

## FASE 0 — Fundação Arquitetural

Status:

CONCLUÍDA

Objetivo:

Criar a base organizacional e técnica.

Entregas:

- Arquitetura inicial.
- Separação de responsabilidades.
- Modelo de contratos.
- Processo de desenvolvimento.

---

# FASE 1 — Vertical Slice 2.0

Status:

CONCLUÍDO

Objetivo:

Criar o primeiro fluxo completo funcional do jogo.

---

## Account Persistence Foundation

PV-VS-2.0-008

Status:

CONCLUÍDO

Entregas:

- Account.
- Persistência.
- SQLite.
- EF Core.
- Recuperação após reinicialização.

---

## Authentication Flow

PV-VS-2.0-009

Status:

CONCLUÍDO

Entregas:

- Login.
- JWT.
- Contexto autenticado.
- Tratamento de falhas.

---

## Character Persistence Foundation

PV-VS-2.0-010

Status:

CONCLUÍDO

Entregas:

- Character.
- Persistência.
- Recuperação.

---

## Hunt Runtime Flow

PV-VS-2.0-011

Status:

CONCLUÍDO

Entregas:

- Seleção de Hunt.
- Estado de Hunt.
- Execução.
- Resultado.

---

## Monster Data Foundation

PV-VS-2.0-012

Status:

CONCLUÍDO

Entregas:

- Monster.
- Persistência.
- Dados básicos.

---

## Combat Resolution

PV-VS-2.0-013

Status:

CONCLUÍDO

Entregas:

- CombatContext.
- Resolução.
- Resultado.

---

## Item Definition

PV-VS-2.0-014

Status:

CONCLUÍDO

Entregas:

- ItemDefinition.
- ItemCharacteristics.
- Persistência.

---

## Player Loop Validation

PV-VS-2.0-015

Status:

CONCLUÍDO

Fluxo validado:

Account

↓

Authentication

↓

Character

↓

Game Entry

↓

Hunt

↓

Monster

↓

Combat

↓

Result

---

## Final Acceptance

PV-VS-2.0-045

Status:

CONCLUÍDO

Resultado:

Vertical Slice 2.0 aprovado.

---

# FASE 2 — Web Entry Experience

Status:

EM ANDAMENTO

Objetivo:

Transformar o sistema funcional em uma experiência acessível ao jogador.

---

# UPDATE-001 — Web Entry Foundation

Status:

CONCLUÍDO

Objetivo:

Disponibilizar acesso pelo navegador.

Resultado:

- ProjectVictory.Client identificado.
- React + Vite funcionando.
- Browser carregando aplicação.

---

# UPDATE-002 — Web Account Registration Flow

Status:

EM ANDAMENTO

Objetivo:

Permitir criação de conta pelo navegador.

Fluxo:

Usuário

↓

Frontend

↓

Register Page

↓

API Account

↓

Persistence

↓

Conta criada

Estado:

Contrato definido.

Implementação aguardando Engenharia.

---

# FASE 3 — Web Player Experience

Status:

PLANEJADA

Objetivo:

Criar a primeira experiência completa do jogador.

---

## Web Authentication Flow

Objetivo:

Integrar:

Frontend

↓

Login

↓

API Authentication

↓

JWT

↓

Sessão do jogador

---

## Player Entry Screen

Objetivo:

Criar a primeira tela após login.

Possibilidades:

- Dados do jogador.
- Personagem.
- Entrada no jogo.

---

# FASE 4 — Gameplay Foundation

Status:

PLANEJADA

Objetivo:

Transformar infraestrutura em experiência de jogo.

Possíveis ciclos:

## Character Creation Experience

Criar personagem pelo navegador.

---

## Hunt Experience

Melhorar:

- seleção;
- apresentação;
- resultados.

---

## Combat Feedback

Adicionar:

- informações de combate;
- eventos;
- retorno visual.

---

# FASE 5 — Progression Foundation

Status:

PLANEJADA

Objetivo:

Criar evolução do jogador.

Inclui:

- Experiência.
- Level.
- Recompensas.
- Progressão.

---

# FASE 6 — Core MMORPG Systems

Status:

PLANEJADA

Objetivo:

Adicionar sistemas principais.

Inclui:

## Classes

- Knight.
- Paladin.
- Druid.
- Sorcerer.
- Monk.

---

## Skills

Sistema manual de habilidades.

---

## Equipamentos

Evolução dos itens.

---

## Inventário

Gerenciamento de objetos.

---

## Economia

Sistemas econômicos.

---

# FASE 7 — Conteúdo e Escala

Status:

PLANEJADA

Objetivo:

Transformar o projeto em um jogo vivo.

Inclui:

- Novas hunts.
- Novos monstros.
- Cosméticos.
- Pets.
- Mounts.
- Títulos.
- Eventos.
- Conteúdo social.

---

# Regras Permanentes

## Não criar antes da hora:

- sistemas futuros;
- balanceamento;
- economia;
- conteúdo sem fundação.

---

## Prioridades

1. Estabilidade.

2. Experiência do usuário.

3. Expansão de gameplay.

---

# Estado Atual Oficial

Último marco concluído:

PV-VS-2.0-045

Vertical Slice 2.0 Final Acceptance

---

Ciclo atual:

PV-VS-2.0-UPDATE-002

Web Account Registration Flow

---

Próxima ação:

Implementação pela Engenharia.

---

Fim do Roadmap.