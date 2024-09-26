﻿using System;

namespace QuestPDF.Infrastructure
{
    public readonly struct Size
    {
        internal const float Epsilon = 0.001f;
        internal const float Infinity = 14_400;

        public readonly float Width;
        public readonly float Height;
        
        internal static Size Zero { get; } = new Size(0, 0);
        internal static Size Max { get; } = new Size(Infinity, Infinity);

        public Size(float width, float height)
        {
            Width = width;
            Height = height;
        }
        
        internal static bool Equal(Size first, Size second)
        {
            if (Math.Abs(first.Width - second.Width) > Size.Epsilon)
                return false;
            
            if (Math.Abs(first.Height - second.Height) > Size.Epsilon)
                return false;

            return true;
        }
        
        public override string ToString() => $"(Width: {Width:N3}, Height: {Height:N3})";
    }
}