using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoiseVisualiser : MonoBehaviour
{
    public RawImage noiseTextureImage;

    public int width = 256;
    public int height = 256;

    [Header("Offsets")]
    public float offsetX;
    public float offsetY;

    [Header("Radial gradient")]
    [Range(0, 1)] public float islandScale = 1;

    [Header("Perlin noise")]
    [Range(0.0001f, 0.1f)] public float detailScale = 0.1f;
    public int octaves;
    public float persistence;
    public float lacunarity;


    public void SetNoiseTexture()
    {

        noiseTextureImage.texture = GetTexture();
    }

    public Texture2D GetTexture() {

        return SubtractTextures(GeneratePerlinNoiseTexture(), GenerateInvertedRadialGradientTexture());

    }

    public Texture2D GeneratePerlinNoiseTexture()
    {
        Color[] pixels = new Color[width * height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float val = 0;
                float frequency = 1;
                float amplitude = 1;

                for (int octave = 0; octave < octaves; octave++)
                {
                    float sampleX = x * detailScale * frequency + offsetX;
                    float sampleY = y * detailScale * frequency + offsetY;
                    val += Mathf.PerlinNoise(sampleX, sampleY) * amplitude;

                    frequency *= lacunarity;
                    amplitude *= persistence;
                }

                pixels[x + width * y] = new Color(val, val, val, 1);
            }
        }

        Texture2D texture = new Texture2D(width, height);
        texture.SetPixels(pixels);
        texture.Apply();
        return texture;
    }

    public Texture2D GenerateInvertedRadialGradientTexture()
    {
        Texture2D texture = new Texture2D(width, height);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float distance = Vector2.Distance(new Vector2(x, y), new Vector2(width / 2, height / 2));
                float normalizedDistance = distance / (width * islandScale / 2);
                float val = normalizedDistance; // Inverterad gradient
                texture.SetPixel(x, y, new Color(val, val, val, 1));
            }
        }

        texture.Apply();
        return texture;
    }

    public Texture2D SubtractTextures(Texture2D textureA, Texture2D textureB)
    {
        int width = textureA.width;
        int height = textureA.height;

        Color[] pixelsA = textureA.GetPixels();
        Color[] pixelsB = textureB.GetPixels();

        Color[] resultPixels = new Color[width * height];

        for (int i = 0; i < resultPixels.Length; i++)
        {
            resultPixels[i] = pixelsA[i] - pixelsB[i];
            resultPixels[i].a = 1;
        }

        Texture2D resultTexture = new Texture2D(width, height);
        resultTexture.SetPixels(resultPixels);
        resultTexture.Apply();

        return resultTexture;
    }


    public void SetRandomCoord() {

        offsetX = Random.Range(0, 99999);
        offsetY = Random.Range(0, 99999);
    }
}