using System.Diagnostics;
using System.Text.RegularExpressions;
using UnityEngine;

namespace loveFromWiki
{
    public class WikiScreen : DetailScreenTab
    {
        public CollapsibleDetailContentPanel wikiPanel;

        public override bool IsValidForTarget(GameObject target)
        {
            return true;
        }

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            wikiPanel = CreateCollapsableSection("WIKI面板");
            wikiPanel.GetComponent<Transform>().localPosition = new Vector3(0, -25, 0);
        }

        protected override void OnSelectTarget(GameObject target)
        {
            base.OnSelectTarget(target);
            wikiPanel.gameObject.SetActive(true);
            Refresh(true);
        }

        public override void OnDeselectTarget(GameObject target)
        {
            base.OnDeselectTarget(target);
        }

        protected override void Refresh(bool force = false)
        {
            RefreshWikiPanel(wikiPanel, selectedTarget);
        }

        private static void RefreshWikiPanel(CollapsibleDetailContentPanel targetPanel, GameObject targetEntity)
        {
            var itemName = "";
            targetPanel.SetActive(true);
            var baseurl = "https://wiki.biligame.com/oni/";
            var ggurl = "https://oxygennotincluded.wiki.gg/zh/";
            if (targetEntity != null)
            {
                // targetEntity.GetComponent<KSelectable>()?.GetName() ?? " "  拿游戏内的名字
                // 这个鬼东西会返回 <link="DININGTABLE">餐桌</link> 这样的鬼东西，用正则提取名字

                var pattern = @"<link=""[^""]+"">(.*?)</link>";
                var regex = new Regex(pattern);
                var match = Regex.Match(targetEntity.GetComponent<KSelectable>()?.GetName() ?? " ", pattern);
                if (match.Success)
                {
                    // 提取 <link> 标签内的文本
                    var text = match.Groups[1].Value;
                    itemName = text;
                }
            }

            //targetPanel.SetLabel("url1", baseurl, baseurl);
            //targetPanel.SetLabel("WIKILabel1", "点击下面按钮空就能跳转啦！", "皮肤名称没法识别");
            targetPanel.SetLabelWithButton("BWIKI", "跳转到BWIKI镜像站点", "跳转到镜像站点",
                delegate { Process.Start(new ProcessStartInfo(baseurl + itemName) { UseShellExecute = true }); });
            targetPanel.SetLabelWithButton("GGWIKI", "跳转到WIKIGG源站点", "跳转到WIKIGG源站点",
                delegate { Process.Start(new ProcessStartInfo(ggurl + itemName) { UseShellExecute = true }); });
            targetPanel.Commit();
        }
    }
}