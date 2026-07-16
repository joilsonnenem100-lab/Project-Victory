# PV-VS-032 — Technical Contract

## Nome

Encaminhar Resultado da Hunt para Reward

---

# Objetivo Técnico

Implementar a transição responsável entre o resultado final de uma Hunt concluída e o fluxo de processamento de Reward, mantendo a separação de responsabilidades entre os domínios.

---

# Escopo Autorizado

Implementar exclusivamente:

- validação do resultado final da Hunt;
- encaminhamento do resultado para o fluxo de Reward;
- integração entre Hunt Result e Reward Processing;
- garantia de que a Hunt não seja responsável por aplicar recompensas.

---

# Fluxo Técnico Esperado

```
Hunt Completed

↓

Validate Hunt Result

↓

Create Reward Request

↓

Reward Processing Flow
```

---

# Implementação Permitida

A Equipe de Engenharia pode alterar somente componentes existentes relacionados a:

```
src/ProjectVictory.Domain/Hunt

src/ProjectVictory.Domain/Reward

src/ProjectVictory.Application/Hunt

src/ProjectVictory.Application/Reward
```

A implementação deve reutilizar contratos e estruturas existentes.

---

# Regras Obrigatórias

- Somente Hunts concluídas podem gerar encaminhamento para Reward.
- Hunts canceladas não devem gerar Reward.
- Hunts sem resultado válido não devem seguir para Reward.
- Hunt não deve calcular recompensa.
- Hunt não deve aplicar recompensa diretamente.
- Reward permanece responsável pelo processamento da recompensa.
- O Character não deve ser alterado diretamente neste contrato.

---

# Não Implementar

Não implementar:

- cálculo de recompensa;
- experiência;
- level;
- progressão;
- loot;
- inventário;
- equipamentos;
- economia;
- drop tables;
- balanceamento;
- novos sistemas;
- novos contratos;
- Runtime.

---

# Critérios de Aceite Técnico

## Caso válido

Dado:

- Hunt finalizada;
- resultado válido disponível;

Quando:

- solicitar encaminhamento para Reward;

Então:

- uma solicitação de Reward é criada;
- o fluxo de Reward recebe o resultado;
- a Hunt permanece sem responsabilidade de aplicação.

---

## Caso inválido

Dado:

- Hunt cancelada;
- Hunt incompleta;
- resultado inválido;

Quando:

- tentar encaminhar para Reward;

Então:

- a operação deve ser rejeitada;
- nenhuma recompensa deve ser processada;
- nenhum estado inválido deve ser criado.

---

# Arquivos Esperados

Alterar somente arquivos necessários ao contrato.

Possíveis áreas:

```
src/ProjectVictory.Domain/Hunt

src/ProjectVictory.Domain/Reward

src/ProjectVictory.Application/Hunt

src/ProjectVictory.Application/Reward
```

Não criar novas camadas ou estruturas arquiteturais.

---

# Testes Obrigatórios

Executar:

```bash
dotnet test ProjectVictory.sln
```

Validar:

- encaminhamento de Hunt concluída;
- rejeição de Hunt inválida;
- ausência de Reward em cancelamento;
- manutenção dos testes existentes.

---

# Restrições Arquiteturais

A Equipe de Engenharia:

- não deve criar decisões arquiteturais;
- não deve implementar regras de recompensa;
- não deve criar sistemas econômicos;
- não deve expandir o escopo do Vertical Slice.

Caso exista ausência de definição arquitetural:

interromper implementação e retornar para a equipe Arquitetura.

---

# Status

Ready for Implementation