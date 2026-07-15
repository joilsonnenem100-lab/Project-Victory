# FT-002 — Account Creation Flow Foundation

Status: Approved

Tipo: Foundation Task

---

# Objetivo

Definir a arquitetura oficial necessária para implementação futura do caso de uso Criar Conta.

Esta Foundation Task não implementa o caso de uso.

Ela apenas define responsabilidades, contratos e fluxo arquitetural.

---

# Fluxo Oficial

O fluxo de criação de conta seguirá:

Request
    ↓
Application Layer
    ↓
Account Aggregate
    ↓
Repository Contract
    ↓
Persistence

---

# Responsabilidade da Application

A camada Application é responsável por:

- receber a intenção de criar uma conta;
- coordenar o caso de uso;
- solicitar criação do Aggregate;
- utilizar contratos externos;
- persistir alterações através do repositório.

A Application não contém regras de negócio do Account.

---

# Responsabilidade do Domain

O domínio Account é responsável por:

- garantir suas invariantes;
- criar um Aggregate válido;
- validar seus Value Objects;
- impedir estados inválidos.

---

# Criação do Aggregate

O Aggregate Account deve ser criado através de uma operação de domínio.

A criação deve garantir:

- AccountId válido;
- Email válido;
- PasswordHash válido.

A Application não cria objetos internos manualmente ignorando o domínio.

---

# Email

Email é um Value Object.

Responsabilidades:

- validar formato;
- normalizar valor;
- garantir consistência.

A criação de Email inválido deve falhar no domínio.

---

# Password

O domínio não recebe senha em texto puro como estado.

O fluxo esperado:

Senha informada
    ↓
IPasswordHasher
    ↓
PasswordHash
    ↓
Account

---

# PasswordHash

PasswordHash representa exclusivamente o resultado seguro do processo de hash.

Responsabilidades:

- armazenar hash;
- garantir que não seja vazio;
- manter representação válida.

---

# IPasswordHasher

IPasswordHasher é um contrato.

Responsabilidade:

- transformar senha em PasswordHash;
- validar senha contra hash quando necessário.

A implementação pertence à Infrastructure.

O domínio não conhece algoritmo de hash.

---

# IAccountRepository

O contrato de persistência pertence ao Domain.

Responsabilidade:

- armazenar Account;
- recuperar Account;
- verificar existência quando necessário.

A implementação pertence à Persistence.

---

# Operações esperadas

O repositório deverá permitir:

- adicionar Account;
- buscar Account por identificador;
- consultar existência conforme necessidade do domínio.

Os detalhes de tecnologia pertencem à Persistence.

---

# Fora do Escopo

Esta FT não implementa:

- CreateAccountHandler;
- Commands;
- DTOs;
- Controllers;
- Endpoints;
- Banco de dados;
- Login.

---

# Critério de Aceite

Esta FT será considerada concluída quando:

- O fluxo arquitetural estiver definido.
- Application, Domain e Persistence possuírem responsabilidades claras.
- PV-VS-001 puder ser implementado sem decisões arquiteturais.