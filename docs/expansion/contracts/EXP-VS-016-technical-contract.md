# EXP-VS-016 — Technical Contract

## Nome

Criar Persistent Character State

---

# Objetivo Técnico

Criar a representação persistível do estado principal do Character.

Este PV-VS cria somente uma estrutura de dados persistível.

Não cria regras de domínio.

Não substitui o Aggregate Root Character.

---

# Contexto Arquitetural

O Character continua sendo o proprietário das próprias regras e invariantes.

Persistence possui apenas responsabilidade de representar estado para armazenamento e recuperação.

Fluxo:
