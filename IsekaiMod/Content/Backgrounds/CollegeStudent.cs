using IsekaiMod.Utilities;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using static IsekaiMod.Main;

namespace IsekaiMod.Content.Backgrounds {

    internal class CollegeStudent {

        public static void Add() {
            // Background
            var BackgroundCollegeStudent = Helpers.CreateBlueprint<BlueprintFeature>(IsekaiContext, "BackgroundCollegeStudent", bp => {
                bp.SetName(IsekaiContext, "College Student");
                bp.SetBackgroundDescription(IsekaiContext, "The College Student gains a +2 bonus to all attributes.");
                bp.AddComponent<AddStatBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Trait;
                    c.Stat = StatType.Strength;
                    c.Value = 2;
                });
                bp.AddComponent<AddStatBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Trait;
                    c.Stat = StatType.Dexterity;
                    c.Value = 2;
                });
                bp.AddComponent<AddStatBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Trait;
                    c.Stat = StatType.Constitution;
                    c.Value = 2;
                });
                bp.AddComponent<AddStatBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Trait;
                    c.Stat = StatType.Intelligence;
                    c.Value = 2;
                });
                bp.AddComponent<AddStatBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Trait;
                    c.Stat = StatType.Wisdom;
                    c.Value = 2;
                });
                bp.AddComponent<AddStatBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Trait;
                    c.Stat = StatType.Charisma;
                    c.Value = 2;
                });
            });

            // Register Background
            IsekaiBackgroundSelection.AddToSelection(BackgroundCollegeStudent);
        }
    }
}