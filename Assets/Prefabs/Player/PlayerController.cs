using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Tower TowerPrefab;
    [SerializeField] Tower FreezeTowerPrefab;
    [SerializeField] LayerMask FloorLayerMask;
    [SerializeField] int StartingMoney;
    [SerializeField] MainUI mainUI;
    int PlayersMoney;
    InputActions inputActions;
    Vector2 MousePos;
    TowerBuildingScripts towerBuilding;

    Tower TowerToPlace;
    Coroutine TowerPlacingCoroutine;
    // Start is called before the first frame update

    private void Awake()
    {
        inputActions = new InputActions();
    }
    private void OnEnable()
    {
        inputActions.Enable();
    }
    private void OnDisable()
    {
        inputActions.Disable();
    }


    void Start()
    {
        inputActions.Gameplay.MousePosition.performed += ctx => MousePos = ctx.ReadValue<Vector2>();
        inputActions.Gameplay.LeftClick.performed += LeftClicked;
        StartingMoney = PlayersMoney;
        PlayersMoney = 100;
        mainUI.UpdateMoneyUI(PlayersMoney);
    }

    void StartPlaceTower(Tower TowerToChoose)
    {
        Debug.Log(PlayersMoney);
        Debug.Log(TowerToChoose.GetCost());
        if(TowerToChoose.GetCost() <= PlayersMoney)
        {
            TowerToPlace = Instantiate(TowerToChoose);
            TowerPlacingCoroutine = StartCoroutine(PlacingTower());
        }
    }

    IEnumerator PlacingTower()
    {
        while(true)
        {
            PlaceTowerUnderCursor();
            Debug.Log("Placing tower under cursor");
            yield return new WaitForEndOfFrame();
        }
    }
    void LeftClicked(InputAction.CallbackContext ctx)
    {
        if(TowerToPlace != null && TowerPlacingCoroutine != null)
        {
            StopCoroutine(TowerPlacingCoroutine);
            TowerToPlace.gameObject.GetComponent<TowerBuildingScripts>().StartBuilding();
            UpdateMoney(-TowerToPlace.GetCost());
            TowerToPlace = null;
            TowerPlacingCoroutine = null;
            ChangeTag();
        }
    }

    void PlaceTowerUnderCursor()
    {
        Ray TestRay = Camera.main.ScreenPointToRay(MousePos);
        if(Physics.Raycast(TestRay,out RaycastHit hitInfo,FloorLayerMask))
        {
            Floor floorFound = hitInfo.collider.GetComponentInParent<Floor>();
            if(floorFound && floorFound.GetFloorType() != FloorTypes.Road && floorFound.gameObject.tag != "TileFilled")
            {
                TowerToPlace.transform.position = hitInfo.collider.transform.position + new Vector3(0f, 1f, 0f);
            }
            
        }
    }
    public void UpdateMoney(int amt)
    {
        PlayersMoney += amt;
        mainUI.UpdateMoneyUI(PlayersMoney);
    }
    // Update is called once per frame
    
    public void OnNormalButtonClicked()
    {
        StartPlaceTower(TowerPrefab);
    }
    
    public void OnFreezeButtonClicked()
    {
        StartPlaceTower(FreezeTowerPrefab);
    }

    private void ChangeTag()
    {
        Ray TestRay = Camera.main.ScreenPointToRay(MousePos);
        if (Physics.Raycast(TestRay, out RaycastHit hitInfo, FloorLayerMask))
        {
            Floor floorFound = hitInfo.collider.GetComponentInParent<Floor>();
            if (floorFound && floorFound.GetFloorType() != FloorTypes.Road)
            {
                floorFound.gameObject.tag = "TileFilled";
            }

        }
    }
}
