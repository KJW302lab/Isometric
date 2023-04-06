using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour
{
    protected string ClassName;
    protected int Hp;
    protected int Ap;
    protected int Att;

    protected void SetData(CharacterData data)
    {
        ClassName = data.className;
        Hp = data.hp;
        Ap = data.ap;
        Att = data.att;
    }
}
