# PV-VS-003 — Criar Primeiro Character

Status: Planned

Fase: FASE 0 — Character System

Milestone: M0.3 — Primeiro Character Disponível

---

# Objetivo

Adicionar ao Vertical Slice a capacidade de um jogador autenticado criar seu primeiro Character.

---

# Capacidade Entregue

Um jogador autenticado consegue criar um Character inicial válido associado à sua conta.

---

# Critério de Aceite

- Um jogador autenticado consegue iniciar a criação de um Character.
- O Character criado fica associado corretamente à conta autenticada.
- Dados inválidos são rejeitados conforme as regras oficiais do domínio Character.
- A implementação utiliza a fundação existente do domínio Character.
- O comportamento respeita integralmente a arquitetura oficial do Project-Victory.
- Nenhuma funcionalidade fora do escopo deste contrato é implementada.

---

# Fora do Escopo

- Criação de múltiplos Characters.
- Seleção de Character.
- Classes.
- Skills.
- Equipamentos.
- Inventário.
- Progressão.
- Gameplay.
- Customização visual avançada.

---

# Referências

- PROJECT_COMPASS.md
- Documentação oficial do domínio Character.
- ADRs aplicáveis ao domínio Character.
- ACCEPTANCE_CRITERIA.md

---

# Validação Manual

1. Autenticar uma conta válida.
2. Solicitar criação do primeiro Character.
3. Informar os dados necessários.
4. Confirmar criação.
5. Confirmar associação entre conta e Character.
6. Confirmar rejeição de dados inválidos.

---

# Definição de Conclusão

A capacidade **Criar Primeiro Character** está disponível, atende aos critérios de aceite deste contrato e permite que o jogador avance para a entrada no jogo.