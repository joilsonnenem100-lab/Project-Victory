# FT-003 — Character Creation Foundation

Status: Approved

Tipo: Foundation Task

---

# Objetivo

Definir a arquitetura oficial necessária para permitir a criação do primeiro Character de uma Account.

Esta Foundation Task não implementa o PV-VS-003.

Seu objetivo é definir responsabilidades, relacionamentos e contratos arquiteturais necessários para que o Vertical Slice implemente posteriormente a criação do Character sem tomar decisões de arquitetura.

---

# Responsabilidade do Domínio Character

O domínio Character representa o personagem jogável pertencente a uma Account.

Character é responsável por:

- manter sua identidade;
- manter seu estado interno;
- garantir suas invariantes;
- controlar sua evolução futura.

Character não é responsável por:

- autenticação;
- gerenciamento de Account;
- login;
- sessão;
- permissões.

---

# Relacionamento Account → Character

A propriedade oficial é:

```
Account
    |
    | possui
    |
    v
Character
```

A Account é a proprietária lógica dos Characters.

O domínio Character não possui responsabilidade sobre a existência da Account.

---

# Modelo de Propriedade

Uma Account pode possuir Characters.

A criação de um Character deve sempre estar vinculada a uma Account existente.

O Character deve possuir referência da Account proprietária através de:

```
AccountId
```

Character não deve possuir uma referência direta ao Aggregate Account.

---

# Aggregate Root

O Aggregate Root oficial permanece:

```
Character
```

Alterações no Character devem ocorrer através do próprio Aggregate.

---

# Fluxo Oficial de Criação

O fluxo arquitetural será:

```
Request

↓

Application Layer

↓

Validação dos dados necessários

↓

Criação do Character Aggregate

↓

Persistência através do ICharacterRepository

↓

Conclusão do caso de uso
```

---

# Responsabilidade da Application

A Application é responsável por:

- receber a intenção de criação;
- coordenar o fluxo;
- localizar dependências necessárias;
- solicitar criação do Aggregate;
- persistir através do contrato oficial.

A Application não contém regras internas do Character.

---

# Contrato de Persistência

O domínio expõe:

```
ICharacterRepository
```

Responsabilidade:

- adicionar Character;
- recuperar Character conforme necessidade futura.

A implementação pertence à camada Persistence.

---

# Dados obrigatórios para criação

A criação inicial de Character necessita:

- AccountId proprietário;
- identidade do Character;
- informações obrigatórias definidas pelo contrato PV-VS-003.

Nenhuma informação opcional faz parte desta Foundation Task.

---

# Estado Inicial Oficial

O Character criado deve iniciar em estado válido.

A Foundation define apenas que:

- possui identidade própria;
- pertence a uma Account;
- possui estado consistente.

Detalhes de progressão inicial pertencem ao PV-VS-003.

---

# Fora do Escopo

Esta Foundation Task não implementa:

- criação de Character;
- escolha de classe;
- atributos iniciais;
- inventário inicial;
- equipamentos;
- skills;
- gameplay;
- login;
- seleção de personagem.

---

# Critério de Aceite

Esta Foundation Task será considerada concluída quando:

- O relacionamento Account → Character estiver definido.
- O fluxo arquitetural de criação estiver definido.
- O contrato de persistência estiver definido.
- O PV-VS-003 puder ser implementado sem novas decisões arquiteturais.