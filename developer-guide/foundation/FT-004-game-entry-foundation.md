# FT-004 — Game Entry Foundation

Status: Approved

Tipo: Foundation Task

---

# Objetivo

Definir a arquitetura oficial necessária para permitir a entrada de um jogador autenticado no ambiente de jogo.

Esta Foundation Task não implementa Game Entry.

Seu objetivo é definir responsabilidades, contratos e fluxo arquitetural para que futuros Vertical Slices possam implementar a entrada no jogo sem tomar decisões de arquitetura.

---

# Contexto

O Project-Victory possui:

- Account;
- autenticação;
- Character;
- Runtime.

É necessário definir a transição entre essas áreas.

---

# Fluxo Oficial

O fluxo arquitetural será:

```
Account Authentication

↓

Game Session

↓

Character Loading

↓

Game Entry

↓

Runtime Environment
```

---

# Responsabilidade da Authentication

Authentication é responsável por:

- validar credenciais;
- identificar a Account;
- confirmar identidade do jogador.

Authentication não é responsável por:

- carregar Character;
- iniciar Runtime;
- criar estado de jogo.

---

# Game Session

A Session representa o contexto autenticado de jogo.

Responsabilidades:

- manter referência da Account autenticada;
- controlar o contexto da entrada no jogo;
- permitir comunicação entre autenticação e ambiente de jogo.

A Session não representa o Character.

---

# Relacionamento Session → Character

O fluxo oficial é:

```
Session

↓

AccountId

↓

Character

↓

Runtime
```

A Session utiliza a Account autenticada para localizar o Character disponível.

A Session não possui propriedade direta sobre o Aggregate Character.

---

# Carregamento do Character

O carregamento do Character deve ocorrer através de contrato de persistência.

A responsabilidade de buscar o Character pertence à camada Application.

O fluxo esperado:

```
Application

↓

ICharacterRepository

↓

Character Aggregate

↓

Runtime Preparation
```

---

# Responsabilidade da Application

A Application é responsável por:

- coordenar entrada no jogo;
- validar existência da Account;
- carregar Character;
- preparar dados necessários para Runtime.

A Application não executa lógica de gameplay.

---

# Responsabilidade do Runtime

Runtime é responsável por:

- representar o estado ativo do jogo;
- executar sistemas de gameplay;
- controlar ciclo de execução.

Runtime não deve:

- acessar Persistence diretamente;
- conhecer detalhes de banco;
- autenticar usuários.

---

# Contratos necessários

A arquitetura prevê:

```
IAccountRepository

ICharacterRepository

Game Session Contract

Game Entry Contract
```

As implementações pertencem às camadas apropriadas.

---

# Fora do Escopo

Esta Foundation Task não implementa:

- criação de sessão;
- login;
- carregamento real de Character;
- mapa;
- movimento;
- combate;
- Hunt;
- gameplay.

---

# Critério de Aceite

Esta Foundation Task será considerada concluída quando:

- O fluxo Authentication → Session → Character → Runtime estiver definido.
- As responsabilidades das camadas estiverem claras.
- A Engenharia puder implementar futuros contratos de entrada no jogo sem decisões arquiteturais.