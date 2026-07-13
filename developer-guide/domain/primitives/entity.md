# Entity

Status: LOCKED

Version: 1.0

---

# Objetivo

Definir o contrato arquitetural da classe base `Entity`.

Toda Entity do Project-Victory deverá herdar desta classe.

---

# Responsabilidade

Uma Entity representa um objeto do domínio que possui identidade própria.

Sua identidade permanece constante durante todo o ciclo de vida.

---

# Contrato

Toda Entity deve possuir:

- Identidade única.
- Igualdade baseada exclusivamente na identidade.
- Estado mutável.
- Encapsulamento do próprio estado.

---

# Identidade

A identidade deverá ser representada por um identificador único.

A implementação da identidade será centralizada na classe base.

---

# Igualdade

Duas Entities são iguais somente quando possuem a mesma identidade.

Os demais atributos não participam da comparação.

---

# Mutabilidade

Entities podem alterar seu estado.

Toda alteração deve preservar as invariantes do domínio.

---

# Encapsulamento

O estado interno nunca poderá ser alterado diretamente por classes externas.

Toda modificação deverá ocorrer através de métodos do próprio Aggregate.

---

# Dependências

A classe Entity:

- não conhece Infrastructure;
- não conhece Application;
- não conhece Runtime;
- não conhece Persistência.

---

# Proibições

É proibido:

- acessar banco de dados;
- conter lógica de infraestrutura;
- depender de frameworks;
- conhecer UI.

---

# Filosofia

Entity representa identidade.

Seu objetivo é proteger o estado e o comportamento do domínio.