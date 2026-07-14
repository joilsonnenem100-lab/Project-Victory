# Skills

## Objetivo

Skills representam a proficiência permanente de um Character em uma determinada disciplina.

Toda Skill pertence exclusivamente ao Character, evolui permanentemente e influencia diretamente a eficiência em combate, magias ou outras atividades do jogo.

Skills são parte do domínio e não podem ser alteradas diretamente por sistemas externos.

---

# Skills Oficiais

O Project-Victory possui as seguintes Skills oficiais:

- Magic Level
- Fist Fighting
- Club Fighting
- Sword Fighting
- Axe Fighting
- Distance Fighting
- Shielding
- Fishing

Não existem Attributes tradicionais como Strength, Dexterity, Intelligence ou Vitality.

Toda progressão permanente do personagem ocorre através dessas Skills.

---

# Responsabilidade das Skills

Cada Skill representa domínio sobre uma categoria específica.

## Magic Level

Representa a proficiência em magia.

Influencia curas, dano mágico, suporte e demais efeitos relacionados à utilização de spells.

Evolui através da utilização de magias.

---

## Fist Fighting

Representa a proficiência na utilização de armas da categoria Fist.

É a principal Skill da vocação Monk.

Também pode evoluir nas demais vocações conforme seus multiplicadores.

---

## Club Fighting

Representa a proficiência na utilização de armas da categoria Club.

---

## Sword Fighting

Representa a proficiência na utilização de armas da categoria Sword.

---

## Axe Fighting

Representa a proficiência na utilização de armas da categoria Axe.

---

## Distance Fighting

Representa a proficiência na utilização de armas da categoria Distance.

Inclui arcos, bestas, lanças e demais armas de ataque à distância.

---

## Shielding

Representa a proficiência defensiva utilizando equipamentos defensivos compatíveis.

---

## Fishing

Representa a proficiência na atividade de pesca.

Sua mecânica será definida futuramente.

---

# Estrutura da Skill

Toda Skill possui dois estados permanentes:

- Level
- Progress

Level representa o nível atual da Skill.

Progress representa o progresso acumulado dentro do nível atual.

O progresso é persistente.

---

# Progressão

Toda evolução ocorre através da geração de Progress.

Cada ação válida gera uma quantidade de Progress correspondente à Skill relacionada.

Ao atingir o valor necessário para o próximo nível:

- o Level aumenta;
- o Progress excedente permanece armazenado.

Progress nunca é perdido.

---

# Skill Progress Policy

A quantidade de Progress necessária para evoluir uma Skill é determinada por uma política de progressão baseada em curva exponencial parametrizável.

A implementação da fórmula faz parte da Engine e permanece desacoplada do domínio.

---

# Barra de Progressão

Toda Skill possui barra de progresso.

A barra representa exatamente o Progress persistido da Skill.

Não se trata de um efeito visual independente.

---

# Evolução das Skills

Cada Skill evolui exclusivamente através de sua fonte oficial.

- Magic Level → utilização de magias.
- Fist Fighting → utilização de armas Fist.
- Club Fighting → utilização de armas Club.
- Sword Fighting → utilização de armas Sword.
- Axe Fighting → utilização de armas Axe.
- Distance Fighting → utilização de armas Distance.
- Shielding → utilização de equipamentos defensivos durante combate.
- Fishing → atividade de pesca.

---

# Trainer

O Trainer nunca altera diretamente uma Skill.

Ele apenas gera Progress.

A evolução utiliza exatamente as mesmas regras do combate.

---

# Offline Training

Offline Training também gera apenas Progress.

Toda evolução utiliza a mesma Skill Progress Policy.

---

# Multiplicadores por Vocação

Todas as vocações podem evoluir todas as Skills.

Cada vocação possui multiplicadores próprios para determinar a velocidade de evolução de cada Skill.

A diferença entre vocações está na velocidade de progressão, nunca na possibilidade de evolução.

---

# Equipamentos

Equipamentos nunca alteram permanentemente uma Skill.

Eles fornecem apenas bônus temporários utilizados durante cálculos de combate.

A Skill permanente do Character permanece inalterada.

---

# Princípios Arquiteturais

- Skills pertencem exclusivamente ao Character.
- Skills representam progresso permanente.
- Skills nunca diminuem.
- Progress nunca é perdido.
- Equipamentos nunca modificam permanentemente uma Skill.
- Trainer e Offline Training utilizam exatamente a mesma política de evolução do combate.