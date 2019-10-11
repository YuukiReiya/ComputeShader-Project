using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sample
{

    public class TwinCShader : MonoBehaviour
    {
        [SerializeField] ComputeShader shader;
        // Start is called before the first frame update
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
            ComputeBuffer buffer = new ComputeBuffer(4 * 4 * 2 * 2, sizeof(int));
            int kIndex = shader.FindKernel("Sample");
            shader.SetBuffer(kIndex, "buffer", buffer);
            shader.Dispatch(kIndex, 2, 2, 1);
            var data = new int[4 * 4 * 2 * 2];
            buffer.GetData(data);
            for (int i = 0; i < 8; i++)
            {
                string line = string.Empty;
                for (int j = 0; j < 8; j++)
                {
                    line += " " + data[j + i * 8];
                }
                Debug.Log(line);
            }
            buffer.Release();
        }
    }
}