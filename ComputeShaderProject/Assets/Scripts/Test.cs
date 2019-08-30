using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public ComputeShader computeShader;
    ComputeBuffer compRet;
    int kIndexSum;
    int val = -1;

    public int r = 2;
    public int l = 4;

    // Start is called before the first frame update
    void Start ()
    {

    }

    // Update is called once per frame
    void Update ()
    {
        //  index
        kIndexSum = computeShader.FindKernel ("Sum");
        this.compRet = new ComputeBuffer (4, sizeof (int));

        //  バッファ
        this.computeShader.SetBuffer (kIndexSum, "buffer", compRet);

        //  変数セット
        this.computeShader.SetInt ("r", r);
        this.computeShader.SetInt ("l", l);

        //  実行
        computeShader.Dispatch (kIndexSum, 1, 1, 1);

        //  実行結果格納
        int[] ret = new int[4];
        this.compRet.GetData (ret);
        foreach (var it in ret)
        {
            Debug.Log ("ret = " + it);
        }

        //  破棄
        compRet.Release ();
    }
}