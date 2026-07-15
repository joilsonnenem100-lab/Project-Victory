# PV-VS-002 — Login

Status: Planned

Fase: FASE 0 — Account System

Milestone: M0.2 — Conta Autenticada

---

# Objetivo

Adicionar ao Project-Victory a capacidade de autenticar uma conta existente utilizando email e senha.

---

# Capacidade Entregue

Uma conta válida consegue autenticar-se com sucesso no sistema.

---

# Critério de Aceite

- É possível autenticar utilizando email e senha.
- Credenciais inválidas são rejeitadas.
- O comportamento respeita integralmente a arquitetura oficial do Project-Victory.
- O comportamento respeita a documentação oficial do domínio Account.
- Nenhuma funcionalidade fora do escopo deste contrato é implementada.

---

# Fora do Escopo

- Criação de Character.
- Entrada no jogo.
- Recuperação de senha.
- Alteração de senha.
- Gameplay.

---

# Referências

- PROJECT_COMPASS.md
- Documentação oficial do domínio Account.
- ADRs aplicáveis ao domínio Account.
- ACCEPTANCE_CRITERIA.md

---

# Validação Manual

1. Registrar uma conta válida.
2. Autenticar utilizando email e senha.
3. Confirmar autenticação com sucesso.
4. Tentar autenticar utilizando senha inválida.
5. Confirmar que a autenticação é rejeitada.

---

# Definição de Conclusão

A capacidade **Login** está disponível, atende aos critérios de aceite deste contrato e identifica corretamente uma conta autenticada.

A navegação após a autenticação não faz parte deste contrato.