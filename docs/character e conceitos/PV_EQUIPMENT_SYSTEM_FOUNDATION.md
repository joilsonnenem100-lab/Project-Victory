# PROJECT-VICTORY --- Equipment System Foundation

## Status

Documento de consolidação das decisões de Product Owner relacionadas ao
sistema de equipamentos.

Este documento define a fundação do sistema que conecta:

Character

↓

Equipment

↓

Attributes

↓

Combat

↓

Hunt

------------------------------------------------------------------------

# 1. Objetivo do Sistema

Equipamentos representam objetos utilizados pelo personagem para
modificar sua capacidade de combate e progressão.

Eles possuem:

-   identidade;
-   categoria;
-   atributos;
-   relação com classes;
-   impacto no gameplay.

------------------------------------------------------------------------

# 2. Slots de Equipamento

Decisão:

O sistema utilizará slots completos.

Slots definidos:

-   Helmet
-   Armor
-   Legs
-   Boots
-   Weapon
-   Shield
-   Ring
-   Amulet

Este modelo permite expansão futura.

------------------------------------------------------------------------

# 3. Relação entre Classe e Equipamento

Decisão:

Cada classe possui equipamentos principais.

Exemplo:

Knight:

-   Sword
-   Axe
-   Club
-   Shield
-   Armaduras pesadas

Paladin:

-   Bow
-   Crossbow
-   Spears
-   Shield

Sorcerer:

-   Wands
-   Equipamentos mágicos

Druid:

-   Equipamentos mágicos e suporte

Monk:

-   Equipamentos relacionados ao combate corporal

------------------------------------------------------------------------

# 4. Liberdade de Uso

Decisão:

Equipamentos possuem relação com classes.

A compatibilidade de equipamentos seguirá a identidade da classe.

------------------------------------------------------------------------

# 5. Raridade de Equipamentos

Decisão:

Equipamentos possuem raridade.

Possíveis categorias futuras:

-   Comum
-   Raro
-   Lendário

A implementação detalhada será definida posteriormente.

------------------------------------------------------------------------

# 6. Durabilidade

Decisão:

Equipamentos não possuem durabilidade.

Eles não quebram com uso.

------------------------------------------------------------------------

# 7. Melhorias e Upgrades

Decisão:

Equipamentos podem receber melhorias.

Exemplo conceitual:

Iron Sword

↓

Upgrade +1

↓

Upgrade +2

O sistema completo será definido posteriormente.

------------------------------------------------------------------------

# 8. Atributos de Equipamentos

Decisão:

Equipamentos possuem atributos próprios.

Exemplos:

Arma:

-   ataque;
-   dano;
-   bônus.

Armadura:

-   defesa;
-   resistência.

Esses atributos influenciam diretamente o personagem.

------------------------------------------------------------------------

# 9. Equipamento Inicial

Decisão:

Equipamento inicial é fixo por classe.

Fluxo:

Criar personagem

↓

Escolher classe

↓

Receber equipamento inicial correspondente

------------------------------------------------------------------------

# 10. Inventário Inicial

Decisão:

O personagem inicia somente com equipamento equipado.

Não recebe itens extras inicialmente.

------------------------------------------------------------------------

# 11. Impacto no Combate

Decisão:

Equipamentos influenciam diretamente o combate.

Exemplo:

Weapon

↓

Attack Power

↓

Damage

------------------------------------------------------------------------

Armor

↓

Defense

↓

Mitigação de dano

------------------------------------------------------------------------

# 12. Relação com Progressão

Equipamentos complementam:

-   classe;
-   atributos;
-   skills;
-   evolução.

Eles não substituem a identidade da classe.

------------------------------------------------------------------------

# 13. Estado do Bloco

Definido:

-   slots;
-   relação classe/equipamento;
-   raridade;
-   ausência de durabilidade;
-   upgrades;
-   atributos próprios;
-   equipamento inicial;
-   impacto no combate.

Próximos blocos:

-   Combat Formula Foundation;
-   Loot System;
-   Advanced Equipment System.
