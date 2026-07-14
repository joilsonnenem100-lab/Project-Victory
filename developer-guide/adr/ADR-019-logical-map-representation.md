# ADR-019 - Logical Map Representation

Status: ACCEPTED

Version: 1.0

---

# Contexto

O Project-Victory possui uma filosofia Idle First.

O gameplay não depende de movimentação em mapas nem de representação gráfica permanente.

Era necessário definir o papel arquitetural do módulo Maps.

---

# Decisão

Maps representam exclusivamente o contexto lógico dos ambientes do jogo.

A representação gráfica torna-se uma responsabilidade exclusiva da camada de apresentação.

A Engine permanece completamente independente da renderização.

---

# Consequências

Benefícios:

- baixo acoplamento;
- evolução independente da interface;
- facilidade para futuras versões gráficas;
- simplicidade arquitetural;
- alta testabilidade.

Custos:

- necessidade de uma camada de apresentação para futuras representações visuais.

---

# Aplicações

Este princípio aplica-se a:

- Maps;
- Hunts;
- Interface;
- Gameplay Foundation;
- World Foundation.

---

# Restrições

Maps nunca poderão:

- executar regras de gameplay;
- controlar combate;
- depender da interface.

---

# Referências

Ver também:

- developer-guide/domain/maps.md
- developer-guide/domain/hunt.md