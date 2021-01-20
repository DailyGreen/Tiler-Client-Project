using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forest_Soldier_0 : Unit
{
    void Awake()
    {
        _name = "전사 1";
        _desc = "생성까지 " + (3 - createCount) + "턴 남음";
        _cost = 0;
        _hp = 15;
        _code = (int)UNIT.FOREST_SOLDIER_0;
        _damage = 5;

        GameMng.I._BuiltGM.act = ACTIVITY.NONE;
        GameMng.I.AddDelegate(this.waitingCreate);
    }

    void init()
    {
        _activity.Add(ACTIVITY.MOVE);
        _activity.Add(ACTIVITY.ATTACK);
    }

    public void waitingCreate()
    {
        createCount++;
        _desc = "생성까지 " + (3 - createCount) + "턴 남음";

        if (createCount > 2)        // 2턴 후에 생성됨
        {
            _desc = "모조리 죽여주마!";

            _anim.SetTrigger("isSpawn");

            if (NetworkMng.getInstance.uniqueNumber.Equals(_uniqueNumber))
                init();

            GameMng.I.RemoveDelegate(this.waitingCreate);
        }
    }

    public void walking()
    {
        _anim.SetTrigger("isRunning");
    }

    public void dying()
    {
        _anim.SetTrigger("isDying");
    }

    void OnDestroy()
    {
        if (!(createCount > 2))
            GameMng.I.RemoveDelegate(waitingCreate);
    }
}
