//参考:http://neareal.com/2616/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureSample : MonoBehaviour
{
    RenderTexture tex;
    [SerializeField] ComputeShader shader;
    ComputeBuffer buffer;
    Renderer renderer;
    int kIndex;//実行するカーネルのインデックス

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
        SetupTexture();
        SetupComputeShader();
        GetThreadSize();
        Execute();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void Execute()
    {
        //Thead Group
        int x, y, z;
        x = tex.width / thSize.x;
        y = tex.height / thSize.y;
        z = thSize.z;
        //実行
        shader.Dispatch(
            kIndex,
            x,
            y,
            z
            );

        Debug.Log("x:" + x);
        Debug.Log("y:" + y);
        Debug.Log("z:" + z);
        buffer.Release();
        //マテリアル変更
        renderer.material.mainTexture = tex;
    }

    /// <summary>
    /// テクセル書き込み用のテクスチャの動的用意
    /// </summary>
    void SetupTexture()
    {
        //合計テクセル数: 512 * 512 = 262,144
        tex = new RenderTexture(512, 512, 0, RenderTextureFormat.ARGB32);

        //ランダムアクセス書き込みの許可(シェーダーからの書き込み許可):trueにしないと書き込めないはず
        tex.enableRandomWrite = true;
        tex.Create();
    }

    /// <summary>
    /// コンピュートシェーダーのセットアップ
    /// カーネルインデックス等
    /// </summary>
    void SetupComputeShader()
    {
        //  index
        kIndex = shader.FindKernel("Func");

        //  buffer
        buffer = new ComputeBuffer(512 * 512, sizeof(float));
        shader.SetBuffer(kIndex, "textureBuffer", buffer);

    }

    //以下 コンピュートシェーダー側のカーネルの使用スレッド数取得関連

    /// <summary>
    /// スレッド数取得用の構造体
    /// </summary>
    public struct ThreadSize
    {
        public int x, y, z;

        public ThreadSize(uint x, uint y, uint z)
        {
            this.x = (int)x;
            this.y = (int)y;
            this.z = (int)z;
        }
    }
    ThreadSize thSize;
    public void GetThreadSize()
    {
        uint x, y, z;
        shader.GetKernelThreadGroupSizes(kIndex, out x, out y, out z);
        thSize = new ThreadSize(x, y, z);

        //確認用
        //※カーネル宣言時の numthread数を取得できる？
        Debug.Log("Thread size = (" + thSize.x + "," + thSize.y + "," + thSize.z + ")");
    }
}
