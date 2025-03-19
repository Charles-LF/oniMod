using HarmonyLib;
using KMod;
using UnityEngine;
using System.Reflection;

namespace loveFromWiki
{
    public class Patches:UserMod2
    {
        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
        }

        [HarmonyPatch(typeof(DetailTabHeader), "Init")]
        public class DetailTabHeader_Init_Patch
        {
            

            [HarmonyPostfix]
            public static void Postfix(DetailTabHeader __instance)
            {
                string tabName = "WIKITAB";
                string title = "WIKI";
                string desc = "<b>查看WIKI</b>\n爱来自缺氧中文维基";
                bool lang = Localization.GetCurrentLanguageCode() == "zh";
                // 使用反射调用私有方法 MakeTab
                MethodInfo makeTabMethod = typeof(DetailTabHeader).GetMethod("MakeTab", BindingFlags.NonPublic | BindingFlags.Instance);
                if (makeTabMethod != null)
                {
                    if (!lang)
                    {
                        makeTabMethod.Invoke(__instance, new object[] { tabName, title, Assets.GetSprite("icon_display_screen_properties"), "<b>LearnWIKI</b>\n Love from the ONI Wiki.", simpleInfoScreen });
                    }
                    makeTabMethod.Invoke(__instance, new object[] { tabName, title, Assets.GetSprite("icon_display_screen_properties"), desc, simpleInfoScreen });
                }
            }
            private static GameObject simpleInfoScreen;
        }
    }


}

