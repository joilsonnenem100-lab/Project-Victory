# PV-VS-008 — Reexecutar Hunt com Character Evoluído

Status: Planned

Fase: FASE 1 — First Fun

Milestone: M1.3 — Gameplay Loop Inicial Validado

---

# Objetivo

Adicionar ao Vertical Slice a capacidade de um jogador continuar executando Hunts utilizando um Character que já recebeu progressão.

---

# Capacidade Entregue

Um Character que possui progresso aplicado consegue executar uma nova Hunt utilizando seu estado atualizado.

---

# Critério de Aceite

- Um Character evoluído consegue iniciar uma nova Hunt.
- O estado atual do Character é utilizado durante a execução.
- A Hunt é concluída normalmente.
- As recompensas são processadas novamente.
- A progressão continua sendo aplicada corretamente.
- O jogador consegue permanecer no ciclo principal do Vertical Slice.

---

# Fluxo Esperado

```
Character atualizado

↓

Iniciar Hunt

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

Continuar Loop
```

---

# Fora do Escopo

- Novas classes.
- Skills.
- Equipamentos.
- Balanceamento.
- Conteúdo adicional.
- PvP.
- Economia.
- Sistemas sociais.
- Endgame.

---

# Dependências

- PV-VS-005 — Executar Primeira Hunt.
- PV-VS-006 — Executar Segunda Hunt.
- PV-VS-007 — Aplicar Progressão após Hunt.
- FT-005 — Hunt Foundation.
- FT-007 — Reward Foundation.
- FT-008 — Progression Foundation.

---

# Referências

- PROJECT_COMPASS.md
- Documentações oficiais dos domínios envolvidos.
- Foundations aprovadas.
- ACCEPTANCE_CRITERIA.md

---

# Validação Manual

1. Possuir um Character criado.
2. Executar uma Hunt.
3. Receber progressão.
4. Confirmar estado atualizado do Character.
5. Executar uma nova Hunt.
6. Confirmar utilização do estado atualizado.
7. Confirmar novo resultado e progressão.

---

# Definição de Conclusão

A capacidade **Reexecutar Hunt com Character Evoluído** está disponível, demonstrando que o Vertical Slice suporta um ciclo contínuo de gameplay após progressão.

---

# Restrições

Este contrato não define:

- Arquitetura interna.
- Estrutura de código.
- Balanceamento.
- Fórmulas de progressão.
- Novos sistemas.

A implementação deve seguir exclusivamente as definições fornecidas pela equipe Arquitetura.