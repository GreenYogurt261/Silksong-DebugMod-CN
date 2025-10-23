using System;
using DebugMod.MonoBehaviours;
using GlobalEnums;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DebugMod
{
    public static partial class BindableFunctions
    {
        [BindableMethod(name = "织针伤害+4", category = "玩法修改")]
        public static void IncreaseNeedleDamage()
        {
            if (PlayerData.instance.nailDamage == 0)
            {
                PlayerData.instance.nailUpgrades = 0;
                DebugMod.extraNailDamage = 0;
                Console.AddLine("正在将针刺伤害重置为 5");
            }
            else if (PlayerData.instance.nailUpgrades == 4 || DebugMod.extraNailDamage < 0)
            {
                DebugMod.extraNailDamage += 4;
                Console.AddLine("增加4点织针伤害");
            }
            else
            {
                PlayerData.instance.nailUpgrades++;
                Console.AddLine("织针升级已添加");
            }

            PlayMakerFSM.BroadcastEvent("UPDATE NAIL DAMAGE");
        }

        [BindableMethod(name = "织针伤害-4", category = "玩法修改")]
        public static void DecreaseNeedleDamage()
        {
            if (PlayerData.instance.nailUpgrades == 0 || DebugMod.extraNailDamage > 0)
            {
                DebugMod.extraNailDamage -= 4;
                if (DebugMod.extraNailDamage < -5)
                {
                    DebugMod.extraNailDamage = -5;
                    Console.AddLine("织针伤害已为0");
                }
                else
                {
                    Console.AddLine("减少4点织针伤害");
                }
            }
            else
            {
                PlayerData.instance.nailUpgrades--;
                Console.AddLine("针刺升级已移除");
            }

            PlayMakerFSM.BroadcastEvent("UPDATE NAIL DAMAGE");
        }
        
        [BindableMethod(name = "降低游戏时间流速（减速）", category = "玩法修改")]
        public static void TimescaleDown()
        {
            //This needs to be added because the game sets timescale to 0 when paused to pause the game if this is changed to a 
            //non-zero value, the game continues to play even tho the pause menu is up which is scuffed so this makes it less skuffed
            if (DebugMod.GM.IsGamePaused())
            {
                Console.AddLine("暂停时无法修改游戏时间流速");
                return;
            }
            float oldScale = Time.timeScale;
            bool wasTimeScaleActive = DebugMod.TimeScaleActive;
            Time.timeScale = Time.timeScale = Mathf.Round(Time.timeScale * 10 - 1f) / 10f;
            DebugMod.CurrentTimeScale = Time.timeScale;

            DebugMod.TimeScaleActive = Math.Abs(DebugMod.CurrentTimeScale - 1f) > Mathf.Epsilon;

            switch (DebugMod.TimeScaleActive)
            {
                case true when wasTimeScaleActive == false:
                    if (GameManager.instance.GetComponent<TimeScale>() == null) GameManager.instance.gameObject.AddComponent<TimeScale>();
                    break;
                case false when wasTimeScaleActive:
                    if (GameManager.instance.GetComponent<TimeScale>() != null)
                    {
                        Object.Destroy(GameManager.instance.gameObject.GetComponent<TimeScale>());
                    }

                    break;
            }
            Console.AddLine("新时间流速值: " + DebugMod.CurrentTimeScale + " 旧值: " + oldScale);

        }

        [BindableMethod(name = "增加游戏时间流速（加速）", category = "玩法修改")]
        public static void TimescaleUp()
        {
            if (DebugMod.GM.IsGamePaused())
            {
                Console.AddLine("暂停时无法修改游戏时间流速");
                return;
            }
            float oldScale = Time.timeScale;
            bool wasTimeScaleActive = DebugMod.TimeScaleActive;
            Time.timeScale = Time.timeScale = Mathf.Round(Time.timeScale * 10 + 1f) / 10f;
            DebugMod.CurrentTimeScale = Time.timeScale;

            DebugMod.TimeScaleActive = Math.Abs(DebugMod.CurrentTimeScale - 1f) > Mathf.Epsilon;

            switch (DebugMod.TimeScaleActive)
            {
                case true when wasTimeScaleActive == false:
                    if (GameManager.instance.GetComponent<TimeScale>() == null) GameManager.instance.gameObject.AddComponent<TimeScale>();
                    break;
                case false when wasTimeScaleActive:
                    if (GameManager.instance.GetComponent<TimeScale>() != null)
                    {
                        Object.Destroy(GameManager.instance.gameObject.GetComponent<TimeScale>());
                    }

                    break;
            }
            Console.AddLine("新时间流速值: " + DebugMod.CurrentTimeScale + " 旧值: " + oldScale);
        }

        [BindableMethod(name = "冻结游戏时间", category = "玩法修改")]
        public static void PauseGameNoUI()
        {
            DebugMod.PauseGameNoUIActive = !DebugMod.PauseGameNoUIActive;

            if (DebugMod.PauseGameNoUIActive)
            {
                Time.timeScale = 0;
                GameCameras.instance.StopCameraShake();
                SetAlwaysShowCursor();
                Console.AddLine("游戏已冻结");
            }
            else
            {
                GameCameras.instance.ResumeCameraShake();
                GameManager.instance.isPaused = false;
                GameManager.instance.ui.SetState(UIState.PLAYING);
                GameManager.instance.SetState(GameState.PLAYING);
                if (HeroController.instance != null) HeroController.instance.UnPause();
                Time.timeScale = DebugMod.CurrentTimeScale;
                GameManager.instance.inputHandler.AllowPause();

                if (!DebugMod.settings.ShowCursorWhileUnpaused)
                {
                    UnsetAlwaysShowCursor();
                }
                
                Console.AddLine("游戏冻结已解除");
            }
        }

        [BindableMethod(name = "重置游戏各参数", category = "玩法修改")]
        public static void Reset()
        {
            var pd = PlayerData.instance;
            var HC = HeroController.instance;
            var GC = GameCameras.instance;
            
            //nail damage
            DebugMod.extraNailDamage = 0;
            PlayerData.instance.nailUpgrades = 0;
            PlayMakerFSM.BroadcastEvent("UPDATE NAIL DAMAGE");

            //Hero Light
            GameObject gameObject = DebugMod.RefKnight.transform.Find("HeroLight").gameObject;
            Color color = gameObject.GetComponent<SpriteRenderer>().color;
            color.a = 0.7f;
            gameObject.GetComponent<SpriteRenderer>().color = color;
            
            //HUD
            // if (!GC.hudCanvas.gameObject.activeInHierarchy) 
            //     GC.hudCanvas.gameObject.SetActive(true);
            
            //Hide Hero
            tk2dSprite component = DebugMod.RefKnight.GetComponent<tk2dSprite>();
            color = component.color;  color.a = 1f;
            component.color = color;

            //rest all is self explanatory
            if (GameManager.instance.GetComponent<TimeScale>() != null)
            {
                Object.Destroy(GameManager.instance.gameObject.GetComponent<TimeScale>());
            }
            GC.tk2dCam.ZoomFactor = 1f;
            HC.vignette.enabled = false;
            EnemiesPanel.hpBars = false;
            pd.infiniteAirJump=false;
            DebugMod.infiniteSilk = false;
            DebugMod.infiniteHP = false; 
            pd.isInvincible=false; 
            DebugMod.noclip=false;
        }
    }
}