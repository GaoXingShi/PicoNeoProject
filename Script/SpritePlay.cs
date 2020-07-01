using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class SpritePlay : MonoBehaviour
{
    public int speed = 1;
    public SpriteAtlas atlas;
    public List<Sprite> spriteArray;
    private int playIndex = 0;

    private Image mImage;
    private int fixedValue = 0;
    void Start()
    {
        mImage = GetComponent<Image>();
        mImage.sprite = atlas.GetSprite("合成 1_00000000");
        Debug.Log(atlas.spriteCount);

        for (int i = 0; i < atlas.spriteCount; i++)
        {
            Sprite temp = mImage.sprite = atlas.GetSprite($"合成 1_00000{i.ToString("D3")}");
            if (temp != null)
                spriteArray.Add(temp);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        fixedValue++;
        if (fixedValue == speed)
        {
            fixedValue = 0;
        }
        else
        {
            return;
        }

        playIndex++;
        if (playIndex >= spriteArray.Count)
        {
            playIndex = 0;
        }

        mImage.sprite = spriteArray[playIndex];
    }
}
