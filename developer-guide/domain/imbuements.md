# Imbuements

# Objetivo

Imbuements representam modificadores temporários aplicados pelo jogador a uma Item Instance.

Seu objetivo é fornecer benefícios temporários sem alterar permanentemente o Item.

Imbuements pertencem exclusivamente às Item Instances.

---

# Filosofia

Imbuements são sistemas independentes dos Affixes.

Affixes representam características permanentes geradas durante a criação do Item.

Imbuements representam melhorias temporárias aplicadas posteriormente pelo jogador.

Os dois sistemas nunca compartilham responsabilidades.

---

# Aplicação

Imbuements podem ser aplicados apenas em Item Instances compatíveis.

A disponibilidade de Imbuements depende da quantidade de Imbuement Slots existentes na Item Instance.

---

# Imbuement Slots

Imbuement Slots representam espaços disponíveis para aplicação de Imbuements.

A quantidade de Slots pertence ao Item Template.

Cada Item Template define quantos Slots suas Instances possuirão.

---

# Duração

Todo Imbuement possui duração limitada.

Ao expirar:

- o Imbuement é removido;
- seus bônus deixam de existir;
- o Item permanece inalterado.

---

# Substituição

Um Imbuement pode ser removido ou substituído.

Essa operação nunca altera:

- Template;
- Tier;
- Rarity;
- Affixes.

---

# Relação com Affixes

Affixes

- permanentes;
- gerados pela Item Factory;
- fazem parte da identidade da Item Instance.

Imbuements

- temporários;
- aplicados posteriormente;
- podem expirar;
- podem ser substituídos.

Os dois sistemas são completamente independentes.

---

# Relação com Tier

Tier pode influenciar futuramente:

- quantidade de Imbuement Slots;
- tipos de Imbuements permitidos;
- potência máxima dos Imbuements.

Essa relação será definida em documentação específica.

---

# Relação com Combate

Imbuements fornecem apenas modificadores temporários utilizados durante os cálculos do sistema de combate.

Eles nunca alteram permanentemente o Character.

---

# Princípios Arquiteturais

- Imbuements pertencem exclusivamente às Item Instances.
- Imbuements são temporários.
- Affixes e Imbuements são sistemas independentes.
- Item Templates definem a quantidade de Imbuement Slots.
- A remoção de um Imbuement nunca altera permanentemente um Item.