# Collection System

Version: 1.0

Status: LOCKED

Last Sprint: SPRINT-013

---

# Objetivo

Definir o funcionamento oficial do sistema de Collections do Project-Victory.

---

# Filosofia

No Project-Victory, uma Collection representa diversão e opção de diversão.

Collections são objetivos paralelos que permitem ao jogador escolher novas formas de aproveitar o jogo.

---

# Conceito

Collection é um sistema onde o jogador:

- coleta;
- derrota;
- entrega;
- completa objetivos.

---

# Arquitetura

```
CollectionService

↓

CollectionEntity

↓

CollectionProgress

↓

RewardService
```

---

# Categorias

Categorias iniciais:

- Monstros;
- Itens;
- Equipamentos;
- Recursos;
- Bosses.

---

# Monster Collection

Progresso automático.

Exemplo:

```
Dragon derrotado:

+1 Dragon Collection
```

Não existe entrega de item.

---

# Item Collection

Pode solicitar itens específicos.

Exemplo:

```
Entregar:

100 Dragon Leather

↓

Collection Complete
```

Itens entregues são destruídos.

---

# Equipamentos

Itens de raridades diferentes contam para a Collection base.

Exemplo:

```
Fire Sword

Normal

Magic

Rare
```

Todos contam como Fire Sword.

---

# Recompensas

Collections podem fornecer:

- Gold;
- XP;
- Títulos;
- Cosméticos;
- Bônus temporários;
- Atributos permanentes.

---

# Collections Únicas

Podem existir registros únicos.

Exemplo:

```
Primeiro jogador a completar Dragon Collection
```

---

# Visibilidade

O jogador controla quais Collections serão mostradas publicamente.

---

# Relação com Achievement

São sistemas separados:

```
Achievement

"Eu conquistei algo"


Collection

"Eu reuni algo"
```

---

# Pendências futuras

Definir:

- Interface;
- Quantidade de Collections iniciais;
- Recompensas individuais;
- Balanceamento;
- Collections de eventos.