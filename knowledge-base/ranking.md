# Ranking System

Version: 1.0

Status: LOCKED

Last Sprint: SPRINT-015

---

# Objetivo

Definir o funcionamento oficial do sistema de Rankings do Project-Victory.

---

# Filosofia

No Project-Victory, um Ranking representa reconhecimento e história.

Ranking mostra jogadores que construíram uma jornada relevante dentro do servidor.

---

# Princípios

O Ranking deve:

- ser simples;
- possuir valor social;
- mostrar feitos importantes.

Não deve:

- criar estatísticas sem significado;
- expor informações irrelevantes;
- incentivar competição artificial.

---

# Rankings Oficiais

## 1. Ranking de EXP

Principal ranking de progressão.

Critério:

EXP acumulada.

Informações:

- Nome;
- EXP Total;
- Vocação;
- Outfit;
- Ascension;
- Level.

---

## 2. Ranking de Achievement

Critério:

Achievement Points.

Representa:

- conquistas;
- feitos;
- história.

---

## 3. Ranking de Collection

Critério:

Collection Progress.

Representa:

- dedicação;
- objetivos opcionais;
- exploração.

---

## 4. Ranking de Guild

Critério:

- Guild Level;
- Conquistas da Guild.

Representa:

- união;
- progresso coletivo.

---

# Rankings Removidos

## Riqueza

Não existe.

Motivo:

Valor dos itens depende do Market.

---

## Equipamentos

Não existe.

Motivo:

Equipamentos são conquistas individuais.

---

## Seasons

Não existe.

O servidor possui progressão permanente.

---

# Atualização

Ranking não precisa recalcular todos os personagens constantemente.

Arquitetura:

```
Evento

↓

RankingService

↓

Atualização Cache
```

---

# Histórico

O servidor registra eventos históricos:

Exemplos:

- Primeiro Level 100;
- Primeiro Level 500;
- Primeiro Legend;
- Primeiros feitos únicos.

---

# Privacidade

Informações públicas:

- EXP;
- Level;
- Ascension;
- Guild;
- Títulos selecionados.

Informações opcionais:

- Achievements;
- Equipamentos.

---

# Arquitetura

```
RankingService

↓

RankingCache

↓

RankingEntry

↓

Event Updates
```

---

# Pendências futuras

Definir:

- Interface do Ranking;
- Posição máxima exibida;
- Atualização visual;
- Regras de desempate.