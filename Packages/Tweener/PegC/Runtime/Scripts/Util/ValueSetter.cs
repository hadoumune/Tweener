using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System.Threading;
using TMPro;
using System;

namespace PegC.Util.ValueSetter
{
	public struct Vector1 {
		public static readonly Vector1 zero = new Vector1(0);
		public static readonly Vector1 one = new Vector1(1);
		public static readonly Vector1 up = new Vector1(0);
		public static readonly Vector1 down = new Vector1(0);
		public static readonly Vector1 right = new Vector1(1);
		public static readonly Vector1 left = new Vector1(-1);
		public static readonly Vector1 forward = new Vector1(0);
		public static readonly Vector1 back = new Vector1(0);
		public float x;

		public Vector1(float x) {
			this.x = x;
		}

		// Convert Vector1 to float
		public static implicit operator float(Vector1 v1) {
			return v1.x;
		}

		// Convert float to Vector1
		public static explicit operator Vector1(float x)
		{
			return new Vector1(x);
		}

		// Convert Vector1 to Vector2
		public static implicit operator Vector2(Vector1 v1) {
			return new Vector2(v1.x, 0f);
		}

		// Convert Vector1 to Vector3
		public static implicit operator Vector3(Vector1 v1) {
			return new Vector3(v1.x, 0f, 0f);
		}

		// Convert Vector1 to Vector4
		public static implicit operator Vector4(Vector1 v1) {
			return new Vector4(v1.x, 0f, 0f, 0f);
		}

		// Vector1との加算
		public static Vector1 operator +(Vector1 v, Vector1 scalar)
		{
			return new Vector1(v.x + scalar.x);
		}

		// Vector1との減算
		public static Vector1 operator -(Vector1 v, Vector1 scalar)
		{
			return new Vector1(v.x - scalar.x);
		}

		// Vector1との乗算
		public static Vector1 operator *(Vector1 v, Vector1 scalar)
		{
			return new Vector1(v.x * scalar.x);
		}

		// Vector1との除算
		public static Vector1 operator /(Vector1 v, Vector1 scalar)
		{
			if (scalar.x == 0)
				throw new DivideByZeroException("Cannot divide by zero.");
			return new Vector1(v.x / scalar.x);
		}

		// スカラーとの加算
		public static Vector1 operator +(Vector1 v, float scalar)
		{
			return new Vector1(v.x + scalar);
		}

		// スカラーとの減算
		public static Vector1 operator -(Vector1 v, float scalar)
		{
			return new Vector1(v.x - scalar);
		}

		// スカラーとの乗算
		public static Vector1 operator *(Vector1 v, float scalar)
		{
			return new Vector1(v.x * scalar);
		}

		// スカラーとの除算
		public static Vector1 operator /(Vector1 v, float scalar)
		{
			if (scalar == 0)
				throw new DivideByZeroException("Cannot divide by zero.");
			return new Vector1(v.x / scalar);
		}
	}

	public struct ValueReflector{
		public Vector1 Get(ref Vector1 param) => param;
		public Vector2 Get(ref Vector2 param) => param;
		public Vector3 Get(ref Vector3 param) => param;
		public Vector4 Get(ref Vector4 param) => param;
		public Quaternion Get(ref Quaternion param) => param;
	}

	public delegate void UpdateComponentValueDelegate<S,T>( S src, ref T value) where S : Component where T: struct;

	public struct ComponentUpdater<S,T> where S : Component where T: struct
	{
		private S _srcComponent;
		private UpdateComponentValueDelegate<S,T> _updateMethod;

		public ComponentUpdater(S srcComponent,UpdateComponentValueDelegate<S,T> updateMethod)
		{
			_srcComponent = srcComponent;
			_updateMethod = updateMethod;
		}

		public void Update(ref T value)
		{
			_updateMethod( _srcComponent,ref value);
		}
	}

	class Updater{
		public static void LocalPositionX(Transform transform,ref Vector1 pos ) { var oldPos = transform.localPosition; oldPos.x = pos.x; transform.localPosition = oldPos; }
		public static void LocalPositionY(Transform transform,ref Vector1 pos ) { var oldPos = transform.localPosition; oldPos.y = pos.x; transform.localPosition = oldPos; }
		public static void LocalPositionZ(Transform transform,ref Vector1 pos ) { var oldPos = transform.localPosition; oldPos.z = pos.x; transform.localPosition = oldPos; }
		public static void LocalPositionXY(Transform transform,ref Vector2 pos ) { var oldPos = transform.localPosition; oldPos.x = pos.x; oldPos.y = pos.y; transform.localPosition = oldPos; }
		public static void LocalPositionXZ(Transform transform,ref Vector2 pos ) { var oldPos = transform.localPosition; oldPos.x = pos.x; oldPos.z = pos.y; transform.localPosition = oldPos; }
		public static void LocalPositionYZ(Transform transform,ref Vector2 pos ) { var oldPos = transform.localPosition; oldPos.y = pos.x; oldPos.z = pos.y; transform.localPosition = oldPos; }
		public static void LocalPosition(Transform transform,ref Vector3 pos ) => transform.localPosition = pos;
		public static void PositionX(Transform transform,ref Vector1 pos ) { var oldPos = transform.position; oldPos.x = pos.x; transform.position = oldPos; }
		public static void PositionY(Transform transform,ref Vector1 pos ) { var oldPos = transform.position; oldPos.y = pos.x; transform.position = oldPos; }
		public static void PositionZ(Transform transform,ref Vector1 pos ) { var oldPos = transform.position; oldPos.z = pos.x; transform.position = oldPos; }
		public static void PositionXY(Transform transform,ref Vector2 pos ) { var oldPos = transform.position; oldPos.x = pos.x; oldPos.y = pos.y; transform.position = oldPos; }
		public static void PositionXZ(Transform transform,ref Vector2 pos ) { var oldPos = transform.position; oldPos.x = pos.x; oldPos.z = pos.y; transform.position = oldPos; }
		public static void PositionYZ(Transform transform,ref Vector2 pos ) { var oldPos = transform.position; oldPos.y = pos.x; oldPos.z = pos.y; transform.position = oldPos; }
		public static void Position(Transform transform,ref Vector3 pos ) => transform.position = pos;
		public static void LocalScaleX(Transform transform,ref Vector1 pos ) { var oldPos = transform.localScale; oldPos.x = pos.x; transform.localScale = oldPos; }
		public static void LocalScaleY(Transform transform,ref Vector1 pos ) { var oldPos = transform.localScale; oldPos.y = pos.x; transform.localScale = oldPos; }
		public static void LocalScaleZ(Transform transform,ref Vector1 pos ) { var oldPos = transform.localScale; oldPos.z = pos.x; transform.localScale = oldPos; }
		public static void LocalScaleXY(Transform transform,ref Vector2 pos ) { var oldPos = transform.localScale; oldPos.x = pos.x; oldPos.y = pos.y; transform.localScale = oldPos; }
		public static void LocalScaleXZ(Transform transform,ref Vector2 pos ) { var oldPos = transform.localScale; oldPos.x = pos.x; oldPos.z = pos.y; transform.localScale = oldPos; }
		public static void LocalScaleYZ(Transform transform,ref Vector2 pos ) { var oldPos = transform.localScale; oldPos.y = pos.x; oldPos.z = pos.y; transform.localScale = oldPos; }
		public static void LocalScale(Transform transform,ref Vector3 pos ) => transform.localPosition = pos;
		public static void LocalEulerX(Transform transform,ref Vector1 pos ) { var oldPos = transform.localEulerAngles; oldPos.x = pos.x; transform.localEulerAngles = oldPos; }
		public static void LocalEulerY(Transform transform,ref Vector1 pos ) { var oldPos = transform.localEulerAngles; oldPos.y = pos.x; transform.localEulerAngles = oldPos; }
		public static void LocalEulerZ(Transform transform,ref Vector1 pos ) { var oldPos = transform.localEulerAngles; oldPos.z = pos.x; transform.localEulerAngles = oldPos; }
		public static void LocalEuler(Transform transform,ref Vector3 pos ) => transform.localEulerAngles = pos;
		public static void EulerX(Transform transform,ref Vector1 pos ) { var oldPos = transform.eulerAngles; oldPos.x = pos.x; transform.eulerAngles = oldPos; }
		public static void EulerY(Transform transform,ref Vector1 pos ) { var oldPos = transform.eulerAngles; oldPos.y = pos.x; transform.eulerAngles = oldPos; }
		public static void EulerZ(Transform transform,ref Vector1 pos ) { var oldPos = transform.eulerAngles; oldPos.z = pos.x; transform.eulerAngles = oldPos; }
		public static void Euler(Transform transform,ref Vector3 pos ) => transform.position = pos;
		public static void LocalRotationX(Transform transform,ref Vector1 pos ) { var oldPos = transform.localRotation; oldPos.x = pos.x; transform.localRotation = oldPos; }
		public static void LocalRotationY(Transform transform,ref Vector1 pos ) { var oldPos = transform.localRotation; oldPos.y = pos.x; transform.localRotation = oldPos; }
		public static void LocalRotationZ(Transform transform,ref Vector1 pos ) { var oldPos = transform.localRotation; oldPos.z = pos.x; transform.localRotation = oldPos; }
		public static void LocalRotation(Transform transform,ref Vector3 pos ) => transform.localRotation = Quaternion.Euler(pos);
		public static void LocalRotation(Transform transform,ref Vector4 pos ) => transform.localRotation = new Quaternion(pos.x,pos.y,pos.z,pos.w);
		public static void RotationX(Transform transform,ref Vector1 pos ) { var oldPos = transform.rotation; oldPos.x = pos.x; transform.rotation = oldPos; }
		public static void RotationY(Transform transform,ref Vector1 pos ) { var oldPos = transform.rotation; oldPos.y = pos.x; transform.rotation = oldPos; }
		public static void RotationZ(Transform transform,ref Vector1 pos ) { var oldPos = transform.rotation; oldPos.z = pos.x; transform.rotation = oldPos; }
		public static void Rotation(Transform transform,ref Vector3 pos ) => transform.rotation = Quaternion.Euler(pos);
		public static void Rotation(Transform transform,ref Vector4 pos ) => transform.rotation = new Quaternion(pos.x,pos.y,pos.z,pos.w);

		public static void AnchorX(RectTransform transform,ref Vector1 pos ) { var oldPos = transform.anchoredPosition; oldPos.x = pos.x; transform.anchoredPosition = oldPos; }
		public static void AnchorY(RectTransform transform,ref Vector1 pos ) { var oldPos = transform.anchoredPosition; oldPos.y = pos.x; transform.anchoredPosition = oldPos; }
		public static void AnchorXY(RectTransform transform,ref Vector2 pos ) { var oldPos = transform.anchoredPosition; oldPos.x = pos.x; oldPos.y = pos.y; transform.anchoredPosition = oldPos; }

		public static void Size(SpriteRenderer transform,ref Vector2 pos ) => transform.size = pos;
		public static void Size(RectTransform transform,ref Vector2 pos ) => transform.sizeDelta = pos;

		public static void Color(Graphic graphic,ref Color val ) {graphic.color = val;}
		public static void Color(Renderer graphic,ref Color val ) {graphic.material.color = val;}
		public static void Color(SpriteRenderer graphic,ref Color val ) {graphic.color = val;}
		public static void Color(Material graphic,ref Color val ) {graphic.color = val;}

		public static void HSV(Graphic graphic,ref Vector3 val ) {
			var newCol = UnityEngine.Color.HSVToRGB(val.x,val.y,val.z);
			newCol.a = graphic.color.a;
			graphic.color = newCol;
		}
		public static void HSV(Renderer graphic,ref Vector3 val ){
			var newCol = UnityEngine.Color.HSVToRGB(val.x,val.y,val.z);
			newCol.a = graphic.material.color.a;
			graphic.material.color = newCol;
		}
		public static void HSV(SpriteRenderer graphic,ref Vector3 val ) {
			var newCol = UnityEngine.Color.HSVToRGB(val.x,val.y,val.z);
			newCol.a = graphic.color.a;
			graphic.color = newCol;
		}
		public static void HSV(Material graphic,ref Vector3 val ) {
			var newCol = UnityEngine.Color.HSVToRGB(val.x,val.y,val.z);
			newCol.a = graphic.color.a;
			graphic.color = newCol;
		}

		public static void Alpha(Graphic graphic,ref Vector1 val ) { var oldCol = graphic.color; oldCol.a = val.x; graphic.color = oldCol; }
		public static void Alpha(Renderer graphic,ref Vector1 val ) { var oldCol = graphic.material.color; oldCol.a = val.x; graphic.material.color = oldCol; }
		public static void Alpha(SpriteRenderer graphic,ref Vector1 val ) { var oldCol = graphic.color; oldCol.a = val.x; graphic.color = oldCol; }
		public static void Alpha(Material graphic,ref Vector1 val ) { var oldCol = graphic.color; oldCol.a = val.x; graphic.color = oldCol; }
		public static void Alpha(CanvasGroup graphic,ref Vector1 val ) { graphic.alpha = val.x; }

		public static void TextSend(TMP_Text graphic,ref Vector1 val ) { graphic.maxVisibleCharacters = (int)Mathf.Clamp(val.x,0, graphic.text.Length); }
	}

}