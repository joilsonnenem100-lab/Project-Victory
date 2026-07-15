# PV-VS-007 — Aplicar Progressão após Hunt

Status: Planned

Fase: FASE 1 — First Fun

Milestone: M1.2 — Progressão Inicial Validada

---

# Objetivo

Adicionar ao Vertical Slice a capacidade de aplicar a progressão obtida após a conclusão de uma Hunt.

---

# Capacidade Entregue

Um Character que conclui uma Hunt recebe sua progressão disponível e mantém seu estado atualizado para futuras ações.

---

# Critério de Aceite

- Uma Hunt concluída gera uma recompensa de progressão válida.
- A progressão recebida é aplicada ao Character.
- O estado atualizado do Character é mantido após a progressão.
- O Character pode continuar executando novas ações após receber progressão.
- A aplicação da progressão respeita as Foundations existentes.
- Nenhuma funcionalidade fora do escopo deste contrato é implementada.

---

# Fluxo Esperado

```
Character válido

↓

Executar Hunt

↓

Receber Resultado

↓

Processar Reward

↓

Aplicar Progressão

↓

Atualizar Character

↓

Continuar Gameplay Loop
```

---

# Fora do Escopo

- Balanceamento de experiência.
- Sistema completo de níveis.
- Skills.
- Classes.
- Equipamentos.
- Ascensão.
- Economia.
- Ranking.
- Sistemas sociais.
- Conteúdo Endgame.

---

# Dependências

- PV-VS-005 — Executar Primeira Hunt.
- PV-VS-006 — Executar Segunda Hunt.
- FT-007 — Reward Foundation.
- FT-008 — Progression Foundation.
- Arquitetura oficial dos domínios envolvidos.

---

# Referências

- PROJECT_COMPASS.md
- FT-007 — Reward Foundation.
- FT-008 — Progression Foundation.
- Documentação oficial de Character.
- ACCEPTANCE_CRITERIA.md

---

# Validação Manual

1. Possuir um Character válido.
2. Executar uma Hunt.
3. Confirmar geração de recompensa.
4. Aplicar progressão disponível.
5. Confirmar atualização do Character.
6. Executar uma nova ação utilizando o Character atualizado.

---

# Definição de Conclusão

A capacidade **Aplicar Progressão após Hunt** está disponível, demonstrando que o Vertical Slice consegue atualizar o estado do Character após uma ação de gameplay.

---

# Restrições

Este contrato não define:

- Arquitetura interna.
- Estrutura de código.
- Persistência.
- Fórmulas de progressão.
- Balanceamento.

A implementação deve seguir exclusivamente as definições fornecidas pela equipe Arquitetura.