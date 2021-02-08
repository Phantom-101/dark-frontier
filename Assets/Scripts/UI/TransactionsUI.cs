using System.Collections.Generic;
using UnityEngine;

public class TransactionsUI : LogTabUI {

    [SerializeField] private Transform _content;
    [SerializeField] private GameObject _prefab;
    [SerializeField] private List<Transaction> _transactions = new List<Transaction> ();
    [SerializeField] private List<TransactionInfoUI> _instantiated = new List<TransactionInfoUI> ();

    private static TransactionsUI _instance;

    private void Awake () {

        _instance = this;

    }

    public override void SwitchIn () {

        foreach (TransactionInfoUI info in _instantiated) Destroy (info.gameObject);
        _instantiated = new List<TransactionInfoUI> ();

        int index = 0;
        foreach (Transaction t in _transactions) {

            GameObject go = Instantiate (_prefab, _content);
            go.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (0, -150 * index);
            TransactionInfoUI info = go.GetComponent<TransactionInfoUI> ();
            info.SetTransaction (t);
            info.Initialize ();
            _instantiated.Add (info);
            index++;

        }
        _content.GetComponent<RectTransform> ().sizeDelta = new Vector2 (0, 150 * index);

        base.SwitchIn ();

    }

    public static TransactionsUI GetInstance () { return _instance; }

    public void AddTransaction (Transaction transaction) { if (!_transactions.Contains (transaction)) _transactions.Add (transaction); }

}
