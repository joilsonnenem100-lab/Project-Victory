# Value Object

Status: LOCKED

Version: 1.0

---

# Objetivo

Definir o contrato arquitetural da classe base `ValueObject`.

Todo Value Object do Project-Victory deverá herdar desta classe.

---

# Responsabilidade

Representar um conceito do domínio sem identidade própria.

---

# Contrato

Todo Value Object deve:

- Ser imutável.
- Não possuir Id.
- Implementar igualdade por valor.
- Não possuir estado mutável.

---

# Igualdade

Dois Value Objects são iguais quando todos os seus valores internos são iguais.

---

# Dependências

Nenhuma.

---

# Proibições

É proibido:

- acessar banco de dados;
- conhecer infraestrutura;
- conhecer Runtime;
- possuir Guid;
- possuir identidade.

---

# Filosofia

Value Objects representam conceitos.

São definidos exclusivamente pelos seus valores.