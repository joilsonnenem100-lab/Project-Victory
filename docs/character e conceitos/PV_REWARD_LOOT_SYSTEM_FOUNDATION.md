# PROJECT-VICTORY --- Reward & Loot System Foundation

## Status

Documento de consolidação das decisões de Product Owner relacionadas ao
sistema de recompensas e loot.

Fluxo:

Combat Result

↓

Reward System

↓

Loot / XP / Gold

↓

Character Progression

# 1. Recompensa por Vitória

Toda vitória gera recompensa.

Resultado de combate:

-   XP
-   possibilidade de loot
-   Gold

# 2. Experiência

A experiência inicialmente é obtida através da derrota de monstros.

Exemplo:

Rat derrotado

↓

XP recebido

# 3. Modelo de XP

Decisão:

Tabela fixa de experiência.

Todos os personagens utilizam a mesma referência.

Exemplo:

5020 XP total

↓

Level 30

↓

30% de progresso para Level 31

# 4. Level Up

O crescimento é automático.

O jogador não distribui pontos.

A classe define:

-   HP
-   Mana
-   evolução
-   progressão

# 5. Loot

O loot é gerado automaticamente após a morte do monstro.

Fluxo:

Monster Death

↓

Loot Generation

↓

Entrega da recompensa

# 6. Quantidade de Drops

A Loot Table controla quantidade e resultados.

Pode existir:

-   múltiplos drops;
-   quantidade variável;
-   diferentes resultados.

# 7. Chance de Drop

Modelo:

Peso + porcentagem.

A Loot Table define:

-   chance;
-   peso;
-   quantidade.

# 8. Gold

Gold não é item.

Ele pertence à Wallet do jogador.

Exemplo:

Rat:

1 até 5 Gold

Processo:

↓

Calcula chance

↓

Define quantidade

↓

Adiciona ao saldo da Wallet

O jogador visualiza somente o saldo.

# 9. Itens Raros

Monstros comuns não possuem itens raros inicialmente.

Itens raros ficam para:

-   monstros especiais;
-   bosses;
-   conteúdos futuros.

# 10. Recompensa de Hunt

O sistema suporta recompensa de Hunt.

Inicialmente:

A principal recompensa vem dos monstros.

Futuro:

-   conclusão de Hunt;
-   bônus;
-   recompensas especiais.

# 11. Aplicação da Progressão

A recompensa é aplicada imediatamente.

Fluxo:

Combat Result

↓

Reward

↓

XP aplicada

↓

Character atualizado

# 12. Estado do Bloco

Definido:

-   recompensa por vitória;
-   XP por monstros;
-   tabela fixa de XP;
-   crescimento automático;
-   loot automático;
-   Loot Table;
-   peso + porcentagem;
-   Gold em Wallet;
-   itens raros futuros;
-   recompensa de Hunt;
-   atualização imediata.

Próximos blocos:

-   Wallet/Economy Foundation;
-   Runtime State Foundation.
