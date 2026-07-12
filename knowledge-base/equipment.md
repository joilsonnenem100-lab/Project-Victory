# Equipment System

Version: 1.0

Status: LOCKED

Last Sprint: SPRINT-025

---

# Objetivo

Definir o funcionamento oficial do sistema de equipamentos do Project-Victory.

Equipamentos são uma das principais fontes de poder do personagem.

---

# Filosofia

No Project-Victory, um equipamento representa força.

Equipamentos representam:

- poder;
- conquista;
- evolução;
- identidade de build.

---

# Relação com Item

Equipamento é um ItemInstance equipado.

Fluxo:

```
ItemInstance

↓

EquipmentService

↓

Character

↓

Combat
```

---

# Arquitetura

Equipment é um sistema separado do Inventário.

```
Inventory

=

armazenamento


Equipment

=

utilização de poder
```

---

# Slots Oficiais

Slots disponíveis:

```
Helmet

Armor

Legs

Boots

Weapon

Shield

Ring

Amulet

Pet
```

---

# Sistema de Mãos

Equipamentos podem ocupar:

## Uma mão

Exemplo:

```
Fire Sword

↓

Weapon Slot
```

---

## Duas mãos

Exemplo:

```
Two Handed Weapon

↓

Weapon Slot

↓

Bloqueia Shield Slot
```

---

# Pet Slot

Existe inicialmente:

```
Pet Slot
```

Uso futuro:

- bônus;
- Founder Rarity;
- benefícios especiais.

---

# Requisitos para Equipar

Equipamentos podem exigir:

- Level;
- Vocação;
- Ascension.

A arquitetura permite adicionar novos requisitos.

---

# Equipamentos por Vocação

Equipamentos podem possuir restrição de vocação.

Exemplo:

```
Mage Staff

↓

Sorcerer/Druid
```

---

# Tier

Tier influencia:

- qualidade;
- possíveis afixos;
- potencial do equipamento.

---

# Upgrade

Upgrade aumenta:

```
Atributo Base
```

Não altera:

- afixos;
- imbuements.

---

# Sistema de Upgrade

Fluxo:

```
+0

↓

+9

↓

Chance especial

↓

Craft Affix
```

---

# Craft Affix

Após +9:

Existe possibilidade de desbloquear:

```
Craft Affix
```

Limite inicial:

```
2 Craft Affixes
```

---

# Imbuement

Equipamentos podem possuir:

```
Imbuement Slots
```

Características:

- podem nascer no drop;
- são permanentes;
- possuem duração;
- ficam vazios após expirar.

---

# Durabilidade

Equipamentos possuem:

```
Durabilidade
```

Regra:

```
100%

↓

50%
```

Ao atingir 50%:

- durabilidade para de reduzir;
- item continua equipado.

---

# Penalidade de Durabilidade

Quando abaixo do limite:

Aplica:

```
Redução temporária de dano
```

Valor será definido futuramente.

---

# Quebra de Equipamento

Não existe.

Equipamentos:

- não desaparecem;
- não quebram;
- continuam utilizáveis.

---

# Troca de Equipamento

Durante combate:

```
Não permitido
```

Fluxo:

```
Fora de combate

↓

Trocar equipamento

↓

Atualizar poder
```

---

# Sets

Itens com mesmo nome não formam conjunto automaticamente.

Exemplo:

```
Dragon Helmet

+

Dragon Armor

+

Dragon Boots
```

Não gera:

```
Dragon Set Bonus
```

Cada peça possui seus próprios atributos.

---

# Unique Equipment

Equipamentos Unique possuem:

- exclusividade no servidor;
- controle de quantidade;
- características especiais futuras.

---

# Pet Equipment

Inicialmente:

```
Não existe
```

Arquitetura preparada para expansão futura.

---

# Influência no Combate

Equipamentos podem alterar:

- dano;
- defesa;
- resistência;
- efeitos especiais.

---

# Integrações

EquipmentService comunica com:

```
ItemService

↓

InventoryService

↓

CharacterService

↓

CombatService
```

---

# Arquitetura Final

```
EquipmentService

↓

EquipmentComponent

↓

EquipmentSlot

↓

ItemInstance
```

---

# Pendências futuras

Definir:

- lista de slots final;
- atributos por equipamento;
- fórmula de upgrade;
- custo de repair;
- tipos de armas;
- efeitos especiais;
- Founder Pets.