# PV-VS-033 — Technical Contract

## Nome

Aplicar Reward da Hunt

---

# Objetivo Técnico

Implementar a capacidade de processar e aplicar uma recompensa válida originada de uma Hunt concluída, respeitando a separação de responsabilidades entre Application, Reward Domain e Character Domain.

---

# Responsabilidade Arquitetural

O fluxo obrigatório deve seguir:

```
Application

↓

Reward Domain

↓

Character
```

Responsabilidades:

## Application

Responsável por:

- coordenar o fluxo;
- receber a solicitação de aplicação;
- chamar o domínio responsável.

Application NÃO define regras de recompensa.

---

## Reward Domain

Responsável por:

- validar a recompensa;
- processar regras próprias de Reward;
- determinar os efeitos da recompensa;
- entregar o resultado de aplicação.

Reward possui responsabilidade própria dentro do Domain.

---

## Character Domain

Responsável por:

- receber alterações permitidas;
- aplicar mudanças próprias do Aggregate Character.

Character NÃO calcula Reward.

---

# Escopo Autorizado

Implementar exclusivamente:

- processamento de Reward válido;
- aplicação através do Reward Domain;
- integração Application → Reward → Character;
- validações necessárias para rejeitar Rewards inválidos.

---

# Fluxo Técnico Esperado

```
Hunt Completed

↓

Application

↓

Reward Domain

↓

Validate Reward

↓

Apply Reward Rules

↓

Character Update

↓

Final State
```

---

# Implementação Permitida

A Engenharia pode alterar somente componentes relacionados a:

```
src/ProjectVictory.Application

    Reward Flow


src/ProjectVictory.Domain

    Reward

    Character
```

A implementação deve respeitar as fronteiras dos Aggregates existentes.

---

# Regras Obrigatórias

- Reward possui responsabilidade própria no Domain.
- Application apenas coordena.
- Character não calcula recompensa.
- Reward não deve depender da camada Application.
- Reward inválido deve ser rejeitado.
- Uma Hunt cancelada não deve gerar aplicação de Reward.
- O Character somente recebe efeitos após validação do Reward.
- O estado final deve permanecer consistente.

---

# Não Implementar

Não implementar:

- Loot avançado.
- Inventário.
- Equipamentos.
- Economia.
- Mercado.
- Drop tables.
- Sistemas de raridade.
- Balanceamento.
- Progression avançada.
- Skills.
- Classes.
- Novos sistemas.
- Novos contratos.

---

# Critérios de Aceite Técnico

## Caso válido

Dado:

- Hunt concluída;
- Reward válida;

Quando:

- Application iniciar o processamento;

Então:

- Application encaminha para Reward Domain;
- Reward Domain valida e processa;
- Character recebe somente o resultado autorizado.

---

## Caso inválido

Dado:

- Reward inválida;
- Hunt cancelada;
- Resultado inconsistente;

Quando:

- solicitar aplicação;

Então:

- Reward deve ser rejeitada;
- Character não deve sofrer alteração.

---

# Testes Obrigatórios

Executar:

```bash
dotnet test ProjectVictory.sln
```

Validar:

- fluxo Application → Reward → Character;
- rejeição de Reward inválida;
- ausência de alteração no Character em caso inválido;
- manutenção dos testes existentes.

---

# Arquivos Esperados

Alterar somente arquivos necessários ao contrato.

Possíveis áreas:

```
src/ProjectVictory.Application/Reward

src/ProjectVictory.Domain/Reward

src/ProjectVictory.Domain/Character
```

Não criar novas camadas arquiteturais.

---

# Restrições Arquiteturais

A Equipe de Engenharia:

- não deve colocar regras de Reward em Application;
- não deve colocar regras de Reward em Character;
- não deve criar dependência circular entre domínios;
- não deve expandir para Progression;
- não deve implementar sistemas futuros.

Caso exista ausência de definição arquitetural:

interromper implementação e retornar para a equipe Arquitetura.

---

# Status

Ready for Implementation