using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using Cysharp.Threading.Tasks;
using System.Threading;
using PegC.Util;
using System;

public class PegCMemoryCheck : MonoBehaviour
{
	public SpriteRenderer checkSprite;

	// Start is called before the first frame update
	async void TestStart()
	{
		checkSprite.color = new Color(1,1,1,0);
		try{
			await checkSprite.AlphaTo(1.0f, 1.5f);
			Debug.Log("MemoryCheck: <color=green>Cancelled</color>");
		}
		catch{
			Debug.Log("MemoryCheck: <color=red>Cancelled</color>");
		}
		finally{
		}
	}

	void setColor(Action<float> func){
		func?.Invoke(1);
	}

	void setColor(SpriteRenderer fromObj,float color){
		var col = fromObj.color;
		col.a = color;
		fromObj.color = col;
	}

	struct ColorData{
		public SpriteRenderer fromObj;
		public ColorData(SpriteRenderer fromObj){
			this.fromObj = fromObj;
		}
	}

	class ColorData2{
		public SpriteRenderer fromObj;
		public ColorData2(SpriteRenderer fromObj){
			this.fromObj = fromObj;
		}
	}

	public interface IColorData{
		SpriteRenderer fromObj { get; }
	}

	struct ColorData3 : IColorData{
		public SpriteRenderer fromObj { get; private set; }
		public ColorData3(SpriteRenderer fromObj){
			this.fromObj = fromObj;
		}
	}

	void setColor(ColorData data){
		var col = data.fromObj.color;
		col.a = 1;
		data.fromObj.color = col;
	}

	void setColor(SpriteRenderer sprr){
		var data = new ColorData3(sprr);// as IColorData;
		var col = data.fromObj.color;
		col.a = 1;
		data.fromObj.color = col;
	}

	void Start()
	{
	}

	string sampleName = "PegC";

	void test(){
		checkSprite.color = new Color(1,1,1,0);
		Profiler.BeginSample(sampleName);
		//TestStart();
		var fromObj = checkSprite;
		//setColor((newPos)=>{ var col = fromObj.color; col.a = newPos; fromObj.color = col; });
		//setColor(fromObj,1);
		//setColor(new ColorData(fromObj));
		setColor(fromObj);
		Profiler.EndSample();
	}

	// Update is called once per frame
	void Update()
	{
		test();
	}
}
