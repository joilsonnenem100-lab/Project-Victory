# PV-ASSET-002 — UI Coordinate Mapping

## Identificação

Código:

PV-ASSET-002


Nome:

Login Interface System Coordinate Mapping


Versão:

v002


Status:

Approved for Engineering Reference


---

# Objetivo

Definir o mapeamento técnico entre o asset visual PV-ASSET-002 e os componentes funcionais da tela de Login.

Este documento define que:

- O asset visual é a única fonte de aparência.
- Componentes funcionais existem somente como áreas interativas.
- Nenhum componente funcional deve criar elementos visuais próprios.


---

# Arquitetura Visual Oficial

A tela final deve utilizar:


## Camada 1 — World Background

Asset:

PV-ASSET-001-login-world-background-v001.webp


Função:

Representar o mundo PROJECT-VICTORY.


Responsabilidades:

- Cenário.
- Atmosfera.
- Fortaleza.
- Dragões.
- Iluminação.


Não possui interação.


---

## Camada 2 — Login Interface Overlay

Asset:

PV-ASSET-002-login-interface-system-v001.png


Função:

Representar toda a identidade visual da interface de Login.


Responsabilidades:

- Logo.
- Moldura.
- Campos visuais.
- Botão visual.
- Elementos decorativos.


Não possui lógica.


---

## Camada 3 — Functional Hotspots

Componentes React responsáveis por interação:


- EmailHotspot
- PasswordHotspot
- LoginHotspot
- RegisterHotspot
- ForgotPasswordHotspot


Responsabilidade:

Somente:

- clique;
- foco;
- entrada de dados;
- execução de ações.


---

# Regra Principal de Implementação

Os componentes funcionais NÃO devem renderizar aparência visual.


Não permitido:

- caixas HTML visíveis;
- botões padrão;
- bordas;
- fundos;
- sombras;
- estilos próprios.


Toda aparência pertence exclusivamente ao:

PV-ASSET-002-login-interface-system-v001.png


---

# Estrutura Final Esperada


LoginPage

↓

Background Layer

PV-ASSET-001


+

Interface Layer

PV-ASSET-002


+

Functional Hotspots

React Components


---

# Localização dos Assets


## Background

Caminho:

src/ProjectVictory.Client/src/assets/backgrounds/login/


Arquivo:

PV-ASSET-001-login-world-background-v001.webp


---

## Interface

Caminho:

src/ProjectVictory.Client/src/assets/interfaces/login/


Arquivo:

PV-ASSET-002-login-interface-system-v001.png


---

# Sistema Único de Coordenadas

## Resolução Base

PV-ASSET-002:

1536 x 864 pixels


Esta resolução é a referência oficial para todos os posicionamentos.


---

# Regra de Container

Todos os elementos devem existir dentro de um único container de composição.


O container deve controlar:

- escala;
- posição;
- alinhamento;
- proporção.


O background, interface e hotspots devem compartilhar o mesmo sistema de coordenadas.


---

# Proibição de Coordenadas Independentes

Não utilizar:

- posições calculadas separadamente pelo viewport;
- escalas independentes;
- elementos fora do container principal.


Exemplo proibido:

input com posição baseada em viewport enquanto a imagem usa outra escala.


---

# Mapeamento dos Hotspots


## EmailHotspot

Referência visual:

Campo Email existente no PV-ASSET-002.


Tipo:

Input funcional transparente.


Função:

Capturar email do usuário.


Regras:

Não possuir:

- background;
- border;
- shadow;
- aparência padrão do navegador.


---

## PasswordHotspot

Referência visual:

Campo Password existente no PV-ASSET-002.


Tipo:

Input funcional transparente.


Função:

Capturar senha do usuário.


Regras:

Não possuir:

- background;
- border;
- shadow;
- aparência padrão do navegador.


---

## LoginHotspot

Referência visual:

Botão LOGIN existente no PV-ASSET-002.


Tipo:

Área clicável transparente.


Função:

Executar:

- login();
- accountService;
- onLoginSuccess.


Não criar botão visual adicional.


---

## RegisterHotspot

Referência visual:

"Don't have an account? Register"


Tipo:

Link funcional transparente.


Função:

Abrir fluxo de criação de conta.


Deve permanecer dentro da composição visual.


---

## ForgotPasswordHotspot

Referência visual:

"Forgot password?"


Tipo:

Link funcional transparente.


Função:

Preparado para futura implementação.


Deve permanecer dentro da composição visual.


---

# Controle de Browser Styling

Os inputs devem neutralizar alterações automáticas do navegador.


Controlar:

- autocomplete;
- autofill;
- appearance;
- box-shadow.


Especialmente:

input:-webkit-autofill


O navegador não pode aplicar:

- fundo branco;
- borda;
- sombra;
- alteração de cor.


---

# Regras de Escala

Quando a resolução mudar:

O conjunto completo deve escalar como uma única composição.


Manter:

- posição do logo;
- posição dos campos;
- posição do botão;
- posição dos links.


---

# Não Permitido

Não permitido:


❌ Criar novo layout HTML.

❌ Recriar interface via CSS.

❌ Alterar composição do asset.

❌ Adicionar novos painéis.

❌ Adicionar overlays sobre o PV-ASSET-002.

❌ Modificar autenticação.


---

# Arquivo de Implementação

Arquivo principal:

src/ProjectVictory.Client/src/pages/LoginPage.tsx


Arquivos adicionais permitidos:

LoginOverlay component


---

# Validação Obrigatória

Considerar correto quando:


✅ Background PV-ASSET-001 carregado.

✅ Interface PV-ASSET-002 carregada.

✅ Nenhum elemento visual HTML aparece.

✅ Inputs funcionam sem alterar a arte.

✅ Botão visual continua sendo o botão apresentado.

✅ Register permanece alinhado.

✅ Forgot Password presente.

✅ Login funcionando.


---

# Critério de Conclusão

PV-ASSET-002 UI Coordinate Mapping será aprovado quando:

A tela final apresentar:


PV-ASSET-001

+

PV-ASSET-002

+

Functional Hotspots


com separação completa entre:

Visual

e

Interação.


---

# Status Esperado

PV-ASSET-002

UI Coordinate Mapping v002

READY FOR ENGINEERING