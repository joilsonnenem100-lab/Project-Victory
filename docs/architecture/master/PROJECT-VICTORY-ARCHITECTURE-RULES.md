---

# Evolution Rule — Additive Development Principle

Status: LOCKED

## Objetivo

Garantir que novas implementações evoluam o Project-Victory sem remover ou degradar funcionalidades existentes.

---

# Regra principal

Uma implementação deve adicionar capacidades ao sistema existente.

Ela nunca deve substituir, remover ou quebrar um comportamento validado sem uma decisão arquitetural explícita.

---

# Princípio

Antes:

Sistema funcional

↓

Nova funcionalidade

↓

Depois:

Sistema funcional + nova funcionalidade

---

# Proibições

Não é permitido:

- substituir uma tela funcional por uma versão incompleta;
- substituir serviços existentes ignorando métodos já utilizados;
- remover integrações existentes para implementar uma nova;
- alterar contratos existentes sem autorização;
- recriar sistemas paralelos quando já existe infraestrutura;
- simplificar removendo comportamentos existentes.

---

# Regra de Frontend

Ao adicionar uma nova interface:

Obrigatório preservar:

- páginas existentes;
- navegação existente;
- chamadas de API existentes;
- estados já exibidos;
- fluxos já validados.

Uma nova UI deve ser incorporada ao frontend atual.

Ela não deve apagar telas funcionais para criar uma nova estrutura.

---

# Regra de Backend

Ao adicionar uma capacidade:

Obrigatório:

- manter endpoints existentes;
- manter DTOs utilizados;
- manter handlers existentes;
- manter persistência existente.

Novas responsabilidades devem ser adicionadas através de extensão.

---

# Validação obrigatória

Antes de concluir uma implementação:

Verificar:

- funcionalidades anteriores continuam funcionando;
- builds continuam passando;
- fluxos validados continuam disponíveis;
- nenhuma funcionalidade foi removida.

---

# Exceção

Uma remoção ou substituição somente pode ocorrer quando:

1. existe decisão arquitetural documentada;
2. existe contrato autorizando;
3. existe validação do impacto.