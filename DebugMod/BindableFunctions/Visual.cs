using System;
using DebugMod.MethodHelpers;
using UnityEngine;

namespace DebugMod
{
    public static partial class BindableFunctions
    {

        [BindableMethod(name = "显示碰撞箱", category = "视觉")]
        public static void ShowHitboxes()
        {
            if (++DebugMod.settings.ShowHitBoxes > 2) DebugMod.settings.ShowHitBoxes = 0;
            Console.AddLine("碰撞箱显示已切换: " + DebugMod.settings.ShowHitBoxes);
        }

        [BindableMethod(name = "显示丝茧位置", category = "视觉")]
        public static void PreviewCocoonPosition()
        {
            CocoonPreviewer component = GameManager.instance.GetComponent<CocoonPreviewer>()
                ?? GameManager.instance.gameObject.AddComponent<CocoonPreviewer>();

            if (!component.previewEnabled)
            {
                component.previewEnabled = true;
                Console.AddLine("已显示丝茧生成点");
            }
            else
            {
                component.previewEnabled = false;
                Console.AddLine("已禁用显示丝茧生成点");
            }
        }

        [BindableMethod(name = "切换暗角效果", category = "视觉")]
        public static void ToggleVignette()
        {
            VisualMaskHelper.ToggleVignette();
        }

        [BindableMethod(name = "禁用视觉遮罩", category = "视觉")]
        public static void DoDeactivateVisualMasks()
        {
            MethodHelpers.VisualMaskHelper.ToggleAllMasks();
        }

        [BindableMethod(name = "切换大黄蜂光源", category = "视觉")]
        public static void ToggleHeroLight()
        {
            GameObject gameObject = DebugMod.RefKnight.transform.Find("HeroLight").gameObject;
            Color color = gameObject.GetComponent<SpriteRenderer>().color;
            if (Math.Abs(color.a) > 0f)
            {
                color.a = 0f;
                gameObject.GetComponent<SpriteRenderer>().color = color;
                Console.AddLine("正在渲染大黄蜂光源为不可见...");
            }
            else
            {
                color.a = 0.7f;
                gameObject.GetComponent<SpriteRenderer>().color = color;
                Console.AddLine("正在渲染大黄蜂光源为可见...");
            }
        }

        [BindableMethod(name = "切换HUD显示", category = "视觉")]
        public static void ToggleHUD()
        {
            if (GameCameras.instance.hudCanvasSlideOut.gameObject.activeInHierarchy)
            {
                GameCameras.instance.hudCanvasSlideOut.gameObject.SetActive(false);
                Console.AddLine("正在禁用HUD...");
            }
            else
            {
                GameCameras.instance.hudCanvasSlideOut.gameObject.SetActive(true);
                Console.AddLine("正在启用HUD...");
            }
        }

        [BindableMethod(name = "重置相机缩放", category = "视觉")]
        public static void ResetZoom()
        {
            GameCameras.instance.tk2dCam.ZoomFactor = 1f;
            Console.AddLine("缩放系数已重置");
        }

        [BindableMethod(name = "扩大相机视野", category = "视觉")]
        public static void ZoomIn()
        {
            float zoomFactor = GameCameras.instance.tk2dCam.ZoomFactor;
            GameCameras.instance.tk2dCam.ZoomFactor = zoomFactor + zoomFactor * 0.05f;
            Console.AddLine("Zoom level increased to: " + GameCameras.instance.tk2dCam.ZoomFactor);
        }

        [BindableMethod(name = "缩小相机视野", category = "视觉")]
        public static void ZoomOut()
        {
            float zoomFactor2 = GameCameras.instance.tk2dCam.ZoomFactor;
            GameCameras.instance.tk2dCam.ZoomFactor = zoomFactor2 - zoomFactor2 * 0.05f;
            Console.AddLine("缩放级别已减少至:: " + GameCameras.instance.tk2dCam.ZoomFactor);
        }

        [BindableMethod(name = "隐藏大黄蜂", category = "视觉")]
        public static void HideHero()
        {
            tk2dSprite component = DebugMod.RefKnight.GetComponent<tk2dSprite>();
            Color color = component.color;
            if (Math.Abs(color.a) > 0f)
            {
                color.a = 0f;
                component.color = color;
                Console.AddLine("正在渲染大黄蜂为不可见...");
            }
            else
            {
                color.a = 1f;
                component.color = color;
                Console.AddLine("正在渲染主角精灵为可见...");
            }
        }

        [BindableMethod(name = "切换相机抖动", category = "视觉")]
        public static void ToggleCameraShake()
        {
            bool newValue = !GameCameras.instance.cameraShakeFSM.enabled;
            GameCameras.instance.cameraShakeFSM.enabled = newValue;
            Console.AddLine($"{(newValue ? "正在启用" : "正在禁用")} 相机抖动...");
        }

        [BindableMethod(name = "切换光标显示", category = "视觉")]
        public static void ToggleAlwaysShowCursor()
        {
            DebugMod.settings.ShowCursorWhileUnpaused = !DebugMod.settings.ShowCursorWhileUnpaused;

            if (DebugMod.settings.ShowCursorWhileUnpaused)
            {
                SetAlwaysShowCursor();
                Console.AddLine("非暂停状态下显示光标");
            }
            else
            {
                UnsetAlwaysShowCursor();
                Console.AddLine("非暂停状态下不显示光标");
            }
        }

        internal static void SetAlwaysShowCursor()
        {
            ModHooks.CursorHook -= CursorDisplayActive;
            ModHooks.CursorHook += CursorDisplayActive;
        }

        internal static void UnsetAlwaysShowCursor()
        {
            ModHooks.CursorHook -= CursorDisplayActive;
        }

        private static void CursorDisplayActive()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

    }
}