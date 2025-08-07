using UnityEngine;

public struct NormalMapSettings
{
    public Texture2D sourceTexture;
    public float strengthEdges;
    public int blurEdgesRadius;
    public float strengthBorder;
    public int blurBorderRadius;
    public float softenBorder;
    public float slopePercentageBorder;
    public int finalBlurRadius;
}
