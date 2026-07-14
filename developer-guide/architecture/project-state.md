# Project-Victory

# Project State

Versão do Documento: 1.0

Status: Oficial

---

# Objetivo

Este documento representa o estado atual oficial do desenvolvimento do Project-Victory.

Seu objetivo é permitir que qualquer nova conversa ou novo integrante compreenda imediatamente:

- o estágio atual do projeto;
- os módulos concluídos;
- os módulos em desenvolvimento;
- o próximo objetivo do Roadmap;
- as decisões arquiteturais já consolidadas.

Este documento deve ser atualizado ao final de cada Sprint.

---

# Fonte de Verdade

O Project State nunca substitui a documentação oficial.

Sua responsabilidade é apenas consolidar o estado atual do projeto.

A documentação oficial permanece sendo a única fonte de verdade.

---

# Sprint Atual

Nome:

Sprint de Consolidação para Transição de Conversa

Status:

Em andamento

Objetivo:

Transformar toda a documentação produzida em uma base suficiente para permitir a continuidade do projeto em uma nova conversa sem perda significativa de contexto.

---

# Estado do Roadmap

Fase Atual:

Sprint de Consolidação

Desenvolvimento da Engine:

PAUSADO

Motivo:

Consolidação da documentação.

Após a conclusão desta Sprint o Roadmap continuará normalmente.

---

# Módulos LOCKED

Foundation

- Aggregate Root
- Value Objects
- Entities
- Character

Character Domain

- Health
- Mana
- Experience
- Level
- Inventory
- Equipment
- Skills

Progression

- Skill Progression
- Trainer
- Offline Training

Item System

- Items
- Item Template
- Item Instance
- Item Factory
- Weapons
- Weapon Categories
- Affixes
- Imbuements

Loot

- Loot
- Loot Tables
- Loot Entries

Engine Foundation

- Vocations
- Resources
- Combat Architecture

---

# Módulos em Desenvolvimento

Nenhum.

O desenvolvimento encontra-se oficialmente pausado durante esta Sprint.

---

# Próximo Módulo

Após o encerramento desta Sprint:

Monsters

Nenhum outro módulo deverá ser iniciado antes de Monsters sem uma revisão arquitetural documentada.

---

# Revisões Arquiteturais Pendentes

No momento não existe nenhuma revisão arquitetural bloqueando o Roadmap.

Melhorias identificadas durante a Sprint serão incorporadas normalmente na documentação.

---

# Decisões Consolidadas

Até o momento encontram-se consolidados:

- Character como Aggregate Root.
- Skills permanentes.
- Progressão baseada em uso.
- Weapon Categories.
- Item Template separado de Item Instance.
- Affixes permanentes.
- Imbuements temporários.
- Loot independente de Character.
- Combat Engine desacoplada.
- Combat Context imutável.
- Combat Result como saída única.
- Combat Pipeline determinístico.
- Vocation como perfil de configuração.
- Resources desacoplados da Combat Engine.

---

# Regras do Projeto

Durante o desenvolvimento:

- Nunca assumir decisões de Game Design.
- Nunca assumir comportamento não documentado.
- Toda dúvida relacionada à experiência do jogador deverá ser transformada em questionário.
- Toda implementação deverá consultar o Roadmap.
- Toda implementação deverá consultar este documento.
- Toda implementação deverá consultar a documentação oficial correspondente ao módulo.

---

# Estado da Documentação

Situação atual:

Em consolidação.

Objetivo da Sprint:

Preparar o projeto para transição segura entre conversas.

---

# Critério para Encerramento desta Sprint

Esta Sprint será considerada concluída quando:

- toda a documentação estrutural estiver organizada;
- o índice da arquitetura estiver concluído;
- o glossário oficial existir;
- o log de decisões estiver consolidado;
- o prompt oficial de transição estiver finalizado.