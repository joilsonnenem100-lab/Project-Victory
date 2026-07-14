# Spawn System

Status: DRAFT

Version: 1.0

---

# Objetivo

Definir o contrato arquitetural do módulo Spawn System.

---

# Responsabilidade

O módulo Spawn System é responsável exclusivamente por criar novos packs de Monsters durante uma Hunt.

A criação das instâncias pertence ao Monster Spawn Service.

---

# Responsabilidades

O módulo Spawn System é responsável por:

- iniciar o Spawn de um novo pack;
- respeitar o Spawn Delay;
- consultar a Monster Pool da Hunt;
- solicitar a criação das instâncias;
- entregar o novo pack para a Combat Engine.

---

# Dependências

O módulo Spawn System poderá depender apenas de:

- Hunt Configuration;
- Monster Pool;
- Monster Spawn Service;
- Combat Engine Events.

---

# Proibições

O módulo Spawn System não poderá:

- executar Combat;
- controlar Monster AI;
- calcular Loot;
- calcular Experience;
- conhecer UI;
- acessar banco de dados;
- persistir Packs.

---

# Regras

Cada Hunt possui exatamente uma Monster Pool.

Cada novo pack representa um novo sorteio.

A composição do pack é definida pela Hunt.

A quantidade de Monsters é configurável.

O Spawn Delay pertence à configuração da Hunt.

Todo pack cria novas instâncias de Monster.

No máximo um Elite poderá existir por pack.

O Spawn ocorre somente após o encerramento completo do combate do pack anterior.

Ao encerrar uma Hunt, todos os Monsters do pack atual são descartados.

O Spawn System nunca mantém histórico de packs.

---

# Filosofia

O Spawn System cria oportunidades de combate.

Não cria conteúdo persistente.

Toda configuração pertence à Hunt.

Toda criação de Monsters pertence ao Monster Spawn Service.

O Spawn System permanece desacoplado dos demais módulos da Engine.

---

# Fluxo

Hunt

↓

Spawn System

↓

Monster Pool

↓

Monster Spawn Service

↓

Monster Factory

↓

Monster Instances

↓

Combat Engine

---

# Referências

Ver também:

- monsters.md
- monster-ai.md
- combat.md
- hunt.md