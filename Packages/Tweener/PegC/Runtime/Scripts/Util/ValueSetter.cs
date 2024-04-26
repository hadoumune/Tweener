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
		public float x;

		public Vector1(float x) {
			this.x = x;
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
		public static void LocalPositionX(Transform transform,Vector1 pos ) { var oldPos = transform.localPosition; oldPos.x = pos.x; transform.localPosition = oldPos; }
		public static void LocalPositionY(Transform transform,Vector1 pos ) { var oldPos = transform.localPosition; oldPos.y = pos.x; transform.localPosition = oldPos; }
		public static void LocalPositionZ(Transform transform,Vector1 pos ) { var oldPos = transform.localPosition; oldPos.z = pos.x; transform.localPosition = oldPos; }
		public static void LocalPositionXY(Transform transform,Vector2 pos ) { var oldPos = transform.localPosition; oldPos.x = pos.x; oldPos.y = pos.y; transform.localPosition = oldPos; }
		public static void LocalPositionXZ(Transform transform,Vector2 pos ) { var oldPos = transform.localPosition; oldPos.x = pos.x; oldPos.z = pos.y; transform.localPosition = oldPos; }
		public static void LocalPositionYZ(Transform transform,Vector2 pos ) { var oldPos = transform.localPosition; oldPos.y = pos.x; oldPos.z = pos.y; transform.localPosition = oldPos; }
		public static void LocalPosition(Transform transform,Vector3 pos ) => transform.localPosition = pos;
		public static void PositionX(Transform transform,Vector1 pos ) { var oldPos = transform.position; oldPos.x = pos.x; transform.position = oldPos; }
		public static void PositionY(Transform transform,Vector1 pos ) { var oldPos = transform.position; oldPos.y = pos.x; transform.position = oldPos; }
		public static void PositionZ(Transform transform,Vector1 pos ) { var oldPos = transform.position; oldPos.z = pos.x; transform.position = oldPos; }
		public static void PositionXY(Transform transform,Vector2 pos ) { var oldPos = transform.position; oldPos.x = pos.x; oldPos.y = pos.y; transform.position = oldPos; }
		public static void PositionXZ(Transform transform,Vector2 pos ) { var oldPos = transform.position; oldPos.x = pos.x; oldPos.z = pos.y; transform.position = oldPos; }
		public static void PositionYZ(Transform transform,Vector2 pos ) { var oldPos = transform.position; oldPos.y = pos.x; oldPos.z = pos.y; transform.position = oldPos; }
		public static void Position(Transform transform,Vector3 pos ) => transform.position = pos;
		public static void LocalScaleX(Transform transform,Vector1 pos ) { var oldPos = transform.localScale; oldPos.x = pos.x; transform.localScale = oldPos; }
		public static void LocalScaleY(Transform transform,Vector1 pos ) { var oldPos = transform.localScale; oldPos.y = pos.x; transform.localScale = oldPos; }
		public static void LocalScaleZ(Transform transform,Vector1 pos ) { var oldPos = transform.localScale; oldPos.z = pos.x; transform.localScale = oldPos; }
		public static void LocalScaleXY(Transform transform,Vector2 pos ) { var oldPos = transform.localScale; oldPos.x = pos.x; oldPos.y = pos.y; transform.localScale = oldPos; }
		public static void LocalScaleXZ(Transform transform,Vector2 pos ) { var oldPos = transform.localScale; oldPos.x = pos.x; oldPos.z = pos.y; transform.localScale = oldPos; }
		public static void LocalScaleYZ(Transform transform,Vector2 pos ) { var oldPos = transform.localScale; oldPos.y = pos.x; oldPos.z = pos.y; transform.localScale = oldPos; }
		public static void LocalScale(Transform transform,Vector3 pos ) => transform.localPosition = pos;
		public static void LocalEulerX(Transform transform,Vector1 pos ) { var oldPos = transform.localEulerAngles; oldPos.x = pos.x; transform.localEulerAngles = oldPos; }
		public static void LocalEulerY(Transform transform,Vector1 pos ) { var oldPos = transform.localEulerAngles; oldPos.y = pos.x; transform.localEulerAngles = oldPos; }
		public static void LocalEulerZ(Transform transform,Vector1 pos ) { var oldPos = transform.localEulerAngles; oldPos.z = pos.x; transform.localEulerAngles = oldPos; }
		public static void LocalEuler(Transform transform,Vector3 pos ) => transform.localEulerAngles = pos;
		public static void EulerX(Transform transform,Vector1 pos ) { var oldPos = transform.eulerAngles; oldPos.x = pos.x; transform.eulerAngles = oldPos; }
		public static void EulerY(Transform transform,Vector1 pos ) { var oldPos = transform.eulerAngles; oldPos.y = pos.x; transform.eulerAngles = oldPos; }
		public static void EulerZ(Transform transform,Vector1 pos ) { var oldPos = transform.eulerAngles; oldPos.z = pos.x; transform.eulerAngles = oldPos; }
		public static void Euler(Transform transform,Vector3 pos ) => transform.position = pos;
		public static void LocalRotationX(Transform transform,Vector1 pos ) { var oldPos = transform.localRotation; oldPos.x = pos.x; transform.localRotation = oldPos; }
		public static void LocalRotationY(Transform transform,Vector1 pos ) { var oldPos = transform.localRotation; oldPos.y = pos.x; transform.localRotation = oldPos; }
		public static void LocalRotationZ(Transform transform,Vector1 pos ) { var oldPos = transform.localRotation; oldPos.z = pos.x; transform.localRotation = oldPos; }
		public static void LocalRotation(Transform transform,Vector3 pos ) => transform.localRotation = Quaternion.Euler(pos);
		public static void LocalRotation(Transform transform,Vector4 pos ) => transform.localRotation = new Quaternion(pos.x,pos.y,pos.z,pos.w);
		public static void RotationX(Transform transform,Vector1 pos ) { var oldPos = transform.rotation; oldPos.x = pos.x; transform.rotation = oldPos; }
		public static void RotationY(Transform transform,Vector1 pos ) { var oldPos = transform.rotation; oldPos.y = pos.x; transform.rotation = oldPos; }
		public static void RotationZ(Transform transform,Vector1 pos ) { var oldPos = transform.rotation; oldPos.z = pos.x; transform.rotation = oldPos; }
		public static void Rotation(Transform transform,Vector3 pos ) => transform.rotation = Quaternion.Euler(pos);
		public static void Rotation(Transform transform,Vector4 pos ) => transform.rotation = new Quaternion(pos.x,pos.y,pos.z,pos.w);

		public static void Size(SpriteRenderer transform,Vector2 pos ) => transform.size = pos;
		public static void Size(RectTransform transform,Vector2 pos ) => transform.sizeDelta = pos;

		public static void Color(Graphic graphic,Color val ) {graphic.color = val;}
		public static void Color(Renderer graphic,Color val ) {graphic.material.color = val;}
		public static void Color(SpriteRenderer graphic,Color val ) {graphic.color = val;}
		public static void Color(Material graphic,Color val ) {graphic.color = val;}

		public static void Alpha(Graphic graphic,Vector1 val ) { var oldCol = graphic.color; oldCol.a = val.x; graphic.color = oldCol; }
		public static void Alpha(Renderer graphic,Vector1 val ) { var oldCol = graphic.material.color; oldCol.a = val.x; graphic.material.color = oldCol; }
		public static void Alpha(SpriteRenderer graphic,Vector1 val ) { var oldCol = graphic.color; oldCol.a = val.x; graphic.color = oldCol; }
		public static void Alpha(Material graphic,Vector1 val ) { var oldCol = graphic.color; oldCol.a = val.x; graphic.color = oldCol; }
		public static void Alpha(CanvasGroup graphic,Vector1 val ) { graphic.alpha = val.x; }

		public static void TextSend(TMP_Text graphic,Vector1 val ) { graphic.maxVisibleCharacters = (int)Mathf.Clamp(val.x,0, graphic.text.Length); }
	}

}