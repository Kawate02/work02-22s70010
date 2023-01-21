using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Button[] buttons = new Button[8];
    [SerializeField] private Button undo;
    [SerializeField] private Button[] zoom = new Button[2];

    private int iszoom = 0;

    private CompositeDisposable disposable = new();
    public FieldManager field;
    public FieldManager sample;
    private int posx = 0, posy = 0;
    private List<int> history = new();
    void Start()
    {
        field = GameObject.Find("FieldManager").GetComponent<FieldManager>();
        sample = GameObject.Find("Sample").GetComponent<FieldManager>();

        Init();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
    }
    void OnDestroy()
    {
        disposable.Dispose();
    }

    private void Init()
    {
        disposable.Clear();
        posx = 0;
        posy = 0;
        transform.position = Vector3.zero;
        cam.transform.position = transform.position + new Vector3(0, 10, 0);
        field.CreateField(5, 5, transform.position);
        CreateSample();

        Subscribe();
        HideAllButton();
        ShowButton();
    }

    private void Subscribe()
    {
        buttons[0].OnClickAsObservable().Subscribe(_ => Move(0)).AddTo(disposable);
        buttons[1].OnClickAsObservable().Subscribe(_ => Move(1)).AddTo(disposable);
        buttons[2].OnClickAsObservable().Subscribe(_ => Move(2)).AddTo(disposable);
        buttons[3].OnClickAsObservable().Subscribe(_ => Move(3)).AddTo(disposable);
        buttons[4].OnClickAsObservable().Subscribe(_ => Move(4)).AddTo(disposable);
        buttons[5].OnClickAsObservable().Subscribe(_ => Move(5)).AddTo(disposable);
        buttons[6].OnClickAsObservable().Subscribe(_ => Move(6)).AddTo(disposable);
        buttons[7].OnClickAsObservable().Subscribe(_ => Move(7)).AddTo(disposable);

        undo.OnClickAsObservable().Subscribe(_ => ReadHistory()).AddTo(disposable);

        zoom[0].OnClickAsObservable().Subscribe(_ => Zoom(0)).AddTo(disposable);
        zoom[1].OnClickAsObservable().Subscribe(_ => Zoom(1)).AddTo(disposable);
    }

    private void HideAllButton()
    {
        for (int i = 0, length = buttons.Length; i < length; i++)
        {
            buttons[i].gameObject.SetActive(false);
        }
    }

    private void ShowButton()
    {
        List<int> activeBone = new();
        Bone[] bones = field.mass[posx, posy].bones;
        for (int i = 0, length = bones.Length; i < length; i++)
        {
            if (bones[i] != null && !bones[i].isActive)
            {
                activeBone.Add(i);
            }
        }
        for (int i = 0, length = activeBone.Count; i < length; i++)
        {
            buttons[activeBone[i]].gameObject.SetActive(true);
        }
    }

    private void Move(int n)
    {
        if (field.mass[posx, posy].bones[n] == null || field.mass[posx, posy].bones[n].isActive) return;
 
        history.Add(n);

        HideAllButton();

        field.mass[posx, posy].bones[n].BeActive();
        switch (n)
        {
            case 0:
                posx += 0;
                posy += 1;
                break;
            case 1:
                posx += 1;
                posy += 1;
                break;
            case 2:
                posx += 1;
                posy += 0;
                break;
            case 3:
                posx += 1;
                posy += -1;
                break;
            case 4:
                posx += 0;
                posy += -1;
                break;
            case 5:
                posx += -1;
                posy += -1;
                break;
            case 6:
                posx += -1;
                posy += 0;
                break;
            case 7:
                posx += -1;
                posy += 1;
                break;

            default:
                break;
        }
        transform.position = field.mass[posx, posy].transform.position;
        field.mass[posx, posy].bones[n + 4 >= 8 ? n - 4 : n + 4].BeActive();

        if (iszoom == 0)
        {
            cam.transform.position = transform.position + new Vector3(0, 10, 0);
            ShowButton();
        }
        if (Judge()) Init();
    }

    private void ReadHistory()
    {
        if (history.Count == 0) return;

        Undo(history[history.Count - 1]);
        history.RemoveAt(history.Count - 1);
    }

    private void Undo(int n)
    {
        n = n + 4 >= 8 ? n - 4 : n + 4;

        HideAllButton();

        field.mass[posx, posy].bones[n].BeUnActive();
        switch (n)
        {
            case 0:
                posx += 0;
                posy += 1;
                break;
            case 1:
                posx += 1;
                posy += 1;
                break;
            case 2:
                posx += 1;
                posy += 0;
                break;
            case 3:
                posx += 1;
                posy += -1;
                break;
            case 4:
                posx += 0;
                posy += -1;
                break;
            case 5:
                posx += -1;
                posy += -1;
                break;
            case 6:
                posx += -1;
                posy += 0;
                break;
            case 7:
                posx += -1;
                posy += 1;
                break;

            default:
                break;
        }
        transform.position = field.mass[posx, posy].transform.position;
        field.mass[posx, posy].bones[n + 4 >= 8 ? n - 4 : n + 4].BeUnActive();

        if (iszoom == 0)
        {
            cam.transform.position = transform.position + new Vector3(0, 10, 0);
            ShowButton();
        }
    }

    private void Zoom(int n)
    {
        if (iszoom == n) return;

        if (n == 0)
        {
            iszoom = 0;
            cam.transform.position = transform.position + new Vector3(0, 10, 0);
            ShowButton();
        }
        if (n == 1)
        {
            iszoom = 1;
            cam.transform.position = new Vector3(-8, 35, 12);
            HideAllButton();
        }
    }

    private void CreateSample()
    {
        sample.CreateField(5, 5, transform.position + new Vector3(-40, 0, 0));
        for (int i = 0; i < 300; i++)
        {
            SampleMove(Random.Range(0, 7));
        }
        posx = 0;
        posy = 0;
    }

    private void SampleMove(int n)
    {
        if (sample.mass[posx, posy].bones[n] == null || sample.mass[posx, posy].bones[n].isActive) return;

        sample.mass[posx, posy].bones[n].BeActive();
        switch (n)
        {
            case 0:
                posx += 0;
                posy += 1;
                break;
            case 1:
                posx += 1;
                posy += 1;
                break;
            case 2:
                posx += 1;
                posy += 0;
                break;
            case 3:
                posx += 1;
                posy += -1;
                break;
            case 4:
                posx += 0;
                posy += -1;
                break;
            case 5:
                posx += -1;
                posy += -1;
                break;
            case 6:
                posx += -1;
                posy += 0;
                break;
            case 7:
                posx += -1;
                posy += 1;
                break;

            default:
                break;
        }
        sample.mass[posx, posy].bones[n + 4 >= 8 ? n - 4 : n + 4].BeActive();
    }

    private bool Judge()
    {
        bool clear = true;

        for (int i = 0, length = field.mass.GetLength(0); i < length; i++)
        {
            for (int j = 0; j < length;  j++)
            {
                List<int> activeBone = new();
                Bone[] bones = field.mass[i, j].bones;
                for (int k = 0, count = bones.Length; k < count; k++)
                {
                    if (bones[k] != null)
                    {
                        activeBone.Add(k);
                    }
                }
                for (int k = 0, count = activeBone.Count; k < count; k++)
                {
                    if (field.mass[i, j].bones[activeBone[k]].isActive != sample.mass[i, j].bones[activeBone[k]].isActive)
                    {
                        clear = false;
                    }
                }
            }
        }
        return clear;
    }
}
