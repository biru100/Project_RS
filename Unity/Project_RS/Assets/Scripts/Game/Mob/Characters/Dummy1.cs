﻿using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using Photon.Pun;

public sealed class Dummy1 : Character
{
    protected override void InitializeMob()
    {
        MobName = "Dummy1";
        Health = MaxHealth = 100;
        Speed = 7;

        Skill testSkill = new Skill(this, "펀치", "자기 자신을 때린다 ㅋㅋ", cooldown: 10, PunchLogic);

        Skills = new Dictionary<string, Skill>
        {
            ["punch"] = testSkill
        };
    }


    private GameObject[] _skillObjectList = new GameObject[3];
    private void Start()
    {
        for (int i = 0; i < transform.GetChild(1).childCount; i++)
            _skillObjectList[i] = transform.GetChild(1).GetChild(i).gameObject;
    }

    private IEnumerator PunchLogic()
    {
        photonView.RPC("PunchOnRPC", RpcTarget.All, Skills["punch"]._skillDirection.x, Skills["punch"]._skillDirection.y);
        yield return new WaitForSeconds(0.2f);
        photonView.RPC("PunchOffRPC", RpcTarget.All);
    }

    [PunRPC]
    private void PunchOnRPC(float x, float y)
    {
        _skillObjectList[0].SetActive(true);
        _skillObjectList[0].transform.localPosition = new Vector3(x, 0, y);
    }

    [PunRPC]
    private void PunchOffRPC()
    {
        _skillObjectList[0].SetActive(false);
    }
}
