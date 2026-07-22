# PROJECT-VICTORY

# Character Identity & Progression Master Document v2

## 1. Objetivo do Documento

Este documento consolida as decisões de produto relacionadas ao
nascimento, identidade, classes, progressão inicial e primeira
experiência do personagem no Project-Victory.

Ele serve como referência antes de novas decisões ou questionários
relacionados ao personagem.

------------------------------------------------------------------------

# 2. Filosofia da Criação de Personagem

A criação de personagem deve transmitir:

"Existe um mundo gigante para explorar e minha história começa agora."

O personagem não é apenas uma configuração inicial.

Ele representa:

-   identidade;
-   escolha;
-   caminho;
-   evolução;
-   história construída pelo jogador.

Fluxo conceitual:

Conta ↓ Personagem ↓ Classe ↓ Entrada no mundo ↓ Primeira aventura ↓
Evolução

------------------------------------------------------------------------

# 3. Character Creation Experience

Fluxo definido:

Login

↓

Sem personagem

↓

Criação

↓

Nome

↓

Classe

↓

Equipamento inicial

↓

Confirmação

↓

Entrada no mundo

------------------------------------------------------------------------

## Decisões confirmadas

-   Existem 5 classes iniciais.
-   A classe é escolhida no nascimento.
-   Classes possuem papel fixo.
-   Aparência customizável não faz parte da primeira versão.
-   O personagem recebe equipamento inicial.
-   O personagem entra no mundo após criação.

------------------------------------------------------------------------

# 4. Classes Iniciais

Classes oficiais:

-   Knight
-   Paladin
-   Sorcerer
-   Druid
-   Monk

A classe representa:

-   estilo de combate;
-   progressão;
-   identidade;
-   habilidades futuras.

------------------------------------------------------------------------

# 5. Filosofia das Classes

Uma classe não é apenas um modificador numérico.

Ela representa uma fantasia de personagem.

O jogador deve olhar para a classe e entender:

"Que tipo de aventureiro estou criando?"

------------------------------------------------------------------------

# 6. Knight

## Identidade

Guerreiro focado em combate físico, resistência e domínio de armas.

## Crescimento

HP: +15 por level

Mana: +5 por level

## Especialização

Maior evolução de skills físicas:

-   Sword Fighting
-   Axe Fighting
-   Club Fighting
-   Shielding

## Característica

É a classe com maior crescimento de vida.

------------------------------------------------------------------------

# 7. Paladin

## Identidade

Combatente híbrido entre resistência, distância e magia.

## Crescimento

HP: +10 por level

Mana: +15 por level

## Especialização

-   Distance Fighting
-   Shielding moderado
-   Magic Level intermediário

## Característica

Equilíbrio entre combate físico e recursos mágicos.

------------------------------------------------------------------------

# 8. Sorcerer

## Identidade

Especialista em poder mágico ofensivo.

## Crescimento

HP: +5 por level

Mana: +30 por level

## Especialização

Maior progressão de Magic Level.

## Característica

Maior ganho de mana do jogo.

Contrapartida:

Baixa resistência física.

------------------------------------------------------------------------

# 9. Druid

## Identidade

Usuário de magia focado em suporte, cura e controle.

## Crescimento

HP: +5 por level

Mana: +30 por level

## Especialização

-   Cura
-   Suporte
-   Poder mágico

## Característica

Mesmo crescimento mágico do Sorcerer, porém com identidade diferente.

------------------------------------------------------------------------

# 10. Monk

## Identidade

Classe equilibrada baseada em disciplina e controle corporal.

## Crescimento

HP: +10 por level

Mana: +10 por level

## Característica

Classe intermediária entre resistência e recurso mágico.

------------------------------------------------------------------------

# 11. Modelo de Atributos

Decisão confirmada:

Todas as classes começam com atributos iniciais iguais.

A diferenciação acontece através de:

-   crescimento por level;
-   habilidades;
-   progressão;
-   equipamentos.

------------------------------------------------------------------------

# 12. Habilidades

Decisão confirmada:

As habilidades são desbloqueadas por nível.

Não existe escolha inicial de habilidade.

Modelo:

Level

↓

Novo desbloqueio

↓

Nova capacidade

------------------------------------------------------------------------

# 13. Equipamento Inicial

Decisão confirmada:

O equipamento inicial possui função mecânica.

Não possui narrativa própria inicialmente.

Exemplo:

Knight:

Espada inicial Armadura inicial

------------------------------------------------------------------------

# 14. Quantidade de Personagens

Decisão inicial:

Uma conta poderá possuir até 5 personagens.

Modelo:

Account

├── Knight ├── Paladin ├── Sorcerer ├── Druid └── Monk

------------------------------------------------------------------------

# 15. Entrada no Mundo

Após criar personagem:

Criar

↓

Entrar no mundo

↓

Cidade Principal

------------------------------------------------------------------------

# 16. Cidade Principal

A Cidade Principal será o centro permanente do jogador.

Futuras funções:

-   NPCs;
-   comércio;
-   evolução;
-   serviços;
-   eventos;
-   preparação para Hunts.

------------------------------------------------------------------------

# 17. Primeiro Spawn

O personagem inicia:

Praça Central da Cidade Principal.

------------------------------------------------------------------------

# 18. Primeira Mensagem

O jogador recebe uma mensagem simples de boas-vindas.

Objetivo:

Orientar sem retirar descoberta.

------------------------------------------------------------------------

# 19. Primeira Hunt

Nome:

Ascendancy Path

------------------------------------------------------------------------

## Objetivo

A primeira Hunt representa:

Primeira vitória.

Primeiro progresso.

Primeiro passo da evolução.

------------------------------------------------------------------------

# 20. Estrutura da Hunt

Características:

-   Solo.
-   Loop contínuo como visão de produto.
-   Encontros.
-   Combate.
-   Recompensas.

------------------------------------------------------------------------

# 21. Primeiros Monstros

Definidos:

-   Rat
-   Cave Rat

------------------------------------------------------------------------

# 22. Loot

Decisão:

A Hunt utiliza Loot Table normal dos monstros.

A primeira implementação pode possuir limitações, mas a arquitetura deve
considerar loot real.

------------------------------------------------------------------------

# 23. Derrota Inicial

A primeira Hunt não possui derrota obrigatória.

O objetivo inicial é apresentar:

-   combate;
-   recompensa;
-   evolução.

------------------------------------------------------------------------

# 24. Progressão de Conteúdo

Após retornar para a cidade:

Novo conteúdo é desbloqueado progressivamente.

Regra:

Completar Hunt + requisito de nível.

------------------------------------------------------------------------

# 25. Conteúdo Permanente

Uma Hunt concluída continua disponível.

Estados conceituais:

Locked

Available

Completed

------------------------------------------------------------------------

# 26. Relação com Arquitetura Futura

Character representa:

"Quem o jogador é."

Runtime representa:

"O que o jogador está fazendo."

Separação:

Character State

↓

Runtime State

↓

Hunt Runtime

↓

Combat Runtime

------------------------------------------------------------------------

# 27. Lacunas Futuras

Ainda não definidos:

-   nomes finais de atributos;
-   valores iniciais;
-   fórmulas de dano;
-   árvores avançadas de skills;
-   evoluções de classe;
-   equipamentos específicos;
-   lore das classes;
-   cidade principal detalhada.

------------------------------------------------------------------------

# 28. Princípio Final

O Project-Victory deve fazer o jogador sentir:

"Este personagem é meu."

A classe define o caminho inicial.

A progressão constrói a identidade.

O mundo cria a história.
