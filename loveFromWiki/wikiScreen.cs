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
                this.A = base.CreateCollapsableSection("");
                
            }
            protected override void OnSelectTarget(GameObject target) { }
            public override void OnDeselectTarget(GameObject target) { }
            protected override void Refresh(bool force = false)
            {
                WikiScreen.RefreshA(this.A, this.selectedTarget);
            }
            private static void RefreshA(CollapsibleDetailContentPanel targetPanel, GameObject targetEntity)
            {
                targetPanel.SetActive(true);
                targetPanel.SetLabel("这个是id", "这是名字", "不知道填什么");
                targetPanel.Commit();
            }

            public CollapsibleDetailContentPanel A;
    }
}
