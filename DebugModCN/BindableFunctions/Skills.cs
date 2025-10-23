namespace DebugMod
{
    public static partial class BindableFunctions
    {
        [BindableMethod(name = "给予全技能", category = "技能")]
        public static void GiveAllSkills()
        {
            PlayerData.instance.silkRegenMax = 3;

            PlayerData.instance.hasDash = true;
            PlayerData.instance.hasBrolly = true;
            PlayerData.instance.hasWalljump = true;
            PlayerData.instance.hasHarpoonDash = true;
            PlayerData.instance.hasDoubleJump = true;
            PlayerData.instance.hasSuperJump = true;

            PlayerData.instance.hasNeedolin = true;
            PlayerData.instance.UnlockedFastTravelTeleport = true;
            PlayerData.instance.hasNeedolinMemoryPowerup = true;

            PlayerData.instance.hasChargeSlash = true;

            Console.AddLine("正在给予玩家所有技能");
        }

        [BindableMethod(name = "丝之心升级", category = "技能")]
        public static void IncrementSilkHeart()
        {
            if (PlayerData.instance.silkRegenMax < 3)
            {
                PlayerData.instance.silkRegenMax++;
                Console.AddLine($"Giving player Silk Heart (now {PlayerData.instance.silkRegenMax})");
            }
            else
            {
                PlayerData.instance.silkRegenMax = 0;
                Console.AddLine("已移除玩家的丝之心");
            }
        }

        [BindableMethod(name = "给予疾风步", category = "技能")]
        public static void ToggleSwiftStep()
        {
            if (!PlayerData.instance.hasDash)
            {
                PlayerData.instance.hasDash = true;
                Console.AddLine("已给予玩家疾风步");
            }
            else
            {
                PlayerData.instance.hasDash = false;
                Console.AddLine("已移除玩家的疾风步");
            }
        }

        [BindableMethod(name = "切换斗篷等级", category = "技能")]
        public static void IncrementCloak()
        {
            if (!PlayerData.instance.hasBrolly && !PlayerData.instance.hasDoubleJump)
            {
                PlayerData.instance.hasBrolly = true;
                Console.AddLine("给予玩家流浪者的斗篷");
            }
            else if (PlayerData.instance.hasBrolly && !PlayerData.instance.hasDoubleJump)
            {
                PlayerData.instance.hasDoubleJump = true;
                Console.AddLine("已给予玩家幻羽披风");
            }
            else
            {
                PlayerData.instance.hasBrolly = false;
                PlayerData.instance.hasDoubleJump = false;
                Console.AddLine("取消斗篷升级");
            }
        }

        [BindableMethod(name = "给予蛛攀术", category = "技能")]
        public static void ToggleClingGrip()
        {
            if (!PlayerData.instance.hasWalljump)
            {
                PlayerData.instance.hasWalljump = true;
                Console.AddLine("已给予玩家蛛判术");
            }
            else
            {
                PlayerData.instance.hasWalljump = false;
                Console.AddLine("已移除玩家的蛛攀术");
            }
        }

        [BindableMethod(name = "给予织忆弦针", category = "技能")]
        public static void ToggleNeedolin()
        {
            if (!PlayerData.instance.hasNeedolin)
            {
                PlayerData.instance.hasNeedolin = true;
                Console.AddLine("已给予玩家织忆弦针");
            }
            else
            {
                PlayerData.instance.hasNeedolin = false;
                PlayerData.instance.UnlockedFastTravelTeleport = false;
                PlayerData.instance.hasNeedolinMemoryPowerup = false;
                Console.AddLine("已移除玩家的织忆弦针");
            }
        }

        [BindableMethod(name = "给予飞针冲刺", category = "技能")]
        public static void ToggleClawline()
        {
            if (!PlayerData.instance.hasHarpoonDash)
            {
                PlayerData.instance.hasHarpoonDash = true;
                Console.AddLine("已给予玩家飞针冲刺");
            }
            else
            {
                PlayerData.instance.hasHarpoonDash = false;
                Console.AddLine("已移除玩家的飞针冲刺");
            }
        }

        [BindableMethod(name = "给予灵丝升腾", category = "技能")]
        public static void ToggleSilkSoar()
        {
            if (!PlayerData.instance.hasSuperJump)
            {
                PlayerData.instance.hasSuperJump = true;
                Console.AddLine("已给予玩家灵丝升腾");
            }
            else
            {
                PlayerData.instance.hasSuperJump = false;
                Console.AddLine("已移除玩家的灵丝升腾");
            }
        }

        [BindableMethod(name = "给予唤兽曲", category = "技能")]
        public static void ToggleBeastlingCall()
        {
            if (!PlayerData.instance.hasNeedolin && !PlayerData.instance.UnlockedFastTravelTeleport)
            {
                PlayerData.instance.hasNeedolin = true;
                PlayerData.instance.UnlockedFastTravelTeleport = true;
                Console.AddLine("已给予玩家织忆弦针和唤兽曲");
            }
            else if (PlayerData.instance.hasNeedolin && !PlayerData.instance.UnlockedFastTravelTeleport)
            {
                PlayerData.instance.UnlockedFastTravelTeleport = true;
                Console.AddLine("已给予玩家唤兽曲");
            }
            else
            {
                PlayerData.instance.UnlockedFastTravelTeleport = false;
                Console.AddLine("已移除玩家的唤兽曲");
            }
        }

        [BindableMethod(name = "给予深邃挽歌", category = "技能")]
        public static void ToggleElegyOfTheDeep()
        {
            if (!PlayerData.instance.hasNeedolin && !PlayerData.instance.hasNeedolinMemoryPowerup)
            {
                PlayerData.instance.hasNeedolin = true;
                PlayerData.instance.hasNeedolinMemoryPowerup = true;
                Console.AddLine("已给予玩家织忆弦针和深邃挽歌");
            }
            else if (PlayerData.instance.hasNeedolin && !PlayerData.instance.hasNeedolinMemoryPowerup)
            {
                PlayerData.instance.hasNeedolinMemoryPowerup = true;
                Console.AddLine("已给予玩家深邃挽歌");
            }
            else
            {
                PlayerData.instance.hasNeedolinMemoryPowerup = false;
                Console.AddLine("已移除玩家的深邃挽歌");
            }
        }

        [BindableMethod(name = "给予蓄力斩", category = "技能")]
        public static void ToggleNeedleStrike()
        {
            if (!PlayerData.instance.hasChargeSlash)
            {
                PlayerData.instance.hasChargeSlash = true;
                Console.AddLine("已给予玩家蓄力斩");
            }
            else
            {
                PlayerData.instance.hasChargeSlash = false;
                Console.AddLine("已移除玩家的蓄力斩");
            }
        }
    }
}