using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using PegC.Util;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;

namespace PegC.Sample
{
	/// <summary>
	/// グラフの描画
	/// </summary>
	public class Graph : MonoBehaviour
	{
		CancellationToken ct => gameObject.GetCancellationTokenOnDestroy();

		public TMP_Text label;
		public EaseType easeType = EaseType.Linear;
		public SpriteRenderer point;
		public TrailRenderer trail;

		Func<float,float,float,float> easeFunc = null;

		EaseType prevEaseType = EaseType.Linear;

		Func<float,float,float,float> getFunc(EaseType type){
			if ( !Tweener.EaseFloat.ContainsKey(type) ){
				type = EaseType.Linear;
			}
			return Tweener.EaseFloat[type];
		}

		void Awake()
		{
			easeFunc = getFunc(easeType);
		}

		// Start is called before the first frame update
		async UniTask Start()
		{
			label.text = easeType.ToString();
			try{
				while( true ){
					await label.AlphaTo(0.0f,0.5f,ct:ct);
					await label.AlphaTo(1.0f,0.5f,ct:ct);
					for( float t = 0.0f ; t < 1.0f ; ){
						var time = Mathf.Clamp01(t);
						var v = easeFunc(-50.0f,50.0f,time);
						var pos = new Vector3(-50.0f+time*100.0f,v,0.0f);
						point.transform.localPosition = pos;
						//trail.transform.localPosition = pos;
						if ( t <= 0.0f){
							trail.Clear();
						}
						await UniTask.Yield(ct);
						t += Time.deltaTime;
					}
					await UniTask.WaitForSeconds(1.0f,cancellationToken:ct);
					trail.Clear();
					await UniTask.Yield(ct);
				}
			}
			catch
			{
			}
		}

		// Update is called once per frame
		void LateUpdate()
		{
			if ( prevEaseType != easeType ){
				prevEaseType = easeType;
				easeFunc = getFunc(easeType);
				label.text = easeType.ToString();
			}
		}
	}
}