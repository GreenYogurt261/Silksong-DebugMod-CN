namespace DebugMod
{
    public static partial class BindableFunctions
    {
        [BindableMethod(name = "开关所有面板", category = "模组界面", allowLock = false)]
        public static void ToggleAllPanels()
        {
            bool active = !(
                DebugMod.settings.HelpPanelVisible ||
                DebugMod.settings.InfoPanelVisible ||
                DebugMod.settings.EnemiesPanelVisible ||
                DebugMod.settings.TopMenuVisible ||
                DebugMod.settings.ConsoleVisible ||
                DebugMod.settings.SaveStatePanelVisible
                );

            DebugMod.settings.InfoPanelVisible = active;
            DebugMod.settings.TopMenuVisible = active;
            DebugMod.settings.EnemiesPanelVisible = active;
            DebugMod.settings.ConsoleVisible = active;
            DebugMod.settings.HelpPanelVisible = active;

            if (!active)
            {
                DebugMod.settings.ClearSaveStatePanel = true;
            }

            if (DebugMod.settings.EnemiesPanelVisible)
            {
                EnemiesPanel.EnemyUpdate();
            }
        }

        [BindableMethod(name = "开关设置面板", category = "模组界面")]
        public static void ToggleHelpPanel()
        {
            DebugMod.settings.HelpPanelVisible = !DebugMod.settings.HelpPanelVisible;
        }

        [BindableMethod(name = "开关信息面板", category = "模组界面")]
        public static void ToggleInfoPanel()
        {
            DebugMod.settings.InfoPanelVisible = !DebugMod.settings.InfoPanelVisible;
        }

        [BindableMethod(name = "开关顶部菜单", category = "模组界面")]
        public static void ToggleTopRightPanel()
        {
            DebugMod.settings.TopMenuVisible = !DebugMod.settings.TopMenuVisible;
        }

        [BindableMethod(name = "开关控制台", category = "模组界面")]
        public static void ToggleConsole()
        {
            DebugMod.settings.ConsoleVisible = !DebugMod.settings.ConsoleVisible;
        }

        [BindableMethod(name = "开关敌人面板", category = "模组界面")]
        public static void ToggleEnemyPanel()
        {
            DebugMod.settings.EnemiesPanelVisible = !DebugMod.settings.EnemiesPanelVisible;
            if (DebugMod.settings.EnemiesPanelVisible)
            {
                EnemiesPanel.EnemyUpdate();
            }
        }

        // View handled in the InfoPanel classes
        [BindableMethod(name = "信息面板切换", category = "模组界面")]
        public static void SwitchActiveInfoPanel()
        {
            InfoPanel.ToggleActivePanel();
        }
    }
}