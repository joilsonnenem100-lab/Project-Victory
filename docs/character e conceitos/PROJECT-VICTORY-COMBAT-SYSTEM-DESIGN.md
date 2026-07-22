# PROJECT-VICTORY-COMBAT-SYSTEM-DESIGN

## Documento de Visão --- Combat, Damage, Resistance e Resolution Pipeline

Status: - Documento de visão arquitetural - Conceitos alinhados ao
Project-Victory - Não representa implementação concluída - Fórmulas
finais ainda não definidas

------------------------------------------------------------------------

# 1. Filosofia do Combate

O combate do Project-Victory deve evoluir de um resolvedor simples para
um sistema baseado em contexto.

O objetivo não é apenas calcular números, mas resolver uma situação de
combate considerando:

-   personagem;
-   monstro;
-   equipamentos;
-   habilidades;
-   estados;
-   regras de combate.

Fluxo conceitual:

Character State

↓

Combat Context

↓

Combat Rules

↓

Resolution

↓

Combat Result

------------------------------------------------------------------------

# 2. Estado Atual Confirmado

Implementado no Vertical Slice:

-   CombatContext
-   CombatState
-   CombatResolution
-   CombatResolver
-   Integração Hunt → Combat → Result

Ainda não definido:

-   fórmula final de dano;
-   resistências oficiais;
-   atributos completos;
-   balanceamento.

------------------------------------------------------------------------

# 3. Separação de Responsabilidades

CombatResolver não deve possuir todas as regras.

Arquitetura futura:

Combat Resolver

↓

Combat Rules Engine

↓

Damage Calculator

↓

Resistance Calculator

↓

Final Resolution

------------------------------------------------------------------------

# 4. Combat Action Layer

Uma evolução natural:

CombatContext

-   

Combat Action

↓

Resolution

Exemplos:

-   Attack
-   Skill Cast
-   Block
-   Dodge
-   Use Item
-   Flee

------------------------------------------------------------------------

# 5. Damage Pipeline

Modelo conceitual:

Base Damage

↓

Character Modifiers

↓

Skill Modifiers

↓

Equipment Modifiers

↓

Critical Modifiers

↓

Defense

↓

Resistance

↓

Final Damage

------------------------------------------------------------------------

# 6. Dano Bruto e Dano Final

O sistema deve separar:

Raw Damage

de

Final Damage

Exemplo:

Ataque gera dano bruto.

Depois:

-   defesa;
-   resistência;
-   modificadores;

determinam o resultado final.

------------------------------------------------------------------------

# 7. Damage Types

Conceito futuro:

-   Physical
-   Magical
-   Elemental
-   Special

Exemplo:

Espada:

Physical Damage

Magia:

Fire Damage

------------------------------------------------------------------------

# 8. Damage Source

Cada dano deve possuir origem.

Modelo:

Damage Event

-   Source
-   Target
-   Type
-   Amount
-   Tags

Isso permite:

-   logs;
-   efeitos;
-   estatísticas;
-   balanceamento.

------------------------------------------------------------------------

# 9. Resistências

Resistência é uma camada separada de defesa.

Exemplo:

Armor:

↓

Physical Damage

Resistance:

↓

Elemental Damage

------------------------------------------------------------------------

Possíveis resistências:

-   Fire
-   Ice
-   Lightning
-   Poison
-   Shadow

Lista oficial ainda não definida.

------------------------------------------------------------------------

# 10. Mitigação Física

Conceito:

Physical Damage

↓

Armor Calculation

↓

Final Damage

------------------------------------------------------------------------

# 11. Resistência Elemental

Conceito:

Fire Damage

↓

Fire Resistance

↓

Resultado Final

------------------------------------------------------------------------

# 12. Penetração e Vulnerabilidade

Conceito futuro:

Fire Resistance:

50%

-   

Fire Penetration:

20%

=

Resistência efetiva reduzida

------------------------------------------------------------------------

Vulnerabilidade:

Alvo congelado:

↓

Recebe mais dano de fogo

------------------------------------------------------------------------

# 13. Status Effects

Dano pode gerar estados.

Exemplos:

-   Burn
-   Freeze
-   Poison
-   Bleed
-   Stun

Fluxo:

Damage

↓

Effect Check

↓

Apply Status

↓

Update State

------------------------------------------------------------------------

# 14. Character Stats

O combate deve receber o estado final do personagem.

Fluxo correto:

Equipment

↓

Character Stats

↓

Combat

------------------------------------------------------------------------

O Combat não deve consultar diretamente todos os itens.

------------------------------------------------------------------------

# 15. Monster Combat State

Separação:

Monster Definition

↓

Monster Instance

↓

Combat Stats

↓

Combat

------------------------------------------------------------------------

Permite:

-   monstros normais;
-   elites;
-   chefes.

------------------------------------------------------------------------

# 16. Combat Snapshot

Conceito futuro importante.

Ao iniciar combate:

Capture:

-   atacante;
-   defensor;
-   equipamentos;
-   efeitos ativos;
-   atributos.

Evita alterações indevidas durante resolução.

------------------------------------------------------------------------

# 17. Critical System

Conceito futuro:

Critical Chance

-   

Critical Damage

↓

Critical Result

------------------------------------------------------------------------

Fórmula ainda não definida.

------------------------------------------------------------------------

# 18. Item Integration

Fluxo:

Equipment

↓

Character Stats

↓

Combat Calculation

------------------------------------------------------------------------

Itens podem fornecer:

-   dano;
-   defesa;
-   resistências;
-   efeitos.

------------------------------------------------------------------------

# 19. Skill Integration

Fluxo:

Skills

-   

Equipment

-   

Character Stats

↓

Combat Identity

------------------------------------------------------------------------

O objetivo é permitir diferentes builds.

------------------------------------------------------------------------

# 20. Build Identity

Exemplos conceituais:

Tank:

-   Health
-   Defense
-   Block
-   Resistance

Damage:

-   Critical
-   Attack
-   Speed

Magic:

-   Mana
-   Spell Power
-   Elemental Damage

------------------------------------------------------------------------

# 21. Balance Layer

Regras de combate devem ser separadas do código.

Objetivo:

Permitir alterar:

-   valores;
-   multiplicadores;
-   resistência;
-   dano;

sem modificar arquitetura.

------------------------------------------------------------------------

# 22. Combat History

Sistema futuro:

Registrar:

-   ações;
-   danos;
-   efeitos;
-   resultados.

Uso:

-   suporte;
-   análise;
-   balanceamento.

------------------------------------------------------------------------

# 23. Arquitetura Macro

Character State

↓

Attributes

↓

Equipment

↓

Skills

↓

Combat Context

↓

Combat Rules Engine

↓

Damage Calculation

↓

Resistance Calculation

↓

Combat Resolution

↓

Reward System

------------------------------------------------------------------------

# 24. Relação com Item System

Fluxo completo:

Monster

↓

Combat

↓

Reward

↓

Loot

↓

Equipment

↓

Character Build

↓

Combat

------------------------------------------------------------------------

# 25. Estado do Projeto

Oficial:

-   Combat Foundation
-   Combat Context
-   Combat Resolution

Conceitual:

-   Damage Pipeline
-   Resistance System
-   Damage Types
-   Status Effects
-   Critical System

Futuro:

-   Balance Engine
-   Combat Logs
-   Advanced Effects
-   PvP Rules

------------------------------------------------------------------------

# 26. Direção Final

O objetivo do Combat System do Project-Victory é criar uma resolução
baseada em estado.

O resultado de uma luta deve ser consequência de:

-   escolhas do jogador;
-   construção do personagem;
-   equipamentos;
-   habilidades;
-   contexto do combate.

Não apenas um número fixo.
