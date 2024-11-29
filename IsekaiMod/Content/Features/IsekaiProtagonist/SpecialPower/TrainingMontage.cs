using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;
using static IsekaiMod.Main;
using Kingmaker.UnitLogic.Abilities.Blueprints;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.SpecialPower {

    internal class TrainingMontage {
        private static readonly Sprite Icon_LegendaryProportions = BlueprintTools.GetBlueprint<BlueprintAbility>("da1b292d91ba37948893cdbe9ea89e28").m_Icon;

        public static void Add() {
            // Create the Training Montage feature
            var TrainingMontage = Helpers.CreateBlueprint<BlueprintFeature>(IsekaiContext, "TrainingMontage", bp => {
                bp.SetName(IsekaiContext, "Training Montage");
                bp.SetDescription(IsekaiContext, "Through relentless training and an unwavering desire to better yourself, you gain a +2 bonus to all attributes at level 1, increasing by +1 every 4 levels (to a maximum of +8 at level 20).");
                bp.m_Icon = Icon_LegendaryProportions;

                // Initial +2 bonus to all stats
                foreach (StatType stat in new[] {
                    StatType.Strength, StatType.Dexterity, StatType.Constitution,
                    StatType.Intelligence, StatType.Wisdom, StatType.Charisma
                }) {
                    bp.AddComponent<AddStatBonus>(c => {
                        c.Descriptor = ModifierDescriptor.UntypedStackable;
                        c.Stat = stat;
                        c.Value = 2; // Starting bonus
                    });
                }

                // Scaling bonuses based on character level
                foreach (StatType stat in new[] {
                    StatType.Strength, StatType.Dexterity, StatType.Constitution,
                    StatType.Intelligence, StatType.Wisdom, StatType.Charisma
                }) {
                    bp.AddComponent<ScalingStatBonus>(c => {
                        c.Stat = stat;
                        c.LevelDivisor = 4; // Gain +1 every 4 levels
                        c.InitialValue = 2; // Initial +2
                        c.MaxBonus = 8; // Max bonus at level 20
                    });
                }
            });

            // Add Training Montage to the Special Power selection
            SpecialPowerSelection.AddToSelection(TrainingMontage);
        }
    }

    // Custom component for scaling bonuses
    public class ScalingStatBonus : UnitFactComponentDelegate {
        public StatType Stat;
        public int LevelDivisor = 4; // Divisor for character level scaling
        public int InitialValue = 2; // Initial bonus value
        public int MaxBonus = 8; // Maximum bonus

        public override void OnActivate() {
            ApplyScalingBonus();
        }

        public override void OnDeactivate() {
            RemoveScalingBonus();
        }

        private void ApplyScalingBonus() {
            int levelBonus = Owner.Progression.CharacterLevel / LevelDivisor;
            int totalBonus = Mathf.Min(InitialValue + levelBonus, MaxBonus);

            Owner.Stats.GetStat(Stat).AddModifier(totalBonus, Runtime, ModifierDescriptor.UntypedStackable);
        }

        private void RemoveScalingBonus() {
            Owner.Stats.GetStat(Stat).RemoveModifiersFrom(Runtime);
        }
    }
}
