# PV-VS-006 — Executar Segunda Hunt

Status: Planned

Fase: FASE 1 — First Fun

Milestone: M1.1 — Loop de Gameplay Validado

---

# Objetivo

Adicionar ao Vertical Slice a capacidade de um jogador executar uma nova Hunt após concluir a primeira, validando a continuidade do ciclo principal de gameplay.

---

# Capacidade Entregue

Um jogador que concluiu uma Hunt consegue iniciar uma nova Hunt utilizando seu Character existente e continuar o fluxo principal do jogo.

---

# Critério de Aceite

- Um Character válido consegue iniciar uma nova Hunt.
- A nova Hunt utiliza o estado atual do Character.
- A execução da Hunt possui um resultado final.
- O jogador recebe as recompensas definidas pelas Foundations existentes.
- O estado de progressão do Character é mantido conforme a arquitetura oficial.
- O jogador consegue continuar o ciclo de Hunts.

---

# Fluxo Esperado

```
Character válido

↓

Iniciar Hunt

↓

Executar Hunt

↓

Finalizar Hunt

↓

Receber Resultado

↓

Aplicar Recompensas

↓

Atualizar Estado do Character

↓

Pronto para próxima Hunt
```

---

# Fora do Escopo

- Novos sistemas de combate.
- PvP.
- Multiplayer.
- Bosses.
- Novas áreas.
- Economia avançada.
- Equipamentos.
- Skills.
- Balanceamento definitivo.
- Conteúdo Endgame.

---

# Dependências

- PV-VS-005 — Executar Primeira Hunt.
- FT-005 — Hunt Foundation.
- Foundations oficiais de Reward e Progression, quando aplicáveis.

---

# Referências

- PROJECT_COMPASS.md
- FT-005 — Hunt Foundation.
- FT-006 — Combat Foundation.
- FT-007 — Reward Foundation.
- FT-008 — Progression Foundation.
- Documentação oficial dos domínios envolvidos.
- ACCEPTANCE_CRITERIA.md

---

# Validação Manual

1. Possuir um Character válido.
2. Entrar no jogo.
3. Executar uma Hunt concluída com sucesso.
4. Confirmar recebimento das recompensas.
5. Iniciar uma nova Hunt.
6. Confirmar execução da nova Hunt.
7. Confirmar resultado final.
8. Confirmar manutenção do estado atualizado do Character.

---

# Definição de Conclusão

A capacidade **Executar Segunda Hunt** está disponível, demonstrando que o ciclo principal do Vertical Slice pode continuar após a primeira execução.

---

# Restrições

Este contrato não define:

- Arquitetura interna.
- Estrutura de código.
- Persistência.
- Modelos de domínio.
- Estratégias de implementação.

A implementação deve seguir exclusivamente as definições fornecidas pela equipe Arquitetura.