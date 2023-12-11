using System;
using UnityEngine;

namespace Ghosts
{
    public class TextureConverterUtil
    {
        public static string TextureToBase64(Texture2D texture)
        {
            byte[] bytes = texture.EncodeToPNG();
            return Convert.ToBase64String(bytes);
        }

        public static Texture2D Base64ToTexture(string base64String)
        {
            byte[] bytes = Convert.FromBase64String(base64String);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(bytes);
            return texture;
        }
    }
}
