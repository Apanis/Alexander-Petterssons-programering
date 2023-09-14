using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : ItemScript
{
   public abstract override void Use();
   public GameObject bulletImpactPrefab;
}
