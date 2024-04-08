using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System.Threading;
using TMPro;
using System;
using UnityEngine.Experimental.GlobalIllumination;

namespace PegC.Util
{
	public enum EaseType
	{
		Linear,
		SineIn, SineOut, SineInOut,
		QuadIn, QuadOut, QuadInOut,
		CubicIn, CubicOut, CubicInOut,
		QuartIn, QuartOut, QuartInOut,
		QuintIn, QuintOut, QuintInOut,
		ExpoIn, ExpoOut, ExpoInOut,
		CircIn, CircOut, CircInOut,
		BackIn, BackOut, BackInOut,
		ElasticIn, ElasticOut, ElasticInOut,
		BounceIn, BounceOut, BounceInOut,
		Spring,
		Default,
	}

	public interface IEaseTypeHint{
		public EaseType FuncType();
	}
	// Tween<T>に渡すクラス.
	public struct Linear:IEaseTypeHint{public EaseType FuncType()=>EaseType.Linear;}
	public struct SineIn:IEaseTypeHint{public EaseType FuncType()=>EaseType.SineIn;}
	public struct SineOut:IEaseTypeHint{public EaseType FuncType()=>EaseType.SineOut;}
	public struct SineInOut:IEaseTypeHint{public EaseType FuncType()=>EaseType.SineInOut;}
	public struct QuadIn:IEaseTypeHint{public EaseType FuncType()=>EaseType.QuadIn;}
	public struct QuadOut:IEaseTypeHint{public EaseType FuncType()=>EaseType.QuadOut;}
	public struct QuadInOut:IEaseTypeHint{public EaseType FuncType()=>EaseType.QuadInOut;}
	public struct CubicIn:IEaseTypeHint{public EaseType FuncType()=>EaseType.CubicIn;}
	public struct CubicOut:IEaseTypeHint{public EaseType FuncType()=>EaseType.CubicOut;}
	public struct CubicInOut:IEaseTypeHint{public EaseType FuncType()=>EaseType.CubicInOut;}
	public struct QuartIn:IEaseTypeHint{public EaseType FuncType()=>EaseType.QuartIn;}
	public struct QuartOut:IEaseTypeHint{public EaseType FuncType()=>EaseType.QuartOut;}
	public struct QuartInOut:IEaseTypeHint{public EaseType FuncType()=>EaseType.QuartInOut;}
	public struct QuintIn:IEaseTypeHint{public EaseType FuncType()=>EaseType.QuintIn;}
	public struct QuintOut:IEaseTypeHint{public EaseType FuncType()=>EaseType.QuintOut;}
	public struct QuintInOut:IEaseTypeHint{public EaseType FuncType()=>EaseType.QuintInOut;}
	public struct ExpoIn:IEaseTypeHint{public EaseType FuncType()=>EaseType.ExpoIn;}
	public struct ExpoOut:IEaseTypeHint{public EaseType FuncType()=>EaseType.ExpoOut;}
	public struct ExpoInOut:IEaseTypeHint{public EaseType FuncType()=>EaseType.ExpoInOut;}
	public struct CircIn:IEaseTypeHint{public EaseType FuncType()=>EaseType.CircIn;}
	public struct CircOut:IEaseTypeHint{public EaseType FuncType()=>EaseType.CircOut;}
	public struct CircInOut:IEaseTypeHint{public EaseType FuncType()=>EaseType.CircInOut;}
	public struct BackIn:IEaseTypeHint{public EaseType FuncType()=>EaseType.BackIn;}
	public struct BackOut:IEaseTypeHint{public EaseType FuncType()=>EaseType.BackOut;}
	public struct BackInOut:IEaseTypeHint{public EaseType FuncType()=>EaseType.BackInOut;}
	public struct ElasticIn:IEaseTypeHint{public EaseType FuncType()=>EaseType.ElasticIn;}
	public struct ElasticOut:IEaseTypeHint{public EaseType FuncType()=>EaseType.ElasticOut;}
	public struct ElasticInOut:IEaseTypeHint{public EaseType FuncType()=>EaseType.ElasticInOut;}
	public struct BounceIn:IEaseTypeHint{public EaseType FuncType()=>EaseType.BounceIn;}
	public struct BounceOut:IEaseTypeHint{public EaseType FuncType()=>EaseType.BounceOut;}
	public struct BounceInOut:IEaseTypeHint{public EaseType FuncType()=>EaseType.BounceInOut;}
	public struct Spring:IEaseTypeHint{public EaseType FuncType()=>EaseType.Spring;}
	public struct Default:IEaseTypeHint{public EaseType FuncType()=>Tweener.DefaultEasing;}

	public static class Tweener
	{
		public static EaseType DefaultEasing { get; set;} = EaseType.Linear;

		readonly static Dictionary<EaseType, Interporate > Types = new Dictionary<EaseType, Interporate>
		{
			{EaseType.Linear, new CalcLerp()},
			{EaseType.SineIn, new CalcSineIn()}, {EaseType.SineOut, new CalcSineOut()}, {EaseType.SineInOut, new CalcSineInOut()},
			{EaseType.QuadIn, new CalcQuadIn()}, {EaseType.QuadOut, new CalcQuadOut()}, {EaseType.QuadInOut, new CalcQuadInOut()},
			{EaseType.CubicIn, new CalcCubicIn()}, {EaseType.CubicOut, new CalcCubicOut()}, {EaseType.CubicInOut, new CalcCubicInOut()},
			{EaseType.QuartIn, new CalcQuartIn()}, {EaseType.QuartOut, new CalcQuartOut()}, {EaseType.QuartInOut, new CalcQuartInOut()},
			{EaseType.QuintIn, new CalcQuintIn()}, {EaseType.QuintOut, new CalcQuintOut()}, {EaseType.QuintInOut, new CalcQuintInOut()},
			{EaseType.ExpoIn, new CalcExpoIn()}, {EaseType.ExpoOut, new CalcExpoOut()}, {EaseType.ExpoInOut, new CalcExpoInOut()},
			{EaseType.CircIn, new CalcCircIn()}, {EaseType.CircOut, new CalcCircOut()}, {EaseType.CircInOut, new CalcCircInOut()},
			{EaseType.BackIn, new CalcBackIn()}, {EaseType.BackOut, new CalcBackOut()}, {EaseType.BackInOut, new CalcBackInOut()},
			{EaseType.ElasticIn, new CalcElasticIn()}, {EaseType.ElasticOut, new CalcElasticOut()}, {EaseType.ElasticInOut, new CalcElasticInOut()},
			{EaseType.BounceIn, new CalcBounceIn()}, {EaseType.BounceOut, new CalcBounceOut()}, {EaseType.BounceInOut, new CalcBounceInOut()},
			{EaseType.Spring, new CalcSpring()}
		};

		interface IUpdaterBase {
			void Update(float t);
			void ReverseUpdate(float t);
		}

		struct UpdaterParam<T> {
			public T from;
			public T to;
			public System.Action<T> update;
			public Interporate op;
			public UpdaterParam( T from, T to, Interporate op,System.Action<T> update){
				this.from = from;
				this.to = to;
				this.update = update;
				this.op = op;
			}
		}

		struct FloatUpdater : IUpdaterBase
		{
			UpdaterParam<float> param;
			public FloatUpdater(float from, float to, Interporate op,System.Action<float> update ) { param = new UpdaterParam<float>(from, to, op, update); }
			public void Update(float t) => param.update.Invoke(param.op.Op(param.from, param.to, Mathf.Clamp01(t)));
			public void ReverseUpdate(float t) => param.update.Invoke(param.op.Op(param.to, param.from, Mathf.Clamp01(t)));
		}

		struct Vector2Updater : IUpdaterBase
		{
			UpdaterParam<Vector2> param;
			public Vector2Updater(Vector2 from, Vector2 to, Interporate op,System.Action<Vector2> update) { param = new UpdaterParam<Vector2>(from, to, op, update);}
			public void Update(float t) => param.update.Invoke(param.op.Op(param.from, param.to, Mathf.Clamp01(t)));
			public void ReverseUpdate(float t) => param.update.Invoke(param.op.Op(param.to, param.from, Mathf.Clamp01(t)));
		}

		struct Vector3Updater : IUpdaterBase
		{
			UpdaterParam<Vector3> param;
			public Vector3Updater(Vector3 from, Vector3 to, Interporate op,System.Action<Vector3> update) {	param = new UpdaterParam<Vector3>(from, to, op, update);}
			public void Update(float t) => param.update.Invoke(param.op.Op(param.from, param.to, Mathf.Clamp01(t)));
			public void ReverseUpdate(float t) => param.update.Invoke(param.op.Op(param.to, param.from, Mathf.Clamp01(t)));
		}

		struct Vector4Updater : IUpdaterBase
		{
			UpdaterParam<Vector4> param;
			public Vector4Updater(Vector4 from, Vector4 to, Interporate op,System.Action<Vector4> update) { param = new UpdaterParam<Vector4>(from, to, op, update);}
			public void Update(float t) => param.update.Invoke(param.op.Op(param.from, param.to, Mathf.Clamp01(t)));
			public void ReverseUpdate(float t) => param.update.Invoke(param.op.Op(param.to, param.from, Mathf.Clamp01(t)));
		}

		static async UniTask tweenBase(IUpdaterBase updater, float duration, CancellationToken ct,
												System.Action<bool> complete=null,int repeat=0, float delay=0, bool pingPong=false)
		{
			var counter = repeat+1;
			var isInfinite = repeat < 0;
			var invDuration = 1.0f;
			if ( duration > 0.0f ) invDuration = 1.0f/duration;
			try
			{
				while ( isInfinite || counter > 0 )
				{
					ct.ThrowIfCancellationRequested();
					if (delay > 0f) await UniTask.WaitForSeconds(delay,cancellationToken:ct);
					var t = 0f;
					while ( t <= 1f )
					{
						t += Time.deltaTime * invDuration;
						updater.Update(Mathf.Clamp01(t));
						await UniTask.NextFrame(cancellationToken:ct);
					}

					if ( pingPong )
					{
						if (delay > 0f) await UniTask.WaitForSeconds(delay,cancellationToken:ct);
						t = 0f;
						while ( t <= 1f )
						{
							t += Time.deltaTime * invDuration;
							updater.ReverseUpdate(Mathf.Clamp01(t));
							await UniTask.NextFrame(cancellationToken:ct);
						}
					}
					if ( !isInfinite ) counter--;

					if ( isInfinite ) complete?.Invoke(false);
				}
				// 有限回ならこちらにくる.
				if ( !isInfinite ) complete?.Invoke(true);
			}
			catch (System.Exception e)
			{
				throw;
			}
		}

		static CancellationToken getCT(GameObject go,CancellationToken? ct=null){
			if ( ct != null ) return ct.Value;
			return go?go.GetCancellationTokenOnDestroy():CancellationToken.None;
		}
		static CancellationToken getCT(Transform t,CancellationToken? ct=null) => getCT(t.gameObject);
		static CancellationToken getCT(Renderer r,CancellationToken? ct=null) => getCT(r.gameObject);
		static CancellationToken getCT(Graphic r,CancellationToken? ct=null) => getCT(r.gameObject);
		static CancellationToken getCT(SpriteRenderer r,CancellationToken? ct=null) => getCT(r.gameObject);
		static CancellationToken getCT(CanvasGroup g,CancellationToken? ct=null) => getCT(g.gameObject);
		static CancellationToken getCT(TMP_Text t,CancellationToken? ct=null) => getCT(t.gameObject);
		static CancellationToken getCT(Material m,CancellationToken? ct=null) {
			return ct!=null?ct.Value:CancellationToken.None;
		}

		// Transform
		public static async UniTask XTo(this Transform transform, float to,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			await Tween( transform.position.x, to, duration, (newPos)=>{ var p = transform.position; p.x = newPos; transform.position = p; },
												type, getCT(transform,ct), complete, repeat, delay, pingPong );
		}
		public static async UniTask YTo(this Transform transform, float to,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			await Tween( transform.position.y, to, duration, (newPos)=>{ var p = transform.position; p.y = newPos; transform.position = p; },
												type, getCT(transform,ct), complete, repeat, delay, pingPong );
		}
		public static async UniTask ZTo(this Transform transform, float to,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			await Tween( transform.position.z, to, duration, (newPos)=>{ var p = transform.position; p.z = newPos; transform.position = p; },
												type, getCT(transform,ct), complete, repeat, delay, pingPong );
		}
		public static async UniTask XOffset(this Transform transform, float offset,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			await transform.XTo( transform.position.x+offset, duration, type, getCT(transform,ct), complete, repeat, delay, pingPong);
		}
		public static async UniTask YOffset(this Transform transform, float offset,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			await transform.YTo( transform.position.y+offset,duration, type, getCT(transform,ct), complete, repeat, delay, pingPong);
		}
		public static async UniTask ZOffset(this Transform transform, float offset,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			await transform.ZTo( transform.position.z+offset, duration, type, getCT(transform,ct), complete, repeat, delay, pingPong);
		}

		public static async UniTask LocalXTo(this Transform transform, float to,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			await Tween( transform.localPosition.x, to, duration, (newPos)=>{ var p = transform.localPosition; p.x = newPos; transform.localPosition = p; },
												type, getCT(transform,ct), complete, repeat, delay, pingPong );
		}
		public static async UniTask LocalYTo(this Transform transform, float to,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			await Tween( transform.localPosition.y, to, duration, (newPos)=>{ var p = transform.localPosition; p.y = newPos; transform.localPosition = p; },
												type, getCT(transform,ct), complete, repeat, delay, pingPong );
		}
		public static async UniTask LocalZTo(this Transform transform, float to,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			await Tween( transform.localPosition.z, to, duration, (newPos)=>{ var p = transform.localPosition; p.z = newPos; transform.localPosition = p; },
												type, getCT(transform,ct), complete, repeat, delay, pingPong );
		}
		public static async UniTask LocalXOffset(this Transform transform, float offset,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			await transform.LocalXTo( transform.localPosition.x+offset, duration, type, getCT(transform,ct), complete, repeat, delay, pingPong);
		}
		public static async UniTask LocalYOffset(this Transform transform, float offset,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			await transform.LocalYTo( transform.localPosition.y+offset, duration, type, getCT(transform,ct), complete, repeat, delay, pingPong);
		}
		public static async UniTask LocalZOffset(this Transform transform, float offset,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			await transform.LocalZTo( transform.localPosition.z+offset, duration, type, getCT(transform,ct), complete, repeat, delay, pingPong);
		}

		public static async UniTask XYTo(this Transform transform, Vector2 to, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			await Tween( (Vector2)transform.position, to, duration, (newPos)=>{ var p = transform.position; p.x = newPos.x; p.y = newPos.y; transform.position = p; },
												type, getCT(transform,ct), complete, repeat, delay, pingPong );
		}
		public static async UniTask XYOffset(this Transform transform, Vector2 offset, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			await transform.XYTo( (Vector2)transform.localPosition+offset, duration, type, getCT(transform,ct), complete, repeat, delay, pingPong);
		}

		public static async UniTask LocalXYTo(this Transform transform, Vector2 to, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			await Tween( (Vector2)transform.localPosition, to, duration, (newPos)=>{ var p = transform.localPosition; p.x = newPos.x; p.y = newPos.y; transform.localPosition = p; },
												type, getCT(transform,ct), complete, repeat, delay, pingPong );
		}
		public static async UniTask LocalXYOffset(this Transform transform, Vector2 offset, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{

			await transform.LocalXYTo( (Vector2)transform.localPosition+offset, duration, type, getCT(transform,ct), complete, repeat, delay, pingPong);
		}

		public static async UniTask XYZTo(this Transform transform, Vector3 to, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			await Tween( transform.position, to, duration, (newPos)=>{ transform.position = newPos; },
												type, getCT(transform,ct), complete, repeat, delay, pingPong );
		}
		public static async UniTask XYZOffset(this Transform transform, Vector3 offset, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			await transform.XYZTo( transform.localPosition+offset, duration, type, getCT(transform,ct), complete, repeat, delay, pingPong);
		}
		public static async UniTask LocalXYZTo(this Transform transform, Vector3 to, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			await Tween( transform.localPosition, to, duration, (newPos)=>{ transform.localPosition = newPos; },
												type, getCT(transform,ct), complete, repeat, delay, pingPong );
		}

		public static async UniTask LocalXYZOffset(this Transform transform, Vector3 offset, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			await transform.LocalXYZTo( transform.localPosition+offset, duration, type, getCT(transform,ct), complete, repeat, delay, pingPong);
		}

		public static async UniTask LocalRotXTo(this Transform transform, float toAngle,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			await Tween( transform.localEulerAngles.x, toAngle, duration, (newPos)=>{ var p = transform.localEulerAngles; p.x = newPos; transform.localEulerAngles = p;},
												type, getCT(transform,ct), complete, repeat, delay, pingPong );
		}
		public static async UniTask LocalRotYTo(this Transform transform, float toAngle,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			await Tween( transform.localEulerAngles.y, toAngle, duration, (newPos)=>{ var p = transform.localEulerAngles; p.y = newPos; transform.localEulerAngles = p; },
												type, getCT(transform,ct), complete, repeat, delay, pingPong );
		}
		public static async UniTask LocalRotZTo(this Transform transform, float toAngle,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			await Tween( transform.localEulerAngles.z, toAngle, duration, (newPos)=>{ var p = transform.localEulerAngles; p.z = newPos; transform.localEulerAngles = p; },
												type, getCT(transform,ct), complete, repeat, delay, pingPong );
		}

		public static async UniTask RotXTo(this Transform transform, float toAngle,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			await Tween( transform.eulerAngles.x, toAngle, duration, (newPos)=>{ var p = transform.eulerAngles; p.x = newPos; transform.eulerAngles = p; },
												type, getCT(transform,ct), complete, repeat, delay, pingPong );
		}
		public static async UniTask RotYTo(this Transform transform, float toAngle,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			await Tween( transform.eulerAngles.y, toAngle, duration, (newPos)=>{ var p = transform.eulerAngles; p.y = newPos; transform.eulerAngles = p; },
												type, getCT(transform,ct), complete, repeat, delay, pingPong );
		}
		public static async UniTask RotZTo(this Transform transform, float toAngle,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			await Tween( transform.eulerAngles.z, toAngle, duration, (newPos)=>{ var p = transform.eulerAngles; p.z = newPos; transform.eulerAngles = p; },
												type, getCT(transform,ct), complete, repeat, delay, pingPong );
		}

		public static async UniTask ScaleXTo(this Transform transform, float to,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			await Tween( transform.localScale.x, to, duration, (newPos)=>{ var p = transform.localScale; p.x = newPos; transform.localScale = p; },
												type, getCT(transform,ct), complete, repeat, delay, pingPong );
		}
		public static async UniTask ScaleYTo(this Transform transform, float to,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			await Tween( transform.localScale.y, to, duration, (newPos)=>{ var p = transform.localScale; p.y = newPos; transform.localScale = p; },
												type, getCT(transform,ct), complete, repeat, delay, pingPong );
		}
		public static async UniTask ScaleZTo(this Transform transform, float to,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			await Tween( transform.localScale.z, to, duration, (newPos)=>{ var p = transform.localScale; p.z = newPos; transform.localScale = p; },
												type, getCT(transform,ct), complete, repeat, delay, pingPong );
		}

		public static async UniTask ScaleXYTo(this Transform transform, Vector2 to, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			await Tween( (Vector2)transform.localScale, to, duration, (newPos)=>{ var p = transform.localScale; p.x = newPos.x; p.y = newPos.y; transform.localScale = p; },
												type, getCT(transform,ct), complete, repeat, delay, pingPong );
		}

		public static async UniTask ScaleXYZTo(this Transform transform, Vector3 to, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			await Tween( transform.localScale, to, duration, (newPos)=>{ transform.localScale = newPos; },
												type, getCT(transform,ct), complete, repeat, delay, pingPong );
		}


		// GameObject
		public static async UniTask XTo(this GameObject fromObj, float to,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.XTo( to, duration, type, getCT(fromObj,ct), complete, repeat, delay, pingPong);
		public static async UniTask YTo(this GameObject fromObj, float to,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.YTo( to, duration, type, getCT(fromObj,ct), complete, repeat, delay, pingPong);
		public static async UniTask ZTo(this GameObject fromObj, float to,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.ZTo( to, duration, type, getCT(fromObj,ct), complete, repeat, delay, pingPong);
		public static async UniTask XOffset(this GameObject fromObj, float offset,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.XOffset( offset, duration, type, getCT(fromObj,ct), complete, repeat, delay, pingPong);
		public static async UniTask YOffset(this GameObject fromObj, float offset,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.YOffset( offset, duration, type, getCT(fromObj,ct), complete, repeat, delay, pingPong);
		public static async UniTask ZOffset(this GameObject fromObj, float offset,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.ZOffset( offset, duration, type, getCT(fromObj,ct), complete, repeat, delay, pingPong);
		public static async UniTask LocalXTo(this GameObject fromObj, float to,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.LocalXTo( to, duration, type, getCT(fromObj,ct), complete, repeat, delay, pingPong);
		public static async UniTask LocalYTo(this GameObject fromObj, float to,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.LocalYTo( to, duration, type, getCT(fromObj,ct), complete, repeat, delay, pingPong);
		public static async UniTask LocalZTo(this GameObject fromObj, float to,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.LocalZTo( to, duration, type, getCT(fromObj,ct), complete, repeat, delay, pingPong);
		public static async UniTask LocalXOffset(this GameObject fromObj, float offset,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.LocalXOffset( offset, duration, type, getCT(fromObj,ct), complete, repeat, delay, pingPong);
		public static async UniTask LocalYOffset(this GameObject fromObj, float offset,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.LocalYOffset( offset, duration, type, getCT(fromObj,ct), complete, repeat, delay, pingPong);
		public static async UniTask LocalZOffset(this GameObject fromObj, float offset,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.LocalZOffset( offset, duration, type, getCT(fromObj,ct), complete, repeat, delay, pingPong);
		public static async UniTask XYTo(this GameObject fromObj, Vector2 to, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.XYTo( to, duration, type, getCT(fromObj,ct), complete, repeat, delay, pingPong);
		public static async UniTask XYOffset(this GameObject fromObj, Vector2 offset, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.XYOffset( offset, duration, type, getCT(fromObj,ct), complete, repeat, delay, pingPong);
		public static async UniTask LocalXYTo(this GameObject fromObj, Vector2 to, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.LocalXYTo( to, duration, type, getCT(fromObj,ct), complete, repeat, delay, pingPong);
		public static async UniTask LocalXYOffset(this GameObject fromObj, Vector2 offset, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.LocalXYOffset( offset, duration, type, getCT(fromObj,ct), complete, repeat, delay, pingPong);
		public static async UniTask XYZTo(this GameObject fromObj, Vector3 to, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.XYZTo( to, duration, type, getCT(fromObj,ct), complete, repeat, delay, pingPong);
		public static async UniTask XYZOffset(this GameObject fromObj, Vector3 offset, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.XYZOffset( offset, duration, type, getCT(fromObj,ct), complete, repeat, delay, pingPong);
		public static async UniTask LocalXYZTo(this GameObject fromObj, Vector3 to, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.LocalXYZTo( to, duration, type, getCT(fromObj,ct), complete, repeat, delay, pingPong);
		public static async UniTask LocalXYZOffset(this GameObject fromObj, Vector3 offset, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.LocalXYZOffset( offset, duration, type, getCT(fromObj,ct), complete, repeat, delay, pingPong);

		public static async UniTask RotXTo(this GameObject fromObj, float toAngle,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.RotXTo( toAngle, duration, type, getCT(fromObj,ct), complete, repeat, delay, pingPong);
		public static async UniTask RotYTo(this GameObject fromObj, float toAngle,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.RotYTo( toAngle, duration, type, getCT(fromObj,ct), complete, repeat, delay, pingPong);
		public static async UniTask RotZTo(this GameObject fromObj, float toAngle,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.RotZTo( toAngle, duration, type, getCT(fromObj,ct), complete, repeat, delay, pingPong);

		public static async UniTask LocalRotXTo(this GameObject fromObj, float toAngle,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.LocalRotXTo( toAngle, duration, type, getCT(fromObj,ct), complete, repeat, delay, pingPong);
		public static async UniTask LocalRotYTo(this GameObject fromObj, float toAngle,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.LocalRotYTo( toAngle, duration, type, getCT(fromObj,ct), complete, repeat, delay, pingPong);
		public static async UniTask LocalRotZTo(this GameObject fromObj, float toAngle,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.LocalRotZTo( toAngle, duration, type, getCT(fromObj,ct), complete, repeat, delay, pingPong);

		public static async UniTask ScaleXTo(this GameObject fromObj, float to,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.ScaleXTo( to, duration, type, getCT(fromObj,ct), complete, repeat, delay, pingPong);
		public static async UniTask ScaleYTo(this GameObject fromObj, float to,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.ScaleYTo( to, duration, type, getCT(fromObj,ct), complete, repeat, delay, pingPong);
		public static async UniTask ScaleZTo(this GameObject fromObj, float to,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.ScaleZTo( to, duration, type, getCT(fromObj,ct), complete, repeat, delay, pingPong);
		public static async UniTask ScaleXYTo(this GameObject fromObj, Vector2 to, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.ScaleXYTo( to, duration, type, getCT(fromObj,ct), complete, repeat, delay, pingPong);
		public static async UniTask ScaleXYZTo(this GameObject fromObj, Vector3 to, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.ScaleXYZTo( to, duration, type, getCT(fromObj,ct), complete, repeat, delay, pingPong);

		// SpriteRenderer
		public static async UniTask SizeTo(this SpriteRenderer fromObj, Vector2 to, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			await Tween( (Vector2)fromObj.size, to, duration, (newPos)=>{ fromObj.size = newPos; },
												type, getCT(fromObj,ct), complete, repeat, delay, pingPong );
		}

		public static async UniTask AlphaTo(this Renderer fromObj, float to, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			await Tween( fromObj.material.color.a, to, duration, (newPos)=>{ var col = fromObj.material.color; col.a = newPos; fromObj.material.color = col; },
												type, getCT(fromObj,ct), complete, repeat, delay, pingPong );
		}

		public static async UniTask AlphaTo(this Renderer fromObj, float from, float to, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			var col = fromObj.material.color;
			col.a = from;
			fromObj.material.color = col;
			await Tween( from, to, duration, (newPos)=>{ var col = fromObj.material.color; col.a = newPos; fromObj.material.color = col; },
												type, getCT(fromObj,ct), complete, repeat, delay, pingPong );
		}

		public static async UniTask AlphaTo(this Graphic fromObj, float to, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{

			await Tween( fromObj.color.a, to, duration, (newPos)=>{ var col = fromObj.color; col.a = newPos; fromObj.color = col; },
												type, getCT(fromObj,ct), complete, repeat, delay, pingPong );
		}
		public static async UniTask AlphaTo(this Graphic fromObj,float from, float to, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			var col = fromObj.color;
			col.a = from;
			fromObj.color = col;
			await Tween( fromObj.color.a, to, duration, (newPos)=>{ var col = fromObj.color; col.a = newPos; fromObj.color = col; },
												type, getCT(fromObj,ct), complete, repeat, delay, pingPong );
		}

		public static async UniTask AlphaTo(this Material fromObj, float to, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			await Tween( fromObj.color.a, to, duration, (newPos)=>{ var col = fromObj.color; col.a = newPos; fromObj.color = col; },
												type, getCT(fromObj,ct), complete, repeat, delay, pingPong );
		}
		public static async UniTask AlphaTo(this Material fromObj, float from,float to, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			var col = fromObj.color;
			col.a = from;
			fromObj.color = col;
			await Tween( fromObj.color.a, to, duration, (newPos)=>{ var col = fromObj.color; col.a = newPos; fromObj.color = col; },
												type, getCT(fromObj,ct), complete, repeat, delay, pingPong );
		}


		public static async UniTask ColorTo(this Renderer fromObj, Vector3 to, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{

			await Tween( fromObj.material.color, to, duration,
								(newPos)=>{ fromObj.material.color = new Color(newPos.x,newPos.y,newPos.z,fromObj.material.color.a);  },
								type, getCT(fromObj,ct), complete, repeat, delay, pingPong );
		}

		public static async UniTask ColorTo(this Graphic fromObj, Vector3 to, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			await Tween( fromObj.color, to, duration,
								(newPos)=>{ fromObj.color = new Color(newPos.x,newPos.y,newPos.z,fromObj.color.a);  },
								type, getCT(fromObj,ct), complete, repeat, delay, pingPong );
		}

		public static async UniTask ColorTo(this SpriteRenderer fromObj, Color to, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{

			await Tween( fromObj.color, to, duration,
								(newPos)=>{ fromObj.color = newPos; },
								type, getCT(fromObj,ct), complete, repeat, delay, pingPong );
		}

		public static async UniTask ColorTo(this Renderer fromObj, Color to, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{

			await Tween( fromObj.material.color, to, duration,
								(newPos)=>{ fromObj.material.color = newPos; },
								type, getCT(fromObj,ct), complete, repeat, delay, pingPong );
		}

		public static async UniTask ColorTo(this Graphic fromObj, Color to, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			await Tween( fromObj.color, to, duration,
								(newPos)=>{ fromObj.color = newPos; },
								type, getCT(fromObj,ct), complete, repeat, delay, pingPong );
		}

		public static async UniTask ColorTo(this Material fromObj, Color to, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			await Tween( fromObj.color, to, duration,
								(newPos)=>{ fromObj.color = newPos; },
								type, getCT(fromObj,ct), complete, repeat, delay, pingPong );
		}
		public static async UniTask ColorTo(this Material fromObj, Color from,Color to, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			fromObj.color = from;
			await Tween( fromObj.color, to, duration,
								(newPos)=>{ fromObj.color = newPos; },
								type, getCT(fromObj,ct), complete, repeat, delay, pingPong );
		}

		public static async UniTask AlphaTo(this CanvasGroup fromObj, float to, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{

			await Tween( fromObj.alpha, to, duration, (newPos)=>{ var col = fromObj.alpha = newPos; },
												type, getCT(fromObj,ct), complete, repeat, delay, pingPong );
		}

		public static async UniTask AlphaTo(this CanvasGroup fromObj,float from, float to, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			fromObj.alpha = from;
			await Tween( fromObj.alpha, to, duration, (newPos)=>{ var col = fromObj.alpha = newPos; },
												type, getCT(fromObj,ct), complete, repeat, delay, pingPong );
		}

		public static async UniTask TextSend(this TMP_Text fromObj, int fromIndex, int toIndex, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			var len = fromObj.text.Length; // @todo:本当はGetParsedTextを使いたいが1フレ待つ必要があるためLengthにする.
			fromIndex = Mathf.Clamp(fromIndex,0,len);
			toIndex = Mathf.Clamp(toIndex+1,0,len+1); // 最後の1文字がintだと頭しか表示されないので+1する.
			await Tween( (float)fromIndex, (float)toIndex, duration,
								(newPos)=>{ fromObj.maxVisibleCharacters = (int)Mathf.Clamp(newPos,0,len); },
								type, getCT(fromObj,ct), complete, repeat, delay, pingPong );
		}


		// ジェネリクス版
#region UseGenericsEasing
		public static async UniTask XTo<T>(this Transform transform, float to,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T: IEaseTypeHint,new()=>
					await transform.XTo( to,  duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong);
		public static async UniTask YTo<T>(this Transform transform, float to,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T: IEaseTypeHint,new()=>
					await transform.YTo( to,  duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong);
		public static async UniTask ZTo<T>(this Transform transform, float to,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T: IEaseTypeHint,new()=>
					await transform.ZTo( to,  duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong);
		public static async UniTask XOffset<T>(this Transform transform, float offset,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T: IEaseTypeHint,new()=>
					await transform.XOffset( offset,  duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong);
		public static async UniTask YOffset<T>(this Transform transform, float offset,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T: IEaseTypeHint,new()=>
					await transform.YOffset( offset,  duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong);
		public static async UniTask ZOffset<T>(this Transform transform, float offset,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T: IEaseTypeHint,new()=>
					await transform.ZOffset( offset,  duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong);

		public static async UniTask LocalXTo<T>(this Transform transform, float to,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T: IEaseTypeHint,new()=>
					await transform.LocalXTo( to,  duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong);
		public static async UniTask LocalYTo<T>(this Transform transform, float to,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T: IEaseTypeHint,new()=>
					await transform.LocalYTo( to,  duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong);
		public static async UniTask LocalZTo<T>(this Transform transform, float to,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T: IEaseTypeHint,new()=>
					await transform.LocalZTo( to,  duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong);
		public static async UniTask LocalXOffset<T>(this Transform transform, float offset,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T: IEaseTypeHint,new()=>
					await transform.LocalXOffset( offset,  duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong);
		public static async UniTask LocalYOffset<T>(this Transform transform, float offset,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T: IEaseTypeHint,new()=>
					await transform.LocalYOffset( offset,  duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong);
		public static async UniTask LocalZOffset<T>(this Transform transform, float offset,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T: IEaseTypeHint,new()=>
					await transform.LocalZOffset( offset,  duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong);

		public static async UniTask XYTo<T>(this Transform transform, Vector2 to, float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T: IEaseTypeHint,new()=>
					await transform.XYTo( to, duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong);
		public static async UniTask XYOffset<T>(this Transform transform, Vector2 offset, float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T: IEaseTypeHint,new()=>
					await transform.XYOffset( offset, duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong);

		public static async UniTask LocalXYTo<T>(this Transform transform, Vector2 to, float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T: IEaseTypeHint,new()=>
					await transform.LocalXYTo( to, duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong);
		public static async UniTask LocalXYOffset<T>(this Transform transform, Vector2 offset, float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T: IEaseTypeHint,new()=>
					await transform.LocalXYOffset( offset, duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong);
		public static async UniTask XYZTo<T>(this Transform transform, Vector3 to, float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T: IEaseTypeHint,new()=>
					await transform.XYZTo( to, duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong);
		public static async UniTask XYZOffset<T>(this Transform transform, Vector3 offset, float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T: IEaseTypeHint,new()=>
					await transform.XYZOffset( offset, duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong);
		public static async UniTask LocalXYZTo<T>(this Transform transform, Vector3 to, float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T: IEaseTypeHint,new()=>
					await transform.LocalXYZTo( to, duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong);

		public static async UniTask LocalXYZOffset<T>(this Transform transform, Vector3 offset, float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T: IEaseTypeHint,new()=>
					await transform.LocalXYZOffset( offset, duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong);

		public static async UniTask LocalRotXTo<T>(this Transform transform, float toAngle,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T: IEaseTypeHint,new()=>
					await transform.LocalRotXTo( toAngle, duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong);
		public static async UniTask LocalRotYTo<T>(this Transform transform, float toAngle,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T: IEaseTypeHint,new()=>
					await transform.LocalRotYTo( toAngle, duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong);
		public static async UniTask LocalRotZTo<T>(this Transform transform, float toAngle,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T: IEaseTypeHint,new()=>
					await transform.LocalRotZTo( toAngle, duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong);

		public static async UniTask RotXTo<T>(this Transform transform, float toAngle,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T: IEaseTypeHint,new()=>
					await transform.RotXTo( toAngle, duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong);
		public static async UniTask RotYTo<T>(this Transform transform, float toAngle,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T: IEaseTypeHint,new()=>
					await transform.RotYTo( toAngle, duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong);
		public static async UniTask RotZTo<T>(this Transform transform, float toAngle,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T: IEaseTypeHint,new()=>
					await transform.RotZTo( toAngle, duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong);

		public static async UniTask ScaleXTo<T>(this Transform transform, float to,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T: IEaseTypeHint,new()=>
					await transform.ScaleXTo( to, duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong);
		public static async UniTask ScaleYTo<T>(this Transform transform, float to,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T: IEaseTypeHint,new()=>
					await transform.ScaleYTo( to, duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong);
		public static async UniTask ScaleZTo<T>(this Transform transform, float to,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T: IEaseTypeHint,new()=>
					await transform.ScaleZTo( to, duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong);

		public static async UniTask ScaleXYTo<T>(this Transform transform, Vector2 to, float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T: IEaseTypeHint,new()=>
					await transform.ScaleXYTo( to, duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong);
		public static async UniTask ScaleXYZTo<T>(this Transform transform, Vector3 to, float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T: IEaseTypeHint,new()=>
					await transform.ScaleXYZTo( to, duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong);


		// GameObject
		public static async UniTask XTo<T>(this GameObject fromObj, float to,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T: IEaseTypeHint,new()
												=> await fromObj.transform.XTo( to, duration,new T().FuncType(), ct, complete, repeat, delay, pingPong);
		public static async UniTask YTo<T>(this GameObject fromObj, float to,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T: IEaseTypeHint,new()
												=> await fromObj.transform.YTo( to, duration,new T().FuncType(), ct, complete, repeat, delay, pingPong);
		public static async UniTask ZTo<T>(this GameObject fromObj, float to,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T: IEaseTypeHint,new()
												=> await fromObj.transform.ZTo( to, duration,new T().FuncType(), ct, complete, repeat, delay, pingPong);
		public static async UniTask XOffset<T>(this GameObject fromObj, float offset,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T: IEaseTypeHint,new()
												=> await fromObj.transform.XOffset( offset, duration,new T().FuncType(), ct, complete, repeat, delay, pingPong);
		public static async UniTask YOffset<T>(this GameObject fromObj, float offset,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T: IEaseTypeHint,new()
												=> await fromObj.transform.YOffset( offset, duration,new T().FuncType(), ct, complete, repeat, delay, pingPong);
		public static async UniTask ZOffset<T>(this GameObject fromObj, float offset,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T: IEaseTypeHint,new()
												=> await fromObj.transform.ZOffset( offset, duration,new T().FuncType(), ct, complete, repeat, delay, pingPong);
		public static async UniTask LocalXTo<T>(this GameObject fromObj, float to,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T: IEaseTypeHint,new()
												=> await fromObj.transform.LocalXTo( to, duration,new T().FuncType(), ct, complete, repeat, delay, pingPong);
		public static async UniTask LocalYTo<T>(this GameObject fromObj, float to,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T: IEaseTypeHint,new()
												=> await fromObj.transform.LocalYTo( to, duration,new T().FuncType(), ct, complete, repeat, delay, pingPong);
		public static async UniTask LocalZTo<T>(this GameObject fromObj, float to,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T: IEaseTypeHint,new()
												=> await fromObj.transform.LocalZTo( to, duration,new T().FuncType(), ct, complete, repeat, delay, pingPong);
		public static async UniTask LocalXOffset<T>(this GameObject fromObj, float offset,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T: IEaseTypeHint,new()
												=> await fromObj.transform.LocalXOffset( offset, duration,new T().FuncType(), ct, complete, repeat, delay, pingPong);
		public static async UniTask LocalYOffset<T>(this GameObject fromObj, float offset,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T: IEaseTypeHint,new()
												=> await fromObj.transform.LocalYOffset( offset, duration,new T().FuncType(), ct, complete, repeat, delay, pingPong);
		public static async UniTask LocalZOffset<T>(this GameObject fromObj, float offset,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T: IEaseTypeHint,new()
												=> await fromObj.transform.LocalZOffset( offset, duration,new T().FuncType(), ct, complete, repeat, delay, pingPong);
		public static async UniTask XYTo<T>(this GameObject fromObj, Vector2 to, float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T: IEaseTypeHint,new()
												=> await fromObj.transform.XYTo( to, duration,new T().FuncType(), ct, complete, repeat, delay, pingPong);
		public static async UniTask XYOffset<T>(this GameObject fromObj, Vector2 offset, float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T: IEaseTypeHint,new()
												=> await fromObj.transform.XYOffset( offset, duration,new T().FuncType(), ct, complete, repeat, delay, pingPong);
		public static async UniTask LocalXYTo<T>(this GameObject fromObj, Vector2 to, float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T: IEaseTypeHint,new()
												=> await fromObj.transform.LocalXYTo( to, duration,new T().FuncType(), ct, complete, repeat, delay, pingPong);
		public static async UniTask LocalXYOffset<T>(this GameObject fromObj, Vector2 offset, float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T: IEaseTypeHint,new()
												=> await fromObj.transform.LocalXYOffset( offset, duration,new T().FuncType(), ct, complete, repeat, delay, pingPong);
		public static async UniTask XYZTo<T>(this GameObject fromObj, Vector3 to, float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T: IEaseTypeHint,new()
												=> await fromObj.transform.XYZTo( to, duration,new T().FuncType(), ct, complete, repeat, delay, pingPong);
		public static async UniTask XYZOffset<T>(this GameObject fromObj, Vector3 offset, float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T: IEaseTypeHint,new()
												=> await fromObj.transform.XYZOffset( offset, duration,new T().FuncType(), ct, complete, repeat, delay, pingPong);
		public static async UniTask LocalXYZTo<T>(this GameObject fromObj, Vector3 to, float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T: IEaseTypeHint,new()
												=> await fromObj.transform.LocalXYZTo( to, duration,new T().FuncType(), ct, complete, repeat, delay, pingPong);
		public static async UniTask LocalXYZOffset<T>(this GameObject fromObj, Vector3 offset, float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T: IEaseTypeHint,new()
												=> await fromObj.transform.LocalXYZOffset( offset, duration,new T().FuncType(), ct, complete, repeat, delay, pingPong);

		public static async UniTask RotXTo<T>(this GameObject fromObj, float toAngle,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T: IEaseTypeHint,new()
												=> await fromObj.transform.RotXTo( toAngle, duration,new T().FuncType(), ct, complete, repeat, delay, pingPong);
		public static async UniTask RotYTo<T>(this GameObject fromObj, float toAngle,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T: IEaseTypeHint,new()
												=> await fromObj.transform.RotYTo( toAngle, duration,new T().FuncType(), ct, complete, repeat, delay, pingPong);
		public static async UniTask RotZTo<T>(this GameObject fromObj, float toAngle,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T: IEaseTypeHint,new()
												=> await fromObj.transform.RotZTo( toAngle, duration,new T().FuncType(), ct, complete, repeat, delay, pingPong);

		public static async UniTask LocalRotXTo<T>(this GameObject fromObj, float toAngle,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T: IEaseTypeHint,new()
												=> await fromObj.transform.LocalRotXTo( toAngle, duration,new T().FuncType(), ct, complete, repeat, delay, pingPong);
		public static async UniTask LocalRotYTo<T>(this GameObject fromObj, float toAngle,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T: IEaseTypeHint,new()
												=> await fromObj.transform.LocalRotYTo( toAngle, duration,new T().FuncType(), ct, complete, repeat, delay, pingPong);
		public static async UniTask LocalRotZTo<T>(this GameObject fromObj, float toAngle,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T: IEaseTypeHint,new()
												=> await fromObj.transform.LocalRotZTo( toAngle, duration,new T().FuncType(), ct, complete, repeat, delay, pingPong);

		public static async UniTask ScaleXTo<T>(this GameObject fromObj, float to,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T: IEaseTypeHint,new()
												=> await fromObj.transform.ScaleXTo( to, duration,new T().FuncType(), ct, complete, repeat, delay, pingPong);
		public static async UniTask ScaleYTo<T>(this GameObject fromObj, float to,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T: IEaseTypeHint,new()
												=> await fromObj.transform.ScaleYTo( to, duration,new T().FuncType(), ct, complete, repeat, delay, pingPong);
		public static async UniTask ScaleZTo<T>(this GameObject fromObj, float to,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T: IEaseTypeHint,new()
												=> await fromObj.transform.ScaleZTo( to, duration,new T().FuncType(), ct, complete, repeat, delay, pingPong);
		public static async UniTask ScaleXYTo<T>(this GameObject fromObj, Vector2 to, float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T: IEaseTypeHint,new()
												=> await fromObj.transform.ScaleXYTo( to, duration,new T().FuncType(), ct, complete, repeat, delay, pingPong);
		public static async UniTask ScaleXYZTo<T>(this GameObject fromObj, Vector3 to, float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T: IEaseTypeHint,new()
												=> await fromObj.transform.ScaleXYZTo( to, duration,new T().FuncType(), ct, complete, repeat, delay, pingPong);

		// SpriteRenderer
		public static async UniTask SizeTo<T>(this SpriteRenderer fromObj, Vector2 to, float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T: IEaseTypeHint,new()=>
					await fromObj.SizeTo( to, duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong);

		public static async UniTask AlphaTo<T>(this Renderer fromObj, float to, float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T: IEaseTypeHint,new()=>
					await fromObj.AlphaTo( to, duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong);

		public static async UniTask AlphaTo<T>(this Renderer fromObj, float from, float to, float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T: IEaseTypeHint,new()=>
					await fromObj.AlphaTo( from, to, duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong);

		public static async UniTask AlphaTo<T>(this Graphic fromObj, float to, float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T: IEaseTypeHint,new()=>
					await fromObj.AlphaTo( to, duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong);
		public static async UniTask AlphaTo<T>(this Graphic fromObj,float from, float to, float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T: IEaseTypeHint,new()=>
					await fromObj.AlphaTo( from,to, duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong);
		public static async UniTask AlphaTo<T>(this Material fromObj, float to, float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T: IEaseTypeHint,new()=>
					await fromObj.AlphaTo( to, duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong);
		public static async UniTask AlphaTo<T>(this Material fromObj, float from,float to, float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T: IEaseTypeHint,new()=>
					await fromObj.AlphaTo( from, to, duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong);

		public static async UniTask ColorTo<T>(this Renderer fromObj, Vector3 to, float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T: IEaseTypeHint,new()=>
					await fromObj.ColorTo( to, duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong);
		public static async UniTask ColorTo<T>(this Graphic fromObj, Vector3 to, float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T: IEaseTypeHint,new()=>
					await fromObj.ColorTo( to, duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong);
		public static async UniTask ColorTo<T>(this SpriteRenderer fromObj, Color to, float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T: IEaseTypeHint,new()=>
					await fromObj.ColorTo( to, duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong);

		public static async UniTask ColorTo<T>(this Renderer fromObj, Color to, float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T: IEaseTypeHint,new()=>
					await fromObj.ColorTo( to, duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong);
		public static async UniTask ColorTo<T>(this Graphic fromObj, Color to, float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T: IEaseTypeHint,new()=>
					await fromObj.ColorTo( to, duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong);
		public static async UniTask ColorTo<T>(this Material fromObj, Color to, float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T: IEaseTypeHint,new()=>
					await fromObj.ColorTo( to, duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong);
		public static async UniTask ColorTo<T>(this Material fromObj, Color from,Color to, float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T: IEaseTypeHint,new()=>
					await fromObj.ColorTo( from, to, duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong);

		public static async UniTask AlphaTo<T>(this CanvasGroup fromObj, float to, float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T: IEaseTypeHint,new()=>
					await fromObj.AlphaTo( to, duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong);

		public static async UniTask AlphaTo<T>(this CanvasGroup fromObj,float from, float to, float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T: IEaseTypeHint,new()=>
					await fromObj.AlphaTo( from,to, duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong);

		public static async UniTask TextSend<T>(this TMP_Text fromObj, int fromIndex, int toIndex, float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T: IEaseTypeHint,new()=>
					await fromObj.TextSend( fromIndex, toIndex, duration,new T().FuncType(), ct, complete, repeat, delay, pingPong);
#endregion UseGenericsEasing

		// 汎用Tween
		public static async UniTask Tween(float from, float to, float duration, System.Action<float> update, EaseType type=EaseType.Default, CancellationToken ct=default,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			if ( type == EaseType.Default ) type = DefaultEasing;
			var func = Types[type];
			var updater = new FloatUpdater(from,to,func,update);
			await tweenBase( updater, duration, ct, complete, repeat, delay, pingPong );
		}

		public static async UniTask Tween(Vector2 from, Vector2 to, float duration, System.Action<Vector2> update, EaseType type=EaseType.Default, CancellationToken ct=default,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			if ( type == EaseType.Default ) type = DefaultEasing;
			var func = Types[type];
			var updater = new Vector2Updater(from,to,func,update);
			await tweenBase( updater, duration, ct, complete, repeat, delay, pingPong );
		}
		public static async UniTask Tween(Vector3 from, Vector3 to, float duration, System.Action<Vector3> update, EaseType type=EaseType.Default, CancellationToken ct=default,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			if ( type == EaseType.Default ) type = DefaultEasing;
			var func = Types[type];
			var updater = new Vector3Updater(from,to,func,update);
			await tweenBase( updater, duration, ct, complete, repeat, delay, pingPong );
		}

		public static async UniTask Tween(Vector4 from, Vector4 to, float duration, System.Action<Vector4> update, EaseType type=EaseType.Default, CancellationToken ct=default,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			if ( type == EaseType.Default ) type = DefaultEasing;
			var func = Types[type];
			var updater = new Vector4Updater(from,to,func,update);
			await tweenBase( updater, duration, ct, complete, repeat, delay, pingPong );
		}


		// 汎用ジェネリクス版
		public static async UniTask Tween<T>(float from, float to, float duration, System.Action<float> update, CancellationToken ct=default,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T : IEaseTypeHint,new()
		{
			var types = new T();
			var func = Types[types.FuncType()];
			var updater = new FloatUpdater(from,to,func,update);
			await tweenBase( updater, duration, ct, complete, repeat, delay, pingPong );
		}

		public static async UniTask Tween<T>(Vector2 from, Vector2 to, float duration, System.Action<Vector2> update, CancellationToken ct=default,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T : IEaseTypeHint,new()
		{
			var types = new T();
			var func = Types[types.FuncType()];
			var updater = new Vector2Updater(from,to,func,update);
			await tweenBase( updater, duration, ct, complete, repeat, delay, pingPong );
		}
		public static async UniTask Tween<T>(Vector3 from, Vector3 to, float duration, System.Action<Vector3> update, CancellationToken ct=default,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T : IEaseTypeHint,new()
		{
			var types = new T();
			var func = Types[types.FuncType()];
			var updater = new Vector3Updater(from,to,func,update);
			await tweenBase( updater, duration, ct, complete, repeat, delay, pingPong );
		}

		public static async UniTask Tween<T>(Vector4 from, Vector4 to, float duration, System.Action<Vector4> update, CancellationToken ct=default,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false) where T : IEaseTypeHint,new()
		{
			var types = new T();
			var func = Types[types.FuncType()];
			var updater = new Vector4Updater(from,to,func,update);
			await tweenBase( updater, duration, ct, complete, repeat, delay, pingPong );
		}


		// 補間アルゴリズム.
		private const float HalfPi = Mathf.PI * .5f;
		private const float DoublePi = Mathf.PI * 2f;

		class Interporate
		{
			public static EaseType Type = DefaultEasing;
			public virtual float Time(float time){return time;}
			public virtual float Op(float from,float to, float time){ return Mathf.Lerp(from, to, Time(time)); }
			public virtual Vector2 Op(Vector2 from,Vector2 to, float time){ return Vector2.Lerp(from,to,Time(time));}
			public virtual Vector3 Op(Vector3 from,Vector3 to, float time){ return Vector3.Lerp(from,to,Time(time));}
			public virtual Vector4 Op(Vector4 from,Vector4 to, float time){ return Vector4.Lerp(from,to,Time(time));}
		}

		// Lerp
		class CalcLerp : Interporate {
			public static new EaseType Type = EaseType.Linear;
			public override float Time(float time) => time;
		}

		// SineIn
		class CalcSineIn : Interporate{
			public static new EaseType Type = EaseType.SineIn;
			public override float Time(float time) => 1f - Mathf.Cos(time * HalfPi); 
		}

		// SineOut
		class CalcSineOut : Interporate{
			public static new EaseType Type = EaseType.SineOut;
			public override float Time(float time) => Mathf.Sin(time * HalfPi);
		}

		// SineInOut
		class CalcSineInOut : Interporate{
			public static new EaseType Type = EaseType.SineInOut;
			public override float Time(float time) => .5f * (1f - Mathf.Cos(Mathf.PI * time));
		}

		// QuadIn
		class CalcQuadIn : Interporate{
			public static new EaseType Type = EaseType.QuadIn;
			public override float Time(float time) => time * time;
		}

		// QuadOut
		class CalcQuadOut : Interporate{
			public static new EaseType Type = EaseType.QuadOut;
			public override float Time(float time) => -time * (time - 2f);
		}

		// QuadInOut
		class CalcQuadInOut : Interporate{
			public static new EaseType Type = EaseType.QuadInOut;
			public override float Time(float time){
				if ((time /= .5f) < 1f) return .5f * time * time;
				return -.5f * (((--time) * (time - 2f) - 1f));
			}
		}

		// CubicIn
		class CalcCubicIn : Interporate{
			public static new EaseType Type = EaseType.CubicIn;
			public override float Time(float time) => time * time * time;
		}

		// CubicOut
		class CalcCubicOut : Interporate{
			public static new EaseType Type = EaseType.CubicOut;
			public override float Time(float time) => (time -= 1f) * time * time + 1f;
		}

		// CubicInOut
		class CalcCubicInOut : Interporate{
			public static new EaseType Type = EaseType.CubicInOut;
			public override float Time(float time){
				if ((time /= .5f) < 1f) return .5f * time * time * time;
				return .5f * ((time -= 2) * time * time + 2f);
			}
		}
		// QuadIn
		class CalcQuartIn : Interporate{
			public static new EaseType Type = EaseType.QuartIn;
			public override float Time(float time) => time * time * time * time;
		}

		// QuartOut
		class CalcQuartOut : Interporate{
			public static new EaseType Type = EaseType.QuadOut;
			public override float Time(float time) => -((time -= 1f) * time * time * time - 1f);
		}

		// QuartInOut
		class CalcQuartInOut : Interporate{
			public static new EaseType Type = EaseType.QuartInOut;
			public override float Time(float time){
				if ((time /= .5f) < 1f) return .5f * time * time * time * time;
				return -.5f * ((time -= 2f) * time * time * time - 2f);
			}
		}

		// QuintIn
		class CalcQuintIn : Interporate{
			public static new EaseType Type = EaseType.QuintIn;
			public override float Time(float time) => time * time * time * time * time;
		}

		// QuintOut
		class CalcQuintOut : Interporate{
			public static new EaseType Type = EaseType.QuintOut;
			public override float Time(float time) => (time -= 1f) * time * time * time * time + 1f;
		}

		// QuintInOut
		class CalcQuintInOut : Interporate{
			public static new EaseType Type = EaseType.QuintInOut;
			public override float Time(float time){
				if ((time /= .5f) < 1f) return .5f * time * time * time * time * time;
				return .5f * ((time -= 2f) * time * time * time * time + 2f);
			}
		}

		// ExpoIn
		class CalcExpoIn : Interporate{
			public static new EaseType Type = EaseType.ExpoIn;
			public override float Time(float time) => Mathf.Pow(2f, 10f * (time - 1f));
		}

		// ExpoIn
		class CalcExpoOut : Interporate{
			public static new EaseType Type = EaseType.ExpoOut;
			public override float Time(float time) => -Mathf.Pow(2f, -10f * time) + 1f;
		}

		// ExpoInOut
		class CalcExpoInOut : Interporate{
			public static new EaseType Type = EaseType.ExpoInOut;
			public override float Time(float time){
				if ((time /= .5f) < 1f) return .5f * Mathf.Pow(2f, 10f * (time - 1f));
				return .5f * (-Mathf.Pow(2f, -10f * --time) + 2f);
			}
		}

		// CircIn
		class CalcCircIn : Interporate{
			public static new EaseType Type = EaseType.CircIn;
			public override float Time(float time) => -(Mathf.Sqrt(1f - time * time) - 1f);
		}

		// CircOut
		class CalcCircOut : Interporate{
			public static new EaseType Type = EaseType.CircOut;
			public override float Time(float time) => Mathf.Sqrt(1f - (time -= 1f) * time);
		}

		// CircInOut
		class CalcCircInOut : Interporate{
			public static new EaseType Type = EaseType.CircInOut;
			public override float Time(float time){
				if ((time /= .5f) < 1f) return -.5f * (Mathf.Sqrt(1f - time * time) - 1f);
				return .5f * (Mathf.Sqrt(1f - (time -= 2f) * time) + 1f);
			}
		}

		// BackIn
		class CalcBackIn : Interporate{
			public static new EaseType Type = EaseType.BackIn;
			const float s = 1.70158f;
			public override float Time(float time) => time * time * ((s + 1f) * time - s);
			public override float Op(float from,float to, float time){ to -= from; return to * Time(time) + from; }
			public override Vector2 Op(Vector2 from,Vector2 to, float time){ to -= from; return to*Time(time) + from;}
			public override Vector3 Op(Vector3 from,Vector3 to, float time){ to -= from; return to*Time(time) + from;}
			public override Vector4 Op(Vector4 from,Vector4 to, float time){ to -= from; return to*Time(time) + from;}
		}

		// BackOut
		class CalcBackOut : Interporate{
			public static new EaseType Type = EaseType.BackOut;
			const float s = 1.70158f;
			public override float Time(float time) => --time * time * ((s + 1f) * time + s) + 1f;
			public override float Op(float from,float to, float time){ to -= from; return to * Time(time) + from; }
			public override Vector2 Op(Vector2 from,Vector2 to, float time){ to -= from; return to*Time(time) + from;}
			public override Vector3 Op(Vector3 from,Vector3 to, float time){ to -= from; return to*Time(time) + from;}
			public override Vector4 Op(Vector4 from,Vector4 to, float time){ to -= from; return to*Time(time) + from;}
		}

		// BackInOut
		class CalcBackInOut : Interporate{
			public static new EaseType Type = EaseType.BackInOut;
			const float s = 1.70158f * 1.525f;
			public override float Time(float time){
				if ((time /= .5f) < 1f)
					return .5f * (time * time * ((s + 1f) * time - s));
				return .5f * ((time -= 2) * time * ((s + 1f) * time  + s) + 2f);
			}
			public override float Op(float from,float to, float time){ to -= from; return to * Time(time) + from; }
			public override Vector2 Op(Vector2 from,Vector2 to, float time){ to -= from; return to*Time(time) + from;}
			public override Vector3 Op(Vector3 from,Vector3 to, float time){ to -= from; return to*Time(time) + from;}
			public override Vector4 Op(Vector4 from,Vector4 to, float time){ to -= from; return to*Time(time) + from;}
		}

		// ElasticIn
		class CalcElasticIn : Interporate{
			public static new EaseType Type = EaseType.ElasticIn;
			const float p = .3f;
			const float s = p / 4f;
			public override float Time(float time) => -(Mathf.Pow(2f, 10f * (time -= 1f)) * Mathf.Sin((time - s) * DoublePi / p));
			public override float Op(float from,float to, float time){ to -= from; return to * Time(time) + from; }
			public override Vector2 Op(Vector2 from,Vector2 to, float time){ to -= from; return to*Time(time) + from;}
			public override Vector3 Op(Vector3 from,Vector3 to, float time){ to -= from; return to*Time(time) + from;}
			public override Vector4 Op(Vector4 from,Vector4 to, float time){ to -= from; return to*Time(time) + from;}
		}

		// ElasticOut
		class CalcElasticOut : Interporate{
			public static new EaseType Type = EaseType.ElasticOut;
			const float p = .3f;
			const float s = p / 4f;
			public override float Time(float time) => Mathf.Pow(2f, -10f * time) * Mathf.Sin((time - s) * DoublePi / p);
			public override float Op(float from,float to, float time){ to -= from; return to * Time(time) + to + from; }
			public override Vector2 Op(Vector2 from,Vector2 to, float time){ to -= from; return to*Time(time) + to + from;}
			public override Vector3 Op(Vector3 from,Vector3 to, float time){ to -= from; return to*Time(time) + to + from;}
			public override Vector4 Op(Vector4 from,Vector4 to, float time){ to -= from; return to*Time(time) + to + from;}
		}

		// ElasticInOut
		class CalcElasticInOut : Interporate{
			public static new EaseType Type = EaseType.ElasticInOut;
			const float p = .3f * 1.5f;
			const float s = p / 4f;
			public override float Time(float time){
				if ((time /= .5f) < 1f)
					return -.5f * Mathf.Pow(2f, 10f * (time -= 1f)) * Mathf.Sin((time - s) * DoublePi / p);
				return (Mathf.Pow(2f, -10f * (time -= 1f)) * Mathf.Sin((time - s) * DoublePi / p)) * 1.5f;
			}
			public override float Op(float from,float to, float time){ to -= from; return to * Time(time) + from; }
			public override Vector2 Op(Vector2 from,Vector2 to, float time){ to -= from; return to*Time(time) + from;}
			public override Vector3 Op(Vector3 from,Vector3 to, float time){ to -= from; return to*Time(time) + from;}
			public override Vector4 Op(Vector4 from,Vector4 to, float time){ to -= from; return to*Time(time) + from;}
		}

		// BounceIn
		class CalcBounceIn : Interporate{
			public static new EaseType Type = EaseType.BounceIn;
			static CalcBounceOut bout = new CalcBounceOut();
			public override float Time(float time) => 1f - time;
			public override float Op(float from,float to, float time){ to -= from; return to - bout.Op(0f, to, Time(time)) + from; }
			public override Vector2 Op(Vector2 from,Vector2 to, float time){ to -= from; return to - bout.Op(Vector2.zero, to, Time(time)) + from; }
			public override Vector3 Op(Vector3 from,Vector3 to, float time){ to -= from; return to - bout.Op(Vector3.zero, to, Time(time)) + from; }
			public override Vector4 Op(Vector4 from,Vector4 to, float time){ to -= from; return to - bout.Op(Vector4.zero, to, Time(time)) + from; }
		}

		// BounceOut
		class CalcBounceOut : Interporate{
			public static new EaseType Type = EaseType.BounceOut;
			public override float Time(float time){
				if (time < (1f / 2.75f))
					return (7.5625f * time * time);
				if (time < (2f / 2.75f))
					return (7.5625f * (time -= (1.5f / 2.75f)) * time + .75f);
				if (time < (2.5f / 2.75f))
					return (7.5625f * (time -= (2.25f / 2.75f)) * time + .9375f);
				return (7.5625f * (time -= (2.625f / 2.75f)) * time + .984375f);
			}
			public override float Op(float from,float to, float time){ to -= from; return to * Time(time) + from; }
			public override Vector2 Op(Vector2 from,Vector2 to, float time){ to -= from; return to*Time(time) + from;}
			public override Vector3 Op(Vector3 from,Vector3 to, float time){ to -= from; return to*Time(time) + from;}
			public override Vector4 Op(Vector4 from,Vector4 to, float time){ to -= from; return to*Time(time) + from;}
		}

		// BounceInOut
		class CalcBounceInOut : Interporate{
			public static new EaseType Type = EaseType.BounceInOut;
			static CalcBounceIn bin = new CalcBounceIn();
			static CalcBounceOut bout = new CalcBounceOut();
			public override float Time(float time) => time < .5f?(time * 2f):(time*2f-1f);
			public override float Op(float from,float to, float time){ to -= from; return time < .5f?(bin.Op(0f, to, Time(time))*.5f+from):(bout.Op(0f, to, Time(time)) * .5f + to * .5f + from); }
			public override Vector2 Op(Vector2 from,Vector2 to, float time){ to -= from; return time < .5f?(bin.Op(Vector2.zero, to, Time(time))*.5f+from):(bout.Op(Vector2.zero, to, Time(time)) * .5f + to * .5f + from); }
			public override Vector3 Op(Vector3 from,Vector3 to, float time){ to -= from; return time < .5f?(bin.Op(Vector3.zero, to, Time(time))*.5f+from):(bout.Op(Vector3.zero, to, Time(time)) * .5f + to * .5f + from); }
			public override Vector4 Op(Vector4 from,Vector4 to, float time){ to -= from; return time < .5f?(bin.Op(Vector4.zero, to, Time(time))*.5f+from):(bout.Op(Vector4.zero, to, Time(time)) * .5f + to * .5f + from); }
		}

		// Spring
		class CalcSpring : Interporate{
			public static new EaseType Type = EaseType.Spring;
			public override float Time(float time) {
				time = Mathf.Clamp01(time);
				time = (Mathf.Sin(time * Mathf.PI * (.2f + 2.5f * time * time * time)) * Mathf.Pow(1f - time, 2.2f) + time) * (1f + (1.2f * (1f - time)));
				return time;
			}
			public override float Op(float from,float to, float time){ return from + (to - from) * Time(time); }
			public override Vector2 Op(Vector2 from,Vector2 to, float time){ return from + (to - from) * Time(time); }
			public override Vector3 Op(Vector3 from,Vector3 to, float time){ return from + (to - from) * Time(time); }
			public override Vector4 Op(Vector4 from,Vector4 to, float time){ return from + (to - from) * Time(time); }
		}
	}
}
