using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;
using PegC.Util;
using TMPro;

public class Test : MonoBehaviour
{
	public TMP_Text text;

	async UniTask joinStatus(CancellationToken cancel,string baseString="Join",string statusString="･ ･ ･"){
		text.text = $"{baseString}{statusString}";
		text.maxVisibleCharacters = baseString.Length;
		text.alpha = 0.0f;
		try{
			var baseLen = baseString.Length;
			var statusLen = statusString.Length;
			await text.AlphaTo(1.0f,1.0f,EaseType.Linear,cancel);
			await text.TextSend(baseLen,baseLen+statusLen,statusLen*0.2f,EaseType.Linear,cancel,repeat:-1);
		}
		catch( System.Exception e )
		{
			Debug.Log($"JoinComplelte:{e.Message}");
		}
	}

	CancellationToken ct => gameObject.GetCancellationTokenOnDestroy();

	// Start is called before the first frame update
	async UniTask Start()
	{
		Tweener.DefaultEasing = EaseType.BounceOut;

		var mesh = GetComponentInChildren<MeshRenderer>();

		while(true){
			var joinComplete = new CancellationTokenSource();
			joinStatus(joinComplete.Token).Forget();
			await gameObject.LocalRotYTo(200.0f,2.0f);
			await mesh.ColorTo( Color.red,1.0f);
			await gameObject.XTo(3.0f,2.0f);
			mesh.ColorTo( Color.green,1.0f).Forget();
			await gameObject.YOffset(3.0f,1.0f);
			joinComplete.Cancel();

			{
				await text.AlphaTo<Linear>(0.0f,0.2f,ct);
				text.text = "Join complete";
				text.maxVisibleCharacters = text.text.Length;
				await text.AlphaTo<Linear>(1.0f,0.2f,ct);
				await UniTask.WaitForSeconds(1.0f,cancellationToken:ct);
				await text.AlphaTo<Linear>(0.0f,1.0f,ct);

			}
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
