using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace BehaviorDesigner.Runtime.Tasks.Basic.SharedVariables
{
    [TaskCategory("Basic/SharedVariable")]
    [TaskDescription("Sets the SharedObjectList variable to the specified object. Returns Success.")]
    public class SetSharedObjectList : Action
    {
        [Tooltip("The value to set the SharedObjectList to.")]
        public SharedObjectList targetValue;
        [Tooltip("The SharedObjectList to set")]
        public SharedObjectList targetVariable;

        public override TaskStatus OnUpdate()
        {
            targetVariable.Value = targetValue.Value;

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            if (targetValue != null) {
                targetValue.Value = null;
            }
            if (targetVariable != null) {
                targetVariable.Value = null;
            }
        }
    }
}