# VC-LAB Database — Account

Status: Draft
Sprint: 029

## Objetivo

Definir a implementação da entidade Account.

## Tabela

Account

Campos:

- Id
- Email
- PasswordHash
- EmailConfirmed
- PremiumType
- PremiumExpiresAt
- VCPoints
- CharacterSlots
- Status
- LastLoginAt
- CreatedAt
- UpdatedAt
- DeletedAt

## Responsabilidade

Apenas identidade do jogador.

Nenhuma regra de gameplay pertence a esta tabela.

## Relacionamentos

- Wallet (1:1)
- Character (1:N)
- Settings (1:1)