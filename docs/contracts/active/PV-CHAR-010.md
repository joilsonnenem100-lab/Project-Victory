# PV-CHAR-010 — Vida e Mana máximas derivadas do Level (ADR-029)

Status: **IMPLEMENTADO** (branch `chief/m3-char-010`) — valores **PROVISÓRIOS** (R-021)
Emitido por: Arquitetura, 2026-11-06 (ordem "MODO FILA 3", item 2).
Lastro:
- ADR-029 (Derived Attribute Model: atributos base derivados de Class + Experience + Ascension + Attribute Curve, nunca editáveis);
- ADR-027/028 (Level derivado);
- pendência "Vida/Mana máximas não crescem com o Level", aberta no `PV-PROG-010`.

## Fontes lidas

| Fonte | O que diz |
|---|---|
| `knowledge-base/progression.md` (EM DESENVOLVIMENTO) | HP inicial **100** para todas as classes; crescimento por Level Knight **+0.75**, Paladin +0.50, Monk +0.65, Druid +0.25, Sorcerer +0.25. Mana inicial Knight 50, Paladin 60, Monk 55, Druid 100, Sorcerer 100; crescimento Knight +0.25, Paladin +0.40, Monk +0.55, Druid +0.75, Sorcerer +0.75 |
| `knowledge-base/character.md` (LOCKED) | Todo personagem tem HP e Mana; na morte volta à cidade com HP e Mana cheios. Sem números |
| `src/ProjectVictory.Domain/Character/CharacterClassDefinition.cs` (código em uso desde o Vertical Slice) | Knight 185 HP / 55 Mana, +15 / +5 por Level; Paladin 155/95, +10/+15; Sorcerer e Druid 125/140, +5/+30; Monk 150/80, +10/+10 |

**Conflito (R-021):** os números do documento e os do código não batem, e "+0.75" é ambíguo: pode ser 0,75 HP por Level ou um fator. Pela hierarquia do projeto, o código aprovado vem antes da documentação, mas o `progression.md` registra decisões de Game Design. **Não foi escolhido por intuição:** os valores do código ficam como **PROVISÓRIOS** (mantêm o personagem de Level 1 igual ao de hoje) e a pergunta vai para o PO.

## Escopo implementado

1. `Domain/Character/AttributeCurve` (Attribute Curve do ADR-029): máximo no Level L = inicial + floor(ganho × (L − 1)). O ganho aceita fração, para comportar o formato do `progression.md`.
2. `AttributeCurveTable.ForClass(class, ascension)` — **PROVISÓRIO**: lê os números de `CharacterClassDefinition` (fonte única). Para trocar pelos valores do PO, basta mudar esses dados; nenhuma regra muda. Por enquanto a curva é a mesma em todas as Ascensions (ADR-026: atributos mantidos).
3. `Character.MaximumHealth` e `MaximumMana` passam a ser **derivados** de Class + Level (o Level já vem de Experience + Ascension). Personagem sem definição de classe (construtor antigo) mantém o máximo recebido na criação.
4. **Persistência:** nada muda. As colunas `Health` e `Mana` continuam guardando os valores **atuais**; o máximo não era persistido. Nenhuma migration nem coluna foi apagada.

## Fora do escopo

- Vida e Mana **atuais** no Level-up (ex.: encher ao subir de Level) e regeneração (Food, `ascension.md`): ficam para o combate e o Hunt Loop.
- Modificadores de equipamento (ADR-030/034).

## Testes

`Domain.Tests/DerivedAttributeTests.cs`:
- as 5 classes no Level 1 e no Level 6;
- crescimento ao ganhar XP;
- curva fracionária;
- construtor antigo.

Também neste branch, as ressalvas da revisão do `PV-SEC-002`: teste HTTP em 6 linhas (claim × URL) e teste de que o login público continua funcionando com um token sem claim.

Resultado da solução: build com 0 avisos; **191 aprovados, 0 ignorados, 0 falhas**.

## Pergunta ao PO

**R-021:** quais são os valores oficiais de Vida e Mana (inicial e por Level) de Knight, Paladin, Sorcerer, Druid e Monk? Os de `knowledge-base/progression.md` (100 HP para todos, Knight +0.75 por Level…) ou os do jogo atual (Knight 185 HP, +15 por Level…)? Se valerem os do documento, "+0.75" significa 0,75 HP por Level?
