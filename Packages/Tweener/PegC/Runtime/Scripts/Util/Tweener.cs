using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System.Threading;
using TMPro;
using PegC.Util.ValueSetter;
using System;

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
		Custom,
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
	public struct Custom:IEaseTypeHint{public EaseType FuncType()=>EaseType.Custom;}
	public struct Default:IEaseTypeHint{public EaseType FuncType()=>Tweener.DefaultEasing;}

	public static class Tweener
	{
		public static EaseType DefaultEasing { get; set;} = EaseType.Linear;

		readonly static Dictionary<EaseType, Interporate> Types = new Dictionary<EaseType, Interporate>
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

		public readonly static Dictionary<EaseType, System.Func<float,float,float,float> > EaseFloat = new Dictionary<EaseType, Func<float,float,float,float> >
		{
			{EaseType.Linear, EaseLinear},
			{EaseType.SineIn, EaseSineIn}, {EaseType.SineOut, EaseSineOut}, {EaseType.SineInOut, EaseSineInOut},
			{EaseType.QuadIn, EaseQuadIn}, {EaseType.QuadOut, EaseQuadOut}, {EaseType.QuadInOut, EaseQuadInOut},
			{EaseType.CubicIn, EaseCubicIn}, {EaseType.CubicOut, EaseCubicOut}, {EaseType.CubicInOut, EaseCubicInOut},
			{EaseType.QuartIn, EaseQuartIn}, {EaseType.QuartOut, EaseQuartOut}, {EaseType.QuartInOut, EaseQuartInOut},
			{EaseType.QuintIn, EaseQuintIn}, {EaseType.QuintOut, EaseQuintOut}, {EaseType.QuintInOut, EaseQuintInOut},
			{EaseType.ExpoIn, EaseExpoIn}, {EaseType.ExpoOut, EaseExpoOut}, {EaseType.ExpoInOut, EaseExpoInOut},
			{EaseType.CircIn, EaseCircIn}, {EaseType.CircOut, EaseCircOut}, {EaseType.CircInOut, EaseCircInOut},
			{EaseType.BackIn, EaseBackIn}, {EaseType.BackOut, EaseBackOut}, {EaseType.BackInOut, EaseBackInOut},
			{EaseType.ElasticIn, EaseElasticIn}, {EaseType.ElasticOut, EaseElasticOut}, {EaseType.ElasticInOut, EaseElasticInOut},
			{EaseType.BounceIn, EaseBounceIn}, {EaseType.BounceOut, EaseBounceOut}, {EaseType.BounceInOut, EaseBounceInOut},
			{EaseType.Spring, EaseSpring}
		};

		public readonly static Dictionary<EaseType, System.Func<Vector2,Vector2,float,Vector2> > EaseVector2 = new Dictionary<EaseType, Func<Vector2,Vector2,float,Vector2> >
		{
			{EaseType.Linear, EaseLinear},
			{EaseType.SineIn, EaseSineIn}, {EaseType.SineOut, EaseSineOut}, {EaseType.SineInOut, EaseSineInOut},
			{EaseType.QuadIn, EaseQuadIn}, {EaseType.QuadOut, EaseQuadOut}, {EaseType.QuadInOut, EaseQuadInOut},
			{EaseType.CubicIn, EaseCubicIn}, {EaseType.CubicOut, EaseCubicOut}, {EaseType.CubicInOut, EaseCubicInOut},
			{EaseType.QuartIn, EaseQuartIn}, {EaseType.QuartOut, EaseQuartOut}, {EaseType.QuartInOut, EaseQuartInOut},
			{EaseType.QuintIn, EaseQuintIn}, {EaseType.QuintOut, EaseQuintOut}, {EaseType.QuintInOut, EaseQuintInOut},
			{EaseType.ExpoIn, EaseExpoIn}, {EaseType.ExpoOut, EaseExpoOut}, {EaseType.ExpoInOut, EaseExpoInOut},
			{EaseType.CircIn, EaseCircIn}, {EaseType.CircOut, EaseCircOut}, {EaseType.CircInOut, EaseCircInOut},
			{EaseType.BackIn, EaseBackIn}, {EaseType.BackOut, EaseBackOut}, {EaseType.BackInOut, EaseBackInOut},
			{EaseType.ElasticIn, EaseElasticIn}, {EaseType.ElasticOut, EaseElasticOut}, {EaseType.ElasticInOut, EaseElasticInOut},
			{EaseType.BounceIn, EaseBounceIn}, {EaseType.BounceOut, EaseBounceOut}, {EaseType.BounceInOut, EaseBounceInOut},
			{EaseType.Spring, EaseSpring}
		};

		public readonly static Dictionary<EaseType, System.Func<Vector3,Vector3,float,Vector3> > EaseVector3 = new Dictionary<EaseType, Func<Vector3,Vector3,float,Vector3> >
		{
			{EaseType.Linear, EaseLinear},
			{EaseType.SineIn, EaseSineIn}, {EaseType.SineOut, EaseSineOut}, {EaseType.SineInOut, EaseSineInOut},
			{EaseType.QuadIn, EaseQuadIn}, {EaseType.QuadOut, EaseQuadOut}, {EaseType.QuadInOut, EaseQuadInOut},
			{EaseType.CubicIn, EaseCubicIn}, {EaseType.CubicOut, EaseCubicOut}, {EaseType.CubicInOut, EaseCubicInOut},
			{EaseType.QuartIn, EaseQuartIn}, {EaseType.QuartOut, EaseQuartOut}, {EaseType.QuartInOut, EaseQuartInOut},
			{EaseType.QuintIn, EaseQuintIn}, {EaseType.QuintOut, EaseQuintOut}, {EaseType.QuintInOut, EaseQuintInOut},
			{EaseType.ExpoIn, EaseExpoIn}, {EaseType.ExpoOut, EaseExpoOut}, {EaseType.ExpoInOut, EaseExpoInOut},
			{EaseType.CircIn, EaseCircIn}, {EaseType.CircOut, EaseCircOut}, {EaseType.CircInOut, EaseCircInOut},
			{EaseType.BackIn, EaseBackIn}, {EaseType.BackOut, EaseBackOut}, {EaseType.BackInOut, EaseBackInOut},
			{EaseType.ElasticIn, EaseElasticIn}, {EaseType.ElasticOut, EaseElasticOut}, {EaseType.ElasticInOut, EaseElasticInOut},
			{EaseType.BounceIn, EaseBounceIn}, {EaseType.BounceOut, EaseBounceOut}, {EaseType.BounceInOut, EaseBounceInOut},
			{EaseType.Spring, EaseSpring}
		};

		public readonly static Dictionary<EaseType, System.Func<Vector4,Vector4,float,Vector4> > EaseVector4 = new Dictionary<EaseType, Func<Vector4,Vector4,float,Vector4> >
		{
			{EaseType.Linear, EaseLinear},
			{EaseType.SineIn, EaseSineIn}, {EaseType.SineOut, EaseSineOut}, {EaseType.SineInOut, EaseSineInOut},
			{EaseType.QuadIn, EaseQuadIn}, {EaseType.QuadOut, EaseQuadOut}, {EaseType.QuadInOut, EaseQuadInOut},
			{EaseType.CubicIn, EaseCubicIn}, {EaseType.CubicOut, EaseCubicOut}, {EaseType.CubicInOut, EaseCubicInOut},
			{EaseType.QuartIn, EaseQuartIn}, {EaseType.QuartOut, EaseQuartOut}, {EaseType.QuartInOut, EaseQuartInOut},
			{EaseType.QuintIn, EaseQuintIn}, {EaseType.QuintOut, EaseQuintOut}, {EaseType.QuintInOut, EaseQuintInOut},
			{EaseType.ExpoIn, EaseExpoIn}, {EaseType.ExpoOut, EaseExpoOut}, {EaseType.ExpoInOut, EaseExpoInOut},
			{EaseType.CircIn, EaseCircIn}, {EaseType.CircOut, EaseCircOut}, {EaseType.CircInOut, EaseCircInOut},
			{EaseType.BackIn, EaseBackIn}, {EaseType.BackOut, EaseBackOut}, {EaseType.BackInOut, EaseBackInOut},
			{EaseType.ElasticIn, EaseElasticIn}, {EaseType.ElasticOut, EaseElasticOut}, {EaseType.ElasticInOut, EaseElasticInOut},
			{EaseType.BounceIn, EaseBounceIn}, {EaseType.BounceOut, EaseBounceOut}, {EaseType.BounceInOut, EaseBounceInOut},
			{EaseType.Spring, EaseSpring}
		};

		interface IUpdaterBase {
			void Update(float t);
			void ReverseUpdate(float t);
		}

		struct ValueSetter<S,T>{
			public S updateSrc;
			public ValueSetter( S src )
			{
				updateSrc = src;
			}
			public void UpdateFunction(T value){}
		}

		struct UpdaterParamNew<S,T> where S : Component where T: struct {
			public T from;
			public T to;
			public ComponentUpdater<S,T> updater;
			public Interporate op;
			public UpdaterParamNew( T from, T to, Interporate op,ComponentUpdater<S,T> updater){
				this.from = from;
				this.to = to;
				this.updater = updater;
				this.op = op;
			}
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

		static async UniTask tweenBase<T>(T updater, float duration, CancellationToken ct,
												System.Action<bool> complete=null,int repeat=0, float delay=0, bool pingPong=false) where T: IUpdaterBase
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
			if ( ct != null ){
				//Debug.Log($"GetCt:<color=green>src:{ct.Value}</color>");
				return ct.Value;
			}
			/*
			else{
				var name = go!=null?go.name:"nullObject";
				Debug.Log($"GetCt:<color=red>src:null,go:{name}</color>");
			}
			*/
			return go!=null?go.GetCancellationTokenOnDestroy():CancellationToken.None;
		}
		static CancellationToken getCT(Transform t,CancellationToken? ct=null) => getCT(t.gameObject,ct);
		static CancellationToken getCT(Renderer r,CancellationToken? ct=null) => getCT(r.gameObject,ct);
		static CancellationToken getCT(Graphic r,CancellationToken? ct=null) => getCT(r.gameObject,ct);
		static CancellationToken getCT(SpriteRenderer r,CancellationToken? ct=null) => getCT(r.gameObject,ct);
		static CancellationToken getCT(CanvasGroup g,CancellationToken? ct=null) => getCT(g.gameObject,ct);
		static CancellationToken getCT(TMP_Text t,CancellationToken? ct=null) => getCT(t.gameObject,ct);
		static CancellationToken getCT(Material m,CancellationToken? ct=null) {
			return ct!=null?ct.Value:CancellationToken.None;
		}

		// Transform
		public static async UniTask XTo(this Transform transform, float to,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
		{
			await Tween( transform.position.x, to, duration, (newPos)=>{ var p = transform.position; p.x = newPos; transform.position = p; },
												type, getCT(transform,ct), complete, repeat, delay, pingPong, customFunc );
		}
		public static async UniTask YTo(this Transform transform, float to,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
		{
			await Tween( transform.position.y, to, duration, (newPos)=>{ var p = transform.position; p.y = newPos; transform.position = p; },
												type, getCT(transform,ct), complete, repeat, delay, pingPong, customFunc );
		}
		public static async UniTask ZTo(this Transform transform, float to,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
		{
			await Tween( transform.position.z, to, duration, (newPos)=>{ var p = transform.position; p.z = newPos; transform.position = p; },
												type, getCT(transform,ct), complete, repeat, delay, pingPong, customFunc );
		}
		public static async UniTask XOffset(this Transform transform, float offset,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
		{
			await transform.XTo( transform.position.x+offset, duration, type, getCT(transform,ct), complete, repeat, delay, pingPong, customFunc);
		}
		public static async UniTask YOffset(this Transform transform, float offset,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
		{
			await transform.YTo( transform.position.y+offset,duration, type, getCT(transform,ct), complete, repeat, delay, pingPong, customFunc);
		}
		public static async UniTask ZOffset(this Transform transform, float offset,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
		{
			await transform.ZTo( transform.position.z+offset, duration, type, getCT(transform,ct), complete, repeat, delay, pingPong, customFunc);
		}

		public static async UniTask LocalXTo(this Transform transform, float to,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
		{
			await Tween( transform.localPosition.x, to, duration, (newPos)=>{ var p = transform.localPosition; p.x = newPos; transform.localPosition = p; },
												type, getCT(transform,ct), complete, repeat, delay, pingPong, customFunc );
		}
		public static async UniTask LocalYTo(this Transform transform, float to,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
		{
			await Tween( transform.localPosition.y, to, duration, (newPos)=>{ var p = transform.localPosition; p.y = newPos; transform.localPosition = p; },
												type, getCT(transform,ct), complete, repeat, delay, pingPong, customFunc );
		}
		public static async UniTask LocalZTo(this Transform transform, float to,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
		{
			await Tween( transform.localPosition.z, to, duration, (newPos)=>{ var p = transform.localPosition; p.z = newPos; transform.localPosition = p; },
												type, getCT(transform,ct), complete, repeat, delay, pingPong, customFunc );
		}
		public static async UniTask LocalXOffset(this Transform transform, float offset,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
		{
			await transform.LocalXTo( transform.localPosition.x+offset, duration, type, getCT(transform,ct), complete, repeat, delay, pingPong, customFunc);
		}
		public static async UniTask LocalYOffset(this Transform transform, float offset,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
		{
			await transform.LocalYTo( transform.localPosition.y+offset, duration, type, getCT(transform,ct), complete, repeat, delay, pingPong, customFunc);
		}
		public static async UniTask LocalZOffset(this Transform transform, float offset,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
		{
			await transform.LocalZTo( transform.localPosition.z+offset, duration, type, getCT(transform,ct), complete, repeat, delay, pingPong, customFunc);
		}

		public static async UniTask XYTo(this Transform transform, Vector2 to, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
		{
			await Tween( (Vector2)transform.position, to, duration, (newPos)=>{ var p = transform.position; p.x = newPos.x; p.y = newPos.y; transform.position = p; },
												type, getCT(transform,ct), complete, repeat, delay, pingPong, customFunc );
		}
		public static async UniTask XYOffset(this Transform transform, Vector2 offset, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
		{
			await transform.XYTo( (Vector2)transform.position+offset, duration, type, getCT(transform,ct), complete, repeat, delay, pingPong, customFunc);
		}

		public static async UniTask LocalXYTo(this Transform transform, Vector2 to, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
		{
			await Tween( (Vector2)transform.localPosition, to, duration, (newPos)=>{ var p = transform.localPosition; p.x = newPos.x; p.y = newPos.y; transform.localPosition = p; },
												type, getCT(transform,ct), complete, repeat, delay, pingPong, customFunc );
		}
		public static async UniTask LocalXYOffset(this Transform transform, Vector2 offset, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
		{

			await transform.LocalXYTo( (Vector2)transform.localPosition+offset, duration, type, getCT(transform,ct), complete, repeat, delay, pingPong, customFunc);
		}

		public static async UniTask XYZTo(this Transform transform, Vector3 to, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
		{
			await Tween( transform.position, to, duration, (newPos)=>{ transform.position = newPos; },
												type, getCT(transform,ct), complete, repeat, delay, pingPong, customFunc );
		}
		public static async UniTask XYZOffset(this Transform transform, Vector3 offset, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
		{
			await transform.XYZTo( transform.localPosition+offset, duration, type, getCT(transform,ct), complete, repeat, delay, pingPong, customFunc);
		}
		public static async UniTask LocalXYZTo(this Transform transform, Vector3 to, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
		{
			await Tween( transform.localPosition, to, duration, (newPos)=>{ transform.localPosition = newPos; },
												type, getCT(transform,ct), complete, repeat, delay, pingPong, customFunc );
		}

		public static async UniTask LocalXYZOffset(this Transform transform, Vector3 offset, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
		{
			await transform.LocalXYZTo( transform.localPosition+offset, duration, type, getCT(transform,ct), complete, repeat, delay, pingPong, customFunc);
		}

		public static async UniTask LocalRotXTo(this Transform transform, float toAngle,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
		{
			await Tween( transform.localEulerAngles.x, toAngle, duration, (newPos)=>{ var p = transform.localEulerAngles; p.x = newPos; transform.localEulerAngles = p;},
												type, getCT(transform,ct), complete, repeat, delay, pingPong, customFunc );
		}
		public static async UniTask LocalRotYTo(this Transform transform, float toAngle,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
		{
			await Tween( transform.localEulerAngles.y, toAngle, duration, (newPos)=>{ var p = transform.localEulerAngles; p.y = newPos; transform.localEulerAngles = p; },
												type, getCT(transform,ct), complete, repeat, delay, pingPong, customFunc );
		}
		public static async UniTask LocalRotZTo(this Transform transform, float toAngle,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
		{
			await Tween( transform.localEulerAngles.z, toAngle, duration, (newPos)=>{ var p = transform.localEulerAngles; p.z = newPos; transform.localEulerAngles = p; },
												type, getCT(transform,ct), complete, repeat, delay, pingPong, customFunc );
		}

		public static async UniTask RotXTo(this Transform transform, float toAngle,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
		{
			await Tween( transform.eulerAngles.x, toAngle, duration, (newPos)=>{ var p = transform.eulerAngles; p.x = newPos; transform.eulerAngles = p; },
												type, getCT(transform,ct), complete, repeat, delay, pingPong, customFunc );
		}
		public static async UniTask RotYTo(this Transform transform, float toAngle,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
		{
			await Tween( transform.eulerAngles.y, toAngle, duration, (newPos)=>{ var p = transform.eulerAngles; p.y = newPos; transform.eulerAngles = p; },
												type, getCT(transform,ct), complete, repeat, delay, pingPong, customFunc );
		}
		public static async UniTask RotZTo(this Transform transform, float toAngle,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
		{
			await Tween( transform.eulerAngles.z, toAngle, duration, (newPos)=>{ var p = transform.eulerAngles; p.z = newPos; transform.eulerAngles = p; },
												type, getCT(transform,ct), complete, repeat, delay, pingPong, customFunc );
		}

		public static async UniTask ScaleXTo(this Transform transform, float to,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
		{
			await Tween( transform.localScale.x, to, duration, (newPos)=>{ var p = transform.localScale; p.x = newPos; transform.localScale = p; },
												type, getCT(transform,ct), complete, repeat, delay, pingPong, customFunc );
		}
		public static async UniTask ScaleYTo(this Transform transform, float to,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
		{
			await Tween( transform.localScale.y, to, duration, (newPos)=>{ var p = transform.localScale; p.y = newPos; transform.localScale = p; },
												type, getCT(transform,ct), complete, repeat, delay, pingPong, customFunc );
		}
		public static async UniTask ScaleZTo(this Transform transform, float to,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
		{
			await Tween( transform.localScale.z, to, duration, (newPos)=>{ var p = transform.localScale; p.z = newPos; transform.localScale = p; },
												type, getCT(transform,ct), complete, repeat, delay, pingPong, customFunc );
		}

		public static async UniTask ScaleXYTo(this Transform transform, Vector2 to, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
		{
			await Tween( (Vector2)transform.localScale, to, duration, (newPos)=>{ var p = transform.localScale; p.x = newPos.x; p.y = newPos.y; transform.localScale = p; },
												type, getCT(transform,ct), complete, repeat, delay, pingPong, customFunc );
		}

		public static async UniTask ScaleXYZTo(this Transform transform, Vector3 to, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
		{
			await Tween( transform.localScale, to, duration, (newPos)=>{ transform.localScale = newPos; },
												type, getCT(transform,ct), complete, repeat, delay, pingPong, customFunc );
		}

		// RectTransform
		public static async UniTask AnchorXTo(this RectTransform transform, float to,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
		{
			await Tween( transform.anchoredPosition.x, to, duration, (newPos)=>{ var p = transform.anchoredPosition; p.x = newPos; transform.anchoredPosition = p; },
												type, getCT(transform,ct), complete, repeat, delay, pingPong, customFunc );
		}
		public static async UniTask AnchorYTo(this RectTransform transform, float to,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
		{
			await Tween( transform.anchoredPosition.y, to, duration, (newPos)=>{ var p = transform.anchoredPosition; p.y = newPos; transform.anchoredPosition = p; },
												type, getCT(transform,ct), complete, repeat, delay, pingPong, customFunc );
		}

		public static async UniTask AnchorXOffset(this RectTransform transform, float offset,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
		{
			await transform.XTo( transform.anchoredPosition.x+offset, duration, type, getCT(transform,ct), complete, repeat, delay, pingPong, customFunc);
		}
		public static async UniTask AnchorYOffset(this RectTransform transform, float offset,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
		{
			await transform.YTo( transform.anchoredPosition.y+offset,duration, type, getCT(transform,ct), complete, repeat, delay, pingPong, customFunc);
		}

		public static async UniTask AnchorXYTo(this RectTransform transform, Vector2 to, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
		{
			await Tween( (Vector2)transform.anchoredPosition, to, duration, (newPos)=>{ var p = transform.anchoredPosition; p.x = newPos.x; p.y = newPos.y; transform.anchoredPosition = p; },
												type, getCT(transform,ct), complete, repeat, delay, pingPong, customFunc );
		}
		public static async UniTask AnchorXYOffset(this RectTransform transform, Vector2 offset, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
		{
			await transform.XYTo( (Vector2)transform.anchoredPosition+offset, duration, type, getCT(transform,ct), complete, repeat, delay, pingPong, customFunc);
		}

		public static async UniTask AnchorMinMaxXTo(this RectTransform transform, Vector2 to,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
		{
			var minmax = new Vector2(transform.anchorMin.x,transform.anchorMax.x);
			await Tween( minmax, to, duration, (newPos)=>{var min = transform.anchorMin;var max = transform.anchorMax;min.x = newPos.x;max.x = newPos.y;
															transform.anchorMin = min;transform.anchorMax = max;},
												type, getCT(transform,ct), complete, repeat, delay, pingPong, customFunc );
		}
		public static async UniTask AnchorMinMaxXTo(this RectTransform transform, Vector2 from, Vector2 to,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
		{
			var min = transform.anchorMin;min.x = from.x;transform.anchorMin = min;
			var max = transform.anchorMax;max.x = from.y;transform.anchorMax = max;
			await transform.AnchorMinMaxXTo( to, duration, 
												type, getCT(transform,ct), complete, repeat, delay, pingPong, customFunc );
		}

		public static async UniTask AnchorMinMaxYTo(this RectTransform transform, Vector2 to,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
		{
			var minmax = new Vector2(transform.anchorMin.y,transform.anchorMax.y);
			await Tween( minmax, to, duration, (newPos)=>{var min = transform.anchorMin;var max = transform.anchorMax;min.y = newPos.x;max.y = newPos.y;
															transform.anchorMin = min;transform.anchorMax = max;},
												type, getCT(transform,ct), complete, repeat, delay, pingPong, customFunc );
		}

		public static async UniTask AnchorMinMaxYTo(this RectTransform transform, Vector2 from, Vector2 to,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
		{
			var min = transform.anchorMin;min.y = from.x;transform.anchorMin = min;
			var max = transform.anchorMax;max.y = from.y;transform.anchorMax = max;
			await transform.AnchorMinMaxYTo( to, duration, 
												type, getCT(transform,ct), complete, repeat, delay, pingPong, customFunc );
		}

		public static async UniTask AnchorMinMaxXOffset(this RectTransform transform, Vector2 offset,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
		{
			var to = new Vector2(transform.anchorMin.x+offset.x,transform.anchorMax.x+offset.y);
			await transform.AnchorMinMaxXTo( to, duration, 
												type, getCT(transform,ct), complete, repeat, delay, pingPong, customFunc );
		}
		public static async UniTask AnchorMinMaxYOffset(this RectTransform transform, Vector2 offset,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
		{
			var to = new Vector2(transform.anchorMin.y+offset.x,transform.anchorMax.y+offset.y);
			await transform.AnchorMinMaxYTo( to, duration, 
												type, getCT(transform,ct), complete, repeat, delay, pingPong, customFunc );
		}

		public static async UniTask AnchorMinMaxTo(this RectTransform transform, Vector2 toMin,Vector2 toMax, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
		{
			var minmax = new Vector4(transform.anchorMin.x,transform.anchorMin.y,transform.anchorMax.x,transform.anchorMax.y);
			var to = new Vector4(toMin.x,toMin.y,toMax.x,toMax.y);
			await Tween( minmax, to, duration, (newPos)=>{var min = transform.anchorMin;var max = transform.anchorMax;
															min = new Vector2(newPos.x,newPos.y);
															max = new Vector2(newPos.z,newPos.w);
															transform.anchorMin = min;transform.anchorMax = max;},
												type, getCT(transform,ct), complete, repeat, delay, pingPong, customFunc );
		}

		public static async UniTask AnchorMinMaxTo(this RectTransform transform, Vector2 fromMin,Vector2 toMin,Vector2 fromMax,Vector2 toMax, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
		{
			transform.anchorMin = fromMin;
			transform.anchorMax = fromMax;
			await transform.AnchorMinMaxTo( toMin, toMax, duration,
												type, getCT(transform,ct), complete, repeat, delay, pingPong, customFunc );
		}

		public static async UniTask AnchorMinMaxOffset(this RectTransform transform, Vector2 offsetMin,Vector2 offsetMax, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
		{
			await transform.AnchorMinMaxTo( transform.anchorMin+offsetMin, transform.anchorMax+offsetMax, duration, 
												type, getCT(transform,ct), complete, repeat, delay, pingPong, customFunc );
		}

		// GameObject
		public static async UniTask XTo(this GameObject fromObj, float to,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
												=> await fromObj.transform.XTo( to, duration, type, getCT(fromObj,ct), complete, repeat, delay, pingPong, customFunc);
		public static async UniTask YTo(this GameObject fromObj, float to,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
												=> await fromObj.transform.YTo( to, duration, type, getCT(fromObj,ct), complete, repeat, delay, pingPong, customFunc);
		public static async UniTask ZTo(this GameObject fromObj, float to,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
												=> await fromObj.transform.ZTo( to, duration, type, getCT(fromObj,ct), complete, repeat, delay, pingPong, customFunc);
		public static async UniTask XOffset(this GameObject fromObj, float offset,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
												=> await fromObj.transform.XOffset( offset, duration, type, getCT(fromObj,ct), complete, repeat, delay, pingPong, customFunc);
		public static async UniTask YOffset(this GameObject fromObj, float offset,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
												=> await fromObj.transform.YOffset( offset, duration, type, getCT(fromObj,ct), complete, repeat, delay, pingPong, customFunc);
		public static async UniTask ZOffset(this GameObject fromObj, float offset,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
												=> await fromObj.transform.ZOffset( offset, duration, type, getCT(fromObj,ct), complete, repeat, delay, pingPong, customFunc);
		public static async UniTask LocalXTo(this GameObject fromObj, float to,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
												=> await fromObj.transform.LocalXTo( to, duration, type, getCT(fromObj,ct), complete, repeat, delay, pingPong, customFunc);
		public static async UniTask LocalYTo(this GameObject fromObj, float to,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
												=> await fromObj.transform.LocalYTo( to, duration, type, getCT(fromObj,ct), complete, repeat, delay, pingPong, customFunc);
		public static async UniTask LocalZTo(this GameObject fromObj, float to,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
												=> await fromObj.transform.LocalZTo( to, duration, type, getCT(fromObj,ct), complete, repeat, delay, pingPong, customFunc);
		public static async UniTask LocalXOffset(this GameObject fromObj, float offset,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
												=> await fromObj.transform.LocalXOffset( offset, duration, type, getCT(fromObj,ct), complete, repeat, delay, pingPong, customFunc);
		public static async UniTask LocalYOffset(this GameObject fromObj, float offset,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
												=> await fromObj.transform.LocalYOffset( offset, duration, type, getCT(fromObj,ct), complete, repeat, delay, pingPong, customFunc);
		public static async UniTask LocalZOffset(this GameObject fromObj, float offset,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
												=> await fromObj.transform.LocalZOffset( offset, duration, type, getCT(fromObj,ct), complete, repeat, delay, pingPong, customFunc);
		public static async UniTask XYTo(this GameObject fromObj, Vector2 to, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
												=> await fromObj.transform.XYTo( to, duration, type, getCT(fromObj,ct), complete, repeat, delay, pingPong, customFunc);
		public static async UniTask XYOffset(this GameObject fromObj, Vector2 offset, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
												=> await fromObj.transform.XYOffset( offset, duration, type, getCT(fromObj,ct), complete, repeat, delay, pingPong, customFunc);
		public static async UniTask LocalXYTo(this GameObject fromObj, Vector2 to, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
												=> await fromObj.transform.LocalXYTo( to, duration, type, getCT(fromObj,ct), complete, repeat, delay, pingPong, customFunc);
		public static async UniTask LocalXYOffset(this GameObject fromObj, Vector2 offset, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
												=> await fromObj.transform.LocalXYOffset( offset, duration, type, getCT(fromObj,ct), complete, repeat, delay, pingPong, customFunc);
		public static async UniTask XYZTo(this GameObject fromObj, Vector3 to, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
												=> await fromObj.transform.XYZTo( to, duration, type, getCT(fromObj,ct), complete, repeat, delay, pingPong, customFunc);
		public static async UniTask XYZOffset(this GameObject fromObj, Vector3 offset, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
												=> await fromObj.transform.XYZOffset( offset, duration, type, getCT(fromObj,ct), complete, repeat, delay, pingPong, customFunc);
		public static async UniTask LocalXYZTo(this GameObject fromObj, Vector3 to, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
												=> await fromObj.transform.LocalXYZTo( to, duration, type, getCT(fromObj,ct), complete, repeat, delay, pingPong, customFunc);
		public static async UniTask LocalXYZOffset(this GameObject fromObj, Vector3 offset, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
												=> await fromObj.transform.LocalXYZOffset( offset, duration, type, getCT(fromObj,ct), complete, repeat, delay, pingPong, customFunc);

		public static async UniTask RotXTo(this GameObject fromObj, float toAngle,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
												=> await fromObj.transform.RotXTo( toAngle, duration, type, getCT(fromObj,ct), complete, repeat, delay, pingPong, customFunc);
		public static async UniTask RotYTo(this GameObject fromObj, float toAngle,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
												=> await fromObj.transform.RotYTo( toAngle, duration, type, getCT(fromObj,ct), complete, repeat, delay, pingPong, customFunc);
		public static async UniTask RotZTo(this GameObject fromObj, float toAngle,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
												=> await fromObj.transform.RotZTo( toAngle, duration, type, getCT(fromObj,ct), complete, repeat, delay, pingPong, customFunc);

		public static async UniTask LocalRotXTo(this GameObject fromObj, float toAngle,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
												=> await fromObj.transform.LocalRotXTo( toAngle, duration, type, getCT(fromObj,ct), complete, repeat, delay, pingPong, customFunc);
		public static async UniTask LocalRotYTo(this GameObject fromObj, float toAngle,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
												=> await fromObj.transform.LocalRotYTo( toAngle, duration, type, getCT(fromObj,ct), complete, repeat, delay, pingPong, customFunc);
		public static async UniTask LocalRotZTo(this GameObject fromObj, float toAngle,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
												=> await fromObj.transform.LocalRotZTo( toAngle, duration, type, getCT(fromObj,ct), complete, repeat, delay, pingPong, customFunc);

		public static async UniTask ScaleXTo(this GameObject fromObj, float to,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
												=> await fromObj.transform.ScaleXTo( to, duration, type, getCT(fromObj,ct), complete, repeat, delay, pingPong, customFunc);
		public static async UniTask ScaleYTo(this GameObject fromObj, float to,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
												=> await fromObj.transform.ScaleYTo( to, duration, type, getCT(fromObj,ct), complete, repeat, delay, pingPong, customFunc);
		public static async UniTask ScaleZTo(this GameObject fromObj, float to,  float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
												=> await fromObj.transform.ScaleZTo( to, duration, type, getCT(fromObj,ct), complete, repeat, delay, pingPong, customFunc);
		public static async UniTask ScaleXYTo(this GameObject fromObj, Vector2 to, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
												=> await fromObj.transform.ScaleXYTo( to, duration, type, getCT(fromObj,ct), complete, repeat, delay, pingPong, customFunc);
		public static async UniTask ScaleXYZTo(this GameObject fromObj, Vector3 to, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
												=> await fromObj.transform.ScaleXYZTo( to, duration, type, getCT(fromObj,ct), complete, repeat, delay, pingPong, customFunc);

		// SpriteRenderer
		public static async UniTask SizeTo(this SpriteRenderer fromObj, Vector2 to, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
		{
			await Tween( (Vector2)fromObj.size, to, duration, (newPos)=>{ fromObj.size = newPos; },
												type, getCT(fromObj,ct), complete, repeat, delay, pingPong, customFunc );
		}

		public static async UniTask AlphaTo(this Renderer fromObj, float to, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
		{
			await Tween( fromObj.material.color.a, to, duration, (newPos)=>{ var col = fromObj.material.color; col.a = newPos; fromObj.material.color = col; },
												type, getCT(fromObj,ct), complete, repeat, delay, pingPong, customFunc );
		}

		public static async UniTask AlphaTo(this Renderer fromObj, float from, float to, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
		{
			var col = fromObj.material.color;
			col.a = from;
			fromObj.material.color = col;
			await Tween( from, to, duration, (newPos)=>{ var col = fromObj.material.color; col.a = newPos; fromObj.material.color = col; },
												type, getCT(fromObj,ct), complete, repeat, delay, pingPong, customFunc );
		}

		public static async UniTask AlphaTo(this Graphic fromObj, float to, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
		{

			await Tween( fromObj.color.a, to, duration, (newPos)=>{ var col = fromObj.color; col.a = newPos; fromObj.color = col; },
												type, getCT(fromObj,ct), complete, repeat, delay, pingPong, customFunc );
		}
		public static async UniTask AlphaTo(this Graphic fromObj,float from, float to, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
		{
			var col = fromObj.color;
			col.a = from;
			fromObj.color = col;
			await Tween( fromObj.color.a, to, duration, (newPos)=>{ var col = fromObj.color; col.a = newPos; fromObj.color = col; },
												type, getCT(fromObj,ct), complete, repeat, delay, pingPong, customFunc );
		}

		public static async UniTask AlphaTo(this Material fromObj, float to, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
		{
			await Tween( fromObj.color.a, to, duration, (newPos)=>{ var col = fromObj.color; col.a = newPos; fromObj.color = col; },
												type, getCT(fromObj,ct), complete, repeat, delay, pingPong, customFunc );
		}
		public static async UniTask AlphaTo(this Material fromObj, float from,float to, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
		{
			var col = fromObj.color;
			col.a = from;
			fromObj.color = col;
			await Tween( fromObj.color.a, to, duration, (newPos)=>{ var col = fromObj.color; col.a = newPos; fromObj.color = col; },
												type, getCT(fromObj,ct), complete, repeat, delay, pingPong, customFunc );
		}

		public static async UniTask ColorTo(this Renderer fromObj, Vector3 to, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
		{

			await Tween( fromObj.material.color, to, duration,
								(newPos)=>{ fromObj.material.color = new Color(newPos.x,newPos.y,newPos.z,fromObj.material.color.a);  },
								type, getCT(fromObj,ct), complete, repeat, delay, pingPong, customFunc );
		}

		public static async UniTask ColorTo(this Graphic fromObj, Vector3 to, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
		{
			await Tween( fromObj.color, to, duration,
								(newPos)=>{ fromObj.color = new Color(newPos.x,newPos.y,newPos.z,fromObj.color.a);  },
								type, getCT(fromObj,ct), complete, repeat, delay, pingPong, customFunc );
		}

		public static async UniTask ColorTo(this SpriteRenderer fromObj, Color to, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
		{

			await Tween( fromObj.color, to, duration,
								(newPos)=>{ fromObj.color = newPos; },
								type, getCT(fromObj,ct), complete, repeat, delay, pingPong, customFunc );
		}

		public static async UniTask ColorTo(this Renderer fromObj, Color to, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
		{

			await Tween( fromObj.material.color, to, duration,
								(newPos)=>{ fromObj.material.color = newPos; },
								type, getCT(fromObj,ct), complete, repeat, delay, pingPong, customFunc );
		}

		public static async UniTask ColorTo(this Graphic fromObj, Color to, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
		{
			await Tween( fromObj.color, to, duration,
								(newPos)=>{ fromObj.color = newPos; },
								type, getCT(fromObj,ct), complete, repeat, delay, pingPong, customFunc );
		}

		public static async UniTask ColorTo(this Material fromObj, Color to, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
		{
			await Tween( fromObj.color, to, duration,
								(newPos)=>{ fromObj.color = newPos; },
								type, getCT(fromObj,ct), complete, repeat, delay, pingPong, customFunc );
		}
		public static async UniTask ColorTo(this Material fromObj, Color from,Color to, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
		{
			fromObj.color = from;
			await Tween( fromObj.color, to, duration,
								(newPos)=>{ fromObj.color = newPos; },
								type, getCT(fromObj,ct), complete, repeat, delay, pingPong, customFunc );
		}

		public static async UniTask HSVTo(this Renderer fromObj, Vector3 to, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
		{
			Vector3 hsv = Vector3.zero;
			Color.RGBToHSV(fromObj.material.color,out hsv.x, out hsv.y, out hsv.z );
			await Tween( hsv, to, duration,
								(newPos)=>{
									var newCol = Color.HSVToRGB(newPos.x,newPos.y,newPos.z);
									newCol.a = fromObj.material.color.a;
									fromObj.material.color = newCol;
								},
								type, getCT(fromObj,ct), complete, repeat, delay, pingPong, customFunc );
		}

		public static async UniTask HSVTo(this Graphic fromObj, Vector3 to, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
		{
			Vector3 hsv = Vector3.zero;
			Color.RGBToHSV(fromObj.color,out hsv.x, out hsv.y, out hsv.z );
			await Tween( hsv, to, duration,
								(newPos)=>{
									var newCol = Color.HSVToRGB(newPos.x,newPos.y,newPos.z);
									newCol.a = fromObj.material.color.a;
									fromObj.color = newCol;
								},
								type, getCT(fromObj,ct), complete, repeat, delay, pingPong, customFunc );
		}

		public static async UniTask HSVTo(this SpriteRenderer fromObj, Color to, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
		{
			Vector4 hsv = Vector4.zero;
			Color.RGBToHSV(fromObj.color,out hsv.x, out hsv.y, out hsv.z );
			hsv.w = fromObj.color.a;
			await Tween( hsv, to, duration,
								(newPos)=>{
									var newCol = Color.HSVToRGB(newPos.x,newPos.y,newPos.z);
									newCol.a = newPos.w;
									fromObj.color = newCol;
								},
								type, getCT(fromObj,ct), complete, repeat, delay, pingPong, customFunc );
		}

		public static async UniTask HSVTo(this Renderer fromObj, Color to, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
		{
			Vector4 hsv = Vector4.zero;
			Color.RGBToHSV(fromObj.material.color,out hsv.x, out hsv.y, out hsv.z );
			hsv.w = fromObj.material.color.a;
			await Tween( hsv, to, duration,
								(newPos)=>{
									var newCol = Color.HSVToRGB(newPos.x,newPos.y,newPos.z);
									newCol.a = newPos.w;
									fromObj.material.color = newCol;
								},
								type, getCT(fromObj,ct), complete, repeat, delay, pingPong, customFunc );
		}

		public static async UniTask HSVTo(this Graphic fromObj, Color to, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
		{
			Vector4 hsv = Vector4.zero;
			Color.RGBToHSV(fromObj.color,out hsv.x, out hsv.y, out hsv.z );
			hsv.w = fromObj.color.a;
			await Tween( hsv, to, duration,
								(newPos)=>{
									var newCol = Color.HSVToRGB(newPos.x,newPos.y,newPos.z);
									newCol.a = newPos.w;
									fromObj.color = newCol;
								},
								type, getCT(fromObj,ct), complete, repeat, delay, pingPong, customFunc );
		}

		public static async UniTask HSVTo(this Material fromObj, Color to, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
		{
			Vector4 hsv = Vector4.zero;
			Color.RGBToHSV(fromObj.color,out hsv.x, out hsv.y, out hsv.z );
			hsv.w = fromObj.color.a;
			await Tween( hsv, to, duration,
								(newPos)=>{
									var newCol = Color.HSVToRGB(newPos.x,newPos.y,newPos.z);
									newCol.a = newPos.w;
									fromObj.color = newCol;
								},
								type, getCT(fromObj,ct), complete, repeat, delay, pingPong, customFunc );
		}
		public static async UniTask HSVTo(this Material fromObj, Color from,Color to, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
		{
			fromObj.color = from;
			Vector4 hsv = Vector4.zero;
			Color.RGBToHSV(fromObj.color,out hsv.x, out hsv.y, out hsv.z );
			hsv.w = fromObj.color.a;
			await Tween( hsv, to, duration,
								(newPos)=>{
									var newCol = Color.HSVToRGB(newPos.x,newPos.y,newPos.z);
									newCol.a = newPos.w;
									fromObj.color = newCol;
								},
								type, getCT(fromObj,ct), complete, repeat, delay, pingPong, customFunc );
		}

		public static async UniTask AlphaTo(this CanvasGroup fromObj, float to, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
		{

			await Tween( fromObj.alpha, to, duration, (newPos)=>{ var col = fromObj.alpha = newPos; },
												type, getCT(fromObj,ct), complete, repeat, delay, pingPong, customFunc );
		}

		public static async UniTask AlphaTo(this CanvasGroup fromObj,float from, float to, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
		{
			fromObj.alpha = from;
			await Tween( fromObj.alpha, to, duration, (newPos)=>{ var col = fromObj.alpha = newPos; },
												type, getCT(fromObj,ct), complete, repeat, delay, pingPong, customFunc );
		}

		public static async UniTask TextSend(this TMP_Text fromObj, int fromIndex, int toIndex, float duration, EaseType type=EaseType.Default, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
		{
			var len = fromObj.text.Length; // @todo:本当はGetParsedTextを使いたいが1フレ待つ必要があるためLengthにする.
			fromIndex = Mathf.Clamp(fromIndex,0,len);
			toIndex = Mathf.Clamp(toIndex+1,0,len+1); // 最後の1文字がintだと頭しか表示されないので+1する.
			await Tween( (float)fromIndex, (float)toIndex, duration,
								(newPos)=>{ fromObj.maxVisibleCharacters = (int)Mathf.Clamp(newPos,0,len); },
								type, getCT(fromObj,ct), complete, repeat, delay, pingPong, customFunc );
		}


		// ジェネリクス版
	#region UseGenericsEasing
		public static async UniTask XTo<T>(this Transform transform, float to,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T: IEaseTypeHint,new()=>
					await transform.XTo( to,  duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong, customFunc);
		public static async UniTask YTo<T>(this Transform transform, float to,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T: IEaseTypeHint,new()=>
					await transform.YTo( to,  duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong, customFunc);
		public static async UniTask ZTo<T>(this Transform transform, float to,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T: IEaseTypeHint,new()=>
					await transform.ZTo( to,  duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong, customFunc);
		public static async UniTask XOffset<T>(this Transform transform, float offset,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T: IEaseTypeHint,new()=>
					await transform.XOffset( offset,  duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong, customFunc);
		public static async UniTask YOffset<T>(this Transform transform, float offset,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T: IEaseTypeHint,new()=>
					await transform.YOffset( offset,  duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong, customFunc);
		public static async UniTask ZOffset<T>(this Transform transform, float offset,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T: IEaseTypeHint,new()=>
					await transform.ZOffset( offset,  duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong, customFunc);

		public static async UniTask LocalXTo<T>(this Transform transform, float to,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T: IEaseTypeHint,new()=>
					await transform.LocalXTo( to,  duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong, customFunc);
		public static async UniTask LocalYTo<T>(this Transform transform, float to,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T: IEaseTypeHint,new()=>
					await transform.LocalYTo( to,  duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong, customFunc);
		public static async UniTask LocalZTo<T>(this Transform transform, float to,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T: IEaseTypeHint,new()=>
					await transform.LocalZTo( to,  duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong, customFunc);
		public static async UniTask LocalXOffset<T>(this Transform transform, float offset,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T: IEaseTypeHint,new()=>
					await transform.LocalXOffset( offset,  duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong, customFunc);
		public static async UniTask LocalYOffset<T>(this Transform transform, float offset,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T: IEaseTypeHint,new()=>
					await transform.LocalYOffset( offset,  duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong, customFunc);
		public static async UniTask LocalZOffset<T>(this Transform transform, float offset,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T: IEaseTypeHint,new()=>
					await transform.LocalZOffset( offset,  duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong, customFunc);

		public static async UniTask XYTo<T>(this Transform transform, Vector2 to, float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T: IEaseTypeHint,new()=>
					await transform.XYTo( to, duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong, customFunc);
		public static async UniTask XYOffset<T>(this Transform transform, Vector2 offset, float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T: IEaseTypeHint,new()=>
					await transform.XYOffset( offset, duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong, customFunc);

		public static async UniTask LocalXYTo<T>(this Transform transform, Vector2 to, float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T: IEaseTypeHint,new()=>
					await transform.LocalXYTo( to, duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong, customFunc);
		public static async UniTask LocalXYOffset<T>(this Transform transform, Vector2 offset, float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T: IEaseTypeHint,new()=>
					await transform.LocalXYOffset( offset, duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong, customFunc);
		public static async UniTask XYZTo<T>(this Transform transform, Vector3 to, float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T: IEaseTypeHint,new()=>
					await transform.XYZTo( to, duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong, customFunc);
		public static async UniTask XYZOffset<T>(this Transform transform, Vector3 offset, float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T: IEaseTypeHint,new()=>
					await transform.XYZOffset( offset, duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong, customFunc);
		public static async UniTask LocalXYZTo<T>(this Transform transform, Vector3 to, float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T: IEaseTypeHint,new()=>
					await transform.LocalXYZTo( to, duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong, customFunc);

		public static async UniTask LocalXYZOffset<T>(this Transform transform, Vector3 offset, float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T: IEaseTypeHint,new()=>
					await transform.LocalXYZOffset( offset, duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong, customFunc);

		public static async UniTask LocalRotXTo<T>(this Transform transform, float toAngle,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T: IEaseTypeHint,new()=>
					await transform.LocalRotXTo( toAngle, duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong, customFunc);
		public static async UniTask LocalRotYTo<T>(this Transform transform, float toAngle,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T: IEaseTypeHint,new()=>
					await transform.LocalRotYTo( toAngle, duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong, customFunc);
		public static async UniTask LocalRotZTo<T>(this Transform transform, float toAngle,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T: IEaseTypeHint,new()=>
					await transform.LocalRotZTo( toAngle, duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong, customFunc);

		public static async UniTask RotXTo<T>(this Transform transform, float toAngle,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T: IEaseTypeHint,new()=>
					await transform.RotXTo( toAngle, duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong, customFunc);
		public static async UniTask RotYTo<T>(this Transform transform, float toAngle,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T: IEaseTypeHint,new()=>
					await transform.RotYTo( toAngle, duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong, customFunc);
		public static async UniTask RotZTo<T>(this Transform transform, float toAngle,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T: IEaseTypeHint,new()=>
					await transform.RotZTo( toAngle, duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong, customFunc);

		public static async UniTask ScaleXTo<T>(this Transform transform, float to,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T: IEaseTypeHint,new()=>
					await transform.ScaleXTo( to, duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong, customFunc);
		public static async UniTask ScaleYTo<T>(this Transform transform, float to,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T: IEaseTypeHint,new()=>
					await transform.ScaleYTo( to, duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong, customFunc);
		public static async UniTask ScaleZTo<T>(this Transform transform, float to,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T: IEaseTypeHint,new()=>
					await transform.ScaleZTo( to, duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong, customFunc);

		public static async UniTask ScaleXYTo<T>(this Transform transform, Vector2 to, float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T: IEaseTypeHint,new()=>
					await transform.ScaleXYTo( to, duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong, customFunc);
		public static async UniTask ScaleXYZTo<T>(this Transform transform, Vector3 to, float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T: IEaseTypeHint,new()=>
					await transform.ScaleXYZTo( to, duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong, customFunc);


		// GameObject
		public static async UniTask XTo<T>(this GameObject fromObj, float to,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T: IEaseTypeHint,new()
												=> await fromObj.transform.XTo( to, duration,new T().FuncType(), ct, complete, repeat, delay, pingPong, customFunc);
		public static async UniTask YTo<T>(this GameObject fromObj, float to,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T: IEaseTypeHint,new()
												=> await fromObj.transform.YTo( to, duration,new T().FuncType(), ct, complete, repeat, delay, pingPong, customFunc);
		public static async UniTask ZTo<T>(this GameObject fromObj, float to,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T: IEaseTypeHint,new()
												=> await fromObj.transform.ZTo( to, duration,new T().FuncType(), ct, complete, repeat, delay, pingPong, customFunc);
		public static async UniTask XOffset<T>(this GameObject fromObj, float offset,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T: IEaseTypeHint,new()
												=> await fromObj.transform.XOffset( offset, duration,new T().FuncType(), ct, complete, repeat, delay, pingPong, customFunc);
		public static async UniTask YOffset<T>(this GameObject fromObj, float offset,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T: IEaseTypeHint,new()
												=> await fromObj.transform.YOffset( offset, duration,new T().FuncType(), ct, complete, repeat, delay, pingPong, customFunc);
		public static async UniTask ZOffset<T>(this GameObject fromObj, float offset,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T: IEaseTypeHint,new()
												=> await fromObj.transform.ZOffset( offset, duration,new T().FuncType(), ct, complete, repeat, delay, pingPong, customFunc);
		public static async UniTask LocalXTo<T>(this GameObject fromObj, float to,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T: IEaseTypeHint,new()
												=> await fromObj.transform.LocalXTo( to, duration,new T().FuncType(), ct, complete, repeat, delay, pingPong, customFunc);
		public static async UniTask LocalYTo<T>(this GameObject fromObj, float to,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T: IEaseTypeHint,new()
												=> await fromObj.transform.LocalYTo( to, duration,new T().FuncType(), ct, complete, repeat, delay, pingPong, customFunc);
		public static async UniTask LocalZTo<T>(this GameObject fromObj, float to,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T: IEaseTypeHint,new()
												=> await fromObj.transform.LocalZTo( to, duration,new T().FuncType(), ct, complete, repeat, delay, pingPong, customFunc);
		public static async UniTask LocalXOffset<T>(this GameObject fromObj, float offset,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T: IEaseTypeHint,new()
												=> await fromObj.transform.LocalXOffset( offset, duration,new T().FuncType(), ct, complete, repeat, delay, pingPong, customFunc);
		public static async UniTask LocalYOffset<T>(this GameObject fromObj, float offset,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T: IEaseTypeHint,new()
												=> await fromObj.transform.LocalYOffset( offset, duration,new T().FuncType(), ct, complete, repeat, delay, pingPong, customFunc);
		public static async UniTask LocalZOffset<T>(this GameObject fromObj, float offset,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T: IEaseTypeHint,new()
												=> await fromObj.transform.LocalZOffset( offset, duration,new T().FuncType(), ct, complete, repeat, delay, pingPong, customFunc);
		public static async UniTask XYTo<T>(this GameObject fromObj, Vector2 to, float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T: IEaseTypeHint,new()
												=> await fromObj.transform.XYTo( to, duration,new T().FuncType(), ct, complete, repeat, delay, pingPong, customFunc);
		public static async UniTask XYOffset<T>(this GameObject fromObj, Vector2 offset, float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T: IEaseTypeHint,new()
												=> await fromObj.transform.XYOffset( offset, duration,new T().FuncType(), ct, complete, repeat, delay, pingPong, customFunc);
		public static async UniTask LocalXYTo<T>(this GameObject fromObj, Vector2 to, float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T: IEaseTypeHint,new()
												=> await fromObj.transform.LocalXYTo( to, duration,new T().FuncType(), ct, complete, repeat, delay, pingPong, customFunc);
		public static async UniTask LocalXYOffset<T>(this GameObject fromObj, Vector2 offset, float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T: IEaseTypeHint,new()
												=> await fromObj.transform.LocalXYOffset( offset, duration,new T().FuncType(), ct, complete, repeat, delay, pingPong, customFunc);
		public static async UniTask XYZTo<T>(this GameObject fromObj, Vector3 to, float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T: IEaseTypeHint,new()
												=> await fromObj.transform.XYZTo( to, duration,new T().FuncType(), ct, complete, repeat, delay, pingPong, customFunc);
		public static async UniTask XYZOffset<T>(this GameObject fromObj, Vector3 offset, float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T: IEaseTypeHint,new()
												=> await fromObj.transform.XYZOffset( offset, duration,new T().FuncType(), ct, complete, repeat, delay, pingPong, customFunc);
		public static async UniTask LocalXYZTo<T>(this GameObject fromObj, Vector3 to, float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T: IEaseTypeHint,new()
												=> await fromObj.transform.LocalXYZTo( to, duration,new T().FuncType(), ct, complete, repeat, delay, pingPong, customFunc);
		public static async UniTask LocalXYZOffset<T>(this GameObject fromObj, Vector3 offset, float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T: IEaseTypeHint,new()
												=> await fromObj.transform.LocalXYZOffset( offset, duration,new T().FuncType(), ct, complete, repeat, delay, pingPong, customFunc);

		public static async UniTask RotXTo<T>(this GameObject fromObj, float toAngle,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T: IEaseTypeHint,new()
												=> await fromObj.transform.RotXTo( toAngle, duration,new T().FuncType(), ct, complete, repeat, delay, pingPong, customFunc);
		public static async UniTask RotYTo<T>(this GameObject fromObj, float toAngle,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T: IEaseTypeHint,new()
												=> await fromObj.transform.RotYTo( toAngle, duration,new T().FuncType(), ct, complete, repeat, delay, pingPong, customFunc);
		public static async UniTask RotZTo<T>(this GameObject fromObj, float toAngle,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T: IEaseTypeHint,new()
												=> await fromObj.transform.RotZTo( toAngle, duration,new T().FuncType(), ct, complete, repeat, delay, pingPong, customFunc);

		public static async UniTask LocalRotXTo<T>(this GameObject fromObj, float toAngle,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T: IEaseTypeHint,new()
												=> await fromObj.transform.LocalRotXTo( toAngle, duration,new T().FuncType(), ct, complete, repeat, delay, pingPong, customFunc);
		public static async UniTask LocalRotYTo<T>(this GameObject fromObj, float toAngle,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T: IEaseTypeHint,new()
												=> await fromObj.transform.LocalRotYTo( toAngle, duration,new T().FuncType(), ct, complete, repeat, delay, pingPong, customFunc);
		public static async UniTask LocalRotZTo<T>(this GameObject fromObj, float toAngle,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T: IEaseTypeHint,new()
												=> await fromObj.transform.LocalRotZTo( toAngle, duration,new T().FuncType(), ct, complete, repeat, delay, pingPong, customFunc);

		public static async UniTask ScaleXTo<T>(this GameObject fromObj, float to,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T: IEaseTypeHint,new()
												=> await fromObj.transform.ScaleXTo( to, duration,new T().FuncType(), ct, complete, repeat, delay, pingPong, customFunc);
		public static async UniTask ScaleYTo<T>(this GameObject fromObj, float to,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T: IEaseTypeHint,new()
												=> await fromObj.transform.ScaleYTo( to, duration,new T().FuncType(), ct, complete, repeat, delay, pingPong, customFunc);
		public static async UniTask ScaleZTo<T>(this GameObject fromObj, float to,  float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T: IEaseTypeHint,new()
												=> await fromObj.transform.ScaleZTo( to, duration,new T().FuncType(), ct, complete, repeat, delay, pingPong, customFunc);
		public static async UniTask ScaleXYTo<T>(this GameObject fromObj, Vector2 to, float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T: IEaseTypeHint,new()
												=> await fromObj.transform.ScaleXYTo( to, duration,new T().FuncType(), ct, complete, repeat, delay, pingPong, customFunc);
		public static async UniTask ScaleXYZTo<T>(this GameObject fromObj, Vector3 to, float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T: IEaseTypeHint,new()
												=> await fromObj.transform.ScaleXYZTo( to, duration,new T().FuncType(), ct, complete, repeat, delay, pingPong, customFunc);

		// SpriteRenderer
		public static async UniTask SizeTo<T>(this SpriteRenderer fromObj, Vector2 to, float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T: IEaseTypeHint,new()=>
					await fromObj.SizeTo( to, duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong, customFunc);

		public static async UniTask AlphaTo<T>(this Renderer fromObj, float to, float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T: IEaseTypeHint,new()=>
					await fromObj.AlphaTo( to, duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong, customFunc);

		public static async UniTask AlphaTo<T>(this Renderer fromObj, float from, float to, float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T: IEaseTypeHint,new()=>
					await fromObj.AlphaTo( from, to, duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong, customFunc);

		public static async UniTask AlphaTo<T>(this Graphic fromObj, float to, float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T: IEaseTypeHint,new()=>
					await fromObj.AlphaTo( to, duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong, customFunc);
		public static async UniTask AlphaTo<T>(this Graphic fromObj,float from, float to, float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T: IEaseTypeHint,new()=>
					await fromObj.AlphaTo( from,to, duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong, customFunc);
		public static async UniTask AlphaTo<T>(this Material fromObj, float to, float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T: IEaseTypeHint,new()=>
					await fromObj.AlphaTo( to, duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong, customFunc);
		public static async UniTask AlphaTo<T>(this Material fromObj, float from,float to, float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T: IEaseTypeHint,new()=>
					await fromObj.AlphaTo( from, to, duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong, customFunc);

		public static async UniTask ColorTo<T>(this Renderer fromObj, Vector3 to, float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T: IEaseTypeHint,new()=>
					await fromObj.ColorTo( to, duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong, customFunc);
		public static async UniTask ColorTo<T>(this Graphic fromObj, Vector3 to, float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T: IEaseTypeHint,new()=>
					await fromObj.ColorTo( to, duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong, customFunc);
		public static async UniTask ColorTo<T>(this SpriteRenderer fromObj, Color to, float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T: IEaseTypeHint,new()=>
					await fromObj.ColorTo( to, duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong, customFunc);

		public static async UniTask ColorTo<T>(this Renderer fromObj, Color to, float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T: IEaseTypeHint,new()=>
					await fromObj.ColorTo( to, duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong, customFunc);
		public static async UniTask ColorTo<T>(this Graphic fromObj, Color to, float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T: IEaseTypeHint,new()=>
					await fromObj.ColorTo( to, duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong, customFunc);
		public static async UniTask ColorTo<T>(this Material fromObj, Color to, float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T: IEaseTypeHint,new()=>
					await fromObj.ColorTo( to, duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong, customFunc);
		public static async UniTask ColorTo<T>(this Material fromObj, Color from,Color to, float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T: IEaseTypeHint,new()=>
					await fromObj.ColorTo( from, to, duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong, customFunc);

		public static async UniTask AlphaTo<T>(this CanvasGroup fromObj, float to, float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T: IEaseTypeHint,new()=>
					await fromObj.AlphaTo( to, duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong, customFunc);

		public static async UniTask AlphaTo<T>(this CanvasGroup fromObj,float from, float to, float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T: IEaseTypeHint,new()=>
					await fromObj.AlphaTo( from,to, duration,new T().FuncType(), ct, complete, repeat,  delay, pingPong, customFunc);

		public static async UniTask TextSend<T>(this TMP_Text fromObj, int fromIndex, int toIndex, float duration, CancellationToken? ct=null,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T: IEaseTypeHint,new()=>
					await fromObj.TextSend( fromIndex, toIndex, duration,new T().FuncType(), ct, complete, repeat, delay, pingPong, customFunc);
	#endregion UseGenericsEasing

	#region Tween
		// 汎用Tween
		public static async UniTask Tween(float from, float to, float duration, System.Action<float> update, EaseType type=EaseType.Default, CancellationToken ct=default,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
		{
			if ( type == EaseType.Default ) type = DefaultEasing;
			var func = (type == EaseType.Custom)?customFunc:Types[type];
			if ( func == null ) return;
			var updater = new FloatUpdater(from,to,func,update);
			await tweenBase( updater, duration, ct, complete, repeat, delay, pingPong );
		}

		public static async UniTask Tween(Vector2 from, Vector2 to, float duration, System.Action<Vector2> update, EaseType type=EaseType.Default, CancellationToken ct=default,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
		{
			if ( type == EaseType.Default ) type = DefaultEasing;
			var func = (type == EaseType.Custom)?customFunc:Types[type];
			if ( func == null ) return;
			var updater = new Vector2Updater(from,to,func,update);
			await tweenBase( updater, duration, ct, complete, repeat, delay, pingPong );
		}
		public static async UniTask Tween(Vector3 from, Vector3 to, float duration, System.Action<Vector3> update, EaseType type=EaseType.Default, CancellationToken ct=default,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
		{
			if ( type == EaseType.Default ) type = DefaultEasing;
			var func = (type == EaseType.Custom)?customFunc:Types[type];
			if ( func == null ) return;
			var updater = new Vector3Updater(from,to,func,update);
			await tweenBase( updater, duration, ct, complete, repeat, delay, pingPong );
		}

		public static async UniTask Tween(Vector4 from, Vector4 to, float duration, System.Action<Vector4> update, EaseType type=EaseType.Default, CancellationToken ct=default,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null)
		{
			if ( type == EaseType.Default ) type = DefaultEasing;
			var func = (type == EaseType.Custom)?customFunc:Types[type];
			if ( func == null ) return;
			var updater = new Vector4Updater(from,to,func,update);
			await tweenBase( updater, duration, ct, complete, repeat, delay, pingPong );
		}


		// 汎用ジェネリクス版
		public static async UniTask Tween<T>(float from, float to, float duration, System.Action<float> update, CancellationToken ct=default,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T : IEaseTypeHint,new()
		{
			var types = new T();
			var func = (types.FuncType() == EaseType.Custom)?customFunc:Types[types.FuncType()];
			var updater = new FloatUpdater(from,to,func,update);
			await tweenBase( updater, duration, ct, complete, repeat, delay, pingPong );
		}

		public static async UniTask Tween<T>(Vector2 from, Vector2 to, float duration, System.Action<Vector2> update, CancellationToken ct=default,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T : IEaseTypeHint,new()
		{
			var types = new T();
			var func = (types.FuncType() == EaseType.Custom)?customFunc:Types[types.FuncType()];
			var updater = new Vector2Updater(from,to,func,update);
			await tweenBase( updater, duration, ct, complete, repeat, delay, pingPong );
		}
		public static async UniTask Tween<T>(Vector3 from, Vector3 to, float duration, System.Action<Vector3> update, CancellationToken ct=default,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T : IEaseTypeHint,new()
		{
			var types = new T();
			var func = (types.FuncType() == EaseType.Custom)?customFunc:Types[types.FuncType()];
			var updater = new Vector3Updater(from,to,func,update);
			await tweenBase( updater, duration, ct, complete, repeat, delay, pingPong );
		}

		public static async UniTask Tween<T>(Vector4 from, Vector4 to, float duration, System.Action<Vector4> update, CancellationToken ct=default,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false,Interporate customFunc=null) where T : IEaseTypeHint,new()
		{
			var types = new T();
			var func = (types.FuncType() == EaseType.Custom)?customFunc:Types[types.FuncType()];
			var updater = new Vector4Updater(from,to,func,update);
			await tweenBase( updater, duration, ct, complete, repeat, delay, pingPong );
		}
	#endregion Tween

		// 補間アルゴリズム.
	#region Interporate
		private const float HalfPi = Mathf.PI * .5f;
		private const float DoublePi = Mathf.PI * 2f;

		public class Interporate
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
			static CalcLerp _instance = null;
			public static CalcLerp instance => _instance ?? (_instance = new ());
			public static new EaseType Type = EaseType.Linear;
			public override float Time(float time) => time;
		}

		// SineIn
		class CalcSineIn : Interporate{
			static CalcSineIn _instance = null;
			public static CalcSineIn instance => _instance ?? (_instance = new ());
			public static new EaseType Type = EaseType.SineIn;
			public override float Time(float time) => 1f - Mathf.Cos(time * HalfPi); 
		}

		// SineOut
		class CalcSineOut : Interporate{
			static CalcSineOut _instance = null;
			public static CalcSineOut instance => _instance ?? (_instance = new ());
			public static new EaseType Type = EaseType.SineOut;
			public override float Time(float time) => Mathf.Sin(time * HalfPi);
		}

		// SineInOut
		class CalcSineInOut : Interporate{
			static CalcSineInOut _instance = null;
			public static CalcSineInOut instance => _instance ?? (_instance = new ());
			public static new EaseType Type = EaseType.SineInOut;
			public override float Time(float time) => .5f * (1f - Mathf.Cos(Mathf.PI * time));
		}

		// QuadIn
		class CalcQuadIn : Interporate{
			static CalcQuadIn _instance = null;
			public static CalcQuadIn instance => _instance ?? (_instance = new ());
			public static new EaseType Type = EaseType.QuadIn;
			public override float Time(float time) => time * time;
		}

		// QuadOut
		class CalcQuadOut : Interporate{
			static CalcQuadOut _instance = null;
			public static CalcQuadOut instance => _instance ?? (_instance = new ());
			public static new EaseType Type = EaseType.QuadOut;
			public override float Time(float time) => -time * (time - 2f);
		}

		// QuadInOut
		class CalcQuadInOut : Interporate{
			static CalcQuadInOut _instance = null;
			public static CalcQuadInOut instance => _instance ?? (_instance = new ());
			public static new EaseType Type = EaseType.QuadInOut;
			public override float Time(float time){
				if ((time /= .5f) < 1f) return .5f * time * time;
				return -.5f * (((--time) * (time - 2f) - 1f));
			}
		}

		// CubicIn
		class CalcCubicIn : Interporate{
			static CalcCubicIn _instance = null;
			public static CalcCubicIn instance => _instance ?? (_instance = new ());
			public static new EaseType Type = EaseType.CubicIn;
			public override float Time(float time) => time * time * time;
		}

		// CubicOut
		class CalcCubicOut : Interporate{
			static CalcCubicOut _instance = null;
			public static CalcCubicOut instance => _instance ?? (_instance = new ());
			public static new EaseType Type = EaseType.CubicOut;
			public override float Time(float time) => (time -= 1f) * time * time + 1f;
		}

		// CubicInOut
		class CalcCubicInOut : Interporate{
			static CalcCubicInOut _instance = null;
			public static CalcCubicInOut instance => _instance ?? (_instance = new ());
			public static new EaseType Type = EaseType.CubicInOut;
			public override float Time(float time){
				if ((time /= .5f) < 1f) return .5f * time * time * time;
				return .5f * ((time -= 2) * time * time + 2f);
			}
		}
		// QuadIn
		class CalcQuartIn : Interporate{
			static CalcQuartIn _instance = null;
			public static CalcQuartIn instance => _instance ?? (_instance = new ());
			public static new EaseType Type = EaseType.QuartIn;
			public override float Time(float time) => time * time * time * time;
		}

		// QuartOut
		class CalcQuartOut : Interporate{
			static CalcQuartOut _instance = null;
			public static CalcQuartOut instance => _instance ?? (_instance = new ());
			public static new EaseType Type = EaseType.QuadOut;
			public override float Time(float time) => -((time -= 1f) * time * time * time - 1f);
		}

		// QuartInOut
		class CalcQuartInOut : Interporate{
			static CalcQuartInOut _instance = null;
			public static CalcQuartInOut instance => _instance ?? (_instance = new ());
			public static new EaseType Type = EaseType.QuartInOut;
			public override float Time(float time){
				if ((time /= .5f) < 1f) return .5f * time * time * time * time;
				return -.5f * ((time -= 2f) * time * time * time - 2f);
			}
		}

		// QuintIn
		class CalcQuintIn : Interporate{
			static CalcQuintIn _instance = null;
			public static CalcQuintIn instance => _instance ?? (_instance = new ());
			public static new EaseType Type = EaseType.QuintIn;
			public override float Time(float time) => time * time * time * time * time;
		}

		// QuintOut
		class CalcQuintOut : Interporate{
			static CalcQuintOut _instance = null;
			public static CalcQuintOut instance => _instance ?? (_instance = new ());
			public static new EaseType Type = EaseType.QuintOut;
			public override float Time(float time) => (time -= 1f) * time * time * time * time + 1f;
		}

		// QuintInOut
		class CalcQuintInOut : Interporate{
			static CalcQuintInOut _instance = null;
			public static CalcQuintInOut instance => _instance ?? (_instance = new ());
			public static new EaseType Type = EaseType.QuintInOut;
			public override float Time(float time){
				if ((time /= .5f) < 1f) return .5f * time * time * time * time * time;
				return .5f * ((time -= 2f) * time * time * time * time + 2f);
			}
		}

		// ExpoIn
		class CalcExpoIn : Interporate{
			static CalcExpoIn _instance = null;
			public static CalcExpoIn instance => _instance ?? (_instance = new ());
			public static new EaseType Type = EaseType.ExpoIn;
			public override float Time(float time) => Mathf.Pow(2f, 10f * (time - 1f));
		}

		// ExpoIn
		class CalcExpoOut : Interporate{
			static CalcExpoOut _instance = null;
			public static CalcExpoOut instance => _instance ?? (_instance = new ());
			public static new EaseType Type = EaseType.ExpoOut;
			public override float Time(float time) => -Mathf.Pow(2f, -10f * time) + 1f;
		}

		// ExpoInOut
		class CalcExpoInOut : Interporate{
			static CalcExpoInOut _instance = null;
			public static CalcExpoInOut instance => _instance ?? (_instance = new ());
			public static new EaseType Type = EaseType.ExpoInOut;
			public override float Time(float time){
				if ((time /= .5f) < 1f) return .5f * Mathf.Pow(2f, 10f * (time - 1f));
				return .5f * (-Mathf.Pow(2f, -10f * --time) + 2f);
			}
		}

		// CircIn
		class CalcCircIn : Interporate{
			static CalcCircIn _instance = null;
			public static CalcCircIn instance => _instance ?? (_instance = new ());
			public static new EaseType Type = EaseType.CircIn;
			public override float Time(float time) => -(Mathf.Sqrt(1f - time * time) - 1f);
		}

		// CircOut
		class CalcCircOut : Interporate{
			static CalcCircOut _instance = null;
			public static CalcCircOut instance => _instance ?? (_instance = new ());
			public static new EaseType Type = EaseType.CircOut;
			public override float Time(float time) => Mathf.Sqrt(1f - (time -= 1f) * time);
		}

		// CircInOut
		class CalcCircInOut : Interporate{
			static CalcCircInOut _instance = null;
			public static CalcCircInOut instance => _instance ?? (_instance = new ());
			public static new EaseType Type = EaseType.CircInOut;
			public override float Time(float time){
				if ((time /= .5f) < 1f) return -.5f * (Mathf.Sqrt(1f - time * time) - 1f);
				return .5f * (Mathf.Sqrt(1f - (time -= 2f) * time) + 1f);
			}
		}

		// BackIn
		class CalcBackIn : Interporate{
			static CalcBackIn _instance = null;
			public static CalcBackIn instance => _instance ?? (_instance = new ());
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
			static CalcBackOut _instance = null;
			public static CalcBackOut instance => _instance ?? (_instance = new ());
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
			static CalcBackInOut _instance = null;
			public static CalcBackInOut instance => _instance ?? (_instance = new ());
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
			static CalcElasticIn _instance = null;
			public static CalcElasticIn instance => _instance ?? (_instance = new ());
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
			static CalcElasticOut _instance = null;
			public static CalcElasticOut instance => _instance ?? (_instance = new ());
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
			static CalcElasticInOut _instance = null;
			public static CalcElasticInOut instance => _instance ?? (_instance = new ());
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
			static CalcBounceIn _instance = null;
			public static CalcBounceIn instance => _instance ?? (_instance = new ());
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
			static CalcBounceOut _instance = null;
			public static CalcBounceOut instance => _instance ?? (_instance = new ());
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
			static CalcBounceInOut _instance = null;
			public static CalcBounceInOut instance => _instance ?? (_instance = new ());
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
			static CalcSpring _instance = null;
			public static CalcSpring instance => _instance ?? (_instance = new ());
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


	#endregion Interporate

		// イージング関数.
	#region Easing
		public static float EaseLinear(float from,float to,float time) => CalcLerp.instance.Op(from,to,time);
		public static Vector2 EaseLinear(Vector2 from, Vector2 to,float time) => CalcLerp.instance.Op(from,to,time);
		public static Vector3 EaseLinear(Vector3 from, Vector3 to,float time) => CalcLerp.instance.Op(from,to,time);
		public static Vector4 EaseLinear(Vector4 from, Vector4 to,float time) => CalcLerp.instance.Op(from,to,time);

		public static float EaseSineIn(float from,float to,float time) => CalcSineIn.instance.Op(from,to,time);
		public static Vector2 EaseSineIn(Vector2 from, Vector2 to,float time) => CalcSineIn.instance.Op(from,to,time);
		public static Vector3 EaseSineIn(Vector3 from, Vector3 to,float time) => CalcSineIn.instance.Op(from,to,time);
		public static Vector4 EaseSineIn(Vector4 from, Vector4 to,float time) => CalcSineIn.instance.Op(from,to,time);

		public static float EaseSineOut(float from,float to,float time) => CalcSineOut.instance.Op(from,to,time);
		public static Vector2 EaseSineOut(Vector2 from, Vector2 to,float time) => CalcSineOut.instance.Op(from,to,time);
		public static Vector3 EaseSineOut(Vector3 from, Vector3 to,float time) => CalcSineOut.instance.Op(from,to,time);
		public static Vector4 EaseSineOut(Vector4 from, Vector4 to,float time) => CalcSineOut.instance.Op(from,to,time);

		public static float EaseSineInOut(float from,float to,float time) => CalcSineInOut.instance.Op(from,to,time);
		public static Vector2 EaseSineInOut(Vector2 from, Vector2 to,float time) => CalcSineInOut.instance.Op(from,to,time);
		public static Vector3 EaseSineInOut(Vector3 from, Vector3 to,float time) => CalcSineInOut.instance.Op(from,to,time);
		public static Vector4 EaseSineInOut(Vector4 from, Vector4 to,float time) => CalcSineInOut.instance.Op(from,to,time);

		public static float EaseQuadIn(float from,float to,float time) => CalcQuadIn.instance.Op(from,to,time);
		public static Vector2 EaseQuadIn(Vector2 from, Vector2 to,float time) => CalcQuadIn.instance.Op(from,to,time);
		public static Vector3 EaseQuadIn(Vector3 from, Vector3 to,float time) => CalcQuadIn.instance.Op(from,to,time);
		public static Vector4 EaseQuadIn(Vector4 from, Vector4 to,float time) => CalcQuadIn.instance.Op(from,to,time);

		public static float EaseQuadOut(float from,float to,float time) => CalcQuadOut.instance.Op(from,to,time);
		public static Vector2 EaseQuadOut(Vector2 from, Vector2 to,float time) => CalcQuadOut.instance.Op(from,to,time);
		public static Vector3 EaseQuadOut(Vector3 from, Vector3 to,float time) => CalcQuadOut.instance.Op(from,to,time);
		public static Vector4 EaseQuadOut(Vector4 from, Vector4 to,float time) => CalcQuadOut.instance.Op(from,to,time);

		public static float EaseQuadInOut(float from,float to,float time) => CalcQuadInOut.instance.Op(from,to,time);
		public static Vector2 EaseQuadInOut(Vector2 from, Vector2 to,float time) => CalcQuadInOut.instance.Op(from,to,time);
		public static Vector3 EaseQuadInOut(Vector3 from, Vector3 to,float time) => CalcQuadInOut.instance.Op(from,to,time);
		public static Vector4 EaseQuadInOut(Vector4 from, Vector4 to,float time) => CalcQuadInOut.instance.Op(from,to,time);

		public static float EaseCubicIn(float from,float to,float time) => CalcCubicIn.instance.Op(from,to,time);
		public static Vector2 EaseCubicIn(Vector2 from, Vector2 to,float time) => CalcCubicIn.instance.Op(from,to,time);
		public static Vector3 EaseCubicIn(Vector3 from, Vector3 to,float time) => CalcCubicIn.instance.Op(from,to,time);
		public static Vector4 EaseCubicIn(Vector4 from, Vector4 to,float time) => CalcCubicIn.instance.Op(from,to,time);

		public static float EaseCubicOut(float from,float to,float time) => CalcCubicOut.instance.Op(from,to,time);
		public static Vector2 EaseCubicOut(Vector2 from, Vector2 to,float time) => CalcCubicOut.instance.Op(from,to,time);
		public static Vector3 EaseCubicOut(Vector3 from, Vector3 to,float time) => CalcCubicOut.instance.Op(from,to,time);
		public static Vector4 EaseCubicOut(Vector4 from, Vector4 to,float time) => CalcCubicOut.instance.Op(from,to,time);

		public static float EaseCubicInOut(float from,float to,float time) => CalcCubicInOut.instance.Op(from,to,time);
		public static Vector2 EaseCubicInOut(Vector2 from, Vector2 to,float time) => CalcCubicInOut.instance.Op(from,to,time);
		public static Vector3 EaseCubicInOut(Vector3 from, Vector3 to,float time) => CalcCubicInOut.instance.Op(from,to,time);
		public static Vector4 EaseCubicInOut(Vector4 from, Vector4 to,float time) => CalcCubicInOut.instance.Op(from,to,time);

		public static float EaseQuartIn(float from,float to,float time) => CalcQuartIn.instance.Op(from,to,time);
		public static Vector2 EaseQuartIn(Vector2 from, Vector2 to,float time) => CalcQuartIn.instance.Op(from,to,time);
		public static Vector3 EaseQuartIn(Vector3 from, Vector3 to,float time) => CalcQuartIn.instance.Op(from,to,time);
		public static Vector4 EaseQuartIn(Vector4 from, Vector4 to,float time) => CalcQuartIn.instance.Op(from,to,time);

		public static float EaseQuartOut(float from,float to,float time) => CalcQuartOut.instance.Op(from,to,time);
		public static Vector2 EaseQuartOut(Vector2 from, Vector2 to,float time) => CalcQuartOut.instance.Op(from,to,time);
		public static Vector3 EaseQuartOut(Vector3 from, Vector3 to,float time) => CalcQuartOut.instance.Op(from,to,time);
		public static Vector4 EaseQuartOut(Vector4 from, Vector4 to,float time) => CalcQuartOut.instance.Op(from,to,time);

		public static float EaseQuartInOut(float from,float to,float time) => CalcQuartInOut.instance.Op(from,to,time);
		public static Vector2 EaseQuartInOut(Vector2 from, Vector2 to,float time) => CalcQuartInOut.instance.Op(from,to,time);
		public static Vector3 EaseQuartInOut(Vector3 from, Vector3 to,float time) => CalcQuartInOut.instance.Op(from,to,time);
		public static Vector4 EaseQuartInOut(Vector4 from, Vector4 to,float time) => CalcQuartInOut.instance.Op(from,to,time);

		public static float EaseQuintIn(float from,float to,float time) => CalcQuintIn.instance.Op(from,to,time);
		public static Vector2 EaseQuintIn(Vector2 from, Vector2 to,float time) => CalcQuintIn.instance.Op(from,to,time);
		public static Vector3 EaseQuintIn(Vector3 from, Vector3 to,float time) => CalcQuintIn.instance.Op(from,to,time);
		public static Vector4 EaseQuintIn(Vector4 from, Vector4 to,float time) => CalcQuintIn.instance.Op(from,to,time);

		public static float EaseQuintOut(float from,float to,float time) => CalcQuintOut.instance.Op(from,to,time);
		public static Vector2 EaseQuintOut(Vector2 from, Vector2 to,float time) => CalcQuintOut.instance.Op(from,to,time);
		public static Vector3 EaseQuintOut(Vector3 from, Vector3 to,float time) => CalcQuintOut.instance.Op(from,to,time);
		public static Vector4 EaseQuintOut(Vector4 from, Vector4 to,float time) => CalcQuintOut.instance.Op(from,to,time);

		public static float EaseQuintInOut(float from,float to,float time) => CalcQuintInOut.instance.Op(from,to,time);
		public static Vector2 EaseQuintInOut(Vector2 from, Vector2 to,float time) => CalcQuintInOut.instance.Op(from,to,time);
		public static Vector3 EaseQuintInOut(Vector3 from, Vector3 to,float time) => CalcQuintInOut.instance.Op(from,to,time);
		public static Vector4 EaseQuintInOut(Vector4 from, Vector4 to,float time) => CalcQuintInOut.instance.Op(from,to,time);

		public static float EaseExpoIn(float from,float to,float time) => CalcExpoIn.instance.Op(from,to,time);
		public static Vector2 EaseExpoIn(Vector2 from, Vector2 to,float time) => CalcExpoIn.instance.Op(from,to,time);
		public static Vector3 EaseExpoIn(Vector3 from, Vector3 to,float time) => CalcExpoIn.instance.Op(from,to,time);
		public static Vector4 EaseExpoIn(Vector4 from, Vector4 to,float time) => CalcExpoIn.instance.Op(from,to,time);

		public static float EaseExpoOut(float from,float to,float time) => CalcExpoOut.instance.Op(from,to,time);
		public static Vector2 EaseExpoOut(Vector2 from, Vector2 to,float time) => CalcExpoOut.instance.Op(from,to,time);
		public static Vector3 EaseExpoOut(Vector3 from, Vector3 to,float time) => CalcExpoOut.instance.Op(from,to,time);
		public static Vector4 EaseExpoOut(Vector4 from, Vector4 to,float time) => CalcExpoOut.instance.Op(from,to,time);

		public static float EaseExpoInOut(float from,float to,float time) => CalcExpoInOut.instance.Op(from,to,time);
		public static Vector2 EaseExpoInOut(Vector2 from, Vector2 to,float time) => CalcExpoInOut.instance.Op(from,to,time);
		public static Vector3 EaseExpoInOut(Vector3 from, Vector3 to,float time) => CalcExpoInOut.instance.Op(from,to,time);
		public static Vector4 EaseExpoInOut(Vector4 from, Vector4 to,float time) => CalcExpoInOut.instance.Op(from,to,time);

		public static float EaseCircIn(float from,float to,float time) => CalcCircIn.instance.Op(from,to,time);
		public static Vector2 EaseCircIn(Vector2 from, Vector2 to,float time) => CalcCircIn.instance.Op(from,to,time);
		public static Vector3 EaseCircIn(Vector3 from, Vector3 to,float time) => CalcCircIn.instance.Op(from,to,time);
		public static Vector4 EaseCircIn(Vector4 from, Vector4 to,float time) => CalcCircIn.instance.Op(from,to,time);

		public static float EaseCircOut(float from,float to,float time) => CalcCircOut.instance.Op(from,to,time);
		public static Vector2 EaseCircOut(Vector2 from, Vector2 to,float time) => CalcCircOut.instance.Op(from,to,time);
		public static Vector3 EaseCircOut(Vector3 from, Vector3 to,float time) => CalcCircOut.instance.Op(from,to,time);
		public static Vector4 EaseCircOut(Vector4 from, Vector4 to,float time) => CalcCircOut.instance.Op(from,to,time);

		public static float EaseCircInOut(float from,float to,float time) => CalcCircInOut.instance.Op(from,to,time);
		public static Vector2 EaseCircInOut(Vector2 from, Vector2 to,float time) => CalcCircInOut.instance.Op(from,to,time);
		public static Vector3 EaseCircInOut(Vector3 from, Vector3 to,float time) => CalcCircInOut.instance.Op(from,to,time);
		public static Vector4 EaseCircInOut(Vector4 from, Vector4 to,float time) => CalcCircInOut.instance.Op(from,to,time);

		public static float EaseBackIn(float from,float to,float time) => CalcBackIn.instance.Op(from,to,time);
		public static Vector2 EaseBackIn(Vector2 from, Vector2 to,float time) => CalcBackIn.instance.Op(from,to,time);
		public static Vector3 EaseBackIn(Vector3 from, Vector3 to,float time) => CalcBackIn.instance.Op(from,to,time);
		public static Vector4 EaseBackIn(Vector4 from, Vector4 to,float time) => CalcBackIn.instance.Op(from,to,time);

		public static float EaseBackOut(float from,float to,float time) => CalcBackOut.instance.Op(from,to,time);
		public static Vector2 EaseBackOut(Vector2 from, Vector2 to,float time) => CalcBackOut.instance.Op(from,to,time);
		public static Vector3 EaseBackOut(Vector3 from, Vector3 to,float time) => CalcBackOut.instance.Op(from,to,time);
		public static Vector4 EaseBackOut(Vector4 from, Vector4 to,float time) => CalcBackOut.instance.Op(from,to,time);

		public static float EaseBackInOut(float from,float to,float time) => CalcBackInOut.instance.Op(from,to,time);
		public static Vector2 EaseBackInOut(Vector2 from, Vector2 to,float time) => CalcBackInOut.instance.Op(from,to,time);
		public static Vector3 EaseBackInOut(Vector3 from, Vector3 to,float time) => CalcBackInOut.instance.Op(from,to,time);
		public static Vector4 EaseBackInOut(Vector4 from, Vector4 to,float time) => CalcBackInOut.instance.Op(from,to,time);

		public static float EaseElasticIn(float from,float to,float time) => CalcElasticIn.instance.Op(from,to,time);
		public static Vector2 EaseElasticIn(Vector2 from, Vector2 to,float time) => CalcElasticIn.instance.Op(from,to,time);
		public static Vector3 EaseElasticIn(Vector3 from, Vector3 to,float time) => CalcElasticIn.instance.Op(from,to,time);
		public static Vector4 EaseElasticIn(Vector4 from, Vector4 to,float time) => CalcElasticIn.instance.Op(from,to,time);

		public static float EaseElasticOut(float from,float to,float time) => CalcElasticOut.instance.Op(from,to,time);
		public static Vector2 EaseElasticOut(Vector2 from, Vector2 to,float time) => CalcElasticOut.instance.Op(from,to,time);
		public static Vector3 EaseElasticOut(Vector3 from, Vector3 to,float time) => CalcElasticOut.instance.Op(from,to,time);
		public static Vector4 EaseElasticOut(Vector4 from, Vector4 to,float time) => CalcElasticOut.instance.Op(from,to,time);

		public static float EaseElasticInOut(float from,float to,float time) => CalcElasticInOut.instance.Op(from,to,time);
		public static Vector2 EaseElasticInOut(Vector2 from, Vector2 to,float time) => CalcElasticInOut.instance.Op(from,to,time);
		public static Vector3 EaseElasticInOut(Vector3 from, Vector3 to,float time) => CalcElasticInOut.instance.Op(from,to,time);
		public static Vector4 EaseElasticInOut(Vector4 from, Vector4 to,float time) => CalcElasticInOut.instance.Op(from,to,time);

		public static float EaseBounceIn(float from,float to,float time) => CalcBounceIn.instance.Op(from,to,time);
		public static Vector2 EaseBounceIn(Vector2 from, Vector2 to,float time) => CalcBounceIn.instance.Op(from,to,time);
		public static Vector3 EaseBounceIn(Vector3 from, Vector3 to,float time) => CalcBounceIn.instance.Op(from,to,time);
		public static Vector4 EaseBounceIn(Vector4 from, Vector4 to,float time) => CalcBounceIn.instance.Op(from,to,time);

		public static float EaseBounceOut(float from,float to,float time) => CalcBounceOut.instance.Op(from,to,time);
		public static Vector2 EaseBounceOut(Vector2 from, Vector2 to,float time) => CalcBounceOut.instance.Op(from,to,time);
		public static Vector3 EaseBounceOut(Vector3 from, Vector3 to,float time) => CalcBounceOut.instance.Op(from,to,time);
		public static Vector4 EaseBounceOut(Vector4 from, Vector4 to,float time) => CalcBounceOut.instance.Op(from,to,time);

		public static float EaseBounceInOut(float from,float to,float time) => CalcBounceInOut.instance.Op(from,to,time);
		public static Vector2 EaseBounceInOut(Vector2 from, Vector2 to,float time) => CalcBounceInOut.instance.Op(from,to,time);
		public static Vector3 EaseBounceInOut(Vector3 from, Vector3 to,float time) => CalcBounceInOut.instance.Op(from,to,time);
		public static Vector4 EaseBounceInOut(Vector4 from, Vector4 to,float time) => CalcBounceInOut.instance.Op(from,to,time);

		public static float EaseSpring(float from,float to,float time) => CalcSpring.instance.Op(from,to,time);
		public static Vector2 EaseSpring(Vector2 from, Vector2 to,float time) => CalcSpring.instance.Op(from,to,time);
		public static Vector3 EaseSpring(Vector3 from, Vector3 to,float time) => CalcSpring.instance.Op(from,to,time);
		public static Vector4 EaseSpring(Vector4 from, Vector4 to,float time) => CalcSpring.instance.Op(from,to,time);

	#endregion Easing


	}
}
