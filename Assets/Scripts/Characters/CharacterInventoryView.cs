﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class CharacterInventoryView : MonoBehaviour {

    [SerializeField]
    private GameObject InventoryBox;

    private List<ItemSlot> m_ItemSlots;
    private int m_EmptySlots;
    private int m_SelectedIndex;

    private void Awake() {
        if (InventoryBox == null) {
            Debug.LogError("No Inventory panel attached!", gameObject);
            enabled = false;
        }

        m_ItemSlots = new List<ItemSlot>(InventoryBox.GetComponentsInChildren<ItemSlot>());
    }

    private void Start() {
        m_SelectedIndex = 0;
        m_EmptySlots = m_ItemSlots.Count;
        PreviousItem();
    }

    public void NextItem() {
        m_ItemSlots[m_SelectedIndex].Select(false);
        if (m_SelectedIndex < m_ItemSlots.Count - 1) m_SelectedIndex++;
        m_ItemSlots[m_SelectedIndex].Select(true);
    }

    public void PreviousItem() {
        m_ItemSlots[m_SelectedIndex].Select(false);
        if (m_SelectedIndex > 0) m_SelectedIndex--;
        m_ItemSlots[m_SelectedIndex].Select(true);
    }

    public ItemType GetSelectedItem() {
        foreach (ItemSlot slot in m_ItemSlots) {
            if (slot.Selected) {
                return slot.Type;
            }
        }

        return ItemType.NONE;
    }

    public void AddItem(ItemType itemType, int amount) {

        bool typeFound = false;

        foreach (ItemSlot slot in m_ItemSlots) {
            if (slot.Type == itemType) {
                typeFound = true;
                slot.AddItem(itemType, amount);
                break;
            }
        }

        if (!typeFound) PlaceEmpty(itemType, amount);
    }

    public void RemoveItem(ItemType itemType, int amount) {
        foreach (ItemSlot slot in m_ItemSlots) {
            if (slot.Type == itemType) {
                slot.RemoveItem(amount);
            }
        }
    }

    private bool PlaceEmpty(ItemType itemType, int amount) {
        if (m_EmptySlots == 0) return false;

        foreach (ItemSlot slot in m_ItemSlots) {
            if (slot.IsEmpty) {
                slot.AddItem(itemType, amount);
                m_EmptySlots--;
                return true;
            }
        }

        return false;
    }
}
