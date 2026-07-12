# Character System

Version: 1.0

Status: LOCKED

Last Sprint: SPRINT-010

---

# Objetivo

Definir a estrutura oficial do personagem do Project-Victory.

O Character é a entidade principal do jogador.

Todos os sistemas importantes se conectam ao personagem.

---

# Filosofia

No Project-Victory, um personagem representa identidade e poder.

O personagem representa:

- história;
- evolução;
- escolhas;
- conquistas;
- reconhecimento.

---

# Arquitetura

O personagem não será uma classe única gigante.

Modelo:

```
CharacterEntity

↓

Components
```

---

# Componentes

## IdentityComponent

Responsável por:

- Nome;
- Sexo;
- Histórico;
- Informações públicas.

---

## ProgressionComponent

Responsável por:

- Level;
- Experience;
- Ascension.

---

## SkillComponent

Responsável por:

- Sword;
- Club;
- Axe;
- Distance;
- Magic Level;
- Shield;
- First Fighting.

---

## InventoryComponent

Responsável pelo inventário.

---

## EquipmentComponent

Responsável pelos equipamentos equipados.

---

## WalletComponent

Responsável pelas moedas.

---

## AchievementComponent

Responsável por:

- Achievements;
- Títulos;
- Reconhecimento.

---

## GuildComponent

Responsável pela integração com Guilds.

---

# Criação do Personagem

Ao criar um personagem:

Jogador define:

- Nome;
- Sexo;
- Vocação.

---

# Aparência Inicial

Todos os personagens iniciam com preset padrão.

Customização avançada será definida futuramente.

---

# Vocação

Vocação é escolhida no momento da criação.

Vocação inicial:

- Knight;
- Paladin;
- Druid;
- Sorcerer;
- Monk.

---

# Mudança de Vocação

Pode existir futuramente.

Regra:

- Apenas uma vez na vida do personagem;
- Custo extremamente elevado.

---

# Vocação Define

A vocação influencia:

- HP;
- MP;
- Ganho por nível;
- Skills principais;
- Equipamentos permitidos;
- Estilo de combate.

---

# Progressão

O personagem evolui através de:

- Monstros;
- Bosses;
- Quests;
- Eventos.

---

# Level

O Level influencia:

- HP;
- MP;
- Dano através das regras da vocação.

Não influencia:

- Capacidade;
- Regeneração permanente.

---

# Ascension

Sistema de progressão avançada.

Fluxo:

```
Level 500

↓

Rei da Cidade

↓

Ascension

↓

Level retorna para 1

↓

Atributos permanecem
```

---

# Ascensions

Existem:

```
Ascension 1

Ascension 2

Ascension 3

Ascension 4

Ascension 5
```

---

# Legend

Após:

```
Ascension 5

↓

Level 500
```

O personagem torna-se:

```
Legend
```

A partir deste ponto:

- Level não possui limite máximo;
- Progressão continua indefinidamente.

---

# Skills

Skills oficiais:

- Sword;
- Club;
- Axe;
- Distance;
- Magic Level;
- Shield;
- First Fighting.

---

# Training

Existe:

- Training Online;
- Training Offline.

Regras de acesso serão definidas futuramente.

Podem existir diferenças entre:

- Free Account;
- Premium Account;
- Sistemas especiais.

---

# Morte

Ao morrer:

Fluxo:

```
Character

↓

Cidade Principal

↓

HP cheio

↓

MP cheio
```

---

# Buffs após morte

Mantidos:

- Food;
- XP Boost;
- Imbuements;
- Buffs ativos.

---

# Identidade

O personagem possui histórico completo.

Exemplos:

- Primeiros feitos;
- Achievements;
- Títulos;
- Conquistas.

---

# Exposição Pública

Nem todas as informações serão exibidas para outros jogadores.

A visualização pública será controlada pelo sistema.

---

# Nome

Mudança de nome:

- Permitida;
- Possui custo crescente;
- Histórico protegido.

---

# Sexo

Mudança de sexo:

- Permitida uma vez por personagem.

---

# Mount

Sistema futuro.

Regras:

- Mounts não serão customizáveis;
- Outfits possuem sistema separado.

---

# Salvamento

Character utiliza salvamento contínuo.

Eventos importantes devem ser persistidos imediatamente.

---

# Integrações

CharacterService comunica com:

- InventoryService;
- EquipmentService;
- WalletService;
- SkillService;
- CombatService;
- GuildService;
- AchievementService.