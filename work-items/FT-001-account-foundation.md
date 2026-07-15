# WI-001 — Implementar Foundation do Domínio Account

Status: Planned

Tipo: Foundation Task

---

# Objetivo

Implementar a fundação arquitetural do domínio Account conforme definido em:

developer-guide/domain/account-foundation.md

Esta atividade transforma a arquitetura aprovada em código.

Nenhuma funcionalidade de negócio deverá ser implementada.

---

# Escopo

Criar a estrutura mínima do domínio Account.

---

# Arquivos a criar

ProjectVictory.Domain

Account/

- Account.cs
- AccountId.cs
- Email.cs
- PasswordHash.cs
- IAccountRepository.cs
- IPasswordHasher.cs

---

# Critério de Aceite

Será considerada concluída quando:

- Todos os arquivos existirem.
- O domínio Account passar a existir oficialmente na solução.
- Nenhuma regra de negócio estiver implementada.
- Nenhum caso de uso existir.
- A implementação respeitar integralmente a arquitetura oficial.

---

# Fora do Escopo

Não implementar:

- Criar Conta
- Login
- Autenticação
- JWT
- APIs
- Commands
- Queries
- Handlers
- Persistência funcional
- Casos de uso

---

# Resultado Esperado

Após esta implementação, o domínio Account estará disponível para evolução pelos próximos Contratos PV-VS.

Os contratos PV-VS-001 e seguintes poderão ser implementados sem necessidade de novas decisões arquiteturais.