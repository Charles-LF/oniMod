using System.Diagnostics;
using System.Text.RegularExpressions;
using UnityEngine;

namespace loveFromWiki
{
    public class WikiScreen : DetailScreenTab
    {
        string itemName;
        public override bool IsValidForTarget(GameObject target)
        {
            return true;
        }
        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            this.wikiPanel = base.CreateCollapsableSection("WIKI面板");
            this.wikiPanel.GetComponent<Transform>().localPosition = new Vector3(0, -20, 0);

        }
        protected override void OnSelectTarget(GameObject target) 
        {
            base.OnSelectTarget(target);
            this.wikiPanel.gameObject.SetActive(true);
            this.Refresh(true);

        }
        public override void OnDeselectTarget(GameObject target) 
        {
            base.OnDeselectTarget(target);

        }
        protected override void Refresh(bool force = false)
        {
            WikiScreen.RefreshWikiPanel(this.wikiPanel, this.selectedTarget);
        }
        private static void RefreshWikiPanel(CollapsibleDetailContentPanel targetPanel, GameObject targetEntity)
        {
            string itemName = "";
            targetPanel.SetActive(true);
            string baseurl = "https://wiki.biligame.com/oni/";
            string ggurl = "https://oxygennotincluded.wiki.gg/zh/";
            if (targetEntity != null)
            {
                // targetEntity.GetComponent<KSelectable>()?.GetName() ?? " "  拿游戏内的名字
                // 这个鬼东西会返回 <link="DININGTABLE">餐桌</link> 这样的鬼东西，用正则提取名字

                string pattern = @"<link=""[^""]+"">(.*?)</link>";
                Regex regex = new Regex(pattern);
                Match match = Regex.Match(targetEntity.GetComponent<KSelectable>()?.GetName() ?? " ", pattern);
                if (match.Success)
                {
                    // 提取 <link> 标签内的文本
                    string text = match.Groups[1].Value;
                    itemName = text;
                }
            }
            //targetPanel.SetLabel("url1", baseurl, baseurl);
            //targetPanel.SetLabel("WIKI", "搞不懂这个怎么布局，下面两个可以点","太难了，什么鬼东西啊");
            targetPanel.SetLabelWithButton("BWIKI", "跳转到BWIKI镜像站点", "跳转到镜像站点", delegate
            {
                Process.Start(new ProcessStartInfo(baseurl + itemName) { UseShellExecute = true });
            });
            targetPanel.SetLabelWithButton("GGWIKI", "跳转到WIKIGG源站点", "跳转到WIKIGG源站点", delegate
            {
                Process.Start(new ProcessStartInfo(ggurl + itemName) { UseShellExecute = true });
            });
            targetPanel.Commit();
        }
        
        public CollapsibleDetailContentPanel wikiPanel;
    }
}
