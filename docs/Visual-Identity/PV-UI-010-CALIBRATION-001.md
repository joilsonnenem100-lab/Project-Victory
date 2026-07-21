# PV-UI-010-CALIBRATION-001

## Functional Hotspot Coordinate Calibration


## Identificação

Código:

PV-UI-010-CALIBRATION-001


Nome:

Functional Hotspot Coordinate Calibration


Status:

Approved for Engineering Calibration


---

# Objetivo

Criar um modo temporário de calibração visual dos hotspots funcionais da tela de Login.

O objetivo é alinhar exatamente as áreas interativas com os elementos visuais existentes no:

PV-ASSET-002-login-interface-system-v001.png


---

# Contexto

A arquitetura Two-Layer permanece aprovada:


PV-ASSET-001

+

PV-ASSET-002

+

Functional Hotspots


O problema atual está exclusivamente no posicionamento dos hotspots.


---

# Escopo autorizado

Arquivo:

src/ProjectVictory.Client/src/pages/LoginPage.tsx


Permitido criar componente auxiliar caso necessário:

LoginOverlay


---

# Não alterar

Proibido modificar:

❌ Backend

❌ API

❌ Authentication Logic

❌ accountService

❌ Login Flow

❌ Assets

❌ Layout base


---

# Implementação Temporária

Criar modo de debug:


DEBUG_HOTSPOTS = true


Quando ativo:


Todos os hotspots devem apresentar:

- borda visual;
- identificação textual;
- área de interação visível.


---

# Hotspots para calibração


## EmailHotspot

Exibir:

- nome do hotspot;
- posição;
- largura;
- altura.


Objetivo:

Alinhar exatamente com o campo Email do asset.


---

## PasswordHotspot

Exibir:

- nome do hotspot;
- posição;
- largura;
- altura.


Objetivo:

Alinhar exatamente com o campo Password do asset.


---

## LoginHotspot

Exibir:

- nome do hotspot;
- posição;
- largura;
- altura.


Objetivo:

Alinhar exatamente com o botão Login visual.


---

## RegisterHotspot

Exibir:

- nome do hotspot;
- posição;
- largura;
- altura.


Objetivo:

Alinhar exatamente com:

"Don't have an account? Register"


---

## ForgotPasswordHotspot

Exibir:

- nome do hotspot;
- posição;
- largura;
- altura.


Objetivo:

Alinhar exatamente com:

"Forgot password?"


---

# Regras Técnicas

Manter:

- container único de coordenadas;
- escala atual;
- proporção do asset.


Não alterar:

- tamanho do PV-ASSET-002;
- posição da interface;
- composição visual.


---

# Validação

Executar:


npm run build


Depois:


Abrir tela Login.


Capturar imagem com:

- hotspots visíveis;
- labels ativos;
- interface completa.


---

# Próxima etapa

Após análise visual dos hotspots:

- ajustar coordenadas;
- remover debug;
- retornar hotspots para modo transparente.


---

# Critério de conclusão

PV-UI-010-CALIBRATION-001 será concluído quando:

Todos os hotspots estiverem exatamente alinhados aos elementos visuais do PV-ASSET-002.


---

# Status esperado

PV-UI-010

Hotspot Coordinates Calibrated