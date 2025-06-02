public class DefenseAgentBT
{
    private BTNode root;

    public DefenseAgentBT(ArenaAgent agent)
    {
        root = new Sequence(
            new Selector(
                new IsAlive(agent),
                new EndEpisode(agent)
            ),
            new Selector(
                new Sequence(
                    new EnemyAttacking(agent),
                    new DefendCooldownReady(agent),
                    new Defend(agent)
                ),
                new Sequence(
                    new CanCounterAttack(agent),
                    new AttackCooldownReady(agent),
                    new Attack(agent)
                ),
                new Sequence(
                    new TooCloseToEnemy(agent),
                    new MoveAway(agent)
                )
            )
        );
    }

    public void Tick()
    {
        root.Evaluate();
    }
}
