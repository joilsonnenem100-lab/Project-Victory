# ADR-046 - Hunt Context and Hunt Result

Status: ACCEPTED

Version: 1.0

---

# Contexto

Era necessário padronizar a entrada e a saída do módulo Hunt.

---

# Decisão

Toda execução utiliza:

- Hunt Context como entrada;
- Hunt Result como saída.

O Hunt Context fornece todas as informações necessárias para a execução.

O Hunt Result consolida os resultados produzidos pelos módulos participantes.

---

# Consequências

Benefícios:

- desacoplamento;
- previsibilidade;
- facilidade de evolução;
- reutilização do padrão Context → Orchestrator → Result.

---

# Restrições

O Hunt Result é um DTO.

Não implementa comportamento de domínio.

O Hunt Context é imutável durante a execução.