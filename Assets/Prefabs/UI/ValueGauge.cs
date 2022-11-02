using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ValueGauge : MonoBehaviour
{
    GameObject owner;
    [SerializeField] Vector2 Offset;
    [SerializeField] Slider slider;
    public void SetOwner(GameObject newOwner)
    {
        owner = newOwner;
    }
    public void setValue(float value)
    {
        slider.value = value;
    }
    // Start is called before the first frame update
    void Start()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        GetComponent<RectTransform>().SetParent(canvas.transform);
    }

    // Update is called once per frame
    void Update()
    {
        if(owner)
        {
            Vector3 ScreenPos = Camera.main.WorldToScreenPoint(owner.transform.position);
            transform.position = ScreenPos + new Vector3(Offset.x, Offset.y,0f);
        }
    }
}
