# PV-UI-010 — Two Layer Integration Contract

## Identificação

Código:

PV-UI-010


Nome:

Login Experience Two Layer Integration


Status:

Approved for Engineering Integration


---

# Objetivo

Implementar a tela de Login do PROJECT-VICTORY utilizando arquitetura visual em duas camadas:

- Background do mundo.
- Interface visual de Login.

Os componentes funcionais devem permanecer separados da arte visual.


---

# Arquitetura Aprovada

A tela final será composta por:


## Camada 1 — World Background

Asset:

PV-ASSET-001-login-world-background-v001.webp


Função:

Representar o mundo PROJECT-VICTORY.


Localização:

src/ProjectVictory.Client/src/assets/backgrounds/login/


Características:

- Tela completa.
- Sem interação.
- Preservar castelo.
- Preservar dragões.
- Preservar atmosfera.


---

## Camada 2 — Login Interface Overlay

Asset:

PV-ASSET-002-login-interface-system-v001.png


Função:

Representar a interface visual de autenticação.


Localização:

src/ProjectVictory.Client/src/assets/interfaces/login/


Características:

- Interface cinematográfica.
- Painel visual.
- Campos decorativos.
- Botão visual.
- Elementos de identidade.


---

# Componentes Funcionais

Os elementos interativos continuam sendo componentes React.


Responsabilidades:

## Email

Componente real:

Input de email.


## Password

Componente real:

Input de senha.


## Login

Componente real:

Botão responsável pelo fluxo de autenticação.


## Register

Componente real:

Navegação para criação de conta.


## Forgot Password

Componente preparado para futura implementação.


---

# Arquivo Autorizado

Arquivo principal:

src/ProjectVictory.Client/src/pages/LoginPage.tsx


---

# Escopo de Implementação

Implementar:

✅ Carregamento do Background PV-ASSET-001.

✅ Carregamento do Overlay PV-ASSET-002.

✅ Posicionamento dos componentes funcionais sobre seus respectivos elementos visuais.

✅ Manutenção do alinhamento da interface.

✅ Preservação da experiência visual aprovada.


---

# Não Alterar

Proibido modificar:

❌ Backend

❌ API

❌ Authentication Logic

❌ accountService

❌ Login Flow

❌ Account System

❌ Regras de negócio


---

# Regras Visuais

A Engenharia deve:

- Usar os assets aprovados como fonte visual oficial.
- Não recriar a interface via HTML/CSS genérico.
- Não adicionar novos painéis.
- Não alterar a composição artística.
- Não reduzir a identidade visual.


---

# Mapeamento de Interação

Documento de referência:

PV-ASSET-002-INTERACTION-MAPPING.md


Objetivo:

Garantir que:

- Inputs reais fiquem alinhados com campos visuais.
- Botão funcional fique alinhado com botão visual.
- Links fiquem posicionados corretamente.


---

# Responsividade

A composição deve manter:

- proporção dos assets;
- alinhamento dos elementos;
- posição relativa dos componentes.


O conjunto deve escalar como uma única experiência visual.


---

# Validação Obrigatória

Executar:

npm run build


Validar:

✅ Background carregando.

✅ Interface carregando.

✅ Campos alinhados.

✅ Botão funcionando.

✅ Login funcionando.

✅ Nenhuma regressão funcional.


---

# Critério de Conclusão

PV-UI-010 será considerado concluído quando:

- Os dois assets estiverem integrados.
- Os componentes funcionais estiverem alinhados.
- O fluxo de login continuar funcionando.
- A experiência visual for aprovada pelo Product Owner.


---

# Status Esperado

PV-UI-010

Two Layer Integrated