namespace DebugMod
{
    public static partial class BindableFunctions
    {
        [BindableMethod(name = "获取念珠", category = "物品")]
        public static void GiveRosaries()
        {
            HeroController.instance.AddGeo(1000);
            Console.AddLine("已获取1000念珠");
        }

        [BindableMethod(name = "获取兽甲碎片", category = "物品")]
        public static void GiveShellShards()
        {
            HeroController.instance.AddShards(100);
            Console.AddLine("已获取1000兽甲碎片");
        }

        private static void SetCollectable(string name, int amount)
        {
            if (CollectableItemManager.IsInHiddenMode())
            {
                CollectableItemManager.Instance.AffectItemData(name, (ref CollectableItemsData.Data data) => data.AmountWhileHidden = amount);
            }
            else
            {
                CollectableItemManager.Instance.AffectItemData(name, (ref CollectableItemsData.Data data) => data.Amount = amount);
            }
        }

        [BindableMethod(name = "全忆境纪念盒", category = "物品")]
        public static void GiveMemoryLockets()
        {
            SetCollectable("Crest Socket Unlocker", 20);
            Console.AddLine("已获得20个忆境纪念盒");
        }

        [BindableMethod(name = "获取制造金属", category = "物品")]
        public static void GiveCraftmetal()
        {
            SetCollectable("制造金属", 8);
            Console.AddLine("已获得8个制造金属");
        }

        [BindableMethod(name = "获得噬丝蛆", category = "物品")]
        public static void GiveSilkeater()
        {
            CollectableItemManager.GetItemByName("Silk Grub").AddAmount(1);
            Console.AddLine("已获得1个噬丝蛆");
        }

        // TODO: add bind to give all items needed for the active quest(s)
    }
}