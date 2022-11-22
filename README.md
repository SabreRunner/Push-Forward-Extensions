# Push Forward Extensions
The Push Forward Extensions is a collection of useful components to help you get your project off the ground faster.
The biggest parts are the MonoBehaviour extensions to enhance MonoBehaviour functionality while not interfering with other scripts that replace MonoBehaviour, the collection of extension methods which allows writing more English-looking code, and the Publisher-Subscriber system that allows you to almost completely decouple all your components and extend it to your own needs.
For automatic line drop, you need to use TMP instead of regular Unity's text component.

## Installation

* Make sure [Git](https://git-scm.com/) is installed on your system and is in your PATH.
* Open your Package Manager in Unity Editor, click the + button and "Add package from git url"
* Insert the following URL: [https://github.com/SabreRunner/Push-Forward-Extensions.git](https://github.com/SabreRunner/Push-Forward-Extensions.git)
* And confirm.

Consult [install packages from git](https://docs.unity3d.com/Manual/upm-ui-giturl.html "Installing from a Git URL") for help.

## How to Use

There are several ways to use this package

* Android Manager: For taking care of Native Android access as well as toasts.
* Application Public Access: Exposes several useful application functions for editor event use.
* Timer: A user-defined timer component that can output numbers and text as well as trigger events at certain times.
* TriggerAt: A component that allows you to trigger public function at specified times.
* Unity Events: A collection of extension to the regular UnityEvent that allows you to pass different variables.
* Unity Extension Methods: Various methods to make basic unity functions easier and more elegant. Just try the methods available on game objects and components. Try to write logic in proper English. (It's not in the ExtensionMethods namespace because they need to be accessible to primitive Unity objects)
* Extension Methods\Math Extension Methods: Various methods to make math function easier and moore elegant. Just try the methods available on int, float, and others. Try to write logic in proper English. 