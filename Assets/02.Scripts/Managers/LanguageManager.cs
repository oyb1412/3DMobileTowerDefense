using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// ������ ��� ���� ����
/// </summary>
public class LanguageManager
{
    private Define.Language _currentLanguage = Define.Language.Korean;  //������ ��� ����
    private Dictionary<Define.TextKey, Text> _textList = new Dictionary<Define.TextKey, Text>();  //���ӿ� ǥ������ ��� text ����

    public Define.Language CurrentLanguage => _currentLanguage;

    /// <summary>
    /// ��� ����
    /// </summary>
    /// <param name="language"></param>
    public void ChangeLanguage(Define.Language language) {
        _currentLanguage = language;
        foreach (var text in _textList) {
            SetText(text.Value, text.Key, false);  //��� text ���� ����
        }
    }

    /// <summary>
    /// ��� ���� �ʱ�ȭ
    /// </summary>
    public void Clear() {
        _textList.Clear();
    }

    /// <summary>
    /// text���� ����
    /// </summary>
    /// <param name="text">������ text</param>
    /// <param name="key">������ key</param>
    /// <param name="trigger">������ �߰��� �ʿ䰡 �ִ� text�� ��� true</param>
    /// <param name="add">���ڿ��� �������� �����ؾ� �� ��</param>
    public void SetText(Text text, Define.TextKey key,  bool trigger = true, string add = null) {
        if(add != null)
            text.text = add;  //���ڿ� ���� ����
        else
            text.text = Managers.Data.GetLanguage((int)key, (int)_currentLanguage);  //����� �����Ϳ��� ���ڿ� ����
        if(trigger) {
            if(!_textList.ContainsValue(text)) {
                _textList.Add(key, text);
            }
        }
    }
}
