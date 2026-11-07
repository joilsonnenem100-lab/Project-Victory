# PO-DECISOES-BASE — Decisões de Game Design do Product Owner

Versão: 1.1 · Data: 2026-11-07 · Status: **FONTE DA VERDADE (Game Design)**

Origem: rodada de 52 perguntas respondidas pelo Product Owner (documento `Perguntas-PO-Project-Victory.docx`, 2026-11-06/07). Autoridade: ADR-016 (o PO decide Game Design, mecânicas, experiência do jogador e universo).

## Regras deste documento

1. As respostas abaixo valem como **decisão do PO** e devem ser consultadas antes de qualquer contrato que toque o assunto.
2. Só o PO altera uma decisão. Cada alteração exige: (a) atualizar a linha aqui e o campo *Última alteração*; (b) registrar um `DEC-` novo no `decision-log.md`; (c) atualizar, no mesmo momento, os documentos afetados (por nota de superação, sem apagar histórico) e o `docs/INDEX.md`.
3. Conflito com documento antigo: vale este documento. O documento antigo recebe nota de superação.
4. Conflito com código já implementado: registrar em `project-state.md` (Revisões Pendentes) e resolver por contrato.
5. As observações do PO estão transcritas, em caixa normal, sem mudar o sentido.


## Combate e classes

| ID | Pergunta | Decisão | Observação do PO | Aplicação |
|---|---|---|---|---|
| PO-01 | Druid: qual o papel além do dano de terra e gelo? | **B**: Dano elemental e cura própria. | — | Cura por spell para todas as vocações. Ver PO-53 (resolve o alerta A1). |
| PO-02 | Monk: qual a identidade de combate? | **A**: Dano físico alto e vida alta, simples. | Sujeito a alteração no futuro | Identidade provisória, sujeita a alteração futura. |
| PO-03 | Quando entra o combate elemental (fogo, energia, terra, gelo)? | **B**: Depois da Fase World (cidades e mapas). | No nosso jogo base queremos testar o ciclo basico primeiro. | Combate elemental só depois da Fase 7 (World). Hoje o dano é só físico. |
| PO-04 | Existem Critical e Dodge? | **A**: Nenhum dos dois. | — | — |
| PO-05 | Quem começa o combate? | **A**: Sempre o jogador. | — | — |
| PO-06 | Habilidades dos monstros da Spider Cave: | **A**: Nenhuma por enquanto. | Vamos deixar sem aplicar veneno por hora, depois podemos colocar o dano do veneno por segundo. | Sem habilidades de monstro agora (fecha R-043/R-050 para esta fase). Veneno por segundo fica para depois. |
| PO-07 | XP em grupo: | **C**: Sem grupo até a Fase 9. | Vamos manter uma hunt sem party, somente o jogador na hunt apenas por hora, talvez no futuro adicione algum conteudo em party e criaremos a mecanica. | Hunt solo. R-045 (XP em grupo) fica adiado; Party só em conteúdo futuro. |
| PO-08 | Penalidade de morte: | **A**: Nenhuma perda (como hoje). | — | — |

## Hunts e conteúdo

| ID | Pergunta | Decisão | Observação do PO | Aplicação |
|---|---|---|---|---|
| PO-09 | Como as regiões se relacionam com os títulos (Novice, Ascension 1...)? | **B**: Regiões livres, com nível recomendado em cada Hunt. | — | RequiredTitleTier deixa de ser requisito e passa a ser informativo. Alerta A2 resolvido: campo informativo, confirmado pelo PO. |
| PO-10 | O jogo mostra qual Hunt é a melhor? | **B**: Só o nível recomendado. | — | — |
| PO-11 | Duração máxima de uma Hunt: | **A**: Sem limite enquanto conectado. | — | — |
| PO-12 | Bosses: | **A**: Só na Fase 9. | — | Bosses só na Fase 9. |
| PO-13 | Quantas regiões no Primeiro Teste Jogável? | **A**: Uma (Spider Cave). | — | — |
| PO-14 | Reaparecimento dos monstros (respawn): | **B**: Com atraso curto. | — | Respawn com atraso curto (R-057). |

## Economia

| ID | Pergunta | Decisão | Observação do PO | Aplicação |
|---|---|---|---|---|
| PO-15 | Tipos de poção: | **B**: Três níveis (pequena, média, grande). | — | Três níveis de poção (pequena, média, grande). |
| PO-16 | Como obter poções antes de existir comércio? | **A**: Kit inicial e recompensa diária. | — | Kit inicial e recompensa diária (resolve a pendência de como obter poções). |
| PO-17 | O que consome ouro (sumidouros)? | **C**: Poções, taxas de mercado e serviços da cidade. | — | Sumidouros de ouro: poções, taxas de mercado e serviços da cidade. |
| PO-18 | Taxa no mercado universal: | **B**: Taxa pequena fixa. | — | — |
| PO-19 | Moeda premium (gemas): | **B**: Existe, só para cosméticos. | Faremos uma loja para uso das moedas com donates do jogador para personalizar seu personagem, e exploraremos de alguma forma para que ele mostre seus cosmeticos para outros players para ter incentivo de donates para manter o projeto, que é uma das nossas preocupações. | Gemas: loja de cosméticos paga com donates; cosméticos visíveis a outros jogadores. Ver PO-54 (dinheiro compra tempo). |
| PO-20 | Venda de loot ao NPC: | **A**: Preço fixo. | — | — |

## Itens e equipamentos

| ID | Pergunta | Decisão | Observação do PO | Aplicação |
|---|---|---|---|---|
| PO-21 | Raridade dos itens: | **A**: Cinco níveis (comum a lendário). | — | — |
| PO-22 | Slots de equipamento: | **A**: Os do Tibia. | — | — |
| PO-23 | Melhoria e craft: | **A**: Nenhum até a Fase 9. | — | — |
| PO-24 | Itens ligados à conta (não negociáveis): | **A**: Nenhum. | Mas gostaria de deixar uma opção “trancar item” para evitar que o player venda oque não deseja, ou adicionar no futuro a aba recomprar no npc. | Novo: opção Trancar item. Futuro: aba Recomprar no NPC. |
| PO-25 | Armazenamento (o inventário não tem limite): | **C**: Depósito global. | — | Depósito global. |

## Progressão e treino

| ID | Pergunta | Decisão | Observação do PO | Aplicação |
|---|---|---|---|---|
| PO-26 | Como o jogador obtém o item de treino? | **C**: As duas formas. | Tera a opção de fazer donate para comprar, por um preço alto pois mexe no balanceamento de damage do personagem. | Item de treino por compra em NPC, recompensa e donate (preço alto). Ver PO-54 (dinheiro compra tempo). |
| PO-27 | Free e Premium no treino: | **B**: Premium treina um pouco mais rápido. | — | Resolve a pendência Free × Premium do DEC-20261106-21 (Premium treina um pouco mais rápido). |
| PO-28 | Recompensa diária: | **B**: Simples (ouro e poções). | Vamos melhorar depois, adicionando um passe free e um passe com gemas do jogo para quem quiser pagar dinheiro real. | Futuro: passe Free e passe com gemas. |
| PO-29 | Teto de skills: | **A**: Sem teto. | — | — |
| PO-30 | Redistribuir skills (respec): | **A**: Não existe. | — | — |

## Social

| ID | Pergunta | Decisão | Observação do PO | Aplicação |
|---|---|---|---|---|
| PO-31 | Guildas, quando entram? | **B**: Na Fase 9. | — | — |
| PO-32 | Chat: | **B**: Global e de guilda. | — | — |
| PO-33 | Rankings: | **C**: Várias categorias (mercador, colecionador, caçador de bosses). | — | — |
| PO-34 | Vantagens do título Legend: | **B**: Título e moldura ou aura visual. | — | — |
| PO-35 | Hall da Fama: | **C**: Lista permanente com data e história. | — | — |

## Conta e personagem

| ID | Pergunta | Decisão | Observação do PO | Aplicação |
|---|---|---|---|---|
| PO-36 | Personagens por conta: | **C**: Até cinco, uma por classe. | Porém ao criar uma conta ficarão disponiveis apenas 3 slots e 2 bloqueados que poderão ser desbloqueados com gemas. | 5 slots por conta; 3 liberados e 2 bloqueados, destravados com gemas. Ver PO-54 (dinheiro compra tempo). |
| PO-37 | Aparência na criação: | **B**: Gênero e aparência. | — | — |
| PO-38 | Regras de nome: | **B**: Formato do Tibia. | — | — |
| PO-39 | Excluir personagem: | **B**: Com espera de 7 dias. | — | — |
| PO-40 | Trocar de classe: | **A**: Não existe. | — | — |

## Servidor e operação

| ID | Pergunta | Decisão | Observação do PO | Aplicação |
|---|---|---|---|---|
| PO-41 | Eventos de XP em dobro: | **C**: Eventos temáticos agendados pelo administrador. | — | — |
| PO-42 | Proteção contra bots: | **B**: Detecção de padrão e verificação no login. | — | — |
| PO-43 | Manutenção do servidor: | **A**: Avisar e encerrar as Hunts pelo pipeline, sem perda. | — | — |
| PO-44 | Plataforma: | **B**: Desktop e celular pelo navegador. | — | Interface responsiva para celular pelo navegador. |
| PO-45 | Idiomas: | **B**: Português e inglês. | — | — |

## Experiência do jogador

| ID | Pergunta | Decisão | Observação do PO | Aplicação |
|---|---|---|---|---|
| PO-46 | Avisos (Hunt terminou, morreu): | **B**: Dentro da página. | — | — |
| PO-47 | Hunt Analyzer, dados extras: | **C**: Mais histórico das últimas Hunts. | — | — |
| PO-48 | Som: | **B**: Música ambiente. | Colocar um botão de mute bem visivel para o jogador desativar, e por nas opções do jogo de desligar os soms do jogo caso não queira nunca | Botão de mute bem visível e opção nas configurações para desligar os sons. |
| PO-49 | Animação da batalha: | **B**: Média. | Podemos por uma representação um pouco aproximada do esperado, mas iremos discutir isso com mais detalhes depois | Detalhes da animação serão discutidos depois. |

## Longo prazo

| ID | Pergunta | Decisão | Observação do PO | Aplicação |
|---|---|---|---|---|
| PO-50 | Temporadas: | **A**: Não existem. | — | — |
| PO-51 | Conquistas: | **C**: Com recompensas. | — | — |
| PO-52 | Conteúdo depois da Ascension 4: | **B**: Regiões novas por etapa. | — | — |

## Decisões complementares (resposta aos alertas, 2026-11-07)

| ID | Tema | Decisão do PO |
|---|---|---|
| PO-53 | Spell de cura (resolve A1) | **Todas as vocações** têm 1 spell de cura que melhora com o Level. A cura é uma **porcentagem fixa da vida**, para balancear por Level sem tabela de números por Level. Cada uso consome Mana. Sem poção de vida, o jogador cura por spell e, como gasta muita Mana para curar e para conjurar skills, usa poção de Mana. **Emenda `DEC-20261106-03`**: a regra "sem suprimento não há cura" passa a valer só para poções; a cura por spell, que gasta Mana, é permitida. |
| PO-54 | Dinheiro compra tempo (resolve A3) | Filosofia de monetização: **dinheiro real compra tempo, não poder exclusivo**. Quem tem menos horas pode acelerar o progresso pagando, com **custo alto**, para apoiar o projeto. Itens pagos (item de treino por donate, slots extras de personagem com gemas, passes) são intencionais. Use este princípio como base para ideias de captação de renda. Gemas: loja de cosméticos visíveis a outros jogadores como incentivo ao apoio. |
| PO-55 | Região informativa (resolve A2) | Confirmado: `RequiredTitleTier` fica como dado informativo (nível recomendado), sem bloquear a entrada. |

**Ponto a confirmar com o PO (pequeno):** no PO-53, "% fixa da vida" é sobre a **vida máxima** (hipótese de trabalho, pois uma % da vida *atual* curaria menos justamente quando o jogador está pior). Valores da % e custos de Mana: PROVISÓRIOS, a balancear.

## Alertas de consistência

- **A1, A2, A3: RESOLVIDOS** em 2026-11-07 (ver PO-53, PO-55 e PO-54).
- **A4 — Pendências antigas resolvidas por estas decisões:** R-045 (XP em grupo, adiado), R-043 e R-050 (habilidades de monstro, nenhuma por ora), Free × Premium do treino (PO-27), como obter poções (PO-16), "quem começa o combate" (PO-05) e Critical/Dodge (PO-04, não existem).

## Backlog derivado (candidatos a contrato; não implementar sem fila)

- **Antes do Primeiro Teste Jogável:** spell de cura por vocação (PO-53, contrato novo no combate e no seletor automático); poções em três níveis; kit inicial e recompensa diária simples; opção Trancar item; Hunt Analyzer com histórico das últimas Hunts; mute visível e opções de som; interface responsiva; avisos dentro da página.
- **Fase 7 em diante:** combate elemental (depois do World); depósito global; regiões por Ascension de conteúdo; respawn com atraso.
- **Fase 8 (Economia):** taxa pequena fixa no mercado; sumidouros de ouro; aba Recomprar no NPC; loja de gemas e cosméticos visíveis.
- **Fase 9:** guildas, chat global e de guilda, rankings por categoria, Hall da Fama permanente, bosses, conquistas com recompensa; passe Free e passe com gemas.
- **Conta:** 5 slots (3 livres, 2 com gemas); nome no formato do Tibia; exclusão com espera de 7 dias; sem troca de classe.
- **Operação:** eventos de XP em dobro agendados pelo administrador; proteção contra bots por detecção de padrão e verificação no login; manutenção encerra as Hunts pelo pipeline, sem perda; português e inglês.


## Última alteração

2026-11-07 — criação a partir das 52 respostas do PO; v1.1 acrescenta PO-53, PO-54 e PO-55 (respostas aos alertas A1, A3 e A2).
