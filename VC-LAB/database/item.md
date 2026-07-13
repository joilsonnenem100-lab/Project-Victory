# VC-LAB Database — Item

Status: Draft

Sprint: 032

## Objetivo

Definir a implementação do domínio de Itens.

## Arquitetura

Container

↓

ContainerSlot

↓

ItemInstance

↓

ItemMetadata

↓

ItemTemplate

## Princípios

- ItemInstance é imutável em identidade.
- ItemMetadata concentra atributos mutáveis.
- Todo Item possui exatamente uma localização.
- A localização pertence ao Slot.
- Todo armazenamento implementa IContainer.

## Benefícios

- Escalabilidade.
- Reutilização de código.
- Compatibilidade com sistemas futuros.
- Redução de duplicação.