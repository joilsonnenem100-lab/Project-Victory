# PV-VS-035 — Technical Contract

## Nome

Encaminhar Reward para Progression

---

# Objetivo Técnico

Implementar a capacidade de encaminhar os efeitos produzidos por uma Reward válida para o fluxo de Progression, mantendo a separação de responsabilidades entre os domínios.

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

Character Domain
```

Porém, neste PV-VS, o fluxo termina em Progression.

O Character NÃO sofre alteração.

---

# Responsabilidades dos Domínios

## Application

Responsável por:

- coordenar o fluxo;
- iniciar a solicitação;
- encaminhar dados entre os domínios.

Application NÃO possui regras de evolução.

---

## Reward Domain

Responsável por:

- processar a recompensa;
- disponibilizar os efeitos produzidos;
- informar quais efeitos estão disponíveis para Progression.

Reward NÃO:

- calcula evolução;
- aplica experiência;
- altera Character;
- define regras de Progression.

---

## Progression Domain

Responsável por:

- receber a solicitação;
- interpretar efeitos recebidos;
- aplicar futuramente suas próprias regras de evolução.

Progression é dona das regras de evolução.

---

## Character Domain

Responsável por:

- manter suas próprias invariantes;
- receber alterações futuras quando autorizadas.

Neste PV-VS:

**Character não sofre alteração.**

---

# Escopo Autorizado

Implementar exclusivamente:

- criação de solicitação de Progression;
- transporte dos efeitos produzidos pelo Reward Domain;
- recebimento pelo Progression Domain;
- validações necessárias para garantir consistência do fluxo.

---

# Fluxo Técnico Esperado

```
Reward Applied

↓

Reward Effects Available

↓

Progression Request

↓

Progression Receives Effects

↓

Await Evolution Processing
```

---

# Regras Obrigatórias

- Reward apenas disponibiliza efeitos.
- Progression recebe a solicitação.
- Progression é responsável pelas regras futuras de evolução.
- Character não deve ser alterado.
- Application não deve possuir regras de Progression.
- Reward não deve possuir regras de Progression.
- Nenhum efeito deve ser aplicado diretamente neste PV-VS.

---

# Implementação Permitida

A Equipe de Engenharia pode alterar somente componentes relacionados a:

```
src/ProjectVictory.Application/Reward

src/ProjectVictory.Application/Progression

src/ProjectVictory.Domain/Reward

src/ProjectVictory.Domain/Progression
```

Não alterar:

```
src/ProjectVictory.Domain/Character
```

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

- Reward processada;
- efeitos disponíveis;

Quando:

- Application encaminhar para Progression;

Então:

- Progression recebe a solicitação;
- efeitos são disponibilizados corretamente;
- Character permanece inalterado.

---

## Caso inválido

Dado:

- Reward inexistente;
- efeitos inválidos;

Quando:

- solicitar Progression;

Então:

- solicitação deve ser rejeitada;
- nenhum estado de Character deve ser alterado.

---

# Testes Obrigatórios

Executar:

```bash
dotnet test ProjectVictory.sln
```

Validar:

- Reward disponibiliza efeitos;
- Progression recebe solicitação;
- ausência de alteração no Character;
- manutenção dos testes existentes.

---

# Arquivos Esperados

Alterar somente arquivos necessários ao contrato.

Possíveis áreas:

```
src/ProjectVictory.Application/Reward

src/ProjectVictory.Application/Progression

src/ProjectVictory.Domain/Reward

src/ProjectVictory.Domain/Progression
```

---

# Restrições Arquiteturais

A Equipe de Engenharia:

- não deve implementar regras de evolução;
- não deve alterar Character;
- não deve colocar Progression dentro de Reward;
- não deve colocar Reward dentro de Character;
- não deve criar atalhos entre domínios;
- não deve expandir o escopo deste Vertical Slice.

Caso exista ausência de definição arquitetural:

interromper implementação e retornar para a equipe Arquitetura.

---

# Status

Ready for Implementation