# PV-PROG-013 — Títulos e tier derivados do Level, por configuração

- **Status:** Implementado (Fila 5, branch `chief/q5-prog-013`)
- **Origem:** ordem "MODO FILA 5", item 2; `DEC-20261106-30` (títulos por Level; resolve R-034); ADR-026 v1.2.
- **Regras preservadas:** Level derivado da Experience, sem teto, nunca reseta (ADR-027/028, `DEC-20261106-19`). Título e tier são **leituras** do Level (não persistidos).

## Regra

| Levels | Título | Tier |
|---|---|---|
| 1–499 | Novice | 0 |
| 500–999 | Ascension 1 | 1 |
| 1000–1499 | Ascension 2 | 2 |
| 1500–1999 | Ascension 3 | 3 |
| 2000–2499 | Ascension 4 | 4 |
| 2500+ | Legend (permanente) | 5 |

## Implementação

- **Domain:** `Progression/LevelTitles.cs` — `LevelTitle(Tier, Label, MinimumLevel)` e `LevelTitleTable` (valida: começa no Level 1, tiers 0,1,2… em ordem, faixas sem sobreposição, rótulos não vazios; `For(level)`; o último título vale para sempre). `Character.Title` / `Character.TitleUnder(table)`.
- **Application:** `ILevelTitleProvider` (+ `FixedLevelTitleProvider.Default`). `GetCharactersHandler` e `EnterGameHandler` usam o provedor.
- **API:** `Configuration/ConfigurationLevelTitleProvider` (registrado pela DI) lê a seção `LevelTitles` **a cada consulta**; configuração inválida mantém a última tabela válida (aviso no log); sem seção, vale a tabela padrão.
- **Respostas (aditivo):** `GET api/character` e `POST api/game/enter` ganham `title` e `titleTier`. A UI só exibe (`PV-CLIENT-020`).
- **Hunt areas:** `HuntAreaDefinition.RequiredTitleTier` (padrão 0); `IHuntAreaCatalog.GetAvailable(tier)` devolve as áreas do tier e dos anteriores. **Spider Cave = tier 0.** O nome `TitleTier` evita confusão com o Tier de item (`knowledge-base/world.md`, emendado).
- **Schema:** inalterado — **nenhuma migration**.

## Como trocar rótulos e faixas (sem recompilar)

Editar a seção `LevelTitles` do `src/ProjectVictory.API/appsettings.json` (ou do `appsettings.{Ambiente}.json`):

```json
"LevelTitles": [
  { "Tier": 0, "Label": "Novice",      "MinimumLevel": 1 },
  { "Tier": 1, "Label": "Ascension 1", "MinimumLevel": 500 },
  ...
  { "Tier": 5, "Label": "Legend",      "MinimumLevel": 2500 }
]
```

O ASP.NET recarrega o arquivo quando ele muda, e o provedor relê a seção a cada consulta: **não precisa reiniciar**. Também vale variável de ambiente, ex.: `LevelTitles__0__Label=Rookie` (nesse caso, com reinício). Regras: o primeiro título começa no Level 1 com tier 0; os tiers seguem 0, 1, 2… na ordem dos Levels; pode haver mais ou menos títulos que 6.

## Substituições (Ascension da Fila 4)

Lista completa em `docs/architecture/CLEANUP-MANIFEST-2026-11-06.md` (seção PV-PROG-013): `AscensionRules` → `LevelTitleTable`; `Character.Ascension` → `Character.Title`; `RequiredAscension` → `RequiredTitleTier`; `Ascension:LevelStep/Maximum` → `LevelTitles`. Nenhum campo de DTO publicado foi removido. **R-037 encerrado.**

## Testes

- `Domain.Tests/AscensionTests` (reescrito): limites 1/499/500, 999/1000, 1499/1500, 1999/2000, 2499/2500, 10.000, 821.009 e `int.MaxValue` (Legend permanente); tabela trocável; configuração inválida rejeitada; personagem no Level 1000 = "Ascension 2", tier 2, sem reset de vida. Os testes da revisão do `PV-CHAR-011` no mesmo arquivo continuam.
- `Domain.Tests/DerivedLevelTests`: Level 1 = tier 0; Level 575 = "Ascension 1".
- `SmokeTests/AscensionContentTests`: Spider Cave tier 0; áreas por tier.
- Novo `Api.Tests/LevelTitleHttpTests`: personagem novo = Novice/0 em `GET api/character` e `POST api/game/enter`; rótulo vindo da configuração ("Rookie"); provedor relê a configuração (troca a quente); configuração inválida mantém a última válida.

Resultado: build com 0 avisos; **308 aprovados, 0 ignorados, 0 falhas** (Application 23, Domain 113, Runtime 17, Smoke 66, Api 89).

Também nesta branch: as ressalvas da revisão do `PV-GOV-010` (ver o contrato `PV-GOV-010`).

## Revisão (`pv-architecture-reviewer`): APROVADO COM RESSALVAS — aplicadas na branch `chief/q5-client-020`

1. **Aplicada:** o provedor é registrado pela DI no `Program.cs` (recebe `ILogger`), substituindo o padrão da composição; o aviso de configuração inválida sai **uma vez** por quebra (volta a avisar só depois de uma configuração válida).
2. **Aplicada:** `EnterGameHandler` e `GetCharactersHandler` leem a tabela uma vez e calculam o título uma vez (rótulo e tier sempre da mesma tabela).
3. **Sem mudança (conferido):** o `AuthorizationHttpTests` já está na coleção `"Api"` (`ApiCollection.Name`), então não roda em paralelo com o teste que muda `LevelTitles__0__Label`.
4. **Registrada:** **R-041** — o tier mínimo da Hunt area ainda não é checado no servidor ao iniciar a Hunt (só existe a Spider Cave, tier 0).
5. **Aplicada:** a chave `"LevelTitles"` saiu do Domain (`ConfigurationLevelTitleProvider.SectionKey`).
6. **Aplicada:** nota de superação no contrato `PV-PROG-012`; nota no `DEC-20261106-30` sobre a remoção do código substituído.

Resultado: build com 0 avisos; **311 aprovados**.
