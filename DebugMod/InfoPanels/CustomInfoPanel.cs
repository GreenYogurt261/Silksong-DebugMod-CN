using System;
using System.Collections.Generic;
using DebugMod.Canvas;
using GlobalSettings;
using UnityEngine;

namespace DebugMod.InfoPanels
{
    public class CustomInfoPanel : TogglableInfoPanel
    {
        public bool ShowSprite;
        public CustomInfoPanel(string Name, bool ShowSprite) : base(Name)
        {
            this.ShowSprite = ShowSprite;
        }

        protected List<(float xLabel, float xInfo, float y, string label, Func<string> textFunc)> PanelBuildInfo = new();
        private Dictionary<string, Func<string>> UpdateActions;
        public override void BuildPanel(GameObject canvas)
        {
            if (ShowSprite)
            {
                panel = new CanvasPanel(
                    canvas,
                    GUIController.Instance.images["StatusPanelBG"],
                    new Vector2(0f, 223f),
                    Vector2.zero,
                    new Rect(
                        0f,
                        0f,
                        GUIController.Instance.images["StatusPanelBG"].width,
                        GUIController.Instance.images["StatusPanelBG"].height));
            }
            else
            {
                Texture2D tex = new Texture2D(1, 1);
                tex.LoadRawTextureData(new byte[] { 0x00, 0x00, 0x00, 0x00 });
                tex.Apply();

                // Puke
                panel = new CanvasPanel(
                    canvas,
                    tex,
                    new Vector2(130f, 230f),
                    Vector2.zero,
                    new Rect(
                        0f,
                        0f,
                        1f,
                        1f));
            }


            UpdateActions = new();
            int counter = 0;

            foreach ((float xLabel, float xInfo, float y, string label, Func<string> textFunc) in PanelBuildInfo)
            {
                panel.AddText($"Label-{counter++}", label, new Vector2(xLabel, y), Vector2.zero, GUIController.Instance.arial, 15);
                panel.AddText($"Info-{counter}", "", new Vector2(xInfo, y + 4f), Vector2.zero, GUIController.Instance.trajanNormal);
                UpdateActions.Add($"Info-{counter}", textFunc);
            }

            panel.FixRenderOrder();
        }
        public override void UpdatePanel()
        {
            if (panel == null) return;

            foreach (var kvp in UpdateActions)
            {
                panel.GetText(kvp.Key).UpdateText(kvp.Value.Invoke());
            }
        }

        public void AddInfo(float xLabel, float xInfo, float y, string label, Func<string> textFunc)
            => PanelBuildInfo.Add((xLabel, xInfo, y, label, textFunc));

        internal static CustomInfoPanel GetMainInfoPanel()
        {
            CustomInfoPanel MainInfoPanel = new CustomInfoPanel(InfoPanel.MainInfoPanelName, true);

            float y = 0f;

            MainInfoPanel.AddInfo(10f, 150f, y += 20, "大黄蜂状态", () => HeroController.instance.hero_state.ToString());
            MainInfoPanel.AddInfo(10f, 150f, y += 20, "速度", () => HeroController.instance.current_velocity.ToString());
            MainInfoPanel.AddInfo(10f, 150f, y += 20, "生命值", () => PlayerData.instance.health + " / " + PlayerData.instance.maxHealth);
            MainInfoPanel.AddInfo(10f, 150f, y += 20, "灵丝值", () => PlayerData.instance.silk + " / " + PlayerData.instance.CurrentSilkMaxBasic);
            y += 25f;
            MainInfoPanel.AddInfo(10f, 150f, y += 20, "完成度", () => PlayerData.instance.completionPercentage + "%");
            MainInfoPanel.AddInfo(10f, 150f, y += 20, "跳蚤", () => Gameplay.FleasCollectedCount + " / 30");
            MainInfoPanel.AddInfo(10, 150f, y += 20, "祈愿", () => QuestManager.GetQuest("Soul Snare Pre").requiredCompleteTotalGroups[0].CurrentValueCount
                + " / " + QuestManager.GetQuest("Soul Snare Pre").requiredCompleteTotalGroups[0].target);
            y += 25f;
            MainInfoPanel.AddInfo(10f, 150f, y += 20, "织针伤害", () => PlayerData.instance.nailDamage.ToString());
            MainInfoPanel.AddInfo(10f, 150f, y += 20, "最后伤害", () => DebugMod.lastHit != null ?
                $"{DebugMod.lastDamage} ({DebugMod.lastHit?.DamageDealt} x {DebugMod.lastHit?.Multiplier})" : "None");
            MainInfoPanel.AddInfo(10f, 150f, y += 20, "最后类型", () => DebugMod.lastHit?.AttackType.ToString() ?? "None");
            MainInfoPanel.AddInfo(10f, 150f, y += 20, "最后缩放", () => DebugMod.lastHit != null ? DebugMod.lastScaling.ToString() : "None");
            y += 25f;
            MainInfoPanel.AddInfo(10f, 150f, y += 20, "无敌状态", () => GetStringForBool(HeroController.instance.cState.Invulnerable));
            MainInfoPanel.AddInfo(10f, 150f, y += 20, "完全无敌", () => GetStringForBool(PlayerData.instance.isInvincible));
            MainInfoPanel.AddInfo(10f, 150f, y += 20, "受伤状态", () => HeroController.instance.damageMode.ToString());
            MainInfoPanel.AddInfo(10f, 150f, y += 20, "死亡状态", () => GetStringForBool(HeroController.instance.cState.dead));
            MainInfoPanel.AddInfo(10f, 150f, y += 20, "危险死亡", () => GetStringForBool(HeroController.instance.cState.hazardDeath));
            MainInfoPanel.AddInfo(10f, 150f, y += 20, "丝茧场景", () => string.IsNullOrEmpty(PlayerData.instance.HeroCorpseScene) ? "None" : PlayerData.instance.HeroCorpseScene);
            MainInfoPanel.AddInfo(10f, 150f, y += 20, "丝茧位置", () => string.IsNullOrEmpty(PlayerData.instance.HeroCorpseScene) ? "None" : PlayerData.instance.HeroDeathScenePos.ToString());
            y += 25f;
            MainInfoPanel.AddInfo(10f, 150f, y += 20, "场景名称", () => DebugMod.GetSceneName());
            MainInfoPanel.AddInfo(10f, 150f, y += 20, "过渡中", () => GetStringForBool(HeroController.instance.cState.transitioning));
            MainInfoPanel.AddInfo(10f, 150f, y += 20, "过渡状态", () => GetTransState());
            MainInfoPanel.AddInfo(10f, 150f, y += 20, "游戏状态", () => GameManager.instance.GameState.ToString());
            MainInfoPanel.AddInfo(10f, 150f, y += 20, "界面状态", () => UIManager.instance.uiState.ToString());
            MainInfoPanel.AddInfo(10f, 150f, y += 20, "相机模式", () => DebugMod.RefCamera.mode.ToString());

            y = 10f;

            MainInfoPanel.AddInfo(300f, 440f, y += 20, "接受输入", () => GetStringForBool(HeroController.instance.acceptingInput));
            MainInfoPanel.AddInfo(300f, 440f, y += 20, "控制", () => GetStringForBool(HeroController.instance.controlReqlinquished));
            MainInfoPanel.AddInfo(300f, 440f, y += 20, "在长椅", () => GetStringForBool(PlayerData.instance.atBench));
            y += 20f;
            MainInfoPanel.AddInfo(300f, 440f, y += 20, "冲刺中", () => GetStringForBool(HeroController.instance.cState.dashing));
            MainInfoPanel.AddInfo(300f, 440f, y += 20, "奔跑中", () => GetStringForBool(HeroController.instance.cState.isSprinting));
            MainInfoPanel.AddInfo(300f, 440f, y += 20, "跳跃中", () => GetStringForBool((HeroController.instance.cState.jumping || HeroController.instance.cState.doubleJumping)));
            MainInfoPanel.AddInfo(300f, 440f, y += 20, "灵丝升腾中", () => GetStringForBool(HeroController.instance.cState.superDashing));
            MainInfoPanel.AddInfo(300f, 440f, y += 20, "下落中", () => GetStringForBool(HeroController.instance.cState.falling));
            MainInfoPanel.AddInfo(300f, 440f, y += 20, "落地硬直", () => GetStringForBool(HeroController.instance.cState.willHardLand));
            MainInfoPanel.AddInfo(300f, 440f, y += 20, "游泳中", () => GetStringForBool(HeroController.instance.cState.swimming));
            MainInfoPanel.AddInfo(300f, 440f, y += 20, "击退中", () => GetStringForBool(HeroController.instance.cState.recoiling));
            y += 20f;
            MainInfoPanel.AddInfo(300f, 440f, y += 20, "贴墙挂壁", () => GetStringForBool(HeroController.instance.wallLocked));
            MainInfoPanel.AddInfo(300f, 440f, y += 20, "贴墙跳跃", () => GetStringForBool(HeroController.instance.cState.wallJumping));
            MainInfoPanel.AddInfo(300f, 440f, y += 20, "接触墙壁", () => GetStringForBool(HeroController.instance.cState.touchingWall));
            MainInfoPanel.AddInfo(300f, 440f, y += 20, "贴墙滑落", () => GetStringForBool(HeroController.instance.cState.wallSliding));
            MainInfoPanel.AddInfo(300f, 440f, y += 20, "左侧墙壁", () => GetStringForBool(HeroController.instance.touchingWallL));
            MainInfoPanel.AddInfo(300f, 440f, y += 20, "右侧墙壁", () => GetStringForBool(HeroController.instance.touchingWallR));
            y += 20f;
            MainInfoPanel.AddInfo(300f, 440f, y += 20, "攻击中", () => GetStringForBool(HeroController.instance.cState.attacking));
            MainInfoPanel.AddInfo(300f, 440f, y += 20, "可释放法术", () => GetStringForBool(HeroController.instance.CanCast()));
            MainInfoPanel.AddInfo(300f, 440f, y += 20, "可灵丝升腾", () => GetStringForBool(HeroController.instance.CanSuperJump()));
            MainInfoPanel.AddInfo(300f, 440f, y += 20, "可快捷地图", () => GetStringForBool(HeroController.instance.CanQuickMap()));
            MainInfoPanel.AddInfo(300f, 440f, y += 20, "可打开背包", () => GetStringForBool(HeroController.instance.CanOpenInventory()));
            y += 20f;
            MainInfoPanel.AddInfo(300f, 440f, y += 20, "是游戏场景", () => GetStringForBool(DebugMod.GM.IsGameplayScene()));
            MainInfoPanel.AddInfo(300f, 440f, y += 20, "大黄蜂已暂停", () => GetStringForBool(HeroController.instance.cState.isPaused));

            return MainInfoPanel;
        }

        internal static CustomInfoPanel GetMinimalInfoPanel()
        {
            CustomInfoPanel MinimalInfoPanel = new CustomInfoPanel(InfoPanel.MinimalInfoPanelName, false);

            MinimalInfoPanel.AddInfo(10, 40, 10, "Vel", () => HeroController.instance.current_velocity.ToString());
            MinimalInfoPanel.AddInfo(110, 140, 10, "Pos", () => GetHeroPos());
            
            MinimalInfoPanel.AddInfo(10, 40, 30, "MP", () => (PlayerData.instance.silk).ToString());
            
            MinimalInfoPanel.AddInfo(10, 100, 50, "NailDmg", () => PlayerData.instance.nailDamage.ToString());
            
            MinimalInfoPanel.AddInfo(10, 95, 70, "Completion", () => PlayerData.instance.completionPercentage + "%");
            MinimalInfoPanel.AddInfo(140, 195, 70, "Fleas", () => Gameplay.FleasCollectedCount + " / 30");
            
            MinimalInfoPanel.AddInfo(10, 140, 90, "Scene Name", () => DebugMod.GetSceneName());
            MinimalInfoPanel.AddInfo(10, 140, 110, "Current SaveState", () => SaveStateManager.quickState.IsSet() ? SaveStateManager.quickState.GetSaveStateID() : "No savestate");
            MinimalInfoPanel.AddInfo(110, 200, 130, "Current slot", GetCurrentSlotString);
            MinimalInfoPanel.AddInfo(10, 80, 130, "Hardfall", () => GetStringForBool(HeroController.instance.cState.willHardLand));

            return MinimalInfoPanel;
        }

        private static string GetCurrentSlotString()
        {
            string slotSet = SaveStateManager.GetCurrentSlot().ToString();
            if (slotSet == "-1")
            {
                slotSet = "unset";
            }
            return slotSet;
        }
    }
}
