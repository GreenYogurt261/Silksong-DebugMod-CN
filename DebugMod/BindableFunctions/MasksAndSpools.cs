using System;
using DebugMod.MethodHelpers;

namespace DebugMod
{
    public static partial class BindableFunctions
    {
        [BindableMethod(name = "面甲（白血）+1", category = "面甲 & 灵丝轴")]
        public static void GiveMask()
        {
            if (PlayerData.instance.maxHealthBase < 10)
            {
                HeroController.instance.MaxHealth();
                HeroController.instance.AddToMaxHealth(1);
                HudHelper.RefreshMasks();

                Console.AddLine("面甲（白血）+1");
            }
            else
            {
                Console.AddLine("面甲数量已到达上限");
            }
        }
        
        [BindableMethod(name = "灵丝轴（丝槽）+1", category = "面甲 & 灵丝轴")]
        public static void GiveSpool()
        {
            if (PlayerData.instance.silkMax < 18)
            {
                HeroController.instance.AddToMaxSilk(1);
                HudHelper.RefreshSpool();

                Console.AddLine("灵丝轴（丝槽）+1");
            }
            else
            {
                Console.AddLine("灵丝轴已到达上限");
            }

            PlayerData.instance.IsSilkSpoolBroken = false;
            EventRegister.SendEvent("SPOOL UNBROKEN");
        }
        
        [BindableMethod(name = "面甲（白血）-1", category = "面甲 & 灵丝轴")]
        public static void TakeAwayMask()
        {
            if (PlayerData.instance.maxHealthBase > 1)
            {
                PlayerData.instance.maxHealth -= 1;
                PlayerData.instance.maxHealthBase -= 1;
                PlayerData.instance.health = Math.Min(PlayerData.instance.health, PlayerData.instance.maxHealth);
                HudHelper.RefreshMasks();

                Console.AddLine("面甲（白血）-1");
            }
            else
            {
                Console.AddLine("面甲数量已到达下限");
            }
        }

        [BindableMethod(name = "灵丝轴（丝槽）-1", category = "面甲 & 灵丝轴")]
        public static void TakeAwaySpool()
        {
            if (PlayerData.instance.silkMax > 9)
            {
                PlayerData.instance.silkMax--;
                PlayerData.instance.silk = Math.Min(PlayerData.instance.silk, PlayerData.instance.silkMax);
                HudHelper.RefreshSpool();

                Console.AddLine("灵丝轴（丝槽）-1");
            }
            else
            {
                Console.AddLine("灵丝轴数量已到达下限");
            }
        }

        private static bool CanModifyHealth(int health)
        {
            if (health <= 0)
            {
                Console.AddLine("无法增加/扣除生命值：生命值过低");
                return false;
            }

            if (HeroController.instance.cState.dead)
            {
                Console.AddLine("无法增加/扣除生命值：玩家已死亡");
                return false;
            }

            if (!GameManager.instance.IsGameplayScene())
            {
                Console.AddLine("无法增加/扣除生命值：当前不是游戏场景");
                return false;
            }

            return true;
        }

        [BindableMethod(name = "生命值+1", category = "面甲 & 灵丝轴")]
        public static void AddHealth()
        {
            if (CanModifyHealth(PlayerData.instance.health + 1))
            {
                HeroController.instance.AddHealth(1);
                HudHelper.RefreshMasks();

                Console.AddLine("已增加生命值");
            }
        }

        [BindableMethod(name = "生命值-1", category = "面甲 & 灵丝轴")]
        public static void TakeHealth()
        {
            if (CanModifyHealth(PlayerData.instance.health - 1))
            {
                HeroController.instance.TakeHealth(1);
                HudHelper.RefreshMasks();

                Console.AddLine("已减少生命值");
            }
        }
        
        [BindableMethod(name = "灵丝+1", category = "面甲 & 灵丝轴")]
        public static void AddSilk()
        {
            HeroController.instance.AddSilk(1, true);

            Console.AddLine("已增加灵丝");
        }

        [BindableMethod(name = "灵丝-1", category = "面甲 & 灵丝轴")]
        public static void TakeSilk()
        {
            HeroController.instance.TakeSilk(1);

            Console.AddLine("已减少灵丝");
        }

        [BindableMethod(name = "生命血+1", category = "面甲 & 灵丝轴")]
        public static void Lifeblood()
        {
            EventRegister.SendEvent("ADD BLUE HEALTH");

            Console.AddLine("正在增加生命血");
        }
    }
}
