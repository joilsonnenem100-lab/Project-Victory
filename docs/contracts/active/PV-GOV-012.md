# PV-GOV-012 — Resumo de uma página e economia de contexto

Origem: ordem do PO e do Arquiteto, MODO FILA 8, item 1 (2026-11-06). Decisão: `DEC-20261106-45`.

## Escopo
1. Criar `docs/INDEX.md` (no máximo uma página): estado atual, regras vivas, últimas decisões, onde está cada coisa, próxima fila.
2. `CLAUDE.md`: leitura obrigatória passa a ser `CLAUDE.md` → `docs/INDEX.md`; ADRs e contratos só quando o item os cita; regra de economia de contexto; regra "uma fila por sessão"; relatório final com no máximo uma página.
3. Mover relatórios de filas encerradas para `docs/architecture/historico/` com `git mv` (sem apagar), registrar no manifesto e corrigir as referências.

## Fora do escopo
Código; conteúdo dos relatórios movidos; documentos LOCKED (só caminho de referência no `roadmap.md`).

## Aceite
INDEX ≤ 1 página; nenhum arquivo apagado; nenhum link quebrado para os relatórios movidos (`grep`); build e testes inalterados (documentação apenas).
