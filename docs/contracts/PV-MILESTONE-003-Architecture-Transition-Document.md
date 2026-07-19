docs/contracts/PV-MILESTONE-003-Architecture-Transition-Document.md

# PV-MILESTONE-003 — Vertical Slice 2.0 Architecture Transition Document

## Tipo

Architecture Handover Document

---

## Objetivo

Registrar o estado arquitetural do Vertical Slice 2.0 para continuidade do projeto em novos ciclos de desenvolvimento.

---

## Estado atual

Vertical Slice 1.0 validado:

Account

↓

Login JWT

↓

Character

↓

World Entry

↓

Hunt Execution

↓

Combat Result

↓

Reward Presentation

---

## Evolução planejada

O Vertical Slice 2.0 adiciona:

- Persistência real.
- Fluxo completo de Account pelo Client.
- Hunt selecionável.
- Monster Encounter.
- Combate baseado em entidades.
- Drop de Item.
- Apresentação de Item.

---

## Contratos aprovados

PV-VS-2.0-001  
Persistence Foundation

PV-VS-2.0-002  
Account Experience Foundation

PV-VS-2.0-003  
Hunt Selection Foundation

PV-VS-2.0-004  
Monster Encounter Foundation

PV-VS-2.0-005  
Combat Evolution Foundation

PV-VS-2.0-006  
Item Drop Foundation

PV-VS-2.0-007  
Integration Validation

---

## Princípios mantidos

- Evolução incremental.
- Contratos antes de implementação.
- Separação entre Arquitetura e Engenharia.
- Controle rigoroso de escopo.
- Evitar retrabalho através de dependências corretas.

---

## Responsabilidades futuras

### Arquitetura

Continuar responsável por:

- Revisar contratos.
- Definir evolução técnica.
- Garantir consistência arquitetural.

---

### Engenharia

Continuar responsável por:

- Implementar contratos aprovados.
- Não criar decisões arquiteturais sem aprovação.
- Reportar bloqueios.

---

### Product Owner

Continuar responsável por:

- Definir visão do jogo.
- Definir prioridades de experiência.
- Aprovar decisões de produto.

---

## Próximo ciclo

A implementação deve iniciar somente após:

- Contratos revisados.
- Estado atual do repositório confirmado.
- Engenharia receber solicitação oficial.

---

## Status

PV-MILESTONE-003 — Documentação de transição concluída.