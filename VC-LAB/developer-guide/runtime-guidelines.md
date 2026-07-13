# Runtime Guidelines

Status: LOCKED

Version: 1.0

---

# Objetivo

Definir como a Runtime Engine do Project-Victory deve ser implementada.

Toda lógica de execução do jogo deve seguir obrigatoriamente estas regras.

---

# Filosofia

A Runtime representa a simulação do mundo.

Ela nunca representa persistência.

Ela nunca representa interface.

Ela nunca representa banco.

Ela existe apenas enquanto a simulação acontece.

---

# Estrutura

Runtime

↓

Game Loop

↓

Systems

↓

Components

↓

Events

↓

Snapshots (opcional)

↓

Destroy()

---

# Regra 1

Toda simulação ocorre em memória.

Nunca consultar o banco durante a execução da Hunt.

---

# Regra 2

Runtime Objects nunca são persistidos.

Persistimos apenas resultados.

---

# Regra 3

Components armazenam estado.

Nunca regras.

---

# Regra 4

Systems executam regras.

Nunca armazenam estado permanente.

---

# Regra 5

Systems comunicam-se por Events.

Nunca modificam diretamente Components de outros Systems.

---

# Regra 6

Toda atualização ocorre através do Game Loop.

Nunca utilizar timers espalhados pelo projeto.

---

# Regra 7

O Tick é o único relógio oficial da Runtime.

---

# Regra 8

Todo processamento deve ser determinístico.

Mesmo input.

Mesmo resultado.

---

# Regra 9

Factories criam Runtime Objects.

Nunca utilizar "new" diretamente em código de alto nível para criar objetos complexos da simulação.

---

# Regra 10

Runtime nunca depende da API.

---

# Regra 11

Runtime nunca depende da Interface.

---

# Regra 12

Runtime nunca depende da Persistence.

---

# Regra 13

Runtime nunca conhece Entity Framework.

---

# Regra 14

Todo Runtime Object possui ciclo de vida bem definido.

Create()

↓

Execute()

↓

Destroy()

---

# Regra 15

Toda Hunt deve poder ser destruída completamente sem deixar objetos órfãos.

---

# Regra 16

Toda comunicação externa ocorre através de Commands e Events.

---

# Regra 17

Runtime deve ser facilmente simulável em testes.

---

# Regra 18

Runtime deve permitir Replay.

Mesmo que inicialmente essa funcionalidade não seja implementada.

---

# Regra 19

Runtime deve permitir futura renderização visual.

A Engine nunca produz gráficos.

Produz apenas eventos e estados.

---

# Regra 20

Quando houver dúvida,

preservar a simplicidade da Engine.

Nunca adicionar lógica apenas por conveniência.