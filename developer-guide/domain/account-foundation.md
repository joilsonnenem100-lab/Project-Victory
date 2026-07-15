# FT-001 — Account Foundation

Status: Approved

Tipo: Foundation Task

---

# Objetivo

Adicionar ao Project-Victory a fundação arquitetural oficial do domínio **Account**.

Esta Foundation Task não implementa funcionalidades de negócio.

Seu objetivo é estabelecer apenas a estrutura arquitetural permanente que permitirá a evolução do domínio através dos futuros Contratos PV-VS.

---

# Escopo

Esta Foundation Task define exclusivamente:

- Responsabilidade do domínio.
- Aggregate Root.
- Estrutura física do domínio.
- Limites arquiteturais.
- Dependências permitidas.
- Contrato de persistência.
- Regras permanentes de evolução.

---

# Responsabilidade

O domínio **Account** representa a identidade de autenticação de um jogador dentro do Project-Victory.

Sua responsabilidade limita-se ao gerenciamento do ciclo de vida da conta.

O domínio não possui responsabilidades relacionadas a:

- Character
- Gameplay
- Inventory
- Equipment
- Quest
- Economia
- Mundo

---

# Aggregate Root

O Aggregate Root oficial do domínio é:

```
Account
```

Toda alteração do domínio deverá ocorrer através deste Aggregate.

---

# Estrutura Física

O domínio deverá possuir a seguinte estrutura mínima:

```
ProjectVictory.Domain
└── Account
    ├── Account.cs
    ├── AccountId.cs
    └── IAccountRepository.cs
```

Nenhum outro componente faz parte desta Foundation Task.

---

# Dependências

O domínio Account:

Pode depender de:

- ProjectVictory.Shared
- Componentes base do Domain

Não pode depender de:

- ProjectVictory.Application
- ProjectVictory.Infrastructure
- ProjectVictory.Persistence
- ProjectVictory.API
- Character
- Inventory
- Equipment
- Gameplay

---

# Persistência

O domínio expõe exclusivamente o contrato:

```
IAccountRepository
```

A implementação da persistência pertence à camada **ProjectVictory.Persistence**.

---

# Domain Events

O domínio poderá publicar Domain Events.

Nenhum evento específico faz parte desta Foundation Task.

---

# Evolução

Novos componentes poderão ser adicionados posteriormente através de Contratos PV-VS, como por exemplo:

- Email
- Password
- PasswordHash
- Policies
- Domain Services
- Domain Events

A inclusão desses componentes não altera a arquitetura definida nesta Foundation Task.

---

# Fora do Escopo

Esta Foundation Task não implementa:

- Criar Conta
- Login
- Autenticação
- Email
- Password
- Password Hash
- JWT
- Sessão
- Casos de Uso
- API
- Persistência funcional
- Regras de negócio

---

# Critério de Aceite

Esta Foundation Task será considerada concluída quando:

- O domínio Account existir oficialmente na solução.
- A estrutura mínima do domínio estiver implementada.
- A arquitetura do domínio estiver alinhada à arquitetura oficial do Project-Victory.
- Os próximos Contratos PV-VS puderem evoluir o domínio sem necessidade de novas decisões arquiteturais.

---

# Resultado Esperado

Após a conclusão desta Foundation Task:

- O domínio Account passa a existir oficialmente no Project-Victory.
- A Engenharia possui uma fundação arquitetural permanente para evolução do domínio.
- O Vertical Slice pode prosseguir com o PV-VS-001 sem bloqueios arquiteturais.