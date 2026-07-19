docs/contracts/PV-MILESTONE-003-Risk-Register.md

# PV-MILESTONE-003 — Vertical Slice 2.0 Risk Register

## Tipo

Architecture Control Document

---

## Objetivo

Registrar riscos conhecidos durante a evolução do Vertical Slice 2.0 e definir ações preventivas.

---

## Risco 1 — Evolução sem persistência adequada

### Descrição

Criar novos sistemas utilizando armazenamento temporário antes da fundação de banco de dados.

### Impacto

Retrabalho para migração futura.

### Mitigação

Implementar Persistence Foundation antes da expansão dos sistemas de gameplay.

---

## Risco 2 — Expansão excessiva de escopo

### Descrição

Adicionar sistemas completos de MMORPG durante a construção do protótipo.

### Impacto

Aumento de complexidade e atraso das entregas.

### Mitigação

Manter cada contrato limitado ao escopo aprovado.

---

## Risco 3 — Acoplamento entre sistemas

### Descrição

Permitir que sistemas como Combat, Reward, Monster e Hunt controlem responsabilidades de outros domínios.

### Impacto

Dificuldade de evolução e manutenção.

### Mitigação

Manter responsabilidades separadas entre camadas e contextos.

---

## Risco 4 — Alterações sem contrato aprovado

### Descrição

Implementar funcionalidades antes da definição arquitetural.

### Impacto

Conflitos e retrabalho.

### Mitigação

Seguir fluxo:

Arquitetura

↓

Contrato

↓

Engenharia

↓

Validação

---

## Risco 5 — Falta de validação funcional

### Descrição

Validar apenas compilação sem testar o fluxo real do jogador.

### Impacto

Problemas de integração descobertos tardiamente.

### Mitigação

Executar validação funcional após cada fundação.

---

## Risco 6 — Decisões técnicas conflitarem com visão do produto

### Descrição

Criar soluções técnicas que dificultem a experiência desejada pelo Product Owner.

### Impacto

Necessidade de reconstrução de sistemas.

### Mitigação

Manter comunicação entre Product Owner, Arquitetura e Engenharia.

---

## Critério de revisão

Este documento deve ser revisado quando:

- Novo sistema entrar no escopo.
- Uma decisão arquitetural alterar dependências.
- Um risco identificado causar impacto real.

---

## Status

PV-MILESTONE-003 — Registro de riscos definido.