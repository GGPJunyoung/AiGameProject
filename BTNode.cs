using System.Collections.Generic;

public abstract class BTNode
{
    public abstract bool Evaluate();
}

public class Selector : BTNode { /* ... */ }
public class Sequence : BTNode { /* ... */ }


public class IsAlive : BTNode { /* ... */ }
public class EndEpisode : BTNode { /* ... */ }
public class EnemyAttacking : BTNode { /* ... */ }
public class DefendCooldownReady : BTNode { /* ... */ }
public class Defend : BTNode { /* ... */ }