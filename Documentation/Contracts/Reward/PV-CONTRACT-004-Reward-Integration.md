# PV-CONTRACT-004 --- Reward Integration

## Local recomendado

Project-Victory/Documentation/Contracts/Reward/

------------------------------------------------------------------------

# Objetivo

Conectar o resultado do combate com a progressão do personagem.

Fluxo:

Monster Dead

↓

Reward Calculation

↓

XP

↓

Gold

↓

Item Reward

↓

Character Update

------------------------------------------------------------------------

# 1. Momento da Recompensa

A recompensa é entregue imediatamente após a morte do monstro.

Fluxo:

Monster Defeated

↓

Reward Granted

------------------------------------------------------------------------

# 2. Experiência

O jogador visualiza o ganho de experiência.

Exemplo:

Rat defeated

+50 XP

------------------------------------------------------------------------

# 3. Gold

Gold não é item.

O jogador recebe Gold diretamente na Wallet.

Após recompensa:

+Gold

↓

Wallet Update

------------------------------------------------------------------------

# 4. Wallet

O personagem possui Wallet separada.

Estrutura:

Character

├── Inventory

└── Wallet

------------------------------------------------------------------------

# 5. Level Up

Quando ocorre Level Up:

O HP atual acompanha o crescimento máximo.

Exemplo:

185/185

↓

200/200

------------------------------------------------------------------------

# 6. Level Up Durante Combate

Level Up pode acontecer imediatamente após receber XP.

Fluxo:

Monster Dead

↓

XP Reward

↓

Level Up

------------------------------------------------------------------------

# 7. Feedback de Recompensa

Após derrotar um monstro, o jogador recebe mensagem.

Exemplo:

Rat defeated

Experience +50

Gold +3

------------------------------------------------------------------------

# 8. Item Drop Inicial

Rat:

Drop:

Nenhum.

Cave Rat:

Drop:

Nenhum inicialmente.

------------------------------------------------------------------------

# 9. Drop Table

Quando o sistema de drop existir:

Cada monstro terá sua própria tabela.

Exemplo futuro:

Monster

↓

Drop Table

↓

Chance

↓

Item

------------------------------------------------------------------------

# 10. Entrega de Item

Itens recebidos vão diretamente para o inventário.

Fluxo:

Drop Generated

↓

Inventory

------------------------------------------------------------------------

# 11. Inventário Cheio

Caso o inventário esteja cheio:

O jogador recebe aviso.

Regra:

Máximo de 1 aviso a cada 1 hora.

------------------------------------------------------------------------

# 12. Registro de Reward

Não existe registro de histórico de recompensa inicialmente.

------------------------------------------------------------------------

# Estado Final do Contrato

Definido:

-   recompensa imediata;
-   XP visualizado;
-   Gold em Wallet;
-   Wallet separada;
-   HP acompanha Level Up;
-   Level Up durante combate permitido;
-   mensagem de recompensa;
-   drops específicos por monstro;
-   inventário automático;
-   aviso de inventário cheio com limite de frequência.

------------------------------------------------------------------------

# Fora de Escopo

Não implementar:

-   histórico de recompensa;
-   economia avançada;
-   loot procedural;
-   raridade;
-   affixes.

------------------------------------------------------------------------

# Próximo contrato

PV-CONTRACT-005 --- Item MVP
