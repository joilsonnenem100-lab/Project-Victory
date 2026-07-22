# PROJECT-VICTORY --- SYSTEM RESPONSIBILITY MATRIX

  Sistema          Responsabilidade         Não deve controlar
  ---------------- ------------------------ ------------------------------
  Account          Identidade externa       Gameplay
  Authentication   Validação de acesso      Progressão
  Character        Estado permanente        Combat atual
  Hunt             Atividade de caça        Regras internas de Character
  Combat           Resolver combate         Reward e Economy
  Reward           Distribuir recompensas   Alterar combate
  Progression      Evoluir Character        Criar encontros
  Runtime          Orquestrar execução      Regras de domínio

------------------------------------------------------------------------

Princípio:

Cada sistema possui uma responsabilidade clara.
