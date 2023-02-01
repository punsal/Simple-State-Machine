using Simple.StateMachine.Runtime;
using UnityEngine;

namespace Simple.StateMachine.Tests.Runtime.TestStates
{
    public class EmptyState : IState
    {
        public void Initialize()
        {
            Debug.Log("Initialize empty state");
        }

        public void Update()
        {
            Debug.Log("Update empty state");
        }

        public void Dispose()
        {
            Debug.Log("Dispose empty state");
        }
    }
}