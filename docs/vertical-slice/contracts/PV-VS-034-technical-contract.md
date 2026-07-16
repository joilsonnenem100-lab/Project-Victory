# PV-VS-034 — Technical Contract

## Nome

Confirmar Estado após Aplicação de Reward

---

# Objetivo Técnico

Implementar a capacidade de confirmar que o estado produzido após a aplicação de uma Reward permanece consistente, sem modificar estado de domínio.

---

# Responsabilidade Arquitetural

O fluxo deve respeitar as responsabilidades:

```
Application

↓

Reward Domain

↓

Character Domain

↓

Confirmation
```

---

## Application

Responsável por:

- coordenar o fluxo;
- solicitar a confirmação do resultado;
- retornar o resultado da validação.

Application NÃO possui regras de Reward.

---

## Reward Domain

Responsável por:

- processar a recompensa;
- aplicar suas próprias regras;
- produzir o resultado da aplicação.

Reward continua sendo o dono do processamento.

---

## Character Domain

Responsável por:

- manter suas próprias invariantes;
- aceitar apenas alterações válidas;
- garantir consistência do Aggregate.

Character continua sendo o dono das próprias invariantes.

---

## Confirmation

Responsável por:

- validar o resultado produzido;
- confirmar consistência;
- informar sucesso ou falha.

Confirmation NÃO altera estado.

---

# Escopo Autorizado

Implementar exclusivamente:

- validação do estado resultante após Reward;
- confirmação de consistência do resultado produzido;
- retorno do status da confirmação.

---

# Regra Principal

A confirmação:

- NÃO cria estado;
- NÃO modifica estado;
- NÃO recalcula Reward;
- NÃO aplica alterações no Character.

A confirmação apenas valida o resultado existente.

---

# Fluxo Técnico Esperado

```
Hunt Completed

↓

Application

↓

Reward Domain

↓

Apply Reward

↓

Character State Updated

↓

Confirm Result

↓

Validation Response
```

---

# Implementação Permitida

A Engenharia pode alterar somente componentes necessários relacionados a:

```
src/ProjectVictory.Application/Reward

src/ProjectVictory.Domain/Reward

src/ProjectVictory.Domain/Character
```

A implementação deve reutilizar os contratos existentes.

---

# Regras Obrigatórias

- A confirmação deve ser uma operação sem efeito colateral.
- O estado do Character não deve ser alterado durante a confirmação.
- Reward não deve perder responsabilidade sobre processamento.
- Character não deve perder responsabilidade sobre suas invariantes.
- Application não deve conter regras de domínio.
- Resultados inválidos devem ser identificados.

---

# Não Implementar

Não implementar:

- Experience.
- Level.
- Progression.
- Level Up.
- Skills.
- Classes.
- Equipamentos.
- Inventário.
- Economia.
- Loot avançado.
- Balanceamento.
- Novos sistemas.
- Novos contratos.

---

# Critérios de Aceite Técnico

## Caso válido

Dado:

- Reward processada;
- Character atualizado;
- estado consistente;

Quando:

- solicitar confirmação;

Então:

- a confirmação retorna sucesso;
- nenhum estado é alterado.

---

## Caso inválido

Dado:

- resultado inconsistente;
- estado inválido;

Quando:

- solicitar confirmação;

Então:

- a confirmação retorna falha;
- nenhum estado é alterado.

---

# Testes Obrigatórios

Executar:

```bash
dotnet test ProjectVictory.sln
```

Validar:

- confirmação de resultado válido;
- rejeição de resultado inválido;
- ausência de alteração de estado durante confirmação;
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

- não deve colocar regras de Reward na confirmação;
- não deve alterar Character durante validação;
- não deve mover responsabilidades entre domínios;
- não deve implementar Progression neste contrato.

Caso exista ausência de definição arquitetural:

interromper implementação e retornar para a equipe Arquitetura.

---

# Status

Ready for Implementation