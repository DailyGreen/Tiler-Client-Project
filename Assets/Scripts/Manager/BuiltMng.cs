using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuiltMng : MonoBehaviour
{
    public ACTIVITY act = ACTIVITY.NONE;


    public GameObject[] unitobj = null;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0) && act != ACTIVITY.ACTING && GameMng.I._UnitGM.act == ACTIVITY.NONE && !EventSystem.current.IsPointerOverGameObject())
        {
            switch (act)
            {
                case ACTIVITY.WORKER_UNIT_CREATE:
                    CreateUnit((int)UNIT.WORKER);
                    break;
            }
        }


        if (Input.GetMouseButtonDown(0) && GameMng.I._UnitGM.act == ACTIVITY.NONE && act == ACTIVITY.NONE && !EventSystem.current.IsPointerOverGameObject())
        {
            GameMng.I._range.AttackrangeTileReset();                                                     //클릭시 터렛 공격 범위 초기화
            GameMng.I.mouseRaycast();
            if (GameMng.I.selectedTile)
            if (GameMng.I.selectedTile._builtObj != null)
            {
                if (GameMng.I.selectedTile._code == (int)BUILT.ATTACK_BUILDING)
                {
                    GameMng.I.selectedTile._builtObj.GetComponent<Turret>().Attack();
                }
            }
        }
    }

    public void CreateUnit(int index)
    {
        GameMng.I.mouseRaycast(true);                       //캐릭터 정보와 타일 정보를 알아와야해서 false에서 true로 변경
        if (GameMng.I.targetTile._builtObj == null && GameMng.I.targetTile._code < (int)TILE.CAN_MOVE && GameMng.I.targetTile._unitObj == null && Vector2.Distance(GameMng.I.selectedTile.transform.localPosition, GameMng.I.targetTile.transform.localPosition) <= 1.5f)
        {
            GameObject Child = Instantiate(unitobj[index - 300], GameMng.I.targetTile.transform) as GameObject;                 // enum 값 - 100
            Child.transform.parent = transform.parent;
            GameMng.I.targetTile._unitObj = Child.GetComponent<Forest_Worker>();
            GameMng.I._range.rangeTileReset();
            act = ACTIVITY.ACTING;
            GameMng.I.targetTile._unitObj.uniqueNumber = NetworkMng.getInstance.uniqueNumber;
            GameMng.I.cleanActList();
            GameMng.I.cleanSelected();
        }
        else                                     // 범위가 아닌 다른 곳을 누름
        {
            act = ACTIVITY.NONE;
            GameMng.I.selectedTile = GameMng.I.targetTile;
            GameMng.I.targetTile = null;
            GameMng.I._range.rangeTileReset();
        }
    }

    /**
     * @brief 건물 파괴될때 호출됨
     */
    public void DestroyBuilt()
    {
        Destroy(GameMng.I.selectedTile._builtObj.gameObject);
        if (GameMng.I.selectedTile._builtObj._code == (int)BUILT.ATTACK_BUILDING)
        {
            GameMng.I._range.AttackrangeTileReset();
        }
        act = ACTIVITY.NONE;
        GameMng.I.selectedTile._builtObj = null;
        Debug.Log("여기 수정해야함!!!!!");
        GameMng.I.selectedTile._code = (int)TILE.GRASS;                                                             // 나중에 원래 타일 알아오는법 가져오기
        GameMng.I.cleanActList();
        GameMng.I.cleanSelected();
    }
}
