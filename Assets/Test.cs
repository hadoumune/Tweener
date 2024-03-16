using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using PegC.Util;

public class Test : MonoBehaviour
{

    // Start is called before the first frame update
    async UniTask Start()
    {
        //await gameObject.LocalRotXTo(200.0f,5.0f,EaseType.Linear);
        await gameObject.LocalRotYTo(200.0f,5.0f,EaseType.Linear);
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
