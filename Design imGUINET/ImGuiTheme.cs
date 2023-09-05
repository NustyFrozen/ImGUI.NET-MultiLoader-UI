﻿using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Color = System.Drawing.Color;
using System.Numerics;
using Microsoft.VisualBasic.FileIO;
using System.Runtime.InteropServices;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using UI;
using static System.Net.Mime.MediaTypeNames;

namespace Design_imGUINET
{
    public static class ImGuiTheme
    {
        struct colFrame {
            
            public ImGuiCol type;
           public Color start;
           public Color end;
           public float speed;
           public float progress;
        }
        struct glowingInputFrame
        {
            public uint color;
            public float borderThickness;
        }
        struct SliderInputFrame
        {
            public uint color;
            public float borderThickness;
            public float lerpProgress;
            public uint sliderColorActive;
            public Sliderstatus status;
        }
        public enum Sliderstatus
        {
            idle,
            start,
            end
        }
        public enum circleState
        {
            fadeIn,
            fadeOut,
            Spin,
            WaitEnd,
            idle
        }
        struct circleButtonFrame
        {
            public float current;
            public float waitFade;
            public float hoverFade;
            public uint color;
            public circleState status;
        }
        private static Dictionary<string, object> frameData = new Dictionary<string, object>();
        public static int toInt(this ImGuiCol col)
        {
            return (int)col;

        }
        public static void initDefaultTheme()
        {
            var io = ImGui.GetIO();
            var style = ImGui.GetStyle();
            style.Colors[ImGuiCol.WindowBg.toInt()] = Color.FromArgb(255, 19, 20, 25).toVec4();
            style.Colors[ImGuiCol.Border.toInt()] = Color.FromArgb(255, 88, 37, 227).toVec4();
            style.WindowBorderSize = 1;
            style.WindowRounding = 5;
            
        }
        public static bool button(string text)
        {
            if(ImGui.Button(text))
            {
                return true;
            }
            return false;
        }
        public static void AnimateProperties()
        {
            var style = ImGui.GetStyle();
            foreach (KeyValuePair<string,object> pair in frameData)
            {
                if(pair.Value is colFrame)
                {
                    colFrame data = (colFrame)pair.Value;
                    data.progress += data.speed;
                    if (data.progress >= 1)
                    {
                        frameData.Remove(pair.Key);
                        break;
                    }
                    style.Colors[data.type.toInt()] = data.start.lerp(data.end, data.progress).toVec4();
                    frameData[pair.Key] = data;
                }
            }
        }
        public static void lerpColorElement(ImGuiCol colID,Vector4 color,float speed)
        {
            frameData.Add(colID.ToString(), new colFrame()
            {
                start = ImGui.GetStyle().Colors[colID.toInt()].toColor(),
                end = color.toColor(),
                speed = speed,
                type = colID
            });
        }
        public static bool buttonWait(string label,ButtonConfigurator cfg)
        {
            label = "##" + label;
            bool results = false;
            object obj = cfg.bgcolor;
            var borderColorActive = cfg.ColorHover;
            var Fadestep = 0.0f;
            var status = circleState.idle;
            var hoverStep = 0.0f;
            float currentSpinValue = 0f;
            circleButtonFrame frameTickData;
            bool flag = frameData.TryGetValue(label, out obj);
            if (flag)
            {
                frameTickData = ((circleButtonFrame)obj);
                borderColorActive = frameTickData.color;
                Fadestep = frameTickData.waitFade;
                status = frameTickData.status;
                currentSpinValue = frameTickData.current;
                hoverStep = frameTickData.hoverFade;
            }
            else
            {
                frameData.Add(label, new circleButtonFrame()
                {
                    color = borderColorActive
                });
            }

            Vector2 windowpos = ImGui.GetWindowPos();
            var style = ImGui.GetStyle();
            var draw = ImGui.GetWindowDrawList();
            var cursorPos = ImGui.GetCursorPos();
            if (status != circleState.idle)
            {
                var yOffset = 0.0f;
                switch(status)
                {
                    case circleState.fadeIn:
                    Fadestep += cfg.SlideSpeed;
                    yOffset = Fadestep;
                        break;
                        case circleState.fadeOut:
                        Fadestep -= cfg.SlideSpeed;
                        yOffset = Fadestep;
                        break;
                    case circleState.Spin:
                        Fadestep += cfg.waitSpeed;
                        yOffset = 1;
                        break;
                }

                circleProgressBarAnimated(label + "_CIRCLESPIN", new Vector2(cursorPos.X + cfg.size.X / 2.0f
            , cursorPos.Y + cfg.size.Y + yOffset * cfg.circlePositionY)
                    , cfg.circleRadius, cfg.circleColor, cfg.circleThickness, cfg.circleSpeed,0.9f);
                if (Fadestep >= 1 && status == circleState.fadeIn)
                {
                    Fadestep = 0;
                    status = circleState.Spin;
                }
                if (Fadestep <= 0 && status == circleState.fadeOut)
                {
                    status = circleState.idle;
                    results = true;
                }
                if (Fadestep >= 1 && status == circleState.Spin)
                {
                    status = circleState.fadeOut;
                }
            }


            var startDrawBg = new Vector2(windowpos.X + cursorPos.X, windowpos.Y + cursorPos.Y);
            var endDrawBg = new Vector2(windowpos.X + cursorPos.X + cfg.size.X, windowpos.Y + cursorPos.Y + cfg.size.Y);
            draw.AddRectFilled(startDrawBg, endDrawBg, borderColorActive, cfg.roundCorners);
            var WindowbgColor = style.Colors[ImGuiCol.WindowBg.toInt()].toColor();
            var temp = style.Colors[ImGuiCol.FrameBg.toInt()];
            var temp2 = style.Colors[ImGuiCol.Text.toInt()];
            var temp3 = ImGui.GetFontSize();
            style.Colors[ImGuiCol.FrameBg.toInt()] = cfg.bgcolor.toColor().toVec4();
            style.FrameBorderSize = 0;
            style.Colors[ImGuiCol.Text.toInt()] = cfg.textColor.toColor().toVec4();
            
            

                draw.AddText(
                    new Vector2(startDrawBg.X + cfg.size.X / 2.0f - ImGui.CalcTextSize(cfg.text).X / 2.0f
                    , windowpos.Y + cursorPos.Y + cfg.size.Y / 2.0f - ImGui.CalcTextSize(cfg.text).Y / 2.0f)
                    , cfg.textColor.toColor().ToUint(), cfg.text);
            
            if (ImGui.IsMouseHoveringRect(startDrawBg,endDrawBg))
            {
                if(ImGui.IsMouseClicked(ImGuiMouseButton.Left) && status == circleState.idle)
                {
                    status = circleState.fadeIn;
                }
                borderColorActive = cfg.bgcolor.toColor().lerp(cfg.ColorHover.toColor(), hoverStep).ToUint();
                if (hoverStep <= 1)
                    hoverStep += 0.001f;
            }
            else
            {
                if (hoverStep >= 0)
                    hoverStep -= 0.001f;
                borderColorActive = cfg.bgcolor.toColor().lerp(cfg.ColorHover.toColor(), hoverStep).ToUint();
            }


            frameData[label] = new circleButtonFrame()
            {
                color = borderColorActive,
                waitFade = Fadestep,
                current = currentSpinValue,
                status = status,
                hoverFade = hoverStep
            };
            style.Colors[ImGuiCol.FrameBg.toInt()] = temp;
            style.Colors[ImGuiCol.Text.toInt()] = temp2;
            return results;
        }
        public static void gradientBorder(Vector4 topColor,Vector4 bottomColor)
        {
            Vector2 windowpos = ImGui.GetWindowPos();
            var style = ImGui.GetStyle();
            var Windowsize = ImGui.GetWindowSize();
            var draw = ImGui.GetForegroundDrawList();
            var stepper = Windowsize.Y * 0.01f;
            style.WindowBorderSize = 0;
            for(float i = 0.01f;i<1;i +=0.001f)
            {
                draw.AddLine(
                    new Vector2(windowpos.X,
                    windowpos.Y+i* Windowsize.Y)
                    ,new Vector2(windowpos.X + 1,windowpos.Y + (i-0.01f)* Windowsize.Y), topColor.toColor().lerp(bottomColor.toColor(),i).ToUint(),2);
                draw.AddLine(
                    new Vector2(windowpos.X + Windowsize.X - 1,
                    windowpos.Y + i * Windowsize.Y)
                    , new Vector2(windowpos.X + Windowsize.X, windowpos.Y + (i - 0.01f) * Windowsize.Y), topColor.toColor().lerp(bottomColor.toColor(), i).ToUint(), 3);
            }
            draw.AddRect(new Vector2(windowpos.X, windowpos.Y - 1), new Vector2(windowpos.X + Windowsize.X + 1, windowpos.Y)
                , topColor.toColor().ToUint(), style.WindowRounding, ImDrawFlags.RoundCornersTopLeft | ImDrawFlags.RoundCornersTopRight,1.5f);
        }
        public static void gradientRect(Vector2 p_min,Vector2 p_max,Vector4 topColor, Vector4 bottomColor,float CornerRadius)
        {
            Vector2 windowpos = new Vector2(0,0);
            var style = ImGui.GetStyle();
            var Windowsize = new Vector2(p_min.X - p_max.X,p_min.Y - p_max.Y);
            var draw = ImGui.GetForegroundDrawList();
            var stepper = Windowsize.Y * 0.01f;
            style.WindowBorderSize = 0;
            for (float i = 0.01f; i < 1; i += 0.01f)
            {
                draw.AddLine(
                    new Vector2(windowpos.X + p_min.X,
                    p_min.Y + windowpos.Y + i * Windowsize.Y)
                    , new Vector2(windowpos.X + 1 + p_min.X, p_min.Y + windowpos.Y + (i - 0.01f) * Windowsize.Y), topColor.toColor().lerp(bottomColor.toColor(), i).ToUint(), 2);
                draw.AddLine(
                    new Vector2(windowpos.X + Windowsize.X - 1 + p_min.X,
                    windowpos.Y + i * Windowsize.Y + p_min.Y)
                    , new Vector2(windowpos.X + Windowsize.X + p_min.X,  windowpos.Y + p_min.Y + (i - 0.01f) * Windowsize.Y), topColor.toColor().lerp(bottomColor.toColor(), i).ToUint(), 3);
            }
            draw.AddRect(new Vector2(windowpos.X + p_min.X, windowpos.Y + p_min.Y - 1)
                , new Vector2(windowpos.X + Windowsize.X + 1 + p_min.X, p_min.Y+ windowpos.Y)
                , topColor.toColor().ToUint(), CornerRadius, ImDrawFlags.RoundCornersTopLeft | ImDrawFlags.RoundCornersTopRight, 1.5f);
            draw.AddRect(new Vector2(windowpos.X + p_min.X, windowpos.Y + p_min.Y - 1 + Windowsize.Y)
                , new Vector2(windowpos.X + Windowsize.X + 1 + p_min.X, p_min.Y + windowpos.Y + Windowsize.Y)
                , bottomColor.toColor().ToUint(), CornerRadius, ImDrawFlags.RoundCornersTopLeft | ImDrawFlags.RoundCornersTopRight, 1.5f);
        }
        public struct glowingInputConfigurator {
            public uint bgcolor;
            public uint borderColorActive;
            public uint borderColor;
            public uint textColor;
            public float roundCorners;
            public float borderThickness;
            public Vector2 size;
            public string prefix;
            public float fontScale;
            public char passwordChar;
        };
        public struct SliderInputConfigurator
        {
            public uint bgcolor;
            public uint borderColorActive;
            public uint borderColor;
            public uint sliderColor;
            public float roundCorners;
            public float borderThickness;
            public Vector2 size;
            public uint sliderColorActive;
            public float YoffsetLabel;
        };
        public struct ButtonConfigurator
        {
            public uint bgcolor;
            public uint ColorHover;
            public uint textColor;
            public float roundCorners;
            public Vector2 size;
            public string text;
            public float circleRadius;
            public float circleSpeed;
            public uint circleColor;
            public uint circleThickness;
            public float SlideSpeed;
            public float waitSpeed;
            public float circlePositionY;
        };
        public static void GradientGlowingInput(string label, ref string text, glowingInputConfigurator cfg, Vector4 bottomColor, uint maxlength = 32)
        {
            label = "##" + label;
            object obj = cfg.borderColor;
            var borderColorActive = cfg.borderColorActive;
            var BorderThicknessActive = cfg.borderThickness;
            bool flag = frameData.TryGetValue(label, out obj);
            if (flag)
            {
                borderColorActive = ((glowingInputFrame)obj).color;
                BorderThicknessActive = ((glowingInputFrame)obj).borderThickness;
            }
            else
            {
                frameData.Add(label, new glowingInputFrame()
                {
                    borderThickness = BorderThicknessActive,
                    color = borderColorActive
                });
            }
            Vector2 windowpos = ImGui.GetWindowPos();
            var style = ImGui.GetStyle();
            var draw = ImGui.GetWindowDrawList();
            var cursorPos = ImGui.GetCursorPos();
            var startDrawBg = new Vector2(windowpos.X + cursorPos.X, windowpos.Y + cursorPos.Y);
            var endDrawBg = new Vector2(windowpos.X + cursorPos.X + cfg.size.X, windowpos.Y + cursorPos.Y + cfg.size.Y);
            draw.AddRectFilled(startDrawBg, endDrawBg, cfg.bgcolor, cfg.roundCorners);
            var WindowbgColor = style.Colors[ImGuiCol.WindowBg.toInt()].toColor();
            var temp = style.Colors[ImGuiCol.FrameBg.toInt()];
            var temp2 = style.Colors[ImGuiCol.Text.toInt()];
            var temp3 = ImGui.GetFontSize();
            style.Colors[ImGuiCol.FrameBg.toInt()] = cfg.bgcolor.toColor().toVec4();
            style.FrameBorderSize = 0;
            style.Colors[ImGuiCol.Text.toInt()] = cfg.textColor.toColor().toVec4();
            var currentCursor = ImGui.GetCursorPos();

            ImGui.SetCursorPos(new Vector2(ImGui.GetCursorPosX() + style.FramePadding.X,
                ImGui.GetCursorPosY() + cfg.size.Y / 2.0f - ImGui.CalcTextSize(cfg.prefix).Y / 2.0f
                ));
            if (ImGui.InputText(label, ref text, maxlength)) { }
            if (text == string.Empty && !ImGui.IsItemActive())
            {
                draw.AddText(
                    new Vector2(startDrawBg.X + style.FramePadding.X, windowpos.Y + cursorPos.Y + cfg.size.Y / 2.0f - ImGui.CalcTextSize(cfg.prefix).Y / 2.0f)
                    , cfg.textColor.toColor().Brightness(0.4f).ToUint(), cfg.prefix);
            }
            if (ImGui.IsItemActive())
            {

                borderColorActive = borderColorActive.toColor().lerp(cfg.borderColorActive.toColor(), 0.01f).ToUint();
                if (BorderThicknessActive <= cfg.borderThickness + 3)
                    BorderThicknessActive += 0.01f;
            }
            else
            {
                if (BorderThicknessActive > cfg.borderThickness)
                    BorderThicknessActive -= 0.01f;
                borderColorActive = borderColorActive.toColor().lerp(cfg.borderColor.toColor(), 0.01f).ToUint();

            }

            frameData[label] = new glowingInputFrame()
            {
                borderThickness = BorderThicknessActive,
                color = borderColorActive
            };

            
                gradientRect(new Vector2( startDrawBg.X + cfg.size.X,startDrawBg.Y + +cfg.size.Y),
                    new Vector2( endDrawBg.X + cfg.size.X, endDrawBg.Y + cfg.size.Y), borderColorActive.toColor().toVec4(), bottomColor, cfg.roundCorners);
            
            style.Colors[ImGuiCol.FrameBg.toInt()] = temp;
            style.Colors[ImGuiCol.Text.toInt()] = temp2;
        }
        public static bool drawTextGradient(string label,Vector2 pos,string text, Color clrDefault, Color clrHover)
        {

            object obj = clrDefault;
            var ExitColorActive = clrDefault;
            bool flag = frameData.TryGetValue(label, out obj);
            bool results = false;
            if (flag)
            {
                ExitColorActive = (Color)obj;
            }
            else
            {
                frameData.Add(label, ExitColorActive);
            }
            Vector2 windowpos = ImGui.GetWindowPos();
            var style = ImGui.GetStyle();
            var draw = ImGui.GetWindowDrawList();
            var size = ImGui.CalcTextSize(text);
            var start = new Vector2(windowpos.X + pos.X, windowpos.Y + pos.Y);
            var end = new Vector2(start.X + size.X, start.Y + size.Y);
            draw.AddText(start, ExitColorActive.ToUint(), text);
            if (ImGui.IsMouseHoveringRect(start,end))
            {
                ExitColorActive = ExitColorActive.lerp(clrHover, 0.025);
                if (ImGui.IsMouseClicked(ImGuiMouseButton.Left))
                {
                    results = true;
                }
            }
            else
            {
                ExitColorActive = ExitColorActive.lerp(clrDefault, 0.025);
            }
            frameData[label] = ExitColorActive;
            return results;
        }
        public static void drawExitButton(float size,Color clrDefault,Color clrHover)
        {

            object obj = clrDefault;
            var ExitColorActive = clrDefault;
            bool flag = frameData.TryGetValue("DrawExitButtonLabel", out obj);
            if (flag)
            {
                ExitColorActive = (Color)obj;
            }
            else
            {
                frameData.Add("DrawExitButtonLabel", ExitColorActive);
            }
            Vector2 windowpos = ImGui.GetWindowPos();
            var style = ImGui.GetStyle();
            var draw = ImGui.GetWindowDrawList();
            var cursorPos = ImGui.GetCursorPos();
            var start = new Vector2(windowpos.X + ImGui.GetContentRegionAvail().X - size - 5, windowpos.Y - size + 30);
            var end = new Vector2(start.X + size, start.Y + size);
            if (ImGui.IsMouseHoveringRect(start, end))
            {
                ExitColorActive = ExitColorActive.lerp(clrHover, 0.1);
                if (ImGui.IsMouseClicked(ImGuiMouseButton.Left))
                {
                    Environment.Exit(0);
                }
            }
            else
            {
                ExitColorActive = ExitColorActive.lerp(clrDefault, 0.1);
            }
            frameData["DrawExitButtonLabel"] = ExitColorActive;
            draw.AddLine(start, end,ExitColorActive.ToUint(),2);
            draw.AddLine(new Vector2(end.X,start.Y), new Vector2(start.X,end.Y), ExitColorActive.ToUint(), 2);
        }
        public static void slider(string label,ref float value,SliderInputConfigurator cfg)
        {
            label = "##" + label;
            object obj = cfg.borderColor;
            var borderColorActive = cfg.borderColorActive;
            var BorderThicknessActive = cfg.borderThickness;
            var sliderColorActive = cfg.sliderColor;
            float lerpProgress = 0;
            var status = Sliderstatus.idle;
            bool flag = frameData.TryGetValue(label, out obj);
            if (flag)
            {
                var frameData = ((SliderInputFrame)obj);
                borderColorActive = frameData.color;
                BorderThicknessActive = frameData.borderThickness;
                sliderColorActive = frameData.sliderColorActive;
                lerpProgress = frameData.lerpProgress;
                status = frameData.status;
            }
            else
            {
                frameData.Add(label, new SliderInputFrame()
                {
                    borderThickness = BorderThicknessActive,
                    color = borderColorActive,
                    sliderColorActive = sliderColorActive,
                    lerpProgress = lerpProgress,
                    status = status
                });
            }
            Vector2 windowpos = ImGui.GetWindowPos();
            var style = ImGui.GetStyle();
            var draw = ImGui.GetWindowDrawList();
            var cursorPos = ImGui.GetCursorPos();
            System.Drawing.Point mousePos = new System.Drawing.Point(0,0);
            Imports.GetCursorPos(ref mousePos);
            var startDrawBg = new Vector2(windowpos.X + cursorPos.X, windowpos.Y + cursorPos.Y);
            var endDrawBg = new Vector2(windowpos.X + cursorPos.X + cfg.size.X, windowpos.Y + cursorPos.Y + cfg.size.Y);
            draw.AddRectFilled(startDrawBg, endDrawBg, cfg.bgcolor, cfg.roundCorners);
            draw.AddRectFilled(new Vector2(startDrawBg.X, startDrawBg.Y), new Vector2(startDrawBg.X + value*cfg.size.X , endDrawBg.Y)
                , sliderColorActive, cfg.roundCorners);
            var WindowbgColor = style.Colors[ImGuiCol.WindowBg.toInt()].toColor();
            style.FrameBorderSize = 0;
            if (!ImGui.IsMouseDown(ImGuiMouseButton.Left) && status == Sliderstatus.start)
            {
                status = Sliderstatus.end;
            }
            ImGui.InvisibleButton(label, cfg.size); //stop dragging
            if (ImGui.IsMouseHoveringRect(startDrawBg,endDrawBg) || status == Sliderstatus.start)
            {
                if(ImGui.IsMouseDown(ImGuiMouseButton.Left) || status == Sliderstatus.start)
                {
                    status = Sliderstatus.start;
                    float drawPos = mousePos.X - startDrawBg.X;
                    if (drawPos < 0)
                        drawPos = 0;
                    if (drawPos > cfg.size.X)
                        drawPos = cfg.size.X;
                    value = drawPos / cfg.size.X;
                    draw.AddRectFilled(startDrawBg, endDrawBg, cfg.bgcolor, cfg.roundCorners);
                    draw.AddRectFilled(new Vector2(startDrawBg.X, startDrawBg.Y), new Vector2(startDrawBg.X + drawPos, endDrawBg.Y), sliderColorActive, cfg.roundCorners);
                    lerpProgress += 0.0025f;
                } else
                {
                    lerpProgress -= 0.0025f;
                }
                borderColorActive = borderColorActive.toColor().lerp(cfg.borderColorActive.toColor(), 0.01f).ToUint();
                if (BorderThicknessActive <= cfg.borderThickness + 3)
                    BorderThicknessActive += 0.01f;
            }
            else
            {
                if (BorderThicknessActive > cfg.borderThickness)
                    BorderThicknessActive -= 0.01f;
                lerpProgress -= 0.0025f;
                borderColorActive = borderColorActive.toColor().lerp(cfg.borderColor.toColor(), 0.01f).ToUint();

            }
            if (lerpProgress < 0)
                lerpProgress = 0;
            else if (lerpProgress > 1)
                lerpProgress = 1;
            if (lerpProgress != 0)
            {
                float drawPos = mousePos.X - startDrawBg.X;
                if (drawPos < 0)
                    drawPos = 0;
                if (drawPos > cfg.size.X)
                    drawPos = cfg.size.X;
                var addZero = string.Empty;
                if ((value * 100).ToString().Count() < 4)
                    addZero += ".0";
                var Text = new string((value*100).ToString().Take(4).ToArray())+ addZero + "%";
                var textSize = ImGui.CalcTextSize(Text);
                var startRect = new Vector2(startDrawBg.X + drawPos - textSize.X / 2, startDrawBg.Y - textSize.Y + cfg.YoffsetLabel * lerpProgress);
                var endRect = new Vector2(startDrawBg.X + drawPos + textSize.X / 2, startDrawBg.Y + cfg.YoffsetLabel * lerpProgress);
                draw.AddRectFilled(startRect
                    , endRect, sliderColorActive, 4);
                draw.AddTriangleFilled(new Vector2(startRect.X + textSize.X / 2 - 8, endRect.Y),
                    new Vector2(startRect.X + textSize.X / 2 + 8, endRect.Y), 
                    new Vector2(startDrawBg.X + drawPos, startDrawBg.Y + cfg.YoffsetLabel * lerpProgress * 0.5f),sliderColorActive);
                draw.AddText(new Vector2(startDrawBg.X + drawPos - textSize.X / 2, startDrawBg.Y - textSize.Y + cfg.YoffsetLabel * lerpProgress), Color.White.ToUint(), Text);
            }
                sliderColorActive = cfg.sliderColor.toColor().lerp(cfg.sliderColorActive.toColor(),lerpProgress).ToUint();
            frameData[label] = new SliderInputFrame()
            {
                borderThickness = BorderThicknessActive,
                color = borderColorActive,
                sliderColorActive = sliderColorActive,
                status = status,
                lerpProgress = lerpProgress
            };

            for (float i = 0; i < BorderThicknessActive; i += 1f)
            {

                Color lerpedToBackground = borderColorActive.toColor().lerp(WindowbgColor, i / BorderThicknessActive);
                draw.AddRect(new Vector2(startDrawBg.X - i, startDrawBg.Y - i),
                    new Vector2(endDrawBg.X + i, endDrawBg.Y + i), lerpedToBackground.ToUint(), cfg.roundCorners);
            }
        }
        public static void glowingInput(string label,ref string text,glowingInputConfigurator cfg, uint maxlength = 32)
        {
            label = "##" + label;
            object obj = cfg.borderColor;
            var borderColorActive = cfg.borderColorActive;
            var BorderThicknessActive = cfg.borderThickness;
            bool flag = frameData.TryGetValue(label, out obj);
            if (flag)
            {
                borderColorActive = ((glowingInputFrame)obj).color;
                BorderThicknessActive = ((glowingInputFrame)obj).borderThickness;
            }
            else
            {
                frameData.Add(label, new glowingInputFrame()
                {
                    borderThickness = BorderThicknessActive,
                    color = borderColorActive
                });
            }
            Vector2 windowpos = ImGui.GetWindowPos();
            var style = ImGui.GetStyle();
            var draw = ImGui.GetWindowDrawList();
            var cursorPos = ImGui.GetCursorPos();
            var startDrawBg = new Vector2(windowpos.X + cursorPos.X, windowpos.Y + cursorPos.Y );
            var endDrawBg = new Vector2(windowpos.X + cursorPos.X + cfg.size.X, windowpos.Y + cursorPos.Y+ cfg.size.Y);
            draw.AddRectFilled(startDrawBg, endDrawBg, cfg.bgcolor, cfg.roundCorners);
            var WindowbgColor = style.Colors[ImGuiCol.WindowBg.toInt()].toColor();
            var temp = style.Colors[ImGuiCol.FrameBg.toInt()];
            var temp2 = style.Colors[ImGuiCol.Text.toInt()];
            var temp3 = ImGui.GetFontSize();
            style.Colors[ImGuiCol.FrameBg.toInt()] = cfg.bgcolor.toColor().toVec4();
            style.FrameBorderSize = 0;
            style.Colors[ImGuiCol.Text.toInt()] = cfg.textColor.toColor().toVec4();
            var currentCursor = ImGui.GetCursorPos();
            ImGui.SetCursorPos(new Vector2(ImGui.GetCursorPosX() + style.FramePadding.X,
                ImGui.GetCursorPosY() + cfg.size.Y / 2.0f - ImGui.CalcTextSize(cfg.prefix).Y / 2.0f
                ));
            if (ImGui.InputText(label, ref text, maxlength)) { }
            if (cfg.passwordChar != '\0')
            {
                var hiddenText = string.Empty;
                for (int i = 0; i < text.Length; i++)
                {
                    hiddenText += cfg.passwordChar;
                }
                draw.AddRectFilled(startDrawBg, endDrawBg, cfg.bgcolor, cfg.roundCorners);
                draw.AddText(
                    new Vector2(startDrawBg.X + style.FramePadding.X, windowpos.Y + cursorPos.Y + cfg.size.Y / 2.0f - ImGui.CalcTextSize(hiddenText).Y / 2.0f)
                    , cfg.textColor, hiddenText);
            }
            if (text == string.Empty && !ImGui.IsItemActive())
            {
                draw.AddText(
                    new Vector2(startDrawBg.X + style.FramePadding.X, windowpos.Y + cursorPos.Y + cfg.size.Y / 2.0f - ImGui.CalcTextSize(cfg.prefix).Y / 2.0f)
                    , cfg.textColor.toColor().Brightness(0.4f).ToUint(), cfg.prefix);
            }
            if (ImGui.IsItemActive())
            {

                borderColorActive = borderColorActive.toColor().lerp(cfg.borderColorActive.toColor(), 0.01f).ToUint();
                if (BorderThicknessActive <= cfg.borderThickness + 3)
                    BorderThicknessActive += 0.01f;
            }
            else
            {
                if (BorderThicknessActive > cfg.borderThickness)
                    BorderThicknessActive -= 0.01f;
                borderColorActive = borderColorActive.toColor().lerp(cfg.borderColor.toColor(), 0.01f).ToUint();
               
            }
            
            frameData[label] = new glowingInputFrame() {
                borderThickness = BorderThicknessActive,
                color = borderColorActive
                };

                for (float i = 0; i < BorderThicknessActive; i += 1f)
                {
                    
                    Color lerpedToBackground = borderColorActive.toColor().lerp(WindowbgColor, i / BorderThicknessActive);
                    draw.AddRect(new Vector2(startDrawBg.X - i, startDrawBg.Y - i),
                        new Vector2(endDrawBg.X + i, endDrawBg.Y + i), lerpedToBackground.ToUint(), cfg.roundCorners);
                }
            style.Colors[ImGuiCol.FrameBg.toInt()] = temp;
            style.Colors[ImGuiCol.Text.toInt()] = temp2;
        }
        public static void circleProgressBarAnimated(string label,Vector2 pos, float radius,uint Color,float tickness,float speed,float progress)
        {
            Vector2 windowpos = ImGui.GetWindowPos();
            pos = new Vector2(windowpos.X + pos.X, windowpos.Y + pos.Y);
            object obj = 0.0f;
            bool flag = frameData.TryGetValue(label, out obj);
            var draw = ImGui.GetWindowDrawList();
            float angle;
            if (!flag)
            {
                angle = 0;
                frameData.Add(label, angle);
            }
            else
                angle = (float)obj;
            var style = ImGui.GetStyle();
            Color bg = style.Colors[ImGuiCol.WindowBg.toInt()].toColor();

            angle += speed;
            float lim = 2.0f * ((float)Math.PI) * (progress);
            for (float i = 0; i < lim; i += 0.01f) {
                Vector2 unDiscover = new Vector2(radius * (float)Math.Sin(angle + i), radius * (float)Math.Cos(angle + i));
               Color steppedColor = bg.lerp(Color.toColor(), i / lim);
                draw.AddLine(new Vector2(pos.X + unDiscover.X - 1, pos.Y + unDiscover.Y - 1)
                                , new Vector2(pos.X + unDiscover.X, pos.Y + unDiscover.Y ) , steppedColor.ToUint(), tickness);
            }
            //Console.WriteLine(angle);
            if (angle >= 2*Math.PI) angle = 0;
            frameData[label] = angle;
        }
    }
}
