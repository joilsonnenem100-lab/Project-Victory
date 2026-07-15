# PV-VS-004 — Entrar no Jogo

Status: Planned

Fase: FASE 0 — Game Entry

Milestone: M0.4 — Primeiro Acesso ao Jogo

---

# Objetivo

Adicionar ao Vertical Slice a capacidade de um jogador autenticado acessar o ambiente inicial do jogo utilizando seu Character existente.

---

# Capacidade Entregue

Um jogador autenticado consegue iniciar o fluxo de entrada no jogo utilizando um Character válido associado à sua conta.

---

# Critério de Aceite

- Um jogador autenticado consegue iniciar a entrada no jogo.
- O sistema identifica a Account autenticada.
- O Character associado à Account é disponibilizado conforme a arquitetura oficial.
- O fluxo utiliza a fundação existente de Game Entry.
- O jogador chega ao ponto inicial definido pelo Vertical Slice.
- Nenhuma funcionalidade fora do escopo deste contrato é implementada.

---

# Fora do Escopo

- Runtime completo.
- Mundo aberto.
- Multiplayer.
- Movimento.
- Combate.
- Hunt.
- NPCs.
- Inventário.
- Equipamentos.
- Sistemas sociais.

---

# Referências

- PROJECT_COMPASS.md
- FT-004 — Game Entry Foundation
- Documentação oficial do domínio Account.
- Documentação oficial do domínio Character.
- ACCEPTANCE_CRITERIA.md

---

# Validação Manual

1. Autenticar uma conta válida.
2. Possuir um Character criado.
3. Executar entrada no jogo.
4. Confirmar que o contexto da Account está disponível.
5. Confirmar que o Character correto está associado ao fluxo.
6. Confirmar que o jogador está pronto para iniciar a próxima etapa do Vertical Slice.

---

# Definição de Conclusão

A capacidade "Entrar no Jogo" está disponível, permitindo que um jogador autenticado avance para a próxima etapa do Vertical Slice.