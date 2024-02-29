using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTexture : MonoBehaviour
{

    SpriteRenderer m_spriteRenderer;

    public Material AttackTexture;
    public Material WalkTexture;
    public Material FadeTexture;
    public Material BlockTexture;
    public Material EndTexture;
    public Material HurtTexture;
    public Material IdleTexture;
    public Material JumpTexture;

    private void Start()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void ChangeTextureAttack()
    {
        m_spriteRenderer.material = AttackTexture;
    }
    public void ChangeTextureWalk()
    {
        m_spriteRenderer.material = WalkTexture;

    }
    public void ChangeTextureFade()
    {
        m_spriteRenderer.material = FadeTexture;

    }
    public void ChangeTextureBlock()
    {
        m_spriteRenderer.material = BlockTexture;
    }
    public void ChangeTextureEnd()
    {
        m_spriteRenderer.material = EndTexture;
    }
    public void ChangeTextureHurt()
    {
        m_spriteRenderer.material = EndTexture;
    }
    public void ChangeTextureIdle()
    {
        m_spriteRenderer.material = IdleTexture;
    }
    public void ChangeTextureJump()
    {
        m_spriteRenderer.material = JumpTexture;
    }
}
