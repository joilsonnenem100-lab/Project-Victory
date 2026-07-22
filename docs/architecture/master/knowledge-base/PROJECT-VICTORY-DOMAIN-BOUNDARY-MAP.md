# PROJECT-VICTORY --- DOMAIN BOUNDARY MAP

## Objetivo

Definir responsabilidades entre domínios.

## Account Domain

Responsável:

-   identidade externa;
-   autenticação;
-   acesso.

Não deve controlar:

-   gameplay;
-   progressão do personagem.

------------------------------------------------------------------------

## Character Domain

Responsável:

-   identidade do personagem;
-   atributos;
-   experiência;
-   progressão.

Não deve controlar:

-   combate atual;
-   hunt ativa.

------------------------------------------------------------------------

## Hunt Domain

Responsável:

-   execução da Hunt;
-   sessões;
-   encontros.

Não deve controlar:

-   regras internas de Character.

------------------------------------------------------------------------

## Combat Domain

Responsável:

-   resolução de combate;
-   resultado.

Não deve controlar:

-   recompensa;
-   progressão diretamente.
