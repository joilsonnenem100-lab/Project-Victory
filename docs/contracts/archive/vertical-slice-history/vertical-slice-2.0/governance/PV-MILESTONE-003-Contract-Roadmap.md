docs/contracts/PV-MILESTONE-003-Contract-Roadmap.md

# PV-MILESTONE-003 — Vertical Slice 2.0 Contract Roadmap

## Tipo

Execution Roadmap Document

---

## Objetivo

Definir a sequência oficial de contratos e validações para execução do Vertical Slice 2.0.

---

# Fase 1 — Foundation

## PV-VS-2.0-001
Persistence Foundation

Objetivo:

Criar a base persistente do projeto.

Entrega:

- Banco de dados.
- Persistência de Account.
- Persistência de Character.
- Repositories persistentes.

Validação:

- Reiniciar aplicação sem perder dados.
- Login funcionando.
- Character persistente.

---

# Fase 2 — Player Entry Experience

## PV-VS-2.0-002
Account Experience Foundation

Objetivo:

Permitir entrada completa pelo Client.

Entrega:

- Create Account pelo Client.
- Login pelo Client.
- Continuidade para Character.

Validação:

- Novo jogador consegue iniciar pelo Client.

---

# Fase 3 — Content Selection

## PV-VS-2.0-003
Hunt Selection Foundation

Objetivo:

Transformar Hunt em conteúdo selecionável.

Entrega:

- Hunt definida.
- Seleção pelo jogador.
- Execução da Hunt escolhida.

Validação:

- Jogador seleciona e executa Hunt.

---

# Fase 4 — Combat Content

## PV-VS-2.0-004
Monster Encounter Foundation

Objetivo:

Criar o primeiro encontro contra Monster.

Entrega:

- Monster Definition.
- Encounter.
- Dados básicos de combate.

Validação:

- Hunt cria Monster Encounter.

---

# Fase 5 — Combat Evolution

## PV-VS-2.0-005
Combat Evolution Foundation

Objetivo:

Evoluir combate abstrato para combate baseado em entidades.

Entrega:

- Player contra Monster.
- Aplicação de dano.
- Resultado de vitória ou derrota.

Validação:

- Monster possui estado de combate.
- Combate resolve corretamente.

---

# Fase 6 — Reward Expansion

## PV-VS-2.0-006
Item Drop Foundation

Objetivo:

Adicionar recompensa com identidade.

Entrega:

- Item Definition.
- Drop.
- Apresentação das características.

Validação:

- Monster derrotado gera Item.
- Jogador visualiza recompensa.

---

# Fase 7 — Final Validation

## PV-VS-2.0-007
Integration Validation

Objetivo:

Validar o ciclo completo.

Fluxo final:

Create Account

↓

Login

↓

Character

↓

Selecionar Hunt

↓

Monster

↓

Combat

↓

Reward

↓

Item

---

# Processo obrigatório por contrato

Cada contrato segue:

Arquitetura

↓

Contrato aprovado

↓

Engenharia implementa

↓

dotnet test ProjectVictory.sln

↓

npm run build

↓

Validação funcional

↓

Revisão arquitetural

↓

Próximo contrato

---

# Controle de mudanças

Qualquer alteração fora do contrato atual exige:

Identificação

↓

Análise de impacto

↓

Decisão arquitetural

↓

Atualização do contrato

↓

Implementação

---

# Ordem oficial

1. PV-VS-2.0-001  
2. PV-VS-2.0-002  
3. PV-VS-2.0-003  
4. PV-VS-2.0-004  
5. PV-VS-2.0-005  
6. PV-VS-2.0-006  
7. PV-VS-2.0-007

---

## Status

PV-MILESTONE-003 — Roadmap de contratos definido.