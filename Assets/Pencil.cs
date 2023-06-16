using System.Collections.Generic;
using UnityEngine;

public class Pencil : MonoBehaviour
{
    public float pencilDiameter = 0.1f;
    public GameObject targetSprite;

    private bool isDrawing = false;
    private LineRenderer lineRenderer;
    [SerializeField] private List<Vector2Int> pixelPositions;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        pixelPositions = new List<Vector2Int>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDrawing = true;
            lineRenderer.startWidth = pencilDiameter;
            lineRenderer.positionCount = 0;
            pixelPositions.Clear();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDrawing = false;
            FillSpritePixels();
        }

        if (isDrawing)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == targetSprite)
            {
                Vector2Int pixelPosition = WorldToPixelPosition(mousePosition);
                pixelPositions.Add(pixelPosition);

                lineRenderer.positionCount = pixelPositions.Count;
                lineRenderer.SetPosition(pixelPositions.Count - 1, mousePosition);
            }
        }
    }

    void FillSpritePixels()
    {
        SpriteRenderer spriteRenderer = targetSprite.GetComponent<SpriteRenderer>();
        Texture2D spriteTexture = spriteRenderer.sprite.texture;
        Rect spriteRect = spriteRenderer.sprite.rect;

        foreach (Vector2Int pixelPosition in pixelPositions)
        {
            // Konversi posisi piksel menjadi posisi dalam tekstur sprite
            int texX = Mathf.RoundToInt(pixelPosition.x - spriteRect.x);
            int texY = Mathf.RoundToInt(pixelPosition.y - spriteRect.y);

            // Set warna piksel menjadi hitam (contoh, Anda dapat mengubah warna sesuai kebutuhan)
            spriteTexture.SetPixel(texX, texY, Color.black);
        }

        // Terapkan perubahan pada tekstur sprite
        spriteTexture.Apply();
    }

    Vector2Int WorldToPixelPosition(Vector2 worldPosition)
    {
        Vector3 localPosition = targetSprite.transform.InverseTransformPoint(worldPosition);
        SpriteRenderer spriteRenderer = targetSprite.GetComponent<SpriteRenderer>();
        Rect spriteRect = spriteRenderer.sprite.rect;

        float pixelX = spriteRect.x + (localPosition.x + spriteRenderer.size.x * 0.5f) / spriteRenderer.size.x * spriteRect.width;
        float pixelY = spriteRect.y + (localPosition.y + spriteRenderer.size.y * 0.5f) / spriteRenderer.size.y * spriteRect.height;

        return new Vector2Int(Mathf.RoundToInt(pixelX), Mathf.RoundToInt(pixelY));
    }
}



