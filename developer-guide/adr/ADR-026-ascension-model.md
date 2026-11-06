# ADR-026 - Ascension Model

Status: ACCEPTED

Version: 1.2

> **Emenda v1.2 (2026-11-06, Fila 5, `DEC-20261106-30`, revisão formal autorizada pelo PO e pelo Arquiteto-Chefe):** a regra da v1.1 ("Ascension 1 inicial; nova a cada 500; máximo 4") foi **SUPERADA** pelos **títulos por Level**: Level 1–499 **Novice** (tier 0) · 500–999 **Ascension 1** (tier 1) · 1000–1499 **Ascension 2** (tier 2) · 1500–1999 **Ascension 3** (tier 3) · 2000–2499 **Ascension 4** (tier 4) · 2500 em diante **Legend** (tier 5, permanente, sem teto de Level). Tier numérico 0 a 5, derivado do Level; rótulos e faixas em **configuração**. Hunt areas exigem tier mínimo (Spider Cave = tier 0). O Level nunca reseta e não tem teto. Resolve R-034. Implementação: `PV-PROG-013`.

> **Emenda v1.1 (2026-11-06, `DEC-20261106-19`, revisão formal autorizada pelo Arquiteto-Chefe):** o modelo abaixo foi **SUPERADO** quanto ao Level. **A Ascension é DERIVADA do Level**: Ascension 1 inicial; uma nova a cada 500 Levels (2 no 500, 3 no 1000, 4 no 1500). O máximo atual é 4, com máximo e passo em configuração e conteúdo por dados. **O Level nunca volta a 1 e não há Level máximo**; o desbloqueio é automático e o jogador escolhe a Ascension em que caça. Os itens "Level retorna para 1", "limite de Level 500" e "Em aberto: como o Level volta a 1" deixam de valer (R-022 sem efeito). O texto original foi mantido por histórico.

Data: 2026-11-06 · Aceite: Product Owner (ordem de 2026-11-06, registrada em `PV-GOV-006`)

---

# Contexto

ADR-023, ADR-025, ADR-027, ADR-028, ADR-029 e ADR-031 tratam a Ascension como parte da progressão, mas nenhum ADR a definia. O número 026 estava vago (levantamento em `docs/architecture/PV-GOV-005-RELATORIO-2026-11-06.md`).

A mecânica está documentada em `knowledge-base/progression.md` (seções "Level" e "Ascension"). Este ADR registra **somente** o que está documentado ali e nas decisões do PO; não acrescenta regra de jogo.

---

# Decisão

## Mecânica (fonte: `knowledge-base/progression.md`)

- O personagem não possui Level máximo; existe o sistema de Ascension.
- **Cada Ascension possui limite de Level 500.**
- Após atingir esse limite, o jogador **poderá** evoluir gratuitamente para a próxima Ascension.
- Ao realizar uma Ascension:
  - o **Level retorna para 1**;
  - os **atributos permanentes são mantidos**;
  - **novas regiões** são desbloqueadas;
  - **novos monstros** tornam-se disponíveis;
  - **novos equipamentos** podem ser obtidos.
- O jogador continua podendo acessar todas as regiões anteriores; o acesso às novas regiões é opcional.

## Curva de Experience (fonte: `DEC-20261106-11`, PO)

- **A curva de Experience é a mesma em todas as Ascensions**; só muda o conteúdo desbloqueado.
- Cumpre o ADR-025 ("cada Ascension referencia sua própria Experience Table"): todas as Ascensions referenciam tabelas geradas pela **mesma** fórmula (`DEC-20261106-10`).

## Modelo arquitetural (fonte: ADR-023, ADR-025, ADR-027, ADR-028)

- A Ascension atual é **estado permanente** do Character (ADR-028).
- A Ascension seleciona a Experience Table ativa (ADR-025; `developer-guide/domain/experience-table.md`, "Seleção").
- O Level é derivado de Experience + Ascension + Experience Table (ADR-027) e nunca passa de 500 dentro de uma Ascension.
- A Experience continua absoluta: **nunca reinicia** com a Ascension (ADR-023). O que volta a 1 é o Level derivado.

---

# Em aberto (sem respaldo em documento; não decidido aqui)

- **Como o Level volta a 1 sem a Experience reiniciar.** Com Experience absoluta (ADR-023), a interpretação da Ascension N precisa de alguma referência de partida, que seria estado permanente novo (o modelo dessa referência não é decidido aqui). Decisão do Arquiteto, por contrato, quando a Ascension for implementada.
- **Quantas Ascensions existem**, o que exatamente é desbloqueado em cada uma e se a Ascension é automática ou pedida pelo jogador ("poderá evoluir"). Decisão do PO.
- **Por qual caso de uso a Ascension muda.** O ADR-024 trata de ganhos/recompensas e não cita Ascension. Decisão do Arquiteto, por contrato.
- O que acontece com a Experience obtida depois do Level 500 e antes da Ascension. Ela é acumulada normalmente (ADR-023); o efeito dela na Ascension seguinte é decisão do PO.

Até essas decisões, o código trabalha apenas com a **primeira Ascension** (todos os personagens).

---

# Consequências

Benefícios:

- o conceito que seis ADRs já usavam passa a ter definição;
- a Experience Table ganha um teto explícito (Level 500), cumprindo a regra "último nível válido" de `experience-table.md`.

Custos:

- a mudança de Ascension ainda exige contrato próprio (itens em aberto acima).

---

# Restrições

- A Ascension nunca altera a Experience (ADR-023).
- O Level nunca controla desbloqueios (ADR-027); quem desbloqueia conteúdo é a Ascension.

---

# Referências

- `knowledge-base/progression.md`
- ADR-023, ADR-025, ADR-027, ADR-028
- `developer-guide/domain/experience-table.md`, `developer-guide/domain/level.md`
- `developer-guide/architecture/decision-log.md`: `DEC-20261106-10`, `DEC-20261106-11`
