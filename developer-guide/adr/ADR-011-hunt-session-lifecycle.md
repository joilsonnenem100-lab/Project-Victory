# ADR-011 - Hunt Session Lifecycle

Status: ACCEPTED

Version: 1.0

---

# Contexto

O módulo Hunt é responsável por coordenar toda a execução de uma Hunt.

Durante o levantamento arquitetural foi identificado que a configuração permanente de uma Hunt e sua execução temporária representam responsabilidades distintas.

Misturar esses conceitos aumentaria o acoplamento e dificultaria a evolução da Engine.

---

# Decisão

A arquitetura passa a possuir dois conceitos independentes.

## Hunt

Representa a definição permanente do conteúdo.

Contém apenas configurações.

## Hunt Session

Representa exclusivamente a execução temporária de uma Hunt.

Uma Hunt Session:

- nasce quando uma Hunt inicia;
- coordena a execução da Hunt;
- existe apenas durante a Hunt ativa;
- é destruída imediatamente após o encerramento.

Nunca será persistida como entidade permanente.

---

# Consequências

Benefícios:

- separação clara entre configuração e execução;
- redução de acoplamento;
- menor consumo de memória;
- maior facilidade para testes;
- recuperação simples após falhas;
- arquitetura consistente com os demais módulos.

Custos:

- criação de uma nova Hunt Session a cada Hunt iniciada.

---

# Aplicações

Este princípio aplica-se a:

- Hunt;
- Spawn System;
- Combat Engine;
- Reward Pipeline.

---

# Restrições

A Hunt Session nunca poderá:

- sobreviver ao encerramento da Hunt;
- ser reutilizada por outra Hunt;
- ser restaurada após desconexão;
- ser persistida como dado permanente.

---

# Referências

Ver também:

- developer-guide/domain/hunt.md
- developer-guide/domain/spawn-system.md