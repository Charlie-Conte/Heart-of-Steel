# RELEASE NOTES - CHANGE LOG
> All notable changes to this project will be documented in this file.


## Version 1.3.3 : Apr 8, 2018

### New features 

- A new demo "Demo2D" added.


### Bug fixes

- WSA: Fixed a bug "SetHeaderField" not work.


--------------------------------------------------------------------------------------------------------------------------------------


## Version 1.3.2 : Dec 22, 2017

### Bug fixes

- Following up Unity 2017.3.0

- Fixed flickering when the 3D demo started.


--------------------------------------------------------------------------------------------------------------------------------------


## Version 1.3.1 : Dec 19, 2017

### New features 

- A new demo has been added to describe how to embed the webview in a 3D object.

- Win32/WSA: Added SetHeaderField


--------------------------------------------------------------------------------------------------------------------------------------


## Version 1.3.0 : Dec 4, 2017

### New features 

- The new component "WWebView"

- WSA: Downgrades plugin's SDK target version to 14393 from 15063.

- WSA: Following up Unity 2017.2.0

- Win32: Added "SetSilent" interface

- WSA: Added the HoloLensVR demo.


### Bug fixes

- Win32: Fixed an issue where the "EnableContextMenu" function called before "Load" did not work properly.

- WSA: Fixed a bug "SetUserAgent" does not work on "master" build.


--------------------------------------------------------------------------------------------------------------------------------------


## Version 1.2.2 : Nov 3, 2017

### New features

- Supports UniWebView3 integration.

- Absolute position and size are supported.

- WSA: CleanCache

- WSA: Added ability to delete all cookies

- WSA: SetCookie


### Bug fixes

- Win32: Fixed a bug that fixed size does not work.

- Editor: Fixed a problem where WebView was displayed in the wrong place.

- Editor: Fixed invalid focus when the webview hidden.

- WSA: fixed an app block on demo scene while navigating.

- Win32: fixed GetAlpha() function returns an incorrect value.

- Win32: Fixed a problem where TAB & DELETE key did not work.


--------------------------------------------------------------------------------------------------------------------------------------


## Version 1.2.1 : Oct 25, 2017

### New features

- Win32: You can set IE version to be used as webview. (11, 10, 9, 8 and 7 are supported)

- Win32+WSA: CanGoBack function has been added.

- Win32+WSA: CanGoForward function has been added.


### Bug fixes

- Editor: Fixed following error.

- Editor: Fixed an issue

- Win32: GetCookie


--------------------------------------------------------------------------------------------------------------------------------------


## Version 1.2.0 : Oct 14, 2017

### New features

- Editor(Windows x86, x64) supported

- Win32+WSA: GetActualWidth(Height)

- Win32+WSA: Overloaded the Init() and ChangeInsets() function to support fixed width and height.

- Win32+WSA: Managing scrollbar


### Minor

- Added an example

- Added an example


--------------------------------------------------------------------------------------------------------------------------------------


## Version 1.1.2 : Oct 3, 2017

### New features

- Win32: Capture & Render a content as a texture.

- Win32: Transparency with alpha.

- WSA: Capture & Render a content as a texture.

- WSA: Transparency with alpha.


### Bug fixes

- Win32: Fixed window resizing bug.

- Win32: Fixed memory leak on CustomOleControlSite.cpp


### Minor

- Win32: Change plugin folder location to x86 and x64

- Win32: Event callbacks are created and stored as class members. because it is likely to be released by GC.


--------------------------------------------------------------------------------------------------------------------------------------


## Version 1.1.1 : Sept 9, 2017

### New features

- Win32: Managing Cookies.


### Bug fixes

- Win32: Statically binds C Runtime Library. (/MT)


--------------------------------------------------------------------------------------------------------------------------------------


## Version 1.1.0 : Aug 31, 2017

### New features

- Win32: Added Zoom in & out function.

- Win32: Added UserAgent changing function.

- Win32/WSA: Clearing cookies.


### Bug fixes

- WSA: Fixed navigation failure from local file.

- WSA/Win32: Fixed a bug where the success & failure parameter was incorrect


--------------------------------------------------------------------------------------------------------------------------------------


## Version 1.0.0 : July 12, 2017

- First release

- Requires Unity 5.6.0 or higher. The native plugin was built with Visual Studio 2017. It has been tested in Unity 5.6.0f3 and 2017.1.0f3. Windows standalone(x86, x64) are supported. Windows 10 universal app(x86, x64, ARM) are supported. 
