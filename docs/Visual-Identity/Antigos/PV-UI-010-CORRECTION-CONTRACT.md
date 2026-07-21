# PV-UI-010 — Correction Contract

## Identificação

Código:

PV-UI-010


Nome:

Login Experience Correction


Status:

Approved Correction Request


---

# Contexto

A primeira implementação visual da tela de Login foi integrada, porém o resultado final não correspondeu à direção visual aprovada pelo Product Owner.


A correção tem como objetivo restaurar a experiência cinematográfica definida para o PROJECT-VICTORY.


---

# Problemas identificados

## 1. Perda da experiência do mundo

Problema:

O painel visual aplicado ficou opaco e dominante, reduzindo a importância do cenário.


Correção:

O mundo deve permanecer visível e ser o elemento principal da tela.


---

## 2. Background ocultado

Problema:

O castelo e os dragões perderam impacto visual.


Correção obrigatória:

Manter:

- Fortaleza visível.
- Dragões visíveis.
- Escala do cenário.
- Atmosfera misteriosa.


---

## 3. Identidade visual incorreta

Problema:

O nome do jogo foi apresentado como texto simples.


Correção:

Utilizar o logo oficial do PROJECT-VICTORY.


---

# Referência visual aprovada

A implementação deve seguir a referência visual aprovada pelo Product Owner:

Características:

- Background cinematográfico.
- Logo oficial centralizado.
- Interface integrada ao mundo.
- Painel escuro transparente.
- Elementos dourados sutis.
- Castelo e dragões preservados.


---

# Direção visual obrigatória

## Background

Asset:

PV-ASSET-001-login-world-background-v001.webp


Aplicação:

- Tela completa.
- Sem cobertura por painéis opacos.
- Overlay cinematográfico suave.


---

## Logo

Obrigatório:

- Utilizar logo oficial.
- Destaque cinematográfico.
- Não cobrir elementos importantes do cenário.


---

## Painel Login

Direção:

Semi-transparente.


Características:

- Vidro escuro/fumaça.
- Bordas discretas.
- Integração com o cenário.


Não permitido:

- Caixa cinza sólida.
- Painel cobrindo o mundo.


---

## Campos

Manter:

- Boa leitura.
- Usabilidade.
- Design limpo.


---

## Botão

Direção:

Portal.


Características:

- Brilho sutil.
- Destaque dourado.
- Sensação de ativação.


---

# Escopo autorizado

Arquivo:

src/ProjectVictory.Client/src/pages/LoginPage.tsx


Permitido:

✅ Ajustar layout visual.

✅ Ajustar estilos.

✅ Ajustar composição.

✅ Aplicar logo.

✅ Ajustar transparências.


---

# Não alterar

Proibido:

❌ Authentication Logic

❌ accountService

❌ API

❌ Backend

❌ Account System

❌ Fluxo de Login


---

# Validação obrigatória

Executar:

npm run build


Validar:

✅ Login funcionando.

✅ Background preservado.

✅ Castelo visível.

✅ Dragões visíveis.

✅ Logo correto.

✅ Interface integrada.

✅ Nenhuma regressão.


---

# Critério de conclusão

PV-UI-010 será considerado concluído novamente quando:

- A experiência visual estiver alinhada à referência aprovada.
- Engenharia validar implementação.
- Product Owner aprovar resultado.


---

# Status esperado

PV-UI-010

Correction Integrated