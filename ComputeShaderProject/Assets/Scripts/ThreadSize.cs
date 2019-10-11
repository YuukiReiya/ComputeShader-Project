using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ThreadSize
{
    public int x, y, z;

    public ThreadSize(uint x, uint y, uint z)
    {
        this.x = (int)x;
        this.y = (int)y;
        this.z = (int)z;
    }

    public static ThreadSize GetThreadSize(ComputeShader cs,int kIndex)
    {
        uint x, y, z;
        cs.GetKernelThreadGroupSizes(kIndex, out x, out y, out z);
        return new ThreadSize(x, y, z);
    }
}