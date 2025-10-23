using System;
using System.IO;

namespace DebugMod
{
    public static partial class BindableFunctions
    {
         [BindableMethod(name = "快速存档（存储）", category = "存档状态")]
        public static void SaveState()
        {
            DebugMod.saveStateManager.SaveSaveState(SaveStateType.Memory);
        }

        [BindableMethod(name = "快速存档（加载）", category = "存档状态")]
        public static void LoadState()
        {
            DebugMod.saveStateManager.LoadSaveState(SaveStateType.Memory);
        }

        //TODO: Allow these binds to override each other properly
        [BindableMethod(name = "快速存档保存至文件", category = "存档状态")]
        public static void CurrentSaveStateToFile()
        {
            if (SaveStateManager.currentStateOperation != "Quickslot save to file") DebugMod.saveStateManager.SaveSaveState(SaveStateType.File);
            else DebugMod.settings.ClearSaveStatePanel = true;
        }

        [BindableMethod(name = "从文件中加载至快速存档", category = "存档状态")]
        public static void CurrentSlotToSaveMemory()
        {
            if (SaveStateManager.currentStateOperation != "Load file to quickslot") DebugMod.saveStateManager.LoadSaveState(SaveStateType.File);
            else DebugMod.settings.ClearSaveStatePanel = true;
        }

        [BindableMethod(name = "保存新存档到存档页", category = "存档状态")]
        public static void NewSaveStateToFile()
        {
            if (SaveStateManager.currentStateOperation != "Save new state to file") DebugMod.saveStateManager.SaveSaveState(SaveStateType.SkipOne);
            else DebugMod.settings.ClearSaveStatePanel = true;
        }

        [BindableMethod(name = "从存档页中加载存档", category = "存档状态")]
        public static void LoadFromFile()
        {
            if (SaveStateManager.currentStateOperation != "Load new state from file") DebugMod.saveStateManager.LoadSaveState(SaveStateType.SkipOne);
            else DebugMod.settings.ClearSaveStatePanel = true;

        }

        [BindableMethod(name = "文件页往后翻页", category = "存档状态")]
        public static void NextStatePage()
        {
            if (!SaveStateManager.inSelectSlotState) return;
            
            SaveStateManager.currentStateFolder++;
            if (SaveStateManager.currentStateFolder >= SaveStateManager.savePages)
            {
                SaveStateManager.currentStateFolder = 0;
            } //rollback to 0 if higher than max

            SaveStateManager.path = Path.Combine(
                SaveStateManager.saveStatesBaseDirectory,
                SaveStateManager.currentStateFolder.ToString()); //change path
            DebugMod.saveStateManager.RefreshStateMenu(); // update menu
        }

        [BindableMethod(name = "文件页往前翻页", category = "存档状态")]
        public static void PrevStatePage()
        {
            if (!SaveStateManager.inSelectSlotState) return;
            
            SaveStateManager.currentStateFolder--;
            if (SaveStateManager.currentStateFolder < 0)
            {
                SaveStateManager.currentStateFolder = SaveStateManager.savePages - 1;
            } //rollback to max if we go below page 0

            SaveStateManager.path = Path.Combine(
                SaveStateManager.saveStatesBaseDirectory,
                SaveStateManager.currentStateFolder.ToString()); //change path
            DebugMod.saveStateManager.RefreshStateMenu(); // update menu
        }

        [BindableMethod(name = "死亡时加载存档状态", category = "存档状态")]
        public static void LoadStateOnDeath()
        {
            DebugMod.stateOnDeath = !DebugMod.stateOnDeath;
            Console.AddLine("Quickslot SaveState will now" + (DebugMod.stateOnDeath ? " be" : " no longer") + " loaded on death");
        }

        [BindableMethod(name = "覆盖加载锁定", category = "存档状态")]
        public static void OverrideLoadLockout()
        {
            DebugMod.overrideLoadLockout = !DebugMod.overrideLoadLockout;
            Console.AddLine("SaveState Lockout set to " + DebugMod.overrideLoadLockout);
        }

        /*
        [BindableMethod(name = "Toggle auto slot", category = "Savestates")]
        public static void ToggleAutoSlot()
        {
            DebugMod.saveStateManager.ToggleAutoSlot();
        }
        
        
        [BindableMethod(name = "Refresh state menu", category = "Savestates")]
        public static void RefreshSaveStates()
        {
            DebugMod.saveStateManager.RefreshStateMenu();
        }
        */
    }
}