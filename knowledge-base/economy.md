# Economy System

Version: 1.1

Status: EM DESENVOLVIMENTO

Last Sprint: SPRINT-007

---

# Objetivo

Definir o funcionamento oficial da economia do Project-Victory.

Este documento contém decisões aprovadas sobre:

- Gold;
- Wallet;
- Moedas;
- Market;
- Taxas;
- Controle econômico.

---

# Filosofia

No Project-Victory, a economia representa dedicação e longevidade do servidor.

O sistema deve recompensar:

- tempo investido;
- conhecimento do mercado;
- decisões inteligentes;
- interação entre jogadores.

---

# Moeda Principal

## Gold

Gold é o recurso primário do jogo.

É utilizado para:

- serviços básicos;
- consumíveis;
- taxas;
- melhorias;
- sistemas econômicos.

---

# Wallet

Todo personagem possui uma Wallet própria.

Ao criar um personagem:

```text
Character

↓

Wallet ID

↓

Balance
```

---

# Gold Obtido

Gold não existe como item físico.

Exemplo:

Jogador derrota um Dragon.

Loot:

+50 Gold

Resultado:

O valor é adicionado automaticamente à Wallet.

Nenhum slot de inventário é utilizado.

---

# Fontes de Gold

Gold pode ser obtido através de:

- Monstros;
- Bosses;
- Quests;
- Eventos;
- Venda para NPC;
- Market;
- Guild;
- Sistemas futuros.

---

# Gastos de Gold

Gold pode ser utilizado em:

- Repair;
- Upgrade;
- Craft;
- Market Tax;
- Guild;
- Boss Key;
- Imbuement;
- NPC;
- Collections.

---

# Trainer

Trainer não utiliza Gold diretamente.

O acesso ao treinamento depende de itens específicos.

---

# Limite de Gold

Não existe limite oficial de saldo.

A arquitetura deve permitir grandes valores.

A apresentação visual poderá utilizar abreviações.

Exemplo:

```text
5.000.000.000

↓

5B
```

---

# Gold Negativo

Jogadores nunca poderão possuir saldo negativo.

Caso o jogador não possua recursos suficientes:

A operação não poderá ser confirmada.

---

# Múltiplas Moedas

O sistema deverá suportar múltiplas moedas.

Exemplos futuros:

- VC Points;
- Event Tokens;
- Guild Currency;
- Boss Tokens.

A arquitetura não deve limitar o crescimento do sistema.

---

# VC Points

> *Nota de superação (2026-11-07, `DEC-20261107-02`, PO-19/PO-54 em `docs/product/PO-DECISOES-BASE.md`): a moeda premium chama-se **gemas** e vem de donates. Uso: loja de **cosméticos visíveis** a outros jogadores e aceleração paga com **custo alto** (item de treino, slots extras de personagem, passes). Princípio: **dinheiro real compra tempo, não poder exclusivo**.*

VC Points representam uma fonte de monetização externa do projeto.

Serão utilizados para manutenção dos custos do servidor e possíveis sistemas futuros.

---

# Controle de Inflação

O servidor remove Gold através de:

> *Nota de superação (2026-11-07, `DEC-20261107-02`, PO-17/PO-23/PO-12 em `docs/product/PO-DECISOES-BASE.md`): sumidouros de ouro aprovados: **poções, taxas de mercado e serviços da cidade**. Upgrade e Craft não existem até a Fase 9; Bosses só na Fase 9.*

- Market Tax;
- Repair;
- NPC;
- Upgrade;
- Craft;
- Boss;
- Guild;
- Collections.

---

# Market

## Filosofia

O Market representa sabedoria e investimento.

---

## Funcionamento

O Market é global.

Todos os jogadores do servidor podem acessar os anúncios.

---

## Anúncios

O jogador anuncia itens específicos.

Exemplo:

Fire Sword Rare

+7

Critical

Life Leech

O comprador recebe exatamente aquele item.

---

## Duração

Valor padrão:

7 dias.

Este valor deve ser configurável.

---

## Taxa

A taxa do Market é cobrada ao criar o anúncio.

> *Nota de superação (2026-11-07, `DEC-20261107-02`, PO-18 em `docs/product/PO-DECISOES-BASE.md`): a taxa do mercado universal é **pequena e fixa**.*

---

## Preço

O Market suporta:

- Preço fixo;
- Leilão.

---

## Leilão

Itens específicos poderão utilizar sistema de leilão.

A quantidade de itens disponíveis em leilão poderá possuir limites para controle econômico.

---

# Trade Direto

Jogadores podem negociar diretamente.

Fluxo:

Jogador anuncia no chat.

↓

Outro jogador inicia:

trade with Player

↓

Janela de Trade

↓

Transferência direta

---

# Taxas

Trade direto entre jogadores não possui taxas.

O objetivo é incentivar comunicação e interação social.

---

# Item Link no Chat

Jogadores poderão inserir itens no chat.

Outros jogadores poderão visualizar:

- Nome;
- Categoria;
- Raridade;
- Upgrade;
- Afixos;
- Imbuements;
- Informações relevantes.

---

# Unique Tracking

Itens Unique possuem controle de quantidade existente no servidor.

Este dado será utilizado para acompanhamento econômico.

---

# Pendências

Ainda precisam ser definidos:

- Banco de dados da Wallet;
- Estrutura das moedas futuras;
- Fórmula das taxas;
- Sistema completo de leilão;
- Histórico econômico.