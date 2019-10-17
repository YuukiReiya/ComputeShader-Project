//ピクセル書き換え参考:http://www.noshimemo.com/entry/2015/12/15/000000
using UnityEngine;

namespace Sample
{
    public class MonoOverwriteTexture : MonoBehaviour
    {
        [System.Serializable]
        struct ProcessingUnit
        {
            public Renderer render;
            [System.NonSerialized] public RenderTexture tex;
        }
        [SerializeField] ProcessingUnit unit;

        // Start is called before the first frame update
        void Start()
        {
            Setup(ref unit);
            Execute(ref unit);

        }

        // Update is called once per frame
        void Update()
        {
        }

        void Setup(ref ProcessingUnit arg)
        {
            arg.tex = new RenderTexture(512, 512, 0, RenderTextureFormat.ARGB32);
            arg.tex.enableRandomWrite = true;
            arg.tex.Create();
        }

        void Execute(ref ProcessingUnit arg)
        {
            arg.render.material.mainTexture = arg.tex;

            var mainTex = new Texture2D(512, 512);
            mainTex.ReadPixels(
                new Rect(0, 0, arg.tex.width, arg.tex.height),
                0,
                0);
            //Texture src = arg.tex;
            //var mainTex = (Texture2D)src;
            //ピクセル分用意   hegith:512 width:512
            //Color[,] pixels = new Color[512, 512];
            //for (int h = 0; h < 512; ++h)
            //{
            //    for (int w = 0; w < 512; ++w)
            //    {
            //        var cr = new Color
            //            (
            //                w / 512,
            //                w / 512,
            //                w / 512,
            //                1.0f
            //            );
            //    }
            //}
            var pixels = mainTex.GetPixels();
            var updatePixels = new Color[pixels.Length];
            for (int i = 0; i < pixels.Length; ++i)
            {
                pixels[i] = new Color(i / 512, i / 512, i / 512, 1.0f);
            }
            Texture2D tex = new Texture2D(mainTex.width, mainTex.height, TextureFormat.RGBA32, false);
            tex.filterMode = FilterMode.Point;
            tex.SetPixels(pixels);
            tex.Apply();
            arg.render.material.mainTexture = tex;
        }
    }
}
