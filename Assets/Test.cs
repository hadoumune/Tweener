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
        Tweener.DefaultEasing = EaseType.BounceOut;

        var mesh = GetComponentInChildren<MeshRenderer>();

        while(true){
            await gameObject.LocalRotYTo(200.0f,2.0f);
            await mesh.ColorTo( Color.red,1.0f);
            await gameObject.XTo(3.0f,2.0f);
            mesh.ColorTo( Color.green,1.0f).Forget();
            await gameObject.YOffset(3.0f,1.0f);
            await gameObject.XTo(-3.0f,1.0f);
            mesh.ColorTo( Color.white,1.0f).Forget();
            await gameObject.YOffset<Default>(-3.0f,1.0f);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
