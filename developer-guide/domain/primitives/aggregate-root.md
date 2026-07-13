# Aggregate Root

Status: LOCKED

Version: 1.0

---

# Objetivo

Definir o contrato arquitetural da classe base `AggregateRoot`.

Todo Aggregate Root do Project-Victory deverá herdar desta classe.

---

# Responsabilidade

Um Aggregate Root é uma Entity responsável por proteger a consistência de um Aggregate.

Toda alteração em objetos internos deverá ocorrer através dele.

---

# Contrato

Todo Aggregate Root deve:

- Herdar de Entity.
- Controlar as invariantes do Aggregate.
- Registrar Domain Events.
- Expor os Domain Events apenas para leitura.
- Permitir limpar os Domain Events após sua publicação.

---

# Domain Events

Todo Aggregate Root poderá registrar eventos do domínio.

Os eventos pertencem ao Aggregate até serem publicados.

---

# Encapsulamento

Objetos internos nunca poderão ser alterados diretamente.

Toda modificação deverá passar pelo Aggregate Root.

---

# Dependências

AggregateRoot depende apenas de:

- Entity
- IDomainEvent

---

# Proibições

É proibido:

- acessar infraestrutura;
- acessar banco de dados;
- conhecer Runtime;
- conhecer Application.

---

# Filosofia

Aggregate Roots representam a fronteira de consistência do domínio.

Toda alteração relevante deve passar por eles.