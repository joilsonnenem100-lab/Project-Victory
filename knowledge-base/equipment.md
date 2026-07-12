# Equipment System

Version: 1.0

Status: LOCKED

Last Sprint: SPRINT-009

---

# Objetivo

Definir o funcionamento oficial do sistema de equipamentos do Project-Victory.

EquipmentService controla:

- Equipar;
- Remover;
- Validar requisitos;
- Aplicar atributos;
- Gerenciar upgrades;
- Gerenciar Imbuements;
- Controlar durabilidade.

---

# Filosofia

No Project-Victory, um equipamento representa conquista e poder.

Equipamentos representam:

- dedicação;
- investimento;
- evolução;
- reconhecimento.

---

# Arquitetura

O personagem não armazena equipamentos diretamente.

Estrutura:

```
Character

↓

EquipmentComponent

↓

EquipmentSlot

↓

ItemInstance
```

---

# Slots Oficiais

O personagem possui:

- Head
- Necklace
- Backpack Visual
- Armor
- Right Hand
- Left Hand
- Legs
- Boots
- Ring 1
- Ring 2
- Ammo
- Pet

---

# Backpack Visual

O slot Backpack existe apenas para:

- aparência;
- personalização;
- cosméticos.

Não aumenta o espaço do inventário.

---

# Pet Slot

O slot Pet existe para futuras expansões.

Possíveis funcionalidades:

- Founder Rarity;
- bônus de Drop;
- efeitos especiais;
- sistemas futuros.

---

# Sistema de Mãos

## Left Hand

Utilizado para:

- armas de uma mão;
- armas de duas mãos.

---

## Right Hand

Utilizado para:

- Shields;
- Spellbooks.

---

## Two Handed Weapon

Ao equipar:

- ocupa Left Hand;
- bloqueia Right Hand.

---

# Requisitos de Equipamento

Um equipamento pode exigir:

- Level;
- Vocação;
- Ascension.

A arquitetura deve permitir novos requisitos futuros.

---

# Equipamentos por Vocação

Alguns equipamentos são exclusivos de determinadas vocações.

Exemplos:

- armas;
- spellbooks;
- armaduras específicas.

---

# Tier

Itens possuem Tier.

Tier influencia:

- atributo base;
- qualidade dos afixos;
- possibilidade de geração de afixos melhores.

Tier maior representa maior potencial do equipamento.

---

# Upgrade System

Apenas itens Rare podem receber Upgrade.

Limite:

```
+14
```

---

# Progressão de Upgrade

## +0 até +9

Progressão inicial.

Possui maior facilidade.

---

## +10 até +14

Sistema avançado.

Possui:

- maior dificuldade;
- risco;
- valorização econômica.

---

# Craft Affixes

Upgrades acima de +9 possuem chance de desbloquear Craft Affixes.

Características:

- linha exclusiva de afixos;
- diferente dos afixos normais;
- extremamente valorizada.

---

# Limite de Craft Affixes

Valor inicial:

```
2 Craft Affixes
```

Esse limite poderá ser alterado futuramente por Tier.

---

# Afixos Normais

Afixos dependem da raridade:

Normal:

Sem afixo.

Magic:

1 afixo.

Rare:

2~3 afixos.

---

# Imbuement Slots

Somente itens que nasceram com slots podem receber Imbuements.

Características:

- máximo inicial de 3 slots;
- fazem parte da identidade do Item Instance;
- não podem ser adicionados posteriormente.

---

# Imbuement Duration

Imbuements possuem duração.

Quando expiram:

- efeito desaparece;
- slot permanece disponível.

O jogador pode aplicar outro Imbuement.

---

# Durabilidade

Equipamentos possuem durabilidade.

Regras:

- reduz apenas ao morrer;
- não quebra;
- nunca fica abaixo de 50%;
- reparo realizado por NPC.

---

# Poder do Equipamento

Equipamentos representam aproximadamente 50% do poder total do personagem.

O restante vem de:

- Level;
- Skills;
- Outros sistemas de progressão.

---

# Integrações

EquipmentService se comunica com:

- CharacterService;
- InventoryService;
- ItemService;
- CombatService;
- MarketService.

---

# Pendências futuras

Definir:

- Fórmula exata do Upgrade;
- Materiais necessários;
- Chance de sucesso;
- Lista de Craft Affixes;
- Pet System;
- Regras avançadas de Tier.