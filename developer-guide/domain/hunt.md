# Hunt

Status: DRAFT

Version: 1.0

---

# Objetivo

Definir o contrato arquitetural do módulo Hunt.

---

# Responsabilidade

O módulo Hunt é responsável exclusivamente por orquestrar o ciclo de vida de uma Hunt.

A Hunt não executa combate, não cria Monsters e não distribui recompensas.

Sua responsabilidade é coordenar os módulos responsáveis durante toda a execução da Hunt.

---

# Responsabilidades

O módulo Hunt é responsável por:

- iniciar uma Hunt;
- criar a Hunt Session;
- controlar os estados da Hunt;
- coordenar o Spawn System;
- coordenar a Combat Engine;
- coordenar o encerramento da Hunt;
- destruir a Hunt Session.

---

# Dependências

O módulo Hunt poderá depender apenas de:

- Hunt Configuration;
- Hunt Session;
- Spawn System;
- Combat Engine;
- Reward Pipeline.

---

# Proibições

O módulo Hunt não poderá:

- executar combate;
- controlar Monster AI;
- distribuir Loot;
- distribuir Experience;
- calcular recompensas;
- conhecer UI;
- acessar banco de dados.

---

# Máquina de Estados

Toda Hunt percorre obrigatoriamente os seguintes estados:

Created

↓

Starting

↓

Active

↓

Ending

↓

Finished

Não existem estados intermediários.

---

# Hunt

A Hunt representa a definição permanente do conteúdo.

Ela define:

- Monster Pool;
- Pack Size;
- Spawn Delay;
- Elite Chance;
- requisitos;
- configurações futuras.

A Hunt é imutável.

---

# Hunt Session

A Hunt Session representa a execução temporária da Hunt.

Ela existe apenas durante uma Hunt ativa.

Ao término da Hunt, a Hunt Session é destruída.

---

# Regras

Cada Character poderá possuir apenas uma Hunt ativa.

Cada Hunt possui exatamente uma Hunt Session.

Toda Hunt inicia na Cidade Principal.

Toda Hunt termina na Cidade Principal.

Toda Hunt encerrada destrói sua Hunt Session.

Toda desconexão encerra imediatamente a Hunt.

Toda saída manual utiliza o mesmo pipeline de encerramento.

Toda morte utiliza o mesmo pipeline de encerramento.

O Re-entry Cooldown é aplicado ao término da Hunt quando aplicável.

---

# Fluxo Oficial

Cidade Principal

↓

Selecionar Hunt

↓

Criar Hunt Session

↓

Spawn Inicial

↓

Combat

↓

Reward Pipeline

↓

Novo Pack

↓

...

↓

Encerrar Hunt

↓

Destruir Hunt Session

↓

Persistência

↓

Re-entry Cooldown

↓

Cidade Principal

---

# Filosofia

A Hunt é a orquestradora do Gameplay.

Ela coordena os módulos da Gameplay Foundation sem assumir suas responsabilidades.

Todo módulo especializado permanece independente.

---

# Referências

Ver também:

- spawn-system.md
- monster-ai.md
- monsters.md
- combat.md
- experience-distribution.md