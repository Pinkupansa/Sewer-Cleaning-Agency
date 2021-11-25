using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public interface ISkill
{   
    string Name();
    string Description();
    void Use(Vector2 direction, Transform caster);
    string UsingButton();

}
