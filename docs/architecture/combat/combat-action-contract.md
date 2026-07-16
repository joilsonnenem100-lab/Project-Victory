# Combat Action Contract

## Objetivo

Definir o contrato arquitetural para representar uma ação executada dentro de um `CombatContext`.

Este documento estabelece a responsabilidade, localização e limites do conceito `CombatAction` utilizado pelo fluxo de Combat.

---

# Responsabilidade

`CombatAction` representa uma intenção de processamento dentro de um combate ativo.

Ele descreve uma solicitação de ação que pode ser avaliada pelo fluxo de Combat.

O conceito não representa:

- dano;
- ataque específico;
- habilidade;
- magia;
- arma;
- equipamento;
- fórmula de combate;
- resultado final de batalha.

---

# Localização Arquitetural

O `CombatAction` pertence ao contexto:
