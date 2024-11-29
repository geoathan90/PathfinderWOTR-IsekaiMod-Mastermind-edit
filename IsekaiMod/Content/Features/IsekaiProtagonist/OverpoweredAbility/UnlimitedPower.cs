using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Designers.EventConditionActionSystem.Evaluators;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Localization;
using Kingmaker.ResourceLinks;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using TabletopTweaks.Core.Utilities;
using UnityEngine;
using static IsekaiMod.Main;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility {

    internal class UnlimitedPower {
        private const string Name = "Overpowered Ability — Unlimited Power";
        private static readonly LocalizedString Description = Helpers.CreateString(IsekaiContext, "UnlimitedPower.Description",
            "On the brink of defeat, the enemies have surrounded and exhausted you. Just as they are about to deliver the finishing blow, "
            + "you get up and say the following words: Not today.\n"
            + "Benefit: As a standard action, you restore all of your spell slots. You gain an initial 1 use at level 1, scaling to a maximum of 5 uses by level 20 (gaining an additional use every 4 levels).");

        private static readonly Sprite Icon_UnlimitedPower = AssetLoader.LoadInternal(IsekaiContext, "Features", "ICON_UNLIMITED_POWER.png");

        public static void Add() {
            // Create the ability resource with level-based scaling
            var UnlimitedPowerResource = Helpers.CreateBlueprint<BlueprintAbilityResource>(IsekaiContext, "UnlimitedPowerResource", bp => {
                bp.m_MaxAmount = new BlueprintAbilityResource.Amount {
                    BaseValue = 1, // Start with 1 use
                    IncreasedByLevelStartPlusDivStep = true,
                    StartingLevel = 1, // Start scaling from level 1
                    StartingIncrease = 1, // Initial bonus
                    LevelStep = 4, // Gain 1 use every 4 levels
                    PerStepIncrease = 1, // Each step grants 1 use
                    MinClassLevelIncrease = 0, // Minimum is 0 if level is below StartingLevel
                };
                bp.m_UseMax = true; // Enforce a maximum cap
                bp.m_Max = 5; // Limit to 5 max uses
            });

            // Create the ability
            var UnlimitedPowerAbility = Helpers.CreateBlueprint<BlueprintAbility>(IsekaiContext, "UnlimitedPowerAbility", bp => {
                bp.SetName(IsekaiContext, Name);
                bp.SetDescription(Description);
                bp.AddComponent<AbilityEffectRunAction>(c => {
                    c.Actions = Helpers.CreateActionList(
                        new ContextActionRestoreAllSpellSlots() {
                            m_Target = new ContextTargetUnit(),
                            m_UpToSpellLevel = 10 // Restores all spell levels
                        });
                });
                bp.AddComponent<AbilityResourceLogic>(c => {
                    c.m_RequiredResource = UnlimitedPowerResource.ToReference<BlueprintAbilityResourceReference>();
                    c.m_IsSpendResource = true;
                    c.Amount = 1; // Consume 1 resource per use
                });
                bp.AddComponent<AbilitySpawnFx>(c => {
                    c.PrefabLink = new PrefabLink() { AssetId = "0c07afb9ee854184cb5110891324e3ad" };
                    c.Time = AbilitySpawnFxTime.OnApplyEffect;
                    c.Anchor = AbilitySpawnFxAnchor.Caster;
                });
                bp.m_Icon = Icon_UnlimitedPower;
                bp.Type = AbilityType.Special;
                bp.Range = AbilityRange.Personal;
                bp.CanTargetSelf = true;
                bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Self;
                bp.ActionType = UnitCommand.CommandType.Standard; // Standard action for balance
                bp.LocalizedDuration = StaticReferences.Strings.Null;
                bp.LocalizedSavingThrow = StaticReferences.Strings.Null;
            });

            // Create the feature that adds the ability and resource
            var UnlimitedPowerFeature = Helpers.CreateBlueprint<BlueprintFeature>(IsekaiContext, "UnlimitedPowerFeature", bp => {
                bp.SetName(IsekaiContext, Name);
                bp.SetDescription(Description);
                bp.m_Icon = Icon_UnlimitedPower;
                bp.AddComponent<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] { UnlimitedPowerAbility.ToReference<BlueprintUnitFactReference>() };
                });
                bp.AddComponent<AddAbilityResources>(c => {
                    c.m_Resource = UnlimitedPowerResource.ToReference<BlueprintAbilityResourceReference>();
                    c.RestoreAmount = true; // Restores the resource on rest
                });
            });

            OverpoweredAbilitySelection.AddToSelection(UnlimitedPowerFeature);
        }
    }
}

















