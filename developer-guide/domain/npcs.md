# NPCs

Status: DRAFT

Version: 1.0

---

# Objetivo

Definir o contrato arquitetural do módulo NPCs.

---

# Responsabilidade

O módulo NPCs é responsável exclusivamente por representar pontos permanentes de interação entre o Character e os serviços do Project-Victory.

NPCs não executam regras de negócio.

---

# Responsabilidades

O módulo NPCs é responsável por:

- representar serviços do jogo;
- receber interações do Character;
- encaminhar solicitações para os Application Services;
- representar personagens permanentes do mundo.

---

# Dependências

O módulo NPCs poderá depender apenas de:

- Character;
- City;
- Application Services.

---

# Proibições

O módulo NPCs não poderá:

- executar Combat;
- controlar Spawn;
- controlar Monster AI;
- distribuir Experience;
- distribuir Loot;
- conhecer Combat Engine;
- implementar regras de negócio.

---

# Estrutura

Todo NPC pertence exatamente a uma City.

Todo NPC representa exatamente um serviço principal.

O mesmo serviço poderá ser utilizado por NPCs diferentes.

---

# Pipeline Oficial

Character

↓

Interagir com NPC

↓

Application Service

↓

Domain

↓

Persistence (quando necessário)

↓

Resposta

↓

Atualização da Interface

---

# Disponibilidade

NPCs permanentes permanecem disponíveis continuamente.

Não existem:

- horários;
- fechamento;
- indisponibilidade por tempo.

NPCs temporários poderão existir apenas em eventos específicos.

---

# Regras

Cada NPC representa um único serviço.

NPCs nunca executam regras de negócio.

NPCs apenas iniciam interações.

Toda lógica pertence aos serviços da aplicação e ao domínio.

Todo serviço permanente deverá possuir um NPC correspondente.

---

# Filosofia

NPCs representam o mundo.

Os serviços representam a Engine.

A arquitetura mantém completamente separadas a representação visual e a lógica de negócio.

---

# Referências

Ver também:

- cities.md
- hunt.md
- regions.md