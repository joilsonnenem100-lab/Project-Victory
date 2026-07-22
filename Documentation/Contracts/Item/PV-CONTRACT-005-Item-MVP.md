# PV-CONTRACT-005 --- Item MVP

## Local recomendado

Project-Victory/Documentation/Contracts/Item/

------------------------------------------------------------------------

# Objetivo

Criar o primeiro sistema funcional de itens do Project-Victory.

Fluxo:

Monster Reward

↓

Item Drop

↓

Inventory

↓

Equipment

↓

Character Update

------------------------------------------------------------------------

# Escopo

Implementar somente o necessário para o primeiro ciclo jogável.

Incluído:

-   Item Definition;
-   Item Instance;
-   Inventory;
-   Equipment;
-   Slots de equipamento;
-   Bônus básicos de item.

Fora de escopo:

-   Raridade;
-   Affixes;
-   Crafting;
-   Socket;
-   Upgrade;
-   Sets;
-   Durability.

------------------------------------------------------------------------

# 1. Primeiros Equipamentos

Modelo inicial aprovado.

## Knight

-   Rusty Sword;
-   Wood Shield;
-   Leather Armor.

## Paladin

-   Short Bow;
-   Wood Shield;
-   Leather Armor.

## Sorcerer / Druid

-   Basic Staff;
-   Cloth Robe.

## Monk

-   Training Fist Gloves.

------------------------------------------------------------------------

# 2. Slots

Decisão:

Sistema de slots existe conceitualmente.

Implementação de slots nomeados fica preparada para evolução futura.

Não limitar inicialmente o sistema aos novos slots avançados.

------------------------------------------------------------------------

# 3. Slots iniciais de equipamento

Sugestão inicial:

-   Weapon;
-   Shield;
-   Armor.

O sistema deve permitir expansão futura para:

-   Helmet;
-   Legs;
-   Boots;
-   Ring.

------------------------------------------------------------------------

# 4. Inventory

Configuração inicial:

20 slots.

O valor deve ser configurável e alterável futuramente.

------------------------------------------------------------------------

# 5. Item Drop

Quando existir drop:

O monstro entrega um item completo.

Exemplo:

Monster

↓

Iron Sword

↓

Inventory

Não utiliza materiais inicialmente.

------------------------------------------------------------------------

# 6. Equipamento e Character Stats

Ao equipar um item:

O Character Stats é atualizado automaticamente.

Exemplo:

Weapon

↓

Damage Bonus

↓

Character State

------------------------------------------------------------------------

# 7. Troca durante combate

Não permitido.

Regra:

Combat iniciado

↓

Equipment bloqueado

------------------------------------------------------------------------

# 8. Bônus iniciais de equipamento

Modelo aprovado:

## Weapon

Fornece:

-   Damage

## Shield

Fornece:

-   Defense

## Armor

Fornece:

-   HP

------------------------------------------------------------------------

# 9. Venda de itens

Permitida.

Sistema de venda pode existir futuramente conectado a:

-   NPC;
-   Market;
-   Loja.

------------------------------------------------------------------------

# 10. Remoção de item

Não existe destruição de item inicialmente.

------------------------------------------------------------------------

# 11. Entrega de item

Fluxo:

Monster Dead

↓

Drop

↓

Inventory

Entrega automática.

------------------------------------------------------------------------

# 12. Primeiro Drop Real

O sistema de item será utilizado inicialmente através do Cave Rat.

Cave Rat poderá dropar equipamento básico.

Rat continua sem drop inicial.

------------------------------------------------------------------------

# Estado Final do Contrato

Definido:

-   equipamentos iniciais;
-   inventário de 20 slots;
-   item completo como drop;
-   atualização automática de Character Stats;
-   bloqueio de troca em combate;
-   bônus básicos;
-   venda futura;
-   ausência de destruição;
-   entrega automática;
-   Cave Rat como primeiro monstro com drop.

------------------------------------------------------------------------

# Próximo contrato

PV-CONTRACT-006 --- Complete Gameplay Loop Validation
