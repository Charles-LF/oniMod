using PeterHan.PLib.UI;
using System.Diagnostics;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

namespace loveFromWiki
{
    public class WikiScreen : DetailScreenTab
    {
        private CollapsibleDetailContentPanel wikiPanel;
        private static string itemName = "";

        public override bool IsValidForTarget(GameObject target)
        {
            return true;
        }

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            // 所有UI对象必须包含RectTransform
            gameObject.AddComponent<RectTransform>();
            // 需要自动布局时添加
            gameObject.AddOrGet<ContentSizeFitter>();
            var layout = gameObject.AddOrGet<VerticalLayoutGroup>();
            layout.spacing = 10f;          // 子对象间距10像素
            layout.padding = new RectOffset(5, 5, 5, 5); // 上下左右边距5像素
            layout.childAlignment = TextAnchor.UpperCenter; // 子对象顶部居中

            gameObject.AddOrGet<LayoutElement>();
            gameObject.AddOrGet<CanvasRenderer>();
            gameObject.AddOrGet<Image>();

            wikiPanel = CreateCollapsableSection("Charles的碎碎念");

        }
        public override void OnDeselectTarget(GameObject target) { }
        protected override void OnSelectTarget(GameObject target) { }


        protected override void Refresh(bool force = false)
        {
            var baseurl = "https://wiki.biligame.com/oni/";
            var ggurl = "https://oxygennotincluded.wiki.gg/zh/";

            if (wikiPanel != null && !ButtonAdded)
            {
                GameObject ggButton = CreateCustomButton("ggButton", "GG原站点", "点击跳转到原站点", OnClickggButton);
                ggButton.transform.SetParent(this.transform, false);
                ggButton.transform.SetSiblingIndex(1);

                GameObject bwikiButton = CreateCustomButton("bwikiButton", "bwiki镜像站", "点击跳转到镜像站", OnClickBwikiButton);
                bwikiButton.transform.SetParent(this.transform, false);
                bwikiButton.transform.SetSiblingIndex(2);
                ButtonAdded = true;
                Debug.Log("按钮创建成功（Refresh）");
            }

            void OnClickggButton()
            {
                KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click", false));
                Process.Start(new ProcessStartInfo(ggurl+ itemName) { UseShellExecute = true });
            };

            void OnClickBwikiButton()
            {
                KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click", false));
                Process.Start(new ProcessStartInfo(baseurl+ itemName) { UseShellExecute = true });
            };

            RefreshWikiPanel(wikiPanel, selectedTarget);
        }

        private static void RefreshWikiPanel(CollapsibleDetailContentPanel targetPanel, GameObject targetEntity)
        {
            if (targetEntity != null)
            {
                var name = targetEntity.GetComponent<KSelectable>()?.GetName() ?? " ";  //  拿游戏内的名字
                // 这个鬼东西会返回 <link="DININGTABLE">餐桌</link> 这样的鬼东西，用正则提取名字

                var pattern = @"<link=""[^""]+"">(.*?)</link>";
                var match = Regex.Match(name, pattern);
                if (match.Success)
                {
                    // 提取 <link> 标签内的文本
                    var text = match.Groups[1].Value;
                    itemName = text;
                }
            }
            targetPanel.SetActive(true);
            targetPanel.SetLabel("WIKILabel1", "代码基本是抄游佬的\n\nc#基本是不会的\n\n皮肤名称是不会处理的\n\n最后\n\n点击上面按钮空就能跳转啦！", "Alright, go ahead and enjoy your game now!");
            targetPanel.Commit();
        }
        private GameObject CreateCustomButton(string name, string text, string tooltip, System.Action onClick)
        {
            var btn = new PButton(name)
            {
                Text = text,
                ToolTip = tooltip,
                //Sprite = Assets.GetSprite("icon_display_screen_properties"),
                OnClick = (go) => onClick?.Invoke()
            }
            .SetKleiBlueStyle();
            return btn.Build();
        }
        private bool ButtonAdded = false;
    }
}