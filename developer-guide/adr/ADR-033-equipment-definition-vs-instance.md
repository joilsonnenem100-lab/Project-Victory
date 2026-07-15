# ADR-033 - Equipment Definition vs Equipment Instance

Status: ACCEPTED

Version: 1.0

---

# Contexto

Era necessário separar a definição de um equipamento de sua representação pertencente ao jogador.

---

# Decisão

Equipment passa a ser dividido em:

- Equipment Definition;
- Equipment Instance.

Definitions descrevem.

Instances pertencem ao Character.

---

# Consequências

Benefícios:

- escalabilidade;
- economia saudável;
- reutilização;
- suporte a encantamentos;
- suporte a personalizações;
- suporte a itens exclusivos.

Custos:

- necessidade de gerenciamento das Instances.

---

# Aplicações

Este princípio aplica-se a:

- Equipment;
- Inventory;
- Loot;
- Trade;
- Marketplace.

---

# Restrições

Definitions nunca poderão armazenar informações específicas de um jogador.

---

# Referências

Ver também:

- developer-guide/domain/equipment.md