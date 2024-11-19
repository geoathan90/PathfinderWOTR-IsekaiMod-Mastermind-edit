using Kingmaker.Blueprints.Classes;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;
using static IsekaiMod.Main;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.SpecialPower {

    internal class BetaStrike {
        private static readonly Sprite Icon_ArcaneWeaponSpeed = BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("85742dd6788c6914f96ddc4628b23932").m_Icon;

        public static void Add() {
            var BetaStrike = Helpers.CreateBlueprint<BlueprintFeature>(IsekaiContext, "BetaStrike", bp => {
                bp.SetName(IsekaiContext, "Beta Strike");
                bp.SetDescription(IsekaiContext, "You get an additional {g|Encyclopedia:Attack}attack{/g} per {g|Encyclopedia:Combat_Round}round{/g}.");
                bp.m_Icon = Icon_ArcaneWeaponSpeed;
                bp.AddComponent<BuffExtraAttack>(c => {
                    c.Number = 1;
                    c.Haste = false;
                });
            });
            SpecialPowerSelection.AddToSelection(BetaStrike);
        }
    }
}