using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System.Threading;
using TMPro;

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
		Spring
	}

	public static class Tweener
	{
		readonly static Dictionary<EaseType, Interporate > Types = new Dictionary<EaseType, Interporate>
		{
			{EaseType.Linear, new Lerp()},
			{EaseType.SineIn, new SineIn()}, {EaseType.SineOut, new SineOut()}, {EaseType.SineInOut, new SineInOut()},
			{EaseType.QuadIn, new QuadIn()}, {EaseType.QuadOut, new QuadOut()}, {EaseType.QuadInOut, new QuadInOut()},
			{EaseType.CubicIn, new CubicIn()}, {EaseType.CubicOut, new CubicOut()}, {EaseType.CubicInOut, new CubicInOut()},
			{EaseType.QuartIn, new QuartIn()}, {EaseType.QuartOut, new QuartOut()}, {EaseType.QuartInOut, new QuartInOut()},
			{EaseType.QuintIn, new QuintIn()}, {EaseType.QuintOut, new QuintOut()}, {EaseType.QuintInOut, new QuintInOut()},
			{EaseType.ExpoIn, new ExpoIn()}, {EaseType.ExpoOut, new ExpoOut()}, {EaseType.ExpoInOut, new ExpoInOut()},
			{EaseType.CircIn, new CircIn()}, {EaseType.CircOut, new CircOut()}, {EaseType.CircInOut, new CircInOut()},
			{EaseType.BackIn, new BackIn()}, {EaseType.BackOut, new BackOut()}, {EaseType.BackInOut, new BackInOut()},
			{EaseType.ElasticIn, new ElasticIn()}, {EaseType.ElasticOut, new ElasticOut()}, {EaseType.ElasticInOut, new ElasticInOut()},
			{EaseType.BounceIn, new BounceIn()}, {EaseType.BounceOut, new BounceOut()}, {EaseType.BounceInOut, new BounceInOut()},
			{EaseType.Spring, new Spring()}
		};

		interface IUpdaterBase {
			void Update(float t);
			void ReverseUpdate(float t);
		}

		class UpdaterParam<T> {
			protected T from;
			protected T to;
			protected System.Action<T> update;
			protected Interporate op;
			protected UpdaterParam( T from, T to, Interporate op,System.Action<T> update){
				this.from = from;
				this.to = to;
				this.update = update;
				this.op = op;
			}
		}

		class FloatUpdater : UpdaterParam<float>,IUpdaterBase
		{
			public FloatUpdater(float from, float to, Interporate op,System.Action<float> update ) : base(from, to, op, update){}
			public void Update(float t) => update.Invoke(op.Op(from, to, Mathf.Clamp01(t)));
			public void ReverseUpdate(float t) => update.Invoke(op.Op(to, from, Mathf.Clamp01(t)));
		}

		class Vector2Updater : UpdaterParam<Vector2>, IUpdaterBase
		{
			public Vector2Updater(Vector2 from, Vector2 to, Interporate op,System.Action<Vector2> update) : base(from, to, op, update){	}
			public void Update(float t) => update.Invoke(op.Op(from, to, Mathf.Clamp01(t)));
			public void ReverseUpdate(float t) => update.Invoke(op.Op(to, from, Mathf.Clamp01(t)));
		}

		class Vector3Updater : UpdaterParam<Vector3>, IUpdaterBase
		{
			public Vector3Updater(Vector3 from, Vector3 to, Interporate op,System.Action<Vector3> update) : base(from, to, op, update){	}
			public void Update(float t) => update.Invoke(op.Op(from, to, Mathf.Clamp01(t)));
			public void ReverseUpdate(float t) => update.Invoke(op.Op(to, from, Mathf.Clamp01(t)));
		}

		class Vector4Updater : UpdaterParam<Vector4>, IUpdaterBase
		{
			public Vector4Updater(Vector4 from, Vector4 to, Interporate op,System.Action<Vector4> update) : base(from, to, op, update){	}
			public void Update(float t) => update.Invoke(op.Op(from, to, Mathf.Clamp01(t)));
			public void ReverseUpdate(float t) => update.Invoke(op.Op(to, from, Mathf.Clamp01(t)));
		}

		static async UniTask tweenBase(IUpdaterBase updater, float duration, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null,int repeat=0, float delay=0, bool pingPong=false)
		{
			var counter = repeat+1;
			var func = Types[type];
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

		// Transform
		public static async UniTask XTo(this Transform transform, float to,  float duration, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			await Tween( transform.position.x, to, duration, (newPos)=>{ var p = transform.position; p.x = newPos; transform.position = p; },
												type, ct, complete, repeat, delay, pingPong );
		}
		public static async UniTask YTo(this Transform transform, float to,  float duration, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			await Tween( transform.position.y, to, duration, (newPos)=>{ var p = transform.position; p.y = newPos; transform.position = p; },
												type, ct, complete, repeat, delay, pingPong );
		}
		public static async UniTask ZTo(this Transform transform, float to,  float duration, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			await Tween( transform.position.z, to, duration, (newPos)=>{ var p = transform.position; p.z = newPos; transform.position = p; },
												type, ct, complete, repeat, delay, pingPong );
		}
		public static async UniTask XOffset(this Transform transform, float offset,  float duration, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			await transform.XTo( transform.position.x+offset, duration, type, ct, complete, repeat, delay, pingPong);
		}
		public static async UniTask YOffset(this Transform transform, float offset,  float duration, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			await transform.YTo( transform.position.y+offset,duration, type, ct, complete, repeat, delay, pingPong);
		}
		public static async UniTask ZOffset(this Transform transform, float offset,  float duration, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			await transform.ZTo( transform.position.z+offset, duration, type, ct, complete, repeat, delay, pingPong);
		}

		public static async UniTask LocalXTo(this Transform transform, float to,  float duration, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			await Tween( transform.localPosition.x, to, duration, (newPos)=>{ var p = transform.localPosition; p.x = newPos; transform.localPosition = p; },
												type, ct, complete, repeat, delay, pingPong );
		}
		public static async UniTask LocalYTo(this Transform transform, float to,  float duration, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			await Tween( transform.localPosition.y, to, duration, (newPos)=>{ var p = transform.localPosition; p.y = newPos; transform.localPosition = p; },
												type, ct, complete, repeat, delay, pingPong );
		}
		public static async UniTask LocalZTo(this Transform transform, float to,  float duration, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			await Tween( transform.localPosition.z, to, duration, (newPos)=>{ var p = transform.localPosition; p.z = newPos; transform.localPosition = p; },
												type, ct, complete, repeat, delay, pingPong );
		}
		public static async UniTask LocalXOffset(this Transform transform, float offset,  float duration, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			await transform.LocalXTo( transform.localPosition.x+offset, duration, type, ct, complete, repeat, delay, pingPong);
		}
		public static async UniTask LocalYOffset(this Transform transform, float offset,  float duration, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			await transform.LocalYTo( transform.localPosition.y+offset, duration, type, ct, complete, repeat, delay, pingPong);
		}
		public static async UniTask LocalZOffset(this Transform transform, float offset,  float duration, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			await transform.LocalZTo( transform.localPosition.z+offset, duration, type, ct, complete, repeat, delay, pingPong);
		}

		public static async UniTask XYTo(this Transform transform, Vector2 to, float duration, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			await Tween( (Vector2)transform.position, to, duration, (newPos)=>{ var p = transform.position; p.x = newPos.x; p.y = newPos.y; transform.position = p; },
												type, ct, complete, repeat, delay, pingPong );
		}
		public static async UniTask XYOffset(this Transform transform, Vector2 offset, float duration, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			await transform.XYTo( (Vector2)transform.localPosition+offset, duration, type, ct, complete, repeat, delay, pingPong);
		}

		public static async UniTask LocalXYTo(this Transform transform, Vector2 to, float duration, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			await Tween( (Vector2)transform.localPosition, to, duration, (newPos)=>{ var p = transform.localPosition; p.x = newPos.x; p.y = newPos.y; transform.localPosition = p; },
												type, ct, complete, repeat, delay, pingPong );
		}
		public static async UniTask LocalXYOffset(this Transform transform, Vector2 offset, float duration, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{

			await transform.LocalXYTo( (Vector2)transform.localPosition+offset, duration, type, ct, complete, repeat, delay, pingPong);
		}

		public static async UniTask XYZTo(this Transform transform, Vector3 to, float duration, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			await Tween( transform.position, to, duration, (newPos)=>{ transform.position = newPos; },
												type, ct, complete, repeat, delay, pingPong );
		}
		public static async UniTask XYZOffset(this Transform transform, Vector3 offset, float duration, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			await transform.XYZTo( transform.localPosition+offset, duration, type, ct, complete, repeat, delay, pingPong);
		}
		public static async UniTask LocalXYZTo(this Transform transform, Vector3 to, float duration, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			await Tween( transform.localPosition, to, duration, (newPos)=>{ transform.localPosition = newPos; },
												type, ct, complete, repeat, delay, pingPong );
		}

		public static async UniTask LocalXYZOffset(this Transform transform, Vector3 offset, float duration, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			await transform.LocalXYZTo( transform.localPosition+offset, duration, type, ct, complete, repeat, delay, pingPong);
		}

		public static async UniTask LocalRotXTo(this Transform transform, float toAngle,  float duration, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			await Tween( transform.localEulerAngles.x, toAngle, duration, (newPos)=>{ var p = transform.localEulerAngles; p.x = newPos; transform.localEulerAngles = p;},
												type, ct, complete, repeat, delay, pingPong );
		}
		public static async UniTask LocalRotYTo(this Transform transform, float toAngle,  float duration, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			await Tween( transform.localEulerAngles.y, toAngle, duration, (newPos)=>{ var p = transform.localEulerAngles; p.y = newPos; transform.localEulerAngles = p; },
												type, ct, complete, repeat, delay, pingPong );
		}
		public static async UniTask LocalRotZTo(this Transform transform, float toAngle,  float duration, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			await Tween( transform.localEulerAngles.z, toAngle, duration, (newPos)=>{ var p = transform.localEulerAngles; p.z = newPos; transform.localEulerAngles = p; },
												type, ct, complete, repeat, delay, pingPong );
		}

		public static async UniTask RotXTo(this Transform transform, float toAngle,  float duration, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			await Tween( transform.eulerAngles.x, toAngle, duration, (newPos)=>{ var p = transform.eulerAngles; p.x = newPos; transform.eulerAngles = p; },
												type, ct, complete, repeat, delay, pingPong );
		}
		public static async UniTask RotYTo(this Transform transform, float toAngle,  float duration, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			await Tween( transform.eulerAngles.y, toAngle, duration, (newPos)=>{ var p = transform.eulerAngles; p.y = newPos; transform.eulerAngles = p; },
												type, ct, complete, repeat, delay, pingPong );
		}
		public static async UniTask RotZTo(this Transform transform, float toAngle,  float duration, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			await Tween( transform.eulerAngles.z, toAngle, duration, (newPos)=>{ var p = transform.eulerAngles; p.z = newPos; transform.eulerAngles = p; },
												type, ct, complete, repeat, delay, pingPong );
		}

		public static async UniTask ScaleXTo(this Transform transform, float to,  float duration, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			await Tween( transform.localScale.x, to, duration, (newPos)=>{ var p = transform.localScale; p.x = newPos; transform.localScale = p; },
												type, ct, complete, repeat, delay, pingPong );
		}
		public static async UniTask ScaleYTo(this Transform transform, float to,  float duration, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			await Tween( transform.localScale.y, to, duration, (newPos)=>{ var p = transform.localScale; p.y = newPos; transform.localScale = p; },
												type, ct, complete, repeat, delay, pingPong );
		}
		public static async UniTask ScaleZTo(this Transform transform, float to,  float duration, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			await Tween( transform.localScale.z, to, duration, (newPos)=>{ var p = transform.localScale; p.z = newPos; transform.localScale = p; },
												type, ct, complete, repeat, delay, pingPong );
		}

		public static async UniTask ScaleXYTo(this Transform transform, Vector2 to, float duration, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			await Tween( (Vector2)transform.localScale, to, duration, (newPos)=>{ var p = transform.localScale; p.x = newPos.x; p.y = newPos.y; transform.localScale = p; },
												type, ct, complete, repeat, delay, pingPong );
		}

		public static async UniTask ScaleXYZTo(this Transform transform, Vector3 to, float duration, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			await Tween( transform.localScale, to, duration, (newPos)=>{ transform.localScale = newPos; },
												type, ct, complete, repeat, delay, pingPong );
		}


		// GameObject
		public static async UniTask XTo(this GameObject fromObj, float to,  float duration, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.XTo( to, duration, type, ct, complete, repeat, delay, pingPong);
		public static async UniTask YTo(this GameObject fromObj, float to,  float duration, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.YTo( to, duration, type, ct, complete, repeat, delay, pingPong);
		public static async UniTask ZTo(this GameObject fromObj, float to,  float duration, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.ZTo( to, duration, type, ct, complete, repeat, delay, pingPong);
		public static async UniTask XOffset(this GameObject fromObj, float offset,  float duration, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.XOffset( offset, duration, type, ct, complete, repeat, delay, pingPong);
		public static async UniTask YOffset(this GameObject fromObj, float offset,  float duration, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.YOffset( offset, duration, type, ct, complete, repeat, delay, pingPong);
		public static async UniTask ZOffset(this GameObject fromObj, float offset,  float duration, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.ZOffset( offset, duration, type, ct, complete, repeat, delay, pingPong);
		public static async UniTask LocalXTo(this GameObject fromObj, float to,  float duration, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.LocalXTo( to, duration, type, ct, complete, repeat, delay, pingPong);
		public static async UniTask LocalYTo(this GameObject fromObj, float to,  float duration, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.LocalYTo( to, duration, type, ct, complete, repeat, delay, pingPong);
		public static async UniTask LocalZTo(this GameObject fromObj, float to,  float duration, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.LocalZTo( to, duration, type, ct, complete, repeat, delay, pingPong);
		public static async UniTask LocalXOffset(this GameObject fromObj, float offset,  float duration, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.LocalXOffset( offset, duration, type, ct, complete, repeat, delay, pingPong);
		public static async UniTask LocalYOffset(this GameObject fromObj, float offset,  float duration, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.LocalYOffset( offset, duration, type, ct, complete, repeat, delay, pingPong);
		public static async UniTask LocalZOffset(this GameObject fromObj, float offset,  float duration, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.LocalZOffset( offset, duration, type, ct, complete, repeat, delay, pingPong);
		public static async UniTask XYTo(this GameObject fromObj, Vector2 to, float duration, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.XYTo( to, duration, type, ct, complete, repeat, delay, pingPong);
		public static async UniTask XYOffset(this GameObject fromObj, Vector2 offset, float duration, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.XYOffset( offset, duration, type, ct, complete, repeat, delay, pingPong);
		public static async UniTask LocalXYTo(this GameObject fromObj, Vector2 to, float duration, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.LocalXYTo( to, duration, type, ct, complete, repeat, delay, pingPong);
		public static async UniTask LocalXYOffset(this GameObject fromObj, Vector2 offset, float duration, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.LocalXYOffset( offset, duration, type, ct, complete, repeat, delay, pingPong);
		public static async UniTask XYZTo(this GameObject fromObj, Vector3 to, float duration, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.XYZTo( to, duration, type, ct, complete, repeat, delay, pingPong);
		public static async UniTask XYZOffset(this GameObject fromObj, Vector3 offset, float duration, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.XYZOffset( offset, duration, type, ct, complete, repeat, delay, pingPong);
		public static async UniTask LocalXYZTo(this GameObject fromObj, Vector3 to, float duration, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.LocalXYZTo( to, duration, type, ct, complete, repeat, delay, pingPong);
		public static async UniTask LocalXYZOffset(this GameObject fromObj, Vector3 offset, float duration, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.LocalXYZOffset( offset, duration, type, ct, complete, repeat, delay, pingPong);

		public static async UniTask RotXTo(this GameObject fromObj, float toAngle,  float duration, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.RotXTo( toAngle, duration, type, ct, complete, repeat, delay, pingPong);
		public static async UniTask RotYTo(this GameObject fromObj, float toAngle,  float duration, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.RotYTo( toAngle, duration, type, ct, complete, repeat, delay, pingPong);
		public static async UniTask RotZTo(this GameObject fromObj, float toAngle,  float duration, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.RotZTo( toAngle, duration, type, ct, complete, repeat, delay, pingPong);

		public static async UniTask LocalRotXTo(this GameObject fromObj, float toAngle,  float duration, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.LocalRotXTo( toAngle, duration, type, ct, complete, repeat, delay, pingPong);
		public static async UniTask LocalRotYTo(this GameObject fromObj, float toAngle,  float duration, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.LocalRotYTo( toAngle, duration, type, ct, complete, repeat, delay, pingPong);
		public static async UniTask LocalRotZTo(this GameObject fromObj, float toAngle,  float duration, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.LocalRotZTo( toAngle, duration, type, ct, complete, repeat, delay, pingPong);

		public static async UniTask ScaleXTo(this GameObject fromObj, float to,  float duration, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.ScaleXTo( to, duration, type, ct, complete, repeat, delay, pingPong);
		public static async UniTask ScaleYTo(this GameObject fromObj, float to,  float duration, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.ScaleYTo( to, duration, type, ct, complete, repeat, delay, pingPong);
		public static async UniTask ScaleZTo(this GameObject fromObj, float to,  float duration, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.ScaleZTo( to, duration, type, ct, complete, repeat, delay, pingPong);
		public static async UniTask ScaleXYTo(this GameObject fromObj, Vector2 to, float duration, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.ScaleXYTo( to, duration, type, ct, complete, repeat, delay, pingPong);
		public static async UniTask ScaleXYZTo(this GameObject fromObj, Vector3 to, float duration, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.ScaleXYZTo( to, duration, type, ct, complete, repeat, delay, pingPong);

		// SpriteRenderer
		public static async UniTask SizeTo(this SpriteRenderer fromObj, Vector2 to, float duration, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			await Tween( (Vector2)fromObj.size, to, duration, (newPos)=>{ fromObj.size = newPos; },
												type, ct, complete, repeat, delay, pingPong );
		}

		public static async UniTask AlphaTo(this Renderer fromObj, float to, float duration, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			await Tween( fromObj.material.color.a, to, duration, (newPos)=>{ var col = fromObj.material.color; col.a = newPos; fromObj.material.color = col; },
												type, ct, complete, repeat, delay, pingPong );
		}

		public static async UniTask AlphaTo(this Renderer fromObj, float from, float to, float duration, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			var col = fromObj.material.color;
			col.a = from;
			fromObj.material.color = col;
			await Tween( from, to, duration, (newPos)=>{ var col = fromObj.material.color; col.a = newPos; fromObj.material.color = col; },
												type, ct, complete, repeat, delay, pingPong );
		}

		public static async UniTask AlphaTo(this Graphic fromObj, float to, float duration, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{

			await Tween( fromObj.color.a, to, duration, (newPos)=>{ var col = fromObj.color; col.a = newPos; fromObj.color = col; },
												type, ct, complete, repeat, delay, pingPong );
		}
		public static async UniTask AlphaTo(this Graphic fromObj,float from, float to, float duration, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			var col = fromObj.color;
			col.a = from;
			fromObj.color = col;
			await Tween( fromObj.color.a, to, duration, (newPos)=>{ var col = fromObj.color; col.a = newPos; fromObj.color = col; },
												type, ct, complete, repeat, delay, pingPong );
		}

		public static async UniTask AlphaTo(this Material fromObj, float to, float duration, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			await Tween( fromObj.color.a, to, duration, (newPos)=>{ var col = fromObj.color; col.a = newPos; fromObj.color = col; },
												type, ct, complete, repeat, delay, pingPong );
		}
		public static async UniTask AlphaTo(this Material fromObj, float from,float to, float duration, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			var col = fromObj.color;
			col.a = from;
			fromObj.color = col;
			await Tween( fromObj.color.a, to, duration, (newPos)=>{ var col = fromObj.color; col.a = newPos; fromObj.color = col; },
												type, ct, complete, repeat, delay, pingPong );
		}


		public static async UniTask ColorTo(this Renderer fromObj, Vector3 to, float duration, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{

			await Tween( fromObj.material.color, to, duration,
								(newPos)=>{ fromObj.material.color = new Color(newPos.x,newPos.y,newPos.z,fromObj.material.color.a);  },
								type, ct, complete, repeat, delay, pingPong );
		}

		public static async UniTask ColorTo(this Graphic fromObj, Vector3 to, float duration, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			await Tween( fromObj.color, to, duration,
								(newPos)=>{ fromObj.color = new Color(newPos.x,newPos.y,newPos.z,fromObj.color.a);  },
								type, ct, complete, repeat, delay, pingPong );
		}

		public static async UniTask ColorTo(this SpriteRenderer fromObj, Color to, float duration, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{

			await Tween( fromObj.color, to, duration,
								(newPos)=>{ fromObj.color = newPos; },
								type, ct, complete, repeat, delay, pingPong );
		}

		public static async UniTask ColorTo(this Renderer fromObj, Color to, float duration, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{

			await Tween( fromObj.material.color, to, duration,
								(newPos)=>{ fromObj.material.color = newPos; },
								type, ct, complete, repeat, delay, pingPong );
		}

		public static async UniTask ColorTo(this Graphic fromObj, Color to, float duration, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			await Tween( fromObj.color, to, duration,
								(newPos)=>{ fromObj.color = newPos; },
								type, ct, complete, repeat, delay, pingPong );
		}

		public static async UniTask ColorTo(this Material fromObj, Color to, float duration, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			await Tween( fromObj.color, to, duration,
								(newPos)=>{ fromObj.color = newPos; },
								type, ct, complete, repeat, delay, pingPong );
		}
		public static async UniTask ColorTo(this Material fromObj, Color from,Color to, float duration, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			fromObj.color = from;
			await Tween( fromObj.color, to, duration,
								(newPos)=>{ fromObj.color = newPos; },
								type, ct, complete, repeat, delay, pingPong );
		}

		public static async UniTask AlphaTo(this CanvasGroup fromObj, float to, float duration, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{

			await Tween( fromObj.alpha, to, duration, (newPos)=>{ var col = fromObj.alpha = newPos; },
												type, ct, complete, repeat, delay, pingPong );
		}

		public static async UniTask AlphaTo(this CanvasGroup fromObj,float from, float to, float duration, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			fromObj.alpha = from;
			await Tween( fromObj.alpha, to, duration, (newPos)=>{ var col = fromObj.alpha = newPos; },
												type, ct, complete, repeat, delay, pingPong );
		}

		public static async UniTask TextSend(this TMP_Text fromObj, int fromIndex, int toIndex, float duration, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			var len = fromObj.text.Length; // @todo:本当はGetParsedTextを使いたいが1フレ待つ必要があるためLengthにする.
			fromIndex = Mathf.Clamp(fromIndex,0,len);
			toIndex = Mathf.Clamp(toIndex+1,0,len+1); // 最後の1文字がintだと頭しか表示されないので+1する.
			await Tween( (float)fromIndex, (float)toIndex, duration,
								(newPos)=>{ fromObj.maxVisibleCharacters = (int)Mathf.Clamp(newPos,0,len); },
								type, ct, complete, repeat, delay, pingPong );
		}



		// CancellationToken無し（GameObjectから取得する)
		static CancellationToken getCT(GameObject go) => go?go.GetCancellationTokenOnDestroy():CancellationToken.None;
		static CancellationToken getCT(Transform t) => getCT(t.gameObject);
		static CancellationToken getCT(Renderer r) => getCT(r.gameObject);
		static CancellationToken getCT(Graphic r) => getCT(r.gameObject);
		static CancellationToken getCT(SpriteRenderer r) => getCT(r.gameObject);
		static CancellationToken getCT(CanvasGroup g) => getCT(g.gameObject);
		static CancellationToken getCT(TMP_Text t) => getCT(t.gameObject);
		static CancellationToken getCT(Material m) => CancellationToken.None;
		public static async UniTask XTo(this Transform transform, float to,  float duration, EaseType type,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)=>
					await transform.XTo( to,  duration, type, getCT(transform), complete, repeat,  delay, pingPong);
		public static async UniTask YTo(this Transform transform, float to,  float duration, EaseType type,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)=>
					await transform.YTo( to,  duration, type, getCT(transform), complete, repeat,  delay, pingPong);
		public static async UniTask ZTo(this Transform transform, float to,  float duration, EaseType type,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)=>
					await transform.ZTo( to,  duration, type, getCT(transform), complete, repeat,  delay, pingPong);
		public static async UniTask XOffset(this Transform transform, float offset,  float duration, EaseType type,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)=>
					await transform.XOffset( offset,  duration, type, getCT(transform), complete, repeat,  delay, pingPong);
		public static async UniTask YOffset(this Transform transform, float offset,  float duration, EaseType type,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)=>
					await transform.YOffset( offset,  duration, type, getCT(transform), complete, repeat,  delay, pingPong);
		public static async UniTask ZOffset(this Transform transform, float offset,  float duration, EaseType type,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)=>
					await transform.ZOffset( offset,  duration, type, getCT(transform), complete, repeat,  delay, pingPong);

		public static async UniTask LocalXTo(this Transform transform, float to,  float duration, EaseType type,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)=>
					await transform.LocalXTo( to,  duration, type, getCT(transform), complete, repeat,  delay, pingPong);
		public static async UniTask LocalYTo(this Transform transform, float to,  float duration, EaseType type,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)=>
					await transform.LocalYTo( to,  duration, type, getCT(transform), complete, repeat,  delay, pingPong);
		public static async UniTask LocalZTo(this Transform transform, float to,  float duration, EaseType type,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)=>
					await transform.LocalZTo( to,  duration, type, getCT(transform), complete, repeat,  delay, pingPong);
		public static async UniTask LocalXOffset(this Transform transform, float offset,  float duration, EaseType type,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)=>
					await transform.LocalXOffset( offset,  duration, type, getCT(transform), complete, repeat,  delay, pingPong);
		public static async UniTask LocalYOffset(this Transform transform, float offset,  float duration, EaseType type,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)=>
					await transform.LocalYOffset( offset,  duration, type, getCT(transform), complete, repeat,  delay, pingPong);
		public static async UniTask LocalZOffset(this Transform transform, float offset,  float duration, EaseType type,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)=>
					await transform.LocalZOffset( offset,  duration, type, getCT(transform), complete, repeat,  delay, pingPong);

		public static async UniTask XYTo(this Transform transform, Vector2 to, float duration, EaseType type,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)=>
					await transform.XYTo( to, duration, type, getCT(transform), complete, repeat,  delay, pingPong);
		public static async UniTask XYOffset(this Transform transform, Vector2 offset, float duration, EaseType type,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)=>
					await transform.XYOffset( offset, duration, type, getCT(transform), complete, repeat,  delay, pingPong);

		public static async UniTask LocalXYTo(this Transform transform, Vector2 to, float duration, EaseType type,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)=>
					await transform.LocalXYTo( to, duration, type, getCT(transform), complete, repeat,  delay, pingPong);
		public static async UniTask LocalXYOffset(this Transform transform, Vector2 offset, float duration, EaseType type,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)=>
					await transform.LocalXYOffset( offset, duration, type, getCT(transform), complete, repeat,  delay, pingPong);
		public static async UniTask XYZTo(this Transform transform, Vector3 to, float duration, EaseType type,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)=>
					await transform.XYZTo( to, duration, type, getCT(transform), complete, repeat,  delay, pingPong);
		public static async UniTask XYZOffset(this Transform transform, Vector3 offset, float duration, EaseType type,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)=>
					await transform.XYZOffset( offset, duration, type, getCT(transform), complete, repeat,  delay, pingPong);
		public static async UniTask LocalXYZTo(this Transform transform, Vector3 to, float duration, EaseType type,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)=>
					await transform.LocalXYZTo( to, duration, type, getCT(transform), complete, repeat,  delay, pingPong);

		public static async UniTask LocalXYZOffset(this Transform transform, Vector3 offset, float duration, EaseType type,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)=>
					await transform.LocalXYZOffset( offset, duration, type, getCT(transform), complete, repeat,  delay, pingPong);

		public static async UniTask LocalRotXTo(this Transform transform, float toAngle,  float duration, EaseType type,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)=>
					await transform.LocalRotXTo( toAngle, duration, type, getCT(transform), complete, repeat,  delay, pingPong);
		public static async UniTask LocalRotYTo(this Transform transform, float toAngle,  float duration, EaseType type,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)=>
					await transform.LocalRotYTo( toAngle, duration, type, getCT(transform), complete, repeat,  delay, pingPong);
		public static async UniTask LocalRotZTo(this Transform transform, float toAngle,  float duration, EaseType type,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)=>
					await transform.LocalRotZTo( toAngle, duration, type, getCT(transform), complete, repeat,  delay, pingPong);

		public static async UniTask RotXTo(this Transform transform, float toAngle,  float duration, EaseType type,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)=>
					await transform.RotXTo( toAngle, duration, type, getCT(transform), complete, repeat,  delay, pingPong);
		public static async UniTask RotYTo(this Transform transform, float toAngle,  float duration, EaseType type,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)=>
					await transform.RotYTo( toAngle, duration, type, getCT(transform), complete, repeat,  delay, pingPong);
		public static async UniTask RotZTo(this Transform transform, float toAngle,  float duration, EaseType type,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)=>
					await transform.RotZTo( toAngle, duration, type, getCT(transform), complete, repeat,  delay, pingPong);

		public static async UniTask ScaleXTo(this Transform transform, float to,  float duration, EaseType type,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)=>
					await transform.ScaleXTo( to, duration, type, getCT(transform), complete, repeat,  delay, pingPong);
		public static async UniTask ScaleYTo(this Transform transform, float to,  float duration, EaseType type,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)=>
					await transform.ScaleYTo( to, duration, type, getCT(transform), complete, repeat,  delay, pingPong);
		public static async UniTask ScaleZTo(this Transform transform, float to,  float duration, EaseType type,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)=>
					await transform.ScaleZTo( to, duration, type, getCT(transform), complete, repeat,  delay, pingPong);

		public static async UniTask ScaleXYTo(this Transform transform, Vector2 to, float duration, EaseType type,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)=>
					await transform.ScaleXYTo( to, duration, type, getCT(transform), complete, repeat,  delay, pingPong);
		public static async UniTask ScaleXYZTo(this Transform transform, Vector3 to, float duration, EaseType type,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)=>
					await transform.ScaleXYZTo( to, duration, type, getCT(transform), complete, repeat,  delay, pingPong);


		// GameObject
		public static async UniTask XTo(this GameObject fromObj, float to,  float duration, EaseType type,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.XTo( to, duration, type, complete, repeat, delay, pingPong);
		public static async UniTask YTo(this GameObject fromObj, float to,  float duration, EaseType type,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.YTo( to, duration, type, complete, repeat, delay, pingPong);
		public static async UniTask ZTo(this GameObject fromObj, float to,  float duration, EaseType type,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.ZTo( to, duration, type, complete, repeat, delay, pingPong);
		public static async UniTask XOffset(this GameObject fromObj, float offset,  float duration, EaseType type,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.XOffset( offset, duration, type, complete, repeat, delay, pingPong);
		public static async UniTask YOffset(this GameObject fromObj, float offset,  float duration, EaseType type,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.YOffset( offset, duration, type, complete, repeat, delay, pingPong);
		public static async UniTask ZOffset(this GameObject fromObj, float offset,  float duration, EaseType type,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.ZOffset( offset, duration, type, complete, repeat, delay, pingPong);
		public static async UniTask LocalXTo(this GameObject fromObj, float to,  float duration, EaseType type,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.LocalXTo( to, duration, type, complete, repeat, delay, pingPong);
		public static async UniTask LocalYTo(this GameObject fromObj, float to,  float duration, EaseType type,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.LocalYTo( to, duration, type, complete, repeat, delay, pingPong);
		public static async UniTask LocalZTo(this GameObject fromObj, float to,  float duration, EaseType type,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.LocalZTo( to, duration, type, complete, repeat, delay, pingPong);
		public static async UniTask LocalXOffset(this GameObject fromObj, float offset,  float duration, EaseType type,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.LocalXOffset( offset, duration, type, complete, repeat, delay, pingPong);
		public static async UniTask LocalYOffset(this GameObject fromObj, float offset,  float duration, EaseType type,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.LocalYOffset( offset, duration, type, complete, repeat, delay, pingPong);
		public static async UniTask LocalZOffset(this GameObject fromObj, float offset,  float duration, EaseType type,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.LocalZOffset( offset, duration, type, complete, repeat, delay, pingPong);
		public static async UniTask XYTo(this GameObject fromObj, Vector2 to, float duration, EaseType type,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.XYTo( to, duration, type, complete, repeat, delay, pingPong);
		public static async UniTask XYOffset(this GameObject fromObj, Vector2 offset, float duration, EaseType type,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.XYOffset( offset, duration, type, complete, repeat, delay, pingPong);
		public static async UniTask LocalXYTo(this GameObject fromObj, Vector2 to, float duration, EaseType type,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.LocalXYTo( to, duration, type, complete, repeat, delay, pingPong);
		public static async UniTask LocalXYOffset(this GameObject fromObj, Vector2 offset, float duration, EaseType type,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.LocalXYOffset( offset, duration, type, complete, repeat, delay, pingPong);
		public static async UniTask XYZTo(this GameObject fromObj, Vector3 to, float duration, EaseType type,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.XYZTo( to, duration, type, complete, repeat, delay, pingPong);
		public static async UniTask XYZOffset(this GameObject fromObj, Vector3 offset, float duration, EaseType type,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.XYZOffset( offset, duration, type, complete, repeat, delay, pingPong);
		public static async UniTask LocalXYZTo(this GameObject fromObj, Vector3 to, float duration, EaseType type,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.LocalXYZTo( to, duration, type, complete, repeat, delay, pingPong);
		public static async UniTask LocalXYZOffset(this GameObject fromObj, Vector3 offset, float duration, EaseType type,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.LocalXYZOffset( offset, duration, type, complete, repeat, delay, pingPong);

		public static async UniTask RotXTo(this GameObject fromObj, float toAngle,  float duration, EaseType type,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.RotXTo( toAngle, duration, type, complete, repeat, delay, pingPong);
		public static async UniTask RotYTo(this GameObject fromObj, float toAngle,  float duration, EaseType type,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.RotYTo( toAngle, duration, type, complete, repeat, delay, pingPong);
		public static async UniTask RotZTo(this GameObject fromObj, float toAngle,  float duration, EaseType type,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.RotZTo( toAngle, duration, type, complete, repeat, delay, pingPong);

		public static async UniTask LocalRotXTo(this GameObject fromObj, float toAngle,  float duration, EaseType type,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.LocalRotXTo( toAngle, duration, type, complete, repeat, delay, pingPong);
		public static async UniTask LocalRotYTo(this GameObject fromObj, float toAngle,  float duration, EaseType type,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.LocalRotYTo( toAngle, duration, type, complete, repeat, delay, pingPong);
		public static async UniTask LocalRotZTo(this GameObject fromObj, float toAngle,  float duration, EaseType type,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.LocalRotZTo( toAngle, duration, type, complete, repeat, delay, pingPong);

		public static async UniTask ScaleXTo(this GameObject fromObj, float to,  float duration, EaseType type,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.ScaleXTo( to, duration, type, complete, repeat, delay, pingPong);
		public static async UniTask ScaleYTo(this GameObject fromObj, float to,  float duration, EaseType type,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.ScaleYTo( to, duration, type, complete, repeat, delay, pingPong);
		public static async UniTask ScaleZTo(this GameObject fromObj, float to,  float duration, EaseType type,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.ScaleZTo( to, duration, type, complete, repeat, delay, pingPong);
		public static async UniTask ScaleXYTo(this GameObject fromObj, Vector2 to, float duration, EaseType type,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.ScaleXYTo( to, duration, type, complete, repeat, delay, pingPong);
		public static async UniTask ScaleXYZTo(this GameObject fromObj, Vector3 to, float duration, EaseType type,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
												=> await fromObj.transform.ScaleXYZTo( to, duration, type, complete, repeat, delay, pingPong);

		// SpriteRenderer
		public static async UniTask SizeTo(this SpriteRenderer fromObj, Vector2 to, float duration, EaseType type,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)=>
					await fromObj.SizeTo( to, duration, type, getCT(fromObj), complete, repeat,  delay, pingPong);

		public static async UniTask AlphaTo(this Renderer fromObj, float to, float duration, EaseType type,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)=>
					await fromObj.AlphaTo( to, duration, type, getCT(fromObj), complete, repeat,  delay, pingPong);

		public static async UniTask AlphaTo(this Renderer fromObj, float from, float to, float duration, EaseType type,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)=>
					await fromObj.AlphaTo( from, to, duration, type, getCT(fromObj), complete, repeat,  delay, pingPong);

		public static async UniTask AlphaTo(this Graphic fromObj, float to, float duration, EaseType type,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)=>
					await fromObj.AlphaTo( to, duration, type, getCT(fromObj), complete, repeat,  delay, pingPong);
		public static async UniTask AlphaTo(this Graphic fromObj,float from, float to, float duration, EaseType type,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)=>
					await fromObj.AlphaTo( from,to, duration, type, getCT(fromObj), complete, repeat,  delay, pingPong);
		public static async UniTask AlphaTo(this Material fromObj, float to, float duration, EaseType type,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)=>
					await fromObj.AlphaTo( to, duration, type, getCT(fromObj), complete, repeat,  delay, pingPong);
		public static async UniTask AlphaTo(this Material fromObj, float from,float to, float duration, EaseType type,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)=>
					await fromObj.AlphaTo( from, to, duration, type, getCT(fromObj), complete, repeat,  delay, pingPong);


		public static async UniTask ColorTo(this Renderer fromObj, Vector3 to, float duration, EaseType type,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)=>
					await fromObj.ColorTo( to, duration, type, getCT(fromObj), complete, repeat,  delay, pingPong);
		public static async UniTask ColorTo(this Graphic fromObj, Vector3 to, float duration, EaseType type,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)=>
					await fromObj.ColorTo( to, duration, type, getCT(fromObj), complete, repeat,  delay, pingPong);
		public static async UniTask ColorTo(this SpriteRenderer fromObj, Color to, float duration, EaseType type,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)=>
					await fromObj.ColorTo( to, duration, type, getCT(fromObj), complete, repeat,  delay, pingPong);

		public static async UniTask ColorTo(this Renderer fromObj, Color to, float duration, EaseType type,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)=>
					await fromObj.ColorTo( to, duration, type, getCT(fromObj), complete, repeat,  delay, pingPong);
		public static async UniTask ColorTo(this Graphic fromObj, Color to, float duration, EaseType type,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)=>
					await fromObj.ColorTo( to, duration, type, getCT(fromObj), complete, repeat,  delay, pingPong);
		public static async UniTask ColorTo(this Material fromObj, Color to, float duration, EaseType type,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)=>
					await fromObj.ColorTo( to, duration, type, getCT(fromObj), complete, repeat,  delay, pingPong);
		public static async UniTask ColorTo(this Material fromObj, Color from,Color to, float duration, EaseType type,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)=>
					await fromObj.ColorTo( from, to, duration, type, getCT(fromObj), complete, repeat,  delay, pingPong);

		public static async UniTask AlphaTo(this CanvasGroup fromObj, float to, float duration, EaseType type,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)=>
					await fromObj.AlphaTo( to, duration, type, getCT(fromObj), complete, repeat,  delay, pingPong);

		public static async UniTask AlphaTo(this CanvasGroup fromObj,float from, float to, float duration, EaseType type,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)=>
					await fromObj.AlphaTo( from,to, duration, type, getCT(fromObj), complete, repeat,  delay, pingPong);

		public static async UniTask TextSend(this TMP_Text fromObj, int fromIndex, int toIndex, float duration, EaseType type,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)=>
					await fromObj.TextSend( fromIndex, toIndex, duration, type, getCT(fromObj), complete, repeat, delay, pingPong);





		public static async UniTask Tween(float from, float to, float duration, System.Action<float> update, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			var func = Types[type];
			var updater = new FloatUpdater(from,to,func,update);
			await tweenBase( updater, duration, type, ct, complete, repeat, delay, pingPong );
		}

		public static async UniTask Tween(Vector2 from, Vector2 to, float duration, System.Action<Vector2> update, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			var func = Types[type];
			var updater = new Vector2Updater(from,to,func,update);
			await tweenBase( updater, duration, type, ct, complete, repeat, delay, pingPong );
		}
		public static async UniTask Tween(Vector3 from, Vector3 to, float duration, System.Action<Vector3> update, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			var func = Types[type];
			var updater = new Vector3Updater(from,to,func,update);
			await tweenBase( updater, duration, type, ct, complete, repeat, delay, pingPong );
		}

		public static async UniTask Tween(Vector4 from, Vector4 to, float duration, System.Action<Vector4> update, EaseType type, CancellationToken ct,
												System.Action<bool> complete=null, int repeat=0, float delay=0, bool pingPong=false)
		{
			var func = Types[type];
			var updater = new Vector4Updater(from,to,func,update);
			await tweenBase( updater, duration, type, ct, complete, repeat, delay, pingPong );
		}

		private const float HalfPi = Mathf.PI * .5f;
		private const float DoublePi = Mathf.PI * 2f;

		class Interporate
		{
			public virtual float Time(float time){return time;}
			public virtual float Op(float from,float to, float time){ return Mathf.Lerp(from, to, Time(time)); }
			public virtual Vector2 Op(Vector2 from,Vector2 to, float time){ return Vector2.Lerp(from,to,Time(time));}
			public virtual Vector3 Op(Vector3 from,Vector3 to, float time){ return Vector3.Lerp(from,to,Time(time));}
			public virtual Vector4 Op(Vector4 from,Vector4 to, float time){ return Vector4.Lerp(from,to,Time(time));}
		}

		// Lerp
		class Lerp : Interporate{
			public override float Time(float time) => time;
		}

		// SineIn
		class SineIn : Interporate{
			public override float Time(float time) => 1f - Mathf.Cos(time * HalfPi); 
		}

		// SineOut
		class SineOut : Interporate{
			public override float Time(float time) => Mathf.Sin(time * HalfPi);
		}

		// SineInOut
		class SineInOut : Interporate{
			public override float Time(float time) => .5f * (1f - Mathf.Cos(Mathf.PI * time));
		}

		// QuadIn
		class QuadIn : Interporate{
			public override float Time(float time) => time * time;
		}

		// QuadOut
		class QuadOut : Interporate{
			public override float Time(float time) => -time * (time - 2f);
		}

		// QuadInOut
		class QuadInOut : Interporate{
			public override float Time(float time){
				if ((time /= .5f) < 1f) return .5f * time * time;
				return -.5f * (((--time) * (time - 2f) - 1f));
			}
		}

		// CubicIn
		class CubicIn : Interporate{
			public override float Time(float time) => time * time * time;
		}

		// CubicOut
		class CubicOut : Interporate{
			public override float Time(float time) => (time -= 1f) * time * time + 1f;
		}

		// CubicInOut
		class CubicInOut : Interporate{
			public override float Time(float time){
				if ((time /= .5f) < 1f) return .5f * time * time * time;
				return .5f * ((time -= 2) * time * time + 2f);
			}
		}
		// QuartIn
		class QuartIn : Interporate{
			public override float Time(float time) => time * time * time * time;
		}

		// QuartOut
		class QuartOut : Interporate{
			public override float Time(float time) => -((time -= 1f) * time * time * time - 1f);
		}

		// QuartInOut
		class QuartInOut : Interporate{
			public override float Time(float time){
				if ((time /= .5f) < 1f) return .5f * time * time * time * time;
				return -.5f * ((time -= 2f) * time * time * time - 2f);
			}
		}

		// QuintIn
		class QuintIn : Interporate{
			public override float Time(float time) => time * time * time * time * time;
		}

		// QuintOut
		class QuintOut : Interporate{
			public override float Time(float time) => (time -= 1f) * time * time * time * time + 1f;
		}

		// QuintInOut
		class QuintInOut : Interporate{
			public override float Time(float time){
				if ((time /= .5f) < 1f) return .5f * time * time * time * time * time;
				return .5f * ((time -= 2f) * time * time * time * time + 2f);
			}
		}

		// ExpoIn
		class ExpoIn : Interporate{
			public override float Time(float time) => Mathf.Pow(2f, 10f * (time - 1f));
		}

		// ExpoIn
		class ExpoOut : Interporate{
			public override float Time(float time) => -Mathf.Pow(2f, -10f * time) + 1f;
		}

		// ExpoInOut
		class ExpoInOut : Interporate{
			public override float Time(float time){
				if ((time /= .5f) < 1f) return .5f * Mathf.Pow(2f, 10f * (time - 1f));
				return .5f * (-Mathf.Pow(2f, -10f * --time) + 2f);
			}
		}

		// CircIn
		class CircIn : Interporate{
			public override float Time(float time) => -(Mathf.Sqrt(1f - time * time) - 1f);
		}

		// CircOut
		class CircOut : Interporate{
			public override float Time(float time) => Mathf.Sqrt(1f - (time -= 1f) * time);
		}

		// CircInOut
		class CircInOut : Interporate{
			public override float Time(float time){
				if ((time /= .5f) < 1f) return -.5f * (Mathf.Sqrt(1f - time * time) - 1f);
				return .5f * (Mathf.Sqrt(1f - (time -= 2f) * time) + 1f);
			}
		}

		// BackIn
		class BackIn : Interporate{
			const float s = 1.70158f;
			public override float Time(float time) => time * time * ((s + 1f) * time - s);
			public override float Op(float from,float to, float time){ to -= from; return to * Time(time) + from; }
			public override Vector2 Op(Vector2 from,Vector2 to, float time){ to -= from; return to*Time(time) + from;}
			public override Vector3 Op(Vector3 from,Vector3 to, float time){ to -= from; return to*Time(time) + from;}
			public override Vector4 Op(Vector4 from,Vector4 to, float time){ to -= from; return to*Time(time) + from;}
		}

		// BackOut
		class BackOut : Interporate{
			const float s = 1.70158f;
			public override float Time(float time) => --time * time * ((s + 1f) * time + s) + 1f;
			public override float Op(float from,float to, float time){ to -= from; return to * Time(time) + from; }
			public override Vector2 Op(Vector2 from,Vector2 to, float time){ to -= from; return to*Time(time) + from;}
			public override Vector3 Op(Vector3 from,Vector3 to, float time){ to -= from; return to*Time(time) + from;}
			public override Vector4 Op(Vector4 from,Vector4 to, float time){ to -= from; return to*Time(time) + from;}
		}

		// BackInOut
		class BackInOut : Interporate{
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
		class ElasticIn : Interporate{
			const float p = .3f;
			const float s = p / 4f;
			public override float Time(float time) => -(Mathf.Pow(2f, 10f * (time -= 1f)) * Mathf.Sin((time - s) * DoublePi / p));
			public override float Op(float from,float to, float time){ to -= from; return to * Time(time) + from; }
			public override Vector2 Op(Vector2 from,Vector2 to, float time){ to -= from; return to*Time(time) + from;}
			public override Vector3 Op(Vector3 from,Vector3 to, float time){ to -= from; return to*Time(time) + from;}
			public override Vector4 Op(Vector4 from,Vector4 to, float time){ to -= from; return to*Time(time) + from;}
		}

		// ElasticOut
		class ElasticOut : Interporate{
			const float p = .3f;
			const float s = p / 4f;
			public override float Time(float time) => Mathf.Pow(2f, -10f * time) * Mathf.Sin((time - s) * DoublePi / p);
			public override float Op(float from,float to, float time){ to -= from; return to * Time(time) + to + from; }
			public override Vector2 Op(Vector2 from,Vector2 to, float time){ to -= from; return to*Time(time) + to + from;}
			public override Vector3 Op(Vector3 from,Vector3 to, float time){ to -= from; return to*Time(time) + to + from;}
			public override Vector4 Op(Vector4 from,Vector4 to, float time){ to -= from; return to*Time(time) + to + from;}
		}

		// ElasticInOut
		class ElasticInOut : Interporate{
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
		class BounceIn : Interporate{
			static BounceOut bout = new BounceOut();
			public override float Time(float time) => 1f - time;
			public override float Op(float from,float to, float time){ to -= from; return to - bout.Op(0f, to, Time(time)) + from; }
			public override Vector2 Op(Vector2 from,Vector2 to, float time){ to -= from; return to - bout.Op(Vector2.zero, to, Time(time)) + from; }
			public override Vector3 Op(Vector3 from,Vector3 to, float time){ to -= from; return to - bout.Op(Vector3.zero, to, Time(time)) + from; }
			public override Vector4 Op(Vector4 from,Vector4 to, float time){ to -= from; return to - bout.Op(Vector4.zero, to, Time(time)) + from; }
		}

		// BounceOut
		class BounceOut : Interporate{
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
		class BounceInOut : Interporate{
			static BounceIn bin = new BounceIn();
			static BounceOut bout = new BounceOut();
			public override float Time(float time) => time < .5f?(time * 2f):(time*2f-1f);
			public override float Op(float from,float to, float time){ to -= from; return time < .5f?(bin.Op(0f, to, Time(time))*.5f+from):(bout.Op(0f, to, Time(time)) * .5f + to * .5f + from); }
			public override Vector2 Op(Vector2 from,Vector2 to, float time){ to -= from; return time < .5f?(bin.Op(Vector2.zero, to, Time(time))*.5f+from):(bout.Op(Vector2.zero, to, Time(time)) * .5f + to * .5f + from); }
			public override Vector3 Op(Vector3 from,Vector3 to, float time){ to -= from; return time < .5f?(bin.Op(Vector3.zero, to, Time(time))*.5f+from):(bout.Op(Vector3.zero, to, Time(time)) * .5f + to * .5f + from); }
			public override Vector4 Op(Vector4 from,Vector4 to, float time){ to -= from; return time < .5f?(bin.Op(Vector4.zero, to, Time(time))*.5f+from):(bout.Op(Vector4.zero, to, Time(time)) * .5f + to * .5f + from); }
		}

		// Spring
		class Spring : Interporate{
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
