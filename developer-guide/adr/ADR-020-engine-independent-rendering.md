# ADR-020 - Engine Independent Rendering

Status: ACCEPTED

Version: 1.0

---

# Contexto

A arquitetura do Project-Victory prioriza o domínio como principal ativo do projeto.

A interface deverá evoluir ao longo do tempo sem impactar a Engine.

---

# Decisão

Toda renderização será considerada uma camada consumidora da Engine.

A Engine nunca dependerá da forma como o jogo é apresentado.

O mesmo estado poderá ser representado por:

- interface textual;
- imagens;
- interface web;
- cliente desktop;
- cliente mobile;
- futuras representações gráficas.

Sem qualquer alteração no domínio.

---

# Consequências

Benefícios:

- evolução independente da interface;
- reutilização da Engine;
- facilidade para novos clientes;
- maior longevidade da arquitetura.

Custos:

- necessidade de adaptação da camada de apresentação para cada cliente.

---

# Aplicações

Este princípio aplica-se a toda a arquitetura do Project-Victory.

---

# Restrições

Nenhum módulo do domínio poderá depender de:

- renderização;
- animações;
- coordenadas gráficas;
- componentes da interface.

Toda dependência ocorre no sentido inverso.

---

# Referências

Ver também:

- developer-guide/domain/maps.md
- developer-guide/domain/hunt.md
- developer-guide/domain/cities.md