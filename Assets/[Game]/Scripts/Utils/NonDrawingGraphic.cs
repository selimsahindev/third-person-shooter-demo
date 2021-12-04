using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasRenderer))]
public class NonDrawingGraphic : Graphic
{
	#pragma warning disable 0649
	#if UNITY_EDITOR
	[SerializeField]
	private bool visualize;
	#endif
	#pragma warning restore 0649

	#if !UNITY_EDITOR
	public override void SetMaterialDirty() { }
	public override void SetVerticesDirty() { }
	#endif

	protected override void OnPopulateMesh(VertexHelper vh)
	{
		vh.Clear();

		#if UNITY_EDITOR
		if (visualize)
		{
			Rect rect = GetPixelAdjustedRect();
			float width = rect.width;
			float height = rect.height;

			Vector2 pivot = rectTransform.pivot;
			float bottomLeftX = -width * pivot.x;
			float bottomLeftY = -height * pivot.y;

			vh.AddVert(new Vector3(bottomLeftX, bottomLeftY, 0f), color, Vector2.zero);
			vh.AddVert(new Vector3(bottomLeftX, bottomLeftY + height, 0f), color, Vector2.zero);
			vh.AddVert(new Vector3(bottomLeftX + width, bottomLeftY + height, 0f), color, Vector2.zero);
			vh.AddVert(new Vector3(bottomLeftX + width, bottomLeftY, 0f), color, Vector2.zero);

			vh.AddTriangle(0, 1, 2);
			vh.AddTriangle(2, 3, 0);
		}
		#endif
	}
}
