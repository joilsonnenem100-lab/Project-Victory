# Domain Event

Status: LOCKED

Version: 1.0

---

# Objetivo

Definir o contrato arquitetural da interface `IDomainEvent`.

Todo Domain Event do Project-Victory deverá implementar esta interface.

---

# Responsabilidade

Representar um fato ocorrido dentro do domínio.

---

# Contrato

Todo Domain Event deve:

- Ser imutável.
- Representar um acontecimento passado.
- Não conter lógica de negócio.

---

# Dependências

Nenhuma.

---

# Proibições

É proibido:

- acessar banco de dados;
- conhecer infraestrutura;
- conhecer Runtime;
- modificar o domínio.

---

# Filosofia

Domain Events representam fatos consumados do domínio.

Seu único objetivo é comunicar acontecimentos relevantes.