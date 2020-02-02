using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleShakeVIew : MonoBehaviour
{
  public CameraShake shake;

    void Update()
    {
        shake.Shake( 10f, 2f); 
    }
}
