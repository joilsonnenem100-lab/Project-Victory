# ADR-057 - Hunt Session Ephemerality and Runtime State Persistence

Status: PROPOSED
Version: 1.0
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

# Situação da decisão (2026-11-06, `PV-GOV-004`)

Status continua **PROPOSED**. Só passa a **ACCEPTED** quando o Product Owner responder a pergunta aberta da Parte B.

- **Parte A — aprovada pelo Arquiteto.** Separação Hunt Session (efêmera) × Runtime State/Checkpoint (persistível), persistência só em checkpoints e recovery criando nova Hunt Session.
- **Parte B — B3 com restrição de XP offline; escopo do ganho offline pendente de resposta do PO.** O PO escolheu **B3 (Hunt continua offline)** com a restrição **"nenhum ganho offline de experiência em Hunts"** (`decision-log.md`, `DEC-20261106-05`). Em aberto, o que a Hunt rende offline além de XP:
  - a) loot e lucro sem XP;
  - b) a Hunt termina ao desconectar;
  - c) só treino offline de skills (Offline Training, Fase 2).
- A recomendação original "B3 limitado (teto de tempo/ganho)" fica substituída pela restrição do PO; o teto de tempo/ganho será definido junto com a resposta, se a opção escolhida render algo offline.
- Bloqueia apenas o **M6** (`roadmap-to-playable.md`); a restrição "Nenhuma implementação de M6 inicia antes desta ADR estar ACCEPTED" continua valendo.

---

# Consequências

Benefícios:
- preserva o essencial de ADR-011/012 (Hunt Session efêmera, ciclo único, um único pipeline de encerramento);
- cumpre Handbook §17–19 e `PV-RUNTIME-002/002-A`;
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
