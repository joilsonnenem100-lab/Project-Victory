PROJECT-VICTORY-ENGINEERING-HANDOFF
Versão do Documento
Versão: 1.1
Status: Oficial
Responsável: Arquitetura
Projeto: Project-Victory
1. Objetivo deste documento
Este documento representa o contexto operacional oficial para a Equipe de Engenharia do Project-Victory.

Ele permite continuidade entre sessões sem depender do histórico de conversas.

Não substitui contratos técnicos, código fonte ou documentação arquitetural oficial.
2. Responsabilidades
Arquitetura:
- Define arquitetura.
- Cria e valida contratos técnicos.
- Aprova escopo.
- Resolve conflitos arquiteturais.
- Valida entregas.

Equipe de Engenharia:
- Implementa contratos aprovados.
- Preserva arquitetura existente.
- Entrega código funcional.
- Executa validações técnicas.

A Engenharia não cria decisões arquiteturais.
3. Regra principal de implementação
Nenhuma implementação inicia em nova conversa sem validação de contexto.

Antes de criar ou modificar arquivos, a Engenharia deve confirmar entendimento da filosofia do jogo, estado atual, objetivo e limites do Work Item.

A Engenharia implementa somente contratos aprovados pela Arquitetura.
4. Fluxo obrigatório
1. Receber snapshot quando necessário.
2. Ler documentos obrigatórios.
3. Confirmar entendimento.
4. Analisar contrato.
5. Implementar somente o autorizado.
6. Validar.
7. Entregar.
5. Documentos obrigatórios
1. PROJECT-VICTORY-CONTEXT-HANDOFF.md
2. PROJECT-VICTORY-GAME-PHILOSOPHY.md
3. PROJECT-VICTORY-ENGINEERING-HANDOFF.md
4. Documentação arquitetural relacionada
5. Contrato técnico aprovado
6. Limites da Engenharia
A Engenharia possui autonomia técnica dentro do contrato.

Não pode alterar direção do produto, criar sistemas de gameplay sem aprovação, substituir funcionalidades existentes sem autorização ou alterar arquitetura principal fora do escopo.
7. Regra de preservação
Implementar significa adicionar ou evoluir uma capacidade aprovada.

Não significa remover ou substituir algo existente que já funciona sem decisão arquitetural explícita.

# Regra de eficiência de validação

Antes de iniciar uma validação automatizada, a Engenharia deve avaliar se uma validação manual será mais rápida, confiável e suficiente para o objetivo.

Quando uma validação manual for claramente mais eficiente, ela deve ser priorizada.

Exemplos de validação manual:

- verificar fluxo de telas;
- testar navegação;
- confirmar comportamento visual;
- validar experiência do jogador;
- confirmar integração simples já existente.

Exemplos onde validação técnica deve ser utilizada:

- alterações arquiteturais;
- problemas de compilação;
- dependências;
- persistência;
- contratos entre sistemas;
- análise de impacto.

A Engenharia deve informar quando optar por validação manual e registrar o resultado.
