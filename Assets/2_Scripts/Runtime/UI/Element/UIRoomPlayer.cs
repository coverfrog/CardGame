using System;
using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Color = Steamworks.Data.Color;
using Image = Steamworks.Data.Image;

public class UIRoomPlayer : MonoBehaviour
{
    [SerializeField] private RawImage mIcon;
    [SerializeField] private TMP_Text mName;
    [Space]
    [SerializeField] private UnityEngine.UI.Image mReadyImage;
    [SerializeField] private TMP_Text mReadyText;
    [Space]
    [SerializeField] private GameObject mLoadObj;

    private bool _mIsHost;
    
    public async void Enter(Friend friend, bool isHost)
    {
        try
        {
            _mIsHost = isHost;
            
            gameObject.SetActive(true);

            mIcon.texture = null;
            mName.text = friend.Name;
            
            mReadyImage.gameObject.SetActive(isHost);
            mReadyText.text = isHost ? "Host" : "Ready";
            
            mLoadObj.SetActive(true);
            
            var result = await friend.GetLargeAvatarAsync();

            if (!result.HasValue) return;

            Image image = result.Value;

            var avatar = new Texture2D((int)image.Width, (int)image.Height, TextureFormat.ARGB32, false)
            {
                // Set filter type, or else its really blury
                filterMode = FilterMode.Trilinear
            };

            // Flip image
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    Color p = image.GetPixel(x, y);

                    avatar.SetPixel(x, (int)image.Height - y,
                        new UnityEngine.Color(p.r / 255.0f, p.g / 255.0f, p.b / 255.0f, p.a / 255.0f));
                }
            }

            avatar.Apply();

            mIcon.texture = avatar;
            mLoadObj.SetActive(false);

        }
        catch (Exception)
        {
            // ignore
        }
    }

    public UIRoomPlayer Exit()
    {
        gameObject.SetActive(false);
        
        return this;
    }

    public bool OnReadyChanged(bool ready)
    {
        if (_mIsHost)
        {
            return true;
        }
        
        mReadyImage.gameObject.SetActive(ready);
        
        return ready;
    }
}
