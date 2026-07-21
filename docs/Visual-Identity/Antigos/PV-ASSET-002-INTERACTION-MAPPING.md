# PV-ASSET-002 — Interaction Mapping

## Identificação

Código:

PV-ASSET-002


Nome:

Login Interface System Interaction Mapping


Status:

Approved for Engineering Reference


---

# Objetivo

Definir o mapeamento entre o asset visual de interface e os elementos funcionais da tela de Login.

O PV-ASSET-002 deve funcionar como uma camada visual de interface, enquanto os componentes funcionais permanecem implementados pelo React.


---

# Arquitetura Visual

A tela final será composta por duas camadas visuais principais:


## Camada 1 — Background

Asset:

PV-ASSET-001-login-world-background-v001.webp


Função:

Representar o mundo PROJECT-VICTORY.

Não possui elementos interativos.


---

## Camada 2 — Interface Overlay

Asset:

PV-ASSET-002-login-interface-system-v001.png


Função:

Representar a interface visual do Login.

Não possui lógica.

Não recebe eventos.


---

# Estrutura Final

Login Screen

↓

Background Layer

PV-ASSET-001-login-world-background-v001.webp


+

Interface Layer

PV-ASSET-002-login-interface-system-v001.png


+

Functional Components

React Inputs / Buttons


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

# Resolução Base

Asset Interface:

PV-ASSET-002-login-interface-system-v001.png


Resolução de referência:

1536 x 864 pixels


Todos os posicionamentos devem utilizar esta resolução como base.


---

# Áreas Interativas


## Email Input

Elemento visual:

Campo superior de autenticação.


Responsabilidade:

Receber email do usuário.


Integração:

Componente React real deve ser posicionado sobre o campo visual correspondente.


---

## Password Input

Elemento visual:

Campo inferior de autenticação.


Responsabilidade:

Receber senha do usuário.


Integração:

Componente React real deve ser posicionado sobre o campo visual correspondente.


---

## Login Button

Elemento visual:

Botão dourado central.


Responsabilidade:

Executar o fluxo existente de autenticação.


Manter:

- login()
- accountService
- onLoginSuccess


---

## Register

Elemento visual:

Link inferior direito.


Responsabilidade:

Abrir fluxo existente de criação de conta.


---

## Forgot Password

Elemento visual:

Link auxiliar.


Responsabilidade:

Preparado para futura implementação.


---

# Regras de Integração

A Engenharia deve:

- Posicionar componentes funcionais exatamente sobre os elementos visuais.
- Manter proporção do asset.
- Preservar a composição original.
- Utilizar o asset como referência visual oficial.


---

# Não Permitido

Não alterar o design do asset.

Não criar novos elementos visuais.

Não substituir a interface por HTML genérico.

Não adicionar painéis extras sobre a interface.

Não aplicar overlays que escondam a composição aprovada.


---

# Responsividade

A escala deve preservar:

- proporção do painel;
- posição dos campos;
- posição do botão;
- alinhamento visual.


Quando a resolução mudar:

O conjunto completo deve escalar proporcionalmente.


---

# Critério de Validação

Considerar correto quando:

- Background aparece ao fundo.
- Interface aparece sobre o cenário.
- Campos reais ficam alinhados com a arte.
- Botão visual corresponde ao botão funcional.
- Login continua funcionando.
- Nenhuma lógica foi alterada.


---

# Resultado Esperado

Uma tela de Login onde o jogador visualiza uma interface cinematográfica integrada ao mundo PROJECT-VICTORY.

O sistema mantém componentes reais e funcionais separados da camada visual.

A experiência visual e técnica permanecem independentes.