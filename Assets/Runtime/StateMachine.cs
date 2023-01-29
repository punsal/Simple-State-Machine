using System.Collections.Generic;
using UnityEngine;

namespace Simple.StateMachine.Runtime
{
    /// <summary>
    /// Creates StateMachines, manages StateMachineRunners and optimizes Unity callback loads
    /// </summary>
    public class StateMachineManager
    {
        private Dictionary<string, StateMachineRunner> _machines;

        public void Initialize()
        {
            _machines = new Dictionary<string, StateMachineRunner>();
        }

        public StateMachine Create(string machineName)
        {
            var runner = CreateRunner(machineName);
            var machine = new StateMachine(machineName);
            runner.Initialize(machine);
            _machines.Add(machineName, runner);
            return runner.GetMachine();
        }

        private StateMachineRunner CreateRunner(string machineName)
        {
            var temp = new GameObject(StateMachineRunner.CreateEditorNameWith(machineName));
            var runner = temp.AddComponent<StateMachineRunner>();
            return runner;
        }

        public bool IsMachineExist(string name)
        {
            return _machines.ContainsKey(name);
        }

        public bool TryGetMachine(string name, out StateMachine machine)
        {
            machine = null;
            if (!_machines.ContainsKey(name)) return false;
            var runner = _machines[name];
            machine = runner.GetMachine();
            return true;
        }
    }

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

        public static string CreateEditorNameWith(string machineName)
        {
            return $"{EditorNamePrefix} - {machineName}";
        }

        public void Initialize(StateMachine machine)
        {
            _machine = machine;
            _isInitialized = true;
        }
        
        public StateMachine GetMachine() => _machine;
    }

    
    public interface IState
    {
        void Initialize();
        void Update();
        void Dispose();
    }

    public class StateMachine
    {
        public const string ObjectNamePrefix = "StateMachine";
        public string GetName { get; }

        private IState _currentState;
        private IState _nextState;
        internal bool IsNextStateAvailable { get; private set; }


        public StateMachine(string name)
        {
            GetName = $"{ObjectNamePrefix} - {name}";
            _currentState = null;
            _nextState = null;
            IsNextStateAvailable = false;
        }

        internal void Run()
        {
            _currentState.Update();
        }
        
        public void ChangeState(IState state)
        {
            _nextState = state;
            IsNextStateAvailable = true;
        }

        internal void RunNextState()
        {
            _currentState.Dispose();
            
            _currentState = _nextState;
            _nextState = null;
            IsNextStateAvailable = false;
            
            _currentState.Initialize();
        }
    }
}