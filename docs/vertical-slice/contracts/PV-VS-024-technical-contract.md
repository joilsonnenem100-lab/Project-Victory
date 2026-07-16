# PV-VS-024 — Technical Contract

## Nome

Executar Ação de Combate

---

# Objetivo Técnico

Permitir que um Combat Context ativo processe uma ação de combate válida dentro do fluxo oficial de Combat.

A ação deve alterar o estado interno do contexto de combate conforme regras definidas, sem implementar mecânicas avançadas.

---

# Escopo Autorizado

Implementar exclusivamente:

- receber uma ação de combate;
- validar Combat Context ativo;
- processar a ação dentro do fluxo Combat;
- atualizar estado interno permitido;
- retornar resultado da ação.

---

# Pré-condição

Somente Combat Context em estado permitido pode processar ação.

Estado permitido:
