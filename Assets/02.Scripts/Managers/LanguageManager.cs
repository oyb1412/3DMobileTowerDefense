using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageManager
{
    public Define.Language CurrentLanguage => _currentLanguage;
    private Dictionary<Define.TextKey, Text> _textList = new Dictionary<Define.TextKey, Text>();

    private Define.Language _currentLanguage = Define.Language.Korean;
    public void ChangeLanguage(Define.Language language) {
        _currentLanguage = language;
        foreach (var text in _textList) {
            SetText(text.Value, text.Key, false);
        }
    }

    public void SetText(Text text, Define.TextKey key,  bool trigger = true, string add = null) {
        if(add != null)
            text.text = add;
        else
            text.text = Managers.Data.GetLanguage((int)key, (int)_currentLanguage);
        if(trigger) 
            _textList.Add(key, text);
    }
}
