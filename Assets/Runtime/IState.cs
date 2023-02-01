namespace Simple.StateMachine.Runtime
{
    public interface IState
    {
        void Initialize();
        void Update();
        void Dispose();
    }
}