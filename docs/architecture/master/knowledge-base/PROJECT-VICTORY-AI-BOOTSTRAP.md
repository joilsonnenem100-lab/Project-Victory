# PROJECT-VICTORY AI BOOTSTRAP

## Propósito

Este documento define como a IA deve atuar dentro do Project-Victory.

Ele estabelece regras obrigatórias de análise, decisão, documentação e implementação.

O objetivo é preservar a arquitetura, acelerar desenvolvimento e evitar regressões.

---

# 1. Fonte da Verdade

O Project-Victory possui uma única fonte oficial:

O snapshot/documentação fornecido pelo Product Owner.

Antes de qualquer:

- decisão arquitetural;
- criação de contrato;
- sugestão técnica;
- implementação;

a IA deve consultar os arquivos oficiais disponíveis.

Nunca assumir:

- arquivos existentes;
- classes existentes;
- métodos existentes;
- arquitetura não documentada.

---

# 2. Classificação das Informações

Toda informação deve ser classificada como:

## Implementado

Existe no código atual.

Pode ser utilizado.

---

## Planejado

Foi definido arquiteturalmente, mas ainda não existe.

Não deve ser tratado como código existente.

---

## Futuro

É conceito ou possibilidade.

Não deve influenciar implementação atual sem contrato aprovado.

---

# 3. Regra de Impacto de Alteração

Antes de qualquer alteração de código existente:

A Engenharia deve realizar análise de impacto.

Obrigatório identificar:

- arquivos afetados;
- dependências existentes;
- fluxos que utilizam esses arquivos;
- riscos de regressão;
- estratégia de preservação.

Nenhuma alteração deve iniciar sem essa análise.

Fluxo obrigatório:

Analisar snapshot

↓

Identificar impacto

↓

Definir alteração mínima

↓

Implementar

↓

Build

↓

Validar fluxos existentes

↓

Entregar evidências

---

# 4. Contratos Pequenos e Controlados

Todo trabalho deve possuir contrato.

Contratos devem possuir:

- objetivo único;
- escopo limitado;
- arquivos envolvidos;
- critérios de aceite;
- fora de escopo.

Evitar:

- grandes blocos;
- múltiplos sistemas no mesmo contrato;
- refatorações misturadas com features.

Princípio:

Uma alteração deve resolver uma coisa por vez.

---

# 5. Preservação de Sistemas Validados

Sistemas já aprovados possuem proteção.

Antes de alterar:

- Account;
- Character;
- World;
- Hunt;
- Combat;
- Reward;
- Progression;

avaliar impacto.

Uma nova funcionalidade não pode quebrar um fluxo validado.

---

# 6. Análise Antes de Implementação

A IA deve seguir:

Entender objetivo

↓

Consultar documentação

↓

Analisar estado atual

↓

Identificar conflitos

↓

Propor solução

↓

Solicitar decisão quando necessário

↓

Implementar somente após aprovação

---

# 7. Não Repetir Decisões Já Fechadas

Decisões aprovadas devem ser consideradas conhecimento oficial.

Não perguntar novamente:

- conceitos já documentados;
- escolhas já aprovadas;
- regras de gameplay já definidas.

Consultar documentação antes de criar questionários.

---

# 8. Separação de Responsabilidades

Manter separação:

## Arquitetura

Responsável por:

- decisões estruturais;
- contratos;
- padrões;
- evolução técnica.

---

## Engenharia

Responsável por:

- implementação;
- testes;
- build;
- validação técnica.

---

## Product Owner

Responsável por:

- experiência do jogador;
- regras de gameplay;
- identidade do jogo.

---

# 9. Alterações em Arquivos Críticos

Arquivos centrais possuem risco elevado:

- Aggregate Roots;
- App.tsx;
- Services compartilhados;
- Handlers;
- Controllers;
- Persistência;
- Componentes globais.

Alterações nesses arquivos exigem análise adicional.

---

# 10. Preferência por Evolução Incremental

Priorizar:

pequena mudança

↓

validação

↓

próxima evolução

Evitar:

grande implementação

↓

muitos problemas

↓

correção em cadeia

---

# 11. Validação Obrigatória

Toda implementação deve retornar:

- arquivos alterados;
- build realizado;
- testes executados;
- fluxo validado;
- problemas encontrados.

---

# 12. Regra de Comunicação

Respostas devem ser:

- objetivas;
- baseadas na fonte oficial;
- sem teorias desnecessárias;
- sem criar decisões de produto sem autorização.

Quando houver dúvida:

parar

↓

identificar conflito

↓

solicitar decisão correta.

---
# REGRA DE CONTEXTO

Nenhuma informação derivada de memória de conversas anteriores
pode substituir arquivos presentes no snapshot atual.

Caso exista conflito:

Snapshot > Documentação > Contrato > Memória da conversa.

# Princípio Final

O objetivo da IA no Project-Victory é:

Acelerar desenvolvimento sem sacrificar arquitetura.

Toda decisão deve buscar:

- menor risco;
- maior clareza;
- preservação do trabalho existente;
- evolução sustentável do projeto.
---

# Regra de Evolução

Toda implementação deve seguir o princípio:

Adicionar > substituir.

Código existente e funcionalidades validadas são patrimônio do projeto.

Antes de alterar qualquer arquivo existente, verificar:

- qual funcionalidade ele suporta;
- quem depende dele;
- quais fluxos podem ser afetados.

Uma melhoria que remove comportamento existente é considerada regressão.