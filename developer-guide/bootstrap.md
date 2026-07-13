# Bootstrap Guide

Status: LOCKED

Version: 1.0

---

# Objetivo

Este documento define a configuração oficial da Solution Project-Victory.

Nenhuma decisão de bootstrap poderá ser tomada pela implementação.

Toda configuração descrita aqui é obrigatória.

---

# Baseline Tecnológica

Language:

C#

Target Framework:

.NET 10

Test Framework:

xUnit

ORM:

Entity Framework Core

Database:

PostgreSQL

---

# Solution

Nome:

ProjectVictory.sln

Toda implementação deve utilizar esta Solution.

---

# Estrutura

Raiz oficial:

src/

Projetos de testes:

tests/

Ferramentas:

tools/

Documentação:

docs/

Arquitetura:

engineering/

Gameplay:

vc-lab/

Conhecimento:

knowledge-base/

---

# Directory.Build.props

Obrigatório.

Responsável por centralizar:

- Target Framework
- Nullable
- Implicit Usings
- LangVersion
- Analysis Level
- Warnings
- Configuração comum dos projetos

Toda configuração compartilhada deve permanecer neste arquivo.

---

# Nullable

Obrigatório.

Valor:

Enable

---

# Implicit Usings

Obrigatório.

Valor:

Enable

---

# LangVersion

Valor:

latest

---

# AnalysisLevel

Valor:

latest

---

# Warnings

Warnings devem ser tratados como erros apenas quando definidos oficialmente pela arquitetura.

Nenhuma configuração deve ser criada por dedução.

---

# .editorconfig

Obrigatório.

Deve conter apenas regras aprovadas pela arquitetura.

Não adicionar StyleCop ou regras extras sem DEC.

---

# .gitignore

Utilizar o padrão oficial para projetos .NET.

Adicionar apenas exclusões necessárias ao Project-Victory.

---

# README

Objetivo:

Apresentar o Project-Victory.

Não documentar arquitetura detalhada.

A arquitetura pertence à pasta docs/.

---

# Implementação

A Equipe de Desenvolvimento pode implementar:

- Directory.Build.props
- .editorconfig
- .gitignore
- README.md

desde que siga integralmente este documento.

---

# Proibições

É proibido:

- alterar a arquitetura;
- alterar a estrutura da Solution;
- alterar decisões LOCKED;
- criar configurações por hipótese.

Em caso de dúvida, interromper a implementação.

---

# Filosofia

O Bootstrap existe para padronizar toda a Solution.

Ele deve permanecer simples, consistente e centralizado.