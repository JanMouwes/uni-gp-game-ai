using GameAI.Entity.GoalBehaviour.Atomic;

namespace GameAI.Entity.GoalBehaviour.Composite
{
    public class HuntEnemy : GoalComposite<Ship>

    {
        private readonly Ship enemy;
        private readonly World world;

        public HuntEnemy(Ship owner, Ship enemy, World world) : base(owner)
        {
            this.enemy = enemy;
            this.world = world;
        }

        public override void Activate()
        {
            // AddSubgoal(new PursueEnemy(this.Owner, this.enemy, this.Owner.Scale + this.Owner.Scale, this.world.PathFinder));
            AddSubgoal(new SeekTarget(this.Owner, this.enemy, this.Owner.Scale));
            AddSubgoal(new AttackEnemy(this.Owner, this.enemy));
            
            base.Activate();
        }
    }
}