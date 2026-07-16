using System;
using ProjectVictory.Domain.Account;
using ProjectVictory.Domain.Primitives;
using ProjectVictory.Domain.ValueObjects;
using CharacterExperience = ProjectVictory.Domain.ValueObjects.Experience;
using CharacterLevel = ProjectVictory.Domain.ValueObjects.Level;

namespace ProjectVictory.Domain.Entities
{
    public sealed class Character : AggregateRoot
    {
        public AccountId AccountId { get; private set; }

        public string Name { get; private set; }

        public Health Health { get; private set; }

        public Mana Mana { get; private set; }

        public CharacterExperience Experience { get; private set; }

        public CharacterLevel Level { get; private set; }

        public Inventory Inventory { get; }

        public Equipment Equipment { get; }

        public Attributes Attributes { get; }

        public Skills Skills { get; }

        public Character(
            Guid id,
            AccountId accountId,
            string name,
            Health health,
            Mana mana,
            CharacterExperience experience,
            CharacterLevel level)
            : base(id)
        {
            ArgumentNullException.ThrowIfNull(accountId);
            ArgumentNullException.ThrowIfNull(health);
            ArgumentNullException.ThrowIfNull(mana);
            ArgumentNullException.ThrowIfNull(experience);
            ArgumentNullException.ThrowIfNull(level);

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException(
                    "Character name is required.",
                    nameof(name));
            }

            AccountId = accountId;
            Name = name;
            Health = health;
            Mana = mana;
            Experience = experience;
            Level = level;

            Inventory = new Inventory();
            Equipment = new Equipment();
            Attributes = new Attributes();
            Skills = new Skills();
        }

        public void Heal(Health health)
        {
            ArgumentNullException.ThrowIfNull(health);

            Health = health;
        }

        public void SpendMana(Mana mana)
        {
            ArgumentNullException.ThrowIfNull(mana);

            Mana = mana;
        }

        public void GainExperience(CharacterExperience experience)
        {
            ArgumentNullException.ThrowIfNull(experience);

            Experience = experience;
        }

        public EvolutionValidationResult ValidateEvolution()
        {
            if (Id == Guid.Empty ||
                Experience is null ||
                Level is null)
            {
                return new EvolutionValidationResult(
                    Id,
                    false);
            }

            return new EvolutionValidationResult(
                Id,
                true);
        }

        public void ApplyEvolution()
        {
            var validation = ValidateEvolution();

            if (!validation.Accepted)
            {
                throw new InvalidOperationException(
                    "Character evolution validation failed.");
            }
        }

        public LevelValidationResult ValidateLevel(
            int requestedLevel)
        {
            if (Id == Guid.Empty)
            {
                return new LevelValidationResult(
                    Id,
                    false);
            }

            if (requestedLevel < 1)
            {
                return new LevelValidationResult(
                    Id,
                    false);
            }

            if (Level is null)
            {
                return new LevelValidationResult(
                    Id,
                    false);
            }

            return new LevelValidationResult(
                Id,
                true);
        }

        public void ApplyLevel(
            int requestedLevel)
        {
            var validation = ValidateLevel(
                requestedLevel);

            if (!validation.Accepted)
            {
                throw new InvalidOperationException(
                    "Character level validation failed.");
            }

            Level = new CharacterLevel(
                requestedLevel);
        }
    }
}