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

*Emenda (2026-11-06, `DEC-20261106-22`): **SUPERADO** o trecho "**fim de suprimento** encerra a sessão de Hunt". Sem poção o personagem segue sem cura até morrer; a Hunt termina só por morte, saída manual ou queda de conexão. Toda morte vai à tela inicial com mensagem orientando a checar poções, runas ou o level da Hunt. O restante (volta à cidade, sem cura sem suprimento, sem perda) continua valendo.*

## DEC-20261106-05 — Desconexão durante a Hunt (ADR-057, Parte B)

Status: **Accepted (Product Owner)**

**Com o jogador offline a Hunt NÃO rende nada.** Ao desconectar, a Hunt **termina** pelo **pipeline único de encerramento** (ADR-012). O progresso offline vem **somente do Offline Training de skills** (módulo LOCKED, Fase 2 do `roadmap.md`; regras em `knowledge-base/training.md`).

Equivale à opção **B1** do `ADR-057` para a Hunt, **mais Offline Training**. `ADR-057` passa a **ACCEPTED**. Substitui a pendência de `DEC-20261105-06`.

*Emenda (2026-11-06, `DEC-20261106-20/21`): a desconexão só encerra a Hunt depois de **mais de 1 minuto**; ao voltar, depois de mais de 10 s online, o jogador retorna à última Hunt com uma nova Hunt Session. O "Offline Training" virou **Treino com a página aberta**: **não há progresso offline em nenhuma atividade**.*

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

*Emenda (2026-11-06, `DEC-20261106-19`): o teto "Levels 1 a 500 por Ascension" foi **superado**. O Level não tem teto e nunca volta a 1; a Ascension é derivada do Level (uma nova a cada 500). A curva continua a mesma.*

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

*Emenda (2026-11-06, `DEC-20261106-19`): o teto "Levels 1 a 500 por Ascension" foi **superado**. O Level não tem teto e nunca volta a 1; a Ascension é derivada do Level (uma nova a cada 500). A curva continua a mesma.*

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

---

# Decisões do Product Owner — 2026-11-06, tarde (registradas em `PV-GOV-009`, ordem "MODO FILA 4")

Todas são decisões de Game Design do PO (ADR-016 deste log), salvo indicação. A revisão formal dos documentos LOCKED afetados foi **autorizada pelo Arquiteto-Chefe** na mesma ordem. As emendas foram feitas **sem apagar texto**.

## DEC-20261106-15 — Regra permanente: referência Tibia

Status: **Accepted (Product Owner)** — **regra permanente do projeto**

Tabelas e fórmulas de referência (experiência, vida, mana, dano, defesa, treino, capacidade etc.) **seguem o Tibia**, salvo decisão do PO em contrário. Quando o Tibia não tiver o caso, o valor é **PROVISÓRIO** e vira pergunta ao PO.

Nomes provisórios seguem o estilo do Tibia, **preferindo nomes originais ou genéricos** aos nomes exclusivos do Tibia, e ficam **centralizados em dados** para renomear fácil. Registrado também no `CLAUDE.md`.

## DEC-20261106-16 — Combate simples agora; Tibia como base; elementos depois

Status: **Accepted (Product Owner)**

- O combate fica **simples** nesta fase, com as fórmulas do Tibia como base, **adaptadas ao combate por turnos** (ADR-052).
- Danos e resistências **elementais virão depois**: o **tipo de dano é extensível** e hoje só existe o **físico**.

## DEC-20261106-17 — Sem CAP

Status: **Accepted (Product Owner)**

Não existe capacidade de carga (CAP): o **inventário de drops é ilimitado**.

## DEC-20261106-18 — Vida e Mana seguem o Tibia (resolve R-021)

Status: **Accepted (Product Owner)** — **supera** os valores de `knowledge-base/progression.md` (HP inicial 100 para todas as classes; ganhos fracionários) e resolve o **R-021**

Os valores iniciais e os ganhos por Level seguem o Tibia. Ganhos por Level definidos pelo PO:

| Classe | Vida/Level | Mana/Level |
|---|---|---|
| Knight | +15 | +5 |
| Paladin | +10 | +15 |
| Druid | +5 | +30 |
| Sorcerer | +5 | +30 |
| Monk | +12 | +15 |

**Valores iniciais e níveis baixos — conferidos em fonte pública** (Arquiteto, 2026-11-06). A fonte é o código aberto do NabBot (`cogs/utils/tibia.py`, Apache 2.0), que implementa as fórmulas da TibiaWiki. A TibiaWiki recusou acesso automático (HTTP 402/403) e não pôde ser lida diretamente.
- **Levels 1 a 7 (sem vocação, como no Tibia):** Vida = 5 × Level + 145 e Mana = 5 × Level + 50. No **Level 1 são 150 de vida e 55 de mana**, com **+5 de vida e +5 de mana** por Level até o 8.
- **A partir do Level 8:** valem os ganhos da classe acima, a partir de **185 de vida e 90 de mana** no Level 8.
  - Knight: vida 185 + 15 × (Level − 8); mana 5 × Level + 50.
  - Paladin: vida 185 + 10 × (Level − 8); mana 90 + 15 × (Level − 8).
  - Druid e Sorcerer: vida 5 × Level + 145; mana 90 + 30 × (Level − 8).
  - Todos batem com os ganhos do PO.
- **Monk: PROVISÓRIO até conferir na TibiaWiki.** A vocação existe no Tibia atual, mas não está na fonte consultada (o NabBot é anterior a ela). Ela usa o mesmo padrão até o Level 8. Os ganhos depois do 8 (+12/+15) valem **por decisão do PO** (`DEC-20261106-15`: "salvo decisão do PO"), não por conferência com o Tibia.

Implementação: `PV-CHAR-011`.

## DEC-20261106-19 — Ascension derivada do Level; Level sem teto

> *Emenda (Fila 5, `DEC-20261106-30`): a regra "Ascension 1 inicial, nova a cada 500, máximo 4" foi **SUPERADA** pelos **títulos por Level** (Novice, Ascension 1–4, Legend). Continua valendo: Level derivado da Experience, sem teto e sem reset.*

Status: **Accepted (Product Owner)** — **supera** o texto anterior do `ADR-026`, `knowledge-base/progression.md` (seções Level/Ascension) e as decisões que citavam "Levels 1 a 500" (`DEC-20261106-10`, `PV-PROG-011`); o **R-022 fica sem efeito**

- **Ascension 1** é a inicial. Uma nova Ascension é **desbloqueada automaticamente a cada 500 Levels**: Ascension 2 no Level 500, 3 no 1000 e 4 no 1500.
- **Máximo atual: 4.** O jogo suporta expansão futura de conteúdo: o **máximo e o passo ficam em configuração** e o conteúdo de cada Ascension fica em **dados**.
- O **Level nunca volta a 1** e **não há Level máximo**: a tabela do Tibia continua depois do Level 500.
- O jogador **escolhe** caçar na Ascension nova ou nas antigas.

Implementação: `PV-PROG-012`.

## DEC-20261106-20 — Conexão e Hunt

Status: **Accepted (Product Owner)** — complementa o `DEC-20261106-05` e o `ADR-057` (Parte B)

- **Queda de mais de 1 minuto** encerra a Hunt pelo **pipeline único** (ADR-012) e cessam os benefícios. **No primeiro minuto, a Hunt segue no servidor.**
- **Ao voltar**, o jogador cai na **tela inicial**. Se ficar online por **mais de 10 segundos depois de uma queda**, ele **retorna à última Hunt escolhida com uma NOVA Hunt Session**; a anterior não é mantida.
- **Se a conta deslogar por qualquer outro motivo**, o jogador fica na tela inicial (para poções, equipamentos, inventário).
- **Diretriz econômica para o M6 (Arquiteto, por ordem do PO):**
  - **heartbeat** do cliente, **sem WebSocket** (salvo justificativa documentada);
  - **presença em memória thread-safe**;
  - a **última Hunt escolhida** como **checkpoint mínimo do Runtime State** (ADR-057 Parte A).

## DEC-20261106-21 — Treino (antigo Offline Training)

Status: **Accepted (Product Owner)** — **altera o significado** do módulo **Offline Training** (Fase 2 do `roadmap.md`, LOCKED; `knowledge-base/training.md`, LOCKED); revisão formal autorizada pelo Arquiteto-Chefe

- O treino **exige a página aberta**, para manter alto o contador de jogadores online.
- **Queda de mais de 5 minutos** encerra a sessão de treino e cessa a evolução.
- **Não há progresso offline em nenhuma atividade.**
- **Item de treino** com **cargas**, comprado ou recebido como recompensa, usado no **centro da cidade** (tela inicial).
  - **Cada carga dura 1 hora**: uma ferramenta de 10 cargas = 10 horas.
  - **Cada item treina uma skill:** Sword, Axe, Club, Fist, Distance Fighting e Magic Level. Magic Level é para Sorcerer e Druid e, às vezes, Paladin.
  - O **ganho por carga depende do Level** e segue a **fórmula de treino do Tibia**.
- **O Monk treina Fist Fighting** — **PROVISÓRIO** (confirmar).

Emendas: `roadmap.md` (Fase 2), `knowledge-base/training.md`, `ADR-057` (v1.2). O M6 foi atualizado no `roadmap-to-playable.md` (`PV-TRAIN-010`).

## DEC-20261106-22 — Fim de suprimento NÃO encerra a Hunt; toda morte vai à tela inicial

Status: **Accepted (Product Owner)** — **supera** o trecho "fim de suprimento encerra a Hunt" do `DEC-20261106-04` e do `PV-SUPPLY-010`

- A poção é usada quando a vida atinge a porcentagem configurada. **Sem poção**, o personagem **segue sem cura até morrer**.
- A Hunt termina **somente** por **morte**, **saída manual** ou **queda de conexão** (`DEC-20261106-20`).
- **Toda morte** envia o personagem à **tela inicial**, com uma **mensagem** orientando a verificar **poções, runas ou o level da Hunt** antes de voltar.

Implementação: `PV-SUPPLY-011`. A tela da mensagem é do M8.

## DEC-20261106-23 — Skills de classe (4 por classe)

Status: **Accepted (Product Owner)** quanto às regras; números e nomes **PROVISÓRIOS** (balanceados pelo Arquiteto)

- **Knight:** 2 skills em área, de dano **baixo**; 2 de alvo único, de dano **médio**; e **ataque básico forte**.
- **Paladin:** 2 em área e 2 de alvo único, com dano **médio equilibrado** entre elas; e **ataque básico à distância de alvo único muito forte**. Magias Holy e de elementos virão depois.
- **Sorcerer:** 2 em área e 2 de alvo único, de dano **alto**, com o **Magic Level** como contador do dano final.
- **Druid e Monk:** o PO não deu regras. **PROVISÓRIO:** 2 em área e 2 de alvo único, dano médio; o Druid escala por Magic Level e o Monk por Fist Fighting.

Implementação: `PV-COMBAT-013`.

## DEC-20261106-24 — Cidade de retorno provisória: "Victoria"

Status: **Accepted (Product Owner)** — nome **PROVISÓRIO**, em dados

A cidade de retorno (tela inicial) passa a se chamar **"Victoria"**, em dados (substitui "Starting Location" no papel de cidade). Os nomes provisórios seguem a regra do `DEC-20261106-15`.

## DEC-20261106-25 — Remoções autorizadas (R-020, R-031)

Status: **Accepted (Product Owner)**

**Autorizadas** as remoções:
- `src/ProjectVictory.Domain/Playtest/` (R-020) → `PV-CLEAN-002`;
- os 5 handlers mortos que encerram a Hunt por fora do pipeline (R-031) → `PV-CLEAN-003`.

Cada remoção vai num commit isolado e reversível, depois de um `grep` sem nenhuma referência, com build e testes verdes e registro no `CLEANUP-MANIFEST`.

## DEC-20261106-26 — Atributos do personagem = somente os do Tibia

Status: **Accepted (Product Owner)** — Fila 5

- **Skills:** Level, Magic Level, Fist Fighting, Club Fighting, Sword Fighting, Axe Fighting, Distance Fighting, Shielding, Fishing.
- **Status:** HP e Mana.
- **Sem Capacidade** (reforça o `DEC-20261106-17`).

Inventário e ações: `PV-GOV-010` (`docs/architecture/PV-GOV-010-INVENTARIO-ATRIBUTOS.md`).

## DEC-20261106-27 — Remover atributos e stats que não existem no Tibia

Status: **Accepted (Product Owner)** — Fila 5

Remover do projeto **Força, Destreza, Inteligência, Vitalidade, Sorte, Attack Speed** e qualquer outro atributo ou stat inventado (sem equivalente no Tibia).

## DEC-20261106-28 — Exceção formal à Regra Aditiva para o DEC-20261106-27

Status: **Accepted (Product Owner)** — Fila 5

O PO **autoriza** a remoção/substituição desses atributos em código, DTOs, testes, docs e UI, como **exceção formal** à Regra Aditiva (`PROJECT-VICTORY-ARCHITECTURE-RULES.md`). Condições:
- cada remoção listada em `docs/architecture/CLEANUP-MANIFEST-2026-11-06.md`;
- nenhum arquivo inteiro apagado sem estar listado;
- nenhuma migration existente editada (se o schema mudar, **uma** migration nova; se destrutiva, parar e registrar para o Arquiteto).

## DEC-20261106-29 — Identidade das classes (descritiva, sem mecânica elemental)

Status: **Accepted (Product Owner)** — Fila 5

- **Druid:** mago da natureza; dano de **terra e gelo**.
- **Sorcerer:** **energia e fogo**.
- **Monk:** lutador de **Fist Fighting**, com armas de luva e afins.
- **Knight** e **Paladin:** como já decidido (`DEC-20261106-23`).

É **só dado descritivo**: nenhuma regra elemental (resistência, fraqueza, tipo de dano) entra por esta decisão. O tipo de dano em combate continua `Physical` (`PV-COMBAT-013`).

## DEC-20261106-30 — Títulos por Level (substitui a regra de Ascension da Fila 4; resolve R-034)

Status: **Accepted (Product Owner)** — Fila 5; **supera** a regra de Ascension do `DEC-20261106-19` (nova a cada 500, máximo 4)

- Level 1–499 **Novice** (tier 0) · 500–999 **Ascension 1** (tier 1) · 1000–1499 **Ascension 2** (tier 2) · 1500–1999 **Ascension 3** (tier 3) · 2000–2499 **Ascension 4** (tier 4) · 2500 em diante **Legend** (tier 5, permanente, sem teto de Level).
- Os **rótulos e faixas ficam em CONFIGURAÇÃO**, trocáveis a qualquer momento **sem recompilar**.
- **Level** continua derivado da Experience (sem teto); **título** e **tier** são derivados do Level.
- **Tier numérico:** 0 = Novice; 1 a 4 = Ascensions; 5 = Legend.
- **Hunt areas** passam a exigir **tier mínimo**; a Spider Cave exige **tier 0**.
- O **Level NUNCA reseta**.
- Resolve **R-034** ("5 Ascensions / Legend" × máximo 4): passam a existir 4 Ascensions **e** o título Legend a partir do Level 2500.

Implementação: `PV-PROG-013`.

*Nota do Arquiteto (revisão do `PV-PROG-013`):* como esta decisão **supera** a regra de Ascension da Fila 4, o código que a implementava (`AscensionRules`, `RequiredAscension`, chaves `Ascension:*`) foi **substituído** pelo equivalente novo, com cada item listado no `CLEANUP-MANIFEST-2026-11-06.md`. Nenhum DTO publicado perdeu campo (a Ascension nunca foi exposta). Se o PO preferir manter o código antigo, a reversão é um `git revert` do `PV-PROG-013`.

## DEC-20261106-31 — Remoção autorizada (R-038)

Status: **Accepted (Product Owner)** — Fila 5

Autorizada a remoção dos 9 Commands/Results órfãos dos handlers removidos no `PV-CLEAN-003` → `PV-CLEAN-004` (commit isolado, grep de zero referências, registro no manifesto).

## DEC-20261106-32 — Skills das classes aprovadas como PROVISÓRIAS

Status: **Accepted (Product Owner)** — Fila 5

Os nomes, números e Levels das skills de classe (`PV-COMBAT-013`, `ClassSkillTable`) ficam **aprovados como PROVISÓRIOS**, para balanceamento posterior.

## DEC-20261106-33 — Monster AI orientada a dados (decisão do Arquiteto, Fila 6)

Status: **Accepted (Arquiteto)** — implementa o ADR-005/006 no `PV-MONSTER-010`

- O comportamento de cada monstro é a lista de regras `MonsterActionRule` (Secondary Action + chance) na **definição de dados** do monstro; uma única `DataDrivenMonsterAI` lê os dados e sorteia com a semente do encounter.
- O comportamento atual (só ataque básico) foi migrado para dados (`MonsterBehavior.BasicAttackOnly`); sem regras a IA não consome aleatoriedade, então os resultados não mudam.
- Habilidades de monstro (números, chances) são **balanceamento do PO** — hoje nenhuma.

## DEC-20261106-34 — Experience Distribution (decisão do Arquiteto, Fila 6)

Status: **Accepted (Arquiteto)** — implementa o ADR-007 no `PV-XP-010`; regra de grupo **PROVISÓRIA** (PO)

- A Experience **final** (depois do XP rate) passa pela `IExperienceDistributor`, que só reparte; cada parte é aplicada pelo Reward Pipeline (ADR-024).
- Regra provisória: divisão igual, resto aos primeiros; com 1 participante (hoje) recebe tudo — mesmo XP de antes.

## DEC-20261106-35 — Spawn System stateless (decisão do Arquiteto, Fila 6)

Status: **Accepted (Arquiteto)** — implementa o ADR-009/010/047 no `PV-SPAWN-010`

- A configuração de spawn de cada Hunt é a da sua Hunt area (packs do Monster Pool); o `StatelessSpawnSystem` só a interpreta, sem estado, com o mesmo sorteio por semente de antes.
- Spawn Instances efêmeras com máquina de estados explícita; o descarte do encounter no fim da Hunt fica para o Hunt Loop (M6, R-044).

## DEC-20261106-36 — Política de poções é estado Permanente (decisão do Arquiteto, Fila 6)

Status: **Accepted (Arquiteto)** — `PV-PERSIST-010`, Handbook §17

- A política de poções (`PV-SUPPLY-010`) é configuração do jogador → **Permanente**; passa de memória para EF Core (tabela `SupplyPolicies`, migration nova e aditiva `20261106220713_SupplyPolicies`).
- Os demais stores seguem a classificação do `PV-STATE-001` (temporários em memória; Runtime vivo em memória com checkpoint persistido).

## DEC-20261106-37 — Save Strategy por checkpoints (decisão do Arquiteto, Fila 6)

Status: **Accepted (Arquiteto)** — implementa o Handbook §19 no `PV-PERSIST-011`

- Checkpoints em início da Hunt, recompensa, conclusão (pipeline único) e logout (este sem chamador até o M6, R-048).
- Rodada de combate é ação pequena: o consumo de poções fica pendente em memória e é gravado no próximo checkpoint (risco aceito: queda do servidor perde o débito das poções desde o último checkpoint).

## DEC-20261106-38 — Fechamento do M4 (decisão do Arquiteto, Fila 6)

Status: **Accepted (Arquiteto)** — revisão documentada em `docs/architecture/REVISAO-FECHAMENTO-M4.md`

- **LOCKED:** Monster AI e Experience Distribution (regra de grupo PROVISÓRIA do PO).
- **REVIEW:** Spawn System — LOCK no M6, depois do descarte no fim da Hunt (R-044) e dos Engine Events (R-046).
- Fase 6 continua IN PROGRESS.

> *Emenda (mesma data, revisão do `pv-architecture-reviewer`): critério único — um módulo só fica LOCKED quando o seu documento de domínio (contrato arquitetural do módulo) está cumprido. Por isso **Monster AI e Experience Distribution também ficam em REVIEW** (pendências R-050/R-043 e R-045), como o Spawn System. Nenhum módulo da Fase 6 foi travado nesta revisão.*

## DEC-20261106-39 — Sessão de jogo por heartbeat HTTP (decisão do Arquiteto, Fila 7)

Status: **Accepted (Arquiteto)** — implementa a diretriz do M6 (ADR-057 v1.2, `DEC-20261106-20`) no `PV-RUNTIME-010`

- Heartbeat HTTP (`POST api/game/heartbeat`), **sem WebSocket**; presença em memória thread-safe; varredura periódica de quedas (Runtime Coordinator).
- Tempos em configuração (`GameSession`): queda > 60 s, retorno após > 10 s online, varredura a cada 5 s.
- "Última Hunt escolhida" gravada no checkpoint do Runtime (coluna nova `LastHuntArea`, migration aditiva); o retorno cria uma **nova** Hunt.

## DEC-20261106-40 — Hunt Loop contínuo (decisão do Arquiteto, Fila 7)

Status: **Accepted (Arquiteto)** — implementa o `DEC-20261106-22` no `PV-HUNT-011`; fecha R-036 e R-044

- Vencer um encounter gera o próximo pelo Spawn; a Hunt termina só por morte, saída manual (`POST api/hunt/leave`, novo) ou queda.
- Fim da Hunt (pipeline único): mede tempo/XP (XP/hora), descarta encounters, despawna as Spawn Instances e tira a Hunt da memória.
- Comandos sobre a mesma Hunt são serializados (um lock por Hunt).

## DEC-20261106-41 — Encerramento por queda e logout (decisão do Arquiteto, Fila 7)

Status: **Accepted (Arquiteto)** — implementa ADR-057 v1.2 / `DEC-20261106-20` no `PV-HUNT-012`; fecha o R-048

- Queda > 1 min: o `GameSessionCoordinator` encerra a Hunt pelo pipeline único com o motivo `Disconnect` (sob o lock da Hunt), volta à "Victoria" e grava o checkpoint de logout.
- `POST api/game/logout` (novo): encerra com o motivo novo `Logout` (aditivo), grava o checkpoint e remove a presença (não gera retorno automático).
- Saída manual: Hunt desconhecida/alheia → 403; já encerrada → 409.

## DEC-20261106-42 — Recovery do Runtime no startup (decisão do Arquiteto, Fila 7)

Status: **Accepted (Arquiteto)** — `PV-RUNTIME-011`, Handbook §18, ADR-057

- No startup, depois das migrations: recarrega personagem + Runtime dos checkpoints e reconstrói o contexto na tela inicial (Idle), vida/mana limitadas ao máximo; Runtime órfão é ignorado; falha registrada sem derrubar o servidor.
- Hunt Session não é recuperada; reinício do servidor não dispara o retorno automático à última Hunt (pergunta ao PO).

## DEC-20261106-43 — Event Bus tipado e Domain Events (decisão do Arquiteto, Fila 7)

Status: **Accepted (Arquiteto)** — implementa ADR-049/050 no `PV-EVENT-010/011`; fecha o R-046

- `IEventBus` em processo, despacho pelo TIPO (handlers síncronos e assíncronos), só Domain Events (`EngineEvent`).
- `CombatCompletedEvent` → Reward Pipeline (Combat não conhece o Reward); ciclo de vida do Spawn e fim da Hunt publicados.
- Nenhum contrato público alterado.

## DEC-20261106-44 — Fechamento da Fila 7 (decisão do Arquiteto)

Status: **Accepted (Arquiteto)** — revisão documentada em `docs/architecture/REVISAO-FECHAMENTO-FILA-7.md` (`PV-GOV-011`)

- Critério único (LOCK só com o documento de domínio cumprido) aplicado a todos: **nenhum módulo travado**.
- Fase 6: Monster AI, Experience Distribution e Spawn continuam REVIEW (Spawn: R-044/R-046 fechados; falta R-057).
- Módulos novos do M6/M7 (Game Session, Hunt Loop/queda, Recovery, Event System) registrados no `roadmap.md` em REVIEW, com o que falta para cada LOCK.
