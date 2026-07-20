# PV-VS-2.0-UPDATE-005 — Blocos de Implementação

## Bloco 1 — Character Creation Screen

Objetivo:

Criar a experiência visual de criação do primeiro Character.

Implementar:

- Tela de criação.
- Campo Nome.
- Validação básica.
- Ação Criar Character.

Validar:

Usuário
↓
Character Creation Screen
↓
Dados preparados


---

## Bloco 2 — Character Creation API Flow

Objetivo:

Integrar frontend com a capacidade existente.

Fluxo:

Frontend
↓
Character Service
↓
Create Character API
↓
Character Domain
↓
Persistência

Validar:

- Character criado.
- Account associada.
- Dados persistidos.


---

## Bloco 3 — First Character Entry

Objetivo:

Completar o primeiro ciclo do jogador.

Fluxo:

Criar Character
↓
Salvar
↓
Atualizar estado
↓
Entrar no Mundo
↓
Player Entry

Validar:

- Novo Character apresentado.
- Estado inicial correto.
- Hunt continua funcionando.
