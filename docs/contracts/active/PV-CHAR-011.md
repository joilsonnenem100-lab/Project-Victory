# PV-CHAR-011 — Vida e Mana do Tibia

Status: **IMPLEMENTADO** (branch `chief/q4-char-011`)
Emitido por: Arquitetura, 2026-11-06 (ordem "MODO FILA 4", item 2).
Lastro:
- `DEC-20261106-18` (Vida e Mana seguem o Tibia; ganhos por Level do PO; resolve R-021);
- `DEC-20261106-15` (regra Tibia);
- ADR-029 (Attribute Curve).

## Escopo implementado

1. **`AttributeCurve`** ganhou **duas fases** (parâmetros novos com padrão, **aditivo**: a curva antiga de uma fase continua igual):
   - do Level 1 ao `VocationLevel`: inicial + ganho sem vocação × (L − 1);
   - depois: valor no `VocationLevel` + ganho da classe × (L − `VocationLevel`).

   Os ganhos aceitam fração. Há `checked` contra estouro, porque o Level não tem teto.
2. **`AttributeCurveTable`** (dados): `VocationLevel` = 8; ganho sem vocação de +5 de vida e +5 de mana (Tibia).
3. **`CharacterClassDefinition`** (dados por classe):

   | Classe | Level 1 (vida/mana) | Ganho por Level a partir do 8 |
   |---|---|---|
   | Knight | 150 / 55 | +15 / +5 |
   | Paladin | 150 / 55 | +10 / +15 |
   | Sorcerer | 150 / 55 | +5 / +30 |
   | Druid | 150 / 55 | +5 / +30 |
   | Monk | 150 / 55 | +12 / +15 (**PO**; início **PROVISÓRIO**) |

   Antes eram Knight 185/55, Paladin 155/95, Sorcerer e Druid 125/140 e Monk 150/80.
4. **Fonte do Tibia:** código aberto do NabBot (`cogs/utils/tibia.py`, Apache 2.0, fórmulas da TibiaWiki). Conferido:
   - Knight no Level 8 = 185/90 e no Level 100 = 1.565/550 (15 × L + 65 e 5 × L + 50).

   A TibiaWiki recusou acesso automático (HTTP 402/403).

## Conflitos de documentos (registrados)

- `knowledge-base/progression.md`: 100 de vida e ganhos fracionários. **Superado** pelo `DEC-20261106-18`, com nota de emenda no próprio documento.
- `docs/character e conceitos/PV_CHARACTER_FOUNDATION_SPECIFICATION.md`, `PV_CHARACTER_CLASS_PROGRESSION_MASTER.md` e `PV_CHARACTER_IDENTITY_AND_PROGRESSION_MASTER_v2.md`:
  - os valores **iniciais** por classe no Level 1 (Knight 185…) divergem do Tibia, que tem 150 para todos no Level 1 (185 é o valor do Level 8);
  - os **ganhos** batem;
  - prevalece o `DEC-20261106-18`, a decisão mais recente do PO. Não houve emenda nesses documentos porque eles não são LOCKED; o conflito fica registrado aqui e no relatório.

## Efeito no jogo

- Personagens novos começam com **150 de vida e 55 de mana**, em qualquer classe.
- Personagens existentes passam a ter os máximos derivados novos (o máximo não era persistido). A vida **atual** persistida não muda.
- O combate usa os máximos novos.

## Testes

- `Domain.Tests/DerivedAttributeTests.cs`:
  - 5 classes nos Levels 1, 6, 8 e 10;
  - Knight pela fórmula do Tibia nos Levels 8, 100 e 1000;
  - crescimento ao ganhar XP;
  - curva fracionária e construtor antigo mantidos.
- **Testes alterados** (valores antigos → Tibia):
  - `DerivedAttributeTests` (substituídos pelos acima);
  - `SpiderCaveEndToEndTests.Defeat_…`: vida cheia do Sorcerer de 125 → **150**.

Resultado da solução: build com 0 avisos; **259 aprovados, 0 ignorados, 0 falhas**.

Também neste branch, as ressalvas da revisão do `PV-GOV-009`.
