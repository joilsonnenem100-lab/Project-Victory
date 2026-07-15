# FT-001 — Account Foundation

Status: Approved

Tipo: Foundation Task

---

# Objetivo

Adicionar ao Project-Victory a fundação arquitetural oficial do domínio **Account**.

Esta Foundation Task não implementa funcionalidades de negócio.

Seu objetivo é apenas estabelecer a estrutura arquitetural permanente que permitirá a evolução do domínio através dos futuros Contratos PV-VS.

---

# Escopo

Esta FT define exclusivamente:

- Responsabilidade do domínio.
- Aggregate Root.
- Estrutura física do domínio.
- Limites arquiteturais.
- Dependências permitidas.
- Contrato de persistência.
- Regras permanentes de evolução.

---

# Responsabilidade

O domínio **Account** representa a identidade de autenticação do jogador.

Sua responsabilidade limita-se ao gerenciamento do ciclo de vida da conta.

Não possui responsabilidades relacionadas a:

- Character
- Gameplay
- Inventory
- Quest
- Economia
- Mundo

---

# Aggregate Root

O Aggregate Root oficial do domínio é:

```
Account
```

Toda modificação do domínio deverá ocorrer através deste Aggregate.

---

# Estrutura mínima do domínio

A fundação do domínio deverá conter apenas:

```
Account/
├── Account.cs
├── AccountId.cs
└── IAccountRepository.cs
```

Nenhum outro componente faz parte desta Foundation Task.

---

# Dependências

O domínio Account:

Pode depender de:

- Shared Kernel
- Domain Base

Não pode depender de:

- Application
- Infrastructure
- Persistence
- API
- Character
- Inventory
- Gameplay

---

# Persistência

O domínio expõe exclusivamente:

```
IAccountRepository
```

A implementação pertence à camada Persistence.

---

# Domain Events

O domínio poderá publicar Domain Events.

Nenhum evento específico faz parte desta Foundation Task.

---

# Fora do Escopo

Esta FT não implementa:

- Criar Conta
- Login
- Email
- Password
- Password Hash
- JWT
- Sessão
- Casos de Uso
- APIs
- Persistência funcional

Esses comportamentos serão implementados pelos futuros Contratos PV-VS.

---

# Critério de Aceite

Será considerada concluída quando:

- O domínio Account possuir arquitetura oficialmente definida.
- A Engenharia puder implementar o domínio sem assumir decisões arquiteturais.
- O Vertical Slice puder iniciar o PV-VS-001 sem novos bloqueios arquiteturais.

---

# Resultado Esperado

Após esta FT o domínio Account estará oficialmente preparado para evolução.

Os próximos contratos poderão implementar exclusivamente funcionalidades de negócio sem necessidade de novas decisões arquiteturais.