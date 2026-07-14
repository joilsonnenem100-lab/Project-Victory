# Experience Table

Status: DRAFT

Version: 1.0

---

# Objetivo

Definir o contrato arquitetural do módulo Experience Table.

---

# Responsabilidade

Experience Table é responsável exclusivamente por interpretar a Experience absoluta do Character e determinar seu Level efetivo.

Ela nunca armazena Experience.

Ela nunca altera Experience.

---

# Responsabilidades

Experience Table é responsável por:

- interpretar Experience;
- resolver o Current Level;
- calcular o progresso do Level;
- calcular Experience restante para o próximo Level;
- fornecer dados para a interface.

---

# Dependências

Experience Table poderá depender apenas de:

- Experience;
- Ascension.

---

# Proibições

Experience Table não poderá:

- alterar Experience;
- conhecer Character;
- conhecer Combat;
- conhecer Reward Pipeline;
- conhecer Attributes;
- conhecer Skills.

---

# Estrutura

Cada Ascension possui exatamente uma Experience Table.

Cada Experience Table possui entradas explícitas para todos os Levels.

Cada entrada define:

- Level;
- Experience mínima;
- Experience máxima.

---

# Filosofia

Experience representa a verdade.

Experience Table representa a interpretação.

Level representa o resultado.

---

# Seleção

A Experience Table ativa é determinada exclusivamente pela Ascension atual.

A Engine nunca realiza seleção automática.

---

# Validação

Toda Experience Table deverá ser validada antes de ser disponibilizada para uso.

Validações mínimas:

- sem lacunas;
- sem intervalos sobrepostos;
- Experience crescente;
- níveis contínuos;
- último nível válido.

---

# Balanceamento

Toda curva de progressão pertence exclusivamente à Experience Table.

Balanceamentos nunca exigem alterações na Engine.

---

# Fluxo

Experience

↓

Current Ascension

↓

Experience Table

↓

Current Level

---

# Referências

Ver também:

- experience.md
- ascension.md
- level.md