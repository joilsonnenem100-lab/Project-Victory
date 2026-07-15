# Project-Victory
# PV-VS-001 — Criar Conta

Status: Planned

Fase: FASE 0 — Account System

Milestone: M0.1 — Conta Criada

---

# Objetivo

Permitir que um novo jogador crie uma conta válida no Project-Victory.

Ao término deste contrato deverá existir a capacidade de registrar uma nova conta no sistema.

---

# Capacidade Entregue

O sistema permite o cadastro de uma nova conta.

---

# Critério de Aceite

O contrato será considerado concluído quando for possível:

- Criar uma nova conta utilizando um username e uma password.
- Persistir a nova conta.
- Garantir que a senha nunca seja armazenada em texto puro.
- Impedir criação de contas inválidas conforme as regras definidas pelo domínio.
- Retornar sucesso quando a operação for válida.
- Retornar erro quando a operação for inválida.

---

# Fora do Escopo

Este contrato NÃO deverá implementar:

- Login.
- Sessão.
- Recuperação de senha.
- Alteração de senha.
- Confirmação por e-mail.
- Criação automática de Character.
- Qualquer funcionalidade relacionada ao gameplay.

---

# Referências Obrigatórias

A implementação deverá respeitar integralmente a documentação oficial existente do Project-Victory, incluindo os documentos relacionados ao domínio Account, à arquitetura e ao PROJECT_COMPASS.

Este contrato não substitui a documentação oficial.

---

# Validação Manual

Executar o fluxo abaixo:

1. Informar um username válido.
2. Informar uma password válida.
3. Solicitar a criação da conta.
4. Confirmar que a conta foi persistida.
5. Confirmar que a senha foi armazenada utilizando hash.
6. Confirmar que entradas inválidas retornam erro.

---

# Definição de Conclusão

Este contrato estará concluído quando um novo jogador conseguir criar uma conta válida no sistema e essa capacidade puder ser demonstrada em uma execução funcional.