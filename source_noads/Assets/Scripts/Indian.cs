using UnityEngine;
using System.Collections;
using System.Linq;

public class Indian : MonoBehaviour
{
    public void HideBoard()
    {
        transform.parent.renderer.enabled = false;
        transform.parent.GetChild(1).renderer.enabled = false;

        transform.GetComponentsInParent<Collider2D>()[0].enabled = false;
        transform.parent.SendMessage("DestroyEverything");
    }

    public void Die()
    {
        SoundEffectsManager.Instance.PlaySfxWaterSplash(Time.deltaTime);

        transform.parent.GetComponentsInChildren<SpriteRenderer>().ToList().ForEach(s => s.enabled = false);
    }

    public void BackToSurf()
    {
        transform.parent.GetComponent<PlayerHealth>().ChangeBehavior(PlayerBehavior.Surfing);
    }
}