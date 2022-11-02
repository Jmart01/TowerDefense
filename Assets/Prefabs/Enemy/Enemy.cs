using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour
{
    List<Vector3> waypoints;
    NavMeshAgent navMeshAgent;
    int DestinationIndex = 0;
    private PlayerController playerController;

    [SerializeField] int StartHealth = 3;
    [SerializeField] ValueGauge HealthBarPrefab;
    [SerializeField] int MoneyToDrop;
    ValueGauge HealthBar;
    int Health;
    float NormalSpeed;
    bool isSlowed;

    // Start is called before the first frame update
    void Start()
    {
        
        Health = StartHealth;
        waypoints = GetPastWaypoints().ToList();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.SetDestination(waypoints[DestinationIndex]);
        NormalSpeed = navMeshAgent.speed;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Rigidbody>().useGravity = false;
        HealthBar = Instantiate(HealthBarPrefab);
        HealthBar.SetOwner(gameObject);
        isSlowed = false;
        playerController = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(navMeshAgent.remainingDistance <= 0.1f)
        {
            if (DestinationIndex == waypoints.Count - 1)
            {
                OnPathEndReached();
            }
            else
            {
                DestinationIndex = Mathf.Clamp(DestinationIndex + 1, 0, waypoints.Count - 1);
                navMeshAgent.SetDestination(waypoints[DestinationIndex]);
            }
        }
    }
    void OnPathEndReached()
    {
        Destroy(gameObject);
        if(HealthBar)
        {
            Destroy(HealthBar.gameObject);
        }
    }

    public void TakeDamageAndApplyEffect(int amt, Effects stats)
    {
        SetHealth(Health - amt);
        ApplyEffect(stats);
    }

    void SetHealth(int newValue)
    {
        Health = newValue;
        if (Health <= 0)
        {
            Health = 0;
            Dead();
        }
        if (HealthBar)
        {
            HealthBar.setValue((float)Health / (float)StartHealth);
        }
    }
    public void ApplyEffect(Effects stats)
    {
        if(stats.canFreeze && !isSlowed)
        {
            StartCoroutine(ApplyFreeze(stats));
        }
    }
    void Dead()
    {
        if(HealthBar)
        {
            Destroy(HealthBar.gameObject);
        }
        playerController.UpdateMoney(MoneyToDrop);
        Destroy(gameObject);
    }
    Vector3[] GetPastWaypoints()
    {
        LevelGenerator levelGen = FindObjectOfType<LevelGenerator>();
        if(levelGen != null)
        {
            return levelGen.GetPathWaypoints();
        }
        return null;
    }

    IEnumerator ApplyFreeze(Effects Freeze)
    {
        isSlowed = true;
        navMeshAgent.speed = Freeze.SlowdownSpeed* navMeshAgent.speed;
        yield return new WaitForSeconds(Freeze.SlowdownCooldown);
        navMeshAgent.speed = NormalSpeed;
        isSlowed = false;
    }
}
