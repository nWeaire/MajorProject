// Script made by GitHub User "Misato"
// https://gist.github.com/misato/03a89a32b2424754cc07533040dd236f#file-snaptogrid-cs
using UnityEngine;

public class SnapToGrid : MonoBehaviour
{
    public float PPU = 32f; // pixels per unit (your tile size)

    private void LateUpdate()
    {
        Vector3 position = transform.localPosition;

        position.x = (Mathf.Round(transform.parent.position.x * PPU) / PPU) - transform.parent.position.x;
        position.y = (Mathf.Round(transform.parent.position.y * PPU) / PPU) - transform.parent.position.y;

        transform.localPosition = position;
    }
}