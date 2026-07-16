# PV-VS-022 — Technical Contract

## Nome

Finalizar Ciclo da Hunt

---

# Objetivo Técnico

Implementar a capacidade de finalizar completamente uma Hunt após a resolução do resultado.

A finalização deve consolidar o ciclo de execução da Hunt, garantindo que ela não permaneça disponível para nova execução e que seu estado final permaneça consistente para os fluxos posteriores.

---

# Escopo Autorizado

Implementar exclusivamente:

- finalizar uma Hunt com resultado já resolvido;
- validar se a Hunt pode ser finalizada;
- consolidar o estado final;
- impedir nova execução após finalização;
- manter consistência do Character.

---

# Responsabilidade Arquitetural

O fluxo de finalização pertence à camada Application.

Local esperado:
