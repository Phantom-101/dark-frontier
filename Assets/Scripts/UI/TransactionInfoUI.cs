using DarkFrontier.Factions;
using UnityEngine;
using UnityEngine.UI;

namespace DarkFrontier.UI {
    public class TransactionInfoUI : MonoBehaviour {

        [SerializeField] private Text _amountText;
        [SerializeField] private Text _other;
        [SerializeField] private Text _time;
        [SerializeField] private Transaction _transaction;

        public Transaction GetTransaction () { return _transaction; }

        public void SetTransaction (Transaction transaction) { _transaction = transaction; }

        public void Initialize () {

            _amountText.text = "Amount: " + (_transaction.Amount > 0 ? "+" : "") + _transaction.Amount + " Cr";
            _amountText.color = _transaction.Amount > 0 ? Color.green : Color.red;
            _other.text = "To: " + _transaction.Other;
            _time.text = "Time: " + _transaction.Time.ToString ();

        }

    }
}
