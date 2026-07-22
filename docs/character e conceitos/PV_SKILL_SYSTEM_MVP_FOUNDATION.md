# PROJECT-VICTORY --- Skill System MVP Foundation

## Objetivo

Definir o primeiro sistema de habilidades funcional do Project-Victory.

Escopo:

-   habilidade inicial por classe;
-   desbloqueio por level;
-   uso em combate;
-   custo de mana;
-   cooldown;
-   relação com arma;
-   evolução futura.

Não faz parte deste bloco:

-   árvore de talentos;
-   builds avançadas;
-   passivas complexas;
-   centenas de habilidades.

------------------------------------------------------------------------

# 1. Regra Geral de Skills

Cada classe começa com uma habilidade inicial.

Fluxo:

Classe

↓

Skill Inicial

↓

Combate

↓

Progressão

------------------------------------------------------------------------

# 2. Quantidade Inicial

Decisão:

Inicialmente cada classe possui apenas uma habilidade.

O sistema deve permitir expansão futura.

------------------------------------------------------------------------

# 3. Desbloqueio

Skills são desbloqueadas por level fixo.

Fluxo:

Level

↓

Unlock Skill

↓

Disponível para uso

------------------------------------------------------------------------

# 4. Mana

Skills consomem Mana.

Ataque básico não consome Mana.

------------------------------------------------------------------------

# 5. Ataque Básico

O ataque normal:

-   não usa Mana;
-   depende de arma e sistema de combate.

------------------------------------------------------------------------

# 6. Cooldown

Skills possuem cooldown.

O controle de tempo faz parte da execução da habilidade.

------------------------------------------------------------------------

# 7. Relação com Equipamento

Skills dependem da arma/equipamento quando aplicável.

Exemplo:

Knight utiliza habilidades relacionadas ao combate corpo a corpo.

------------------------------------------------------------------------

# 8. Falha de Skill

Decisão inicial:

Skills sempre executam.

Não existe inicialmente:

-   miss de skill;
-   falha;
-   resistência de execução.

------------------------------------------------------------------------

# 9. Skills Iniciais por Classe

## Knight

Skill:

Ground Slam

Característica:

Dano fixo.

------------------------------------------------------------------------

## Paladin

Skill:

Exori Con

Característica:

Dano fixo.

------------------------------------------------------------------------

## Sorcerer

Skill:

Exori Vis

Característica:

Dano fixo.

------------------------------------------------------------------------

## Druid

Skill:

Exori Tera

Característica:

Dano fixo.

------------------------------------------------------------------------

## Monk

Skill:

Power Punch

Característica:

Dano fixo.

------------------------------------------------------------------------

# 10. Sistema de Níveis de Skill

Decisão:

Não existe evolução de nível da mesma skill inicialmente.

Exemplo não utilizado:

Fire Bolt I

↓

Fire Bolt II

↓

Fire Bolt III

------------------------------------------------------------------------

A expansão futura poderá adicionar evolução.

------------------------------------------------------------------------

# 11. Interface

Skills desbloqueadas são exibidas ao jogador.

O jogador consegue visualizar suas habilidades disponíveis.

------------------------------------------------------------------------

# 12. Estado Atual

Definido:

-   uma skill inicial por classe;
-   desbloqueio por level;
-   custo de Mana;
-   cooldown;
-   relação com equipamento;
-   execução garantida inicialmente;
-   dano fixo nas skills iniciais;
-   visualização das skills.

------------------------------------------------------------------------

# Próximos blocos

-   Combat Turn Structure;
-   Hunt Loop Final.

Depois:

Core Game Loop pronto para contratos.
