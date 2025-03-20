using HarmonyLib;
using KMod;
using System.Reflection;
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
            [HarmonyPostfix]
            public static void Postfix(DetailTabHeader __instance)
            {
                string tabName = "WIKITAB";
                string title = "WIKI";
                string desc = "<b>查看WIKI</b>\n爱来自缺氧中文维基";

                // 使用反射获取 simpleInfoScreen 字段的信息
                FieldInfo simpleInfoScreenField = typeof(DetailTabHeader).GetField("simpleInfoScreen", BindingFlags.NonPublic | BindingFlags.Instance);
                // 获取 simpleInfoScreen 字段实际存储的 GameObject 类型的值
                GameObject simpleInfoScreen = simpleInfoScreenField != null ? (GameObject)simpleInfoScreenField.GetValue(__instance) : null;

                // 使用反射调用私有方法 MakeTab
                MethodInfo makeTabMethod = typeof(DetailTabHeader).GetMethod("MakeTab", BindingFlags.NonPublic | BindingFlags.Instance);
                if (makeTabMethod != null)
                {
                    makeTabMethod.Invoke(__instance, new object[] { tabName, title, Assets.GetSprite("icon_display_screen_properties"), desc, simpleInfoScreen });
                }
            }
        }
    }
}

