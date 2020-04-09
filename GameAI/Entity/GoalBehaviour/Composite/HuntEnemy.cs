using GameAI.Entity.GoalBehaviour.Atomic;
using GameAI.GoalBehaviour;

namespace GameAI.Entity.GoalBehaviour.Composite
{
    public class HuntEnemy : GoalComposite<Vehicle>

    {
        private readonly Vehicle enemy;
        private readonly World world;

        public HuntEnemy(Vehicle owner, Vehicle enemy, World world) : base(owner)
        {
            this.enemy = enemy;
            this.world = world;
        }

        public override void Activate()
        {
            this.AddSubgoal(new PursueEnemy(this.Owner, this.enemy, this.Owner.Scale, world.PathFinder));
            this.AddSubgoal(new AttackEnemy(this.Owner, this.enemy));
            
            base.Activate();
        }
    }
}