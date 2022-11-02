using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Effects
{
    public bool canFreeze = false;
    public float SlowdownSpeed;
    public float SlowdownCooldown;
}
public class Tower : MonoBehaviour
{
    [SerializeField] Projectile ProjectilePrefab;
    [SerializeField] float AttackRange = 4f;
    [SerializeField] float AttackCooldownTime = 1f;
    [SerializeField] LayerMask EnemyLayer;
    [SerializeField] int TowerDamage;
    [SerializeField] int CostToBuild = 0;
    Animator AnimationControl;
    Coroutine AttackCoroutine;
    float CooldownTimeLeft = 0f;
    Enemy Target = null;

    [SerializeField] Effects Freeze;
    public int GetCost()
    {
        return CostToBuild;
    }
    
    //[SerializeField] float AttackTimer = 0f;
    // Start is called before the first frame update
    //bool Attacking = false;
    void Start()
    {
        AnimationControl = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Target == null || !isTargetInRange(Target))
        {
            if (AttackCoroutine != null)
            {
                StopCoroutine(AttackCoroutine);
                AttackCoroutine = null;
            }

            Target = FindClosestEnemyInAttackRange();
        }
       
        if(Target != null)
        {
            //Attacking = true;
            
            transform.LookAt(Target.transform.position);
            if(AttackCoroutine == null)
            {
                AttackCoroutine = StartCoroutine(AttackTarget());
            }
            
        }


        CooldownTimeLeft = Mathf.Clamp(CooldownTimeLeft - Time.deltaTime, 0, AttackCooldownTime);
      /*if(Attacking)
        {
            if()
        }*/
    }


    IEnumerator AttackTarget()
    {
        while(true)
        {
            yield return new WaitForSeconds(CooldownTimeLeft);
            AnimationControl.Play("Attack");
            float AnimationTime = AnimationControl.GetCurrentAnimatorStateInfo(0).length;
            float AttackSpeedMult = 1f;
            if(AnimationTime > AttackCooldownTime)
            {
                AttackSpeedMult = AnimationTime / AttackCooldownTime;
            }
            AnimationControl.SetFloat("AttackSpeedMult", AttackSpeedMult);
            CooldownTimeLeft = AttackCooldownTime;

            yield return new WaitForSeconds(AttackCooldownTime);
        }
    }

    Enemy FindClosestEnemyInAttackRange()
    {
        Collider[] colsOverlapped = Physics.OverlapSphere(transform.position, AttackRange,EnemyLayer);
        if (colsOverlapped.Length == 0) return null;
        float ClosestDist = Vector3.Distance(colsOverlapped[0].transform.position, transform.position);
        int ClosestIndex = 0;
        for(int i = 1; i < colsOverlapped.Length; ++i)
        {
            float itemDist = Vector3.Distance(colsOverlapped[i].transform.position, transform.position);
            if(itemDist < ClosestDist)
            {
                ClosestDist = itemDist;
                ClosestIndex = i;
            }
        }
        return colsOverlapped[ClosestIndex].GetComponent<Enemy>();
        
    }
    bool isTargetInRange(Enemy currentTarget)
    {

        Collider[] colsOverlapped = Physics.OverlapSphere(transform.position, AttackRange, EnemyLayer);

        if (colsOverlapped.Length == 0) return false;



        for(int i = 0; i < colsOverlapped.Length; i++)
        {
            if(currentTarget.gameObject == colsOverlapped[i].gameObject)
            {
                return true;
            }
        }
        return false;
    }
    public void SpawnProjectile()
    {
        if (Target == null)
            return;

        Projectile newProjectile = Instantiate(ProjectilePrefab, transform.position, Quaternion.identity);
        newProjectile.SetEffect(Freeze);
        newProjectile.SetDamage(TowerDamage);
        newProjectile.SetTarget(Target);
    }
}
