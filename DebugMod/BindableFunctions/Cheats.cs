using UnityEngine;
using Object = UnityEngine.Object;

namespace DebugMod
{
    public static partial class BindableFunctions
    {
        [BindableMethod(name = "全杀了", category = "作弊")]
        public static void KillAll()
        {
            int ctr = 0;

            foreach (HealthManager hm in Object.FindObjectsOfType<HealthManager>())
            {
                if (!hm.isDead)
                {
                    hm.Die(null, AttackTypes.Generic, true);
                    ctr++;
                }
            }
            Console.AddLine($"Killing {ctr} HealthManagers in scene!");
        }

        [BindableMethod(name = "无限跳", category = "作弊")]
        public static void ToggleInfiniteJump()
        {
            PlayerData.instance.infiniteAirJump = !PlayerData.instance.infiniteAirJump;
            Console.AddLine("无限跳 set to " + PlayerData.instance.infiniteAirJump.ToString().ToUpper());
        }

        [BindableMethod(name = "无限丝", category = "作弊")]
        public static void ToggleInfiniteSilk()
        {
            DebugMod.infiniteSilk = !DebugMod.infiniteSilk;
            Console.AddLine("无限丝 set to " + DebugMod.infiniteSilk.ToString().ToUpper());
        }

        [BindableMethod(name = "无限血", category = "作弊")]
        public static void ToggleInfiniteHP()
        {
            DebugMod.infiniteHP = !DebugMod.infiniteHP;
            Console.AddLine("无限血 set to " + DebugMod.infiniteHP.ToString().ToUpper());
        }

        [BindableMethod(name = "无敌", category = "作弊")]
        public static void ToggleInvincibility()
        {
            DebugMod.playerInvincible = !DebugMod.playerInvincible;
            Console.AddLine("无敌 set to " + DebugMod.playerInvincible.ToString().ToUpper());

            PlayerData.instance.isInvincible = DebugMod.playerInvincible;
        }

        [BindableMethod(name = "飞天遁地", category = "作弊")]
        public static void ToggleNoclip()
        {
            DebugMod.noclip = !DebugMod.noclip;

            if (DebugMod.noclip)
            {
                Console.AddLine("开启飞天遁地");
                DebugMod.noclipPos = DebugMod.RefKnight.transform.position;
            }
            else
            {
                Console.AddLine("关闭飞天遁地");
                DebugMod.RefKnight.GetComponent<Rigidbody2D>().constraints &= ~RigidbodyConstraints2D.FreezePosition;
            }
        }

        [BindableMethod(name = "切换大黄蜂碰撞体积", category = "作弊")]
        public static void ToggleHeroCollider()
        {
            if (!DebugMod.RefHeroCollider.enabled)
            {
                DebugMod.RefHeroCollider.enabled = true;
                DebugMod.RefHeroBox.enabled = true;
                Console.AddLine("切换碰撞体积" + (DebugMod.noclip ? " 和取消飞天遁地" : ""));
                DebugMod.noclip = false;
            }
            else
            {
                DebugMod.RefHeroCollider.enabled = false;
                DebugMod.RefHeroBox.enabled = false;
                Console.AddLine("取消切换碰撞体积" + (DebugMod.noclip ? "" : " 和启用飞天遁地"));
                DebugMod.noclip = true;
                DebugMod.noclipPos = DebugMod.RefKnight.transform.position;
            }
        }

        [BindableMethod(name = "自杀", category = "作弊")]
        public static void KillSelf()
        {
            if (!HeroController.instance.cState.dead && !HeroController.instance.cState.transitioning)
            {
                HeroController.instance.StartCoroutine(HeroController.instance.Die(false, false));
                Console.AddLine("自杀");
            }
        }
    }
}