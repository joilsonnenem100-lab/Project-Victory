# PV-VS-007 — Combat Experience Layer

## Local recomendado

Project-Victory/Documentation/Contracts/VerticalSlice/


---

# Objetivo

Evoluir o Combat Loop validado no PV-VS-006 para uma experiência de combate mais completa, mantendo o escopo controlado do Vertical Slice.

O contrato deve preservar:

- Account;
- Character;
- World;
- Hunt;
- Encounter;
- Reward;
- Experience Progression.

---

# Estado atual validado

Fluxo existente:

Start Hunt

↓

Create Hunt

↓

Generate Encounter

↓

Attack

↓

Damage

↓

Monster Defeated

↓

Reward

↓

Experience Gain


---

# Escopo de Implementação

## 1. Combat State

Adicionar controle de estado do combate.

Estados:

Created

↓

Active

↓

Victory

↓

Defeat


Objetivos:

- impedir ataques em combate encerrado;
- evitar reward duplicada;
- manter consistência do Encounter.


A implementação deve utilizar a estrutura existente de estado temporário de combate.

Não criar sistema paralelo.


---

# 2. Enemy Turn

Adicionar resposta do inimigo após ação do jogador.


Novo fluxo:

Player Attack

↓

Damage aplicado no inimigo

↓

Validar estado do combate

↓

Enemy Turn

↓

Damage aplicado no jogador


---

# 3. Player Health During Combat

Adicionar suporte para atualização de HP do jogador durante combate.


O jogador deve receber dano dos inimigos.


O estado deve refletir:

- HP atual;
- HP máximo.


---

# 4. Combat Result Evolution

Evoluir o resultado de combate existente.


Resultado deve suportar:

## Victory

Quando:

- inimigo derrotado;
- reward processada;
- experiência aplicada.


## Defeat

Quando:

- jogador derrotado;
- combate encerrado.


---

# 5. Combat Client Presentation

Organizar apresentação da tela de combate.


Objetivo:

Ter:

- um painel único de combate;
- uma representação única do Encounter;
- uma área única de Reward.


Adicionar representação visual de HP:

Exemplo:

Health:

██████░░░░

15/20


A alteração visual deve permanecer somente na camada Client.


---

# Fora de Escopo

Não implementar:

- Item Drop;
- Inventory;
- Equipment;
- Affixes;
- Resistências;
- Damage Types;
- Combat Engine completo;
- PvP;
- Skills avançadas;
- Fórmulas avançadas de dano.


---

# Restrições Arquiteturais

- Não alterar sistemas validados sem necessidade.
- Não substituir EncounterStateStore.
- Não criar abstrações futuras sem uso no Vertical Slice.
- Preservar o fluxo atual de Hunt e Reward.
- Alterar somente arquivos necessários.


---

# Critérios de Aceitação

O contrato será considerado concluído quando:


## Combate

- combate possuir estados definidos;
- jogador não conseguir atacar combate encerrado;
- inimigo possuir turno de resposta;
- jogador receber dano.


## Resultado

- vitória finalizar corretamente;
- derrota finalizar corretamente;
- reward continuar funcionando.


## Interface

- tela de combate possuir apresentação consolidada;
- HP possuir representação visual;
- Reward aparecer corretamente.


---

# Entrega esperada da Engenharia

Retornar:

- arquivos alterados;
- código implementado;
- resultado de build;
- validação funcional.


---

# Dependências

- PV-VS-006 — Combat Loop validado.
- Estado atual do snapshot PV-VS-Combat-001.