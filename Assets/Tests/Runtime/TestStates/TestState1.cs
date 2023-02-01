using Simple.StateMachine.Runtime;
using Simple.StateMachine.Tests.Runtime.Mono;
using UnityEngine;

namespace Simple.StateMachine.Tests.Runtime.TestStates
{
    public class TestState1 : IState
    {
        internal StateMachineTestMonoHelper MonoHelper { get; private set; }

        public void Initialize()
        {
            if (MonoHelper != null) return;
            var temp = new GameObject($"Object - {GetType().Name}");
            MonoHelper = temp.AddComponent<StateMachineTestMonoHelper>();
        }

        public void Update()
        {
            MonoHelper.StateName = $"{GetType().Name}";
        }

        public void Dispose()
        {
            MonoHelper.Dispose();
            MonoHelper = null;
        }
    }
}