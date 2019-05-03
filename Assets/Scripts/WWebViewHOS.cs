// WWebViewDemo2D.cs : WWebViewDemo2D implementation file
//
// Description      : WWebViewDemo3D
// Author           : icodes (icodes.studio@gmail.com)
// Maintainer       : icodes
// Created          : 2018/04/07
// Last Update      : 2018/04/08
// Repository       : https://github.com/icodes-studio/WWebView
// Official         : http://www.icodes.studio/
//
// (C) ICODES STUDIO. All rights reserved. 
//

using UnityEngine;
using ICODES.STUDIO.WWebView;

public class WWebViewHOS : WWebViewDemo
{
    private TestMode testMode = TestMode.Nothing;

    private enum TestMode
    {
        Nothing = 0,
        NativeWindow,
        TransparentWindow,
        CustomInput,
    }

    /// <summary>
    /// set and launck web window
    /// </summary>
    public void NativeWindow()
    {
        webView.Navigate(url);
        webView.Alpha = 1f;
        webView.SetZoom(90);
        webView.Show();
        testMode = TestMode.NativeWindow;
    }


}
