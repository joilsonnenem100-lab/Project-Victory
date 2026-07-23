# PROJECT-VICTORY --- ARCHITECTURE RULES

-   Não assumir que conceitos futuros existem no código.
-   Separar arquitetura planejada de implementação existente.
-   Consultar contratos antes de alterações.
-   Manter responsabilidades claras entre sistemas.
-   Separar estado permanente de estado temporário.
-   Evitar acoplamento direto entre agregados.

------------------------------------------------------------------------

# REGRA DE IMPACTO DE ALTERAÇÃO

Antes de alterar qualquer arquivo existente no Project-Victory, a
Engenharia deve realizar uma análise de impacto obrigatória.

Nenhuma implementação deve iniciar sem compreender os efeitos da
alteração no sistema atual.

## Análise obrigatória antes da implementação

A Engenharia deve identificar:

-   arquivos que serão alterados;
-   dependências existentes desses arquivos;
-   fluxos do sistema que utilizam esses arquivos;
-   possíveis riscos de regressão;
-   estratégia para preservar funcionalidades já validadas.

------------------------------------------------------------------------

# Processo obrigatório de alteração

Toda alteração deve seguir:

Analisar snapshot atual

↓

Identificar arquivos envolvidos

↓

Avaliar impacto e riscos

↓

Implementar alteração mínima

↓

Executar build

↓

Validar fluxos existentes

↓

Entregar evidências da implementação

------------------------------------------------------------------------

# Princípios de preservação

Não permitido:

-   alterar arquivos existentes sem análise de impacto;
-   modificar múltiplos sistemas sem necessidade do contrato;
-   refatorar arquitetura durante implementação de uma funcionalidade;
-   substituir comportamentos existentes sem validar seus consumidores;
-   resolver um problema criando regressões em funcionalidades já
    aprovadas.

------------------------------------------------------------------------

# Arquivos de alto impacto

Alterações em arquivos centrais exigem atenção adicional:

-   Aggregate Roots;
-   Controllers;
-   Application Handlers;
-   Services compartilhados;
-   App.tsx;
-   Componentes globais;
-   Configurações;
-   Estruturas de persistência.

------------------------------------------------------------------------

# Princípio fundamental

Toda alteração deve responder:

"Qual impacto essa mudança causa no restante do sistema?"

Caso o impacto não esteja claro, a implementação deve ser interrompida
para análise arquitetural.
