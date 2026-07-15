# PV-VS-009 — Persistir Estado do Character após Progressão

Status: Planned

Fase: FASE 1 — First Fun

Milestone: M1.4 — Estado Persistente Validado

---

# Objetivo

Adicionar ao Vertical Slice a capacidade de manter o estado atualizado do Character após a aplicação de progressão.

---

# Capacidade Entregue

Um Character que recebe progressão durante uma Hunt mantém seu estado atualizado para futuras interações do jogador.

---

# Critério de Aceite

- Um Character pode receber progressão após uma Hunt.
- O estado atualizado do Character é mantido.
- O Character recuperado posteriormente mantém os dados atualizados.
- O fluxo respeita os contratos oficiais de persistência existentes.
- Nenhuma funcionalidade fora do escopo deste contrato é implementada.

---

# Fluxo Esperado

```
Character

↓

Executar Hunt

↓

Receber Reward

↓

Aplicar Progressão

↓

Atualizar Estado

↓

Persistir Estado

↓

Recuperar Character Atualizado
```

---

# Fora do Escopo

- Banco de dados definitivo.
- Migrações.
- Backup.
- Histórico completo de progressão.
- Economia.
- Inventário.
- Equipamentos.
- Skills.
- Sistemas futuros.

---

# Dependências

- PV-VS-007 — Aplicar Progressão após Hunt.
- PV-VS-008 — Reexecutar Hunt com Character Evoluído.
- FT-008 — Progression Foundation.
- Arquitetura oficial de persistência.

---

# Referências

- PROJECT_COMPASS.md
- FT-007 — Reward Foundation.
- FT-008 — Progression Foundation.
- Documentação oficial de Character.
- ACCEPTANCE_CRITERIA.md

---

# Validação Manual

1. Criar um Character.
2. Executar uma Hunt.
3. Receber progressão.
4. Atualizar o estado do Character.
5. Encerrar o fluxo atual.
6. Recuperar o Character.
7. Confirmar que o estado atualizado permanece disponível.

---

# Definição de Conclusão

A capacidade **Persistir Estado do Character após Progressão** está disponível, demonstrando que o Vertical Slice mantém a evolução do jogador entre ciclos.

---

# Restrições

Este contrato não define:

- Arquitetura interna.
- Tecnologia de persistência.
- Estrutura de banco de dados.
- Modelos adicionais.

A implementação deve seguir exclusivamente as definições fornecidas pela equipe Arquitetura.