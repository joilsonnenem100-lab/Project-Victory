# PV-UI-010-CORRECTION-002

## Two Layer Hotspot Alignment


## Identificação

Código:

PV-UI-010-CORRECTION-002


Nome:

Two Layer Hotspot Alignment


Status:

Approved for Engineering Correction


---

# Objetivo

Corrigir a implementação da tela de Login mantendo a arquitetura visual aprovada:

PV-ASSET-001

+

PV-ASSET-002

+

Componentes funcionais transparentes


O objetivo é transformar os elementos funcionais da interface em áreas interativas invisíveis alinhadas ao asset visual.


---

# Contexto

A implementação atual utiliza corretamente o conceito de duas camadas, porém os componentes funcionais ainda estão criando aparência própria.


Problemas identificados:

- Inputs com aparência padrão do navegador.
- Campos brancos sobre o asset.
- Desalinhamento entre componentes funcionais e elementos visuais.
- Links fora da composição.
- Ausência de hotspot puro para interação.


---

# Escopo autorizado

Arquivo principal:

src/ProjectVictory.Client/src/pages/LoginPage.tsx


Arquivos adicionais permitidos:

Criar componente de suporte caso necessário:

LoginOverlay


---

# Não alterar

Proibido modificar:

- Backend.
- API.
- Authentication Logic.
- accountService.
- Account System.
- Regras de negócio.
- Fluxo de Login.


---

# Arquitetura obrigatória

A implementação deve seguir:


LoginPage

↓

Background Layer

PV-ASSET-001-login-world-background-v001.webp


+

Interface Layer

PV-ASSET-002-login-interface-system-v001.png


+

Functional Hotspots


EmailHotspot

PasswordHotspot

LoginHotspot

RegisterHotspot

ForgotPasswordHotspot


---

# Regras dos Hotspots

Os elementos funcionais devem:

- Ser transparentes.
- Não possuir aparência própria.
- Não criar bordas.
- Não criar fundos.
- Não criar sombras.
- Não substituir elementos desenhados no asset.


A arte deve continuar sendo a única fonte visual.


---

# Correção dos Inputs

Os campos Email e Password devem:

Remover:

- background padrão.
- border padrão.
- appearance.
- box-shadow.
- estilos automáticos do navegador.


Controlar:

input:-webkit-autofill


Objetivo:

O usuário deve conseguir digitar normalmente sem que o navegador altere a aparência da interface.


---

# Sistema de Coordenadas

Asset de referência:

PV-ASSET-002-login-interface-system-v001.png


Resolução base:

1536 x 864 pixels


Todos os hotspots devem utilizar o mesmo sistema de coordenadas do asset.


---

# Posicionamento obrigatório

## EmailHotspot

Deve coincidir exatamente com o campo Email visual do asset.


## PasswordHotspot

Deve coincidir exatamente com o campo Password visual do asset.


## LoginHotspot

Deve coincidir exatamente com o botão LOGIN visual do asset.


## RegisterHotspot

Deve coincidir exatamente com:

Don't have an account? Register


## ForgotPasswordHotspot

Deve coincidir exatamente com:

Forgot password?


---

# Regras de Escala

A interface completa deve utilizar um único container de referência.


O background, asset e hotspots devem escalar juntos.


Não utilizar posicionamentos independentes que causem desalinhamento.


---

# Validação obrigatória

Validar:


- Nenhuma caixa branca aparece.
- Nenhum estilo HTML padrão aparece.
- Campos permanecem alinhados.
- Botão visual permanece intacto.
- Register permanece dentro da interface.
- Forgot Password está presente.
- Login continua funcionando.


---

# Critério de Conclusão

PV-UI-010-CORRECTION-002 será aprovado quando:

A tela apresentar:


PV-ASSET-001

+

PV-ASSET-002

+

Hotspots funcionais transparentes


sem qualquer elemento visual criado pelos componentes funcionais.


---

# Status esperado

PV-UI-010

Two Layer Integration Approved