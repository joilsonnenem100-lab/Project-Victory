# Achievement System

Version: 1.0

Status: LOCKED

Last Sprint: SPRINT-012

---

# Objetivo

Definir o funcionamento oficial do sistema de Achievements do Project-Victory.

Achievements registram momentos importantes da história do jogador.

---

# Filosofia

No Project-Victory, um Achievement representa conquista.

Achievements representam:

- história;
- dedicação;
- feitos únicos;
- reconhecimento.

---

# Arquitetura

Achievement não fica armazenado diretamente dentro do Character.

Estrutura:

```
AchievementService

↓

AchievementEntity

↓

AchievementProgress

↓

RewardService

↓

TitleService
```

---

# Categorias

O sistema possui categorias:

## Progressão

Exemplos:

- alcançar determinados Levels;
- realizar Ascension;
- tornar-se Legend.

---

## Exploração

Exemplos:

- descobrir regiões;
- completar Hunts;
- explorar conteúdo.

---

## Combate

Exemplos:

- derrotar Bosses;
- derrotar World Bosses;
- feitos de combate.

---

## Economia

Exemplos:

- acumular Gold;
- grandes negociações;
- feitos comerciais.

---

## Coleção

Exemplos:

- completar coleções;
- reunir itens específicos.

---

## Social

Exemplos:

- participação em Guild;
- eventos sociais;
- interação com jogadores.

---

# Achievement Points

O sistema possui:

- Achievements;
- Achievement Points;
- Ranking.

Achievement Points representam o progresso histórico do jogador.

---

# Recompensas

Achievements podem fornecer:

- Títulos;
- Cosméticos;
- Mounts;
- Itens;
- Gold.

---

# Sistema de Títulos

Títulos representam reconhecimento.

Exemplo:

```
Legendary Hunter
```

Regras:

- alguns títulos podem ser apenas visuais;
- alguns títulos especiais podem possuir efeitos futuros.

---

# Rankings

Existe ranking de Achievement.

Pode exibir:

- maiores Achievement Points;
- conquistas raras;
- feitos históricos.

---

# Achievements Únicos do Servidor

Algumas conquistas acontecem somente uma vez.

Exemplos:

- Primeiro jogador Level 100;
- Primeiro jogador a derrotar determinado Boss;
- Primeiro jogador a conseguir determinado Unique Item.

---

# Registro Histórico

Esses eventos são registrados permanentemente.

Exemplo:

```
Servidor criado

↓

Player Joilson

↓

Primeiro Unique Item obtido
```

Esse registro não pode ser substituído.

---

# Achievements Secretos

O sistema suporta Achievements ocultos.

Características:

- jogador não sabe previamente todos os requisitos;
- descoberta faz parte da experiência;
- incentiva exploração.

---

# Ascension

Achievements nunca são resetados.

Permanecem durante toda a vida do personagem.

Incluindo:

- Level;
- Ascension;
- Legend.

---

# Collections

Collections são um sistema separado.

Relacionamento:

```
Collections

≠

Achievements
```

Uma Collection pode gerar Achievement futuramente, mas possuem estruturas independentes.

---

# Visibilidade

Quando outro jogador visualiza um personagem:

Informações públicas:

- Level;
- Ascension;
- Títulos;
- Guild.

Informações configuráveis pelo jogador:

- Achievements;
- Equipamentos.

---

# Identidade do Jogador

Achievements fazem parte da identidade pública e histórica.

Um personagem veterano deve ser reconhecido através de sua jornada.

---

# Integrações

AchievementService comunica com:

- CharacterService;
- QuestService;
- CombatService;
- WorldService;
- EconomyService;
- GuildService;
- CollectionService.

---

# Pendências futuras

Definir:

- Lista inicial de Achievements;
- Valores de Achievement Points;
- Ranking visual;
- Recompensas específicas;
- Títulos disponíveis.