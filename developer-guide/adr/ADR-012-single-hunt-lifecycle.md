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

---

# Nota de emenda (2026-11-06)

Ver `ADR-057` (ACCEPTED): o **Runtime State** do personagem é persistível em checkpoints e recuperável; a **Hunt Session** continua efêmera e **não** é recuperada — ao desconectar, a Hunt termina pelo pipeline único (decisão do PO, `DEC-20261106-05`). O texto acima permanece válido para a Hunt Session.
