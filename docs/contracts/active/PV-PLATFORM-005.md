# PV-PLATFORM-005 — Dependências do cliente

Status: **IMPLEMENTADO** (branch `chief/q2-platform-005`)
Emitido por: Arquitetura, 2026-11-06 (ordem "MODO FILA 2", item 6): "instalar @types/react* e demais tipos, npm audit fix (sem breaking changes sem registrar), script de typecheck; npm run build deve continuar verde. Não altere lógica do cliente nem apague código inalcançável."
Lastro: baseline do `PV-QA-002` (`project-state.md`); revisão do `PV-QA-002` (R4).

## Escopo implementado (só `package.json` e `package-lock.json`)

| Item | Antes | Depois |
|---|---|---|
| `@types/react`, `@types/react-dom` | ausentes | `^19.3.0` (devDependencies) |
| `npm audit fix` (sem `--force`) | 6 vulnerabilidades (2 moderadas, 4 altas) | **0 vulnerabilidades** |
| Versões alteradas pelo `audit fix` | — | `vite` 7.3.6 → 7.3.7, `postcss` 8.5.19 → 8.5.32, `source-map-js` 1.2.1 → 1.2.2 (todas **patch**, dentro das faixas já declaradas: **nenhuma breaking change**) |
| Script `typecheck` | inexistente | `tsc --noEmit` (separado do `build`; **não** bloqueia o build) |
| `tsc --noEmit` | 444 erros nesta execução (443 no `PV-QA-002`) | **7 erros** |
| `npm run build` | verde | **verde** (53 módulos, 206 kB) |

## Erros de tipo restantes (não corrigidos: são código das telas, fora do escopo)

1. `components/Character/PlayerStatePanel.tsx`: import quebrado `../models/game` (TS2307).
2. `components/player/PlayerIdentityArea.tsx`: `character/` × `Character/` só na caixa (TS1149).
3. `pages/CharacterSelectionPage.tsx`: `Character[]` com dois tipos diferentes (TS2345).
4. `pages/CharacterSelectionPage.tsx`: `Character.id` não existe (TS2339).
5. `pages/WorldScenePage.tsx`: `attackEncounter` não exportado por `huntService` (TS2305).
6. `pages/WorldScenePage.tsx`: `AttackEncounterResult` não exportado (TS2305).
7. `services/accountService.ts`: propriedade `token` fora de `AccountLoginResult` (TS2353).

Destino: `PV-UI-0xx` (M8), junto da navegação e da tipagem. Os itens 1, 5 e 6 estão em telas inalcançáveis (`PV-QA-002`).

## Observação do ambiente

O npm 11 avisa que o script de instalação do `esbuild` não está liberado (`npm approve-scripts`). O build passou mesmo assim, porque o binário vem do pacote opcional da plataforma. Nada foi liberado sem decisão.

## Validação

`npm ci` → `npm audit fix` → `npm install -D @types/react@^19 @types/react-dom@^19` → `npm audit` (0) → `npx tsc --noEmit` (7) → `npm run build` (verde). Solução .NET sem mudança: build com 0 avisos e testes verdes (ver `project-state.md`).
