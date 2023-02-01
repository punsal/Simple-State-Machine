using System;
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

        public StateMachine Create(string machineName, IState startState)
        {
            if (_machines.ContainsKey(machineName))
            {
                throw new ArgumentException($"StateMachineManager already have a StateMachine with given name '{machineName}'");
            }
            var runner = CreateRunner(machineName);
            var machine = new StateMachine(machineName, startState);
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
}