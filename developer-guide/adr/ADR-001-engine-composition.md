# ADR-001 - Engine Orientada por Composição

Status: ACCEPTED

Version: 1.0

---

# Contexto

O Project-Victory possui diversos sistemas que podem modificar o comportamento de uma entidade durante sua criação.

Criar uma definição específica para cada variação aumentaria significativamente a complexidade da Engine.

---

# Decisão

A Engine utilizará composição para representar variações de comportamento.

As definições representam apenas a identidade permanente de cada objeto.

Toda variação deverá ser aplicada através de modificadores durante a criação da instância.

---

# Consequências

Benefícios:

- reduz duplicação de dados;
- simplifica o balanceamento;
- reduz manutenção;
- facilita evolução da Engine;
- permite adicionar novos modificadores sem alterar definições existentes.

Custos:

- necessidade de um pipeline de modificadores.

---

# Aplicações

Este princípio aplica-se a:

- Monsters;
- Hunts;
- Eventos;
- Dungeons;
- Sistemas futuros baseados em modificadores.

---

# Restrições

Uma nova Definition somente deverá ser criada quando representar uma nova entidade do domínio.

Variações de comportamento nunca deverão gerar novas Definitions.

---

# Referências

Ver também:

- developer-guide/domain/monsters.md