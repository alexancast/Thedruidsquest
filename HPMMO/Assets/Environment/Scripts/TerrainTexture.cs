using System.Collections;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class TerrainTexture : MonoBehaviour
{

    public Color livingColor;
    public Color deadColor;
    public int textureSize = 128;
    public int meshSize = 10;
    public float aoeSize = 100;

    private Texture2D texture;
    private Renderer renderer;

    public Transform player;

    public void Start()
    {

        renderer = GetComponent<Renderer>();
        renderer.material.mainTexture = BaseTexture();
        
    }

    public void Attack(CallbackContext context) {

        if (context.started)
        {
            StartCoroutine(RendText(10));
        }
        else if (context.canceled)
        {
            StopAllCoroutines();
        }

    }


    public IEnumerator RendText(float castTime)
    {
        float gradientSize = 0;
        float coveragePercentage = 0.5f; // Adjust this value as desired (e.g., 0.5 for 50% coverage)
        float elapsedTime = 0;
        Texture2D texture;
        Texture2D texture1 = (Texture2D)renderer.material.mainTexture;

        // Get the actual width and height of the texture
        int textureWidth = texture1.width;
        int textureHeight = texture1.height;

        // Calculate the maximum size based on the coverage percentage and texture resolution
        int maxSize = Mathf.FloorToInt(Mathf.Min(textureWidth, textureHeight) * coveragePercentage);

        while (elapsedTime < castTime)
        {
            texture = GenerateGradient(WorldToTexturePos(player.position), (int)gradientSize);
            renderer.material.mainTexture = OverlayTextures(texture1, texture);

            gradientSize = maxSize * elapsedTime / castTime;

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }



    public Vector2 WorldToTexturePos(Vector3 worldPos)
    {
        // Calculate the relative position of worldPos within the texture
        float textureX = (worldPos.x) / meshSize * textureSize;
        float textureY = (worldPos.z) / meshSize * textureSize;

        // Create a Vector2 using the calculated positions
        Vector2 texturePos = new Vector2(textureX, textureY);

        return texturePos;
    }

    public Texture2D BaseTexture()
    {
        Texture2D texture = new Texture2D(textureSize, textureSize);

        for (int x = 0; x < textureSize; x++)
        {
            for (int y = 0; y < textureSize; y++)
            {
                texture.SetPixel(x,y, livingColor);
            }
        }

        texture.Apply();

        return texture;
    }




    public Texture2D GenerateGradient(Vector2 centerOffset, int gradientSize)
    {
        Texture2D texture = new Texture2D(textureSize, textureSize);

        for (int y = 0; y < textureSize; y++)
        {
            for (int x = 0; x < textureSize; x++)
            {
                Vector2 currentPosition = new Vector2(x, y);
                Vector2 centerPosition = new Vector2(textureSize / 2, textureSize / 2) + centerOffset;

                float distance = Vector2.Distance(currentPosition, centerPosition);
                float normalizedDistance = distance / (gradientSize / 2);
                float alpha = 1 - normalizedDistance; // Inverted gradient for alpha value

                Color pixelColor = deadColor * alpha;
                texture.SetPixel(x, y, pixelColor);
            }
        }

        texture.Apply();
        return texture;
    }


    public Texture2D OverlayTextures(Texture2D baseTexture, Texture2D overlayTexture)
    {
        int width = baseTexture.width;
        int height = baseTexture.height;

        // Create a new texture to store the result
        Texture2D resultTexture = new Texture2D(width, height);

        // Iterate over each pixel of the textures and blend the colors
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Color baseColor = baseTexture.GetPixel(x, y);
                Color overlayColor = overlayTexture.GetPixel(x, y);

                // Overlay the colors using alpha blending
                Color resultColor = OverlayBlend(baseColor, overlayColor);

                resultTexture.SetPixel(x, y, resultColor);
            }
        }

        resultTexture.Apply();
        return resultTexture;
    }

    private Color OverlayBlend(Color baseColor, Color overlayColor)
    {
        // Calculate the overlay factor based on the overlay color's alpha
        float overlayFactor = overlayColor.a;

        float r = overlayColor.r * overlayFactor + baseColor.r;
        float g = overlayColor.g * overlayFactor + baseColor.g;
        float b = overlayColor.b * overlayFactor + baseColor.b;
        float a = Mathf.Max(baseColor.a, overlayColor.a); // Use the maximum alpha value

        // Create and return the resulting color
        return new Color(r, g, b, a);
    }




}
