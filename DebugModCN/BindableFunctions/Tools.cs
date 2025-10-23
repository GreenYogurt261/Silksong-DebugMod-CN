using System.Linq;

namespace DebugMod
{
    public static partial class BindableFunctions
    {
        [BindableMethod(name = "解锁所有工具", category = "工具")]
        public static void UnlockAllTools()
        {
            ToolItemManager.UnlockAllTools();
            Console.AddLine("已解锁所有工具");
        }

        [BindableMethod(name = "解锁所有纹章", category = "工具")]
        public static void UnlockAllCrests()
        {
            ToolItemManager.UnlockAllCrests();

            if (ToolItemManager.Instance && ToolItemManager.Instance.crestList)
            {
                foreach (ToolCrest crest in ToolItemManager.Instance.crestList)
                {
                    crest.slots = crest.slots.Select(slotInfo => slotInfo with { IsLocked = false }).ToArray();

                    ToolCrestsData.Data crestData = crest.SaveData;
                    if (crestData.Slots != null)
                    {
                        crestData.Slots = crestData.Slots.Select(slot => slot with { IsUnlocked = true }).ToList();
                    }
                    crest.SaveData = crestData;
                }
            }

            Console.AddLine("已解锁所有纹章");
        }

        [BindableMethod(name = "增加工具袋容量", category = "工具")]
        public static void IncrementPouches()
        {
            if (PlayerData.instance.ToolPouchUpgrades < 4)
            {
                PlayerData.instance.ToolPouchUpgrades++;
                Console.AddLine($"Increasing tool pouch level (now {PlayerData.instance.ToolPouchUpgrades})");
            }
            else
            {
                PlayerData.instance.ToolPouchUpgrades = 0;
                Console.AddLine("已重置工具袋容量等级");
            }
        }

        [BindableMethod(name = "增加工具袋伤害", category = "工具")]
        public static void IncrementKits()
        {
            if (PlayerData.instance.ToolKitUpgrades < 4)
            {
                PlayerData.instance.ToolKitUpgrades++;
                Console.AddLine($"Increasing crafting kit level (now {PlayerData.instance.ToolKitUpgrades})");
            }
            else
            {
                PlayerData.instance.ToolKitUpgrades = 0;
                Console.AddLine("已重置工具袋伤害等级");
            }
        }

        [BindableMethod(name = "无限工具", category = "工具")]
        public static void ToggleInfiniteTools()
        {
            DebugMod.infiniteTools = !DebugMod.infiniteTools;
            Console.AddLine("无限工具设置于 " + DebugMod.infiniteTools.ToString().ToUpper());
        }

        [BindableMethod(name = "制造工具", category = "工具")]
        public static void CraftTools()
        {
            ToolItemManager.TryReplenishTools(true, ToolItemManager.ReplenishMethod.Bench);
            Console.AddLine("已制造新工具");
        }
    }
}