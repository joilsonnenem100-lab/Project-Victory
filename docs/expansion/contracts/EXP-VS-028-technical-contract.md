# EXP-VS-028 — Technical Contract

## Nome

Criar Gameplay Session Context

---

# Objetivo Técnico

Criar a representação inicial do contexto temporário de uma sessão de gameplay.

Este PV-VS cria somente a estrutura de coordenação da sessão.

Não cria persistência de sessão.

---

# Contexto Arquitetural

A sessão de gameplay representa o estado temporário necessário para execução do fluxo do jogador.

Ela não substitui Character nem os domínios existentes.

Fluxo:
