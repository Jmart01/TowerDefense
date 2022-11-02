using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUI : MonoBehaviour
{
    // Start is called before the first frame update
    public Text MoneyVisual;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateMoneyUI(int MoneyAmt)
    {
        MoneyVisual.text = MoneyAmt.ToString();
    }
}
