# PV-VS-023 — Technical Contract

## Nome

Iniciar Combate da Hunt

---

# Objetivo Técnico

Implementar a capacidade de iniciar um contexto de combate associado a uma Hunt ativa.

O combate deve possuir uma representação inicial válida e permanecer associado à Hunt durante seu ciclo de execução.

Este contrato não implementa processamento de dano, cálculo de resultado ou resolução de combate.

---

# Escopo Autorizado

Implementar exclusivamente:

- iniciar contexto de combate;
- validar se a Hunt permite início de combate;
- criar associação entre Combat e Hunt;
- definir estado inicial válido do combate;
- impedir criação de combate para Hunts inválidas.

---

# Responsabilidade Arquitetural

O fluxo de criação do contexto de combate pertence à camada Application.

Local esperado:
