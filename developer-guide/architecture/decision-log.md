# Project-Victory

# Architecture Decision Log

Versão: 1.0

Status: Oficial

---

# Objetivo

Este documento registra todas as decisões arquiteturais oficiais do Project-Victory.

Seu objetivo é preservar o motivo de cada decisão tomada durante o desenvolvimento.

Toda decisão registrada neste documento passa a fazer parte da arquitetura oficial do projeto.

---

# Como utilizar

Cada decisão recebe um identificador único.

Formato:

ADR-XXX

Exemplo:

ADR-001

Uma decisão registrada somente poderá ser alterada através de uma nova ADR.

---

# ADR-001

Título:

Character como Aggregate Root

Status:

Accepted

Resumo:

Character representa o Aggregate Root principal do domínio.

Todos os sistemas pertencentes ao personagem são acessados através dele.

---

# ADR-002

Título:

Skills pertencem ao Character

Status:

Accepted

Resumo:

Skills fazem parte do Aggregate Character.

Skills não são Entity.

Skills não são Aggregate Root.

---

# ADR-003

Título:

Progressão baseada em uso

Status:

Accepted

Resumo:

Skills evoluem através da utilização correspondente.

A velocidade de evolução depende da Vocation.

---

# ADR-004

Título:

Item Template separado de Item Instance

Status:

Accepted

Resumo:

Templates representam definições permanentes.

Instances representam itens existentes no mundo.

---

# ADR-005

Título:

Affixes são permanentes

Status:

Accepted

Resumo:

Affixes são gerados durante a criação da Item Instance.

Nunca fazem parte do Template.

---

# ADR-006

Título:

Imbuements são temporários

Status:

Accepted

Resumo:

Imbuements representam modificações temporárias aplicadas posteriormente pelo jogador.

São independentes dos Affixes.

---

# ADR-007

Título:

Loot gera Templates

Status:

Accepted

Resumo:

Loot nunca cria Item Instances.

Loot seleciona apenas Item Templates.

A Item Factory cria as Instances.

---

# ADR-008

Título:

Combat utiliza Combat Context

Status:

Accepted

Resumo:

Combat Engine nunca consulta Aggregates diretamente durante seus cálculos.

Toda execução utiliza exclusivamente um Combat Context imutável.

---

# ADR-009

Título:

Combat produz Combat Result

Status:

Accepted

Resumo:

Toda Action produz exatamente um Combat Result.

Combat Result representa exclusivamente a saída da Combat Engine.

---

# ADR-010

Título:

Combat Pipeline determinístico

Status:

Accepted

Resumo:

Toda Action percorre exatamente o mesmo Pipeline.

Não existem pipelines específicos por tipo de Action.

---

# ADR-011

Título:

Vocation representa um Profile

Status:

Accepted

Resumo:

Vocation fornece parâmetros para os demais sistemas.

Ela não implementa regras de combate.

---

# ADR-012

Título:

Skill Multipliers pertencem à Vocation

Status:

Accepted

Resumo:

A velocidade de evolução das Skills é determinada pela Vocation.

Skills permanecem independentes.

---

# ADR-013

Título:

Resources desacoplados da Combat Engine

Status:

Accepted

Resumo:

Combat apenas utiliza Resources.

Resources gerenciam seu próprio ciclo de vida.

---

# ADR-014

Título:

Documentação é a única fonte de verdade

Status:

Accepted

Resumo:

Conversas servem apenas para produzir documentação.

Em caso de conflito, a documentação oficial prevalece.

---

# ADR-015

Título:

Roadmap obrigatório

Status:

Accepted

Resumo:

Todo novo módulo deverá consultar obrigatoriamente:

- roadmap.md
- project-state.md
- architecture-index.md

antes de iniciar qualquer evolução do projeto.

---

# ADR-016

Título:

Separação de responsabilidades

Status:

Accepted

Resumo:

Product Owner:

- Game Design
- Mecânicas
- Experiência do jogador
- Universo do jogo

Arquiteto:

- Arquitetura
- Engenharia
- Organização dos módulos
- Roadmap
- Documentação
- Revisões arquiteturais

---

# ADR-017

Título:

Documentação incremental

Status:

Accepted

Resumo:

Toda decisão consolidada deverá ser documentada imediatamente.

Grandes módulos poderão ser escritos em capítulos.

A implementação nunca deverá preceder a documentação oficial.

---

# ADR-018

Título:

Transição segura entre conversas

Status:

Accepted

Resumo:

A continuidade do Project-Victory não depende do histórico da conversa.

A continuidade depende exclusivamente da documentação oficial consolidada.

---

# Registro da Revisão de 2026-11-05

> **Nota de numeração (R-009):** este arquivo possui uma série própria de decisões `ADR-001…018`, distinta da série de `developer-guide/adr/ADR-001…057`. Para evitar nova colisão, as decisões abaixo usam o prefixo `DEC-`. Unificar as séries exige decisão do Product Owner/Arquiteto e não foi feito.

## DEC-20261105-01 — Código se conforma aos ADRs LOCKED

Status: Accepted (Arquitetura)

Onde o código diverge de ADR ACCEPTED sem decisão que o autorize (Level persistido × ADR-027/028; tabela fixa × ADR-025), o código é corrigido. Lastro: hierarquia da fonte da verdade; `PROJECT-VICTORY-ARCHITECTURE-RULES.md` (substituição só com decisão + contrato + validação).

## DEC-20261105-02 — Segurança e Server Authority em todo endpoint de gameplay

Status: Accepted (Arquitetura)

Todo endpoint de gameplay exige autenticação; identidade vem do token; recurso é verificado quanto ao dono. Lastro: `PRINCIPLES` ("O cliente solicita. O servidor decide."), Handbook V3 §23–24.

## DEC-20261105-03 — Stubs são substituídos atrás da mesma interface

Status: Accepted (Arquitetura)

`CombatResolver`, `RewardProcessor` e `ProgressionProcessor` (stubs/provisórios) são substituídos nos marcos M2–M3 mantendo interfaces, DTOs e endpoints. Lastro: Regra Aditiva (Backend).

## DEC-20261105-04 — Código sem referência em quarentena

Status: Accepted (Arquitetura)

`Application/Architecture`, `Playtest` e `Production` (42 arquivos, sem referências) só são removidos após `dotnet build` verde, em contrato de limpeza (M1). Lastro: Regra Aditiva ("builds continuam passando").

*Atualização (2026-11-06): removidos em `PV-CLEAN-001` (autorizado pelo PO), commit `38d1be7`; detalhes no `CLEANUP-MANIFEST-2026-11-05.md`.*

## DEC-20261105-05 — Roadmap de execução

Status: Accepted (Arquitetura) — **aguarda aprovação do Product Owner no M0**

`roadmap-to-playable.md` é o roadmap de execução; `roadmap.md` permanece a ordem oficial de módulos. Resolve R-005. Lastro: ADR-016 (Arquiteto responde por Roadmap) e `roadmap.md` (Roadmap Rules).

## DEC-20261105-06 — Política de desconexão da Hunt

Status: **Resolvida por `DEC-20261106-05`** (2026-11-06). *Texto original: "Pendente — ver `ADR-057` (PROPOSED). A parte de Game Design pertence ao Product Owner (ADR-016 deste log)."*

*Atualização (2026-11-06): decidida pelo PO — ao desconectar a Hunt termina (B1) e o progresso offline vem só do Offline Training de skills; ver `DEC-20261106-05`.*

## DEC-20261105-07 — Higiene do repositório

Status: Accepted (Arquitetura)

`node_modules/` e `dist/` deixam de ser versionados; binários e listagens sem valor de fonte são removidos; `projectvictory.db` permanece até o `PV-PLATFORM-001`. Lastro: Regra Aditiva (nenhuma funcionalidade removida) e clone limpo como pré-requisito de baseline.

*Atualização (2026-11-05): `projectvictory.db` removido do versionamento em `PV-PLATFORM-001`, após a aplicação das migrations no startup em Development — ver `docs/architecture/CLEANUP-MANIFEST-2026-11-05.md`.*

## DEC-20261106-01 — Hunt Analyzer e representação visual da batalha

Status: **Accepted (Product Owner)** — decisão de Game Design / experiência do jogador (ADR-016 deste log). Registrada pela Arquitetura em `PV-GOV-003`.

**Hunt Analyzer.** **Durante a Hunt**, a aba **"Analyzer"** do Game HUD mostra **em tempo real** os valores abaixo, inclusive o **lucro líquido**; **ao final da Hunt**, mostra o **resumo** da Hunt com os mesmos valores:

- **XP obtido** na Hunt;
- **XP por hora** naquela Hunt;
- **Débito:** recursos gastos, valorados pelo **preço de compra no NPC**;
- **Lucro:** recursos e itens encontrados e recebidos;
- **Lucro líquido** (lucro − débito).

**Representação visual da batalha.** Animação central **por local** (ex.: Spider Cave) mostrando um **duelo automático**, apenas para entretenimento, **sem nenhum efeito no resultado**.

Fundamentos:

- `ADR-002` (Engine é a única fonte de verdade; a Presentation Layer não executa regra de domínio; "a simulação visual do combate não interfere no resultado da Hunt") — todos os números do Analyzer são **calculados pelo servidor** e apenas exibidos pelo cliente; a animação é puramente decorativa.
- `PV-FRONTEND-GAME-HUD-FOUNDATION-001` (HUD como ambiente contínuo do jogador; estado *Hunting* dentro do HUD; "o frontend deve representar a simulação e seus resultados"). A aba "Analyzer" é **acréscimo** a esse contrato, que hoje não a define.
- `PROJECT-VICTORY-GAME-PHILOSOPHY` (jogo Idle; combate totalmente automático; evolução por Hunts e Loot).

Consequências para a Arquitetura (dependências e marco de entrega): ver `roadmap-to-playable.md`, M8 (`PV-HUD-ANALYZER-010` e `PV-HUD-BATTLE-VIEW-010`).

*Correção (2026-11-06, PO, registrada em `PV-GOV-006`): a primeira versão dizia "Durante e depois da Hunt… em tempo real". O PO esclareceu: tempo real **durante** a Hunt (incluindo lucro líquido) e **resumo ao final**.*

---

# Decisões do Product Owner — 2026-11-06 (registradas em `PV-GOV-004`)

Todas são decisões de Game Design / experiência do jogador (ADR-016 deste log). Onde há consequência de arquitetura, ela vem indicada.

## DEC-20261106-02 — Suprimentos de Hunt

Status: **Accepted (Product Owner)**

Nesta fase, os únicos suprimentos de Hunt são **poção de vida** e **poção de mana**. O combate será aprimorado depois (novos suprimentos entram por novas decisões do PO).

## DEC-20261106-03 — Uso de poção configurável pelo jogador

Status: **Accepted (Product Owner)**

O jogador configura o uso de poções em **tela própria**, inclusive **durante a Hunt**. O cliente só **solicita** a mudança; o servidor **valida e aplica a partir do próximo turno** (Server Authority — `CLAUDE.md`; combate por turnos — ADR-052). A configuração nunca altera um turno já em andamento.

## DEC-20261106-04 — Fim da Hunt

Status: **Accepted (Product Owner)**

- **Morte** ou **fim de suprimento** encerram a sessão de Hunt.
- O jogador **volta à cidade** e **aguarda comando**.
- **Sem suprimento não há cura.**
- **Nenhuma perda de loot ou XP** ao encerrar (inclusive por morte).

Consequência de arquitetura: "fim de suprimento" é um novo **motivo** de encerramento e passa pelo **mesmo pipeline** de encerramento da Hunt (ADR-012: "Todo encerramento utilizará exatamente o mesmo pipeline, independentemente do motivo"; fluxo termina em "Cidade Principal"). Não cria caminho alternativo.

## DEC-20261106-05 — Desconexão durante a Hunt (ADR-057, Parte B)

Status: **Accepted (Product Owner)**

**Com o jogador offline a Hunt NÃO rende nada.** Ao desconectar, a Hunt **termina** pelo **pipeline único de encerramento** (ADR-012). O progresso offline vem **somente do Offline Training de skills** (módulo LOCKED, Fase 2 do `roadmap.md`; regras em `knowledge-base/training.md`).

Equivale à opção **B1** do `ADR-057` para a Hunt, **mais Offline Training**. `ADR-057` passa a **ACCEPTED**. Substitui a pendência de `DEC-20261105-06`.

*Histórico: uma versão anterior desta entrada, no mesmo dia, registrava "B3 com restrição de XP offline" e uma pergunta aberta; o PO a corrigiu em 2026-11-06 para a decisão acima, e a pergunta aberta deixou de existir.*

## DEC-20261106-06 — Escopo social inicial

Status: **Accepted (Product Owner)**

**Servidor único**, compartilhado com outros jogadores, e **mercado universal de trade**. **Guild adiada.** Consequência de arquitetura: o mercado pertence à Fase 8 Economy do `roadmap.md` e **não** entra antes da fundação (Guardião de Escopo); esta decisão define o alvo, não antecipa a fase.

## DEC-20261106-07 — Classes iniciais

Status: **Accepted (Product Owner)**

**Knight, Paladin, Sorcerer, Druid e Monk** — já existem em `src/ProjectVictory.Domain/Character/CharacterClass.cs` (verificado em 2026-11-06). Nenhuma mudança de código necessária.

## DEC-20261106-08 — Valor do loot no lucro do Hunt Analyzer

Status: **Accepted (Product Owner)** em 2026-11-06 (`PV-GOV-006`) — antes PROVISÓRIA (recomendação do Arquiteto)

No Hunt Analyzer (`DEC-20261106-01`), o **lucro** valora cada item/recurso obtido pelo **preço de venda ao NPC**. (O débito já usa o preço de compra no NPC, por decisão do PO em `DEC-20261106-01`.)

**Todo item terá um preço de venda ao NPC.** Os valores iniciais são **PROVISÓRIOS**, definidos pelo Arquiteto (por delegação do PO) e ajustáveis depois pelo PO. O preço é **dado** da definição do item; comércio com NPC continua na Fase 7/8 (`roadmap.md`).

## DEC-20261106-09 — Primeira Hunt: Spider Cave

Status: **Accepted (Product Owner)** — local e os 5 monstros **confirmados pelo PO em 2026-11-06** (`PV-GOV-006`); **atributos provisórios**

- Local: **Spider Cave** (nome sugerido pelo PO).
- Monstros:
  - **Rat** e **Cave Rat** — já existem em `src/ProjectVictory.Application/Hunt/EncounterGenerator.cs` (verificado em 2026-11-06);
  - **Spider**, **Poison Spider** e **Giant Spider** — **novos**, recomendados pelo Arquiteto e **confirmados pelo PO** em 2026-11-06.
- Atributos (vida, dano, XP, loot) serão propostos como **BALANCEAMENTO PROVISÓRIO** no contrato do M3, para o PO ajustar. Nenhum número é definitivo até o ajuste do PO.

---

# Decisões do Product Owner — 2026-11-06, manhã (registradas em `PV-GOV-006`)

Além das novas entradas abaixo, o `PV-GOV-006` atualizou: `DEC-20261106-01` (texto do Analyzer: tempo real durante a Hunt + resumo ao final), `DEC-20261106-08` (deixa de ser provisória) e `DEC-20261106-09` (monstros confirmados). O `ADR-026` (Ascension Model) foi escrito como ACCEPTED a partir de `knowledge-base/progression.md`.

## DEC-20261106-10 — Curva de Experience do M2

Status: **Accepted (Product Owner)** — a curva é Game Design (ADR-016 deste log); o modelo de dados é da Arquitetura e segue os ADRs citados.

- **Curva:** a Experience **total** necessária para estar no Level `L` é

  `XP(L) = 50/3 × (L³ − 6L² + 17L − 12)`

  (Level 1 = 0, Level 2 = 100, Level 3 = 200, Level 4 = 400, Level 5 = 800, Level 6 = 1.500…). O resultado é sempre inteiro.
- **Experience como `long`** (64 bits): XP(500) = 2.058.474.800, perto do limite de `int` (2.147.483.647); a Experience continua acumulando depois do Level 500 (ADR-023) e o XP rate pode passar de 1x, então `int` estouraria. *(Justificativa corrigida na revisão do `PV-GOV-006`.)* Mudança de persistência por **migration aditiva**, sem editar migrations existentes.
- **Level derivado** de Experience + Ascension + Experience Table (ADR-027, ADR-028); deixa de ser fonte de verdade.
- **Experience Table gerada pela fórmula** (ADR-025): entradas explícitas para os Levels 1 a 500 de cada Ascension (`experience-table.md`, "Estrutura"; teto de 500 do `ADR-026`).
- **XP rate** é configuração, começando em **1x** — detalhada em `DEC-20261106-12`.

Substitui a pendência "Fórmula matemática da experiência / Curva de experiência por Level" de `knowledge-base/progression.md`. Implementação: `PV-PROG-011` e `PV-PROG-010` (M2); o XP rate, no `PV-REWARD-010` (M2).

## DEC-20261106-11 — Mesma curva em todas as Ascensions

Status: **Accepted (Product Owner)**

A curva de Experience (`DEC-20261106-10`) é **a mesma em todas as Ascensions**; só muda o **conteúdo desbloqueado** (regiões, monstros, equipamentos — `ADR-026`). Arquitetura: cada Ascension referencia uma Experience Table (ADR-025), e todas são geradas pela mesma fórmula.

## DEC-20261106-12 — XP rate como configuração do servidor; ajuste por ferramenta de administração

Status: **Accepted (Product Owner)**

- **XP rate inicial = 1x.**
- É **configuração do SERVIDOR** (`appsettings`/user-secrets/variáveis de ambiente), **nunca** controlada pelo cliente nem por jogador comum (Server Authority — `CLAUDE.md`).
- O XP rate é aplicado **no Reward Pipeline** (ADR-024: único responsável por mudar Experience).
- **M2:** entrega só a configuração e o uso dela no Reward Pipeline, **sem nenhum endpoint** que leia ou altere o valor.
- **M8:** uma **tela de ajuste** como **ferramenta de administração**, restrita a uma **conta administradora** (`PV-ADMIN-010`). Pré-requisito arquitetural: conceito de papel de administrador na Account e autorização por papel no servidor — definidos no próprio contrato do M8.

## DEC-20261106-13 — Revisão formal de documentos LOCKED (PV-GOV-007)

Status: **Accepted (Arquiteto-Chefe)** — revisão arquitetural documentada autorizada em 2026-11-06 (ordem "MODO FILA 2"). Aplica as propostas do `docs/architecture/PV-GOV-005-RELATORIO-2026-11-06.md`.

- **ADR-026** Ascension Model: já ACCEPTED desde o `PV-GOV-006`, escrito só a partir de `knowledge-base/progression.md`; confirmado sem mudança.
- **ADR-044 e ADR-048:** declarados **NOT USED** (números reservados, sem conteúdo inventado).
- **ADR-055** Event-Driven Quests: preenchido como **PROPOSED** a partir de ADR-049, 050 e 056 (e ADR-024); aceite depois do M7.
- **R-006:** as 5 referências a `docs/CURRENT_CONTEXT.md` em `docs/AI_WORKFLOW.md` (LOCKED), `docs/PROJECT_CONTEXT.md` (LOCKED), `README_DEVELOPMENT.md` e `docs/checklists/architecture-checklist.md` passam a apontar para `developer-guide/architecture/project-state.md`. O texto antigo foi mantido tachado e há nota de emenda nos dois LOCKED.
- **R-013:** nota de emenda, sem apagar texto, em `ADR-002` ("suporte ao modo Hunt Offline") e `knowledge-base/loot.md` (LOCKED, "regras de Hunt Offline"), apontando para o `ADR-057`.
- Os arquivos com travessão "—" no nome **não** foram renomeados; o mapa de renomeação continua no relatório do `PV-GOV-005`.

Lastro: `roadmap.md` e `CLAUDE.md` (módulo LOCKED só muda por revisão arquitetural documentada), ADR-016 deste log (documentação é do Arquiteto).

## DEC-20261106-14 — Projeto de composição de DI (PV-ARCH-001)

Status: **Accepted (Arquiteto)**

Criado `src/ProjectVictory.Composition`: raiz de composição de DI (`AddProjectVictoryCore`) usada pela API e pelos testes, para resolver o R-014. Referencia Domain, Application, Infrastructure e Persistence; **não contém regra de negócio** e não é uma camada nova da Clean Architecture. Motivo: Infrastructure e Persistence não se referenciam, e acoplá-las só para compor o DI violaria a separação atual. Ficam no host: DbContext, identidade do jogador, JWT, CORS, controllers e tratamento de erro HTTP. Detalhes: `docs/contracts/active/PV-ARCH-001.md`.
