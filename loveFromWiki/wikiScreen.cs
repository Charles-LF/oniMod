using System.Diagnostics;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

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
            this.wikiPanel.GetComponent<VerticalLayoutGroup>().padding.bottom = 20;
            this.wikiPanel.Content.GetComponent<VerticalLayoutGroup>().padding.top = 10;
            this.wikiPanel.Content.GetComponent<VerticalLayoutGroup>().padding.bottom = 10;
            this.wikiPanel.scalerMask.hoverLock = true;
            base.Subscribe<WikiScreen>(-1514841199, WikiScreen.OnRefreshDataDelegate);
        }
        protected override void OnSelectTarget(GameObject target) 
        {
            base.OnSelectTarget(target);
            this.wikiPanel.gameObject.SetActive(true);
            //this.wikiPanel.scalerMask.UpdateSize();
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
            targetPanel.SetLabelWithButton("BWIKI", "跳转到BWIKI镜像站点", "跳转到镜像站点", delegate
            {
                Process.Start(new ProcessStartInfo(baseurl + itemName) { UseShellExecute = true });
            });
            targetPanel.Commit();
        }
        
        public CollapsibleDetailContentPanel wikiPanel;

        private static readonly EventSystem.IntraObjectHandler<WikiScreen> OnRefreshDataDelegate = new EventSystem.IntraObjectHandler<WikiScreen>(delegate (WikiScreen component, object data)
        {
            component.OnRefreshData(data);
        });

        private void OnRefreshData(object obj)
        {
            this.Refresh(false);
        }
    }
}
