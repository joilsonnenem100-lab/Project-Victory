# Project-Victory
# Engineering Rules - Vertical Slice

Status: OFFICIAL

---

# Objetivo

Definir as regras de engenharia utilizadas exclusivamente durante o desenvolvimento do Vertical Slice do Project-Victory.

Estas regras não substituem a arquitetura oficial do projeto.

Elas apenas padronizam a execução do Vertical Slice.

---

# Filosofia

Durante o Vertical Slice priorizamos:

- Progresso constante.
- Simplicidade.
- Pequenas entregas.
- Baixo retrabalho.
- Arquitetura limpa.
- Builds frequentes.

Nosso objetivo não é implementar a maior quantidade de funcionalidades.

Nosso objetivo é alcançar o First Fun no menor tempo possível.

---

# Roadmap

O desenvolvimento segue exclusivamente o Roadmap Oficial localizado em:

/docs/vertical-slice/ROADMAP.md

O Roadmap é considerado congelado.

Alterações somente são permitidas quando aprovadas pelo Product Owner e justificadas por uma necessidade crítica.

---

# Milestones

O Vertical Slice é dividido em Milestones.

Cada Milestone possui um objetivo claro.

Uma Milestone somente é considerada concluída quando sua entrega puder ser validada através de uma build executável.

---

# Work Items

Cada Work Item deve possuir:

- Objetivo
- Entrega
- Critério de Aceite
- Dependências
- Arquivos Permitidos
- Arquivos Proibidos

Os Work Items devem permanecer pequenos.

Caso um Work Item cresça excessivamente durante o planejamento, ele deverá ser dividido antes da implementação.

Todos os Work Items do Vertical Slice são documentos oficiais do repositório.

Local:

/docs/vertical-slice/work-items/

Formato:

PV-VS-001.md

PV-VS-002.md

PV-VS-003.md

...

---

# Numeração

O Vertical Slice utiliza uma numeração independente do projeto principal.

Formato:

PV-VS-001

PV-VS-002

PV-VS-003

...

Esta numeração é exclusiva do Vertical Slice.

---

# Sequenciamento

A implementação sempre seguirá esta ordem:

Product Owner

↓

Technical Director

↓

Work Item

↓

Arquitetura

↓

Implementação

↓

Validação

↓

Próximo Work Item

Nunca existirá mais de um Work Item em implementação simultaneamente.

---

# Build

Cada Milestone deve produzir uma build executável.

Caso isso não seja possível, a Milestone deve ser reavaliada.

---

# Evolução Arquitetural

Cada Work Item deve produzir uma evolução arquitetural mensurável.

Não é obrigatório que um Work Item gere uma funcionalidade visível.

A funcionalidade executável é responsabilidade da Milestone.

---

# Comunicação

## 🚨 DECISÃO DO PRODUCT OWNER

Sempre que uma decisão depender do Product Owner, ela será apresentada utilizando obrigatoriamente o seguinte formato:

- Assunto
- Opções
- Recomendação do Technical Director
- Impacto da decisão

Nenhuma implementação continuará enquanto uma decisão obrigatória estiver pendente.

---

## ⚠️ ATENÇÃO

Indica um risco importante.

Não exige resposta obrigatória.

---

## 💡 RECOMENDAÇÃO

Indica uma sugestão do Technical Director.

Toda recomendação deve informar sua criticidade utilizando uma escala de 0 a 100.

---

# Chat Safety

Sempre que o Technical Director identificar risco relacionado às limitações naturais do ChatGPT, deverá interromper o fluxo de trabalho utilizando um dos alertas abaixo.

## 🚨 CHAT SAFETY — DOCUMENTAR

Utilizado quando decisões importantes ainda não foram documentadas.

## 🚨 CHAT SAFETY — CONTEXTO

Utilizado quando a conversa estiver suficientemente longa para justificar consolidação.

## 🚨 CHAT SAFETY — RESUMO

Utilizado quando for recomendado gerar um resumo executivo da conversa.

## 🚨 CHAT SAFETY — RISCO

Utilizado quando a confiança sobre decisões anteriores não for suficiente para continuar com segurança.

## ⚠️ ASSUMINDO

Sempre que uma resposta depender de uma hipótese não confirmada, essa hipótese será explicitamente informada antes da recomendação técnica.

---

# Papel do Technical Director

O Technical Director possui as seguintes responsabilidades durante o Vertical Slice:

- Definir o sequenciamento técnico.
- Reduzir riscos de engenharia.
- Manter o escopo mínimo.
- Dividir funcionalidades em Work Items pequenos.
- Alimentar continuamente a equipe de arquitetura com o próximo Work Item.
- Alertar riscos técnicos antes que ocorram.
- Proteger a arquitetura existente.
- Manter o foco permanente na redução da distância até o First Fun.

---

# Definição de Sucesso

O sucesso do Vertical Slice não será medido pela quantidade de código produzido.

Será medido pela velocidade com que o projeto alcança o First Fun preservando a arquitetura oficial do Project-Victory.