namespace VRTRPG.Command
{
    public abstract class CommandBase
    {
        protected readonly CommandUnit commandUnit;

        protected CommandBase(CommandUnit unit)
        {
            commandUnit = unit;
        }

        public abstract void Execute();
        public abstract void AddToCommandQueue();
    }
}
