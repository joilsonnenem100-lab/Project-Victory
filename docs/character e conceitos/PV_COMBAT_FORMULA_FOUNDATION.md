# PROJECT-VICTORY --- Combat Formula Foundation

## Status

Documento de consolidação das decisões de Product Owner relacionadas à
fundação do sistema de combate.

Este documento define a filosofia inicial do combate e sua relação com:

Character

↓

Attributes

↓

Skills

↓

Equipment

↓

Combat

↓

Hunt

------------------------------------------------------------------------

# 1. Identidade do Combate

Decisão:

O combate do Project-Victory será híbrido.

Características:

-   execução contínua;
-   ações estratégicas;
-   possibilidade de intervenção do jogador;
-   suporte para evolução futura.

O combate deve funcionar dentro do loop de Hunt.

------------------------------------------------------------------------

# 2. Controle Durante Combate

Decisão:

O combate inicial será automático.

O jogador prepara:

-   classe;
-   equipamentos;
-   habilidades;
-   progressão.

Durante a Hunt:

Personagem

↓

Executa combate automaticamente

------------------------------------------------------------------------

# 3. Ataques Básicos

Decisão:

Modelo híbrido.

Existe:

-   ataque básico;
-   habilidades especiais.

O personagem possui uma forma padrão de causar dano e pode utilizar
habilidades.

------------------------------------------------------------------------

# 4. Dano Físico

Decisão:

O dano físico será calculado através da combinação de fatores.

Modelo conceitual:

Skill de combate

-   

Atributos

-   

Equipamento

=

Dano físico

Exemplos de influência:

-   Sword Fighting;
-   Axe Fighting;
-   Club Fighting;
-   arma equipada.

------------------------------------------------------------------------

# 5. Dano Mágico

Decisão:

O dano mágico considera múltiplos fatores.

Modelo conceitual:

Magic Level

-   

atributos mágicos

-   

equipamento

=

dano mágico

Aplica-se principalmente:

-   Sorcerer;
-   Druid.

Também influencia:

-   Knight;
-   Paladin.

------------------------------------------------------------------------

# 6. Defesa

Decisão:

A defesa será composta por múltiplos elementos.

Modelo conceitual:

Shielding

-   

Equipamento

-   

Atributos

=

Defesa

------------------------------------------------------------------------

# 7. Recursos de Combate

Decisão:

O recurso principal inicial será:

HP.

Recursos adicionais podem existir no futuro.

Quando recebe dano:

HP reduz.

------------------------------------------------------------------------

# 8. Derrota

Decisão:

Quando HP chega a zero:

-   personagem retorna para a cidade;
-   aguarda 30 segundos;
-   pode voltar a caçar novamente.

Não existe perda permanente.

Fluxo:

Derrota

↓

Cidade

↓

Cooldown 30 segundos

↓

Nova Hunt

------------------------------------------------------------------------

# 9. Habilidades

Decisão:

Habilidades utilizam:

-   cooldown;
-   recursos.

Modelo:

Skill usada

↓

Consome recurso

↓

Aguarda cooldown

↓

Disponível novamente

------------------------------------------------------------------------

# 10. Primeiro Combate

Decisão:

A primeira experiência de combate será extremamente simples.

Objetivo:

-   apresentar ataque;
-   mostrar recompensa;
-   validar o loop.

A complexidade será adicionada posteriormente.

------------------------------------------------------------------------

# 11. Estado do Bloco

Definido:

-   combate híbrido;
-   combate automático inicial;
-   ataque básico;
-   habilidades;
-   cálculo baseado em múltiplos fatores;
-   defesa composta;
-   HP como recurso principal;
-   retorno após derrota;
-   cooldown de 30 segundos;
-   habilidades com cooldown e recursos;
-   primeira experiência simples.

Próximos blocos:

-   Monster System;
-   Hunt Loop;
-   Reward System;
-   Loot System;
-   Combat Advanced Mechanics.
