---

# DEC-0323

## Título

Physical Folder Structure

## Status

LOCKED

## Decisão

A estrutura de diretórios do Project-Victory faz parte da arquitetura oficial.

Novos diretórios devem representar responsabilidades claras e respeitar os Bounded Contexts.

Pastas genéricas são proibidas.
---

# DEC-0324

## Título

Technology Baseline

## Status

LOCKED

## Decisão

A stack tecnológica oficial do Project-Victory foi definida e congelada para o início do desenvolvimento.

Alterações futuras somente mediante nova decisão arquitetural documentada.
---

# DEC-0325

## Título

Project Dependency Graph

## Status

LOCKED

## Decisão

As referências entre projetos seguem um grafo fixo.

Referências fora desse grafo são proibidas.

---

# DEC-0326

## Título

Unidirectional Dependencies

## Status

LOCKED

## Decisão

Toda dependência aponta para o núcleo.

Referências circulares são proibidas.

---

# DEC-0327

## Título

Independent Runtime

## Status

LOCKED

## Decisão

A Runtime deve poder ser executada independentemente da API.

---

# DEC-0328

## Título

Build Zero Errors

## Status

LOCKED

## Decisão

A branch principal deve permanecer compilando e com testes aprovados em todos os momentos.
---

# DEC-0329

## Título

Solution Bootstrap First

## Status

LOCKED

## Decisão

A Solution deve compilar integralmente antes da criação das primeiras Entities.

---

# DEC-0330

## Título

Centralized Build Configuration

## Status

LOCKED

## Decisão

A configuração de compilação será centralizada em Directory.Build.props.

---

# DEC-0331

## Título

Compiler Assisted Quality

## Status

LOCKED

## Decisão

A qualidade do código será reforçada por analisadores estáticos e regras do compilador desde o início do projeto.

---

# DEC-0332

## Título

Zero Configuration Drift

## Status

LOCKED

## Decisão

Todos os projetos da Solution devem compartilhar a mesma configuração de compilação.
