# PV-FRONTEND-GAME-HUD-FOUNDATION-001

Status: Active

Version: 1.0

---

# Objetivo

Criar a fundação inicial do Game HUD do Project-Victory.

O Game HUD será a principal interface do jogador durante a experiência de Hunt.

Esta implementação deve criar uma estrutura contínua de gameplay, evitando arquitetura baseada em páginas isoladas para cada sistema.

---

# Contexto de Produto

O Project-Victory possui uma filosofia de jogo baseada em:

- Hunt como atividade principal.
- Combate automático.
- Simulação visual da execução.
- Resultado da Hunt.
- Drops.
- Evolução através de itens e progressão.

O jogador não controla ações durante o combate.

O frontend deve representar a simulação e seus resultados.

---

# Fluxo Principal

Fluxo oficial:

Login

↓

Character Selection

↓

Game HUD

↓

Iniciar Hunt

↓

Execução automática

↓

Resultado

↓

Drops

↓

Inventário

---

# Responsabilidade do Game HUD

O Game HUD representa o ambiente permanente do jogador durante a sessão.

Não deve existir:

- HuntPage como tela principal.
- CombatPage como tela principal.
- RewardPage como tela principal.

Estados devem existir dentro do Game HUD.

---

# Estados suportados

## Idle

Estado inicial após entrada no Game HUD.

Responsabilidade:

Exibir:

- personagem selecionado;
- status atual;
- preparação para Hunt.

---

## Hunting

Estado durante execução da Hunt.

Responsabilidade:

Exibir:

- Hunt em execução;
- estado atual da atividade;
- informações disponíveis pela API existente.

---

## Combat Simulation

Estado visual da simulação de combate.

Responsabilidade:

Exibir:

- encontro encontrado;
- informações disponíveis do combate;
- resultado da simulação.

Não implementar:

- combate manual;
- ações de ataque pelo jogador;
- habilidades clicáveis.

---

## Reward

Estado de resultado da Hunt.

Responsabilidade:

Exibir:

- resultado da execução;
- experiência recebida;
- recompensas disponíveis.

---

# Estrutura visual esperada

## Área superior

Responsabilidade:

Exibir informações permanentes do personagem.

Exemplos:

- Nome;
- Level;
- HP;
- Mana;
- Experience.

Utilizar somente dados existentes.

Não criar dados fictícios.

---

# Área principal central

Responsabilidade:

Representar o contexto atual da Hunt.

Deve suportar:

- Idle;
- Hunting;
- Combat Simulation;
- Reward.

Esta área não representa um sistema separado.

---

# Painel direito do jogador

Área permanente da experiência.

Responsabilidade:

Preparar a apresentação dos sistemas de personagem.

---

## Equipment

Estado inicial:

Somente estrutura visual reservada.

Exibir indicação de sistema futuro.

Não implementar:

- equipar item;
- remover item;
- atributos derivados;
- regras de equipamento.

---

## Inventory

Estado inicial:

Somente estrutura visual reservada.

Exibir indicação de sistema futuro.

Objetivo futuro:

Receber e apresentar drops obtidos durante Hunt.

Não implementar:

- gerenciamento completo de inventário;
- organização avançada;
- regras de item.

---

# Controles da Hunt

Adicionar controle para iniciar Hunt.

Fluxo:

Jogador entra no Game HUD.

↓

Clica iniciar Hunt.

↓

Frontend utiliza serviços/API existentes.

↓

Resultado é apresentado no HUD.

---

# Log de eventos

Preparar área para eventos importantes.

Exemplos:

- início da Hunt;
- encontro encontrado;
- resultado;
- recompensa.

Não implementar sistema completo de chat.

---

# Restrições

Não alterar:

- Backend.
- Domínio.
- Persistência.
- Contratos existentes.
- APIs existentes.

Não criar:

- novos sistemas de gameplay;
- novos modelos de domínio;
- combate manual;
- progressão nova;
- equipamentos funcionais;
- inventário funcional completo.

---

# Regra obrigatória

Seguir:

Adicionar > Substituir.

Nenhuma funcionalidade validada existente pode ser removida.

O Game HUD deve ser adicionado preservando:

- Login;
- Registro;
- Character Selection;
- Fluxos existentes.

---

# Autonomia da Engenharia

A Engenharia pode:

- criar componentes internos necessários;
- organizar arquivos conforme padrões existentes;
- criar abstrações internas de frontend.

A Engenharia não deve:

- criar novas decisões de produto;
- criar novos sistemas de gameplay;
- alterar arquitetura sem aprovação.

---

# Validação obrigatória

Confirmar:

## Fluxo

- Login continua funcionando.
- Character Selection continua funcionando.
- Entrada no Game HUD funciona.
- Hunt pode ser iniciada.

## Visual

- HUD principal renderiza.
- Área central renderiza estados.
- Painel direito aparece.
- Inventory reservado aparece.
- Equipment reservado aparece.

## Integração

- Dados exibidos vêm de fontes existentes.
- Nenhum dado fictício é criado para simular sistemas inexistentes.

---

# Entrega esperada

A primeira entrega deve ser uma fundação funcional do Game HUD.

Não é a versão final do gameplay.

Ela prepara o ambiente onde futuras evoluções serão adicionadas:

- Inventory real;
- Equipment;
- Loot completo;
- Progression;
- Skills;
- Cidade;
- Mundo.