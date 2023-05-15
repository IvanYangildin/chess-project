using UnityEngine;
using UnityEngine.EventSystems;


[RequireComponent(typeof(Collider2D), typeof(SpriteRenderer))]
public class DraggableFigure : MonoBehaviour
{
    public Vector3 ScreenPoint => Camera.main.WorldToScreenPoint(gameObject.transform.position);
    public SpriteRenderer Renderer => gameObject.GetComponent<SpriteRenderer>();

    public void UpdatePosition()
    {
        Vector3 nextScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, ScreenPoint.z);

        Vector3 currPosition = Camera.main.ScreenToWorldPoint(nextScreenPoint);
        transform.position = currPosition;
    }

    public void Activate(Sprite sprite)
    {
        UpdatePosition();
        Renderer.sprite = sprite;
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        UpdatePosition();
    }


}