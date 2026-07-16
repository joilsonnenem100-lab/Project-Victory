# PV-VS-021 — Technical Contract

## Nome

Resolver Resultado da Hunt

---

# Objetivo Técnico

Implementar a capacidade de interpretar o resultado final produzido pela execução de uma Hunt e direcionar o fluxo correspondente.

O processamento deve garantir que o resultado da execução seja resolvido corretamente antes das etapas posteriores.

---

# Escopo Autorizado

Implementar exclusivamente:

- receber resultado de execução da Hunt;
- identificar o tipo de resultado;
- atualizar o estado final da Hunt conforme resultado;
- encaminhar sucesso para Reward;
- manter resultados sem sucesso no fluxo correspondente.

---

# Responsabilidade Arquitetural

O processamento pertence à camada Application.

Local esperado:
