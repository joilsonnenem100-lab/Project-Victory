# ADR-003 - MonsterDefinition Única por Espécie

Status: ACCEPTED

Version: 1.0

---

# Contexto

Durante a definição do módulo Monsters foi necessário estabelecer como representar diferentes níveis de dificuldade e variações de uma mesma criatura.

Criar múltiplas MonsterDefinitions para uma única espécie aumentaria significativamente a complexidade do domínio.

---

# Decisão

Cada espécie possuirá exatamente uma MonsterDefinition.

Toda variação de dificuldade, comportamento ou recompensa deverá ser aplicada através de modificadores durante a criação da instância.

MonsterDefinition representa exclusivamente a identidade permanente da espécie.

---

# Consequências

Benefícios:

- elimina duplicação de definições;
- centraliza o balanceamento;
- reduz manutenção;
- facilita evolução da Engine;
- simplifica documentação.

Custos:

- necessidade de um pipeline de modificadores.

---

# Aplicações

Este princípio aplica-se a todas as espécies do jogo.

Exemplos:

- Rat;
- Spider;
- Poison Spider;
- Dragon;
- Demon.

Elite, Champion, Dungeon e Event nunca representam novas MonsterDefinitions.

---

# Restrições

Uma nova MonsterDefinition somente poderá ser criada para representar uma nova espécie.

Variações de dificuldade, região ou eventos nunca deverão originar novas MonsterDefinitions.

---

# Referências

Ver também:

- developer-guide/domain/monsters.md
- developer-guide/adr/ADR-001-engine-composition.md