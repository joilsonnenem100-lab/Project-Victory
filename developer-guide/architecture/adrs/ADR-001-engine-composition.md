# ADR-001 - Engine Orientada por Composição

Status: ACCEPTED

---

# Contexto

Durante a definição da arquitetura do módulo Monsters foi identificado um padrão recorrente.

Grande parte das funcionalidades futuras do Project-Victory poderia ser implementada através de composição ao invés de duplicação de entidades.

Exemplos:

- Elite
- Eventos
- Dungeons
- Temporadas
- Boss Buffs
- Modificadores de Hunt

Criar novas definições para cada combinação aumentaria exponencialmente a complexidade do projeto.

---

# Problema

Representar cada variação como uma entidade própria produziria situações como:

Rat

Elite Rat

Elite Rat Halloween

Elite Rat Christmas

Dungeon Rat

Champion Rat

Boss Rat

Cada nova combinação aumentaria a quantidade de definições do jogo.

Isso elevaria o custo de:

- manutenção;
- balanceamento;
- testes;
- documentação.

---

# Decisão

O Project-Victory adota oficialmente o princípio de composição.

As definições representam apenas a identidade permanente do objeto.

Toda variação deverá ser aplicada através de modificadores.

Exemplo:

MonsterDefinition

↓

MonsterFactory

↓

Modifier Pipeline

↓

Monster Instance

---

# Consequências

Benefícios:

- menor quantidade de código;
- menor quantidade de dados;
- balanceamento centralizado;
- maior reutilização;
- menor custo de manutenção;
- maior escalabilidade.

Custos:

- necessidade de um pipeline de modificadores;
- necessidade de definir prioridade entre modificadores quando coexistirem.

Os benefícios superam amplamente os custos.

---

# Aplicações

Este princípio deverá ser utilizado sempre que possível.

Exemplos:

Monsters

Elite

Champion

Dungeon

Events

Hunts

Future Systems

---

# Restrições

Não utilizar composição quando a variação representar uma entidade realmente diferente do domínio.

Neste caso deverá existir uma nova Definition.

---

# Status

Esta decisão é considerada permanente.

Qualquer exceção deverá possuir ADR próprio.