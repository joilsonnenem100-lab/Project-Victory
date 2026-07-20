# PV-VS-2.0-UPDATE-002 — Web Account Registration Flow

Status: READY FOR ENGINEERING

## Objetivo
Permitir criação de conta pelo navegador utilizando a infraestrutura Account existente.

## Fluxo
Usuário
↓
Frontend Register Page
↓
Account API
↓
Persistence
↓
Conta criada

## Escopo
Implementar:
- Tela de cadastro web.
- Envio de dados para API Account existente.
- Feedback de sucesso e erro.
- Persistência usando fluxo existente.

## Restrições
Não alterar:
- Domain Account.
- Authentication/JWT.
- Character.
- Gameplay.
- Persistência existente sem necessidade.

## Critérios de Aceite
- Usuário acessa cadastro.
- Cadastro envia dados para API.
- Conta é criada com sucesso.
- Dados persistem.
- Fluxo existente de login permanece funcional.
