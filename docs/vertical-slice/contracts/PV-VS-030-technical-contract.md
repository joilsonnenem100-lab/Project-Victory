# PV-VS-030 — Technical Contract

## Nome

Retornar Controle para Hunt após Combate

---

# Objetivo Técnico

Implementar a transição correta de controle entre o fluxo de Combat e o fluxo de Hunt após a finalização de um combate.

---

# Escopo Autorizado

Implementar exclusivamente:

- retorno do controle do fluxo Combat para Hunt;
- atualização do estado necessário para indicar que o combate foi encerrado;
- validação da transição Combat → Hunt;
- garantia de que o fluxo continue em estado consistente.

---

# Fluxo Técnico Esperado

```
Combat Active

↓

Finalize Combat

↓

Validate Transition

↓

Return Control To Hunt

↓

Continue Hunt Flow
```

---

# Implementação Permitida

A Engenharia pode alterar somente componentes existentes relacionados a:

```
ProjectVictory.Domain

    Combat

    Hunt


ProjectVictory.Application

    Combat

    Hunt
```

A implementação deve utilizar os contratos e estados já existentes.

---

# Regras Obrigatórias

- Somente um combate finalizado pode retornar controle para Hunt.
- Um combate ativo não pode liberar o fluxo da Hunt.
- Um combate finalizado não pode continuar recebendo ações.
- A Hunt deve permanecer em estado consistente após receber o controle.
- O Character não deve sofrer alterações diretas neste contrato.
- O retorno não deve aplicar Reward.
- O retorno não deve aplicar Progression.

---

# Não Implementar

Não implementar:

- Reward.
- Progression.
- Loot.
- Inventário.
- Experiência.
- Dano.
- HP.
- Morte.
- Monstros.
- Skills.
- Classes.
- Equipamentos.
- Resistências.
- Balanceamento.
- IA.
- Runtime.
- Novos sistemas.
- Novos contratos.

---

# Critérios de Aceite Técnico

## Caso válido

Dado:

- Combat Context ativo;
- combate processado;
- combate finalizado;

Quando:

- solicitar retorno para Hunt;

Então:

- o controle retorna para a Hunt;
- o estado permanece consistente;
- o fluxo pode continuar.

---

## Caso inválido

Dado:

- combate ainda ativo;

Quando:

- tentar retornar para Hunt;

Então:

- a transição deve ser rejeitada;
- nenhum estado inválido deve ser criado.

---

# Testes Obrigatórios

Executar:

```bash
dotnet test ProjectVictory.sln
```

Validar:

- retorno após combate finalizado;
- bloqueio de retorno durante combate ativo;
- manutenção dos testes existentes.

---

# Arquivos Esperados

Alterar somente arquivos necessários ao contrato.

Possíveis áreas:

```
src/ProjectVictory.Domain/Combat

src/ProjectVictory.Domain/Hunt

src/ProjectVictory.Application/Combat

src/ProjectVictory.Application/Hunt
```

Não criar novas camadas ou estruturas arquiteturais.

---

# Restrições Arquiteturais

A Equipe de Engenharia:

- não deve criar novas decisões arquiteturais;
- não deve expandir o escopo do Combat Flow;
- não deve implementar sistemas futuros;
- não deve alterar Foundations existentes.

Caso exista ausência de definição arquitetural:

interromper implementação e retornar para a equipe Arquitetura.

---

# Status

Ready for Implementation