﻿using System.Reflection;
using HarmonyLib;
using KMod;
using UnityEngine;

namespace loveFromWiki
{
    public class Patches : UserMod2
    {
        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
        }

        [HarmonyPatch(typeof(DetailTabHeader), "Init")]
        public class DetailTabHeader_Init_Patch
        {
            public static void Postfix(DetailTabHeader __instance)
            {
                var tabName = "WIKITAB";
                var title = "WIKI";
                var desc = "<b>查看WIKI</b>\n爱来自缺氧中文维基";
                
                var wikiGameObject = new GameObject("wikiGameObject");
                var wikiScreen = wikiGameObject.AddComponent<WikiScreen>();

                // 使用反射调用私有方法 MakeTab
                var makeTabMethod =
                    typeof(DetailTabHeader).GetMethod("MakeTab", BindingFlags.NonPublic | BindingFlags.Instance);
                if (makeTabMethod != null)
                {
                    var sprite = Assets.GetSprite("icon_display_screen_properties");
                    if (sprite == null)
                    {
                        Debug.LogError("未能获取到指定的图！");
                        return;
                    }
                    // 传递WikiScreen组件所在的GameObject
                    makeTabMethod.Invoke(__instance,
                        new object[] { tabName, title, sprite, desc, wikiScreen.gameObject });
                }
            }
        }
    }
}