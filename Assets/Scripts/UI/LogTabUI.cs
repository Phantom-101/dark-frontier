using UnityEngine;

public class LogTabUI : MonoBehaviour {

    [SerializeField] protected CanvasGroup _group;

    public virtual void SwitchIn () {

        LeanTween.alphaCanvas (_group, 1, 0.2f).setIgnoreTimeScale (true);
        _group.interactable = true;
        _group.blocksRaycasts = true;

    }

    public virtual void SwitchOut () {

        LeanTween.alphaCanvas (_group, 0, 0.2f).setIgnoreTimeScale (true);
        _group.interactable = false;
        _group.blocksRaycasts = false;

    }

    public virtual void SwitchOutImmediately () {

        LeanTween.alphaCanvas (_group, 0, 0).setIgnoreTimeScale (true);
        _group.interactable = false;
        _group.blocksRaycasts = false;

    }

}
