# Quest System

Version: 1.0

Status: LOCKED

Last Sprint: SPRINT-014

---

# Objetivo

Definir o funcionamento oficial do sistema de Quests do Project-Victory.

---

# Filosofia

No Project-Victory, uma Quest representa esforço e interesse.

Quest é um sistema complementar.

A progressão principal ocorre através de:

- Hunts;
- Loot;
- Evolução do personagem.

---

# Papel das Quests

Quests existem para:

- apresentar sistemas;
- criar objetivos;
- recompensar jogadores;
- liberar conteúdos.

---

# Tipos de Quest

## História

Usada para:

- introdução;
- mundo;
- Ascension.

---

## Caça

Exemplo:

```
Mate:

100 Orcs
```

---

## Coleta

Exemplo:

```
Entregar:

50 Dragon Leather
```

---

## Evento

Quests temporárias.

---

# Quest Principal

Existe uma pequena introdução.

Objetivo:

- apresentar cidade;
- primeira hunt;
- primeiro loot;
- sistemas básicos.

---

# Repetição

Depende da Quest.

Exemplos:

História:

uma vez.

Caça:

repetível.

Evento:

definido pelo evento.

---

# Daily Quest

Não existe inicialmente.

Motivo:

O jogador não deve sentir obrigação diária.

---

# Quest Tracker

Mostra:

- Quest ativa;
- Objetivo atual;
- Progresso.

Não mostra todo conteúdo existente.

---

# Recompensas

Pode fornecer:

- EXP;
- Gold;
- Items;
- Acesso;
- Títulos.

---

# Ascension Quest

Quests especiais podem participar da progressão de Ascension.

---

# Boss Quest

Bosses podem possuir objetivos relacionados.

---

# Quest Global

Eventos do servidor podem criar objetivos coletivos.

---

# Falha

Não existe falha permanente inicialmente.

---

# Histórico

O personagem registra:

- Quests completas;
- Primeira conclusão.

---

# Arquitetura

```
QuestService

↓

QuestEntity

↓

QuestProgress

↓

RewardComponent
```

---

# Pendências futuras

Definir:

- Interface;
- NPCs;
- Lista inicial;
- Recompensas;
- Quests de cada Ascension.