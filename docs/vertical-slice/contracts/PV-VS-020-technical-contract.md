# PV-VS-020 — Technical Contract

## Nome

Processar Execução da Hunt

---

# Objetivo Técnico

Implementar a capacidade de processar uma Hunt existente que esteja em estado `Running`, produzindo um resultado final de execução.

O processamento deve utilizar os contratos existentes e manter a separação entre:

- Hunt;
- Combat;
- Reward;
- Progression.

---

# Escopo Autorizado

Implementar exclusivamente:

- processamento de Hunt em estado `Running`;
- validação do estado permitido;
- execução do fluxo de processamento;
- geração de resultado da execução;
- transição correta do estado da Hunt.

---

# Responsabilidade Arquitetural

O processamento pertence à camada Application.

Local esperado:
