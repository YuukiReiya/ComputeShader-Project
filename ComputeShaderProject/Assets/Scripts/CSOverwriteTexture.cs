using UnityEngine;
namespace Sample
{
    /// <summary>
    /// ComputeTextureSample.computeの動作確認スクリプト
    /// </summary>
    public class CSOverwriteTexture : MonoBehaviour
    {
        //コンピュートシェーダーの処理単位用の構造体
        [System.Serializable]
        struct ProcessingUnit
        {
            public Renderer render;
            [System.NonSerialized] public RenderTexture tex;
            [System.NonSerialized] public int kIndex;
            public ThreadSize threadSize;
        }

        //serialize param
        [SerializeField] private ComputeShader shader;
        [SerializeField] ProcessingUnit unit;
        //private param

        //public param


        // Start is called before the first frame update
        void Start()
        {
            SetupRenderTexture(ref unit);
            unit.kIndex = shader.FindKernel("Func");
            SetupThread(ref unit);
            SetupComputeShaderBuffer(ref unit);
        }

        // Update is called once per frame
        void Update()
        {
            Execute(ref unit);
        }

        void SetupRenderTexture(ref ProcessingUnit arg)
        {
            arg.tex = new RenderTexture(512, 512, 0, RenderTextureFormat.ARGB32);
            arg.tex.enableRandomWrite = true;
            arg.tex.Create();
        }

        void SetupThread(ref ProcessingUnit arg)
        {
            arg.threadSize = ThreadSize.GetThreadSize(shader, arg.kIndex);
        }

        void SetupComputeShaderBuffer(ref ProcessingUnit arg)
        {
            shader.SetTexture(arg.kIndex, "texBuffer", arg.tex);
        }

        void Execute(ref ProcessingUnit arg)
        {
            //カーネルの実行
            shader.Dispatch(
                arg.kIndex,//カーネル番号
                arg.tex.width / arg.threadSize.x,//512 / 8 = 64
                arg.tex.height / arg.threadSize.y,//512 / 8 = 64
                arg.threadSize.z//1
                );

            //実行結果をテクスチャに反映
            arg.render.material.mainTexture = arg.tex;
        }
    }
}
