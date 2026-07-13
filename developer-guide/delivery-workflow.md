# Delivery Workflow

Status: LOCKED

Version: 1.0

---

# Objetivo

Definir o padrão oficial de entrega de qualquer Sprint do Project-Victory.

Toda Sprint deve seguir exatamente este fluxo.

---

# Filosofia

Nenhuma implementação termina quando o código compila.

Uma Sprint somente é considerada concluída quando:

Arquitetura

↓

Código

↓

Testes

↓

Documentação

↓

Commit

estiverem consistentes.

---

# Fluxo Oficial

## 1

Planejamento

Definir:

- objetivo;
- impacto;
- arquitetura.

---

## 2

Implementação

Criar:

- código;
- testes;
- configurações.

---

## 3

Validação

Verificar:

- Build;
- Testes;
- Arquitetura.

---

## 4

Documentação

Atualizar somente o necessário.

---

## 5

Entrega

Toda Sprint deve apresentar:

Resumo Executivo

Arquivos Novos

Arquivos Alterados

Arquivos Removidos

Testes

Migrations

Documentação

Checklist

Status

---

# Resumo Executivo

Explicar em poucas linhas:

- o que foi implementado;
- por que;
- impacto.

---

# Arquivos Novos

Listar todos.

---

# Arquivos Alterados

Listar todos.

---

# Arquivos Removidos

Listar todos.

Caso não existam:

"Nenhum."

---

# Testes

Informar:

Novos testes.

Testes alterados.

Cobertura impactada.

---

# Migrations

Informar:

Criadas.

Alteradas.

Nenhuma.

---

# Documentação

Listar exatamente quais arquivos foram atualizados.

---

# Checklist

Antes do Commit

☐ Build OK

☐ Testes OK

☐ Arquitetura preservada

☐ Developer Guide respeitado

☐ Documentation atualizada

☐ Decision Log atualizado (quando necessário)

☐ Sem warnings críticos

---

# Status

Sempre terminar com apenas um dos estados:

Pronto para Commit

ou

Necessita Revisão

---

# Regra

Nunca entregar uma Sprint sem seguir este formato.