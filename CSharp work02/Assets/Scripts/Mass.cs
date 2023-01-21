using UnityEngine;

public class Mass : MonoBehaviour
{
    public Bone[] bones = new Bone[8];

    public void SetBone(GameObject bone, int n)
    {
        if (bones[n] == null)
        {
            Quaternion rot = Quaternion.identity;
            Vector3 pos = Vector3.zero;
            switch (n)
            {
                case 0:
                    pos = new Vector3(0, 0, 3);
                    rot = Quaternion.AngleAxis(90f, new Vector3(0, 1, 0));
                    break;
                case 1:
                    pos = new Vector3(2, 0, 2);
                    rot = Quaternion.AngleAxis(-45f, new Vector3(0, 1, 0));
                    break;
                case 2:
                    pos = new Vector3(3, 0, 0);
                    rot = Quaternion.AngleAxis(0, new Vector3(0, 1, 0));
                    break;
                case 3:
                    pos = new Vector3(2, 0, -2);
                    rot = Quaternion.AngleAxis(45f, new Vector3(0, 1, 0));
                    break;
                case 4:
                    pos = new Vector3(0, 0, -3);
                    rot = Quaternion.AngleAxis(90f, new Vector3(0, 1, 0));
                    break;
                case 5:
                    pos = new Vector3(-2, 0, -2);
                    rot = Quaternion.AngleAxis(-45f, new Vector3(0, 1, 0));
                    break;
                case 6:
                    pos = new Vector3(-3, 0, 0);
                    rot = Quaternion.AngleAxis(0, new Vector3(0, 1, 0));
                    break;
                case 7:
                    pos = new Vector3(-2, 0, 2);
                    rot = Quaternion.AngleAxis(45f, new Vector3(0, 1, 0));
                    break;

                default:
                    break;
            }
            bones[n] = Instantiate(bone, transform.position + pos, transform.rotation * rot).GetComponent<Bone>();
            bones[n].Initialization();
            bones[n].transform.parent = transform;
        }
    }

    public void Delete()
    {

        for (int i = 0, length = bones.Length; i < length; i++)
        {
            if (bones[i] != null)
            {
                bones[i].Delete();
            }
        }
        Destroy(gameObject);
    }
}
