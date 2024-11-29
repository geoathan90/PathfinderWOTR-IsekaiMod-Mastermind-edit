using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;
using static IsekaiMod.Main;
using static UnityEngine.UI.GridLayoutGroup;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility {

    internal class SupremeBeing {
        private static readonly Sprite Icon_KiPowerSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("3049386713ff04245a38b32483362551").m_Icon;

        public static void Add() {
            var SupremeBeing = Helpers.CreateBlueprint<BlueprintFeature>(IsekaiContext, "SupremeBeing", bp => {
                bp.SetName(IsekaiContext, "Overpowered Ability — Supreme Being");
                bp.SetDescription(IsekaiContext, "You have transcended mortal limitations, achieving perfection in body and mind. "
                    + "\nBenefit: Gain an initial +5 bonus to all attributes. Additionally, gain +1 to all attributes for every 2 character levels and +1 for every mythic rank.");
                bp.m_Icon = Icon_KiPowerSelection;

                // Initial +5 bonus to all stats
                bp.AddComponent<AddStatBonus>(c => {
                    c.Stat = StatType.Strength;
                    c.Value = 5;
                    c.Descriptor = Kingmaker.Enums.ModifierDescriptor.UntypedStackable;
                });
                bp.AddComponent<AddStatBonus>(c => {
                    c.Stat = StatType.Dexterity;
                    c.Value = 5;
                    c.Descriptor = Kingmaker.Enums.ModifierDescriptor.UntypedStackable;
                });
                bp.AddComponent<AddStatBonus>(c => {
                    c.Stat = StatType.Constitution;
                    c.Value = 5;
                    c.Descriptor = Kingmaker.Enums.ModifierDescriptor.UntypedStackable;
                });
                bp.AddComponent<AddStatBonus>(c => {
                    c.Stat = StatType.Intelligence;
                    c.Value = 5;
                    c.Descriptor = Kingmaker.Enums.ModifierDescriptor.UntypedStackable;
                });
                bp.AddComponent<AddStatBonus>(c => {
                    c.Stat = StatType.Wisdom;
                    c.Value = 5;
                    c.Descriptor = Kingmaker.Enums.ModifierDescriptor.UntypedStackable;
                });
                bp.AddComponent<AddStatBonus>(c => {
                    c.Stat = StatType.Charisma;
                    c.Value = 5;
                    c.Descriptor = Kingmaker.Enums.ModifierDescriptor.UntypedStackable;
                });

                // Scaling bonuses based on level and mythic rank
                bp.AddComponent<ScalingStatBonus>(c => {
                    c.Stat = StatType.Strength;
                    c.LevelDivisor = 2; // Gain +1 per 2 levels
                    c.MythicMultiplier = 1; // Gain +1 per mythic rank
                });
                bp.AddComponent<ScalingStatBonus>(c => {
                    c.Stat = StatType.Dexterity;
                    c.LevelDivisor = 2;
                    c.MythicMultiplier = 1;
                });
                bp.AddComponent<ScalingStatBonus>(c => {
                    c.Stat = StatType.Constitution;
                    c.LevelDivisor = 2;
                    c.MythicMultiplier = 1;
                });
                bp.AddComponent<ScalingStatBonus>(c => {
                    c.Stat = StatType.Intelligence;
                    c.LevelDivisor = 2;
                    c.MythicMultiplier = 1;
                });
                bp.AddComponent<ScalingStatBonus>(c => {
                    c.Stat = StatType.Wisdom;
                    c.LevelDivisor = 2;
                    c.MythicMultiplier = 1;
                });
                bp.AddComponent<ScalingStatBonus>(c => {
                    c.Stat = StatType.Charisma;
                    c.LevelDivisor = 2;
                    c.MythicMultiplier = 1;
                });
            });

            OverpoweredAbilitySelection.AddToSelection(SupremeBeing);
        }
    }

    // Custom component for scaling bonuses
    // Custom component for scaling bonuses
    // Custom component for scaling bonuses
    public class ScalingStatBonus : UnitFactComponentDelegate {
        public StatType Stat;
        public int LevelDivisor = 1; // Divisor for character level scaling
        public int MythicMultiplier = 0; // Multiplier for mythic rank scaling

        // Ensure method visibility matches the base class
        public override void OnActivate() {
            ApplyScalingBonus();
        }

        public override void OnDeactivate() {
            RemoveScalingBonus();
        }

        private void ApplyScalingBonus() {
            // Calculate scaling bonuses
            int characterLevelBonus = Owner.Progression.CharacterLevel / LevelDivisor;
            int mythicRankBonus = Owner.Progression.MythicLevel * MythicMultiplier;
            int totalBonus = characterLevelBonus + mythicRankBonus;

            // Apply the bonus
            Owner.Stats.GetStat(Stat).AddModifier(totalBonus, Runtime, Kingmaker.Enums.ModifierDescriptor.UntypedStackable);
        }

        private void RemoveScalingBonus() {
            // Remove all bonuses applied by this component
            Owner.Stats.GetStat(Stat).RemoveModifiersFrom(Runtime);
        }
    }

}