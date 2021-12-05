using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class ExtensionFunctions
{
	public static void RemoveAtFast<T>(this List<T> list, int index)
	{
		int lastElementIndex = list.Count - 1;

		list[index] = list[lastElementIndex];
		list.RemoveAt(lastElementIndex);
	}

	public static T GetRandomElement<T>(this T[] arr)
	{
		return arr[Random.Range(0, arr.Length)];
	}

	public static T GetRandomElement<T>(this List<T> list)
	{
		return list[Random.Range(0, list.Count)];
	}

	public static void Shuffle<T>(this T[] arr)
	{
		for (int i = 0; i < arr.Length - 1; i++)
		{
			int randomIndex = Random.Range(i, arr.Length);

			T temp = arr[i];
			arr[i] = arr[randomIndex];
			arr[randomIndex] = temp;
		}
	}

	public static void Shuffle<T>(this List<T> list)
	{
		for (int i = 0, length = list.Count; i < length - 1; i++)
		{
			int randomIndex = Random.Range(i, length);

			T temp = list[i];
			list[i] = list[randomIndex];
			list[randomIndex] = temp;
		}
	}

	public static bool IsPointerOverGameObjectMultiPlatform(this EventSystem eventSystem)
	{
#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
		return Input.touchCount > 0 && eventSystem.IsPointerOverGameObject( Input.GetTouch( 0 ).fingerId );
#else
		return eventSystem.IsPointerOverGameObject();
#endif
	}

	public static void SetAlpha(this Graphic graphic, float alpha)
	{
		Color color = graphic.color;
		color.a = alpha;
		graphic.color = color;
	}

	public static TweenerCore<float, float, FloatOptions> DOBlendShapeWeight(this SkinnedMeshRenderer renderer, int index, float target, float duration)
	{
		float value = renderer.GetBlendShapeWeight(index);
		return DOTween.To(() => value, x => {
			value = x;
			renderer.SetBlendShapeWeight(index, value);
		}, target, duration).SetTarget(renderer);
	}

	public static void ButtonNotFunctionalFeedback(this Button button)
	{
		button.DOKill(true);
		DOTween.To(() => button.image.color, x => button.image.color = x, Color.red, 0.12f).SetLoops(2, LoopType.Yoyo).SetTarget(button);
		button.transform.DOScale(button.transform.localScale * 1.2f, 0.12f).SetLoops(2, LoopType.Yoyo).SetTarget(button);
	}

#if UNITY_EDITOR
	// Credit: https://forum.unity.com/threads/debug-drawarrow.85980/
	public static void DrawArrowGizmo(Vector3 pos, Quaternion lookRotation, Vector2 size)
	{
		Vector3 direction = lookRotation * new Vector3(0f, 0f, 1f) * size.y;

		Vector3 left = lookRotation * Quaternion.Euler(0f, 160f, 0f) * new Vector3(0f, 0f, 1f);
		Vector3 right = lookRotation * Quaternion.Euler(0f, 200f, 0f) * new Vector3(0f, 0f, 1f);

		Gizmos.DrawRay(pos, direction);
		Gizmos.DrawRay(pos + direction, right * size.x);
		Gizmos.DrawRay(pos + direction, left * size.x);
	}
#endif
}