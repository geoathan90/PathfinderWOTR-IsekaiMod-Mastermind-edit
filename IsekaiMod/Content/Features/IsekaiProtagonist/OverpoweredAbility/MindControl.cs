using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Enums;
using Kingmaker.Localization;
using Kingmaker.ResourceLinks;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using TabletopTweaks.Core.Utilities;
using static IsekaiMod.Main;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility {

    internal class MindControl {

        public static void Add() {
            var Icon_MindControl = AssetLoader.LoadInternal(IsekaiContext, "Features", "ICON_MIND_CONTROL.png");
            var Icon_MindControlImmune = AssetLoader.LoadInternal(IsekaiContext, "Features", "ICON_MIND_CONTROL_IMMUNE.png");

            LocalizedString MindControlDesc = Helpers.CreateString(IsekaiContext, "MindControl.Description",
                "Behold the power of a king! All will listen and obey!"
                + "\nBenefit: You can make any creature fight on your side as if it was your ally. "
                + "It will {g|Encyclopedia:Attack}attack{/g} your opponents to the best of its ability.");

            // Immunity buff
            var MindControlImmunity = TTCoreExtensions.CreateBuff("MindControlImmunity", bp => {
                bp.SetName(IsekaiContext, "Mind Control Immunity");
                bp.SetDescription(IsekaiContext, "This creature cannot be mind-controlled again for 1 minute.");
                bp.m_Icon = Icon_MindControlImmune;
                bp.AddComponent<IsPositiveEffect>();
            });

            // Mind-controlled buff
            var MindControlBuff = TTCoreExtensions.CreateBuff("MindControlBuff", bp => {
                bp.SetName(IsekaiContext, "Mind Controlled");
                bp.SetDescription(IsekaiContext, "This creature has been mind-controlled.");
                bp.m_Icon = Icon_MindControl;
                bp.AddComponent<ChangeFaction>(c => {
                    c.m_Type = ChangeFaction.ChangeType.ToCaster;
                });
                bp.AddComponent<AddFactContextActions>(c => {
                    c.Deactivated = ActionFlow.DoSingle<ContextActionApplyBuff>(c => {
                        c.m_Buff = MindControlImmunity.ToReference<BlueprintBuffReference>();
                        c.DurationValue = new ContextDurationValue() {
                            Rate = DurationRate.Minutes,
                            DiceType = DiceType.Zero,
                            BonusValue = new ContextValue() { ValueType = ContextValueType.Simple, Value = 1 }
                        };
                    });
                });
                bp.Stacking = StackingType.Ignore;
                bp.IsClassFeature = true;
                bp.m_Flags = BlueprintBuff.Flags.StayOnDeath;
            });

            // Mind Control ability
            var MindControlAbility = Helpers.CreateBlueprint<BlueprintAbility>(IsekaiContext, "MindControlAbility", bp => {
                bp.SetName(IsekaiContext, "Overpowered Ability — Mind Control");
                bp.SetDescription(MindControlDesc);
                bp.m_Icon = Icon_MindControl;
                bp.AddComponent<AbilityEffectRunAction>(c => {
                    c.Actions = ActionFlow.DoSingle<Conditional>(c => {
                        c.ConditionsChecker = ActionFlow.IfSingle<ContextConditionHasFact>(c => {
                            c.m_Fact = MindControlImmunity.ToReference<BlueprintUnitFactReference>();
                        });
                        c.IfTrue = ActionFlow.DoSingle<ContextActionApplyBuff>(c => {
                            c.m_Buff = MindControlImmunity.ToReference<BlueprintBuffReference>();
                            c.DurationValue = new ContextDurationValue() {
                                Rate = DurationRate.Minutes,
                                DiceType = DiceType.Zero,
                                BonusValue = new ContextValue() { ValueType = ContextValueType.Simple, Value = 1 }
                            };
                        });
                        c.IfFalse = ActionFlow.DoSingle<ContextActionApplyBuff>(c => {
                            c.m_Buff = MindControlBuff.ToReference<BlueprintBuffReference>();
                            c.DurationValue = new ContextDurationValue() {
                                Rate = DurationRate.Rounds,
                                DiceType = DiceType.Zero,
                                DiceCountValue = 0,
                                BonusValue = new ContextValue() { ValueType = ContextValueType.Rank }
                            };
                        });
                    });
                });
                bp.AddComponent<SpellComponent>(c => {
                    c.School = SpellSchool.Enchantment;
                });
                bp.AddComponent<ContextRankConfig>(c => {
                    c.m_Type = AbilityRankType.Default;
                    c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
                });
                bp.AddComponent<AbilityResourceLogic>(c => {
                    c.m_RequiredResource = Helpers.CreateBlueprint<BlueprintAbilityResource>(IsekaiContext, "MindControlResource", resource => {
                        resource.m_MaxAmount = new BlueprintAbilityResource.Amount() {
                            BaseValue = 1,
                            IncreasedByLevel = true,
                            LevelIncrease = 1 // Gains 1 additional use per level
                        };
                    }).ToReference<BlueprintAbilityResourceReference>();
                    c.m_IsSpendResource = true;
                });
                bp.Type = AbilityType.Supernatural;
                bp.Range = AbilityRange.Medium;
                bp.CanTargetEnemies = true;
                bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Directional;
                bp.ActionType = UnitCommand.CommandType.Standard;
                bp.LocalizedDuration = StaticReferences.Strings.Duration.OneRoundPerLevel;
                bp.LocalizedSavingThrow = Helpers.CreateString(IsekaiContext, "MindControl.SavingThrow", "Will negates");
            });

            // Mind Control feature
            var MindControlFeature = Helpers.CreateBlueprint<BlueprintFeature>(IsekaiContext, "MindControlFeature", bp => {
                bp.SetName(IsekaiContext, "Overpowered Ability — Mind Control");
                bp.SetDescription(MindControlDesc);
                bp.m_Icon = Icon_MindControl;
                bp.AddComponent<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] { MindControlAbility.ToReference<BlueprintUnitFactReference>() };
                });
            });

            OverpoweredAbilitySelection.AddToSelection(MindControlFeature);
        }
    }
}