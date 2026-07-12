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