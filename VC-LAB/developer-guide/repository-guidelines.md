# Repository Guidelines

Status: LOCKED

Version: 1.0

---

# Objetivo

Definir o padrão oficial para implementação dos Repositories do Project-Victory.

Repositories representam exclusivamente a camada de Persistência.

Eles não possuem regras de negócio.

---

# Filosofia

Repository responde apenas uma pergunta:

"Como persistir ou recuperar dados?"

Nunca:

"Como o jogo funciona?"

---

# Responsabilidade

Repository é responsável por:

- Persistir Entities
- Recuperar Entities
- Atualizar Entities
- Remover Entities (quando aplicável)

Nada além disso.

---

# O que Repository NÃO faz

Nunca:

- Calcula dano
- Calcula Loot
- Calcula Experience
- Valida regras de jogo
- Executa Commands
- Publica Events
- Chama Runtime

Essas responsabilidades pertencem a outras camadas.

---

# Fluxo

Application

↓

Repository Interface

↓

Persistence

↓

PostgreSQL

---

# Interfaces

As interfaces pertencem ao Domain.

Exemplo

ICharacterRepository

IInventoryRepository

IItemRepository

IGuildRepository

Nunca na camada Persistence.

---

# Implementações

As implementações pertencem exclusivamente à Persistence.

Exemplo

CharacterRepository

InventoryRepository

ItemRepository

GuildRepository

---

# Regra 1

Repository nunca conhece Controllers.

---

# Regra 2

Repository nunca conhece Runtime.

---

# Regra 3

Repository nunca conhece Event Bus.

---

# Regra 4

Repository nunca chama outro Repository.

Coordenação pertence à Application.

---

# Regra 5

Repository trabalha apenas com Aggregates.

Nunca com metade de uma Entity.

---

# Regra 6

Repository deve retornar objetos completos.

Nunca objetos parcialmente válidos.

---

# Regra 7

Repository nunca contém regras de negócio.

---

# Regra 8

Queries complexas pertencem à camada apropriada de leitura.

Não transformar Repository em mecanismo de relatório.

---

# Regra 9

Repository deve ser facilmente substituível.

A Domain nunca sabe se existe PostgreSQL.

Ou outro banco.

---

# Regra 10

Repository nunca depende da API.

---

# Regra 11

Repository nunca cria Runtime Objects.

---

# Regra 12

Repository deve ser completamente testável.

---

# Regra 13

Toda operação deve possuir comportamento previsível.

---

# Regra 14

Repository nunca utiliza Singleton.

---

# Regra 15

Repository nunca compartilha estado.

---

# Regra 16

Persistência deve ser transacional quando necessário.

---

# Regra 17

Repository deve respeitar Aggregate Boundaries.

---

# Regra 18

Repository nunca gera IDs.

Factories ou Services são responsáveis.

---

# Regra 19

Repository nunca cria Domain Events.

---

# Regra 20

Quando houver dúvida,

Repository apenas persiste.

Nunca decide.