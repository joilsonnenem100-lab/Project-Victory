# PV-VS-001 — Criar Conta

Status: Planned

Fase: FASE 0 — Account System

Milestone: M0.1 — Conta Criada

---

# Objetivo

Adicionar ao Project-Victory a capacidade de registrar uma nova conta utilizando email e senha.

---

# Capacidade Entregue

Um novo jogador consegue criar uma conta válida para acessar o Project-Victory.

---

# Critério de Aceite

- É possível registrar uma conta utilizando email e senha.
- A conta é persistida conforme a arquitetura oficial do Project-Victory.
- A senha nunca é armazenada em texto puro.
- Contas inválidas são rejeitadas conforme as regras do domínio Account.
- Nenhuma funcionalidade fora do escopo deste contrato é implementada.

---

# Fora do Escopo

- Login.
- Sessão.
- Recuperação de senha.
- Alteração de senha.
- Criação de Character.
- Gameplay.

---

# Referências

- PROJECT_COMPASS.md
- Documentação oficial do domínio Account.
- ADRs aplicáveis ao domínio Account.
- ACCEPTANCE_CRITERIA.md

---

# Validação Manual

1. Registrar uma conta utilizando um email válido e uma senha válida.
2. Confirmar que a conta foi persistida.
3. Confirmar que a senha foi armazenada utilizando hash.
4. Tentar registrar uma conta inválida.
5. Confirmar que o sistema rejeita a operação conforme as regras do domínio.

---

# Definição de Conclusão

A capacidade **Criar Conta** está disponível, atende aos critérios de aceite deste contrato e respeita integralmente a arquitetura oficial do Project-Victory.