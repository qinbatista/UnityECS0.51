using System.ComponentModel;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
[GenerateAuthoringComponent]
public struct HealthData : IComponentData
{
    public int currentValue;
    public bool isDead;
}
