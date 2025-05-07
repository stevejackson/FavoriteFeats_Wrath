using System;
using HarmonyLib;
using Kingmaker;
using Kingmaker.UI.MVVM._PCView.CharGen.Phases.FeatureSelector;
using Kingmaker.UI.MVVM._VM.CharGen.Phases.FeatureSelector;
using Kingmaker.UI.MVVM._VM.Other;
using Kingmaker.UnitLogic.Class.LevelUp;

namespace FavoriteFeats_Wrath.Patches
{
    [HarmonyPatch(typeof(CharGenFeatureSelectorPCView), "EntityComparer")]
    static class CharGenFeatureSelectorPCView_EntityComparer_Patch
    {
        static bool Prefix(CharGenFeatureSelectorItemVM a, CharGenFeatureSelectorItemVM b, ref int __result, CharGenFeatureSelectorPCView __instance)
        {
            string cleanedFeatureNameA = Utilities.CleanFeatName(a.FeatureName);
            string cleanedFeatureNameB = Utilities.CleanFeatName(b.FeatureName);
            string characterName = Game.Instance.LevelUpController.Unit.CharacterName;

            bool favoritedA = StateManager.IsFeatFavorited(characterName, cleanedFeatureNameA);
            bool favoritedB = StateManager.IsFeatFavorited(characterName, cleanedFeatureNameB);
            bool alreadyHasA = a.SelectState == FeatureSelectionViewState.SelectState.AlreadyHas;
            bool alreadyHasB = b.SelectState == FeatureSelectionViewState.SelectState.AlreadyHas;

            if (favoritedA && !favoritedB && !alreadyHasA)
            {
                __result = -1;
                goto quit_method;
            }
            if (favoritedB && !favoritedA && !alreadyHasB)
            {
                __result = 1;
                goto quit_method;
            }


            bool flag = a.IsSelected.Value || a.SelectState == FeatureSelectionViewState.SelectState.CanSelect;
            int num = (b.IsSelected.Value || b.SelectState == FeatureSelectionViewState.SelectState.CanSelect).CompareTo(flag);
            if (num != 0)
            {
                __result = num;
                goto quit_method;
            }
            bool flag2 = a.SelectState == FeatureSelectionViewState.SelectState.AlreadyHas;
            int num2 = (a.SelectState == FeatureSelectionViewState.SelectState.AlreadyHas).CompareTo(flag2);
            if (num2 != 0)
            {
                __result = num2;
                goto quit_method;
            }
            RecommendationType value = a.FeatureRecommendation.Value.Recommendation.Value;
            int num3 = b.FeatureRecommendation.Value.Recommendation.Value.CompareTo(value);
            if (num3 != 0)
            {
                __result = num3;
                goto quit_method;
            }
            __result = string.Compare(a.FeatureName, b.FeatureName, StringComparison.CurrentCultureIgnoreCase);
            goto quit_method;

        quit_method:
            return false;
        }
    }
}
