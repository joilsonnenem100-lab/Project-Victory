# Level

Status: DRAFT

Version: 1.0

---

# Objetivo

Definir o contrato arquitetural do módulo Level.

---

# Responsabilidade

O módulo Level é responsável exclusivamente por representar o nível efetivo atual do Character.

Level é um estado derivado.

Ele nunca representa a fonte oficial da progressão.

---

# Responsabilidades

O módulo Level é responsável por:

- representar o nível atual;
- representar a progressão visual do Character;
- disponibilizar informações para Interface;
- fornecer uma abstração amigável da Experience.

---

# Dependências

O módulo Level poderá depender apenas de:

- Experience;
- Experience Table;
- Ascension.

---

# Proibições

O módulo Level não poderá:

- armazenar Experience;
- alterar Experience;
- calcular curvas de progressão;
- conhecer Reward Pipeline;
- conhecer Combat;
- conhecer Skills;
- conhecer Attributes.

---

# Definição

Level é um Value Object.

Seu valor é determinado exclusivamente por:

Experience

↓

Experience Table

↓

Current Level

---

# Persistência

Level não representa a fonte oficial da progressão.

A arquitetura permite sua reconstrução integral a qualquer momento.

Caso seja armazenado para otimização, será considerado apenas um cache.

---

# Interface

A Interface apresenta:

- Ascension atual;
- Current Level.

Experience absoluta permanece um dado interno da Engine.

---

# Filosofia

Experience representa a verdade.

Experience Table interpreta.

Level representa o resultado.

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

- character.md
- ascension.md
- experience.md
- experience-table.md