using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeView : MonoBehaviour
{
   public CameraShake shake;

    public void Shake()
    {
        shake.Shake( 9f, 9f); 
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Shake();
        }
    }
}
