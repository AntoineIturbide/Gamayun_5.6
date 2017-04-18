// Copyright(c) 2017 Untitled Games   | Developed by: Chris Bellini                    | http://untitledgam.es/contact
// http://transformpro.untitledgam.es | http://transformpro.untitledgam.es/quick-start | http://transformpro.untitledgam.es/api

namespace UntitledGames.Transforms
{
    using System;
    using UnityEditor;
    using UnityEngine;

    public class TransformProStylesButtons
    {
        private static readonly int fontSize = 8;
        private static readonly int height = 16;

        private Button icon;
        private Button iconLarge;
        private Button standard;

        public TransformProStylesButtons()
        {
            this.standard = new Button(delegate(GUIStyle style)
            {
                style.fontSize = TransformProStylesButtons.fontSize;
                style.fontStyle = EditorGUIUtility.isProSkin ? FontStyle.Bold : FontStyle.Normal;
                style.fixedHeight = TransformProStylesButtons.height;
                return style;
            });

            this.icon = new Button(delegate(GUIStyle style)
            {
                style.fixedHeight = 16;
                style.padding = new RectOffset(0, 0, 0, 0);
                return style;
            }, this.Standard);

            this.iconLarge = new Button(delegate(GUIStyle style)
            {
                style.fixedHeight = 20;
                style.padding = new RectOffset(1, 1, 1, 1);
                return style;
            }, this.Standard);
        }

        public Button Icon { get { return this.icon; } }
        public Button IconLarge { get { return this.iconLarge; } }
        public Button Standard { get { return this.standard; } }

        public class Button
        {
            private readonly GUIStyle left;
            private readonly GUIStyle middle;
            private readonly GUIStyle right;
            private readonly GUIStyle single;

            public Button(Func<GUIStyle, GUIStyle> function)
                : this(function, EditorStyles.miniButton, EditorStyles.miniButtonLeft, EditorStyles.miniButtonMid, EditorStyles.miniButtonRight)
            {
            }

            public Button(Func<GUIStyle, GUIStyle> function, Button button)
                : this(function, button.Single, button.Left, button.Middle, button.Right)
            {
            }

            public Button(Func<GUIStyle, GUIStyle> function, GUIStyle single, GUIStyle left, GUIStyle middle, GUIStyle right)
            {
                this.single = function(new GUIStyle(single));
                this.left = function(new GUIStyle(left));
                this.middle = function(new GUIStyle(middle));
                this.right = function(new GUIStyle(right));
            }

            public GUIStyle Left { get { return this.left; } }

            public GUIStyle Middle { get { return this.middle; } }

            public GUIStyle Right { get { return this.right; } }

            public GUIStyle Single { get { return this.single; } }
        }
    }
}