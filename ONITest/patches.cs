using HarmonyLib;
using KMod;
using PeterHan.PLib.Core;
using PeterHan.PLib.UI;
using UnityEngine;

namespace oniMod
{
    internal class WikiPanel : SideScreenContent
    {
        private readonly PButton bwikiButton = new PButton("bwikiButton")
        {
            Text = "bwiki镜像站",
            OnClick = delegate { Debug.Log("222"); }
        };

        private readonly PButton ggButton = new PButton("ggButton")
        {
            Text = "GG原站点",
            OnClick = delegate { Debug.Log("1111"); }
        };

        private readonly PPanel wikiPPanel = new PPanel("wikiPPanel")
        {
            FlexSize = Vector2.right,
            Spacing = 8,
            Alignment = TextAnchor.MiddleLeft
        };


        public override string GetTitle()
        {
            return "WIKI";
        }

        public override bool IsValidForTarget(GameObject target)
        {
            return true;
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();
            // 添加按钮到面板
            wikiPPanel.AddChild(ggButton).AddChild(bwikiButton);

            // 构建面板
            var panelObject = wikiPPanel.Build();
            // 将构建好的面板添加到当前对象作为子对象
            panelObject.transform.SetParent(gameObject.transform, false);
        }
    }

    internal class Patch : UserMod2
    {
        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            PUtil.InitLibrary();
        }

        [HarmonyPatch(typeof(DetailsScreen), "OnPrefabInit")]
        private class DetailsScreenOnPrefabInit
        {
            public static void Postfix()
            {
                var wikiP = new GameObject("wikiP");
                wikiP.AddComponent<WikiPanel>();
                PUIUtils.AddSideScreenContent<WikiPanel>(wikiP.gameObject);
            }
        }
    }
}