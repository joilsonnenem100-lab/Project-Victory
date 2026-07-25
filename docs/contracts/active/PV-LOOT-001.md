PV-LOOT-001

Sistema de Loot

Status:
PROPOSTO


Objetivo:

Criar o sistema inicial de Loot do Project-Victory.


Escopo:

Implementar a geração de itens após eventos de recompensa.


Fluxo:

Combat

↓

Reward

↓

Loot Generation

↓

ItemDefinition

↓

Inventory


Implementar:

- Loot Drop básico.
- Definição de chance de drop.
- Quantidade básica.
- Integração com Reward existente.
- Entrega do item para Inventory.


Reutilizar:

- Reward existente.
- RewardDrop existente.
- ItemDefinition existente.
- Inventory existente.


Não implementar:

- Equipamento.
- Slots.
- Stack avançado.
- Raridade.
- Crafting.
- Trading.
- Economia.
- Marketplace.


Critérios de aceite:

1. Um combate pode gerar uma recompensa com item.

2. O item gerado deve utilizar ItemDefinition existente.

3. O item deve ser entregue ao Inventory do Character.

4. O fluxo existente do Smoke Test deve continuar funcionando.


Arquivos esperados para análise:

src/ProjectVictory.Domain

src/ProjectVictory.Application

src/ProjectVictory.Persistence

tests


Entrega:

- Lista de arquivos analisados.
- Lista de arquivos alterados.
- Lista de arquivos novos.
- Build realizado.
- Testes realizados.


Orientação:

Executar usando a arquitetura existente.

Não criar sistemas paralelos.

Não expandir escopo.