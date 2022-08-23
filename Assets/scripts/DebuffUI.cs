using UnityEngine;
using UnityEngine.UI;

public class DebuffUI : MonoBehaviour
{
    [SerializeField] private Sprite[] debuffsprites;

    [SerializeField] private Image SpriteControl;


    public void SpriteSet(int spriteNum)
    {
        SpriteControl.sprite = debuffsprites[spriteNum];
    }
}
