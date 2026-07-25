# PV-VS-2.0-067 --- Persistence Provider Architecture Decision

## Objetivo

Formalizar a decisão arquitetural referente ao mecanismo oficial de
persistência do Vertical Slice 2.0.

## Contexto

O contrato PV-VS-2.0-008 --- Account Persistence Foundation exige
persistência real dos dados após reinicialização.

O estado anterior utilizava armazenamento temporário em memória:

``` text
List<Account>
```

Esse mecanismo não atende ao requisito de persistência.

## Decisão Arquitetural

O mecanismo oficial de persistência do Vertical Slice 2.0 será:

**Entity Framework Core utilizando SQLite inicialmente.**

## Motivação

-   Persistência real após reinicialização.
-   Suporte a migrations.
-   Separação entre domínio e infraestrutura.
-   Possibilidade de evolução futura para outros providers.
-   Evitar soluções temporárias não compatíveis com produção.

## Diretriz de Implementação

Repositories devem utilizar a camada de persistência definida através do
provider oficial.

O domínio não deve conhecer detalhes do banco utilizado.

## Evolução Futura

A arquitetura deve permitir substituição do provider:

SQLite ↓ Outro Database Provider

Sem alteração das regras de negócio.

## Restrições

Não faz parte deste contrato:

-   Sistemas de economia.
-   Sistemas futuros.
-   Alterações fora do Vertical Slice 2.0.
-   Novas funcionalidades não previstas.

## Impacto

A Engenharia está autorizada a continuar a implementação do:

PV-VS-2.0-008 --- Account Persistence Foundation

utilizando a decisão arquitetural definida neste documento.
