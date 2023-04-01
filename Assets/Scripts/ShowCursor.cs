using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowCursor : MonoBehaviour
{
    public bool showCursor = true;

    private void Start()
    {
        Cursor.visible = showCursor;
    }
}
