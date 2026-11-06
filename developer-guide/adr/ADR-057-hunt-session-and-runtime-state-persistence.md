# ADR-057 - Hunt Session Ephemerality and Runtime State Persistence

Status: ACCEPTED
Version: 1.1
Data: 2026-11-05
Autor: Arquitetura (Chefe de Arquitetura)
Decisor da parte de Game Design: Product Owner (ver `decision-log.md`, ADR-016)

---

# Contexto

Quatro fontes oficiais divergem sobre o que acontece com uma Hunt quando o jogador desconecta ou o servidor reinicia:

1. **ADR-011** — Hunt Session "nunca será persistida"; "nunca poderá ser restaurada após desconexão".
2. **ADR-012** — "nenhuma recuperação de Hunt após desconexão"; proíbe recuperação de Hunt Session.
3. **Handbook V3** §17 — classifica Hunt Session e Runtime como estado *semi-permanente*; §18 define Recovery (carregar estado → reconstruir contexto → continuar); §19 define Checkpoints (início, conclusão, recompensa, logout). Contratos `PV-RUNTIME-002` e `PV-RUNTIME-002-A` pedem persistência e recovery de Runtime.
4. **Código** — `HuntSessionRepository` e `PersistentHuntSessionState` persistem a Hunt Session em SQLite.

Além disso, o produto é um MMORPG **Idle** com progressão automática, e o módulo **Offline Training** está LOCKED na Fase 2 do roadmap. O núcleo declarado do produto pressupõe progresso que continua sem o jogador ativo.

Esta ADR separa duas perguntas distintas:

- **Arquitetura (competência do Arquiteto):** como persistir e recuperar sem violar os princípios de separação definidos nas ADR-011/012.
- **Game Design (competência do Product Owner):** *a Hunt deve continuar/ser retomada após desconexão?* — regra de mecânica, não de arquitetura.

---

# Decisão proposta

## Parte A — Arquitetura (o Arquiteto recomenda aprovar)

Separar dois conceitos que hoje se confundem:

- **Hunt Session** permanece como definido na ADR-011: objeto **efêmero** de execução, nasce no início da Hunt, é destruído ao final, nunca reutilizado, nunca serializado como entidade permanente da Engine.
- **Runtime State / Checkpoint** é o registro **persistível** (semi-permanente, Handbook §17) que guarda *o suficiente* para reconstruir uma nova Hunt Session após reinício ou reconexão: Hunt Definition escolhida, localização, recursos temporários e marcos de progresso.

Persistência ocorre **somente em checkpoints** (início, conclusão, recompensa, logout), nunca a cada ação (Handbook §19).
Recovery cria uma **nova** Hunt Session a partir do checkpoint; a sessão anterior não é "ressuscitada".

Consequência para o código: `PersistentHuntSessionState` deixa de representar "a sessão" e passa a representar um checkpoint de Runtime (renomeação e migration em contrato de M5/M6).

## Parte B — Game Design (decisão do Product Owner)

Escolher a política de desconexão. Opções:

| Opção | Comportamento | Arquitetura exige |
|---|---|---|
| B1 | Hunt **encerra** na desconexão (ADR-012 original) | Checkpoint de logout apenas; recovery restaura o personagem, não a Hunt |
| B2 | Hunt é **retomada** do checkpoint ao reconectar | Parte A completa + Recovery |
| B3 | Hunt **continua offline** com progresso simulado/limitado ao reconectar | Parte A + Offline progression pelo Reward Pipeline (ADR-024) |

Recomendação do Arquiteto: **B3 limitado** (teto de tempo/ganho), por ser a única opção coerente com "MMORPG Idle" e com Offline Training LOCKED. A decisão final é do Product Owner.

---

# Decisão final (2026-11-06, `PV-GOV-004`)

Status: **ACCEPTED**.

## Parte B — decidida pelo Product Owner: **B1 + Offline Training**

`decision-log.md`, `DEC-20261106-05`:

- Com o jogador **offline, a Hunt não rende nada**.
- Ao **desconectar**, a Hunt **termina** pelo **pipeline único de encerramento** (ADR-012) — a desconexão é só mais um motivo de encerramento, como morte, fim de suprimento ou saída manual.
- O progresso offline vem **somente do Offline Training de skills** (Fase 2, LOCKED; `knowledge-base/training.md`).

*Histórico: uma versão anterior desta seção, no mesmo dia, registrava "B3 com restrição de XP offline" e status PROPOSED; foi substituída pela decisão acima. A recomendação original do Arquiteto ("B3 limitado") não foi adotada.*

## Parte A — aprovada pelo Arquiteto, **simplificada** pela decisão B1

Como a Hunt **não é recuperada**, o checkpoint de recovery **se limita ao estado do personagem e do Runtime** (localização, vida/mana, atividade, recursos), **sem recuperar Hunt Session**. Ficam **simplificados** (texto original mantido acima como histórico):

- "Runtime State / Checkpoint ... guarda o suficiente para reconstruir uma nova Hunt Session" → passa a guardar o suficiente para reconstruir o **Runtime** do personagem; nenhuma Hunt é recriada a partir dele.
- "Recovery cria uma **nova** Hunt Session a partir do checkpoint" → **não se aplica**: recovery reconstrói o Runtime; o jogador inicia uma nova Hunt por comando.
- Persistência só em checkpoints (Handbook §19) e Hunt Session efêmera (ADR-011) — **mantidos**.
- `PersistentHuntSessionState` continua a precisar de revisão (M5/M6): a sessão não é recuperável; o que for útil vira checkpoint de Runtime ou histórico.

## Consistência com outras ADRs

- **ADR-011 / ADR-012:** a decisão B1 **confirma** "nunca restaurada após desconexão" e "nenhuma recuperação de Hunt após desconexão". As notas de emenda nesses ADRs apenas apontam para cá (Runtime State persistível ≠ Hunt Session).
- **ADR-004 (conflito parcial, emendado):** ADR-004 diz que a Hunt "não depende da permanência do jogador conectado" e que "fechar o cliente não representa o encerramento imediato da Hunt". A decisão B1 **revoga essas frases para a desconexão do jogador**: desconectar encerra a Hunt. O restante do ADR-004 (Hunt pertence à Engine; cliente só inicia, acompanha e apresenta) continua valendo. A diferença entre "fechar a aba" e "fim da sessão do jogador" depende de uma eventual **janela de reconexão** — decisão do PO (dúvida 6 do M6). Nota de emenda no ADR-004.
- **ADR-002** e `knowledge-base/loot.md` (LOCKED) citam um "modo Hunt Offline" como benefício futuro / regra prevista. Pela decisão do PO, **Hunt Offline não está planejado**; divergência documental registrada em `project-state.md` (R-013). A separação Engine/apresentação do ADR-002 continua integralmente válida.

---

# Consequências

Benefícios:
- preserva o essencial de ADR-011/012 (Hunt Session efêmera, ciclo único, um único pipeline de encerramento);
- cumpre Handbook §17–19 e `PV-RUNTIME-002/002-A`; *(com a decisão B1: o Handbook §17/§18 — estado semi-permanente e Recovery "continuar" — é atendido **para o Runtime**, não para a Hunt Session, que não é recuperada; o ADR prevalece sobre o Handbook pela hierarquia da fonte da verdade)*
- remove a contradição entre ADR e código;
- habilita o Hunt Loop contínuo (Known Gaps).

Custos:
- ADR-011 e ADR-012 precisam de **emenda** (restrições "nunca restaurada" e "nenhuma recuperação" passam a se referir à *Hunt Session*, não ao Runtime State);
- renomear/migrar `PersistentHuntSessionState`;
- definir formato do checkpoint.

---

# Restrições

- Hunt Session nunca é reutilizada por outra Hunt (mantida).
- Todo encerramento — morte, saída manual, desconexão, administrativo — passa pelo **mesmo** pipeline (ADR-012, mantida).
- Não é permitido mais de uma Hunt simultânea por Character (ADR-012, mantida).
- Nenhuma implementação de M6 inicia antes desta ADR estar ACCEPTED.

---

# Referências

- ADR-011, ADR-012, ADR-024, ADR-045, ADR-046
- `docs/architecture/master/PROJECT-VICTORY-ARCHITECTURE-HANDBOOK-V3.md` §9–11, §17–19
- `docs/architecture/master/Runtime/finished/PV-RUNTIME-001/002/002-A`
- `developer-guide/architecture/roadmap.md` (Fase 2 — Offline Training)
- `developer-guide/architecture/decision-log.md` (ADR-016 — Separação de responsabilidades)
- `docs/architecture/PV-ANALYSIS-2026-11-05.md` §4.2
