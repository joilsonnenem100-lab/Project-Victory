# Domain Events

Status: LOCKED

Version: 1.0

---

# Objetivo

Este documento define os Domain Events oficiais do Project-Victory.

Domain Events representam acontecimentos relevantes do domínio.

Eles permitem desacoplamento entre Aggregates e demais componentes do sistema.

---

# Filosofia

Um Domain Event representa um fato ocorrido.

Eventos descrevem algo que aconteceu.

Nunca algo que irá acontecer.

---

# CharacterCreated

Disparado quando um novo Character é criado.

---

# CharacterDeleted

Disparado quando um Character é removido.

---

# CharacterLeveledUp

Disparado quando o Character sobe de nível.

---

# CharacterDied

Disparado quando o Character morre.

---

# CharacterRespawned

Disparado quando o Character retorna ao mundo.

---

# ExperienceGained

Disparado quando experiência é recebida.

---

# ItemObtained

Disparado quando um Item entra no inventário.

---

# ItemRemoved

Disparado quando um Item deixa o inventário.

---

# EquipmentChanged

Disparado quando equipamentos são alterados.

---

# MonsterSpawned

Disparado quando um Monster é criado pelo Runtime.

---

# MonsterKilled

Disparado quando um Monster morre.

---

# LootGenerated

Disparado quando o Loot é gerado.

---

# QuestAccepted

Disparado quando uma Quest é aceita.

---

# QuestCompleted

Disparado quando uma Quest é concluída.

---

# Regras

Eventos são imutáveis.

Eventos nunca alteram o domínio.

Eventos apenas notificam acontecimentos.

---

# Proibições

É proibido:

- executar regras de negócio dentro do evento;
- acessar banco de dados;
- acessar infraestrutura;
- modificar Aggregates.

---

# Filosofia Final

Domain Events reduzem acoplamento.

Representam fatos consumados do domínio.

Toda comunicação entre partes independentes do domínio deve preferencialmente ocorrer através de eventos.