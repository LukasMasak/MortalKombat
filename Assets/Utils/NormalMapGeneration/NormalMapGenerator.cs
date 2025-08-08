using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[ExecuteInEditMode]
public class NormalMapGenerator : MonoBehaviour
{
    [Header("Shaders")]
    [SerializeField] private ComputeShader _gaussianShader;
    [SerializeField] private ComputeShader _distanceShader;
    [SerializeField] private ComputeShader _normalMapShader;

    [SerializeField] private ComputeShader _allInOneShader;
    [SerializeField] private bool useAllInOne = true;


    [Header("Manual Debugs")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Texture2D sourceTexture; // Input texture
    [SerializeField] private float normalStrength = 20.0f; // Controls the steepness of slopes
    [SerializeField][Range(0, 10)] private int blurEdges = 5; // Controls the width of the edges (larger value means wider edges)

    [SerializeField] private float bumpHeight = 80.0f; // Controls the height of the bump effect
    [SerializeField][Range(1, 10)] private int blurBump = 5; // Controls the width of the edges (larger value means wider edges)
    [SerializeField][Range(1f, 10f)] private float blurSoften = 1;

    [SerializeField][Range(0f, 1f)] private float slopeDistance = 0.1f; // Controls the distance over which the slope extends inward

    [SerializeField][Range(0, 10)] private int finalBlur = 5; // Controls the width of the edges (larger value means wider edges)


    public float GAUSSIAN_WEIGHT = 10f;


    public bool _update = false;

    private void Update()
    {
        if (_update)
        {
            _update = false;
            NormalMapSettings settings = new NormalMapSettings();
            settings.sourceTexture = sourceTexture;
            settings.strengthEdges = normalStrength;
            settings.blurEdgesRadius = blurEdges;
            settings.strengthBorder = bumpHeight;
            settings.blurBorderRadius = blurBump;
            settings.softenBorder = blurSoften;
            settings.slopePercentageBorder = slopeDistance;
            settings.finalBlurRadius = finalBlur;

            Texture2D test = GenerateNormalMap(settings);
            spriteRenderer.sprite = Sprite.Create(test, new Rect(0, 0, test.width, test.height), new Vector2(0.5f, 0.5f));
        }
    }

    public Texture2D GenerateNormalMap(NormalMapSettings settings)
    {
        //Debug.Log("Called with " + sourceTexture.name + " tex name, " + normalStrength + " strenght edge, " + blurEdges + " edge blur " + bumpHeight + " bump height, " + blurBump + " blur bump, " + softenBump + " soften bump, " + slopePercentage + " slope percentagem, " + finalBlur + " final blur");

        //Debug.Log("texture " + sourceTexture.mipmapLimitGroup + " mipmaplimitgroup, " + sourceTexture.requestedMipmapLevel + " requstedmipmaplevel, " + sourceTexture.streamingMipmaps + " streamingmipmaps, " + sourceTexture.hideFlags + " hide flags, " + sourceTexture.streamingMipmapsPriority + " streamingmipmapspriority, " + sourceTexture.imageContentsHash + " image content hash, " + sourceTexture.updateCount + " updatecount, " + sourceTexture.minimumMipmapLevel + " minimummipmap, " + sourceTexture.mipMapBias + " mipmap bias, " + sourceTexture.mipmapCount + " mipmapcount");
        Texture2D result;

        double startTime = Time.realtimeSinceStartupAsDouble;

        if (useAllInOne)
        {
            result = CSAllInOne(settings);
        }
        else
        {
            // Apply Gaussian blur to the normal map
            //Texture2D blurredTexture = CSApplyGaussianBlur(settings.sourceTexture, settings.blurEdgesRadius);

            // Generate a height map with the "puffed-up" effect
            Texture2D heightMap = CSGenerateHeightMap(settings.sourceTexture, Mathf.RoundToInt((settings.sourceTexture.width / 2f) * settings.slopePercentageBorder));
            heightMap = CSApplyGaussianBlur(heightMap, settings.blurBorderRadius);

            // Generate the normal map with the bump effect
            //Texture2D normalMap = CSApplySourceAndHeightMap(blurredTexture, heightMap, settings.strengthEdges, settings.strengthBorder, settings.blurBorderRadius, settings.softenBorder);

            // Apply Gaussian blur to the normal map
            result = heightMap;//CSApplyGaussianBlur(normalMap, settings.finalBlurRadius);
        }

        double endTime = Time.realtimeSinceStartupAsDouble;

        Debug.Log("Took " + (endTime - startTime) * 1000 + " ms");

        return result;
    }


    private Texture2D CSApplySourceAndHeightMap(Texture2D source, Texture2D heightMap,
                                                     float normalStrength, float bumpHeight, int bumpBlur, float softenBump)
    {
        int width = source.width;
        int height = source.height;

        RenderTexture renderTexture = new RenderTexture(width, height, 0, RenderTextureFormat.ARGBFloat);
        renderTexture.enableRandomWrite = true;
        renderTexture.Create();

        // Pass buffers and textures
        int kernelHandle = _normalMapShader.FindKernel("CSMain");
        _normalMapShader.SetTexture(kernelHandle, "_sourceTex", source);
        _normalMapShader.SetTexture(kernelHandle, "_heightMapTex", heightMap);
        _normalMapShader.SetTexture(kernelHandle, "_resultTex", renderTexture);
        _normalMapShader.SetFloat("_normalStrength", normalStrength);
        _normalMapShader.SetFloat("_bumpStrength", bumpHeight);
        _normalMapShader.SetInt("_bumpBlur", bumpBlur);
        _normalMapShader.SetFloat("_softenBump", softenBump);

        // Start the shader up
        _normalMapShader.GetKernelThreadGroupSizes(kernelHandle, out uint groupSizeX, out uint groupSizeY, out _);
        int threadGroupsX = Mathf.CeilToInt(width / (float)groupSizeX);
        int threadGroupsY = Mathf.CeilToInt(height / (float)groupSizeY);
        _normalMapShader.Dispatch(kernelHandle, threadGroupsX, threadGroupsY, 1);

        // Get data
        RenderTexture.active = renderTexture;
        Texture2D normalTexture = new Texture2D(width, height);
        normalTexture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        normalTexture.Apply();

        // Cleanup
        RenderTexture.active = null;

        return normalTexture;
    }


    private Texture2D CSGenerateHeightMap(Texture2D source, int slopeDistance)
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
        int threadGroupsX = Mathf.CeilToInt(width / (float)groupSizeX);
        int threadGroupsY = Mathf.CeilToInt(height / (float)groupSizeY);
        _distanceShader.Dispatch(kernelHandle, threadGroupsX, threadGroupsY, 1);

        // Get data
        RenderTexture.active = renderTexture;
        Texture2D heightMap = new Texture2D(width, height);
        heightMap.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        heightMap.Apply();

        // Cleanup
        RenderTexture.active = null;

        return heightMap;
    }


    private Texture2D CSApplyGaussianBlur(Texture2D source, int radius)
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

        // Create gaussian kernel
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
        int threadGroupsX = Mathf.CeilToInt(width / (float)groupSizeX);
        int threadGroupsY = Mathf.CeilToInt(height / (float)groupSizeY);
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


    private static float[] GetGaussianKernel(int radius, float weight)
    {
        int length = radius * 2 + 1;
        float[] kernel = new float[length * length];
        float sumTotal = 0;

        //int kernelRadius = halfRadius / 2;

        // G(x, y) = (1 / 2πσ²) * exp(-(x² + y²) / 2σ²)
        float calculatedEuler = 1.0f / (2.0f * Mathf.PI * Mathf.Pow(weight, 2));

        for (int filterY = 0; filterY < length; filterY++)
        {
            for (int filterX = 0; filterX < length; filterX++)
            {
                int filterXOffset = filterX - radius;
                int filterYOffset = filterY - radius;

                float distance = ((filterXOffset * filterXOffset) + (filterYOffset * filterYOffset)) / (2f * (weight * weight));

                kernel[filterY * length + filterX] = calculatedEuler * Mathf.Exp(-distance);

                sumTotal += kernel[filterY * length + filterX];
            }
        }

        // Normalize
        for (int y = 0; y < length; y++)
        {
            for (int x = 0; x < length; x++)
            {
                kernel[y * length + x] /= sumTotal;
            }
        }

        return kernel;
    }
    
    private Texture2D CSAllInOne(NormalMapSettings settings)
    {
        int width = settings.sourceTexture.width;
        int height = settings.sourceTexture.height;

        // Render Texture descriptor for render textures
        RenderTextureDescriptor rendTexDesc = new RenderTextureDescriptor(width, height);
        rendTexDesc.colorFormat = RenderTextureFormat.Default;
        rendTexDesc.sRGB = true;
        rendTexDesc.enableRandomWrite = true;

        // Result and temp texture
        RenderTexture resultRendTex = new RenderTexture(rendTexDesc);
        resultRendTex.enableRandomWrite = true;
        resultRendTex.Create();
        RenderTexture srcBlurRendTex = RenderTexture.GetTemporary(rendTexDesc);
        srcBlurRendTex.enableRandomWrite = true;
        srcBlurRendTex.Create();
        RenderTexture distRendTex = RenderTexture.GetTemporary(rendTexDesc);
        distRendTex.enableRandomWrite = true;
        distRendTex.Create();

        // Create gaussian kernels
        float[] kernelEdge = GetGaussianKernel(settings.blurEdgesRadius, GAUSSIAN_WEIGHT);
        float[] kernelBorder = GetGaussianKernel(settings.blurBorderRadius, GAUSSIAN_WEIGHT);
        float[] kernelFinal = GetGaussianKernel(settings.finalBlurRadius, GAUSSIAN_WEIGHT);

        // Create gauss kernel buffers
        ComputeBuffer kernelEdgeBlurBuffer = new ComputeBuffer(kernelEdge.Length, sizeof(float));
        kernelEdgeBlurBuffer.SetData(kernelEdge);
        ComputeBuffer kernelBorderBlurBuffer = new ComputeBuffer(kernelBorder.Length, sizeof(float));
        kernelBorderBlurBuffer.SetData(kernelBorder);
        ComputeBuffer kernelFinalBlurBuffer = new ComputeBuffer(kernelFinal.Length, sizeof(float));
        kernelFinalBlurBuffer.SetData(kernelFinal);

        // Pass the normal map parameters to the compute shader
        _allInOneShader.SetFloat("_strengthEdges", settings.strengthEdges);
        _allInOneShader.SetInt("_blurEdgesRadius", settings.blurEdgesRadius);
        _allInOneShader.SetFloat("_strengthBorder", settings.strengthBorder);
        _allInOneShader.SetInt("_blurBorderRadius", settings.blurBorderRadius);
        _allInOneShader.SetFloat("_softenBorder", settings.softenBorder);
        _allInOneShader.SetFloat("_slopePercentageBorder", settings.slopePercentageBorder);
        _allInOneShader.SetInt("_finalBlurRadius", settings.finalBlurRadius);

        // Pass buffers and textures
        int kernelHandle = _allInOneShader.FindKernel("CSMain");
        _allInOneShader.SetTexture(kernelHandle, "_sourceTex", settings.sourceTexture);
        _allInOneShader.SetTexture(kernelHandle, "_srcBlurTex", srcBlurRendTex);
        _allInOneShader.SetTexture(kernelHandle, "_distTex", distRendTex);
        _allInOneShader.SetTexture(kernelHandle, "_resultTex", resultRendTex);
        _allInOneShader.SetBuffer(kernelHandle, "_gaussKernelEdge", kernelEdgeBlurBuffer);
        _allInOneShader.SetBuffer(kernelHandle, "_gaussKernelBorder", kernelBorderBlurBuffer);
        _allInOneShader.SetBuffer(kernelHandle, "_gaussKernelFinal", kernelFinalBlurBuffer);

        // Calculate kernels group size
        _allInOneShader.GetKernelThreadGroupSizes(kernelHandle, out uint groupSizeX, out uint groupSizeY, out _);
        int threadGroupsX = Mathf.CeilToInt(width / (float)groupSizeX);
        int threadGroupsY = Mathf.CeilToInt(height / (float)groupSizeY);

        // Start the shader up
        GL.sRGBWrite = true;
        _allInOneShader.Dispatch(kernelHandle, threadGroupsX, threadGroupsY, 1);

        // Get data
        RenderTexture.active = resultRendTex;
        Texture2D finalTexture = new Texture2D(width, height, TextureFormat.RGBA32, true, false);
        finalTexture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        finalTexture.Apply();

        // Cleanup
        RenderTexture.active = null;

        srcBlurRendTex.Release();
        distRendTex.Release();
        resultRendTex.Release();
        kernelEdgeBlurBuffer.Dispose();
        kernelBorderBlurBuffer.Dispose();
        kernelFinalBlurBuffer.Dispose();


        return finalTexture;
    }
}
