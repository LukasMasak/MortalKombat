using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bakc : MonoBehaviour
{
    public Camera cam; // Přiřaďte hlavní kameru v Unity editoru
    public int targetLayer = 8; // Nastavte Layer cílového objektu
    public float transparentAlpha = 0.5f; // Hodnota průhlednosti
    public float normalAlpha = 1.0f; // Normální hodnota průhlednosti

    private GameObject lastHitObject = null;

    void Update()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            GameObject hitObject = hit.collider.gameObject;

            if (hitObject.layer == targetLayer)
            {
                SetTransparency(hitObject, transparentAlpha);

                if (lastHitObject != null && lastHitObject != hitObject)
                {
                    SetTransparency(lastHitObject, normalAlpha);
                }

                lastHitObject = hitObject;
            }
            else
            {
                if (lastHitObject != null)
                {
                    SetTransparency(lastHitObject, normalAlpha);
                    lastHitObject = null;
                }
            }
        }
        else
        {
            if (lastHitObject != null)
            {
                SetTransparency(lastHitObject, normalAlpha);
                lastHitObject = null;
            }
        }
    }


    void SetTransparency(GameObject obj, float alpha)
    {
        SpriteRenderer renderer = obj.GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            Color color = renderer.color;
            color.a = alpha;
            renderer.color = color;
        }
    }
}
