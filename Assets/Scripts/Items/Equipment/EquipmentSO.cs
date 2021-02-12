using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EquipmentSO : ItemSO {

    public int Tier;
    public float EnergyStorage;
    public float EnergyRechargeRate;
    public float Durability;
    public float Wear;
    public bool Activatable;
    public bool AutomaticallyRepeat;
    public bool RequireCharge;
    public List<ChargeSO> Charges = new List<ChargeSO> ();

    /// <summary>
    /// Called when the scene starts and the equipment is already equipped.
    /// </summary>
    /// <param name="slot">The slot to be processed.</param>
    public virtual void OnAwake (EquipmentSlot slot) { }

    /// <summary>
    /// Called when the scene starts and the equipment is then equipped.
    /// </summary>
    /// <param name="slot">The slot to be processed.</param>
    public virtual void OnEquip (EquipmentSlot slot) { }

    /// <summary>
    /// Called when the equipment is unequipped.
    /// </summary>
    /// <param name="slot">The slot to be processed.</param>
    public virtual void OnUnequip (EquipmentSlot slot) { }

    /// <summary>
    /// Sets the target state.
    /// </summary>
    /// <param name="slot">The slot to be processed.</param>
    public virtual void SetTargetState (EquipmentSlot slot, bool state) { slot.TargetState = state; }

    /// <summary>
    /// Checks if the equipment can be activated.
    /// </summary>
    /// <param name="slot">The slot to be processed.</param>
    /// <returns>Whether or not the equipment can be activated.</returns>
    public virtual bool CanCycleStart (EquipmentSlot slot) {

        if (!Activatable) return false;
        if (slot.Equipment.RequireCharge && slot.Equipper.GetInventoryCount (slot.Charge) <= 0) return false;
        return true;

    }

    /// <summary>
    /// Called when the activation cycle starts.
    /// </summary>
    /// <param name="slot">The slot to be processed.</param>
    public virtual void OnCycleStart (EquipmentSlot slot) { }

    /// <summary>
    /// Called when the cycle is elapsing.
    /// </summary>
    /// <param name="slot">The slot to be processed.</param>
    public virtual void OnCycleTick (EquipmentSlot slot) { }

    /// <summary>
    /// Called every tick.
    /// </summary>
    /// <param name="slot">The slot to be processed.</param>
    public virtual void Tick (EquipmentSlot slot) {

        if (!Safe (slot)) return;

        // Check if there is a mismatch between current and target states
        if (slot.CurrentState != slot.TargetState) {

            if (slot.TargetState) {

                if (CanCycleStart (slot)) slot.CurrentState = true;
                else slot.TargetState = false;

            } else slot.CurrentState = false;

        }

        if (slot.CurrentState) {

            if (CanCycleStart (slot)) {

                if (slot.Energy == EnergyStorage) {

                    if (RequireCharge) {

                        if (slot.Equipper.GetInventoryCount (slot.Charge) > 0) {

                            slot.Equipper.ChangeInventoryCount (slot.Charge, -1);
                            slot.Energy = 0;
                            OnCycleStart (slot);
                            slot.Durability -= Wear;

                        } else slot.TargetState = false;

                    } else {

                        slot.Energy = 0;
                        OnCycleStart (slot);
                        slot.Durability -= Wear;

                    }

                }

                OnCycleTick (slot);

            } else slot.TargetState = false;

        }

        SafeTick (slot);

    }

    /// <summary>
    /// Called after the Tick function has ran and passed with no safety checks failing.
    /// </summary>
    /// <param name="slot">The slot to be processed.</param>
    public virtual void SafeTick (EquipmentSlot slot) { }

    /// <summary>
    /// Called every physics tick.
    /// </summary>
    /// <param name="slot">The slot to be processed.</param>
    public virtual void FixedTick (EquipmentSlot slot) {

        if (!Safe (slot)) return;

        SafeFixedTick (slot);

    }

    /// <summary>
    /// Called after the FixedTick function has ran and passed with no safety checks failing.
    /// </summary>
    /// <param name="slot">The slot to be processed.</param>
    public virtual void SafeFixedTick (EquipmentSlot slot) { }

    /// <summary>
    /// Checks if the slot is safe to be processed.
    /// </summary>
    /// <param name="slot">The slot to be processed.</param>
    /// <returns>Whether or not the slot can be safely processed.</returns>
    public virtual bool Safe (EquipmentSlot slot) {

        // If slot is unrelated, return
        if (slot.Equipment != this) return false;

        // If slot durability is 0, destroy equipment
        if (slot.Durability == 0) return false;

        // If slot is orphaned, destroy it
        if (slot.Equipper == null) return false;

        return true;

    }

}

[CustomPropertyDrawer (typeof (EquipmentSO))]
public class EquipmentSOPropertyDrawer : PropertyDrawer {

    public override float GetPropertyHeight (SerializedProperty property, GUIContent label) {

        // Main properties
        float svs = EditorGUIUtility.standardVerticalSpacing;
        float h = EditorGUI.GetPropertyHeight (property, label, true) + svs;

        // Other properties
        if (property.objectReferenceValue != null) {

            SerializedObject so = new SerializedObject (property.objectReferenceValue as EquipmentSO);

            SerializedProperty name = so.FindProperty ("Name");
            h += EditorGUI.GetPropertyHeight (name, label, true) + svs;

            SerializedProperty tier = so.FindProperty ("Tier");
            h += EditorGUI.GetPropertyHeight (tier, label, true) + svs;

        }

        return h;

    }

    public override void OnGUI (Rect position, SerializedProperty property, GUIContent label) {

        EditorGUI.BeginProperty (position, label, property);

        // Main property
        EditorGUI.PrefixLabel (position, GUIUtility.GetControlID (FocusType.Passive), new GUIContent (label.text));
        EditorGUI.PropertyField (position, property, true);

        // Other properties
        if (property.objectReferenceValue != null) {

            float svs = EditorGUIUtility.standardVerticalSpacing;
            float h = EditorGUI.GetPropertyHeight (property) + svs;

            SerializedObject so = new SerializedObject (property.objectReferenceValue);

            SerializedProperty nameProp = so.FindProperty ("Name");
            Rect nameRect = new Rect (position.x, position.y + h, position.width, EditorGUI.GetPropertyHeight (nameProp, label, true));
            EditorGUI.LabelField (nameRect, "Name", nameProp.stringValue);
            h += EditorGUI.GetPropertyHeight (nameProp, label, true) + svs;

            SerializedProperty tierProp = so.FindProperty ("Tier");
            Rect tierRect = new Rect (position.x, position.y + h, position.width, EditorGUI.GetPropertyHeight (nameProp, label, true));
            EditorGUI.LabelField (tierRect, "Tier", tierProp.intValue.ToString ());
            h += EditorGUI.GetPropertyHeight (tierProp, label, true) + svs;

            so.ApplyModifiedProperties ();

        }

        EditorGUI.EndProperty ();

    }

}