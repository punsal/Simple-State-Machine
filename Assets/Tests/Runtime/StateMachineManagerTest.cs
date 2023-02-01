using System;
using System.Collections;
using NUnit.Framework;
using Simple.StateMachine.Runtime;
using Simple.StateMachine.Tests.Runtime.TestStates;
using UnityEngine;
using UnityEngine.TestTools;

namespace Simple.StateMachine.Tests.Runtime
{
    public class StateMachineManagerTest : MonoBehaviour
    {
        private const string TestingName1 = "Test1";
        private const string TestingName2 = "Test2";

        private static StateMachineManager GetStateMachineManager()
        {
            var temp = new StateMachineManager();
            temp.Initialize();
            return temp;
        }
        
        [Test]
        public void CreateStateMachineTest()
        {
            // Arrange
            var managerUnderTest = GetStateMachineManager();
            managerUnderTest.Create(TestingName1, new EmptyState());

            // Act
            var isMachineExist = managerUnderTest.IsMachineExist(TestingName1);
            
            // Asset
            Assert.IsTrue(isMachineExist);
        }

        [Test]
        public void GetStateMachineTest()
        {
            // Arrange
            var managerUnderTest = GetStateMachineManager();
            managerUnderTest.Create(TestingName1, new EmptyState());
            
            // Act
            if (!managerUnderTest.TryGetMachine(TestingName1, out var machine))
            {
                Assert.Fail("StateMachine is not found in StateMachineManager.");
            }
            
            // Assert
            Assert.IsNotNull(machine);
            Assert.AreEqual(TestingName1, machine.Name);
        }

        [Test]
        public void CreateDuplicateMachinesTest()
        {
            // Arrange
            var managerUnderTest = GetStateMachineManager();
            managerUnderTest.Create(TestingName1, new EmptyState());
            
            try
            {
                // Act
                managerUnderTest.Create(TestingName1, new EmptyState());

                // Assert
                Assert.Fail("StateMachineManager shouldn't create multiple StateMachines with same name.");
            }
            catch (ArgumentException e)
            {
                Assert.Pass(e.Message);
            }
            
            Assert.Fail("Test should throw 'ArgumentException'.");
        }

        [Test]
        public void CreateMultipleMachinesTest()
        {
            // Arrange
            var managerUnderTest = GetStateMachineManager();
            managerUnderTest.Create(TestingName1, new EmptyState());
            
            try
            {
                // Act
                managerUnderTest.Create(TestingName2, new EmptyState());
            }
            catch (ArgumentException e)
            {
                // Assert
                Assert.Fail(e.Message);
            }

            // Act Then Assert
            var isMachine1Exist = managerUnderTest.IsMachineExist(TestingName1);
            Assert.IsTrue(isMachine1Exist);

            var isMachine2Exist = managerUnderTest.IsMachineExist(TestingName2);
            Assert.IsTrue(isMachine2Exist);

            if (!managerUnderTest.TryGetMachine(TestingName1, out var expectedMachine1))
            {
                Assert.Fail("StateMachine1 is not found in StateMachineManager.");
            }
            Assert.IsNotNull(expectedMachine1);
            Assert.AreEqual(TestingName1, expectedMachine1.Name);
            
            if (!managerUnderTest.TryGetMachine(TestingName2, out var expectedMachine2))
            {
                Assert.Fail("StateMachine is not found in StateMachineManager.");
            }
            Assert.IsNotNull(expectedMachine2);
            Assert.AreEqual(TestingName2, expectedMachine2.Name);
        }

        [UnityTest]
        public IEnumerator ChangeStateOnSingleMachineStep()
        {
            var manager = GetStateMachineManager();
            var machineUnderTest = manager.Create(TestingName1, new EmptyState());
            yield return null;
            
            var state1 = new TestState1();
            machineUnderTest.ChangeState(state1);
            var waitForEndOfFrame = new WaitForEndOfFrame();
            yield return waitForEndOfFrame;
            yield return waitForEndOfFrame;
            yield return waitForEndOfFrame;

            var monoHelper = state1.MonoHelper;
            if (monoHelper == null)
            {
                Assert.Fail("State should create a MonoHelper.");
            }

            Debug.Log($"MonoHelper.StateName: {monoHelper.StateName}");
            
            if (monoHelper.StateName != state1.GetType().Name)
            {
                Assert.Fail("StateName should be given to MonoHelper.");
            }
            
            machineUnderTest.ChangeState(new EmptyState());
            yield return waitForEndOfFrame;
            yield return waitForEndOfFrame;

            if (monoHelper == null)
            {
                Assert.Pass();
            }
            
            Assert.Fail("There is still a MonoHelper in Scene.");
        }
    }
}
