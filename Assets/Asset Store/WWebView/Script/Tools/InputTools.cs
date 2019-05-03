// InputTools.cs : InputTools implementation file
//
// Description      : InputTools
// Author           : icodes (icodes.studio@gmail.com)
// Maintainer       : icodes
// Created          : 2018/04/08
// Last Update      : 2018/04/08
// Repository       : https://github.com/icodes-studio/WWebView
// Official         : http://www.icodes.studio/
//
// (C) ICODES STUDIO. All rights reserved. 
//

using UnityEngine;

namespace ICODES.STUDIO.WWebView
{
    public static class InputTools
    {
        private enum Event
        {
            MouseMove = 0,
            MouseDown,
            MouseUp,
            MouseWheel
        }

        public static bool GetMouseState(ref int state, ref int key)
        {
            if (DidMouseMove())
            {
                state = (int)Event.MouseMove;
                return true;
            }

            for (key = 0; key < 3; ++key)
            {
                if (DidMouseClick(ref state, ref key))
                    return true;
            }

            if (DidMouseWheel(ref key))
            {
                state = (int)Event.MouseWheel;
                return true;
            }

            return false;
        }

        public static bool DidMouseClick(ref int state, ref int key)
        {
            if (Input.GetMouseButtonDown(key))
            {
                state = (int)Event.MouseDown;
                return true;
            }

            if (Input.GetMouseButtonUp(key))
            {
                state = (int)Event.MouseUp;
                return true;
            }

            return false;
        }

        public static bool DidMouseMove()
        {
            return (Input.GetAxis("Mouse X") != 0) || (Input.GetAxis("Mouse Y") != 0);
        }

        public static bool DidMouseWheel(ref int key)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                key = +1;
                return true;
            }

            if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                key = -1;
                return true;
            }

            return false;
        }
    }
}
