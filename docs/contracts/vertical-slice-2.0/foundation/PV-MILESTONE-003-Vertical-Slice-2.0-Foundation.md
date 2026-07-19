<!-- File Path:
docs/contracts/PV-MILESTONE-003-Vertical-Slice-2.0-Foundation.md
-->

# PV-MILESTONE-003 — Vertical Slice 2.0 Foundation

## Tipo

Technical Contract

---

## Objetivo

Definir a evolução do Vertical Slice atual para uma primeira versão persistente e próxima da experiência final do Project-Victory.

---

## Estado atual

Vertical Slice validado:

Account  
↓  
Login  
↓  
Character  
↓  
World Entry  
↓  
Hunt  
↓  
Combat Result  
↓  
Reward

---

## Objetivos do Vertical Slice 2.0

Adicionar:

- Persistência real.
- Create Account pelo Client.
- Hunt selecionável.
- Monster Encounter.
- Combat baseado em Monster.
- Drop de Item.
- Apresentação de Item.

---

## Princípio de implementação

A evolução deve ocorrer por fundações independentes.

Cada etapa deve gerar valor funcional sem criar sistemas futuros fora do escopo.

---

## Ordem arquitetural de execução

PV-VS-2.0-001  
Persistence Foundation

↓

PV-VS-2.0-002  
Account Experience Foundation

↓

PV-VS-2.0-003  
Hunt Selection Foundation

↓

PV-VS-2.0-004  
Monster Encounter Foundation

↓

PV-VS-2.0-005  
Combat Evolution Foundation

↓

PV-VS-2.0-006  
Item Drop Foundation

---

## Fora do escopo

Não implementar:

- PvP.
- Economia.
- Trade.
- Crafting.
- Guild.
- Skills completas.
- Classes completas.
- Equipamentos completos.
- Balanceamento avançado.
- Sistema MMO completo.

---

## Responsabilidades

### Product Owner

- Definir visão e experiência do jogo.

### Arquitetura

- Definir contratos.
- Garantir coerência técnica.
- Controlar escopo.

### Engenharia

- Implementar somente contratos aprovados.

---

## Critério geral de aprovação

O Vertical Slice 2.0 será considerado aprovado quando:

Jogador cria conta pelo Client

↓

Possui personagem persistente

↓

Escolhe uma Hunt

↓

Enfrenta um Monster

↓

Recebe Item

↓

Visualiza recompensa

---

## Status

PV-MILESTONE-003 — Em planejamento arquitetural.