# PROJECT-VICTORY --- Monster System Foundation

## Status

Documento de consolidação das decisões de Product Owner relacionadas à
fundação do sistema de monstros.

Este documento define a base entre:

Monster Definition

↓

Monster Instance

↓

Combat

↓

Reward

------------------------------------------------------------------------

# 1. Modelo de Existência dos Monstros

Decisão:

O sistema utiliza separação entre definição e instância.

Modelo:

Monster Definition

↓

Monster Instance

------------------------------------------------------------------------

## Monster Definition

Representa o modelo do monstro.

Exemplo:

Rat

Possui:

-   características base;
-   comportamento;
-   possíveis recompensas.

------------------------------------------------------------------------

## Monster Instance

Representa a criatura real criada durante uma execução.

Exemplo:

Rat #1001

Possui:

-   estado atual;
-   vida atual;
-   participação no combate.

------------------------------------------------------------------------

# 2. Categorias de Monstros

Decisão:

Modelo híbrido.

O sistema deve suportar categorias futuras.

Exemplo:

-   Normal;
-   Elite;
-   Boss.

A primeira implementação utiliza monstros comuns.

------------------------------------------------------------------------

# 3. Level dos Monstros

Decisão:

Monstros não possuem level próprio inicialmente.

A dificuldade será definida principalmente por:

-   área;
-   Hunt;
-   configuração do encontro.

------------------------------------------------------------------------

# 4. Atributos dos Monstros

Decisão:

Modelo evolutivo.

Inicialmente:

-   HP;
-   dano;
-   defesa.

O sistema deve permitir expansão futura para:

-   resistências;
-   habilidades;
-   atributos adicionais.

------------------------------------------------------------------------

# 5. Habilidades dos Monstros

Decisão:

Monstros possuem habilidades.

Exemplo:

Rat:

-   mordida.

Monstros futuros podem possuir:

-   ataques especiais;
-   habilidades únicas.

------------------------------------------------------------------------

# 6. Inteligência dos Monstros

Decisão:

Primeira versão sem IA complexa.

Comportamento inicial:

-   atacar;
-   receber dano;
-   morrer.

Sistemas futuros podem adicionar:

-   fuga;
-   estratégias;
-   comportamentos especiais.

------------------------------------------------------------------------

# 7. Spawn e Encontros

Decisão:

Modelo híbrido.

Os encontros utilizam tabelas configuradas.

A Hunt define possibilidades.

O sistema pode gerar variações.

Fluxo:

Hunt

↓

Encounter Configuration

↓

Monster Instance

↓

Combat

------------------------------------------------------------------------

# 8. Loot dos Monstros

Decisão:

Modelo híbrido.

O resultado pode considerar:

Monster Loot Table

-   

Hunt Context

-   

Outras regras futuras

------------------------------------------------------------------------

# 9. Bosses

Decisão:

Sistema futuro.

Bosses terão arquitetura própria posteriormente.

Possíveis características:

-   habilidades especiais;
-   recompensas únicas;
-   mecânicas próprias.

------------------------------------------------------------------------

# 10. Primeiros Monstros Oficiais

Primeira Hunt:

Ascendancy Path

Monstros:

-   Rat
-   Cave Rat

------------------------------------------------------------------------

# 11. Relação com Combat

Fluxo:

Character

↓

Combat Context

↓

Monster Instance

↓

Combat Resolution

↓

Reward

------------------------------------------------------------------------

# 12. Estado do Bloco

Definido:

-   separação Definition/Instance;
-   categorias;
-   ausência de level inicial;
-   atributos evolutivos;
-   habilidades de monstros;
-   comportamento inicial;
-   spawn híbrido;
-   loot híbrido;
-   bosses futuros;
-   primeiros monstros.

Próximo bloco:

Hunt Runtime Foundation.
