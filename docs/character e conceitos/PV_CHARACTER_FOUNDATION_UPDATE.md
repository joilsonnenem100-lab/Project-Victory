# PROJECT-VICTORY --- Character Foundation Update

## Status

Consolidação das decisões adicionais do Product Owner para finalizar o
bloco Character Foundation.

------------------------------------------------------------------------

# 1. Character Name Management

## Alteração de nome

Decisão:

O personagem poderá alterar o nome posteriormente.

Regra definida:

-   Existe possibilidade de alteração.
-   A alteração poderá possuir custo ou regra de controle.
-   A frequência exata ainda será definida em sistema próprio.

Objetivo:

Preservar liberdade do jogador sem permitir abuso.

------------------------------------------------------------------------

# 2. Character Name Uniqueness

Decisão:

Nome globalmente único.

Não podem existir dois personagens com o mesmo nome no mundo.

Motivo:

A identidade do personagem possui valor dentro do mundo persistente.

------------------------------------------------------------------------

# 3. Character Deletion

Decisão:

O personagem pode ser deletado.

Porém existe janela de recuperação:

-   Personagem removido aguarda 20 dias.
-   Após esse período pode desaparecer definitivamente da conta.

Fluxo:

Delete Request

↓

Deleted State

↓

Recovery Window (20 dias)

↓

Permanent Removal

------------------------------------------------------------------------

# 4. Characters Per Account

Decisão:

Cada conta possui até 5 personagens.

Estrutura:

Account

├── Character 1 ├── Character 2 ├── Character 3 ├── Character 4 └──
Character 5

------------------------------------------------------------------------

# 5. Class Permanence

Decisão:

A escolha da classe é permanente.

Exemplo:

Knight permanece Knight.

Não existe troca de classe inicial.

Possíveis evoluções futuras são sistemas separados.

------------------------------------------------------------------------

# 6. Character Visible Progression

Informações visíveis:

-   HP
-   Mana
-   Experiência total
-   Experiência necessária para próximo level
-   Level

Além disso, o personagem possui skills visíveis.

------------------------------------------------------------------------

# 7. Magic Level

Magic Level (ML):

Representa o poder mágico do personagem.

Impacta:

-   força de ataques mágicos;
-   força de curas;
-   eficiência de habilidades mágicas.

------------------------------------------------------------------------

# 8. Combat Skills

## Melee Skills

Habilidades relacionadas a armas corporais:

-   Sword Fighting
-   Axe Fighting
-   Club Fighting

------------------------------------------------------------------------

## Distance Fighting

Eficiência com armas de ataque à distância:

-   Bows
-   Crossbows
-   Spears

------------------------------------------------------------------------

## Shielding

Representa eficiência defensiva usando escudos.

------------------------------------------------------------------------

## Fist Fighting

Eficiência em combate utilizando armas do tipo fist.

------------------------------------------------------------------------

## Fishing

Habilidade secundária.

Uso:

Pesca.

Estado:

Não implementado inicialmente.

------------------------------------------------------------------------

# 9. Equipment Initial Behavior

Decisão:

Equipamento inicial pode ser vendido.

A regra de vínculo inicial não será aplicada.

------------------------------------------------------------------------

# 10. Initial Skill Evolution

Decisão:

A habilidade inicial evolui.

Ela faz parte da progressão do personagem.

Fluxo:

Habilidade inicial

↓

Uso

↓

Progressão

↓

Maior eficiência

------------------------------------------------------------------------

# Character Foundation Final State

Definido:

-   criação;
-   nome;
-   identidade;
-   classes;
-   atributos iniciais;
-   progressão base;
-   skills principais;
-   Magic Level;
-   skills de combate;
-   fishing futuro;
-   gerenciamento de personagens;
-   exclusão;
-   permanência de classe.

Próximos blocos separados:

-   Skill System completo;
-   Attributes System;
-   Combat Formula;
-   Equipment System.
