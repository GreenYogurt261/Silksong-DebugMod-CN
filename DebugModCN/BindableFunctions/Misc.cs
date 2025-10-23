using System;
using System.Collections;
using System.Reflection;
using DebugMod.MonoBehaviours;
using HarmonyLib;
using UnityEngine;

namespace DebugMod
{
    [HarmonyPatch]
    public static partial class BindableFunctions
    {
        private static readonly FieldInfo TimeSlowed = typeof(GameManager).GetField("timeSlowed", BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static);
        private static readonly FieldInfo IgnoreUnpause = typeof(UIManager).GetField("ignoreUnpause", BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static);
        internal static readonly FieldInfo cameraGameplayScene = typeof(CameraController).GetField("isGameplayScene", BindingFlags.Instance | BindingFlags.NonPublic);
        private static float TimeScaleDuringFrameAdvance = 0f;
        internal static int frameCounter = 0;

        [BindableMethod(name = "强制暂停", category = "杂项")]
        public static void ForcePause()
        {
            try
            {
                if ((PlayerData.instance.disablePause || (bool) TimeSlowed.GetValue(GameManager.instance) ||
                     (bool) IgnoreUnpause.GetValue(UIManager.instance)) && DebugMod.GetSceneName() != "Menu_Title" &&
                    DebugMod.GM.IsGameplayScene())
                {
                    TimeSlowed.SetValue(GameManager.instance, false);
                    IgnoreUnpause.SetValue(UIManager.instance, false);
                    PlayerData.instance.disablePause = false;
                    UIManager.instance.TogglePauseGame();
                    Console.AddLine("由于暂停功能被禁用，正在强制打开暂停菜单");
                }
                else
                {
                    Console.AddLine("游戏未反馈暂停功能被禁用，正在正常请求暂停");
                    UIManager.instance.TogglePauseGame();
                }
            }
            catch (Exception e)
            {
                Console.AddLine("尝试暂停时发生错误，请检查 ModLog.txt 文件");
                DebugMod.instance.Log("尝试暂停时发生错误:\n" + e);
            }
        }

        [BindableMethod(name = "场景存档点重生", category = "杂项")]
        public static void Respawn()
        {
            if (GameManager.instance.IsGameplayScene() && !HeroController.instance.cState.dead &&
                PlayerData.instance.health > 0)
            {
                if (UIManager.instance.uiState.ToString() == "PAUSED")
                {
                    InputHandler.Instance.StartCoroutine(GameManager.instance.PauseGameToggle(false));
                    GameManager.instance.HazardRespawn();
                    Console.AddLine("正在关闭暂停菜单并重生...");
                    return;
                }

                if (UIManager.instance.uiState.ToString() == "PLAYING")
                {
                    HeroController.instance.RelinquishControl();
                    GameManager.instance.HazardRespawn();
                    HeroController.instance.RegainControl();
                    Console.AddLine("重生信号已发送");
                    return;
                }

                Console.AddLine("在异常情况下请求重生，终止，立即终止！");
            }
        }

        [BindableMethod(name = "设置场景重生点", category = "杂项")]
        public static void SetHazardRespawn()
        {
            Vector3 manualRespawn = DebugMod.RefKnight.transform.position;
            HeroController.instance.SetHazardRespawn(manualRespawn, false);
            Console.AddLine("Manual respawn point on this map set to" + manualRespawn);
        }

        [BindableMethod(name = "切换到第三幕", category = "杂项")]
        public static void ToggleAct3()
        {
            PlayerData.instance.blackThreadWorld = !PlayerData.instance.blackThreadWorld;
            Console.AddLine("Act 3 world is now " + (PlayerData.instance.blackThreadWorld ? "enabled" : "disabled"));
        }

        [BindableMethod(name = "强制视角跟随", category = "杂项")]
        public static void ForceCameraFollow()
        {
            if (!DebugMod.cameraFollow)
            {
                Console.AddLine("强制视角跟随");
                DebugMod.cameraFollow = true;
            }
            else
            {
                DebugMod.cameraFollow = false;
                cameraGameplayScene.SetValue(DebugMod.RefCamera, true);
                Console.AddLine("正在将相机恢复为正常设置");
            }
        }

        [BindableMethod(name = "清除白屏", category = "杂项")]
        public static void ClearWhiteScreen()
        {
            //fix white screen 
            PlayMakerFSM wakeFSM = HeroController.instance.gameObject.LocateMyFSM("Dream Return");
            wakeFSM.SetState("GET UP");
            wakeFSM.SendEvent("FINISHED");
            GameObject.Find("Blanker White").LocateMyFSM("Blanker Control").SendEvent("FADE OUT");
            HeroController.instance.EnableRenderer();
        }

        private static string saveLevelStateAction;

        [BindableMethod(name = "重置场景数据", category = "杂项")]
        public static void ResetCurrentScene()
        {
            saveLevelStateAction = GameManager.instance.GetSceneNameString();
            Console.AddLine("正在清除此场景的数据，请重新进入场景或传送以使更改生效");
        }

        [BindableMethod(name = "停止场景数据更改", category = "杂项")]
        public static void BlockCurrentSceneChanges()
        {
            saveLevelStateAction = "block";
            Console.AddLine("进入此场景后所做的场景数据更改将不会被保存");
        }

        [HarmonyPatch(typeof(GameManager), nameof(GameManager.SaveLevelState))]
        [HarmonyPrefix]
        private static bool GameManager_SaveLevelState_Prefix()
        {
            if (saveLevelStateAction == "block")
            {
                saveLevelStateAction = null;
                return false;
            }

            return true;
        }

        [HarmonyPatch(typeof(GameManager), nameof(GameManager.SaveLevelState))]
        [HarmonyPostfix]
        private static void GameManager_SaveLevelState_Postfix()
        {
            if (saveLevelStateAction != null && saveLevelStateAction != "block")
            {
                SceneData.instance.persistentBools.scenes.Remove(saveLevelStateAction);
                SceneData.instance.persistentInts.scenes.Remove(saveLevelStateAction);
                SceneData.instance.geoRocks.scenes.Remove(saveLevelStateAction);

                saveLevelStateAction = null;
            }
        }

        [BindableMethod(name = "回收丝茧", category = "杂项")]
        public static void BreakCocoon()
        {
            HeroController.instance?.CocoonBroken();
            EventRegister.SendEvent("BREAK HERO CORPSE");
        }

        [BindableMethod(name = "开始/结束 逐帧前进", category = "杂项")]
        public static void ToggleFrameAdvance()
        {
            frameCounter = 0;
            if (Time.timeScale != 0)
            {
                if (GameManager.instance.GetComponent<TimeScale>() == null)
                    GameManager.instance.gameObject.AddComponent<TimeScale>();
                Time.timeScale = 0f;
                TimeScaleDuringFrameAdvance = DebugMod.CurrentTimeScale;
                DebugMod.CurrentTimeScale = 0;
                DebugMod.TimeScaleActive = true;
                Console.AddLine("正在按键逐帧前进中");
            }
            else
            {
                DebugMod.CurrentTimeScale = TimeScaleDuringFrameAdvance;
                Time.timeScale = DebugMod.CurrentTimeScale;
                Console.AddLine("正在停止按键逐帧前进");
            }
        }

        [BindableMethod(name = "前进一帧", category = "杂项")]
        public static void AdvanceFrame()
        {
            if (Time.timeScale != 0) ToggleFrameAdvance();
            frameCounter++;
            GameManager.instance.StartCoroutine(AdvanceMyFrame());
        }

        private static IEnumerator AdvanceMyFrame()
        {
            DebugMod.CurrentTimeScale = Time.timeScale = 1f;
            yield return new WaitForFixedUpdate();

            DebugMod.CurrentTimeScale = Time.timeScale = 0f;
        }

        [BindableMethod(name = "重置计数器", category = "杂项")]
        public static void ResetCounter()
        {
            frameCounter = 0;
        }

        [BindableMethod(name = "锁定按键绑定", category = "杂项")]
        public static void ToggleLockKeyBinds()
        {
            DebugMod.KeyBindLock = !DebugMod.KeyBindLock;
            Console.AddLine($"{(DebugMod.KeyBindLock ? "Removing" : "Adding")} the ability to use keybinds");
        }

    }
}