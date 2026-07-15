# ADR-040 - Resource Transformation Model

Status: ACCEPTED

Version: 1.0

---

# Contexto

Era necessário definir um modelo uniforme para transformação de Resources.

---

# Decisão

Craft transforma Resources existentes em um único Resource previamente definido.

---

# Consequências

Benefícios:

- simplicidade;
- baixo acoplamento;
- reutilização;
- balanceamento declarativo.

Custos:

- cada Resource produzido possui uma única Recipe oficial.

---

# Aplicações

Aplica-se a todas as Craft Recipes.

---

# Restrições

- Um único Output por Recipe.
- Output deve referenciar uma Resource Definition existente.
- Não existem Recipes reversas.

---

# Referências

Ver também:

- developer-guide/domain/craft.md