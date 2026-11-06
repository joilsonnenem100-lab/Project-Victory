# PV-CHAR-012 — Atributos só do Tibia; identidade das classes em dados

- **Status:** Implementado (Fila 5, branch `chief/q5-char-012`)
- **Origem:** ordem "MODO FILA 5", item 3; `DEC-20261106-26` (atributos = só os do Tibia), `DEC-20261106-27` (remover os inventados), `DEC-20261106-28` (exceção formal à Regra Aditiva), `DEC-20261106-29` (identidade das classes, descritiva). Tabela de ações: `docs/architecture/PV-GOV-010-INVENTARIO-ATRIBUTOS.md`.

## Removido (lista no `CLEANUP-MANIFEST-2026-11-06.md`)

- `src/ProjectVictory.Domain/Entities/Attributes.cs` (Strength, Dexterity, Intelligence, Vitality).
- `Character.Attributes`.

Application, Persistence, API, cliente e testes não tinham nada desses atributos (inventário). **Schema inalterado → nenhuma migration.**

## Acrescentado

- `src/ProjectVictory.Domain/Character/ClassIdentity.cs`:
  - `TibiaSkill` — Magic Level, Fist, Club, Sword, Axe, Distance Fighting, Shielding, Fishing (o Level fica à parte, derivado da Experience);
  - `ElementTheme` — Physical, Holy, Earth, Ice, Energy, Fire — **só descritivo** (sem resistência/fraqueza; o dano continua `Physical`);
  - `ClassIdentityTable` — por classe: descrição, skills principais do Tibia e tema elemental:
    - Knight: Sword/Axe/Club + Shielding; Physical.
    - Paladin: Distance + Shielding; Physical, Holy (Holy só como tema: magias Holy ficam para depois, `DEC-20261106-23`).
    - Sorcerer: Magic Level; Energy, Fire.
    - Druid: Magic Level; Earth, Ice.
    - Monk: Fist Fighting; Physical.
- As skills do personagem continuam em `Skills` (as 8 do Tibia) e o status em HP/Mana (Attribute Curve).

## Testes

Novo `Domain.Tests/TibiaAttributeTests`: o `Character` não tem atributos inventados (nem `Attributes`, `Capacity`, `AttackSpeed`…); `Skills` tem exatamente as 8 skills do Tibia; a identidade das 5 classes segue o `DEC-20261106-29`.

Resultado: build com 0 avisos; **311 aprovados, 0 ignorados, 0 falhas** (Application 23, Domain 116, Runtime 17, Smoke 66, Api 89).

## Pendências

- A `ClassIdentityTable` ainda não tem consumidor fora do teste (é descritiva, `DEC-20261106-29`); será a fonte das skills principais quando o dano por skill for ligado (R-039).

R-040 (Magic Level inicial 10 × 0 do Tibia) e R-039 (scaling por skill) continuam; o PO decide quando ligar dano por Magic Level/Distance/Fist.
