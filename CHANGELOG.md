# CHANGELOG

## 2022.11.27
* Fixed Android pragma if.

## 2022.11.23
* Added setup method to TriggerAt so it can be created programatically.

## 0.6.6
* Documentation and clean up on aisle ApplicationPublicAccess.
* Documentation on aisle TriggerAt

## 0.6.5
* Documentation and clean up on aisle MathExtensionMethods.
* Documentation and clean up on aisle Timer.
* Added double output event to UnityEvents for use in Timer.

## 0.6.4
* Added documentation to Unity Extension Methods.

## 0.6.3
* Removed GameEventEditor from runtime.

## 0.6.2
* Fixed GameEventBool not public.
* Added reversed action calls. Older versions deprecated.

## 0.6.1
* Removed Temp log calls because they are not editor only.
* Hid Editor scripts behind #if UNITY_EDITOR to avoid build issues.

## 0.5.9
* Added Anchor Finder.
* Moved everything to root so namespaces line up.

## 0.5.8
* Added Map as an alias Select.
* Added ChainClear for chaining more methods.
* Added some new property drawers.

## 0.5.7
* Added backwards iteration on enumerables where possible.
* Added changing of values and invoking events in variable references.

## 0.5.6
* Changed GetMainCamera to Component Extension Method.

## 0.5.5
* Made Temp Log Editor Only so building with temp logs will fail.

## 0.5.4
* Rebuilt DestroyAllChildren to extract, detach, and then destroy safely.

## 0.5.3
* Changed TransformLerper to LerpSystem.

## 0.5.2
* Added basic logging functions to ScriptableObject.

## 0.5.1
* Fixed ToastCache not recognising AndroidManager. 

## 0.5.0
* Lots of changes and additions. Too many to count.
* More documentation.

## 0.4.11
* Fixed ToastCache example access.

## 0.4.10
* Fixed namespace errors.

## 0.4.9
* Fixed inaccessible GameEventBool.
