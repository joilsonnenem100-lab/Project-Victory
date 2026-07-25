# PV-FRONTEND-005 — Game HUD Inicial

Status:

APROVADO PARA IMPLEMENTAÇÃO


## Objetivo

Criar a primeira camada permanente de interface durante o gameplay.

O objetivo é permitir que o jogador visualize informações importantes do personagem enquanto está no mundo.


## Base existente

Já disponível:

Character Information UI
✅

Inventory UI
✅

Reward Feedback UI
✅


Utilizar dados existentes do backend.


## Implementar

Criar HUD inicial contendo:

- Nome do personagem.
- Level.
- Experience.
- Informações disponíveis de status do personagem.


## Integração esperada

Backend

↓

Character Data

↓

Frontend Service

↓

Game HUD


## Regras

Não implementar:

- Sistema de atributos novo.
- Skills.
- Combate visual.
- Equipamentos.
- Barras com dados inexistentes.
- Mecânicas novas.


Não alterar:

- Domain.
- Sistemas de gameplay.
- Regras de Progression.


## Antes de implementar

Consultar:

src/ProjectVictory.Client/src/pages/

src/ProjectVictory.Client/src/services/

src/ProjectVictory.Client/src/models/


Confirmar quais dados já estão disponíveis.


## Critérios de aceite

1. HUD aparece durante a experiência de jogo.

2. Informações exibidas vêm de dados reais.

3. Frontend compila.

4. Fluxos existentes continuam funcionando.


## Entrega

Informar:

- Arquivos analisados.
- Arquivos alterados.
- Arquivos novos.
- Resultado do build.

Fim do contrato.