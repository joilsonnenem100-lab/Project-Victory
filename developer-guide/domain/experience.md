# Experience

Status: DRAFT

Version: 1.0

---

# Objetivo

Definir o contrato arquitetural do módulo Experience.

---

# Responsabilidade

O módulo Experience é responsável exclusivamente por representar a experiência absoluta permanente do Character.

Experience representa a única fonte oficial da progressão do Character.

---

# Responsabilidades

O módulo Experience é responsável por:

- armazenar a Experience absoluta;
- representar a progressão permanente;
- fornecer a base para o cálculo do Level;
- servir como entrada para a Experience Table.

---

# Dependências

O módulo Experience poderá depender apenas de:

- Character.

---

# Proibições

O módulo Experience não poderá:

- calcular Level;
- conhecer Experience Table;
- conhecer Ascension;
- conhecer Attributes;
- conhecer Skills;
- executar Combat;
- distribuir Rewards.

---

# Definição

Experience representa um único contador absoluto.

Exemplo:

Experience = 18.452.334.921

Este valor nunca representa:

- Experience do Level;
- percentual do Level;
- progresso parcial.

Toda interpretação pertence à Experience Table.

---

# Progressão

Experience nunca diminui.

Experience nunca reinicia.

Experience nunca sofre penalidades.

Experience representa toda a evolução permanente do Character.

---

# Reward Pipeline

Toda Experience é aplicada exclusivamente pelo Reward Pipeline.

Nenhum outro módulo poderá modificar Experience diretamente.

---

# Filosofia

Experience é a única fonte de verdade da progressão.

Level representa apenas uma interpretação desse valor.

---

# Fluxo

Combat

↓

Reward Pipeline

↓

Experience

↓

Experience Table

↓

Level

---

# Referências

Ver também:

- character.md
- ascension.md
- experience-table.md
- level.md