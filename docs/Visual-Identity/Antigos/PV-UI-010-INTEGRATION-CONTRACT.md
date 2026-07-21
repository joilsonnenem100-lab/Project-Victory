# PV-UI-010 — Integration Contract

## Identificação

Código:

PV-UI-010


Nome:

Login Experience Enhancement


Status:

Approved for Engineering Implementation


---

# Origem

Especificação:

PV-UI-010-LOGIN-EXPERIENCE-SPECIFICATION.md


Asset principal:

PV-ASSET-001-login-world-background-v001.webp


Arquivo alvo:

src/ProjectVictory.Client/src/pages/LoginPage.tsx


---

# Objetivo

Implementar a experiência visual oficial do Login do PROJECT-VICTORY.

A alteração deve transformar a tela inicial em uma entrada cinematográfica para o mundo do jogo.


---

# Escopo autorizado

## Arquivo principal

src/ProjectVictory.Client/src/pages/LoginPage.tsx


## Alterações permitidas

- Aplicação do background oficial.
- Implementação do layout visual aprovado.
- Posicionamento do logo.
- Criação do painel visual de login.
- Aplicação de overlay cinematográfico.
- Ajustes de estilos.
- Organização visual dos links auxiliares.


---

# Composição obrigatória

A tela deve seguir:

Logo

↓

Mensagem:

"Seja bem-vindo.
Uma grande batalha o aguarda."


↓

Campos:

Usuário

Senha


↓

Botão:

ENTRAR


↓

Links:

Criar conta

Recuperar senha


---

# Direção visual

## Background

Asset:

PV-ASSET-001-login-world-background-v001.webp


Manter:

- Fortaleza visível.
- Dragões visíveis.
- Escala do mundo.
- Atmosfera misteriosa.


Aplicar:

Overlay cinematográfico suave.


---

# Interface

## Logo

Direção:

Grande presença cinematográfica.


Restrição:

Não cobrir elementos importantes do cenário.


---

## Painel

Estilo:

Híbrido.


Combinação:

- Interface moderna.
- Detalhes medievais discretos.


---

## Campos

Estilo:

Moderno limpo.


Prioridade:

Usabilidade e leitura.


---

## Botão

Estilo:

Portal.


Características:

- brilho sutil;
- destaque;
- sensação de ativação.


---

# Não alterar

Proibido modificar:

❌ Authentication Logic

❌ accountService

❌ API

❌ Backend

❌ Account System

❌ Fluxo de Login


---

# Validação obrigatória

Após implementação:

Executar:

npm run build


Validar:

✅ Login funcionando.

✅ Background carregando.

✅ Layout correto.

✅ Links funcionando.

✅ Nenhum fluxo quebrado.


---

# Critério de conclusão

PV-UI-010 será concluído quando:

- Interface implementada.
- Build aprovado.
- Login validado.
- Experiência visual aprovada pelo Product Owner.


---

# Status esperado

PV-UI-010

Integrated