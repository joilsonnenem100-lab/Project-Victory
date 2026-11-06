# PV-GOV-010 — Inventário de atributos (Fila 5, 2026-11-06)

Regra: `DEC-20261106-26` (atributos = só os do Tibia), `DEC-20261106-27` (remover o que não existe no Tibia), `DEC-20261106-28` (exceção formal à Regra Aditiva).

Método: grep em todo o repositório (`*.cs`, `*.ts`, `*.tsx`, `*.md`) por Strength/Força, Dexterity/Destreza, Intelligence/Inteligência, Vitality/Vitalidade, Luck/Sorte, Attack Speed, Critical/Crítico, Dodge/Esquiva, Capacity/Capacidade/CAP e pelos tipos de atributo do Domain; leitura de cada ocorrência. Ocorrências da palavra "força" ou "capacidade" no sentido comum do português ("força de ataque", "capacidade de bloquear", "capacidade do sistema", nos work items do vertical slice) **não são atributos** e ficam fora da tabela.

## Código, persistência, API e cliente

| Item | Onde | Existe no Tibia? | Ação |
|---|---|---|---|
| `Strength`, `Dexterity`, `Intelligence`, `Vitality` (classe `Attributes`) | `src/ProjectVictory.Domain/Entities/Attributes.cs` | **Não** | **Removida** (`PV-CHAR-012`, commit `a131122`) |
| `Character.Attributes` (propriedade, sempre `new Attributes()`) | `src/ProjectVictory.Domain/Entities/Character.cs` | **Não** | **Removida** (`PV-CHAR-012`, commit `a131122`) |
| Skills: Fist, Club, Sword, Axe, Distance, Shielding, MagicLevel, Fishing | `src/ProjectVictory.Domain/Entities/Skills.cs` | Sim | **Manter**. *Divergência:* Magic Level inicial = 10 no código; no Tibia é 0 → **R-040** (mudar sem fórmula de dano por ML alteraria o dano de Sorcerer/Druid; ligado ao R-039) |
| Level, Experience | `Character`, `ExperienceCurve` | Sim | Manter |
| HP e Mana (derivados por `AttributeCurve`, ADR-029) | `Domain/Character/AttributeCurve.cs`, `CharacterClassDefinition.cs` | Sim | Manter ("Attribute" no nome é só o termo do ADR-029) |
| `WeaponAttack`, `Armor`, `Defense` (`CombatProfile`, equipamento inicial PROVISÓRIO) | `Domain/Combat/TurnBased/Combatant.cs`, `ClassCombatProfileTable.cs` | Sim (atributos de arma, armadura e escudo) | Manter |
| `AbilityManaCost`, `AbilityDamageMultiplier`, `BasicAttackMultiplier` | `CombatProfile` | Não são atributos do personagem: são **dados de balanceamento** das ações (PROVISÓRIOS) | Manter |
| `BasicCombatAttributes(Health, Attack, Defense)` | `Domain/Combat/BasicCombatAttributes.cs` | Health sim; Attack/Defense existem como valores de arma/escudo | Manter. **Sem uso** no código → candidato a limpeza futura (pergunta ao PO) |
| Monstros: vida, ataque, armadura, defesa, XP | `Domain/Entities/Monster.cs`, `SpiderCaveCatalog` | Sim | Manter |
| Banco (tabela `Characters`) | snapshot de migrations | Não guarda `Attributes` nem `Skills` | **Nenhuma migration** necessária |
| DTOs da API | `src/ProjectVictory.API` | Nenhum DTO expõe `Attributes` | Nada a remover; `title`/`titleTier` acrescentados (`PV-PROG-013`, commit `ca0e5b4`) |
| Cliente React | `src/ProjectVictory.Client/src` (`models/character.ts`, painéis de personagem) | Mostra só nome, Level, Experience e status | Nada a remover; título/tier exibidos (`PV-CLIENT-020`, commit `ee610fc`) |
| Testes | `tests/*` | Nenhum teste usa `Attributes` | Nada a remover |
| Ascension da Fila 4 (`AscensionRules`, `Ascension:LevelStep/Maximum`, `RequiredAscension`) | Domain, Application, API, `appsettings.json` | — (regra superada pelo `DEC-20261106-30`) | **Substituída** por títulos/tier em configuração (`PV-PROG-013`, commit `ca0e5b4`) |

## Documentação

| Item | Onde | Existe no Tibia? | Ação |
|---|---|---|---|
| "5 Ascensions", "Legend = Level 500 + Ascension 5" (R-034) | `knowledge-base/character.md` (LOCKED), `schema/character.md` | — | **Emendado** com os títulos por Level (`DEC-20261106-30`); R-034 resolvido |
| Ascension "nova a cada 500, máximo 4" | `ADR-026` v1.1, `knowledge-base/progression.md` | — | **Emendado** (ADR-026 → v1.2) |
| "Não existem inicialmente: Strength, Intelligence, Dexterity, Vitality" | `knowledge-base/character.md`, `schema/character.md` | Não | Coerente; emenda deixa claro que **não existem** (sem "inicialmente") |
| "Não utiliza / não implementar Strength, Vitality, Intelligence, Dexterity" | `PV_CORE_GAME_LOOP_FOUNDATION.md`, `PV-CONTRACT-001` | Não | Coerente — manter |
| Módulo "Base Attributes" | `developer-guide/domain/attributes.md` (DRAFT) | Não (além de HP/Mana) | **Emendado**: atributos = Skills do Tibia + HP/Mana |
| Lista de skills (sem Fishing) | `PV_ATTRIBUTE_SYSTEM_FOUNDATION.md` | Sim | **Emendado** com o conjunto oficial (inclui Fishing) |
| `AttackSpeed` (value object) | `developer-guide/domain/entities.md`, `value-objects.md` | Não | **Tachado** (manifesto 2026-11-06) |
| `Capacity` (value object) | `developer-guide/domain/entities.md`, `value-objects.md` | O Tibia tem CAP, mas o PO decidiu **sem Capacidade** (`DEC-20261106-17`) | **Tachado** |
| "Weapons não possuem velocidade de ataque"; "Não existe Attack Speed" | `developer-guide/domain/weapons.md`, `knowledge-base/combat.md` | — | Coerente — manter |
| Afixo "Strength" ("of Strength", "Strength I/II/III") | `PROJECT-VICTORY-ITEM-SYSTEM-DESIGN.md` | Não | **Tachado**; afixos só sobre atributos do Tibia |
| Critical (chance/dano) | `entities.md`, `value-objects.md`, `knowledge-base/combat.md`, `spells.md`, design de combate e de itens | Sim, como efeito de item/sistema (não como atributo do personagem) | Manter nos docs de design; **nada no código**. Pergunta ao PO se entra |
| Dodge | `PROJECT-VICTORY-COMBAT-SYSTEM-DESIGN.md` | Não confirmado como atributo do personagem | Manter só no doc de design (não implementado). **Pergunta ao PO** |
| MovementSpeed, Weight | `entities.md`, `value-objects.md` | Sim (speed; peso de item) | Manter |
| "Inteligência" (IA de monstro, "inteligência artificial") | docs de monstro/AI | Não é atributo | Fora do escopo |
| *Revisão:* "Ascension mínima/recomendada" das Hunts; "A Hunt não possui Tier" | `knowledge-base/world.md` | — | **Emendado**: Hunt area exige **tier de título** (≠ Tier de item); no código `RequiredTitleTier` |
| *Revisão:* "Skills Oficiais" sem Fishing; grafia "First Fighting" | `knowledge-base/skills.md` (LOCKED) | Fishing e Fist Fighting existem | **Emendado** (revisão formal) |
| *Revisão:* "Não existe inicialmente: Attack Speed; Cast Speed" | `knowledge-base/combat.md` | Não | **Emendado**: não existem (sem "inicialmente") |
| *Revisão:* Affixes — Resistências, Critical, Life/Mana Leech | `developer-guide/domain/affixes.md` | Sim, como efeitos de item | Manter (emenda) |
| *Revisão:* Affix "Healing Power" | `developer-guide/domain/affixes.md` | Não é stat do Tibia | **Pergunta ao PO** (emenda marca) |

## Resultado

- **Código removido:** só a classe `Attributes` e a propriedade `Character.Attributes` (`PV-CHAR-012`, commit `a131122`).
- **Schema:** não muda; nenhuma migration.
- **Ascension da Fila 4:** substituída por títulos/tier (`PV-PROG-013`).
- **Docs:** 3 documentos com texto tachado (manifesto) e 6 emendados.
- **Pendências novas:** R-040 (Magic Level inicial 10 × 0 do Tibia).
- **Revisão do PV-GOV-010:** acrescentadas as linhas marcadas *Revisão* (world.md, skills.md, combat.md, affixes.md).
- **Perguntas ao PO:** Critical, Dodge e `BasicCombatAttributes` sem uso.
