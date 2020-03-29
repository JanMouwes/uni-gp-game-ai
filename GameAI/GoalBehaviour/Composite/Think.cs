namespace GameAI.GoalBehaviour.Composite
{
    public class Think<TOwner> : GoalComposite<TOwner>
    {
        public Think(TOwner owner) : base(owner) { }
    }
}