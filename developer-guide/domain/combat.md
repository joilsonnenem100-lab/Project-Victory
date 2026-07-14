---

# Damage Formula Pipeline

## Objetivo

Damage Formula Pipeline define a sequência oficial utilizada pela Combat Engine para produzir qualquer resultado de dano ou cura.

Todas as Actions ofensivas e defensivas seguem exatamente o mesmo Pipeline.

A única diferença entre elas está nos dados utilizados durante o cálculo.

---

# Filosofia

A Combat Engine nunca executa cálculos em ordem arbitrária.

Toda resolução segue exatamente a mesma sequência.

Isso garante previsibilidade, repetibilidade e facilidade de manutenção.

---

# Pipeline Oficial

Todo cálculo percorre obrigatoriamente as seguintes etapas:

1. Construção do Combat Context
2. Determinação da Fórmula Base
3. Aplicação dos modificadores da Action
4. Aplicação dos modificadores do Character
5. Aplicação dos modificadores do Equipment
6. Aplicação dos Affixes
7. Aplicação dos Imbuements
8. Aplicação dos Buffs
9. Aplicação dos Debuffs
10. Aplicação das Resistências
11. Validação dos limites
12. Geração do Target Result
13. Geração do Combat Result

Nenhuma etapa poderá alterar a ordem oficial do Pipeline.

---

# Fórmula Base

A Fórmula Base representa o cálculo inicial da Action.

Cada tipo de Action possui sua própria Fórmula Base.

Exemplos:

- Auto Attack
- Offensive Spell
- Healing Spell
- Rune
- Future Actions

As fórmulas serão documentadas separadamente.

---

# Modificadores

Cada etapa do Pipeline recebe o resultado produzido pela etapa anterior.

Nenhuma etapa reinicia o cálculo.

Cada modificador trabalha exclusivamente sobre o resultado acumulado.

---

# Independência

Cada etapa possui responsabilidade única.

Exemplos:

Affixes nunca aplicam Resistências.

Buffs nunca calculam dano base.

Resistências nunca alteram Affixes.

Essa separação reduz acoplamento entre os sistemas.

---

# Determinismo

Dado o mesmo Combat Context, o Pipeline deverá produzir exatamente o mesmo Combat Result.

A Combat Engine não poderá depender de estados externos durante sua execução.

---

# Extensibilidade

Novas etapas poderão ser adicionadas ao Pipeline futuramente.

Toda nova etapa deverá possuir responsabilidade única e posição claramente definida na sequência oficial.

---

# Responsabilidades

Combat Engine é responsável por:

- executar o Pipeline;
- respeitar a ordem oficial;
- produzir Combat Result.

Os demais módulos apenas fornecem informações utilizadas durante o cálculo.

---

# Princípios Arquiteturais

- Todo cálculo segue exatamente o mesmo Pipeline.
- O Pipeline possui ordem oficial.
- Cada etapa possui responsabilidade única.
- Combat Engine permanece determinística.
- Combat Result representa exclusivamente o resultado final produzido pelo Pipeline.