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

### The Event System

1. Create a new event for the action you want to broadcast.

   1.1. Right click inside the directory in the project in which you keep events. Then go to the "Event System" directory and choose the type of event you want.
   
   2.2. Name the event asset according to the specific event and/or data it's supposed to convey.
2. Create a field in the class that will raise the event and use it.
3. Assign the new even asset to that field in the inspector.
4. Add the event's specific listener to any object you want to listen to the event. For example, if you're raising an int event, you need and int listener.
5. Assign the event asset to that listener and use the Unity Events available to point to the public method with the proper parameters. For example, if you have an int event, you will want a component that exposes a public method with a single int parameter. Or, you can add several methods to the array on the listener to be called depending on the int recieved.

##### Note: The Event System is one-to-one, one-to-many, many-to-one, or many-to-many, all depending on how many components raise the event and how many listeners are available to listen to it.