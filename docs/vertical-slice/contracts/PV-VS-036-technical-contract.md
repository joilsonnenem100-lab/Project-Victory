# PV-VS-036 — Technical Contract

## Nome

Criar Contexto de Progression

---

# Objetivo Técnico

Implementar a capacidade do domínio Progression de criar um contexto próprio de processamento a partir dos efeitos fornecidos pelo fluxo de Reward.

Este contrato estabelece apenas a criação e posse do contexto de Progression.

Nenhuma regra de evolução será aplicada neste PV-VS.

---

# Decisões Arquiteturais

O fluxo obrigatório é:

```
Application

↓

Reward Domain

↓

Progression Domain

↓

Progression Context
```

---

# Responsabilidades dos Domínios

## Application

Responsável por:

- coordenar o fluxo;
- encaminhar a solicitação;
- iniciar o processamento.

Application NÃO:

- possui regras de Progression;
- cria decisões de evolução.

---

## Reward Domain

Responsável por:

- processar Reward;
- disponibilizar efeitos produzidos.

Reward NÃO:

- controla Progression;
- cria contexto de Progression;
- aplica evolução;
- altera Character.

---

## Progression Domain

Responsável por:

- receber efeitos fornecidos;
- criar contexto próprio;
- manter o estado de processamento futuro.

Progression possui responsabilidade própria no Domain.

O contexto criado pertence exclusivamente ao domínio Progression.

---

## Character Domain

Neste PV-VS:

- não recebe alterações;
- não participa do processamento;
- mantém suas próprias invariantes.

---

# Escopo Autorizado

Implementar exclusivamente:

- criação do Progression Context;
- armazenamento dos efeitos recebidos;
- validação da criação do contexto;
- representação do estado inicial de Progression.

---

# Fluxo Técnico Esperado

```
Reward Effects

↓

Progression Request

↓

Create Progression Context

↓

Store Effects

↓

Await Evolution Processing
```

---

# Regras Obrigatórias

- Progression possui contexto próprio.
- O contexto pertence ao domínio Progression.
- Reward apenas fornece efeitos.
- Progression é responsável pelo contexto recebido.
- Character não é alterado.
- Nenhuma evolução é aplicada.
- Nenhuma regra de Level é executada.
- Nenhuma regra de Experience é executada.

---

# Implementação Permitida

A Equipe de Engenharia pode alterar somente componentes relacionados a:

```
src/ProjectVictory.Application/Progression

src/ProjectVictory.Domain/Progression
```

Não alterar:

```
src/ProjectVictory.Domain/Character
```

Não mover responsabilidades entre domínios.

---

# Não Implementar

Não implementar:

- Experience.
- Level.
- Level Up.
- Evolução de Character.
- Skills.
- Classes.
- Atributos.
- Vocação.
- Ranking.
- Balanceamento.
- Reward adicional.
- Alteração de Character.
- Novos sistemas.
- Novos contratos arquiteturais.

---

# Critérios de Aceite Técnico

## Caso válido

Dado:

- efeitos válidos fornecidos pelo Reward Domain;

Quando:

- Application solicitar criação do contexto;

Então:

- Progression cria seu próprio contexto;
- efeitos são armazenados no contexto;
- Character permanece inalterado.

---

## Caso inválido

Dado:

- efeitos inválidos;
- solicitação inconsistente;

Quando:

- tentar criar contexto;

Então:

- criação deve ser rejeitada;
- nenhum estado externo deve ser alterado.

---

# Testes Obrigatórios

Executar:

```bash
dotnet test ProjectVictory.sln
```

Validar:

- criação de Progression Context;
- armazenamento dos efeitos;
- isolamento do domínio Progression;
- ausência de alteração no Character;
- manutenção dos testes existentes.

---

# Arquivos Esperados

Alterar somente arquivos necessários ao contrato.

Possíveis áreas:

```
src/ProjectVictory.Application/Progression

src/ProjectVictory.Domain/Progression
```

---

# Restrições Arquiteturais

A Equipe de Engenharia:

- não deve implementar evolução;
- não deve alterar Character;
- não deve colocar regras de Progression em Reward;
- não deve colocar regras de Reward em Progression;
- não deve criar dependências circulares entre domínios.

Caso exista ausência de definição arquitetural:

interromper implementação e retornar para a equipe Arquitetura.

---

# Status

Ready for Implementation