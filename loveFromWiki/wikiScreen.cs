using System;
using System.Diagnostics;
using UnityEngine;

namespace loveFromWiki
{
    public class WikiScreen : DetailScreenTab
    {
        public override bool IsValidForTarget(GameObject target)
        {
            return true;
        }
        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            A = base.CreateCollapsableSection("这是栏目A");
            B = base.CreateCollapsableSection("这是栏目B");

        }
        protected override void OnSelectTarget(GameObject target) 
        {
            base.OnSelectTarget(target);
        }
        public override void OnDeselectTarget(GameObject target) { }
        protected override void Refresh(bool force = false)
        {
            WikiScreen.RefreshA(this.A, this.selectedTarget);
            WikiScreen.RefreshB(this.B, this.selectedTarget);
        }
        private static void RefreshA(CollapsibleDetailContentPanel targetPanel, GameObject targetEntity)
        {
            targetPanel.SetActive(true);
            targetPanel.SetLabel("这个是id1", "这是名字1", "不知道填什么");
            targetPanel.Commit();
        }

        private static void RefreshB(CollapsibleDetailContentPanel targetPanel, GameObject targetEntity)
        {
            targetPanel.SetActive(true);
            targetPanel.SetLabelWithButton("BWIKI", "跳转到BWIKI镜像站点", "跳转到镜像站点", delegate
            {
                string url = "https://wiki.biligame.com/oni/%E9%A6%96%E9%A1%B5";
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            });
            targetPanel.Commit();
        }

        public CollapsibleDetailContentPanel A;
        public CollapsibleDetailContentPanel B;
    }
}
