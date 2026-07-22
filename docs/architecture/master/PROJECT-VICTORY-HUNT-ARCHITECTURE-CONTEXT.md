# PROJECT-VICTORY --- HUNT ARCHITECTURE CONTEXT

## Visão

Hunt não deve ser uma ação isolada.

Ela deve evoluir para um ciclo contínuo.

Fluxo:

Selecionar Hunt

↓

Criar Hunt Session

↓

Gerar Encounter

↓

Criar Monster Encounter

↓

Executar Combat

↓

Resultado

↓

Reward

↓

Progression

↓

Novo Encounter

------------------------------------------------------------------------

# Separações importantes

## Hunt Definition

Conteúdo configurado.

Exemplo:

Dark Forest

Possui:

-   monstros possíveis;
-   dificuldade;
-   recompensas.

## Hunt Session

Execução real do jogador.

------------------------------------------------------------------------

# Encounter

Separação:

Encounter Definition:

Configuração.

Encounter Instance:

Encontro real criado.

------------------------------------------------------------------------

# Monster

Monster Definition:

Modelo.

Monster Instance:

Criatura real em execução.

------------------------------------------------------------------------

# Estado atual

Existe fundação:

-   Hunt;
-   Combat;
-   Monster;
-   Reward.

Ainda futuro:

-   loop contínuo;
-   geração de encontros;
-   recuperação;
-   persistência completa.
