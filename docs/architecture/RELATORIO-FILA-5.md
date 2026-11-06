# Relatório — MODO FILA 5, 2026-11-06

Ordem do PO e do Arquiteto-Chefe: "MODO FILA 5". Branches empilhadas a partir da `main` (`236cb6f`, que contém a Fila 4), worktree `C:/pv5`, push de cada branch, sem `push --force`, `main` intocada, nenhuma migration editada ou criada, nenhum segredo escrito.

## Branches (fazer o merge da última traz todas)

| # | Branch | Commit final | Conteúdo |
|---|---|---|---|
| 1 | `chief/q5-gov-010` | `ed72ac2` | PV-GOV-010 (só `.md`) |
| 2 | `chief/q5-prog-013` | `ca0e5b4` | PV-PROG-013 + ressalvas do GOV-010 |
| 3 | `chief/q5-char-012` | `a131122` | PV-CHAR-012 |
| 4 | `chief/q5-clean-004` | `08aa427` | PV-CLEAN-004 (remoção isolada em `844954c`) |
| 5 | `chief/q5-client-020` | (última; contém este relatório) | PV-CLIENT-020 + ressalvas do PROG-013 + revisão final |

## Status

| Item | Status | Revisão |
|---|---|---|
| Decisões a–g → `DEC-20261106-26…32` | ✅ | — |
| 1. PV-GOV-010 — inventário de atributos, emendas nos docs LOCKED, R-034 resolvido | ✅ | APROVADO COM RESSALVAS (aplicadas) |
| 2. PV-PROG-013 — títulos e tier por Level, em configuração | ✅ | APROVADO COM RESSALVAS (aplicadas) |
| 3. PV-CHAR-012 — atributos só do Tibia; identidade das classes em dados | ✅ | APROVADO COM RESSALVAS (revisão final) |
| 4. PV-CLEAN-004 — 9 Commands/Results órfãos (R-038) | ✅ | APROVADO (revisão final; grep refeito, zero referências) |
| 5. PV-CLIENT-020 — cliente exibe título/tier | ✅ | APROVADO COM RESSALVAS (revisão final) |
| 6. Revisão final | ✅ | APROVADO COM RESSALVAS (aplicadas) |

Nada travou nem foi pulado.

## Build e testes (executados)

| Momento | Build | Testes |
|---|---|---|
| Início (main, Fila 4) | 0 avisos | 298 |
| PV-PROG-013 | 0 avisos | 308 |
| PV-CHAR-012 / PV-CLEAN-004 | 0 avisos | 311 / 311 |
| **Final** (PV-CLIENT-020 + ressalvas) | **0 avisos, 0 erros** | **311 aprovados, 0 ignorados, 0 falhas** (Application 23, Domain 116, Runtime 17, Smoke 66, Api 89) |

**Cliente (React):** `npm run build` (Vite) **verde**. `npm run typecheck` **não verde, por ambiente**: o `node_modules` local não tem `@types/react` instalado (está no `package.json`). A base já tem 444 erros `TS7016/TS7026`; com as mudanças são 458, e os 14 a mais são todos `TS7026` dos elementos JSX novos, da mesma causa. Nenhum erro de tipo nos campos novos. Recomendo rodar `npm install` no cliente. O teste no navegador **não foi executado**.

## Removido × mantido

Lista completa: `docs/architecture/CLEANUP-MANIFEST-2026-11-06.md`. Inventário: `docs/architecture/PV-GOV-010-INVENTARIO-ATRIBUTOS.md`.

**Removido:**
- `Domain/Entities/Attributes.cs` (Strength, Dexterity, Intelligence, Vitality) e `Character.Attributes`. Nunca foram persistidos, expostos ou exibidos.
- A Ascension da Fila 4, substituída pelos títulos (`DEC-20261106-30`):
  - `AscensionRules.cs` → `LevelTitles.cs`;
  - `Character.Ascension` → `Character.Title`;
  - `RequiredAscension` → `RequiredTitleTier`;
  - `Ascension:LevelStep/Maximum` → `LevelTitles`.
- 9 Commands/Results órfãos (R-038), num commit isolado (`844954c`).
- Na documentação, tachados: `AttackSpeed` e `Capacity` em `entities.md` e `value-objects.md`; o afixo "Strength" no design de itens.

**Mantido:**
- Skills do Tibia (`Skills`: Magic Level, Fist, Club, Sword, Axe, Distance, Shielding, Fishing), Level, Experience, HP e Mana.
- `WeaponAttack`, `Armor` e `Defense` (existem no Tibia em arma, armadura e escudo).
- Multiplicadores de balanceamento das skills (PROVISÓRIOS, `DEC-20261106-32`).
- `BasicCombatAttributes`, sem uso (pergunta abaixo).
- Critical e Dodge nos documentos de design (perguntas abaixo).

**Banco:** schema inalterado; **nenhuma migration** foi necessária.

## O que mudou

- **Títulos por Level, em configuração:** Novice (1–499, tier 0), Ascension 1–4 (500–2499, tiers 1–4) e Legend (2500+, tier 5, permanente). Ficam na seção `LevelTitles` do `appsettings.json`; a API relê a seção a cada consulta, então trocar não exige recompilar nem reiniciar. Como trocar: contrato `PV-PROG-013`.
- **A API expõe `title` e `titleTier`** em `GET api/character` e `POST api/game/enter` (campos aditivos). O cliente só exibe.
- **Hunt areas** exigem `RequiredTitleTier`; a Spider Cave exige 0. O nome evita confusão com o Tier de item (`world.md` emendado).
- **Identidade das classes (`ClassIdentityTable`, só descritiva):**
  - Knight: Sword/Axe/Club e Shielding; físico.
  - Paladin: Distance; físico, com Holy só como tema.
  - Sorcerer: Magic Level; energia e fogo.
  - Druid: Magic Level; terra e gelo.
  - Monk: Fist Fighting; físico.
- **Docs LOCKED emendados por revisão formal:** ADR-026 v1.2, `character.md`, `schema/character.md`, `skills.md` (acrescenta Fishing e corrige "Fist Fighting"), `entities.md` e `value-objects.md`. Também emendados: `progression.md`, `world.md`, `combat.md`, `affixes.md`, `attributes.md` e `PV_ATTRIBUTE_SYSTEM_FOUNDATION`.

## Achados dos revisores

- **PV-GOV-010 (APROVADO COM RESSALVAS):**
  - conflito com o `world.md` ("Hunt não possui Tier") → emendado: tier **de título** é diferente do Tier de item, e o código usa `RequiredTitleTier`;
  - `skills.md` sem Fishing e com "First Fighting" → emendado;
  - inventário completado (`affixes.md`, `combat.md`);
  - notas formais nos docs LOCKED;
  - substituição da Ascension explicitada;
  - R-040 com redação mais precisa.
- **PV-PROG-013 (APROVADO COM RESSALVAS):**
  - o aviso de configuração inválida não chegava ao log → provedor registrado pela DI, avisando uma vez;
  - leitura dupla da tabela → corrigido;
  - variável de ambiente no teste → conferido, sem risco (coleção "Api");
  - tier não checado no servidor → **R-041**;
  - chave de configuração no Domain → movida para a API;
  - rastreabilidade → notas no PV-PROG-012 e no DEC-30.
- **Revisão final (CHAR-012, CLEAN-004, CLIENT-020): APROVADO COM RESSALVAS**, sem bloqueio.
  - Confirmado pelo revisor:
    - remoções amparadas e listadas, com grep zerado;
    - nenhuma migration;
    - o cliente não calcula o título;
    - o provedor de configuração é o resolvido pela DI.
  - Ressalvas aplicadas:
    1. inventário marcado como executado, com commits;
    2. registro do provedor com `Replace` (um único registro no container) e comentário da composição corrigido;
    3. o fallback `"Novice"` no cliente virou `"-"`;
    4. este relatório;
    5. **R-042** (typecheck do cliente por ambiente);
    6. `ClassIdentityTable` anotada como fonte futura do R-039.

## Pendências registradas

- **R-034:** resolvido.
- **R-037:** encerrado.
- **R-038:** resolvido.
- **R-040:** Magic Level inicial 10 no código × 0 no Tibia; liga-se ao R-039 (dano por Magic Level).
- **R-041:** tier mínimo da Hunt area não checado no servidor; checar quando houver a segunda área.
- **R-042:** typecheck do cliente não verde por ambiente (`npm install` pendente).

## Perguntas ao PO

1. **Critical** e **Dodge** entram como efeitos de item? Critical existe no Tibia como efeito de item; Dodge não foi confirmado.
2. **"Healing Power"** no design de afixos não é stat do Tibia: removo?
3. **`BasicCombatAttributes`** (Health/Attack/Defense) está sem uso no código: autoriza remover?
4. **R-040:** passar o Magic Level inicial para 0, como no Tibia, junto com a fórmula de dano por Magic Level (R-039)?
5. **Skills provisórias:** a skill "Arcane Lance" do Sorcerer não combina com energia e fogo (`DEC-20261106-29`). Renomeio no balanceamento?
6. **Pendentes de filas anteriores:**
   - R-035 (Monk antes do Level 8);
   - R-036 (Hunt Loop contínuo, M6);
   - fórmula oficial de dano e defesa;
   - poções (cura, preço, como obter);
   - dados e balanceamento da Spider Cave;
   - R-011;
   - R-019.

## Processos

Ao fim de cada contrato: `dotnet build-server shutdown` e checagem de que nenhum processo `dotnet` ou `ProjectVictory.API` ficou rodando. A junction temporária do `node_modules`, usada só para validar o cliente, foi removida com `rmdir`; o diretório original não foi tocado.
