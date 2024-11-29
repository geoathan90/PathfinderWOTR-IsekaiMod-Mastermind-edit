using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;
using static IsekaiMod.Main;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility {

    internal class UnderpoweredAbilitySelection {

        public static void Add() {
            Sprite Icon_Haste = BlueprintTools.GetBlueprint<BlueprintAbility>("486eaff58293f6441a5c2759c4872f98").m_Icon;
            Sprite Icon_BelieveInYourselfStrength = BlueprintTools.GetBlueprint<BlueprintAbility>("9f476da1aa712284d8e652fc387ce5bb").m_Icon;
            Sprite Icon_CrystalMind = BlueprintTools.GetBlueprint<BlueprintAbility>("4733d8dd549ff544395f1684ec73c392").m_Icon;
            Sprite Icon_EuphoricTranquility = BlueprintTools.GetBlueprint<BlueprintAbility>("cbf3bafa8375340498b86a3313a11e2f").m_Icon;

            var DodgeMaster = Helpers.CreateBlueprint<BlueprintFeature>(IsekaiContext, "DodgeMaster", bp => {
                bp.SetName(IsekaiContext, "Underpowered Ability — Dodge Master");
                bp.SetDescription(IsekaiContext, "Blessed by the winds of fate in your new life, you can effortlessly dodge even the most well-aimed strikes."
                    + "\nBenefit: You gain a +4 dodge bonus to AC and can move freely without provoking attacks of opportunity.");
                bp.m_Icon = Icon_Haste;
                bp.AddComponent<AddStatBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Dodge;
                    c.Stat = StatType.AC;
                    c.Value = 4;
                });
                bp.AddComponent<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] {
                        BlueprintTools.GetBlueprint<BlueprintFeature>("c6147854641924442a3bb736080cfeb6").ToReference<BlueprintUnitFactReference>() // Mobility feature for avoiding AoOs
                    };
                });
            });

            var BulkMaster = Helpers.CreateBlueprint<BlueprintFeature>(IsekaiContext, "SuperStrength", bp => {
                bp.SetName(IsekaiContext, "Underpowered Ability — Super Strength");
                bp.SetDescription(IsekaiContext, "Infused with the strength of a titan, your physical might is a testament to your rebirth in this new world."
                    + "\nBenefit: You gain +50 hit points and +4 Strength.");
                bp.m_Icon = Icon_BelieveInYourselfStrength;
                bp.AddComponent<AddStatBonus>(c => {
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                    c.Stat = StatType.HitPoints;
                    c.Value = 50;
                });
                bp.AddComponent<AddStatBonus>(c => {
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                    c.Stat = StatType.Strength;
                    c.Value = 4;
                });
            });

            var ThoughtMaster = Helpers.CreateBlueprint<BlueprintFeature>(IsekaiContext, "ThoughtMaster", bp => {
                bp.SetName(IsekaiContext, "Underpowered Ability — Thought Master");
                bp.SetDescription(IsekaiContext, "Your newfound mind is a gift from the cosmos, giving you unparalleled clarity and understanding."
                    + "\nBenefit: You gain a +4 bonus to Intelligence and Wisdom, and a +2 bonus to Knowledge (Arcana), Knowledge (World), and Perception checks.");
                bp.m_Icon = Icon_CrystalMind;
                bp.AddComponent<AddStatBonus>(c => {
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                    c.Stat = StatType.Intelligence;
                    c.Value = 4;
                });
                bp.AddComponent<AddStatBonus>(c => {
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                    c.Stat = StatType.Wisdom;
                    c.Value = 4;
                });
                bp.AddComponent<AddStatBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Competence;
                    c.Stat = StatType.SkillKnowledgeArcana;
                    c.Value = 2;
                });
                bp.AddComponent<AddStatBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Competence;
                    c.Stat = StatType.SkillKnowledgeWorld;
                    c.Value = 2;
                });
                bp.AddComponent<AddStatBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Competence;
                    c.Stat = StatType.SkillPerception;
                    c.Value = 2;
                });
            });

            var UnderpoweredAbilitySelection = Helpers.CreateBlueprint<BlueprintFeatureSelection>(IsekaiContext, "UnderpoweredAbilitySelection", bp => {
                bp.SetName(IsekaiContext, "Underpowered Ability");
                bp.SetDescription(IsekaiContext, "Even in this new world, you prefer to keep things simple. These humble abilities reflect your decision to avoid overwhelming power in favor of a quiet, comfortable life.");
                bp.m_Icon = Icon_EuphoricTranquility;
                bp.m_AllFeatures = new BlueprintFeatureReference[] {
                    DodgeMaster.ToReference<BlueprintFeatureReference>(),
                    BulkMaster.ToReference<BlueprintFeatureReference>(),
                    ThoughtMaster.ToReference<BlueprintFeatureReference>(),
                };
            });

            OverpoweredAbilitySelection.AddToSelection(UnderpoweredAbilitySelection);
        }
    }
}