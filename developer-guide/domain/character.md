# Character

Status: LOCKED

Version: 1.0

---

# Objetivo

Definir o contrato arquitetural do Aggregate Root Character.

---

# Responsabilidade

O Character representa um personagem controlado pelo jogador.

É o principal Aggregate Root do domínio.

Todo o progresso do jogador pertence ao Character.

---

# Responsabilidades

O Character é responsável por:

- proteger suas invariantes;
- controlar seus atributos;
- controlar sua vida;
- controlar sua mana;
- controlar sua experiência;
- controlar seu nível;
- controlar seu inventário;
- controlar seus equipamentos.

---

# Dependências

O Character poderá depender apenas de:

- Entity
- AggregateRoot
- Value Objects

---

# Proibições

O Character não poderá:

- acessar banco de dados;
- conhecer infraestrutura;
- conhecer Runtime;
- conhecer Application;
- conhecer UI.

---

# Regras

Toda alteração do estado do personagem deverá ocorrer através do próprio Character.

Nenhum objeto interno poderá ser modificado diretamente.

---

# Filosofia

O Character representa toda a consistência do personagem.

Ele é a única porta de entrada para alterações em seu estado.