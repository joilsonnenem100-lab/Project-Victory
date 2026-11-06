using ProjectVictory.Domain.Reward;

namespace ProjectVictory.Application.Reward;

// PV-REWARD-010: configuração do Reward Pipeline lida só no servidor (DEC-20261106-12):
// chave "Progression:ExperienceRate" (appsettings, user-secrets ou variável de ambiente).
// Nenhum endpoint lê ou altera este valor no M2; a tela de administração é o PV-ADMIN-010 (M8).
public sealed class RewardSettings
{
    public const string ExperienceRateKey = "Progression:ExperienceRate";

    public ExperienceRate ExperienceRate { get; }

    public RewardSettings(ExperienceRate experienceRate)
    {
        ExperienceRate = experienceRate ??
            throw new ArgumentNullException(nameof(experienceRate));
    }
}
