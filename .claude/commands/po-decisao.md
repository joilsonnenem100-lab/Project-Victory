---
description: Aplica uma mudança de Game Design do PO (ex.: "PO-19: ...") no PO-DECISOES-BASE.md, no decision-log, nos documentos afetados e no INDEX
argument-hint: "PO-xx: <nova decisão do PO>"
---

# /po-decisao — manutenção das decisões de Game Design do PO

Mudança recebida: **$ARGUMENTS**

Base: `DEC-20261107-01` (o `docs/product/PO-DECISOES-BASE.md` é a fonte da verdade de Game Design; só o PO altera) e ADR-016 (o PO decide Game Design). Este comando **só registra** a decisão do PO: não interpreta, não completa lacunas e não decide por ele.

## Limites (inegociáveis)
- **Nunca** alterar `src/`, `tests/` nem migrations. Nenhum código.
- Não apagar texto de documentos: só **nota de superação**. Arquivos só se movem com `git mv` (registro no `CLEANUP-MANIFEST`).
- Documento **LOCKED** só recebe nota citando o `DEC-` novo como revisão formal documentada (mesmo rito do `DEC-20261106-13` e do `DEC-20261107-02`).
- Se a mudança for ambígua, contradizer outro PO-xx ou não trouxer o ID: **pare e pergunte ao PO** antes de escrever.
- Economia de contexto (`DEC-20261106-45`): leia `docs/INDEX.md` e use busca (Grep) por termos; não releia documentos inteiros.

## Passos, em ordem
1. **PO-DECISOES-BASE.md** — atualize a linha do `PO-xx` (Decisão, Observação do PO transcrita sem mudar o sentido, Aplicação). ID novo: acrescente na seção certa ou em "Decisões complementares". Atualize **Versão** (x.y+1), **Data** e o campo **Última alteração** (data, PO-xx, resumo de uma linha). Ajuste o "Backlog derivado" se a mudança criar ou retirar itens.
2. **decision-log.md** (`developer-guide/architecture/`) — registre **um** `DEC-AAAAMMDD-NN` novo (próximo número do dia), status "Accepted (Product Owner, ADR-016)", citando o `PO-xx`, o que muda e qual `DEC-`/documento antigo ele emenda ou supera. Nos `DEC-` antigos afetados, acrescente nota de superação apontando para o novo.
3. **Varredura** — procure os termos da mudança (nomes do tema, sinônimos, termos em inglês, IDs de R-xxx e DEC- relacionados) em `knowledge-base/`, `developer-guide/` (ADRs, domínio, roadmaps, project-state), `docs/contracts/active/` e `docs/architecture/master/`. Em cada afirmação que contradiz a decisão nova, acrescente logo abaixo:
   `> *Nota de superação (AAAA-MM-DD, \`DEC-…\`, PO-xx em \`docs/product/PO-DECISOES-BASE.md\`): <o que passa a valer>.*`
   Pastas históricas/arquivadas (`historico/`, `archive/`, `docs/character e conceitos/`, `docs/expansion/`, `docs/vertical-slice/`, `docs/Visual-Identity/Antigos/`) não recebem nota: vale a regra 3 do arquivo do PO.
4. **Conflito com código** — confira com Grep em `src/` (só leitura). Se o código contradiz a decisão, registre uma linha nova `R-NNN` em `project-state.md`, seção "Revisões Arquiteturais Pendentes" (conflito, fontes, resolução proposta, gate). Candidato a contrato vai para o `roadmap-to-playable.md`.
5. **docs/INDEX.md** — acrescente o `DEC-` novo em "Últimas decisões" e atualize a data do cabeçalho.
6. **Relatório no fim** (curto): versão nova do arquivo do PO; `DEC-` criado; lista de arquivos alterados (uma linha cada, com o que mudou); conflitos com código (R-xxx); perguntas ao PO, se houver. Não commitar sem ordem; se houver ordem, commit `PV-GOV-NNN - <resumo>` numa branch nova (nunca na `main`, nunca `push --force`).
