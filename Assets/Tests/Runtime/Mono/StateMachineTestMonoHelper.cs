using System;
using UnityEngine;

namespace Simple.StateMachine.Tests.Runtime.Mono
{
    public class StateMachineTestMonoHelper : MonoBehaviour, IDisposable
    {
        public string StateName { get; set; }
        
        public void Dispose()
        {
            Destroy(gameObject);
        }
    }
}