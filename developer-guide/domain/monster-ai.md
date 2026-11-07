# Monster AI

Status: DRAFT

> *Nota de superação (2026-11-07, `DEC-20261107-05`, PO-58 em `docs/product/PO-DECISOES-BASE.md`): a Monster AI atual (só ataque básico, ação escolhida por dados) é o comportamento **oficial desta fase**; o módulo está **LOCKED** (`DEC-20261107-06`). Cooldowns, pesos e condições abaixo valem para a fase com habilidades de monstro (R-050/R-043 suspensos).*

Version: 1.0

---

# Objetivo

Definir o contrato arquitetural do módulo Monster AI.

---

# Responsabilidade

O módulo Monster AI é responsável exclusivamente por selecionar a ação secundária que um Monster poderá executar durante seu turno.

A execução da ação pertence à Combat Engine.

---

# Responsabilidades

O módulo Monster AI é responsável por:

- avaliar o contexto atual do combate;
- selecionar uma ação secundária válida;
- respeitar cooldowns;
- respeitar condições de uso;
- aplicar pesos (Weights);
- retornar a Combat Action selecionada.

---

# Dependências

O módulo Monster AI poderá depender apenas de:

- Monster;
- Combat Context;
- Combat Target;
- Combat Action.

---

# Proibições

O módulo Monster AI não poderá:

- executar ações;
- calcular dano;
- controlar turnos;
- controlar cooldowns;
- conhecer UI;
- acessar banco de dados;
- conhecer infraestrutura;
- controlar Hunts.

---

# Regras

A IA é executada apenas durante o turno do Monster.

A IA nunca executa ações.

A IA apenas retorna uma Combat Action.

Toda decisão é baseada no estado atual do combate.

A IA é determinística.

Toda variação ocorre apenas através do sorteio ponderado das ações disponíveis.

Toda configuração pertence aos dados da espécie.

Não existem implementações específicas para Monsters individuais.

A IA nunca comunica com outros Monsters.

---

# Filosofia

A Monster AI segue os princípios de simplicidade do Project-Victory.

Existe apenas uma implementação de IA para toda a Engine.

O comportamento das espécies é definido exclusivamente por configuração.

O ataque básico não pertence à Monster AI.

Toda criatura executa obrigatoriamente um ataque básico durante seu turno.

A IA decide apenas a ação secundária.

---

# Referências

Ver também:

- combat.md
- monsters.md
- resources.md
- skills.md