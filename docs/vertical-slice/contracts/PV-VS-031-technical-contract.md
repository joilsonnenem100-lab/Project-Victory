# PV-VS-031 — Technical Contract

## Nome

Finalizar Hunt Após Combate

---

# Objetivo Técnico

Implementar a capacidade de finalizar uma Hunt após a conclusão do fluxo de combate associado, garantindo que o ciclo Hunt → Combat → Hunt seja encerrado corretamente.

---

# Escopo Autorizado

Implementar exclusivamente:

- transição da Hunt após o encerramento do combate;
- finalização do estado da Hunt;
- remoção do estado ativo de combate associado;
- validações necessárias para impedir finalizações inválidas.

---

# Fluxo Técnico Esperado

```
Combat Finished

↓

Return Control To Hunt

↓

Validate Hunt State

↓

Finalize Hunt

↓

Hunt Completed
```

---

# Implementação Permitida

A Equipe de Engenharia pode alterar somente componentes existentes relacionados a:

```
src/ProjectVictory.Domain/Hunt

src/ProjectVictory.Domain/Combat

src/ProjectVictory.Application/Hunt

src/ProjectVictory.Application/Combat
```

A implementação deve utilizar os estados e contratos já existentes.

---

# Regras Obrigatórias

- Uma Hunt somente pode ser finalizada após o encerramento do combate associado.
- Uma Hunt finalizada não pode voltar para estado de execução.
- O Combat Context não pode permanecer ativo após a finalização da Hunt.
- O resultado final da Hunt deve permanecer consistente.
- O Character não deve receber alterações diretas neste contrato.
- Nenhuma recompensa deve ser aplicada neste fluxo.
- Nenhuma progressão deve ser aplicada neste fluxo.

---

# Não Implementar

Não implementar:

- Reward.
- Loot.
- Inventário.
- Progression.
- Experience.
- Level.
- Dano.
- HP.
- Morte.
- Monstros.
- Skills.
- Classes.
- Equipamentos.
- Resistências.
- Balanceamento.
- Economia.
- Runtime.
- Novos sistemas.
- Novos contratos.

---

# Critérios de Aceite Técnico

## Caso válido

Dado:

- Hunt em execução;
- combate associado finalizado;
- retorno de controle concluído;

Quando:

- solicitar finalização da Hunt;

Então:

- a Hunt muda para estado final;
- o combate associado deixa de estar ativo;
- o fluxo pode continuar para etapas posteriores.

---

## Caso inválido

Dado:

- combate ainda ativo;

Quando:

- tentar finalizar a Hunt;

Então:

- a operação deve ser rejeitada;
- o estado da Hunt permanece consistente.

---

# Testes Obrigatórios

Executar:

```bash
dotnet test ProjectVictory.sln
```

Validar:

- finalização após combate concluído;
- bloqueio de finalização durante combate ativo;
- impossibilidade de reexecutar Hunt finalizada;
- manutenção dos testes existentes.

---

# Arquivos Esperados

Alterar somente arquivos necessários ao contrato.

Possíveis áreas:

```
src/ProjectVictory.Domain/Hunt

src/ProjectVictory.Domain/Combat

src/ProjectVictory.Application/Hunt

src/ProjectVictory.Application/Combat
```

Não criar novas camadas ou estruturas arquiteturais.

---

# Restrições Arquiteturais

A Equipe de Engenharia:

- não deve criar novas decisões arquiteturais;
- não deve expandir o escopo do Vertical Slice;
- não deve implementar Reward ou Progression;
- não deve alterar Foundations existentes.

Caso exista ausência de definição arquitetural:

interromper implementação e retornar para a equipe Arquitetura.

---

# Status

Ready for Implementation