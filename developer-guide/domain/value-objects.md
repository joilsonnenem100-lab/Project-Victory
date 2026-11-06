# Value Objects

Status: LOCKED

Version: 1.1

> *Emenda (2026-11-06, Fila 5, `DEC-20261106-27` e `DEC-20261106-17/26`; revisão formal do documento LOCKED autorizada pelo PO): `AttackSpeed` e `Capacity` foram **removidos** (texto tachado abaixo; registro no `CLEANUP-MANIFEST-2026-11-06.md`).*

---

# Objetivo

Este documento define os Value Objects oficiais do domínio do Project-Victory.

Value Objects representam conceitos do domínio.

Eles não possuem identidade.

São definidos exclusivamente pelos seus valores.

---

# Filosofia

Sempre que existir um conceito do domínio, ele deverá ser representado por um Value Object.

Evitar o uso de tipos primitivos diretamente nas regras de negócio.

---

# Primary Attributes

## Health

Representa a quantidade de vida de uma entidade.

Imutável.

Nunca utilizar int diretamente.

---

## Mana

Representa energia mágica.

Imutável.

---

## Experience

Representa experiência acumulada.

Imutável.

---

## Level

Representa o nível atual.

Imutável.

---

# Combat

## Damage

Representa um dano.

---

## Defense

Representa defesa.

---

## ~~AttackSpeed~~

~~Representa velocidade de ataque.~~ *Removido (Fila 5, `DEC-20261106-27`): não existe no Tibia.*

---

## MovementSpeed

Representa velocidade de movimentação.

---

## CriticalChance

Representa probabilidade crítica.

---

## CriticalDamage

Representa multiplicador crítico.

---

## Range

Representa alcance.

---

# Economy

## Gold

Representa quantidade de ouro.

Não representa Item.

---

## Weight

Representa peso.

---

## ~~Capacity~~

~~Representa capacidade máxima.~~ *Removido (Fila 5, `DEC-20261106-17/26`): o jogo não tem Capacidade.*

---

# Loot

## DropChance

Representa probabilidade de drop.

---

# World

## Position

Representa uma posição.

Contém:

- X
- Y
- Z

Imutável.

---

## RespawnTime

Representa tempo de respawn.

---

## Duration

Representa duração de efeitos.

---

## Percentage

Representa percentual.

Evitar utilizar float diretamente.

---

# Equality

Dois Value Objects são iguais quando todos os seus valores internos são iguais.

---

# Identity

Value Objects nunca possuem:

- Guid
- Id
- Chave Primária

---

# Mutabilidade

São imutáveis.

Qualquer alteração gera uma nova instância.

---

# Persistência

São persistidos junto da Entity proprietária.

Nunca existem isoladamente.

---

# Regras

É proibido:

- acessar banco de dados;
- conhecer infraestrutura;
- conhecer Runtime;
- possuir identidade.

---

# Filosofia Final

Entities possuem identidade.

Value Objects possuem significado.

Aggregate Roots garantem consistência.

Todo conceito reutilizável do domínio deve preferencialmente ser modelado como Value Object.