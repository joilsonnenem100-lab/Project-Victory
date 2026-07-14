# ADR-012 - Single Hunt Lifecycle

Status: ACCEPTED

Version: 1.0

---

# Contexto

O Project-Victory utiliza a Hunt como núcleo do gameplay.

Era necessário definir um fluxo único de execução para evitar múltiplos caminhos de encerramento e inconsistências entre módulos.

---

# Decisão

Toda Hunt seguirá obrigatoriamente o mesmo ciclo de vida.

Fluxo oficial:

Cidade Principal

↓

Selecionar Hunt

↓

Criar Hunt Session

↓

Starting

↓

Active

↓

Ending

↓

Finished

↓

Cidade Principal

Todo encerramento utilizará exatamente o mesmo pipeline, independentemente do motivo.

Exemplos:

- morte;
- saída manual;
- desconexão;
- encerramento administrativo.

---

# Consequências

Benefícios:

- simplificação da Engine;
- previsibilidade;
- facilidade para testes;
- redução de código duplicado;
- baixo acoplamento.

Custos:

- nenhuma recuperação de Hunt após desconexão.

---

# Aplicações

Este princípio aplica-se a:

- Hunt;
- Spawn System;
- Combat Engine;
- Reward Pipeline;
- Experience Distribution.

---

# Restrições

Não será permitido:

- múltiplas Hunts simultâneas por Character;
- recuperação de Hunt Session;
- caminhos alternativos de encerramento.

Todo encerramento deverá passar obrigatoriamente pelo pipeline oficial da Hunt.

---

# Referências

Ver também:

- developer-guide/domain/hunt.md
- developer-guide/domain/experience-distribution.md
- developer-guide/domain/spawn-system.md