/*
	IntReference

	Description: A reference to an IntVariable Scriptable Object

	Created by: Eran "Sabre Runner" Arbel.
	Last Updated: 2020-02-11
*/

namespace PushForward.ScriptableObjects.Primitives
{
    using System;

    [Serializable]
    public class IntReference
    {
        public bool useInitial = true;
        public IntVariable variable;

        public int Value => this.useInitial ? this.variable.initialValue : this.variable.runtimeValue;
    }
}
