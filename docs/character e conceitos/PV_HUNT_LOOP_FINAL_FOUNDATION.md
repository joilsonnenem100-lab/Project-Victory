# PROJECT-VICTORY --- Hunt Loop Final Foundation

## Objetivo

Definir o fluxo completo da Hunt para o primeiro ciclo jogável do
Project-Victory.

Fluxo:

Cidade/Tela Inicial

↓

Escolher Hunt

↓

Iniciar Hunt

↓

Encounter

↓

Combat

↓

Reward

↓

Próximo Encounter

↓

Finalização

------------------------------------------------------------------------

# 1. Entrada da Hunt

Decisão:

Ao entrar no jogo, a tela inicial possuirá a opção de Hunt.

Fluxo:

Login

↓

Tela Inicial

↓

Hunt disponível

↓

Iniciar Hunt

------------------------------------------------------------------------

# 2. Primeira Hunt

Nome definido:

Ascendancy Path

------------------------------------------------------------------------

# 3. Duração da Hunt

Decisão:

A Hunt continua até o jogador sair ou morrer.

Não existe duração fixa inicial.

------------------------------------------------------------------------

# 4. Quantidade inicial de encontros

Primeira versão:

3 encontros.

Fluxo:

Encounter 1

↓

Combat

↓

Reward

↓

Encounter 2

↓

Combat

↓

Reward

↓

Encounter 3

------------------------------------------------------------------------

# 5. Composição da Hunt

A composição inicial da Hunt é fixa.

Não existe geração aleatória inicial.

------------------------------------------------------------------------

# 6. Nível recomendado

Hunts possuem nível recomendado.

Exemplo:

Ascendancy Path

Level recomendado: 1

------------------------------------------------------------------------

# 7. Saída da Hunt

Existe a opção:

Exit Hunt

Regra:

Ao clicar:

-   aguarda 5 segundos;
-   depois retorna para tela inicial.

Existe também uma penalidade de reentrada:

-   após sair, aguarda 10 segundos para entrar novamente;
-   esse tempo poderá aumentar conforme quantidade de vezes realizada.

------------------------------------------------------------------------

# 8. Morte durante Hunt

Fluxo:

Character Dead

↓

Retorno para cidade/tela inicial

Sem perda inicial.

------------------------------------------------------------------------

# 9. Finalização da Hunt

Decisão:

Não existe recompensa especial de conclusão inicialmente.

As recompensas são provenientes dos encontros:

-   XP;
-   Gold;
-   Drops futuros.

------------------------------------------------------------------------

# 10. Após terminar Hunt

Decisão:

Retorna automaticamente para a tela inicial.

------------------------------------------------------------------------

# 11. Quantidade inicial de Hunts

Primeira versão:

Somente uma Hunt disponível.

Hunt:

Ascendancy Path

------------------------------------------------------------------------

# 12. Desbloqueio de novas Hunts

Inicialmente:

Baseado em level do personagem.

Exemplo:

Level necessário

↓

Nova Hunt disponível

------------------------------------------------------------------------

# 13. Informações da Hunt

Inicialmente:

Não exibe progresso da Hunt.

O foco é o ciclo funcional.

------------------------------------------------------------------------

# 14. Execução da Hunt

Decisão:

Após iniciar:

A Hunt executa automaticamente.

Fluxo:

Hunt Started

↓

Encounter

↓

Combat

↓

Reward

↓

Next Encounter

------------------------------------------------------------------------

# 15. Tutorial

Decisão:

A primeira Hunt não possui tutorial obrigatório.

O jogador aprende através da experiência.

------------------------------------------------------------------------

# CORE GAME LOOP FINALIZADO

O ciclo básico definido:

Login

↓

Character

↓

Tela Inicial

↓

Hunt

↓

Monster Encounter

↓

Turn Combat

↓

Reward

↓

Progression

↓

Novo Loop

------------------------------------------------------------------------

# Estado dos sistemas

Account: Fechado

Character: Fechado

Attributes: Fechado

Skills: Fechado

Combat MVP: Fechado

Monster MVP: Fechado

Reward MVP: Fechado

Hunt Loop: Fechado

------------------------------------------------------------------------

# Próxima etapa

Transformar o Core Gameplay Loop em contratos de implementação.
