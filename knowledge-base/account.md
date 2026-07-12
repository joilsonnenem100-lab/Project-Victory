# Account System

Version: 1.0

Status: LOCKED

Last Sprint: SPRINT-021

---

# Objetivo

Definir o funcionamento oficial da Account no Project-Victory.

A Account é a camada permanente do jogador.

---

# Filosofia

No Project-Victory, uma Account representa identidade.

A Account representa:

- jogador;
- histórico;
- economia;
- acesso ao mundo.

O Character representa a jornada criada dentro dessa identidade.

---

# Estrutura

```
Account

├── Authentication

├── Wallet

├── VC Points

├── Premium

├── Character Slots

└── Characters
```

---

# Account x Character

Separação oficial:

```
Account

=

Identidade do jogador


Character

=

História dentro do mundo
```

---

# Character Slots

Configuração inicial:

```
3 personagens por conta
```

Expansão:

```
VC Points

↓

Compra de Slots

↓

Máximo:

10 personagens
```

O limite deve ser configurável.

---

# Nome da Account

O nome da Account é privado.

A identidade pública será:

```
Character Name
```

---

# Wallet

A Wallet pertence à Account.

Motivo:

O progresso econômico representa o jogador.

Exemplo:

```
Character conquistou Gold

↓

Gold pertence à Wallet da Account
```

---

# VC Points

VC Points pertencem à Account.

Uso futuro:

- Premium;
- cosméticos;
- serviços;
- expansão de slots.

---

# Segurança

Sistema inicial:

```
Email

+

Senha
```

Métodos adicionais podem ser adicionados futuramente.

---

# Recuperação

A recuperação utiliza:

- email;
- métodos futuros.

---

# Exclusão

A Account não pode ser excluída inicialmente.

Personagens possuem solicitação de exclusão.

Fluxo:

```
Solicitação

↓

30 dias

↓

Exclusão do Character
```

---

# Histórico

A Account registra:

- data de criação;
- personagens;
- transações;
- compras;
- eventos.

---

# Banimento

A Account pode receber:

- bloqueio temporário;
- bloqueio permanente.

---

# Premium

Premium pertence à Account.

Permite futuramente:

- recursos adicionais;
- benefícios de conta.

---

# Multi personagem

Personagens da mesma Account podem interagir.

Exemplo:

```
Character A

↓

Trade

↓

Character B
```

---

# Arquitetura

```
AccountService

↓

AccountEntity

├── Authentication

├── Wallet

├── VCPoints

├── Premium

└── CharacterSlots

        ↓

CharacterEntity

        ↓

Progression
```

---

# Integrações

AccountService comunica com:

- CharacterService;
- WalletService;
- PaymentService;
- PremiumService;
- SecurityService.

---

# Pendências futuras

Definir:

- autenticação real;
- banco de dados;
- segurança avançada;
- regras Premium;
- limites de slots;
- sistemas de pagamento.