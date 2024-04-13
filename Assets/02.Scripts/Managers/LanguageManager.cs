using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// 게임의 언어 설정 관리
/// </summary>
public class LanguageManager
{
    private Define.Language _currentLanguage = Define.Language.Korean;  //현재의 언어 설정
    private Dictionary<Define.TextKey, Text> _textList = new Dictionary<Define.TextKey, Text>();  //게임에 표현중인 모든 text 정보

    public Define.Language CurrentLanguage => _currentLanguage;

    /// <summary>
    /// 언어 변경
    /// </summary>
    /// <param name="language"></param>
    public void ChangeLanguage(Define.Language language) {
        _currentLanguage = language;
        foreach (var text in _textList) {
            SetText(text.Value, text.Key, false);  //모든 text 정보 변경
        }
    }

    /// <summary>
    /// 언어 설정 초기화
    /// </summary>
    public void Clear() {
        _textList.Clear();
    }

    /// <summary>
    /// text정보 저장
    /// </summary>
    /// <param name="text">저장할 text</param>
    /// <param name="key">설정할 key</param>
    /// <param name="trigger">여러번 추가할 필요가 있는 text의 경우 true</param>
    /// <param name="add">문자열을 수동으로 지정해야 할 때</param>
    public void SetText(Text text, Define.TextKey key,  bool trigger = true, string add = null) {
        if(add != null)
            text.text = add;  //문자열 수동 지정
        else
            text.text = Managers.Data.GetLanguage((int)key, (int)_currentLanguage);  //저장된 데이터에서 문자열 지정
        if(trigger) {
            if(!_textList.ContainsValue(text)) {
                _textList.Add(key, text);
            }
        }
    }
}
