# ADR-026 - Ascension Model

Status: ACCEPTED

Version: 1.0

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

- **Como o Level volta a 1 sem a Experience reiniciar.** Com Experience absoluta (ADR-023), a interpretação da Ascension N precisa de uma referência de partida (ex.: a Experience no momento da Ascension), que seria estado permanente novo. Decisão do Arquiteto, por contrato, quando a Ascension for implementada.
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
