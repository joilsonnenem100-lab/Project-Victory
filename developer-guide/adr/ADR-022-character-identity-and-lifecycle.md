# ADR-022 - Character Identity and Lifecycle

Status: ACCEPTED

Version: 1.0

---

# Contexto

O Character representa a identidade jogável permanente do jogador.

Era necessário definir regras para identidade, nome, classe e ciclo de vida, preservando consistência e segurança ao longo dos anos.

---

# Decisão

Cada Character pertence exatamente a uma Account.

Uma Account poderá possuir múltiplos Characters.

Apenas um Character poderá permanecer ativo simultaneamente.

A classe é permanente.

A alteração de nome ocorre exclusivamente através do Rename Service.

A exclusão ocorre através de um fluxo seguro utilizando Pending Deletion.

---

# Alteração de Nome

A alteração de nome seguirá as seguintes regras:

- execução exclusiva pelo Rename Service;
- custo progressivo;
- reserva do nome anterior por seis meses;
- histórico permanente de alterações;
- reutilização automática após o período de reserva.

Toda identidade técnica permanece vinculada ao CharacterId.

---

# Exclusão

Fluxo oficial:

Solicitação

↓

Pending Deletion

↓

30 dias

↓

Exclusão definitiva

Durante o período de segurança:

- a exclusão poderá ser cancelada;
- o Character permanece preservado;
- o nome continua reservado.

---

# Consequências

Benefícios:

- preservação da identidade;
- proteção contra exclusões acidentais;
- maior segurança;
- auditoria completa;
- estabilidade da arquitetura.

Custos:

- necessidade de serviços específicos para Rename e Deletion.

---

# Aplicações

Este princípio aplica-se a:

- Character;
- Account;
- Friends;
- Guilds;
- Rankings;
- Social Systems.

---

# Restrições

A classe nunca poderá ser alterada.

A identidade funcional do Character sempre será determinada pelo CharacterId.

O nome é apenas uma propriedade alterável.

---

# Referências

Ver também:

- developer-guide/domain/character.md
- developer-guide/domain/account.md