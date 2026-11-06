# PV-GOV-009 — Decisões do PO da tarde de 2026-11-06

Status: **CONCLUÍDO** (branch `chief/q4-gov-009`; só `.md`)
Emitido por: Arquitetura, 2026-11-06 (ordem "MODO FILA 4", item 1). Revisão formal dos documentos LOCKED afetados **autorizada pelo Arquiteto-Chefe** na mesma ordem.
Lastro: ADR-016 do `decision-log.md` (o PO decide Game Design); `roadmap.md`/`CLAUDE.md` (LOCKED só muda por revisão documentada).

## Decisões registradas (`decision-log.md`)

| Item da ordem | Entrada | Emendas (sem apagar texto) |
|---|---|---|
| a) Regra permanente: referência Tibia | `DEC-20261106-15` | `CLAUDE.md` (Regras inegociáveis) |
| b) Combate simples; Tibia como base; elementos depois | `DEC-20261106-16` | — |
| c) Sem CAP | `DEC-20261106-17` | — |
| d) Vida e Mana do Tibia (resolve R-021) | `DEC-20261106-18` | `knowledge-base/progression.md` (HP/Mana) |
| e) Ascension derivada do Level; sem teto | `DEC-20261106-19` | `ADR-026` (v1.1); `progression.md` (Level/Ascension); `DEC-20261106-10/11`; R-022 sem efeito |
| f) Conexão e Hunt; diretriz do M6 | `DEC-20261106-20` | `ADR-057` (v1.2); `DEC-20261106-05`; M6 do `roadmap-to-playable.md` |
| g) Treino (antigo Offline Training) | `DEC-20261106-21` | `roadmap.md` (Fase 2, LOCKED); `knowledge-base/training.md` (LOCKED); `ADR-057` (v1.2); `progression.md` (Trainer); M6 |
| h) Fim de suprimento não encerra; morte → tela inicial | `DEC-20261106-22` | `DEC-20261106-04` e `PV-SUPPLY-010` (trecho marcado **SUPERADO**) |
| i) 4 skills por classe | `DEC-20261106-23` | — |
| j) Cidade "Victoria" | `DEC-20261106-24` | — |
| k) Remoções R-020 e R-031 autorizadas | `DEC-20261106-25` | `project-state.md` |

## Verificação dos valores do Tibia (item d)

- **Fonte lida:** o código aberto do NabBot, `cogs/utils/tibia.py` (Apache 2.0), que implementa as fórmulas da TibiaWiki. A TibiaWiki (`tibia.fandom.com`, `tibiawiki.com.br`) recusou acesso automático (HTTP 402/403).
- **Resultado:**
  - Level 1 = **150 de vida e 55 de mana**; +5/+5 por Level até o 8 (sem vocação);
  - do Level 8 (185 de vida, 90 de mana) em diante, os ganhos da classe: Knight +15/+5, Paladin +10/+15, Druid e Sorcerer +5/+30;
  - os ganhos **batem com os definidos pelo PO**.
- **Monk:** não está na fonte, então fica **PROVISÓRIO** (mesmo padrão até o Level 8; depois +12/+15, do PO).

## Outras atualizações

- `project-state.md`: R-021 resolvida, R-022 sem efeito, R-020 e R-031 autorizadas.
- `roadmap-to-playable.md`: linha da Fila 4 e nota do M6, com as respostas às dúvidas de treino e de conexão.

Build e testes: não aplicáveis (só `.md`); a suíte segue a da `main`.
