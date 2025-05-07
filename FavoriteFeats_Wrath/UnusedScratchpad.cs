/*
 * unused code to save for testing or reference later.
 */



/*
 * trying to make image icons work for a heart icon next to the favorited feat name. was doing this in CharBuildSelectorItem.
 * couldn't seem to get the gameobject/component manipulation to work well -- maybe the `ref` didn't work on associated objects? unsure.
 * 
 * if I try this again, maybe try doing it without referencing this existing gameobject, instead just create my own relative to the m_ItemLevel's gameObject.
 * 
Main.logger.Log(___m_ItemLevelContainer.ToString());
Main.logger.Log(___m_ItemLevelContainer.gameObject.ToString());

Image heartSprite = ___m_ItemLevelContainer.gameObject.GetComponent<Image>();

if(heartSprite == null)
{
    Main.logger.Log("333333");

    heartSprite = ___m_ItemLevelContainer.gameObject.AddComponent<Image>();

    Main.logger.Log("heartSprite after creation: " + heartSprite.ToString());
}
heartSprite.sprite = Game.Instance.BlueprintRoot.UIRoot.UIIcons.SpellTargetOneFriendly;


Main.logger.Log("Heart sprite after done: " + heartSprite.ToString());

//VerticalLayoutGroupWorkaround wa = ___m_ItemLevelContainer.gameObject.GetComponent<VerticalLayoutGroupWorkaround>();
UnityEngine.CanvasRenderer cr = ___m_ItemLevelContainer.gameObject.GetComponent<UnityEngine.CanvasRenderer>();
//LayoutElement cr = ___m_ItemLevelContainer.gameObject.GetComponent<LayoutElement>();
cr.SetAlpha(0f);
cr.Clear();
cr.SetTexture(null);
cr.SetMaterial(null, 0);
cr.SetMesh(null);
cr.SetAlphaTexture(null);
cr.SetColor(Color.red);
cr.SetPopMaterial(null, 0);

//heartSprite.transform.transform.TransformDirection(new Vector3(0, 0, 100f));
//Image heartSprite = ___m_ItemLevelContainer.AddComponent<Image>();
UnityEngine.Component[] components = ___m_ItemLevelContainer.gameObject.GetComponents(typeof(UnityEngine.Component));
Main.logger.Log("{{{{{Component name checking}}}}}}");

foreach (UnityEngine.Component component in components)
{
    Main.logger.Log("{{{{{Component name}}}}}}" + component.ToString());

    //if (component is not Image)
    //{
    //    Main.logger.Log("{{{{{Deleting component}}}}" + component.ToString());
    //    UnityEngine.GameObject.DestroyImmediate(component);
    //}
}
*/