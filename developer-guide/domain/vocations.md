---

# Spell Profile

## Objetivo

Cada Vocation define quais Spells um Character pode aprender e utilizar.

Spell Profile representa exclusivamente regras de disponibilidade de Spells.

Ele nunca calcula dano, cura ou qualquer efeito de combate.

---

# Filosofia

O sistema de Spells é único para todas as Vocações.

As diferenças entre elas estão apenas na disponibilidade de utilização.

Combat nunca possui regras específicas para uma determinada Vocation.

---

# Responsabilidade

Spell Profile pertence exclusivamente à Vocation.

Ele define:

- quais Spells podem ser aprendidas;
- quais Spells podem ser utilizadas;
- futuras restrições específicas de progressão.

---

# Aprendizado

Toda Spell pertence ao seu próprio domínio.

A Vocation apenas define se aquela Spell faz parte de sua lista oficial.

As regras de aprendizado serão documentadas no módulo de Spells.

---

# Execução

Durante a execução de uma Action do tipo Spell, Combat apenas recebe uma Spell válida.

Combat nunca verifica se a Spell pertence à Vocation.

Essa validação ocorre antes da execução da Action.

---

# Balanceamento

A lista de Spells disponíveis pertence aos dados da Vocation.

Adicionar, remover ou reorganizar Spells não exige alterações na Combat Engine.

---

# Futuras Expansões

A arquitetura permite adicionar novas Spells e novas Vocações sem alterar os sistemas existentes.

Cada nova Vocation define apenas sua própria lista de Spells.

---

# Princípios Arquiteturais

- Toda Vocation possui um Spell Profile.
- Spell Profile define apenas disponibilidade.
- Combat nunca valida Vocações.
- Combat executa apenas Spells previamente consideradas válidas.
- As regras de funcionamento das Spells pertencem ao domínio de Spells.