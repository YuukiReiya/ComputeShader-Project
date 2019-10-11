using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sample
{
    /// <summary>
    /// 単一スレッドグループで動作するコンピュートシェーダーの実行モジュール
    /// </summary>
    public class SingleCShader : MonoBehaviour
    {
        public ComputeShader shader;
        void Start()
        {
            Setup();
        }

        // Update is called once per frame
        void Update()
        {

        }

        void Setup()
        {
#if false
            var th = ThreadSize.GetThreadSize(shader, 0);
            Debug.Log("x:" + th.x);
            Debug.Log("y:" + th.y);
            Debug.Log("z:" + th.z);
#endif
            const int c_Loop = 3;
            ComputeBuffer buffer = new ComputeBuffer(4 * c_Loop, sizeof(int));
            shader.SetBuffer(0, "buffer", buffer);
            shader.Dispatch(0, c_Loop, 1, 1);
            var data = new int[4 * c_Loop];
            buffer.GetData(data);
            foreach(var it in data)
            {
                Debug.Log("item:" + it);
            }
            buffer.Release();
        }
    }
}