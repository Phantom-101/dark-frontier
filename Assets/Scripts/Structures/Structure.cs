using System.Collections.Generic;
using UnityEngine;

public class Structure : MonoBehaviour {

    [SerializeField] private StructureSO _profile;

    [SerializeField] private double _hull;

    [SerializeField] private List<EquipmentSlot> _equipmentSlots;

    [SerializeField] private Structure _target;

    public void Tick () {

        foreach (EquipmentSlot slot in _equipmentSlots) slot.Tick ();

    }

    public StructureSO GetProfile () { return _profile; }

    public double GetHull () { return _hull; }

    public void ChangeHull (double amount) {

        _hull -= amount;

        if (_hull <= 0) {

            _profile.OnDestroyedChannel.RaiseEvent (this, _profile);

        }

    }

    public Structure GetTarget () { return _target; }

}
