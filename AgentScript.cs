using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class ArenaAgent : Agent
{
    public float maxHealth = 100f;
    public float attackCooldown = 2.5f;
    public float defendCooldown = 2.5f;
    public float dodgeCooldown = 5f;
    public float attackRange = 2f;
    public float attackDamage = 10f;
    public float defendStunTime = 2f;

    private float currentHealth;
    private float lastAttackTime, lastDefendTime, lastDodgeTime;
    private bool isStunned = false;
    private float stunTimer = 0f;

    // 초기화
    public override void OnEpisodeBegin()
    {
        currentHealth = maxHealth;
        isStunned = false;
        stunTimer = 0f;
        // 위치, 쿨타임 등 초기화 코드 추가
    }

    // 관찰 공간 정의
    public override void CollectObservations(VectorSensor sensor)
    {
        // 자기 상태
        sensor.AddObservation(currentHealth / maxHealth);
        sensor.AddObservation(Time.time - lastAttackTime < attackCooldown ? 1 : 0);
        sensor.AddObservation(Time.time - lastDefendTime < defendCooldown ? 1 : 0);
        sensor.AddObservation(isStunned ? 1 : 0);

        // 상대 상태 (예시)
        // sensor.AddObservation(...);

        // 위치 정보 등
    }

    // 행동 공간 정의 및 실행
    public override void OnActionReceived(ActionBuffers actions)
    {
        if (isStunned)
        {
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0) isStunned = false;
            return; // 행동 불가
        }

        int moveDir = actions.DiscreteActions[0]; // 0:가만, 1:앞, 2:뒤, 3:좌, 4:우
        int actType = actions.DiscreteActions[1]; // 0:없음, 1:공격, 2:방어, 3:회피

        // 이동 처리
        Move(moveDir);

        // 행동 처리
        if (actType == 1 && Time.time - lastAttackTime >= attackCooldown)
        {
            Attack();
            lastAttackTime = Time.time;
        }
        else if (actType == 2 && Time.time - lastDefendTime >= defendCooldown)
        {
            Defend();
            lastDefendTime = Time.time;
        }
        else if (actType == 3 && Time.time - lastDodgeTime >= dodgeCooldown)
        {
            Dodge();
            lastDodgeTime = Time.time;
        }
    }

    // 공격 함수 (상대와 거리 체크, 데미지 주기)
    void Attack()
    {
        // 상대 에이전트와 거리 체크 후 데미지 부여
        if (상대와_충돌 && 거리_내)
        {
            상대.TakeDamage(attackDamage);
            AddReward(0.2f); // 공격 성공 보상
            if (상대.currentHealth <= 0)
            {
                AddReward(1.0f); // 승리 보상
            }
        }
        // 성공 시 AddReward(공격형 보상)
    }

    // 방어 함수 (방패, 성공 시 상대 스턴)
    void Defend()
    {
        // 만약 공격이 들어오면 데미지 무효화 및 상대 스턴 적용
        // 상대 스크립트에서 agent.Stun(defendStunTime) 호출
        {
            if (상대가_공격안함)
            {
                AddReward(-0.05f); // 쓸데없는 방어 페널티
            }
        }
    }

    // 회피 함수 (무적 및 이동)
   

    // 데미지 받기
    public void TakeDamage(float amount)
    {
        if (/*방어 성공*/ false) return; // 데미지 무효
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            SetReward(-1f); // 패배
            EndEpisode();
        }
    }

    // 스턴 함수
    public void Stun(float duration)
    {
        isStunned = true;
        stunTimer = duration;
    }

    void Move(int dir)
    {
        // 방향별 이동 구현
    }
}