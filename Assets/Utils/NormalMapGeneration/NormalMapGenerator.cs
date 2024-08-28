using System;
using UnityEngine;

[ExecuteInEditMode]
public class NormalMapGenerator : MonoBehaviour
{
    public Texture2D sourceTexture; // Input texture
    public float normalStrength = 1.0f; // Controls the steepness of slopes
    public float bumpHeight = 1.0f; // Controls the height of the bump effect
    public int blurRadius = 4; // Controls the width of the edges (larger value means wider edges)
    public float slopeDistance = 20.0f; // Controls the distance over which the slope extends inward

    public bool update = false;

    private const float GAUSSIAN_WEIGHT = 2.0f; 
    private Texture2D normalMap;

    private void Update()
    {
        if (update)
        {
            update = false;
            GenerateNormalMap();
        }
    }

    void GenerateNormalMap()
    {
        int width = sourceTexture.width;
        int height = sourceTexture.height;
        normalMap = new Texture2D(width, height);

        // Apply Gaussian blur to the normal map
        Texture2D blurredTexture = ApplyGaussianBlur(sourceTexture, blurRadius);

        // Generate a height map with the "puffed-up" effect
        Texture2D heightMap = GenerateHeightMapWithSlope(blurredTexture);
        heightMap = ApplyGaussianBlur(heightMap, blurRadius);
        
        // Generate the normal map with the bump effect
        for (int x = 1; x < width - 1; x++)
        {
            for (int y = 1; y < height - 1; y++)
            {
                // Fetch height values
                float heightCurrBump = 1 - heightMap.GetPixel(x, y).grayscale * heightMap.GetPixel(x, y).a;

                float heightBumpL = heightMap.GetPixel(x - 1, y).grayscale * heightMap.GetPixel(x - 1, y).a;
                float heightBumpR = heightMap.GetPixel(x + 1, y).grayscale * heightMap.GetPixel(x + 1, y).a;
                float heightBumpD = heightMap.GetPixel(x, y - 1).grayscale * heightMap.GetPixel(x, y - 1).a;
                float heightBumpU = heightMap.GetPixel(x, y + 1).grayscale * heightMap.GetPixel(x, y + 1).a;

                // Compute gradients (normal)
                float dXBump = (heightBumpL - heightBumpR) * heightCurrBump * bumpHeight;
                float dYBump = (heightBumpD - heightBumpU) * heightCurrBump * bumpHeight;

                // Fetch height values
                float heightCurr = 1 - blurredTexture.GetPixel(x, y).grayscale * blurredTexture.GetPixel(x, y).a;

                float heightL = blurredTexture.GetPixel(x - 1, y).grayscale * blurredTexture.GetPixel(x - 1, y).a;
                float heightR = blurredTexture.GetPixel(x + 1, y).grayscale * blurredTexture.GetPixel(x + 1, y).a;
                float heightD = blurredTexture.GetPixel(x, y - 1).grayscale * blurredTexture.GetPixel(x, y - 1).a;
                float heightU = blurredTexture.GetPixel(x, y + 1).grayscale * blurredTexture.GetPixel(x, y + 1).a;

                // Compute gradients (normal)
                float dX = (heightL - heightR) * heightCurr * normalStrength;
                float dY = (heightD - heightU) * heightCurr * normalStrength;

                Vector3 normal = new Vector3(dX * 3f/2f + dXBump * 3f/2f, dY * 3f/2f + dYBump * 3f/2f, 1.0f).normalized;

                Color normalColor = new Color(normal.x * 0.5f + 0.5f, normal.y * 0.5f + 0.5f, normal.z * 0.5f + 0.5f);
                normalMap.SetPixel(x, y, normalColor);
            }
        }
        normalMap.Apply();

        // Apply Gaussian blur to the normal map
        //normalMap = ApplyGaussianBlur(normalMap, blurRadius);

        ApplyNormalMap(normalMap);
    }

    Texture2D GenerateHeightMapWithSlope(Texture2D source)
    {
        int width = source.width;
        int height = source.height;
        Texture2D heightMap = new Texture2D(width, height);

        // Create a buffer to store distances from the edge
        float[,] distanceMap = new float[width, height];

        // Initialize the distance map with a large value
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                distanceMap[x, y] = float.MaxValue;
            }
        }

        // First pass: find the distance to the nearest edge for each pixel
        for (int y = 1; y < height - 1; y++)
        {
            for (int x = 1; x < width - 1; x++)
            {
                if (source.GetPixel(x, y).a > 0)
                {
                    // Calculate distances to neighboring transparent pixels (edges)
                    if (source.GetPixel(x - 1, y).a == 0 || source.GetPixel(x + 1, y).a == 0 ||
                        source.GetPixel(x, y - 1).a == 0 || source.GetPixel(x, y + 1).a == 0)
                    {
                        distanceMap[x, y] = 0;
                    }
                    else
                    {
                        // Calculate the minimum distance to the edge
                        float minDistance = Mathf.Min(
                            distanceMap[x - 1, y] + 1,
                            distanceMap[x + 1, y] + 1,
                            distanceMap[x, y - 1] + 1,
                            distanceMap[x, y + 1] + 1,
                            distanceMap[x - 1, y - 1] + 1.414f,
                            distanceMap[x + 1, y - 1] + 1.414f,
                            distanceMap[x - 1, y + 1] + 1.414f,
                            distanceMap[x + 1, y + 1] + 1.414f
                            );

                        distanceMap[x, y] = minDistance;
                    }
                }
            }
        }

        // Second pass: find the distance to the nearest edge for each pixel
        for (int y = height - 2; y > 1; y--)
        {
            for (int x = width - 1; x > 1; x--)
            {
                if (source.GetPixel(x, y).a > 0)
                {
                    // Calculate distances to neighboring transparent pixels (edges)
                    if (source.GetPixel(x - 1, y).a == 0 || source.GetPixel(x + 1, y).a == 0 ||
                        source.GetPixel(x, y - 1).a == 0 || source.GetPixel(x, y + 1).a == 0)
                    {
                        distanceMap[x, y] = 0;
                    }
                    else
                    {
                        // Calculate the minimum distance to the edge
                        float minDistance = Mathf.Min(
                            distanceMap[x - 1, y] + 1,
                            distanceMap[x + 1, y] + 1,
                            distanceMap[x, y - 1] + 1,
                            distanceMap[x, y + 1] + 1,
                            distanceMap[x - 1, y - 1] + 1.414f,
                            distanceMap[x + 1, y - 1] + 1.414f,
                            distanceMap[x - 1, y + 1] + 1.414f,
                            distanceMap[x + 1, y + 1] + 1.414f
                            );

                        distanceMap[x, y] = minDistance;
                    }
                }
            }
        }

        // Second pass: generate the height map using the distance map
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float distance = distanceMap[x, y];
                float heightValue = Mathf.Clamp01(distance / slopeDistance);
                Color heightColor = new Color(heightValue, heightValue, heightValue);

                if (source.GetPixel(x, y).a == 0) heightColor = new Color(0f, 0f, 0f);
                heightMap.SetPixel(x, y, heightColor);
            }
        }

        heightMap.Apply();
        return heightMap;
    }

    Texture2D ApplyGaussianBlur(Texture2D source, int radius)
    {
        int width = source.width;
        int height = source.height;
        Texture2D blurred = new Texture2D(width, height);

        float[,] kernelAlt = GetGaussianKernel(radius, GAUSSIAN_WEIGHT);

        int halfRadius = radius / 2;

        // blur
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Color blurredPixel = new Color();
                float kernelSum = 0;

                for (int rectY = -halfRadius; rectY < halfRadius; rectY++)
                {
                    for (int rectX = -halfRadius; rectX < halfRadius; rectX++)
                    {
                        int sampleX = Mathf.Clamp(x + rectX, 0, width - 1);
                        int sampleY = Mathf.Clamp(y + rectY, 0, height - 1);

                        Color sampleColor = source.GetPixel(sampleX, sampleY);
                        float kernelValue = kernelAlt[rectY + halfRadius, rectX + halfRadius];

                        blurredPixel += sampleColor * kernelValue;
                        kernelSum += kernelValue;
                    }
                }

                blurred.SetPixel(x, y, blurredPixel / kernelSum);
            }
        }

        blurred.Apply();
        return blurred;
    }

    public static float[,] GetGaussianKernel(int length, float weight)
    {
        float[,] kernel = new float[length, length];
        float sumTotal = 0;

        int kernelRadius = length / 2;
        float calculatedEuler = 1.0f / (2.0f * Mathf.PI * Mathf.Pow(weight, 2));

        for (int filterY = -kernelRadius; filterY < kernelRadius; filterY++)
        {
            for (int filterX = -kernelRadius; filterX < kernelRadius; filterX++)
            {
                float distance = ((filterX * filterX) + (filterY * filterY)) / (2 * (weight * weight));

                kernel[filterY + kernelRadius, filterX + kernelRadius] = calculatedEuler * Mathf.Exp(-distance);

                sumTotal += kernel[filterY + kernelRadius, filterX + kernelRadius];
            }
        }

        for (int y = 0; y < length; y++)
        {
            for (int x = 0; x < length; x++)
            {
                kernel[y, x] = kernel[y, x] * (1.0f / sumTotal);
            }
        }

        return kernel;
    }

    void ApplyNormalMap(Texture2D normalMap)
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.sprite = Sprite.Create(normalMap, new Rect(0, 0, normalMap.width, normalMap.height), new Vector2(0.5f, 0.5f));
        //renderer.sharedMaterial.SetTexture("_NormalMap", normalMap);
    }
}
