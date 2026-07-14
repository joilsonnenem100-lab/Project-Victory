# Maps

Status: DRAFT

Version: 1.0

---

# Objetivo

Definir o contrato arquitetural do módulo Maps.

---

# Responsabilidade

O módulo Maps é responsável exclusivamente por representar o contexto lógico dos ambientes do Project-Victory.

Maps não executam gameplay.

Maps não possuem regras de negócio.

Maps são independentes da representação visual.

---

# Responsabilidades

O módulo Maps é responsável por:

- representar ambientes do mundo;
- fornecer identidade temática para as Hunts;
- servir como referência lógica para a execução da Hunt.

---

# Dependências

O módulo Maps poderá depender apenas de:

- Hunt.

---

# Proibições

O módulo Maps não poderá:

- executar Combat;
- controlar Spawn;
- controlar Monster AI;
- distribuir Loot;
- distribuir Experience;
- controlar Progressão;
- conhecer Combat Engine.

---

# Definição

Um Map representa exclusivamente o ambiente lógico de uma Hunt.

A representação gráfica é opcional.

O domínio nunca depende da renderização.

---

# Regras

Cada Hunt referencia exatamente um Map.

Um mesmo Map poderá ser reutilizado por diversas Hunts.

Maps representam conteúdo permanente.

Maps não possuem estado de execução.

Toda alteração temporária pertence à Hunt Session.

Ao término da Hunt todo estado temporário é descartado.

---

# Representação

O mesmo Map poderá ser apresentado através de:

- nenhuma representação gráfica;
- imagem estática;
- ilustração;
- mapa 2D;
- mapa animado;
- representação gráfica futura.

A forma de apresentação nunca altera o domínio.

---

# Filosofia

O Map representa contexto.

A Hunt representa gameplay.

A Engine permanece completamente independente da interface.

---

# Fluxo

Region

↓

Hunt

↓

Map

↓

Gameplay

---

# Referências

Ver também:

- hunt.md
- regions.md
- cities.md