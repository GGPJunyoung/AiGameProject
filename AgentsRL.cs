using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;


public class RLAgentController : AgentController
{
    // 상태, 행동, 보상 등 기존 변수/함수 그대로 사용

    public override void OnEpisodeBegin()
    {
        // 에피소드(학습 단위) 시작 시 초기화 로직
        // ex. 위치/체력/쿨타임 초기화 등
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // 현재 상태(관찰값) 입력
        sensor.AddObservation(blackboard.currentHealth / 100f); // 내 체력
        sensor.AddObservation(blackboard.enemyHealth / 100f);    // 적 체력
        sensor.AddObservation(blackboard.attackCooldown);        // 공격 쿨타임
        sensor.AddObservation(blackboard.defendCooldown);        // 방어 쿨타임
        sensor.AddObservation(blackboard.distanceToEnemy);       // 적과의 거리
        sensor.AddObservation(blackboard.evadeCooldown);         // 회피 쿨타임
        sensor.AddObservation(blackboard.

    public override NodeStatus PerformAttack(float damageMultiplier = 1.0f)
    {
        // ... (공격 로직)
        bool hitEnemy = /* 공격 성공 여부 */;
        if (hitEnemy)
        {
            AddReward(0.3f); // 공격 성공 보상
            if (enemyController.blackboard.currentHealth <= 0)
            {
                AddReward(1.0f); // 승리 보상
                EndEpisode();
            }
        }
        else
        {
            AddReward(-0.1f); // 공격 실패 패널티
        }
        return NodeStatus.SUCCESS;
    }

    public override NodeStatus PerformDefend()
    {
        // ... (방어 로직)
        bool defendSuccess = /* 방어 성공 여부 */;
        if (defendSuccess)
        {
            AddReward(0.2f); // 방어 성공 보상
        }
        else
        {
            AddReward(-0.05f); // 불필요한 방어
        }
        return NodeStatus.SUCCESS;
    }

    public override NodeStatus PerformEvade()
    {
        // ... (회피 로직)
        bool evadeSuccess = /* 회피 성공 여부 */;
        if (evadeSuccess)
        {
            AddReward(0.2f); // 회피 성공 보상
        }
        else
        {
            AddReward(-0.05f); // 불필요한 회피
        }
        return NodeStatus.SUCCESS;
    }

// 체력 깎임/패배 등에도 Reward, EndEpisode 부여
public override void HandleDamage(float damage, AgentController attacker)
    {
        // ...
        AddReward(-0.05f); // 데미지 입었을 때 패널티
        if (blackboard.currentHealth <= 0)
        {
            AddReward(-1.0f); // 패배 패널티
            EndEpisode();
        }
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        int action = actions.DiscreteActions[0];

        switch (action)
        {
            case 0: // Idle
                Idle();
                break;
            case 1: // Attack
                PerformAttack();
                break;
            case 2: // Defend
                PerformDefend();
                break;
            case 3: // Evade
                PerformEvade();
                break;
            case 4: // MoveAway
                MoveAwayFrom(enemy.position, 1.0f, evadeDistance);
                break;
        }
    }

}
