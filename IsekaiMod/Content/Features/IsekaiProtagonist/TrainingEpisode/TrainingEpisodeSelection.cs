using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Localization;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using TabletopTweaks.Core.Utilities;
using UnityEngine;
using static IsekaiMod.Main;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.TrainingEpisode {

    internal class TrainingEpisodeSelection {
        private static readonly LocalizedString TrainingEpisodeDesc = Helpers.CreateString(IsekaiContext, "TrainingEpisode.Description",
            "At 12th level, you and your companions take a short intermission beside a large body of water. "
            + "During this time, you begin a journey of self discovery.");
        private static readonly Sprite Icon_BeachEpisode = BlueprintTools.GetBlueprint<BlueprintAbility>("4e2e066dd4dc8de4d8281ed5b3f4acb6").m_Icon;

        public static void Add() {
            var TrainingEpisodeSelection = Helpers.CreateBlueprint<BlueprintFeatureSelection>(IsekaiContext, "TrainingEpisodeSelection", bp => {
                bp.SetName(IsekaiContext, "Training Episode");
                bp.SetDescription(TrainingEpisodeDesc);
                bp.m_Icon = Icon_BeachEpisode;
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.m_Features = new BlueprintFeatureReference[0];
                bp.m_AllFeatures = new BlueprintFeatureReference[0];
            });
            var TrainingEpisodeBonusSelection = Helpers.CreateBlueprint<BlueprintFeatureSelection>(IsekaiContext, "TrainingEpisodeBonusSelection", bp => {
                bp.SetName(IsekaiContext, "Another Training Episode");
                bp.SetDescription(TrainingEpisodeDesc);
                bp.m_Icon = Icon_BeachEpisode;
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.m_Features = new BlueprintFeatureReference[0];
                bp.m_AllFeatures = new BlueprintFeatureReference[0];
            });
        }

        public static void AddToSelection(BlueprintFeature feature) {
            var TrainingEpisodeSelection = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(IsekaiContext, "TrainingEpisodeSelection");
            TrainingEpisodeSelection.m_Features = TrainingEpisodeSelection.m_Features.AddToArray(feature.ToReference<BlueprintFeatureReference>());
            TrainingEpisodeSelection.m_AllFeatures = TrainingEpisodeSelection.m_AllFeatures.AddToArray(feature.ToReference<BlueprintFeatureReference>());
            var TrainingEpisodeBonusSelection = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(IsekaiContext, "TrainingEpisodeBonusSelection");
            TrainingEpisodeBonusSelection.m_Features = TrainingEpisodeBonusSelection.m_Features.AddToArray(feature.ToReference<BlueprintFeatureReference>());
            TrainingEpisodeBonusSelection.m_AllFeatures = TrainingEpisodeBonusSelection.m_AllFeatures.AddToArray(feature.ToReference<BlueprintFeatureReference>());
        }
    }
}