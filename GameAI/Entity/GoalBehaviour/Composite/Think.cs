using GameAI.GoalBehaviour;
using Microsoft.Xna.Framework;

namespace GameAI.Entity.GoalBehaviour.Composite
{
    public class Think<TOwner> : GoalComposite<TOwner>
    {
        public Think(TOwner owner) : base(owner) { }
    }
}