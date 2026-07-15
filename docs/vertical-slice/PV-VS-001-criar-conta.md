# PV-VS-001 — Criar Conta

Status: Approved

Tipo: Vertical Slice Contract

---

# Objetivo

Implementar a capacidade de criação de uma nova conta no Project-Victory.

Este contrato representa a primeira funcionalidade de negócio do domínio Account.

---

# Escopo

Implementar exclusivamente:

- Caso de uso Criar Conta.
- Fluxo Application → Domain → Persistence.
- Criação de um Aggregate Account válido.
- Persistência da nova Account.

---

# Pré-condições

A fundação arquitetural do domínio Account deve existir.

Disponível:

- Account Aggregate Root.
- AccountId.
- Email Value Object.
- PasswordHash Value Object.
- IAccountRepository.
- IPasswordHasher.

---

# Fluxo Esperado

O fluxo oficial é:

Entrada da solicitação de criação de conta

↓

Application Layer

↓

Criação dos Value Objects necessários

↓

Criação do Aggregate Account

↓

Persistência através do IAccountRepository

↓

Conclusão do caso de uso

---

# Responsabilidades

## Application

Responsável por:

- receber a solicitação;
- coordenar o caso de uso;
- utilizar os contratos necessários;
- persistir o resultado.

Não contém regras de negócio do domínio.

---

## Domain

Responsável por:

- validar invariantes;
- garantir criação de uma Account válida;
- proteger o estado interno do Aggregate.

---

## Persistence

Responsável por:

- implementar IAccountRepository;
- armazenar a Account.

---

# Dados necessários

A criação de conta deve receber:

- Email
- Password

A senha não deve ser armazenada diretamente.

O fluxo deve utilizar:

Password

↓

IPasswordHasher

↓

PasswordHash

↓

Account

---

# Validações obrigatórias

Devem ser respeitadas:

- Email deve ser válido.
- Password deve atender às regras definidas pela arquitetura.
- PasswordHash nunca pode ser vazio.
- Account criada deve possuir identidade válida.

---

# Fora do Escopo

Não implementar:

- Login.
- Recuperação de senha.
- Alteração de senha.
- Perfil do jogador.
- Character.
- Inventário.
- Autenticação externa.
- Social login.
- Interface gráfica.

---

# Critérios de Aceite

O PV-VS-001 será considerado concluído quando:

- Uma nova conta puder ser criada pelo fluxo definido.
- O Aggregate Account for criado corretamente.
- A persistência ocorrer através do contrato oficial.
- A solução continuar compilando.
- Os testes existentes continuarem passando.

---

# Restrições

A implementação não deve:

- alterar a arquitetura do domínio Account;
- criar novos padrões arquiteturais;
- modificar outros domínios sem necessidade;
- adicionar funcionalidades fora deste contrato.

---

# Resultado Esperado

Ao concluir este contrato:

O Project-Victory possuirá o primeiro fluxo funcional do domínio Account.

A conta criada estará pronta para evoluir através dos próximos contratos do Vertical Slice.