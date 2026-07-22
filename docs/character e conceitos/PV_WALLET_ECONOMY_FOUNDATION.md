# PROJECT-VICTORY --- Wallet & Economy Foundation

## Status

Documento de consolidação das decisões de Product Owner relacionadas ao
sistema de moeda, carteira e economia inicial.

Este sistema separa:

Inventory

de

Wallet

------------------------------------------------------------------------

# 1. Objetivo

A Wallet representa os recursos monetários do personagem.

Gold não é um item.

Fluxo:

Reward

↓

Wallet

↓

Saldo do personagem

------------------------------------------------------------------------

# 2. Modelo de Moeda

Decisão:

O Project-Victory inicia com uma moeda principal.

Moeda inicial:

Gold

O sistema deve permitir expansão futura para múltiplas moedas.

------------------------------------------------------------------------

# 3. Responsabilidade da Wallet

Decisão:

Wallet pertence ao Account.

Modelo:

Account

↓

Wallet

↓

Characters

------------------------------------------------------------------------

A carteira pode ser compartilhada entre personagens da mesma conta.

------------------------------------------------------------------------

# 4. Transferência de Gold

Decisão:

Transferência entre jogadores existirá.

Porém haverá taxas em cada envio.

Exemplo:

Jogador A envia Gold

↓

Sistema calcula taxa

↓

Jogador B recebe valor líquido

------------------------------------------------------------------------

# 5. Limite de Gold

Decisão:

Não existe limite inicial.

A capacidade da Wallet é ilimitada inicialmente.

------------------------------------------------------------------------

# 6. Perda de Gold

Decisão:

Não existe perda de Gold inicialmente.

Sistemas futuros podem criar regras econômicas específicas.

------------------------------------------------------------------------

# 7. Origem do Gold

Decisão:

Gold pode vir de múltiplas fontes.

Inicialmente:

-   monstros;
-   quests;
-   comércio;
-   NPCs;
-   mercado do jogo;
-   lojas;
-   sistemas futuros.

------------------------------------------------------------------------

# 8. Uso do Gold

Decisão:

Gold será utilizado em sistemas de compra.

Exemplos:

-   NPCs;
-   lojas;
-   poções;
-   equipamentos;
-   serviços futuros.

------------------------------------------------------------------------

# 9. Compra de Equipamentos

Decisão:

A primeira versão utiliza compra direta.

Exemplo:

NPC

↓

Equipamento

↓

Pagamento em Gold

------------------------------------------------------------------------

# 10. Venda de Itens

Decisão:

Jogadores poderão vender itens.

Fluxo:

Inventory

↓

Venda

↓

Wallet

------------------------------------------------------------------------

# 11. Economia entre Jogadores

Decisão:

O comércio entre jogadores existirá futuramente.

Não será implementado no primeiro ciclo.

Possibilidades futuras:

-   troca direta;
-   mercado;
-   taxas;
-   economia global.

------------------------------------------------------------------------

# 12. Histórico Econômico

Decisão:

Não será armazenado inicialmente.

Futuramente poderá existir:

-   auditoria;
-   histórico;
-   análise econômica.

------------------------------------------------------------------------

# 13. Exibição da Wallet

Decisão:

Gold será exibido diretamente ao jogador.

Exemplo:

Wallet:

5.230 Gold

------------------------------------------------------------------------

# 14. Relação com Inventory

Separação definida:

Inventory:

-   equipamentos;
-   consumíveis;
-   itens.

Wallet:

-   moedas;
-   saldo financeiro.

------------------------------------------------------------------------

# 15. Arquitetura Futura

Modelo:

Account

↓

Wallet Aggregate

↓

Transactions

↓

Economy Systems

------------------------------------------------------------------------

# 16. Estado do Bloco

Definido:

-   Gold como moeda inicial;
-   expansão futura para múltiplas moedas;
-   Wallet separada;
-   Wallet pertencente ao Account;
-   transferências futuras com taxa;
-   sem limite inicial;
-   sem perda inicial;
-   múltiplas fontes de Gold;
-   compras;
-   venda de itens;
-   comércio futuro;
-   ausência inicial de histórico;
-   exibição de saldo.

Próximos blocos:

-   Runtime State Foundation;
-   Game Session Foundation;
-   Primeiro contrato de gameplay.
