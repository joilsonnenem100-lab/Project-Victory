# ADR-038 - Composable Loot Tables

Status: ACCEPTED

Version: 1.0

---

# Contexto

Era necessário evitar duplicação de Loot Tables para eventos e bônus.

---

# Decisão

Uma resolução de Loot poderá utilizar múltiplas Loot Tables independentes.

Todas serão compostas durante a execução.

---

# Consequências

Benefícios:

- reutilização;
- eventos independentes;
- baixo acoplamento;
- facilidade de balanceamento.

Custos:

- necessidade de um Loot Resolver.

---

# Aplicações

Aplica-se a:

- Eventos;
- Bosses;
- Guilds;
- Campanhas;
- Expansões futuras.

---

# Restrições

Eventos nunca alteram diretamente a Loot Table original.

---

# Referências

Ver também:

- developer-guide/domain/loot.md