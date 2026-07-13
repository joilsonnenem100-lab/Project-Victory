# Entity Guidelines

Status: LOCKED

Version: 1.0

---

# Objetivo

Definir como uma Entity deve ser construída dentro do Project-Victory.

Toda Entity do projeto deve seguir obrigatoriamente estas regras.

---

# O que é uma Entity

Uma Entity representa um objeto do domínio que possui identidade própria.

A identidade permanece a mesma durante todo o seu ciclo de vida.

Exemplos

- Character
- ItemInstance
- Inventory
- Guild
- Account

---

# O que NÃO é uma Entity

Não transformar tudo em Entity.

Exemplos

Level

Damage

Health

Mana

Gold

CriticalChance

Esses são Value Objects.

---

# Regra 1

Toda Entity possui um Id único.

Utilizar UUID v7.

Nunca alterar o Id.

---

# Regra 2

Entity representa estado.

Nunca infraestrutura.

Nunca persistência.

Nunca interface.

---

# Regra 3

Entity nunca conhece:

- Entity Framework
- PostgreSQL
- Redis
- ASP.NET
- API
- Controllers
- Repositories

---

# Regra 4

Entity nunca salva a si mesma.

Errado

Character.Save()

Inventory.Load()

Item.Delete()

Correto

CharacterRepository.Save(Character)

---

# Regra 5

Entity nunca possui lógica de infraestrutura.

Toda regra externa pertence à Infrastructure.

---

# Regra 6

Entity deve ser pequena.

Se começar a crescer,

avaliar dividir em novos componentes.

---

# Regra 7

Entity nunca acessa diretamente outra Entity distante.

Sempre utilizar Services quando a regra envolver múltiplos Aggregates.

---

# Regra 8

Entity nunca conhece Runtime.

Runtime utiliza Entity.

Nunca o contrário.

---

# Regra 9

Entity nunca conhece Event Bus.

Ela apenas gera eventos de domínio.

A publicação é responsabilidade da Application.

---

# Regra 10

Entity nunca instancia outras Entities diretamente.

Sempre utilizar Factories quando houver criação complexa.

---

# Regra 11

Toda Entity deve ser serializável.

Sem dependências externas.

---

# Regra 12

Entity deve proteger seu estado.

Evitar setters públicos.

Sempre preferir métodos explícitos.

Exemplo

EquipItem()

LevelUp()

GainExperience()

Nunca:

character.Level++

---

# Regra 13

Entity deve permanecer consistente.

Nunca permitir estados inválidos.

---

# Regra 14

Uma Entity nunca deve conhecer detalhes do banco.

Não utilizar atributos específicos do ORM.

---

# Regra 15

Uma Entity nunca depende de outra camada.

Ela pertence exclusivamente ao Domain.

---

# Regra 16

Toda alteração importante deve gerar um Domain Event.

Exemplo

CharacterLeveledUp

ItemEquipped

ItemDropped

GuildCreated

---

# Regra 17

Entities nunca compartilham comportamento por herança profunda.

Sempre preferir composição.

---

# Regra 18

Uma Entity deve representar um conceito do jogo.

Nunca uma necessidade técnica.

---

# Regra 19

Toda Entity deve possuir testes unitários.

---

# Regra 20

Quando houver dúvida,

preservar simplicidade e consistência da Domain.