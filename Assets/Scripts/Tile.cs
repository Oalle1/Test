using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDropHandler, IDragHandler
{
	public int id;
	public Vector2 positionInitial;
	public Tile(int tileId)
	{
		id = tileId;
	}

	public Canvas canvas;
	public bool isCorrect = false;
	private RectTransform rectTransform;
	public Vector2 startPosition;
	private Vector2 tmpPosition;
	private Gameplay game;

	private void Awake()
	{
		rectTransform = GetComponent<RectTransform>();
		positionInitial = rectTransform.anchoredPosition;
		game = transform.parent.GetComponent<Gameplay>();
	}

	/// <summary>
	/// Check every frame if the Tile is in the right position and communicate it.
	/// </summary>
	private void Update()
	{
		isCorrect = (positionInitial == rectTransform.anchoredPosition) ? true : false;
		game.CorrectTile( id,isCorrect);

	}

	/// <summary>
	/// When clicking on a tile, keep it's initial position.
	/// </summary>
	public void OnBeginDrag(PointerEventData eventData)
	{
		startPosition = rectTransform.anchoredPosition;

	}

	public void OnEndDrag(PointerEventData eventData)
	{

	}

	/// <summary>
	///Check if the Tile is drop near a position where could be and directly position itself in the right spot.
	///I wanted to use this function to check if we dont move our Tile for more than one movement and check if we don't put our tile on another one but
	/// it is very buggy and it needs work.
	/// </summary>
	public void OnDrop(PointerEventData eventData)
	{
		tmpPosition = rectTransform.anchoredPosition;
		if (startPosition.x + 150 > tmpPosition.x && startPosition.x - 150 < tmpPosition.x && startPosition.y + 150 > tmpPosition.y && startPosition.y - 150 < tmpPosition.y)
		{
			if (startPosition != tmpPosition)
			{
				if (50 < tmpPosition.x && 50 < tmpPosition.y)
					tmpPosition = new Vector2(100, 100);
				else if (50 < tmpPosition.x && -50 < tmpPosition.y)
					tmpPosition = new Vector2(100, 0);
				else if (50 < tmpPosition.x)
					tmpPosition = new Vector2(100, -100);
				else if (-50 < tmpPosition.x && 50 < tmpPosition.y)
					tmpPosition = new Vector2(0, 100);
				else if (-50 < tmpPosition.x && -50 < tmpPosition.y)
					tmpPosition = new Vector2(0, 0);
				else if (-50 < tmpPosition.x)
					tmpPosition = new Vector2(0, -100);
				else if (50 < tmpPosition.y)
					tmpPosition = new Vector2(-100, 100);
				else if (-50 < tmpPosition.y)
					tmpPosition = new Vector2(-100, 0);
				else
					tmpPosition = new Vector2(-100, -100);
				startPosition = tmpPosition;
			}
		}
		if (game.NoTilesUnder(tmpPosition,id))
        {
			startPosition = tmpPosition;
			rectTransform.anchoredPosition = new Vector2(startPosition.x, startPosition.y);
		}
		else
        {
			rectTransform.anchoredPosition = new Vector2(startPosition.x, startPosition.y);
		}
	}

	/// <summary>
	/// When we move our mouse, we move our tile as well and check if it's not going out of bounds.
	/// </summary>
	public void OnDrag(PointerEventData eventData)
	{
		rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
		BorderMap();
	}


	/// <summary>
	/// Check if the mouse is not out of bounds, if it is, put it back in the playing ground.
	/// </summary>
	private void BorderMap()
	{
		if (rectTransform.anchoredPosition.x > 100)
			rectTransform.anchoredPosition = new Vector2(100, rectTransform.anchoredPosition.y);
		else if (rectTransform.anchoredPosition.x < -100)
			rectTransform.anchoredPosition = new Vector2(-100, rectTransform.anchoredPosition.y);
		if (rectTransform.anchoredPosition.y > 100)
			rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, 100);
		else if (rectTransform.anchoredPosition.y < -100)
			rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, -100);
	}
}
