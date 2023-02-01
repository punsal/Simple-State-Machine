namespace Simple.StateMachine.Runtime
{
    public class StateMachine
    {
        public string Name { get; }

        private IState _currentState;
        private IState _nextState;
        internal bool IsNextStateAvailable { get; private set; }


        public StateMachine(string name, IState currentState)
        {
            Name = name;

            _currentState = currentState;
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