using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Color = System.Drawing.Color;
using System.Runtime.ConstrainedExecution;
using Vulkan;
using UI;

namespace Design_imGUINET
{
    public static class ColorExtention
    {
        public static Vector4 toVec4(this System.Drawing.Color clr)
        {
            return new Vector4(clr.R / 255.0f, clr.G / 255.0f, clr.B / 255.0f, clr.A / 255.0f);
        }
        public static Color toColor(this Vector4 vec)
        {
            return Color.FromArgb((int)(vec.W * 255.0f * Program.globalOpacity), (int)(vec.X * 255.0f), (int)(vec.Y * 255.0), (int)(vec.Z * 255.0));
        }
        public static Color toColor(this uint col)
        {
            var a = (byte)(col >> 24);
            var r = (byte)(col >> 16);
            var g = (byte)(col >> 8);
            var b = (byte)(col >> 0);
            a = (byte)((float)a * Program.globalOpacity);
            return Color.FromArgb(a,b,g,r);
        }
        public static int lerp(this int x, int final, double progress)
        {
            return Convert.ToInt16((1 - progress) * x + final * progress);
        }
        public static float lerp(this float x, float final, double progress)
        {
            return (float)((1 - progress) * x + final * progress);
        }
        public static Color Brightness(this Color A, float t) //linear interpolation
        {

           
            return Color.FromArgb(Convert.ToInt32(A.R * t *Program.globalOpacity), Convert.ToInt32(A.G * t), Convert.ToInt32(A.B * t));
        }
        public static Color lerp(this Color A, Color B,double t) //linear interpolation
        {

            double R = (1 - t) * A.R + B.R * t;
            double G = (1 - t) * A.G + B.G * t;
            double BB = (1 - t) * A.B + B.B * t;
            return Color.FromArgb((int)(255.0f*Program.globalOpacity), Convert.ToInt32(R), Convert.ToInt32(G), Convert.ToInt32(BB));
        }
        public static Color Rainbow(this System.Drawing.Color clr, float progress)
        {
            float div = (System.Math.Abs(progress % 1) * 6);
            int ascending = (int)((div % 1) * 255);
            int descending = 255 - ascending;

            switch ((int)div)
            {
                case 0:
                    return Color.FromArgb(255, 255, ascending, 0);
                case 1:
                    return Color.FromArgb(255, descending, 255, 0);
                case 2:
                    return Color.FromArgb(255, 0, 255, ascending);
                case 3:
                    return Color.FromArgb(255, 0, descending, 255);
                case 4:
                    return Color.FromArgb(255, ascending, 0, 255);
                default: // case 5:
                    return Color.FromArgb(255, 255, 0, descending);
            }
        }

        public static uint ToUint(this System.Drawing.Color c)
        {

            return (uint)((((int)(c.A *Program.globalOpacity) << 24) | (c.B << 16) | (c.G << 8) | c.R) & 0xffffffffL);
        }
    }
}
