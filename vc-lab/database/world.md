# VC-LAB Database — World

Status: Draft

Sprint: 033

## Objetivo

Definir a implementação do domínio World.

## Arquitetura

WorldService

↓

ContinentTemplate

↓

RegionTemplate

↓

HuntTemplate

↓

PackTemplate

↓

MonsterTemplate

## Runtime

HuntTemplate

↓

HuntFactory

↓

HuntInstance

↓

PackInstance

↓

MonsterInstance

## Princípios

- O mundo é composto por Templates.
- O runtime é composto por Instances.
- O mundo nunca conhece o jogador.
- Hunt é um pipeline.
- Templates são somente leitura.