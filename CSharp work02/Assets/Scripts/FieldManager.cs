using System.Collections.Generic;
using UnityEngine;

public class FieldManager : MonoBehaviour
{
    [SerializeField] private GameObject _mass;
    [SerializeField] private GameObject _bone;

    public Mass[,] mass = null;

    public void CreateField(int x, int y, Vector3 pos)
    {
        if (mass != null)
        {
            for (int i = 0, length = mass.GetLength(0); i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    mass[i, j].Delete();
                }
            }
        }
        mass = new Mass[x, y];

        for (int i = 0, length = x; i < length; i++)
        {
            for (int j = 0, size = y; j < size; j++)
            {
                mass[i, j] = Instantiate(_mass, new Vector3(pos.x + 6 * i, pos.y, pos.z + 6 * j), Quaternion.identity).GetComponent<Mass>();
                mass[i, j].transform.parent = transform;
                CreateBone(x, y, i, j);
            }
        }
    }

    private void CreateBone(int x, int y, int i, int j)
    {
        List<int> boneDir = new() { 0, 1, 2, 3, 4, 5, 6, 7};

        if (i == 0)
        {
            boneDir.Remove(5);
            boneDir.Remove(6);
            boneDir.Remove(7);
        }
        if (i == x - 1)
        {
            boneDir.Remove(1);
            boneDir.Remove(2);
            boneDir.Remove(3);
        }
        if (j == 0)
        {
            boneDir.Remove(3);
            boneDir.Remove(4);
            boneDir.Remove(5);
        }
        if (j == y - 1)
        {
            boneDir.Remove(0);
            boneDir.Remove(1);
            boneDir.Remove(7);
        }

        for (int _i = 0, length = boneDir.Count; _i < length; _i++)
        {
            mass[i, j].SetBone(_bone, boneDir[_i]);
        }
    }
}
