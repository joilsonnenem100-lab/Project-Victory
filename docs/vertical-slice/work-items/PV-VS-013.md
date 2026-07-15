# PV-VS-013 — Finalizar Hunt sem Sucesso

Status: Planned

Fase: FASE 1 — First Fun

Milestone: M1.8 — Estados Alternativos da Hunt Validados

---

# Objetivo

Adicionar ao Vertical Slice a capacidade de finalizar uma Hunt quando o Character não consegue concluí-la com sucesso.

---

# Capacidade Entregue

Uma Hunt possui um resultado final mesmo quando o Character não obtém sucesso na execução.

---

# Critério de Aceite

- Um Character válido consegue iniciar uma Hunt.
- A Hunt pode finalizar com resultado sem sucesso.
- O resultado final da Hunt é informado corretamente.
- O sistema mantém o estado consistente do Character.
- Recompensas são aplicadas somente quando definidas pelas regras oficiais.
- O jogador pode continuar o fluxo após a finalização.
- Nenhuma funcionalidade fora do escopo deste contrato é implementada.

---

# Fluxo Esperado

```
Character válido

↓

Iniciar Hunt

↓

Executar Hunt

↓

Resultado da Hunt

↓

Sucesso ou Falha

↓

Processar Resultado

↓

Atualizar Estado

↓

Continuar Fluxo
```

---

# Fora do Escopo

- Balanceamento de dificuldade.
- Morte permanente.
- Penalidades complexas.
- PvP.
- Ranking.
- Sistemas sociais.
- Economia.
- Conteúdo Endgame.

---

# Dependências

- PV-VS-005 — Executar Primeira Hunt.
- PV-VS-012 — Executar Múltiplas Hunts com Estado Persistente.
- FT-005 — Hunt Foundation.
- Arquitetura oficial de Hunt e Combat.

---

# Referências

- PROJECT_COMPASS.md
- FT-005 — Hunt Foundation.
- Documentação oficial dos domínios envolvidos.
- ACCEPTANCE_CRITERIA.md

---

# Validação Manual

1. Possuir um Character válido.
2. Iniciar uma Hunt.
3. Executar uma Hunt que não finalize com sucesso.
4. Confirmar resultado final.
5. Confirmar consistência do estado do Character.
6. Confirmar possibilidade de continuar o fluxo.

---

# Definição de Conclusão

A capacidade **Finalizar Hunt sem Sucesso** está disponível, demonstrando que o Vertical Slice suporta diferentes resultados de execução sem quebrar o ciclo principal.

---

# Restrições

Este contrato não define:

- Arquitetura interna.
- Regras de balanceamento.
- Fórmulas de combate.
- Penalidades.
- Conteúdo de jogo.

A implementação deve seguir exclusivamente as definições fornecidas pela equipe Arquitetura.