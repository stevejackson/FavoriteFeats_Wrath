using System;
using HarmonyLib;
using Kingmaker;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.UI;
using Kingmaker.UI.MVVM._PCView.CharGen.Phases.FeatureSelector;
using Kingmaker.UI.MVVM._VM.CharGen.Phases.FeatureSelector;
using Kingmaker.UI.MVVM._VM.Other.NestedSelectionGroup;
using Kingmaker.UnitLogic.Class.LevelUp;
using Owlcat.Runtime.UI.Controls.Button;
using Owlcat.Runtime.UI.Tooltips;
using TMPro;
using UniRx;
using UnityEngine.Events;
using UnityEngine.UI;

namespace FavoriteFeats_Wrath.Patches
{
    [HarmonyPatch(typeof(CharGenFeatureSelectorItemVM), MethodType.Constructor, new Type[]{
            typeof(LevelUpController),
            typeof(IFeatureSelectionItem),
            typeof(INestedListSource),
            typeof(ReactiveProperty<TooltipBaseTemplate>),
            typeof(BoolReactiveProperty)
        })]
    static class CharGenFeatureSelectorItemVM_Constructor_Patch
    {
        static void Postfix(ref string ___FeatureName, CharGenFeatureSelectorItemVM __instance)
        {
            string characterName = Game.Instance.LevelUpController.Unit.CharacterName;
            bool IsFeatFavorited = StateManager.IsFeatFavorited(characterName, ___FeatureName);
        }
    }

    [HarmonyPatch(typeof(CharGenFeatureSelectorItemPCView), "BindViewImplementation")]
    static class CharGenFeatureSelectorItemPCView_BindViewImplementation_Patch
    {
        static void Postfix(
            CharGenFeatureSelectorItemPCView __instance,
            ref OwlcatMultiButton ___m_Button,
            ref TextMeshProUGUI ___m_FeatureNameText)
        {
            Main.Logger.Log("CharGenFeatureSelectorItemPCView BindViewImplementation xyz");

            string cleanedFeatureName = Utilities.CleanFeatName(___m_FeatureNameText.text);
            string characterName = Game.Instance.LevelUpController.Unit.CharacterName;
            bool isFavorited = StateManager.IsFeatFavorited(characterName, cleanedFeatureName);

            if (isFavorited)
            {
                ___m_FeatureNameText.text = Utilities.FavoriteFeatTextMarker() + cleanedFeatureName;
            }

            UnityAction toggleFavorite = () =>
            {
                Main.Logger.Log("Toggling favorite status for feat: " + cleanedFeatureName);
                StateManager.ToggleFavoriteStatus(characterName, cleanedFeatureName);
                Game.Instance.UI.UISound.Play(UISoundType.CampCheckSucceeded);
            };

            ___m_Button.OnRightDoubleClick.RemoveAllListeners();
            ___m_Button.OnRightDoubleClick.AddListener(toggleFavorite);

            ___m_Button.OnRightDoubleClickNotInteractable.RemoveAllListeners();
            ___m_Button.OnRightDoubleClickNotInteractable.AddListener(toggleFavorite);
        }
    }
}