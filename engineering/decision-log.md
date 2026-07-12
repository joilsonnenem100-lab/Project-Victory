# Project-Victory Decision Log

> Status: ATIVO
>
> Este documento registra todas as decisões oficiais aprovadas durante o desenvolvimento do Project-Victory.
>
> Nenhuma decisão deve ser removida. Caso uma decisão seja substituída, ela deverá permanecer registrada e receber o status SUPERSEDED.

---

# DEC-0001

## Título

Gold não ocupa espaço no inventário.

## Status

LOCKED

## Decisão

Todo gold obtido através de loot será adicionado diretamente ao saldo do jogador.

Não existirá item "Gold Coin".

## Impacto

- Inventory
- Economy
- Loot
- Market

---

# DEC-0002

## Título

Project-Victory não utiliza sistema de Capacity.

## Status

LOCKED

## Decisão

O limite do inventário será definido exclusivamente pela quantidade de slots disponíveis.

Não existirá sistema de peso ou capacidade.

## Impacto

- Inventory
- Character

---

# DEC-0003

## Título

Inventário inicial.

## Status

LOCKED

## Decisão

Todo personagem inicia com 50 slots.

A quantidade poderá ser aumentada futuramente através de VC Points.

## Impacto

- Inventory
- Monetização

---

# DEC-0004

## Título

Mochilas.

## Status

LOCKED

## Decisão

Não existirão mochilas como containers.

O slot Backpack será apenas visual.

Todo loot será armazenado diretamente no inventário principal.

## Impacto

- Inventory
- UI
- Backend

---

# DEC-0005

## Título

Progressão sem perda.

## Status

LOCKED

## Decisão

O personagem nunca perde:

- EXP
- Level

Ao morrer.

## Impacto

- Progression
- Character

---

# DEC-0006

## Título

Ascension.

## Status

LOCKED

## Decisão

Cada Ascension possui limite de Level 500.

Após atingir o limite, o jogador poderá realizar gratuitamente sua Ascension.

Os atributos permanentes serão mantidos.

Novas regiões serão desbloqueadas.

O retorno às regiões antigas continuará permitido.

## Impacto

- Progression
- World

---

# DEC-0007

## Título

Sistema de Food.

## Status

LOCKED

## Decisão

O jogo não possui regeneração natural.

Toda regeneração depende de Food ativa.

## Impacto

- Character
- Combat

---

# DEC-0008

## Título

Boost de EXP.

## Status

LOCKED

## Decisão

O tempo do Boost somente começa a ser consumido após o jogador obter experiência.

O tempo continua correndo normalmente até terminar.

## Impacto

- Progression
- Premium

---

# DEC-0009

## Título

Sistema de raridade.

## Status

LOCKED

## Decisão

Raridades oficiais:

- Normal
- Magic
- Rare

Categoria Especial:

- Unique

## Impacto

- Item
- Loot
- Market

---

# DEC-0010

## Título

Sistema de afixos.

## Status

LOCKED

## Decisão

Normal

- nenhum afixo

Magic

- exatamente 1 afixo

Rare

- exatamente 2 ou 3 afixos

Unique

- atributos fixos

## Impacto

- Item
- Equipment

---

# DEC-0011

## Título

Upgrade.

## Status

LOCKED

## Decisão

Somente equipamentos Rare poderão receber upgrade até +14.

Unique não recebe upgrade.

## Impacto

- Equipment
- Economy

---

# DEC-0012

## Título

Durabilidade.

## Status

LOCKED

## Decisão

Equipamentos possuem durabilidade.

Nunca quebram.

Nunca ficam abaixo de 50%.

A durabilidade diminui apenas ao morrer.

O reparo é realizado através de NPC.

## Impacto

- Equipment

---

# DEC-0013

## Título

Filosofia dos equipamentos.

## Status

LOCKED

## Decisão

Equipamentos representam uma das maiores fontes de poder do personagem.

Itens extremamente raros e totalmente evoluídos devem representar uma conquista de longo prazo.

## Impacto

- Equipment
- Combat
- Economy

---

# DEC-0014

## Título

Filosofia Idle.

## Status

LOCKED

## Decisão

Project-Victory é um Idle MMORPG.

As decisões de engenharia devem priorizar:

- backend
- estabilidade
- progressão
- economia

O desenvolvimento de animações complexas não possui prioridade.

## Impacto

Projeto inteiro.

---

# DEC-0015

## Título

Knowledge Base.

## Status

LOCKED

## Decisão

Nenhuma regra oficial poderá existir apenas na conversa.

Toda decisão deverá ser registrada no GitHub.

## Impacto

Projeto inteiro.

---

# DEC-0016

## Título

Prioridade visual das Hunts.

## Origem

HS-001 — HuntService

## Status

LOCKED

## Decisão

As Hunts possuem prioridade mínima de desenvolvimento visual.

O foco principal do Project-Victory está em:

- Backend
- Progressão
- Economia
- Estabilidade

Elementos visuais servem apenas para contextualizar a Hunt.

---

# DEC-0017

## Título

Pack Size configurável.

## Origem

HS-001 — HuntService

## Status

LOCKED

## Decisão

Toda Hunt possui um Pack Size configurável.

Valor padrão aprovado:

Pack Size = 3

O valor nunca deverá ser fixo no código.

---

# DEC-0018

## Título

Monster Pool.

## Origem

HS-001 — HuntService

## Status

LOCKED

## Decisão

Cada Hunt possui uma Monster Pool própria.

Exemplo:

Dragon

Dragon Elite

Cada entrada possui sua própria Chance de Spawn.

Monstros Elite possuem limite inicial de um exemplar por Pack.

Eventos poderão alterar temporariamente a Monster Pool sem necessidade de criar novas Hunts.

---

# DEC-0019

## Título

Loot pertence ao Monstro.

## Origem

HS-001 — HuntService

## Status

LOCKED

## Decisão

O Loot pertence exclusivamente ao Monstro.

As Hunts não possuem Loot próprio.

---

# DEC-0020

## Título

Respawn das Waves.

## Origem

HS-001 — HuntService

## Status

LOCKED

## Decisão

Após a morte do último monstro do Pack.

O servidor aguarda:

1 segundo

Então gera um novo Pack completo.

O tempo deverá ser configurável.

---

# DEC-0021

## Título

Instâncias de Hunt.

## Origem

HS-001 — HuntService

## Status

LOCKED

## Decisão

Cada jogador possui sua própria instância de Hunt.

A instância é criada ao entrar na Hunt.

A instância é destruída quando:

- jogador sai;
- jogador morre;
- sessão é encerrada.

---

# DEC-0022

## Título

Troca de Hunt.

## Origem

HS-001 — HuntService

## Status

LOCKED

## Decisão

O jogador pode trocar livremente de Hunt.

Cooldown inicial:

5 segundos.

O servidor poderá aumentar temporariamente este tempo como mecanismo de proteção contra abuso.

---

# DEC-0023

## Título

Modo Offline.

## Origem

HS-001 — HuntService

## Status

LOCKED

## Decisão

Ao fechar o navegador ou perder a conexão.

O personagem entra em modo Offline.

Durante o modo Offline:

- Não recebe Loot.
- Recebe EXP reduzida.
- Recebe Gold reduzido.

Os valores deverão permanecer configuráveis pelo servidor.

---

# DEC-0024

## Título

Filosofia de Descoberta.

## Origem

HS-001 — HuntService

## Status

LOCKED

## Decisão

O jogo não deverá informar automaticamente quais Hunts são melhores para:

- EXP
- Loot
- Recursos

Essas informações deverão ser descobertas pela comunidade.

---

# ENG-007

## Título

Escalabilidade acima da implementação imediata.

## Status

LOCKED

## Regra

Sempre que existirem duas soluções que entreguem a mesma experiência ao jogador.

Deverá ser escolhida a arquitetura:

- mais escalável;
- menor custo computacional;
- menor consumo de memória;
- preparada para futuras expansões.

Qualquer alteração de gameplay continua dependendo de aprovação do Game Design.
---

# DEC-0025

## Título

Combat baseado em turnos fixos.

## Origem

CS-001 — CombatService

## Status

LOCKED

## Decisão

O combate utiliza turnos fixos.

Attack Speed e Cast Speed não existem inicialmente.

A velocidade de ataque não influencia o combate.

O balanceamento será realizado através de:

- Level;
- Skills;
- Equipamentos;
- Afixos;
- Imbuements;
- Spells.

---

# DEC-0026

## Título

Ordem do turno de combate.

## Origem

CS-001 — CombatService

## Status

LOCKED

## Decisão

O ciclo de combate segue:

1. Jogador realiza ação.
2. Jogador causa dano.
3. Monstros sobreviventes realizam ataque.
4. Novo turno inicia.

O jogador sempre possui prioridade de ação.

---

# DEC-0027

## Título

Seleção de alvo.

## Origem

CS-001 — CombatService

## Status

LOCKED

## Decisão

O personagem ataca sempre o primeiro monstro da Pack List.

Quando o monstro é derrotado, o próximo monstro da lista torna-se o alvo automaticamente.

---

# DEC-0028

## Título

Área das Skills.

## Origem

CS-001 — CombatService

## Status

LOCKED

## Decisão

Skills de área possuem comportamento definido individualmente.

Cada Skill determina sua quantidade de alvos atingidos.

Uma Skill pode atingir:

- quantidade fixa de monstros;
- todos os monstros do Pack.

---

# DEC-0029

## Título

Estrutura do CombatService.

## Origem

CS-001 — CombatService

## Status

LOCKED

## Decisão

O CombatService será dividido em responsabilidades menores.

Arquitetura:

CombatService

↓

TargetService

↓

DamageCalculator

↓

CriticalCalculator

↓

DeathService

↓

LootService

---

# DEC-0030

## Título

Tipos oficiais de dano.

## Origem

DS-001 — DamageSystem

## Status

LOCKED

## Decisão

Tipos oficiais:

- Physical
- Fire
- Ice
- Earth
- Death
- Holy

---

# DEC-0031

## Título

Prioridade do dano físico.

## Origem

DS-001 — DamageSystem

## Status

LOCKED

## Decisão

A prioridade do dano físico segue:

Weapon

↓

Skill

↓

Level

Equipamentos representam a maior diferença entre personagens com mesmo nível e Skill.

---

# DEC-0032

## Título

Equipamentos aumentam dano base.

## Origem

DS-001 — DamageSystem

## Status

LOCKED

## Decisão

Equipamentos aumentam principalmente o dano base.

Atributos adicionais pertencem aos sistemas de:

- Afixos
- Imbuements
- Upgrades

---

# DEC-0033

## Título

Skill influencia eficiência geral.

## Origem

DS-001 — DamageSystem

## Status

LOCKED

## Decisão

Skills influenciam o desempenho geral do personagem.

Não representam apenas dano bruto.

---

# DEC-0034

## Título

Variação de dano.

## Origem

DS-001 — DamageSystem

## Status

LOCKED

## Decisão

O dano possui variação calculada através de múltiplos fatores:

- Equipamento
- Skill
- Level
- Outros modificadores futuros

---

# DEC-0035

## Título

Fórmula de Skills e Spells.

## Origem

DS-001 — DamageSystem

## Status

LOCKED

## Decisão

Skills e Spells possuem dano base próprio.

O dano final utiliza modificadores relacionados a:

- Skill utilizada;
- Multiplicador da arma;
- Magic Level quando aplicável;
- Categoria da arma.

---

# DEC-0036

## Título

Resistência Elemental.

## Origem

DS-001 — DamageSystem

## Status

LOCKED

## Decisão

Resistências elementais são aplicadas após o cálculo inicial de dano.

---

# DEC-0037

## Título

Critical Damage.

## Origem

DS-001 — DamageSystem

## Status

LOCKED

## Decisão

Critical é aplicado no final do cálculo.

Ordem:

Dano inicial

↓

Resistências

↓

Critical

Critical representa um grande diferencial de poder.

---

# DEC-0038

## Título

Defesa Elemental.

## Origem

DS-001 — DamageSystem

## Status

LOCKED

## Decisão

Cada elemento possui sua própria defesa/resistência.

---

# DEC-0039

## Título

Arquitetura DamageCalculator.

## Origem

DS-001 — DamageSystem

## Status

LOCKED

## Decisão

O cálculo de dano será genérico.

Arquitetura:

DamageCalculator

↓

DamageContext

↓

Modifiers

O sistema não será separado por vocação.

---

# DEC-0040

## Título

Filosofia do Market.

## Origem

MS-001 — MarketService

## Status

LOCKED

## Decisão

O Market representa sabedoria e investimento.

Ele recompensa jogadores que entendem economia, valor dos itens e oportunidades.

---

# DEC-0041

## Título

Market Global.

## Origem

MS-001 — MarketService

## Status

LOCKED

## Decisão

O Project-Victory utiliza um Market global.

Todos os jogadores do servidor acessam os mesmos anúncios.

---

# DEC-0042

## Título

Venda de Item Individual.

## Origem

MS-001 — MarketService

## Status

LOCKED

## Decisão

O Market negocia instâncias específicas de itens.

O comprador recebe exatamente o item anunciado.

Atributos, raridade, upgrade, afixos e imbuements permanecem iguais.

---

# DEC-0043

## Título

Duração dos anúncios.

## Origem

MS-001 — MarketService

## Status

LOCKED

## Decisão

Anúncios possuem duração padrão de 7 dias.

O valor deve ser configurável pelo servidor.

---

# DEC-0044

## Título

Taxa de anúncio.

## Origem

MS-001 — MarketService

## Status

LOCKED

## Decisão

A taxa do Market é cobrada no momento da criação do anúncio.

---

# DEC-0045

## Título

Sistema de preço.

## Origem

MS-001 — MarketService

## Status

LOCKED

## Decisão

O Market suporta:

- preço fixo;
- sistema de leilão.

---

# DEC-0046

## Título

Trade direto entre jogadores.

## Origem

MS-001 — MarketService

## Status

LOCKED

## Decisão

Jogadores podem negociar diretamente sem taxas.

O objetivo é incentivar interação social.

---

# DEC-0047

## Título

Proteção de item no Market.

## Origem

MS-001 — MarketService

## Status

LOCKED

## Decisão

Itens anunciados no Market ficam bloqueados no sistema.

Eles não permanecem disponíveis no inventário.

---

# DEC-0048

## Título

Unique Item Tracking.

## Origem

MS-001 — MarketService

## Status

LOCKED

## Decisão

Itens Unique terão controle da quantidade existente no servidor.

---

# DEC-0049

## Título

MarketService Architecture.

## Origem

MS-001 — MarketService

## Status

LOCKED

## Decisão

Arquitetura:

MarketService

↓

ListingService

↓

TradeService

↓

PriceHistoryService

↓

TaxService

---

# DEC-0050

## Título

Comunicação de Item no Chat.

## Origem

MS-001 — MarketService

## Status

LOCKED

## Decisão

O jogador poderá inserir um item no chat para outros jogadores visualizarem suas informações.

O sistema deverá mostrar exatamente o item anunciado.
---

# DEC-0051

## Título

Filosofia do Inventário.

## Origem

IS-001 — InventoryService

## Status

LOCKED

## Decisão

No Project-Victory, o inventário representa segurança e exclusividade.

O inventário deve proteger as conquistas obtidas pelo jogador.

---

# DEC-0052

## Título

Slots de Inventário.

## Origem

IS-001 — InventoryService

## Status

LOCKED

## Decisão

O personagem possui:

50 slots base.

A quantidade poderá ser expandida futuramente através de sistemas aprovados.

---

# DEC-0053

## Título

Capacidade.

## Origem

IS-001 — InventoryService

## Status

LOCKED

## Decisão

Não existe sistema de Capacity.

Não existe sistema de peso.

A limitação é baseada em Slots.

---

# DEC-0054

## Título

Backpack Visual.

## Origem

IS-001 — InventoryService

## Status

LOCKED

## Decisão

Existe slot visual de Backpack.

O slot poderá receber itens cosméticos de mochila.

Não funciona como container.

---

# DEC-0055

## Título

Organização do Inventário.

## Origem

IS-001 — InventoryService

## Status

LOCKED

## Decisão

O inventário possui:

Recent Loot

All Container

Recent Loot registra itens obtidos recentemente.

All Container representa todos os itens armazenados.

---

# DEC-0056

## Título

Stack.

## Origem

IS-001 — InventoryService

## Status

LOCKED

## Decisão

Itens consumíveis e recursos possuem Stack.

Equipamentos não possuem Stack.

---

# DEC-0057

## Título

Limite de Stack.

## Origem

IS-001 — InventoryService

## Status

LOCKED

## Decisão

O limite padrão de Stack é:

666 unidades.

O valor deve permanecer configurável.

---

# DEC-0058

## Título

Item Template e Item Instance.

## Origem

IS-001 — InventoryService

## Status

LOCKED

## Decisão

O sistema utiliza separação:

Item Template

↓

Item Instance

Template representa o item base.

Instance representa a variação individual obtida pelo jogador.

---

# DEC-0059

## Título

Persistência do Inventário.

## Origem

IS-001 — InventoryService

## Status

LOCKED

## Decisão

O inventário utiliza salvamento contínuo.

Eventos importantes devem ser persistidos imediatamente.

---

# DEC-0060

## Título

Validação de Transferência.

## Origem

IS-001 — InventoryService

## Status

LOCKED

## Decisão

Toda movimentação de item deve ser validada pelo servidor.

Validações:

- propriedade;
- quantidade;
- espaço disponível;
- bloqueios;
- Market;
- Trade.

---

# DEC-0061

## Título

Arquitetura InventoryService.

## Origem

IS-001 — InventoryService

## Status

LOCKED

## Decisão

Arquitetura:

InventoryService

↓

InventoryContainer

↓

Slot

↓

ItemInstance
---

# DEC-0062

## Título

Filosofia dos Equipamentos.

## Origem

ES-002 — EquipmentService

## Status

LOCKED

## Decisão

No Project-Victory, um equipamento representa conquista e poder.

Equipamentos representam dedicação, investimento e evolução.

---

# DEC-0063

## Título

Equipment Slots.

## Origem

ES-002 — EquipmentService

## Status

LOCKED

## Decisão

Slots oficiais:

- Head
- Necklace
- Backpack Visual
- Armor
- Right Hand
- Left Hand
- Legs
- Boots
- Ring 1
- Ring 2
- Ammo
- Pet

---

# DEC-0064

## Título

Sistema de mãos.

## Origem

ES-002 — EquipmentService

## Status

LOCKED

## Decisão

Armas de uma mão:

Left Hand

Shield:

Right Hand

Spellbook:

Right Hand

Two Handed Weapon:

Left Hand

Bloqueia:

Right Hand

---

# DEC-0065

## Título

Requisitos de equipamento.

## Origem

ES-002 — EquipmentService

## Status

LOCKED

## Decisão

Equipamentos podem possuir requisitos:

- Level
- Vocação
- Ascension

Arquitetura deverá permitir novos requisitos futuros.

---

# DEC-0066

## Título

Itens exclusivos por vocação.

## Origem

ES-002 — EquipmentService

## Status

LOCKED

## Decisão

Equipamentos podem ser exclusivos de determinadas vocações.

---

# DEC-0067

## Título

Sistema de Upgrade.

## Origem

ES-002 — EquipmentService

## Status

LOCKED

## Decisão

Apenas itens Rare recebem Upgrade.

Limite:

+14

Progressão:

+0 até +9 possui dificuldade menor.

Acima de +9 existe sistema avançado.

---

# DEC-0068

## Título

Craft Affixes.

## Origem

ES-002 — EquipmentService

## Status

LOCKED

## Decisão

Upgrades acima de +9 possuem chance de desbloquear Craft Affixes exclusivos.

Características:

- Linha separada dos afixos normais;
- Possuem raridade própria;
- Máximo inicial de 2 Craft Affixes por item.

---

# DEC-0069

## Título

Tier influencia Afixos.

## Origem

ES-002 — EquipmentService

## Status

LOCKED

## Decisão

Tier influencia:

- atributo base;
- possibilidade de geração de afixos;
- qualidade dos afixos disponíveis.

---

# DEC-0070

## Título

Slots de Imbuement.

## Origem

ES-002 — EquipmentService

## Status

LOCKED

## Decisão

Slots de Imbuement fazem parte da identidade do Item Instance.

Quando o item nasce com slots:

Eles são permanentes.

---

# DEC-0071

## Título

Duração de Imbuement.

## Origem

ES-002 — EquipmentService

## Status

LOCKED

## Decisão

Ao finalizar a duração do Imbuement:

O slot permanece disponível.

O jogador pode aplicar novo Imbuement.

---

# DEC-0072

## Título

Impacto do equipamento.

## Origem

ES-002 — EquipmentService

## Status

LOCKED

## Decisão

Equipamentos representam aproximadamente 50% do poder total do personagem.

---

# DEC-0073

## Título

Arquitetura EquipmentComponent.

## Origem

ES-002 — EquipmentService

## Status

LOCKED

## Decisão

Character não armazena equipamentos diretamente.

Arquitetura:

Character

↓

EquipmentComponent

↓

EquipmentSlot

↓

ItemInstance
---

# DEC-0074

## Título

Filosofia do Character.

## Origem

CS-002 — CharacterService

## Status

LOCKED

## Decisão

No Project-Victory, um personagem representa identidade e poder.

O personagem representa a história construída pelo jogador.

---

# DEC-0075

## Título

Criação inicial do personagem.

## Origem

CS-002 — CharacterService

## Status

LOCKED

## Decisão

Na criação do personagem:

Jogador define:

- Nome;
- Sexo;
- Vocação.

A aparência inicial utiliza preset padrão.

---

# DEC-0076

## Título

Vocação definitiva.

## Origem

CS-002 — CharacterService

## Status

LOCKED

## Decisão

A vocação é escolhida na criação.

A mudança de vocação poderá existir futuramente apenas uma vez por personagem com custo elevado.

---

# DEC-0077

## Título

Vocação define identidade.

## Origem

CS-002 — CharacterService

## Status

LOCKED

## Decisão

A vocação define:

- HP;
- MP;
- Ganhos por nível;
- Skills principais;
- Equipamentos permitidos;
- Estilo de combate.

---

# DEC-0078

## Título

Ascension System.

## Origem

CS-002 — CharacterService

## Status

LOCKED

## Decisão

Ao atingir Level 500:

Personagem realiza Ascension.

Fluxo:

Level 500

↓

NPC Rei

↓

Ascension

↓

Level retorna para 1

↓

Atributos permanecem

↓

Novas regiões desbloqueadas

---

# DEC-0079

## Título

Progressão Ascension.

## Origem

CS-002 — CharacterService

## Status

LOCKED

## Decisão

Existem 5 Ascensions.

Após completar Ascension 5 e alcançar Level 500:

Personagem se torna Legend.

Legend possui progressão sem limite de Level.

---

# DEC-0080

## Título

Training System.

## Origem

CS-002 — CharacterService

## Status

LOCKED

## Decisão

Existe Training Offline.

Regras de acesso serão definidas futuramente.

Podem existir diferenças entre:

- Free Account;
- Premium Account;
- Sistemas adicionais.

---

# DEC-0081

## Título

Identidade Pública.

## Origem

CS-002 — CharacterService

## Status

LOCKED

## Decisão

O personagem possui histórico completo.

Informações públicas possuem controle de exposição.

Nem todos os dados do personagem são automaticamente visíveis.

---

# DEC-0082

## Título

Mount System.

## Origem

CS-002 — CharacterService

## Status

LOCKED

## Decisão

Mounts existirão futuramente.

Mounts não serão customizáveis.

Outfits possuem sistema separado de personalização.

---

# DEC-0083

## Título

Persistência do Character.

## Origem

CS-002 — CharacterService

## Status

LOCKED

## Decisão

O Character utiliza salvamento contínuo.

Eventos importantes devem persistir imediatamente.

---

# DEC-0084

## Título

Arquitetura Character.

## Origem

CS-002 — CharacterService

## Status

LOCKED

## Decisão

Character será baseado em componentes.

Arquitetura:

CharacterEntity

↓

IdentityComponent

ProgressionComponent

SkillComponent

InventoryComponent

EquipmentComponent

WalletComponent

AchievementComponent

GuildComponent

---

# DEC-0085

## Título

Filosofia da Guild.

## Origem

GS-001 — GuildService

## Status

LOCKED

## Decisão

No Project-Victory, uma Guild representa união e conquista.

Guilds são responsáveis por criar vínculos sociais e conquistas coletivas.

---

# DEC-0086

## Título

Criação de Guild.

## Origem

GS-001 — GuildService

## Status

LOCKED

## Decisão

Requisitos para criar Guild:

- Level mínimo 250;
- Ascension 1.

---

# DEC-0087

## Título

Participação em Guild.

## Origem

GS-001 — GuildService

## Status

LOCKED

## Decisão

Cada personagem pode participar de apenas uma Guild.

---

# DEC-0088

## Título

Hierarquia da Guild.

## Origem

GS-001 — GuildService

## Status

LOCKED

## Decisão

Estrutura:

Leader

↓

Vice Leader (máximo 5)

↓

Members

Quantidade de membros é definida pelo Guild Level.

---

# DEC-0089

## Título

Guild Progression.

## Origem

GS-001 — GuildService

## Status

LOCKED

## Decisão

Guild Level evolui através de:

- Doações;
- Quests;
- Bosses;
- Eventos;
- Region War.

---

# DEC-0090

## Título

Guild Donations.

## Origem

GS-001 — GuildService

## Status

LOCKED

## Decisão

Jogadores podem doar:

- Gold;
- Resources;
- Materiais;
- Itens.

---

# DEC-0091

## Título

Guild Benefits.

## Origem

GS-001 — GuildService

## Status

LOCKED

## Decisão

Guild Level desbloqueia:

- Mais membros;
- Loja;
- Bosses.

---

# DEC-0092

## Título

Guild Bank.

## Origem

GS-001 — GuildService

## Status

LOCKED

## Decisão

Guild possui banco completo de itens.

Capacidade aumenta conforme Guild Level.

---

# DEC-0093

## Título

Guild Application System.

## Origem

GS-001 — GuildService

## Status

LOCKED

## Decisão

Entrada em Guild ocorre através de aplicação.

Fluxo:

Jogador pesquisa Guild

↓

Visualiza perfil

↓

Envia aplicação

↓

Leader/Vice Leader analisam

↓

Aceitar ou recusar

---

# DEC-0094

## Título

Guild Territory System.

## Origem

GS-001 — GuildService

## Status

LOCKED

## Decisão

Guilds poderão controlar futuramente:

- Cidades;
- Regiões;
- Fortalezas.

---

# DEC-0095

## Título

Guild Member Limit.

## Origem

GS-001 — GuildService

## Status

LOCKED

## Decisão

Limite inicial:

10 membros.

O limite aumenta conforme Guild Level até um máximo futuro definido.

---

# DEC-0096

## Título

Guild Architecture.

## Origem

GS-001 — GuildService

## Status

LOCKED

## Decisão

Arquitetura:

GuildService

↓

GuildEntity

↓

GuildMemberComponent

↓

GuildBankComponent

↓

GuildLevelComponent

↓

GuildLogComponent

↓

GuildQuestComponent

↓

GuildWarComponent

---

# DEC-0097

## Título

Filosofia dos Achievements.

## Origem

AS-001 — AchievementService

## Status

LOCKED

## Decisão

No Project-Victory, um Achievement representa história do jogador.

Achievements registram momentos importantes da jornada.

---

# DEC-0098

## Título

Categorias de Achievement.

## Origem

AS-001 — AchievementService

## Status

LOCKED

## Decisão

Categorias oficiais:

- Progressão;
- Exploração;
- Combate;
- Economia;
- Coleção;
- Social.

---

# DEC-0099

## Título

Visibilidade de Achievements.

## Origem

AS-001 — AchievementService

## Status

LOCKED

## Decisão

O jogador controla quais Achievements serão exibidos publicamente.

---

# DEC-0100

## Título

Achievement Points.

## Origem

AS-001 — AchievementService

## Status

LOCKED

## Decisão

O sistema possui:

- Achievements;
- Achievement Points;
- Rankings.

---

# DEC-0101

## Título

Recompensas de Achievement.

## Origem

AS-001 — AchievementService

## Status

LOCKED

## Decisão

Achievements podem recompensar:

- Títulos;
- Cosméticos;
- Mounts;
- Itens;
- Gold.

---

# DEC-0102

## Título

Sistema de Títulos.

## Origem

AS-001 — AchievementService

## Status

LOCKED

## Decisão

Títulos podem ser apenas visuais ou possuir efeitos especiais futuramente.

---

# DEC-0103

## Título

Achievements únicos do servidor.

## Origem

AS-001 — AchievementService

## Status

LOCKED

## Decisão

Primeiros eventos históricos possuem registro único.

Exemplos:

- Primeiro Level 100;
- Primeiro Boss derrotado;
- Primeiro Unique Item obtido.

Esses registros existem uma única vez no servidor.

---

# DEC-0104

## Título

Achievements Secretos.

## Origem

AS-001 — AchievementService

## Status

LOCKED

## Decisão

O sistema suporta Achievements secretos.

O jogador pode descobrir conquistas ocultas.

---

# DEC-0105

## Título

Persistência dos Achievements.

## Origem

AS-001 — AchievementService

## Status

LOCKED

## Decisão

Achievements nunca são resetados por Ascension.

Eles fazem parte da história permanente do personagem.

---

# DEC-0106

## Título

Collections separado de Achievement.

## Origem

AS-001 — AchievementService

## Status

LOCKED

## Decisão

Collections e Achievements são sistemas independentes.

---

# DEC-0107

## Título

Exposição pública do personagem.

## Origem

AS-001 — AchievementService

## Status

LOCKED

## Decisão

Informações públicas:

- Level;
- Ascension;
- Títulos;
- Guild.

Informações configuráveis:

- Achievements;
- Equipamentos.

---

# DEC-0108

## Título

Arquitetura AchievementService.

## Origem

AS-001 — AchievementService

## Status

LOCKED

## Decisão

Arquitetura:

AchievementService

↓

AchievementEntity

↓

AchievementProgress

↓

RewardService

↓

TitleService
---

# DEC-0109

## Título

Filosofia das Collections.

## Origem

COL-001 — CollectionService

## Status

LOCKED

## Decisão

No Project-Victory, uma Collection representa diversão e opção de diversão.

Collections são sistemas opcionais de objetivos paralelos.

---

# DEC-0110

## Título

Collection como sistema de progressão.

## Origem

COL-001 — CollectionService

## Status

LOCKED

## Decisão

Collections são sistemas completos de progressão.

Podem possuir:

- objetivos;
- progresso;
- recompensas.

---

# DEC-0111

## Título

Categorias iniciais de Collection.

## Origem

COL-001 — CollectionService

## Status

LOCKED

## Decisão

Categorias oficiais:

- Monstros;
- Itens;
- Equipamentos;
- Recursos;
- Bosses.

---

# DEC-0112

## Título

Collection de Monstros.

## Origem

COL-001 — CollectionService

## Status

LOCKED

## Decisão

Collections de monstros são completadas automaticamente.

O jogador progride ao derrotar criaturas.

---

# DEC-0113

## Título

Collection de Entrega.

## Origem

COL-001 — CollectionService

## Status

LOCKED

## Decisão

Collections que solicitam itens ou recursos exigem entrega.

Os itens entregues são destruídos como oferenda.

---

# DEC-0114

## Título

Raridade em Collections.

## Origem

COL-001 — CollectionService

## Status

LOCKED

## Decisão

Itens com raridades diferentes contam para a mesma Collection base.

Exemplo:

Fire Sword Rare

conta como:

Fire Sword.

---

# DEC-0115

## Título

Recompensas de Collection.

## Origem

COL-001 — CollectionService

## Status

LOCKED

## Decisão

Collections podem recompensar:

- Gold;
- XP;
- Títulos;
- Cosméticos;
- Bônus temporários;
- Atributos permanentes.

---

# DEC-0116

## Título

Collections únicas do servidor.

## Origem

COL-001 — CollectionService

## Status

LOCKED

## Decisão

Podem existir Collections únicas.

Exemplo:

Primeiro jogador a completar determinada Collection.

---

# DEC-0117

## Título

Visibilidade das Collections.

## Origem

COL-001 — CollectionService

## Status

LOCKED

## Decisão

O jogador controla a exposição pública das Collections.

---

# DEC-0118

## Título

Arquitetura CollectionService.

## Origem

COL-001 — CollectionService

## Status

LOCKED

## Decisão

Arquitetura:

CollectionService

↓

CollectionEntity

↓

CollectionProgress

↓

RewardService
---

# DEC-0119

## Título

Filosofia das Quests.

## Origem

QST-001 — QuestService

## Status

LOCKED

## Decisão

No Project-Victory, uma Quest representa esforço e interesse.

Quest é um sistema complementar.

A principal fonte de progressão continua sendo Hunt.

---

# DEC-0120

## Título

Importância das Quests.

## Origem

QST-001 — QuestService

## Status

LOCKED

## Decisão

Quests não são o centro do jogo.

Responsabilidades:

- apresentar sistemas;
- criar objetivos paralelos;
- recompensar exploração;
- liberar conteúdos.

---

# DEC-0121

## Título

Tipos de Quest.

## Origem

QST-001 — QuestService

## Status

LOCKED

## Decisão

Tipos oficiais:

- História;
- Caça;
- Coleta;
- Evento.

---

# DEC-0122

## Título

Quest Principal.

## Origem

QST-001 — QuestService

## Status

LOCKED

## Decisão

Existe uma linha inicial simples.

Objetivo:

Apresentar:

- cidade;
- primeira hunt;
- primeiro loot;
- sistemas básicos.

---

# DEC-0123

## Título

Obrigatoriedade de Quest.

## Origem

QST-001 — QuestService

## Status

LOCKED

## Decisão

Somente progressões principais podem exigir Quest.

Quests comuns são opcionais.

---

# DEC-0124

## Título

Repetição de Quest.

## Origem

QST-001 — QuestService

## Status

LOCKED

## Decisão

Repetição depende do tipo da Quest.

Exemplos:

História:
uma vez.

Caça:
repetível.

Evento:
definido pelo evento.

---

# DEC-0125

## Título

Daily Quest.

## Origem

QST-001 — QuestService

## Status

LOCKED

## Decisão

Daily Quest não faz parte da versão inicial.

Motivo:

Evitar obrigação diária.

---

# DEC-0126

## Título

Quest Tracker.

## Origem

QST-001 — QuestService

## Status

LOCKED

## Decisão

O jogador visualiza objetivos ativos.

O sistema não revela todo conteúdo existente.

---

# DEC-0127

## Título

Entrega de Quests.

## Origem

QST-001 — QuestService

## Status

LOCKED

## Decisão

Quests podem ser iniciadas através de:

- NPCs;
- sistemas automáticos.

---

# DEC-0128

## Título

Escolhas narrativas.

## Origem

QST-001 — QuestService

## Status

LOCKED

## Decisão

Sistema de escolhas narrativas não faz parte da versão inicial.

---

# DEC-0129

## Título

Recompensas de Quest.

## Origem

QST-001 — QuestService

## Status

LOCKED

## Decisão

Quests podem recompensar:

- EXP;
- Gold;
- Items;
- Acesso a conteúdo;
- Títulos.

---

# DEC-0130

## Título

Quest de Ascension.

## Origem

QST-001 — QuestService

## Status

LOCKED

## Decisão

Existem quests relacionadas a Ascension.

Objetivo:

liberar progressões importantes.

---

# DEC-0131

## Título

Quest de Boss.

## Origem

QST-001 — QuestService

## Status

LOCKED

## Decisão

Bosses podem possuir quests relacionadas.

---

# DEC-0132

## Título

Quest Global.

## Origem

QST-001 — QuestService

## Status

LOCKED

## Decisão

Eventos globais podem utilizar sistema de Quest Global.

---

# DEC-0133

## Título

Falha de Quest.

## Origem

QST-001 — QuestService

## Status

LOCKED

## Decisão

Quests inicialmente não possuem falha permanente.

---

# DEC-0134

## Título

Histórico de Quest.

## Origem

QST-001 — QuestService

## Status

LOCKED

## Decisão

Personagem registra:

- Quests completas;
- Primeira conclusão.

---

# DEC-0135

## Título

Arquitetura QuestService.

## Origem

QST-001 — QuestService

## Status

LOCKED

## Decisão

Arquitetura:

QuestService

↓

QuestEntity

↓

QuestProgress

↓

RewardComponent
engineering/decision-log.md
---

# DEC-0146

## Título

Filosofia dos Monstros.

## Origem

MON-001 — MonsterService

## Status

LOCKED

## Decisão

No Project-Victory, um monstro representa desafio.

Monstros existem como fonte de:

- desafio;
- EXP;
- Loot;
- progressão.

---

# DEC-0147

## Título

Estrutura de Packs.

## Origem

MON-001 — MonsterService

## Status

LOCKED

## Decisão

Hunts utilizam sistema de Packs.

Configuração inicial:

3 monstros por Pack.

A arquitetura permite alteração futura.

---

# DEC-0148

## Título

Monster Individual.

## Origem

MON-001 — MonsterService

## Status

LOCKED

## Decisão

Cada monstro dentro do Pack é uma entidade independente.

Cada um possui:

- HP próprio;
- dano próprio;
- morte própria;
- EXP própria;
- Loot próprio.

---

# DEC-0149

## Título

Monster Template e Monster Instance.

## Origem

MON-001 — MonsterService

## Status

LOCKED

## Decisão

Arquitetura utiliza separação:

MonsterTemplate:

Define o modelo do monstro.

MonsterInstance:

Representa a criatura criada durante a Hunt.

---

# DEC-0150

## Título

Sistema de Combate em Pack.

## Origem

MON-001 — MonsterService

## Status

LOCKED

## Decisão

Ataques:

Single Target:

Afeta um monstro.

Area:

Afeta todos os monstros do Pack.

---

# DEC-0151

## Título

Tipos de Monstro.

## Origem

MON-001 — MonsterService

## Status

LOCKED

## Decisão

Tipos oficiais:

- Normal;
- Magic;
- Elite;
- Boss;
- World Boss.

---

# DEC-0152

## Título

Sistema Elite.

## Origem

MON-001 — MonsterService

## Status

LOCKED

## Decisão

Elite possui chance configurável por Hunt.

Regra inicial:

Máximo de 1 Elite por Pack.

Exemplo permitido:

Dragon

Dragon

Dragon Elite

---

# DEC-0153

## Título

Boss System.

## Origem

MON-001 — MonsterService

## Status

LOCKED

## Decisão

Boss:

- não aparece em Hunt normal;
- possui acesso próprio;
- utiliza portal/chave específica.

---

# DEC-0154

## Título

World Boss.

## Origem

MON-001 — MonsterService

## Status

LOCKED

## Decisão

World Boss ocorre através de eventos do servidor.

---

# DEC-0155

## Título

Monster Attributes.

## Origem

MON-001 — MonsterService

## Status

LOCKED

## Decisão

Monstros possuem:

- HP;
- Damage;
- Defense;
- EXP;
- Loot Table;
- Resistências;
- Fraquezas.

---

# DEC-0156

## Título

Monster Level.

## Origem

MON-001 — MonsterService

## Status

LOCKED

## Decisão

Monstros não possuem Level tradicional.

Possuem:

- atributos internos;
- raridade;
- versões de Ascension.

---

# DEC-0157

## Título

Monster Skills.

## Origem

MON-001 — MonsterService

## Status

LOCKED

## Decisão

Somente Bosses possuem Skills especiais.

---

# DEC-0158

## Título

Monster AI.

## Origem

MON-001 — MonsterService

## Status

LOCKED

## Decisão

IA simplificada.

Fluxo:

Spawn

↓

Combate automático

↓

Morte

---

# DEC-0159

## Título

Loot independente.

## Origem

MON-001 — MonsterService

## Status

LOCKED

## Decisão

Cada MonsterInstance possui cálculo próprio de Loot.

Exemplo:

Pack:

Dragon

Dragon

Dragon Elite


Cada criatura executa:

Loot Calculation individual.

---

# DEC-0160

## Título

Arquitetura MonsterService.

## Origem

MON-001 — MonsterService

## Status

LOCKED

## Decisão

Arquitetura:

MonsterService

↓

MonsterTemplate

↓

MonsterInstance

↓

SpawnController

↓

LootTable

↓

CombatComponent
---

# DEC-0161

## Título

Filosofia do Combate.

## Origem

COM-001 — CombatService

## Status

LOCKED

## Decisão

No Project-Victory, um combate representa desafio.

O combate é baseado em:

- estratégia;
- evolução;
- equipamentos;
- builds;
- cálculos.

---

# DEC-0162

## Título

Modelo de Combate.

## Origem

COM-001 — CombatService

## Status

LOCKED

## Decisão

Combate utiliza sistema de turnos.

Fluxo:

Jogador ataca

↓

Monstros atacam

↓

Calcula dano

↓

Atualiza HP

↓

Próximo turno

---

# DEC-0163

## Título

Velocidade de Combate.

## Origem

COM-001 — CombatService

## Status

LOCKED

## Decisão

Não existem inicialmente:

- Attack Speed;
- Cast Speed.

Todos possuem turno fixo.

---

# DEC-0164

## Título

Prioridade do Turno.

## Origem

COM-001 — CombatService

## Status

LOCKED

## Decisão

Ordem:

1. Jogador;
2. Monstros.

---

# DEC-0165

## Título

Seleção de Alvo.

## Origem

COM-001 — CombatService

## Status

LOCKED

## Decisão

Ataques Single Target atingem o primeiro monstro da Pack List.

---

# DEC-0166

## Título

Troca de Alvo.

## Origem

COM-001 — CombatService

## Status

LOCKED

## Decisão

Ao morrer:

Novo alvo é selecionado instantaneamente.

---

# DEC-0167

## Título

Dano Físico.

## Origem

COM-001 — CombatService

## Status

LOCKED

## Decisão

Prioridade:

Weapon

↓

Skill

↓

Level

---

# DEC-0168

## Título

Dano Mágico.

## Origem

COM-001 — CombatService

## Status

LOCKED

## Decisão

Prioridade:

Level

↓

Magic Level

↓

Equipment

↓

Spell

---

# DEC-0169

## Título

Spell Upgrade.

## Origem

COM-001 — CombatService

## Status

LOCKED

## Decisão

Spells podem receber upgrades.

O jogador pode especializar uma Spell favorita.

---

# DEC-0170

## Título

Variação de Dano.

## Origem

COM-001 — CombatService

## Status

LOCKED

## Decisão

Todo ataque acerta.

Existe apenas variação entre dano mínimo e máximo.

---

# DEC-0171

## Título

Critical Damage.

## Origem

COM-001 — CombatService

## Status

LOCKED

## Decisão

Critical é calculado após defesa e resistência.

Fluxo:

Dano Final

↓

Critical

↓

Resultado

---

# DEC-0172

## Título

Elemental Calculation.

## Origem

COM-001 — CombatService

## Status

LOCKED

## Decisão

Fluxo:

Dano Calculado

↓

Resistência

↓

Fraqueza

↓

Critical

---

# DEC-0173

## Título

Defesas Elementais.

## Origem

COM-001 — CombatService

## Status

LOCKED

## Decisão

Cada elemento possui defesa própria.

Exemplos:

- Physical Defense;
- Fire Defense;
- Ice Defense;
- Death Defense.

---

# DEC-0174

## Título

Morte do Personagem.

## Origem

COM-001 — CombatService

## Status

LOCKED

## Decisão

Fluxo:

HP = 0

↓

Cidade Principal

↓

HP cheio

↓

MP cheio

---

# DEC-0175

## Título

Morte do Monstro.

## Origem

COM-001 — CombatService

## Status

LOCKED

## Decisão

Fluxo:

Monster Death

↓

EXP

↓

Loot

↓

Collection

↓

Achievement

---

# DEC-0176

## Título

Magias em Área.

## Origem

COM-001 — CombatService

## Status

LOCKED

## Decisão

Área depende da Spell.

Uma Spell pode atingir:

- todos os monstros do Pack;
- quantidade específica definida pela Spell.

---

# DEC-0177

## Título

Arquitetura CombatService.

## Origem

COM-001 — CombatService

## Status

LOCKED

## Decisão

Arquitetura:

CombatService

↓

DamageCalculator

↓

DefenseCalculator

↓

ResistanceCalculator

↓

CriticalCalculator
---

# DEC-0178

## Título

Filosofia das Skills.

## Origem

SKL-001 — SkillService

## Status

LOCKED

## Decisão

No Project-Victory, uma Skill representa poder.

Skills representam a evolução permanente das capacidades do personagem.

---

# DEC-0179

## Título

Skills oficiais.

## Origem

SKL-001 — SkillService

## Status

LOCKED

## Decisão

Skills oficiais:

- Sword;
- Club;
- Axe;
- Distance;
- Shielding;
- First Fighting;
- Magic Level.

Não existem outras Skills inicialmente.

---

# DEC-0180

## Título

Evolução das Skills.

## Origem

SKL-001 — SkillService

## Status

LOCKED

## Decisão

Skills evoluem através de:

- utilização durante gameplay;
- treinamento.

---

# DEC-0181

## Título

Magic Level.

## Origem

SKL-001 — SkillService

## Status

LOCKED

## Decisão

Magic Level evolui através de:

- uso de magias;
- treinamento.

---

# DEC-0182

## Título

Reset de Skill.

## Origem

SKL-001 — SkillService

## Status

LOCKED

## Decisão

Skills não possuem reset.

A evolução da Skill é permanente.

---

# DEC-0183

## Título

Spell Upgrade separado.

## Origem

SKL-001 — SkillService

## Status

LOCKED

## Decisão

Spell Upgrade é separado de Skill.

Spell Upgrade pode possuir reset futuramente.

Skill não.

---

# DEC-0184

## Título

Influência das Skills.

## Origem

SKL-001 — SkillService

## Status

LOCKED

## Decisão

Skills influenciam:

- dano;
- eficiência.

---

# DEC-0185

## Título

Limite das Skills.

## Origem

SKL-001 — SkillService

## Status

LOCKED

## Decisão

Skills não possuem limite máximo.

A progressão é livre.

O custo de evolução aumenta exponencialmente.

---

# DEC-0186

## Título

Arquitetura SkillService.

## Origem

SKL-001 — SkillService

## Status

LOCKED

## Decisão

Arquitetura:

SkillService

↓

SkillEntity

↓

SkillProgression

↓

CharacterComponent
---

# DEC-0187

## Título

Filosofia das Spells.

## Origem

SPELL-001 — SpellService

## Status

LOCKED

## Decisão

No Project-Victory, uma Spell representa força.

Spells representam a expressão do poder do personagem.

---

# DEC-0188

## Título

Tipos de Spell.

## Origem

SPELL-001 — SpellService

## Status

LOCKED

## Decisão

Tipos:

- Attack Spell;
- Area Spell;
- Support Spell;
- Instance Spell.

---

# DEC-0189

## Título

Instance Spell.

## Origem

SPELL-001 — SpellService

## Status

LOCKED

## Decisão

Instance Spell é uma postura permanente da vocação.

Características:

- não consome Mana;
- efeito permanente;
- exclusiva por vocação.

---

# DEC-0190

## Título

Aprendizado de Spell.

## Origem

SPELL-001 — SpellService

## Status

LOCKED

## Decisão

Spell é desbloqueada automaticamente ao atingir requisito de Level.

---

# DEC-0191

## Título

Requisitos de Spell.

## Origem

SPELL-001 — SpellService

## Status

LOCKED

## Decisão

Requisitos:

- Level;
- Vocação;
- Ascension.

---

# DEC-0192

## Título

Spell Upgrade.

## Origem

SPELL-001 — SpellService

## Status

LOCKED

## Decisão

Cada Spell possui upgrade individual.

Limite inicial:

10 níveis.

---

# DEC-0193

## Título

Spell Forge.

## Origem

SPELL-001 — SpellService

## Status

LOCKED

## Decisão

Spell Forge é o sistema responsável pelo upgrade de Spells.

---

# DEC-0194

## Título

Cooldown de Spell.

## Origem

SPELL-001 — SpellService

## Status

LOCKED

## Decisão

Cada Spell possui cooldown próprio verificado por turno.

---

# DEC-0195

## Título

Prioridade de Spell.

## Origem

SPELL-001 — SpellService

## Status

LOCKED

## Decisão

Jogador define prioridade de utilização.

Heal possui prioridade inicial.

---

# DEC-0196

## Título

Arquitetura SpellService.

## Origem

SPELL-001 — SpellService

## Status

LOCKED

## Decisão

Arquitetura:

SpellService

↓

SpellTemplate

↓

CharacterSpell

↓

SpellUpgrade

↓

DamageCalculator
---

# DEC-0197

## Título

Filosofia do Training.

## Origem

TRAIN-001 — TrainingService

## Status

LOCKED

## Decisão

No Project-Victory, um Training representa poder.

Training representa evolução passiva controlada.

---

# DEC-0198

## Título

Objetivo do Training.

## Origem

TRAIN-001 — TrainingService

## Status

LOCKED

## Decisão

Training permite evolução de:

- Skills;
- Magic Level.

Não gera:

- EXP;
- Loot;
- Gold.

---

# DEC-0199

## Título

Modelo Online e Offline.

## Origem

TRAIN-001 — TrainingService

## Status

LOCKED

## Decisão

Training possui o mesmo funcionamento online e offline.

Não existe vantagem por permanecer conectado.

---

# DEC-0200

## Título

Item de Training.

## Origem

TRAIN-001 — TrainingService

## Status

LOCKED

## Decisão

Cada treinamento utiliza item específico.

O item possui cargas.

As cargas determinam duração do treinamento.

---

# DEC-0201

## Título

Bloqueio durante Training.

## Origem

TRAIN-001 — TrainingService

## Status

LOCKED

## Decisão

Durante Training:

- personagem não caça;
- personagem não recebe EXP;
- personagem não participa de combate.

---

# DEC-0202

## Título

Training Progression.

## Origem

TRAIN-001 — TrainingService

## Status

LOCKED

## Decisão

Training é mais lento que Hunt.

Hunt continua sendo a principal fonte de evolução.

---

# DEC-0203

## Título

Training Session.

## Origem

TRAIN-001 — TrainingService

## Status

LOCKED

## Decisão

Arquitetura:

TrainingService

↓

TrainingSession

↓

SkillProgression

↓

Character

---

# DEC-0204

## Título

Training Charge System.

## Origem

TRAIN-001 — TrainingService

## Status

LOCKED

## Decisão

Carga do item controla duração do Training.

Ao finalizar as cargas:

Training é encerrado.

---

# DEC-0205

## Título

Filosofia da Account.

## Origem

ACC-001 — AccountService

## Status

LOCKED

## Decisão

No Project-Victory, uma Account representa identidade.

A Account é a camada permanente do jogador.

---

# DEC-0206

## Título

Slots de Personagem.

## Origem

ACC-001 — AccountService

## Status

LOCKED

## Decisão

Cada conta inicia com:

3 slots de personagem.

Pode expandir através de VC Points.

Limite máximo inicial:

10 personagens.

---

# DEC-0207

## Título

Separação Account e Character.

## Origem

ACC-001 — AccountService

## Status

LOCKED

## Decisão

Account e Character são entidades separadas.

Account representa jogador.

Character representa história dentro do mundo.

---

# DEC-0208

## Título

Wallet da Conta.

## Origem

ACC-001 — AccountService

## Status

LOCKED

## Decisão

Wallet pertence à Account.

Gold pertence à história econômica do jogador.

---

# DEC-0209

## Título

VC Points.

## Origem

ACC-001 — AccountService

## Status

LOCKED

## Decisão

VC Points pertencem à Account.

---

# DEC-0210

## Título

Exclusão de Conta.

## Origem

ACC-001 — AccountService

## Status

LOCKED

## Decisão

Conta não pode ser excluída inicialmente.

Personagens podem solicitar exclusão.

Prazo:

30 dias.

---

# DEC-0211

## Título

Arquitetura AccountService.

## Origem

ACC-001 — AccountService

## Status

LOCKED

## Decisão

Arquitetura:

AccountService

↓

AccountEntity

↓

CharacterEntity

↓

PersistenceLayer
---

# DEC-0212

## Título

Filosofia do Character.

## Origem

CHAR-001 — CharacterService

## Status

LOCKED

## Decisão

No Project-Victory, um personagem representa identidade.

O personagem representa a história permanente do jogador.

---

# DEC-0213

## Título

Vocação permanente.

## Origem

CHAR-001 — CharacterService

## Status

LOCKED

## Decisão

A vocação é escolhida na criação e não pode ser alterada inicialmente.

---

# DEC-0214

## Título

EXP do personagem.

## Origem

CHAR-001 — CharacterService

## Status

LOCKED

## Decisão

EXP pode ser obtida através de:

- Hunts;
- Quests;
- Eventos;
- Sistemas futuros.

Hunt permanece como principal fonte.

---

# DEC-0215

## Título

Sistema de atributos.

## Origem

CHAR-001 — CharacterService

## Status

LOCKED

## Decisão

Não existem atributos tradicionais.

O poder vem de:

- Skills;
- Equipamentos;
- Spells.

---

# DEC-0216

## Título

Penalidade de morte.

## Origem

CHAR-001 — CharacterService

## Status

LOCKED

## Decisão

Morte não remove:

- EXP;
- Gold;
- itens.

---

# DEC-0217

## Título

Persistência de Buff após morte.

## Origem

CHAR-001 — CharacterService

## Status

LOCKED

## Decisão

Buffs ativos permanecem após morte.

---

# DEC-0218

## Título

Arquitetura CharacterService.

## Origem

CHAR-001 — CharacterService

## Status

LOCKED

## Decisão

Arquitetura:

CharacterService

↓

CharacterEntity

↓

Components