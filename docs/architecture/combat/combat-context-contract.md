# Combat Context Contract

## Objetivo

Definir o contrato arquitetural do contexto de combate associado à Hunt.

Este documento estabelece as responsabilidades, limites e regras iniciais do modelo Combat Context utilizado pelo Vertical Slice.

---

# Responsabilidade

O Combat Context representa o ciclo de vida de um combate associado a uma Hunt ativa.

Ele existe para permitir que uma Hunt mantenha um contexto válido de combate durante seu processamento futuro.

O Combat Context não representa:

- dano;
- cálculo de ataque;
- resultado de batalha;
- habilidades;
- balanceamento.

Essas responsabilidades pertencem a fluxos futuros.

---

# Localização Arquitetural

O Combat Context pertence ao contexto:
