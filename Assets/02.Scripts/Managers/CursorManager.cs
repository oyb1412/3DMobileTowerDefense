using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager
{
    private Texture2D[] _cursorTexture = new Texture2D[(int)Define.CursorType.Count];

    public void Init() {
        _cursorTexture[(int)Define.CursorType.DefaultCursor] = Resources.Load<Texture2D>($"Prefabs/Cursor/{Define.CursorType.DefaultCursor.ToString()}");
        _cursorTexture[(int)Define.CursorType.NodeCursor] = Resources.Load<Texture2D>($"Prefabs/Cursor/{Define.CursorType.NodeCursor.ToString()}");
        _cursorTexture[(int)Define.CursorType.ButtonCursor] = Resources.Load<Texture2D>($"Prefabs/Cursor/{Define.CursorType.ButtonCursor.ToString()}");
        _cursorTexture[(int)Define.CursorType.EnemyCursor] = Resources.Load<Texture2D>($"Prefabs/Cursor/{Define.CursorType.EnemyCursor.ToString()}");
    }
}
