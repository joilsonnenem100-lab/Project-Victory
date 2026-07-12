# Combat System

> Status: EM DESENVOLVIMENTO

---

# Objetivo

Definir as regras oficiais do combate.

---

# Filosofia

O combate deve recompensar preparação.

Equipamentos representam uma das maiores fontes de poder do personagem.

---

# Poder do Personagem

A força do personagem é determinada principalmente por:

- Equipamentos
- Skills
- Level

Cada vocação utiliza prioridades diferentes.

---

# Knight

Prioridade de dano.

1. Level
2. Sword Skill
3. Arma

---

# Sorcerer

Prioridade de dano.

1. Level
2. Magic Level
3. Equipamento
4. Spell

---

# Defesa

A defesa é composta por:

- Level
- Equipamentos
- Shield

---

# Regeneração

Toda regeneração depende de Food.

Não existe regeneração natural.

---

# Equipamentos

Itens Rare totalmente evoluídos representam uma grande vantagem sobre equipamentos comuns.

---

# Pendências

Ainda precisam ser definidos.

- Fórmula de dano.
- Fórmula de defesa.
- Fórmula de crítico.
- Fórmula de resistência.
- Dano elemental.
- Ordem dos cálculos.

---

# Decisões Relacionadas

DEC-0007

DEC-0011

DEC-0012

DEC-0013
---

# Sistema de Turnos

O combate utiliza turnos fixos.

Fluxo:

Jogador entra na Hunt

↓

Pack criado

↓

Combate inicia

↓

Jogador ataca

↓

Monstros sobreviventes atacam

↓

Novo turno

↓

Continua até:

- morte do jogador;
- encerramento da Hunt;
- conclusão do combate.

---

# Seleção de Alvo

O jogador sempre ataca o primeiro monstro da Pack List.

Quando o alvo é derrotado:

O próximo monstro da lista torna-se o alvo automaticamente.

---

# Attack Speed e Cast Speed

Não existem inicialmente.

Todos os personagens utilizam velocidade fixa de combate.

A arquitetura deverá permitir inclusão futura desses atributos sem reconstrução do CombatService.

---

# Skills em Área

Skills possuem comportamento próprio.

Cada Skill define:

- quantidade de alvos;
- se atinge todos os monstros do Pack;
- regras específicas.

---

# Estrutura Técnica

CombatService será dividido em:

- TargetService
- DamageCalculator
- CriticalCalculator
- DeathService

---

# Pendências atualizadas

Ainda precisam ser definidos:

- Fórmula de dano.
- Fórmula de defesa.
- Fórmula de crítico.
- Resistências.
- Elementos.
- Valores matemáticos das Skills.