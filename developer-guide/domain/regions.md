# Regions

Status: DRAFT

Version: 1.0

---

# Objetivo

Definir o contrato arquitetural do módulo Regions.

---

# Responsabilidade

O módulo Regions é responsável exclusivamente por organizar logicamente o mundo do Project-Victory.

Regions agrupam Hunts por tema, narrativa e ambientação.

Regions não executam gameplay.

---

# Responsabilidades

O módulo Regions é responsável por:

- organizar Hunts;
- definir agrupamentos temáticos;
- disponibilizar Hunts para navegação;
- controlar a descoberta de novas Regions.

---

# Dependências

O módulo Regions poderá depender apenas de:

- Hunt;
- Character;
- Region Configuration.

---

# Proibições

O módulo Regions não poderá:

- executar Combate;
- controlar Spawn;
- controlar Monster AI;
- controlar Loot;
- controlar Experience;
- controlar Progressão;
- conhecer Combat Engine.

---

# Region

Uma Region representa um agrupador lógico de Hunts.

Ela não possui estado de execução.

Ela não possui sessões temporárias.

Ela representa conteúdo permanente.

---

# Organização

Cada Region contém uma coleção de Hunts.

Cada Hunt pertence exatamente a uma Region.

Toda ordenação das Hunts pertence à configuração de cada Hunt.

A Region apenas apresenta a coleção ordenada.

---

# Descoberta

Regions são descobertas gradualmente.

O jogador não visualiza todas as Regions desde o início.

Cada Character constrói seu próprio mapa do mundo durante sua progressão.

A forma de desbloqueio pertence aos módulos de Progressão.

---

# Regras

Regions não controlam requisitos.

Regions não controlam dificuldade.

Regions não controlam progressão.

Toda progressão pertence às Hunts.

Toda ordenação pertence às Hunts.

Toda descoberta pertence à configuração.

---

# Filosofia

Regions organizam.

Hunts executam.

A arquitetura mantém separadas:

- organização do mundo;
- progressão;
- gameplay.

---

# Fluxo

Cidade Principal

↓

Region

↓

Hunt

↓

Gameplay

---

# Referências

Ver também:

- cities.md
- hunt.md
- monsters.md