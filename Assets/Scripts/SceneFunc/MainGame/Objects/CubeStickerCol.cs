using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeStickerCol : MonoBehaviour, IEventObject
{
    public void Execute()
    {
        BoxMain.instance.RemoveCubeSticker();
    }
}
