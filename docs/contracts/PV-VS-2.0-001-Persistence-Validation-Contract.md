docs/contracts/PV-VS-2.0-001-Persistence-Validation-Contract.md

# PV-VS-2.0-001 — Persistence Validation Contract

## Tipo

Validation Contract

---

## Objetivo

Definir a validação obrigatória da implementação de persistência antes da evolução para os próximos contratos do Vertical Slice 2.0.

---

## Finalidade

Garantir que a aplicação possui armazenamento confiável antes da criação de novos sistemas dependentes.

---

## Escopo de validação

Validar:

- Persistência de Account.
- Persistência de Character.
- Recuperação de dados após reinício.
- Integração entre Application e Persistence.

---

## Fluxo de validação

### Account

Criar Account

↓

Salvar no banco

↓

Encerrar aplicação

↓

Iniciar aplicação novamente

↓

Realizar Login

↓

Confirmar autenticação

---

### Character

Criar Character

↓

Salvar no banco

↓

Encerrar aplicação

↓

Iniciar aplicação novamente

↓

Consultar Characters

↓

Confirmar existência do Character

---

## Requisitos arquiteturais

Validar:

- Repository continua isolando persistência.
- Application não depende da implementação do banco.
- Domain não possui dependência de infraestrutura.
- API continua utilizando os casos de uso existentes.

---

## Critérios de aceite

Considerado aprovado quando:

- Dados permanecem após reinício.
- Login continua funcionando.
- Character continua associado à Account correta.
- Fluxos existentes continuam funcionando.

---

## Critérios de bloqueio

Reprovar quando:

- Dados são perdidos após reinício.
- Regras de negócio foram movidas para persistência.
- Camadas foram acopladas incorretamente.
- Fluxos existentes foram quebrados.

---

## Validação técnica

Executar:

dotnet test ProjectVictory.sln

---

## Dependências

Depende de:

PV-VS-2.0-001 — Database Implementation Contract

---

## Próxima etapa

Após aprovação:

PV-VS-2.0-002 — Account Experience Foundation

---

## Status

PV-VS-2.0-001 — Validação de persistência definida.