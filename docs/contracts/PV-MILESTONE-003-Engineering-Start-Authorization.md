docs/contracts/PV-MILESTONE-003-Engineering-Start-Authorization.md

# PV-MILESTONE-003 — Vertical Slice 2.0 Engineering Start Authorization

## Tipo

Engineering Authorization Document

---

## Objetivo

Autorizar formalmente o início da implementação do Vertical Slice 2.0 pela Engenharia após conclusão da preparação arquitetural.

---

## Estado arquitetural

Documentação concluída:

- Visão do Vertical Slice 2.0 definida.
- Contratos técnicos definidos.
- Ordem de execução definida.
- Critérios de validação definidos.
- Controle de escopo definido.

---

## Contrato autorizado para início

Primeira implementação:

PV-VS-2.0-001

Persistence Foundation

---

## Objetivo da primeira entrega

Implementar a base de persistência necessária para evolução dos sistemas futuros.

---

## Escopo autorizado

Implementar:

- Banco de dados.
- Configuração de persistência.
- Armazenamento de Account.
- Armazenamento de Character.
- Repositories utilizando persistência real.

---

## Fora do escopo

Não implementar:

- Hunt.
- Monster.
- Combat.
- Item.
- Inventory.
- Sistemas futuros.

---

## Requisitos da entrega

Engenharia deve informar:

- Arquivos alterados.
- Resumo das alterações.
- Validação técnica.
- Validação funcional.
- Bloqueios encontrados.

---

## Validação obrigatória

Backend:

dotnet test ProjectVictory.sln

---

Client:

npm run build

---

## Critério de aprovação

A entrega será aprovada quando:

- Persistência funcionar conforme contrato.
- Account continuar funcionando após reinício.
- Character continuar disponível após reinício.
- Nenhum sistema fora do escopo for criado.

---

## Próxima etapa após aprovação

Liberar:

PV-VS-2.0-002

Account Experience Foundation

---

## Status

PV-MILESTONE-003 — Engenharia autorizada a iniciar implementação.