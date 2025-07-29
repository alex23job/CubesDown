using UnityEngine;

public class CubeControl : MonoBehaviour
{
    private int numColor1 = 0;
    private int numColor2 = 0;

    public int CubeColor {  
        get
        {
            if (numColor1 == numColor2)
            {
                return numColor1;
            }
            else
            {
                return 10 * numColor1 + numColor2;
            }
        }
    }

    public void SetColors(Material mat1, Material mat2, int nCol1, int nCol2, bool isBonus = false)
    {
        numColor1 = nCol1 + 1;
        numColor2 = nCol2 + 1;
        MeshRenderer mr = gameObject.GetComponent<MeshRenderer>();
        Material[] mats = mr.materials;
        if (isBonus)
        {
            mats[0] = mat1;
            //numColor1 += 6;
            //numColor2 = numColor1;
        }
        else
        {
            mats[0] = mat1;
            mats[1] = mat2;
        }
        mr.materials = mats;
    }

    public bool CmpColor(int color)
    {
        if (CubeColor == color) 
        {
            return true;
        }
        if (CubeColor > 10)
        {
            int invColor = 10 * numColor2 + numColor1;
            if (invColor == color) return true;
        }
        return false;
    }
}
