---

# Integration with Combat

## Objetivo

O módulo de Resources fornece à Combat Engine acesso controlado aos Resources das Combat Entities.

Combat nunca manipula diretamente o armazenamento interno dos Resources.

---

# Filosofia

Combat depende de Resources.

Resources não dependem de Combat.

Essa relação mantém a arquitetura desacoplada.

---

# Fluxo

Durante a execução de uma Action:

Combat Entity

↓

Resources

↓

Validação

↓

Combat Engine

↓

Combat Result

↓

Atualização dos Resources

---

# Responsabilidades

Resources é responsável por:

- manter o estado atual;
- validar limites;
- permitir consumo;
- permitir restauração.

Combat é responsável por:

- solicitar consumo;
- solicitar restauração;
- utilizar os valores atuais durante os cálculos.

---

# Independência

Combat nunca conhece:

- armazenamento;
- persistência;
- inicialização;
- regeneração.

Essas responsabilidades pertencem exclusivamente ao módulo de Resources.

---

# Future Extensions

Novos Resources poderão ser adicionados sem necessidade de alterações na Combat Engine.

Combat continuará trabalhando apenas com a interface pública disponibilizada pelo módulo de Resources.

---

# Princípios Arquiteturais

- Combat depende de Resources.
- Resources não dependem de Combat.
- Combat nunca manipula diretamente o estado interno dos Resources.
- A integração ocorre apenas através das interfaces públicas do domínio.