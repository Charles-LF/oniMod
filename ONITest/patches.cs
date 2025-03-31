using HarmonyLib;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

// 新面板类
public class WikiPanel : DetailScreenTab
{
    public override bool IsValidForTarget(GameObject target)
    {
        return true;
    }

    protected override void OnPrefabInit()
    {
        base.OnPrefabInit();
        this.wikiPanel = base.CreateCollapsableSection("作者的话");
        // 嗨呀，研究几天才搞懂这Transform
        this.wikiPanel.GetComponent<Transform>().localPosition = new Vector3(0, -20, 0);
        this.wikiPanel.GetComponent<LayoutGroup>();
        
        


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
        WikiPanel.RefreshWikiPanel(this.wikiPanel, this.selectedTarget);
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
        targetPanel.SetLabel("WIKIDesc", "桀桀桀桀桀，欢迎来看Wiki\n你也可以使用短链 oni.wiki 直达源站点\nWIKI组欢迎查阅的各位复制人加入我们!!!\n", "有生之年甚至大概/或许/可能能看到托德的女装喔");

        // 在任意脚本中获取 UserMenuScreen 实例
        //var userMenu = FindObjectOfType<UserMenuScreen>();

        //// 创建按钮信息
        //var newButton = new KIconButtonMenu.ButtonInfo(
        //    iconName: "action_move_to_storage",   // 图标名称（需在父类 icons 数组中）
        //    text: "自定义按钮",          // 按钮文本
        //    on_click: () => {            // 点击事件
        //        Debug.Log("按钮被点击");
        //    },
        //    is_interactable: true        // 是否可交互
        //);

        //// 调用内置方法添加并刷新
        //userMenu.AddButtons(new List<KIconButtonMenu.ButtonInfo> { newButton });
        //userMenu.Refresh(targetEntity); // 立即刷新（或依赖选中对象刷新）


        //targetPanel.SetLabelWithButton("BWIKI", "跳转到BWIKI镜像站点", "跳转到镜像站点", delegate
        //{
        //    Process.Start(new ProcessStartInfo(baseurl + itemName) { UseShellExecute = true });
        //});
        //targetPanel.SetLabelWithButton("GGWIKI", "跳转到WIKIGG源站点", "跳转到WIKIGG源站点", delegate
        //{
        //    Process.Start(new ProcessStartInfo(ggurl + itemName) { UseShellExecute = true });
        //});

        targetPanel.Commit();
    }
    public CollapsibleDetailContentPanel wikiPanel;
    private List<KIconButtonMenu.ButtonInfo> buttonInfos = new List<KIconButtonMenu.ButtonInfo>();

}

// Harmony补丁类
[HarmonyPatch(typeof(DetailTabHeader), "Init")]
public static class AddNewTabPatch
{
    static void Postfix(DetailTabHeader __instance)
    {
        // 创建新面板预制体
        GameObject panelPrefab = new GameObject("WikiPanel");
        panelPrefab.AddComponent<WikiPanel>();
        // 使用反射调用私有方法 MakeTab
        MethodInfo makeTabMethod = typeof(DetailTabHeader).GetMethod("MakeTab", BindingFlags.NonPublic | BindingFlags.Instance);
        if (makeTabMethod != null)
        {
            Sprite sprite = Assets.GetSprite("icon_display_screen_properties");
            // 传递WikiScreen组件所在的GameObject
            makeTabMethod.Invoke(__instance, new object[] { "WIKIPanel", "WIKI", sprite, "爱来自缺氧中文维基", panelPrefab });
        }
    }
}