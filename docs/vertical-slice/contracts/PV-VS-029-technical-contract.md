# PV-VS-029 — Technical Contract

## Nome

Finalizar Combate

---

# Objetivo Técnico

Implementar a capacidade de finalizar um contexto de combate ativo após o processamento dos resultados de combate, garantindo que o estado do Combat Flow seja encerrado corretamente.

---

# Escopo Autorizado

Implementar exclusivamente:

- estado final de combate;
- operação de finalização do Combat Context;
- bloqueio de novas ações após finalização;
- manutenção da consistência do fluxo Combat → Hunt.

---

# Fluxo Técnico Esperado

```
Combat Active

↓

Process Combat Results

↓

Finalize Combat

↓

Combat Finished

↓

Return Control to Hunt
```

---

# Implementação Permitida

A Engenharia pode alterar somente os componentes necessários para suportar:

- finalização do Aggregate/estado de Combat existente;
- transição de estado válida;
- validações necessárias para impedir operações inválidas após encerramento.

---

# Regras Obrigatórias

- Um combate ativo pode ser finalizado.
- Um combate finalizado não aceita novas ações.
- A finalização deve manter o estado consistente.
- A finalização não deve modificar diretamente Character.
- A finalização não deve aplicar Reward.
- A finalização não deve aplicar Progression.
- A Hunt associada deve permanecer consistente.

---

# Não Implementar

Não implementar:

- dano;
- HP;
- morte;
- monstros;
- IA;
- skills;
- magias;
- classes;
- equipamentos;
- resistências;
- balanceamento;
- loot;
- reward;
- progression;
- runtime;
- novos sistemas;
- novos contratos.

---

# Componentes Envolvidos

Utilizar exclusivamente componentes existentes:

```
ProjectVictory.Domain

    Combat

    Hunt
```

Nenhuma nova camada arquitetural deve ser criada.

---

# Critérios de Aceite Técnico

## Caso válido

Dado:

- Combat Context ativo;
- resultados processados;

Quando:

- solicitar finalização;

Então:

- combate muda para estado final;
- novas ações são bloqueadas;
- fluxo retorna para Hunt.

---

## Caso inválido

Dado:

- Combat Context já finalizado;

Quando:

- tentar finalizar novamente;

Então:

- operação deve ser rejeitada ou tratada conforme regras existentes;
- estado permanece consistente.

---

# Testes Obrigatórios

Executar:

```bash
dotnet test ProjectVictory.sln
```

Validar:

- finalização de combate ativo;
- bloqueio de ações após finalização;
- manutenção dos testes existentes.

---

# Arquivos Esperados

A Engenharia deve alterar somente arquivos relacionados ao Combat Flow.

Possíveis áreas:

```
src/ProjectVictory.Domain/Combat

src/ProjectVictory.Application/Combat
```

Não criar novas estruturas fora do contrato.

---

# Restrições Arquiteturais

A Equipe de Engenharia:

- não deve criar novas decisões de domínio;
- não deve alterar Foundations;
- não deve expandir o escopo do Vertical Slice;
- não deve implementar mecânicas futuras.

Caso exista ausência de definição arquitetural:

interromper implementação e retornar para a equipe Arquitetura.

---

# Status

Ready for Implementation