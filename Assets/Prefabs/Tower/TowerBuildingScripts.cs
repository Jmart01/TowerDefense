using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBuildingScripts : MonoBehaviour
{
    [SerializeField] ValueGauge progressBarPrefab;
    [SerializeField] float BuildingTime = 2f;
    ValueGauge ProgressBar;
    
    
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Tower>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartBuilding()
    { 
        //in 2 secs the progress bar will go up 0 to 1, after that turn on tower script
        ProgressBar = Instantiate(progressBarPrefab);
        ProgressBar.SetOwner(gameObject);
        StartCoroutine(Build());
    }

    IEnumerator Build()
    {
        float timeSinceStarted = 0;

        while(timeSinceStarted < BuildingTime)
        {
            //update progress bar
            ProgressBar.setValue(timeSinceStarted / BuildingTime);
            timeSinceStarted += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        GetComponent<Tower>().enabled = true;
        Destroy(ProgressBar.gameObject);
    }
}
