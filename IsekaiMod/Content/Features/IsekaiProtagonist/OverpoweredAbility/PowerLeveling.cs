using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.ElementsSystem;
using Kingmaker.Localization;
using Kingmaker.ResourceLinks;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.AreaEffects;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.Utility;
using TabletopTweaks.Core.Utilities;
using UnityEngine;
using static IsekaiMod.Main;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility {

    internal class PowerLeveling {

        public static void Add() {
            // Icon
            Sprite Icon_DimensionalAnchor = BlueprintTools.GetBlueprint<BlueprintAbility>("c0aa77246b26433fa79c8ac09b1e70d9").m_Icon;

            // Name and Description
            var PowerLevelingName = Helpers.CreateString(IsekaiContext, "PowerLeveling.Name", "Overpowered Ability — Power Leveling");
            var PowerLevelingDesc = Helpers.CreateString(IsekaiContext, "PowerLeveling.Description",
                "You are Overpowered but overly cautious. You use overwhelming force to ensure all enemies you kill are thoroughly defeated."
                + "\nBenefit: When enemies are defeated within a 120-foot radius of you, your party gains temporary bonuses to their attack and saving throws.");

            // Temporary Buff
            var PowerLevelingTempBuff = Helpers.CreateBlueprint<BlueprintBuff>(IsekaiContext, "PowerLevelingTempBuff", bp => {
                bp.SetName(IsekaiContext, "Overpowered Surge");
                bp.SetDescription(IsekaiContext, "Defeating enemies grants temporary bonuses to the party.");
                bp.m_Icon = Icon_DimensionalAnchor;
                bp.IsClassFeature = true;
                bp.AddComponent<AddStatBonus>(c => {
                    c.Stat = Kingmaker.EntitySystem.Stats.StatType.AdditionalAttackBonus;
                    c.Value = 2; // +2 attack bonus
                    c.Descriptor = Kingmaker.Enums.ModifierDescriptor.UntypedStackable;
                });
                bp.AddComponent<AddStatBonus>(c => {
                    c.Stat = Kingmaker.EntitySystem.Stats.StatType.SaveFortitude;
                    c.Value = 2; // +2 Fortitude save bonus
                    c.Descriptor = Kingmaker.Enums.ModifierDescriptor.UntypedStackable;
                });
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi; // Hidden in UI
            });

            // Area Effect
            var PowerLevelingAura = Helpers.CreateBlueprint<BlueprintAbilityAreaEffect>(IsekaiContext, "PowerLevelingAura", bp => {
                bp.m_TargetType = BlueprintAbilityAreaEffect.TargetType.Ally;
                bp.SpellResistance = false;
                bp.AggroEnemies = false;
                bp.AffectEnemies = false;
                bp.Shape = AreaEffectShape.Cylinder;
                bp.Size = new Feet(120);
                bp.Fx = new PrefabLink();
                bp.AddComponent<AbilityAreaEffectBuff>(c => {
                    c.m_Buff = PowerLevelingTempBuff.ToReference<BlueprintBuffReference>();
                    c.Condition = new ConditionsChecker { Conditions = new Condition[0] }; // Apply unconditionally
                });
            });

            // Aura Buff
            var PowerLevelingAreaBuff = Helpers.CreateBlueprint<BlueprintBuff>(IsekaiContext, "PowerLevelingAreaBuff", bp => {
                bp.SetName(PowerLevelingName);
                bp.SetDescription(PowerLevelingDesc);
                bp.m_Icon = Icon_DimensionalAnchor;
                bp.IsClassFeature = true;
                bp.AddComponent<AddAreaEffect>(c => {
                    c.m_AreaEffect = PowerLevelingAura.ToReference<BlueprintAbilityAreaEffectReference>();
                });
            });

            // Final Feature
            var PowerLevelingFeature = Helpers.CreateBlueprint<BlueprintFeature>(IsekaiContext, "PowerLevelingFeature", bp => {
                bp.SetName(PowerLevelingName);
                bp.SetDescription(PowerLevelingDesc);
                bp.m_Icon = Icon_DimensionalAnchor;
                bp.AddComponent<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] { PowerLevelingAreaBuff.ToReference<BlueprintUnitFactReference>() };
                });
            });

            OverpoweredAbilitySelection.AddToSelection(PowerLevelingFeature);
        }
    }
}