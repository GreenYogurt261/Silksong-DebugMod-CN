namespace DebugMod
{
    public static partial class BindableFunctions
    {

        [BindableMethod(name = "开启/关闭敌人血条", category = "敌人面板")]
        public static void ToggleEnemyHPBars()
        {
            EnemiesPanel.hpBars = !EnemiesPanel.hpBars;
            EnemiesPanel.EnemyUpdate();

            if (EnemiesPanel.hpBars)
            {
                Console.AddLine("已开启敌人血条");
            }
            else
            {
                Console.AddLine("已关闭敌人血条");
            }
        }

        [BindableMethod(name = "自残", category = "敌人面板")]
        public static void SelfDamage()
        {
            if (PlayerData.instance.health <= 0)
            {
                Console.AddLine("无法自残 生命值已为0或更低");
            }
            else if (HeroController.instance.cState.dead)
            {
                Console.AddLine("无法自残 玩家已死亡");
            }
            else if (!GameManager.instance.IsGameplayScene())
            {
                Console.AddLine("无法自残 非游戏场景");
            }
            else if (HeroController.instance.cState.recoiling)
            {
                Console.AddLine("无法自残 玩家处于硬直状态");
            }
            else if (HeroController.instance.cState.invulnerable)
            {
                Console.AddLine("无法自残 玩家处于无敌状态");
            }
            else
            {
                HeroController.instance.DamageSelf(1);
                Console.AddLine("正在执行自残");
            }
        }
    }
}