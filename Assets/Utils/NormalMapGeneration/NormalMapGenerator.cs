using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[ExecuteInEditMode]
public class NormalMapGenerator : MonoBehaviour
{
    [SerializeField] private ComputeShader _gaussianShader;
    [SerializeField] private ComputeShader _distanceShader;

    [SerializeField] private Texture2D sourceTexture; // Input texture
    [SerializeField] private float normalStrength = 100.0f; // Controls the steepness of slopes
    [SerializeField] [Range(0, 10)] private int blurEdges = 4; // Controls the width of the edges (larger value means wider edges)

    [SerializeField] private float bumpHeight = 100.0f; // Controls the height of the bump effect
    [SerializeField] [Range(0, 10)] private int blurBump = 4; // Controls the width of the edges (larger value means wider edges)
    [SerializeField] private int slopeDistance = 40; // Controls the distance over which the slope extends inward

    [SerializeField] [Range(0, 10)] private int finalBlur = 4; // Controls the width of the edges (larger value means wider edges)

    public bool update = false;

    public float GAUSSIAN_WEIGHT = 20.0f; 

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
        Texture2D normalMap = new Texture2D(width, height);

        // Apply Gaussian blur to the normal map
        Texture2D blurredTexture = ApplyGaussianBlur(sourceTexture, blurEdges * 2);

        // Generate a height map with the "puffed-up" effect
        Texture2D heightMap = GenerateHeightMapWithSlope(sourceTexture);

        heightMap = ApplyGaussianBlur(heightMap, blurBump * 2);
        heightMap.filterMode = FilterMode.Point;
        
        
        //ApplyNormalMapToChild(heightMap);

        // Generate the normal map with the bump effect
        for (int x = 1; x < width - 1; x++)
        {
            for (int y = 1; y < height - 1; y++)
            {
                Color heightPixel = heightMap.GetPixel(x, y);
                Color leftHeightPixel = heightMap.GetPixel(x - 1, y);
                Color rightHeightPixel = heightMap.GetPixel(x + 1, y);
                Color downHeightPixel = heightMap.GetPixel(x, y - 1);
                Color upHeightPixel = heightMap.GetPixel(x, y + 1);

                float heightCurr = heightPixel.r;
                float heightL = 1 - leftHeightPixel.r * leftHeightPixel.a;
                float heightR = 1 - rightHeightPixel.r  * rightHeightPixel.a;
                float heightD = 1 - downHeightPixel.r * downHeightPixel.a;
                float heightU = 1 - upHeightPixel.r * upHeightPixel.a;

                //Debug.Log(heightCurr);
                // Compute gradients (normal)
                float distanceX = (heightL - heightR) * bumpHeight;
                float distanceY = (heightD - heightU) * bumpHeight;

                distanceX *= Mathf.Pow(heightCurr, blurBump);
                distanceY *= Mathf.Pow(heightCurr, blurBump);


                Color blurPixel = blurredTexture.GetPixel(x, y);
                Color leftBlurPixel = blurredTexture.GetPixel(x - 1, y);
                Color rightBlurPixel = blurredTexture.GetPixel(x + 1, y);
                Color downBlurPixel = blurredTexture.GetPixel(x, y - 1);
                Color upBlurPixel = blurredTexture.GetPixel(x, y + 1);

                float blurCurr = 1 - (blurPixel.r + blurPixel.g + blurPixel.b) / 3f * blurPixel.a;
                float blurL = 1 - (leftBlurPixel.r + leftBlurPixel.g + leftBlurPixel.b) / 3f * leftBlurPixel.a;
                float blurR = 1 - (rightBlurPixel.r + rightBlurPixel.g + rightBlurPixel.b) / 3f * rightBlurPixel.a;
                float blurD = 1 - (downBlurPixel.r + downBlurPixel.g + downBlurPixel.b) / 3f * downBlurPixel.a;
                float blurU = 1 - (upBlurPixel.r + upBlurPixel.g + upBlurPixel.b) / 3f * upBlurPixel.a;

                // Compute gradients (normal)
                float embossX = (blurR - blurL) * blurCurr * normalStrength;
                float embossY = (blurU - blurD) * blurCurr * normalStrength;


                float normalR = embossX * 3f/2f + distanceX * 3f/2f;
                float normalG = embossY * 3f/2f + distanceY * 3f/2f;
                float normalB = 1f;

                Vector3 normal = new Vector3(normalR, normalG, normalB).normalized;

                Color normalColor = new Color(
                    normal.x * 0.5f + 0.5f,
                    normal.y * 0.5f + 0.5f,
                    normal.z * 0.5f + 0.5f);

                // if (leftBlurPixel.a == 0 &&
                //     rightBlurPixel.a == 0 &&
                //     downBlurPixel.a == 0 &&
                //     upBlurPixel.a == 0) normalColor = new Color(0.5f,0.5f,1f);

                normalMap.SetPixel(x, y, normalColor);
            }
        }
        normalMap.Apply();

        // Apply Gaussian blur to the normal map
        Texture2D blurredNormalMap = ApplyGaussianBlur(normalMap, finalBlur*2);
        blurredNormalMap.filterMode = FilterMode.Point;

        byte[] _bytes = blurredNormalMap.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath  + "/Characters" + "/test.png" , _bytes);

        ApplyNormalMapToChild(blurredNormalMap);

        ApplyNormalMap(blurredNormalMap);
    }

    Texture2D GenerateHeightMapWithSlope(Texture2D source)
    {
        int width = source.width;
        int height = source.height;

        RenderTexture renderTexture = new RenderTexture(width, height, 0, RenderTextureFormat.ARGBFloat);
        renderTexture.enableRandomWrite = true;
        renderTexture.Create();

        // Pass the Gaussian size to the compute shader
        _distanceShader.SetInt("_distance", slopeDistance);

        // Pass buffers and textures
        int kernelHandle = _distanceShader.FindKernel("CSMain");
        _distanceShader.SetTexture(kernelHandle, "_sourceTex", source);
        _distanceShader.SetTexture(kernelHandle, "_resultTex", renderTexture);

        // Start the shader up
        _distanceShader.GetKernelThreadGroupSizes(kernelHandle, out uint groupSizeX, out uint groupSizeY, out _);
        int threadGroupsX = Mathf.CeilToInt(sourceTexture.width / groupSizeX);
        int threadGroupsY = Mathf.CeilToInt(sourceTexture.height / groupSizeY);
        _distanceShader.Dispatch(kernelHandle, threadGroupsX, threadGroupsY, 1);

        // Get data
        RenderTexture.active = renderTexture;
        Texture2D heightMap = new Texture2D(width, height);
        heightMap.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        heightMap.Apply();
        
        // Cleanup
        RenderTexture.active = null;

        return heightMap;

        /*
        // First pass to find distances
        // Queue<Vector2Int> queue = new Queue<Vector2Int>();
        // for (int y = 1; y < height - 1; y++)
        // {
        //     for (int x = 1; x < width - 1; x++)
        //     {
        //         float minDistance = float.MaxValue;

        //         if (source.GetPixel(x, y).a > 0.1f)
        //         {
        //             queue.Clear();
        //             queue.Enqueue(new Vector2Int(x, y));
                    
        //             bool[,] visitMap = new bool[width, height];
        //             while (queue.Count > 0)
        //             {
        //                 Vector2Int currCoord = queue.Dequeue();

        //                 float distance = ((Vector2)(new Vector2Int(x, y) - currCoord)).magnitude;

        //                 // Check is we are at border
        //                 if (source.GetPixel(currCoord.x, currCoord.y).a <= 0.1f)
        //                 {
        //                     if (distance < minDistance) 
        //                     {
        //                         minDistance = distance;
        //                         break;
        //                     }
        //                 }

        //                 if (distance < slopeDistance)
        //                 {
        //                     Vector2Int right = new Vector2Int(Mathf.Min(width - 1, currCoord.x + 1), currCoord.y);
        //                     if (!visitMap[right.x, right.y]) 
        //                     {
        //                         queue.Enqueue(right);
        //                         visitMap[right.x, right.y] = true;
        //                     }

        //                     Vector2Int left = new Vector2Int(Mathf.Max(0, currCoord.x - 1), currCoord.y);
        //                     if (!visitMap[left.x, left.y]) 
        //                     {
        //                         queue.Enqueue(left);
        //                         visitMap[left.x, left.y] = true;
        //                     }

        //                     Vector2Int up = new Vector2Int(currCoord.x, Mathf.Min(height - 1, currCoord.y + 1));
        //                     if (!visitMap[up.x, up.y]) 
        //                     {
        //                         queue.Enqueue(up);
        //                         visitMap[up.x, up.y] = true;
        //                     }

        //                     Vector2Int down = new Vector2Int(currCoord.x, Mathf.Max(0, currCoord.y - 1));
        //                     if (!visitMap[down.x, down.y]) 
        //                     {
        //                         queue.Enqueue(down);
        //                         visitMap[down.x, down.y] = true;
        //                     }
        //                 }
        //             }
        //         }
                
        //         float heightValue = 1-Mathf.Clamp01(minDistance / slopeDistance);
        //         Color heightColor = new Color(heightValue, heightValue, heightValue);

        //         if (source.GetPixel(x, y).a == 0) heightColor = new Color(1f, 1f, 1f);
        //         heightMap.SetPixel(x, y, heightColor);
        //     }
        // }

        // First pass: find the distance to the nearest edge for each pixel
        // for (int y = 1; y < height - 1; y++)
        // {
        //     for (int x = 1; x < width - 1; x++)
        //     {
        //         if (source.GetPixel(x, y).a > 0)
        //         {
        //             // Calculate distances to neighboring transparent pixels (edges)
        //             if (source.GetPixel(x - 1, y).a == 0 || source.GetPixel(x + 1, y).a == 0 ||
        //                 source.GetPixel(x, y - 1).a == 0 || source.GetPixel(x, y + 1).a == 0)
        //             {
        //                 distanceMap[x, y] = 0;
        //             }
        //             else
        //             {
        //                 // Calculate the minimum distance to the edge
        //                 float minDistance = Mathf.Min(
        //                     distanceMap[x - 1, y] + 1,
        //                     distanceMap[x + 1, y] + 1,
        //                     distanceMap[x, y - 1] + 1,
        //                     distanceMap[x, y + 1] + 1,
        //                     distanceMap[x - 1, y - 1] + 1.414f,
        //                     distanceMap[x + 1, y - 1] + 1.414f,
        //                     distanceMap[x - 1, y + 1] + 1.414f,
        //                     distanceMap[x + 1, y + 1] + 1.414f
        //                     );

        //                 distanceMap[x, y] = minDistance;
        //             }
        //         }
        //     }
        // }

        // // Second pass: find the distance to the nearest edge for each pixel
        // for (int y = height - 2; y > 1; y--)
        // {
        //     for (int x = width - 1; x > 1; x--)
        //     {
        //         if (source.GetPixel(x, y).a > 0)
        //         {
        //             // Calculate distances to neighboring transparent pixels (edges)
        //             if (source.GetPixel(x - 1, y).a == 0 || source.GetPixel(x + 1, y).a == 0 ||
        //                 source.GetPixel(x, y - 1).a == 0 || source.GetPixel(x, y + 1).a == 0)
        //             {
        //                 distanceMap[x, y] = 0;
        //             }
        //             else
        //             {
        //                 // Calculate the minimum distance to the edge
        //                 float minDistance = Mathf.Min(
        //                     distanceMap[x - 1, y] + 1,
        //                     distanceMap[x + 1, y] + 1,
        //                     distanceMap[x, y - 1] + 1,
        //                     distanceMap[x, y + 1] + 1,
        //                     distanceMap[x - 1, y - 1] + 1.414f,
        //                     distanceMap[x + 1, y - 1] + 1.414f,
        //                     distanceMap[x - 1, y + 1] + 1.414f,
        //                     distanceMap[x + 1, y + 1] + 1.414f
        //                     );

        //                 distanceMap[x, y] = minDistance;
        //             }
        //         }
        //     }
        // }

        // Second pass: generate the height map using the distance map
        // for (int y = 0; y < height; y++)
        // {
        //     for (int x = 0; x < width; x++)
        //     {
        //         float distance = distanceMap[x, y];
        //         float heightValue = 1-Mathf.Clamp01(distance / slopeDistance);
        //         Color heightColor = new Color(heightValue, heightValue, heightValue);

        //         if (source.GetPixel(x, y).a == 0) heightColor = new Color(1f, 1f, 1f);
        //         heightMap.SetPixel(x, y, heightColor);
        //     }
        // }
        */
    }

    Texture2D ApplyGaussianBlur(Texture2D source, int radius)
    {
        int width = source.width;
        int height = source.height;

        if (radius == 0)
        {
            return source;
        }
        
        RenderTexture renderTexture = new RenderTexture(width, height, 0, RenderTextureFormat.ARGBFloat);
        renderTexture.enableRandomWrite = true;
        renderTexture.Create();

        // Create kernel
        float[] kernelAlt = GetGaussianKernel(radius, GAUSSIAN_WEIGHT);

        // Create buffer
        ComputeBuffer gaussianKernelBuffer = new ComputeBuffer(kernelAlt.Length, sizeof(float));
        gaussianKernelBuffer.SetData(kernelAlt);

        // Pass the Gaussian size to the compute shader
        _gaussianShader.SetInt("_blurRadius", radius);

        // Pass buffers and textures
        int kernelHandle = _gaussianShader.FindKernel("CSMain");
        _gaussianShader.SetTexture(kernelHandle, "_sourceTex", source);
        _gaussianShader.SetTexture(kernelHandle, "_resultTex", renderTexture);
        _gaussianShader.SetBuffer(kernelHandle, "_gaussianKernel", gaussianKernelBuffer);

        // Start the shader up
        _gaussianShader.GetKernelThreadGroupSizes(kernelHandle, out uint groupSizeX, out uint groupSizeY, out _);
        int threadGroupsX = Mathf.CeilToInt(sourceTexture.width / groupSizeX);
        int threadGroupsY = Mathf.CeilToInt(sourceTexture.height / groupSizeY);
        _gaussianShader.Dispatch(kernelHandle, threadGroupsX, threadGroupsY, 1);

        // Get data
        RenderTexture.active = renderTexture;
        Texture2D blurredTexture = new Texture2D(width, height);
        blurredTexture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        blurredTexture.Apply();
        
        // Cleanup
        gaussianKernelBuffer.Dispose();
        RenderTexture.active = null;

        return blurredTexture;
    }

    public static float[] GetGaussianKernel(int length, float weight)
    {
        float[] kernel = new float[length * length];
        float sumTotal = 0;

        int kernelRadius = length / 2;
        float calculatedEuler = 1.0f / (2.0f * Mathf.PI * Mathf.Pow(weight, 2));

        for (int filterY = 0; filterY < length; filterY++)
        {
            for (int filterX = 0; filterX < kernelRadius; filterX++)
            {
                int filterXOffset = filterX - kernelRadius;
                int filterYOffset = filterY - kernelRadius;

                float distance = ((filterXOffset * filterXOffset) + (filterYOffset * filterYOffset)) / (2 * (weight * weight));

                kernel[filterY * length + filterX] = calculatedEuler * Mathf.Exp(-distance);

                sumTotal += kernel[filterY * length + filterX];
            }
        }

        for (int y = 0; y < length; y++)
        {
            for (int x = 0; x < length; x++)
            {
                kernel[y * length + x] = kernel[y * length + x] * (1.0f / sumTotal);
            }
        }

        return kernel;
    }

    void ApplyNormalMap(Texture2D normalMap)
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        //renderer.sprite = Sprite.Create(normalMap, new Rect(0, 0, normalMap.width, normalMap.height), new Vector2(0.5f, 0.5f));
        renderer.sharedMaterial.SetTexture("_NormalMap", normalMap);
    }

    void ApplyNormalMapToChild(Texture2D normalMap)
    {
        SpriteRenderer renderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        renderer.sprite = Sprite.Create(normalMap, new Rect(0, 0, normalMap.width, normalMap.height), new Vector2(0.5f, 0.5f));
        //renderer.sharedMaterial.SetTexture("_NormalMap", normalMap);
    }
}
