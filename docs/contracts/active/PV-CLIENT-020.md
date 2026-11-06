# PV-CLIENT-020 — Cliente exibe só atributos reais e o título/tier

- **Status:** Implementado (Fila 5, branch `chief/q5-client-020`)
- **Origem:** ordem "MODO FILA 5", item 5; `DEC-20261106-26/27` (atributos só do Tibia) e `DEC-20261106-30` (títulos por Level). **Sem redesign** (isso é a UI-1).
- **Server Authority:** título e tier vêm do servidor (`title`, `titleTier` em `GET api/character` e `POST api/game/enter`, `PV-PROG-013`); o cliente **só exibe**, não calcula.

## Verificação

O cliente não exibia nenhum atributo inventado (grep em `src/ProjectVictory.Client/src` por Strength/Força, Dexterity, Intelligence, Vitality, Luck/Sorte, Attack Speed, Critical, Capacity: nada). Mostra nome, Level, Experience e status — todos reais.

## Mudanças (aditivas, 8 arquivos, 50 linhas)

- Modelos: `models/game.ts` (`GameState`), `models/character.ts` e `services/characterService.ts` (`Character`) ganham `title?` e `titleTier?`.
- Exibição do título (e do tier, quando houver): `components/Character/CharacterProgressionPanel.tsx`, `components/Character/PlayerStatePanel.tsx`, `pages/PlayerInformationPage.tsx`, `App.tsx` (tela da Hunt) e `pages/EntryExperiencePage.tsx`.

## Validação

- `npm run build` (Vite): **verde**.
- `npm run typecheck`: **não verde, por problema de ambiente anterior** — o `node_modules` local não tem `@types/react` instalado (está no `package.json`), e o `tsc` acusa 444 erros já na base (`TS7016`/`TS7026`: JSX e módulos React sem tipos). Com as mudanças: 458; os **14 a mais são todos `TS7026`** (os elementos JSX novos), da mesma causa. Nenhum erro de tipo nos campos novos. Ação recomendada: `npm install` no cliente (fora do escopo desta fila).
- Revisão final: o fallback `"Novice"` da `EntryExperiencePage` virou `"-"` (o rótulo pertence à configuração do servidor).
- Teste no navegador: **não executado** (sem redesign; a API já é coberta por `Api.Tests/LevelTitleHttpTests`).
