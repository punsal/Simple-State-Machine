using UnityEngine;

namespace Simple.StateMachine.Runtime
{
    /// <summary>
    /// Runs given StateMachine in Unity flow.
    /// </summary>
    public class StateMachineRunner : MonoBehaviour
    {
        private const string EditorNamePrefix = "StateMachineRunner";

        private bool _isInitialized;
        
        private StateMachine _machine;

        private void OnEnable()
        {
            _isInitialized = false;
        }

        private void Update()
        {
            if (!_isInitialized) return;
            if (_machine.IsNextStateAvailable)
            {
                _machine.RunNextState();
                return;
            }
            _machine.Run();
        }

        internal static string CreateEditorNameWith(string machineName)
        {
            return $"{EditorNamePrefix} - {machineName}";
        }

        internal void Initialize(StateMachine machine)
        {
            _machine = machine;
            _isInitialized = true;
        }

        internal StateMachine GetMachine() => _machine;
    }
}