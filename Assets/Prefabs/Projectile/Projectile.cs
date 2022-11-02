using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CapsuleCollider))]
public class Projectile : MonoBehaviour
{
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] int Damage = 1;
    Enemy Target;
    Effects EffectProjectile;

    public void SetDamage(int newDmg)
    {
        Damage = newDmg;
    }
    public void SetEffect(Effects newEffect)
    {
        EffectProjectile = newEffect;
    }
    public void SetTarget(Enemy newTarget)
    {
        Target = newTarget;
    }


    // Start is called before the first frame update
    void Start()
    {
        GetComponent<CapsuleCollider>().isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(Target)
        {
            transform.LookAt(Target.transform);
        }

        transform.position += transform.forward * projectileSpeed * Time.deltaTime;

        if(!gameObject.GetComponentInChildren<MeshRenderer>().isVisible)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == Target.gameObject)
        {
            OnTargetHit();
        }
    }

    void OnTargetHit()
    {
        if(Target)
        {
            Target.TakeDamageAndApplyEffect(Damage, EffectProjectile);
        }
        Destroy(gameObject);
    }
}

