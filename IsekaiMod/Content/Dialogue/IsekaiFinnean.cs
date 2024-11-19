using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.DialogSystem;
using Kingmaker.DialogSystem.Blueprints;
using System.Collections.Generic;
using TabletopTweaks.Core.Utilities;
using static IsekaiMod.Main;

namespace IsekaiMod.Content.Dialogue {

    internal class IsekaiFinnean {
        public static void Add() {
            AddDialogueSuccubus();
        }
        private static void AddDialogueSuccubus() {
            var AnswersList_0003 = BlueprintTools.GetBlueprint<BlueprintAnswersList>("615ef80243cfc184ba42286395880b0e");
            var FinneanCompanionRef = BlueprintTools.GetBlueprintReference<BlueprintUnitReference>("ea8034769ab7d584e97b5227cbc03296");

            var IsekaiDialogueFinneanReply = TTCoreExtensions.CreateCue("IsekaiDialogueFinneanReply", bp => {
                bp.SetText(IsekaiContext, "\"Not sure what you mean, I can be any weapon you want? Am I not good enough?\"");
                bp.Speaker = new DialogSpeaker {
                    m_Blueprint = null,
                    MoveCamera = false,
                    m_SpeakerPortrait = FinneanCompanionRef
                };
                bp.Answers = AnswersList_0003.Answers;
            });
            var IsekaiDialogueFinnean = TTCoreExtensions.CreateAnswer("IsekaiDialogueFinnean", bp => {
                bp.SetText(IsekaiContext, "(Isekai Protagonist) \"Hey can't you transform into anything cooler?\"");
                bp.NextCue = new CueSelection() {
                    Cues = new List<BlueprintCueBaseReference>() { IsekaiDialogueFinneanReply.ToReference<BlueprintCueBaseReference>() },
                    Strategy = Strategy.First
                };
                bp.ShowOnce = true;
            });

            // Add Answer to answers list
            AnswersList_0003.Answers.Insert(0, IsekaiDialogueFinnean.ToReference<BlueprintAnswerBaseReference>());
        }
    }
}