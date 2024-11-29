using IsekaiMod.Utilities;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.Utility;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility {

    internal class PerfectRoll {
        public static void Add() {
            const string PerfectRollDesc = "You navigate every conversation and fight with perfect accuracy. "
                + "Every word is predicted; every action is foreseen. "
                + "Your premonitions guide you on your quest, as if you have experienced this before..."
                + "\nBenefit: You gain a +5 bonus to all d20 rolls.";

            var Icon_TrickFate = BlueprintTools.GetBlueprint<BlueprintAbility>("6e109d21da9e1c44fb772a9eca2cafdd").m_Icon;

            // Create the feature using CreateBlueprint
            var PerfectRollFeature = Helpers.CreateBlueprint<BlueprintFeature>(
                IsekaiMod.Main.IsekaiContext,
                "PerfectRollFeature",
                bp => {
                    bp.SetName(IsekaiMod.Main.IsekaiContext, "Overpowered Ability — Perfect Roll");
                    bp.SetDescription(IsekaiMod.Main.IsekaiContext, PerfectRollDesc);
                    bp.m_Icon = Icon_TrickFate;

                    // Add ModifyD20 component for a +5 bonus
                    bp.AddComponent<ModifyD20>(c => {
                        c.Rule = RuleType.All; // Applies to all d20 rolls
                        c.AddBonus = true; // Enable adding a bonus
                        c.Bonus = new Kingmaker.UnitLogic.Mechanics.ContextValue() {
                            ValueType = Kingmaker.UnitLogic.Mechanics.ContextValueType.Simple,
                            Value = 5 // Flat +5 bonus
                        };
                        c.BonusDescriptor = ModifierDescriptor.UntypedStackable; // Untyped bonus, stacks with other bonuses
                    });
                });

            // Add the feature to the Overpowered Ability selection
            OverpoweredAbilitySelection.AddToSelection(PerfectRollFeature);
        }
    }
}

