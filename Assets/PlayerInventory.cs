using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerInventory : MonoBehaviour
{
    public List<string> bag = new List<string>();        // Bag (max 5)
    public List<string> selected = new List<string>();   // Selected (max 2)
    public Image[] bagSlots;                              // UI Images for bag slots (size should be >= maxBagSize)
    private int maxBagSize = 5;
    private int maxSelectedSize = 2;
    public ColorMixtureStation mixtureStation; // optional reference to the station (set by station when player enters)
    public int motes = 0;
    public TMP_Text moteText;

    // UI Images for the 5 color buttons/images
    public Image image1;
    public Image image2;
    public Image image3;
    public Image image4;
    public Image image5;
    public Image[] selectedImages; // array of 2 images for selected colors
    public GameObject mixButton; // assign Mix button

    // Called when picking up color objects in the world (make sure the pickups have "Is Trigger" on)
    private void OnTriggerEnter2D(Collider2D other)
    {
        string tag = other.tag;
        if (IsValidColorTag(tag)&& bag.Count < maxBagSize)
        {
            AddToBag(tag);
            Destroy(other.gameObject);
        }
    }

    private bool IsValidColorTag(string tag)
    {
        return tag == "Red" || tag == "Yellow" || tag == "Blue" ||
               tag == "Green" || tag == "Violet" || tag == "Orange";
    }

    public void AddToBag(string color)
    {
        if (bag.Count < maxBagSize)
        {
            bag.Add(color);
            Debug.Log($"{color} added to bag. Bag: {string.Join(", ", bag)}");
        }
        else
        {
            Debug.Log("Bag is full!");
        }
    }


    private void TrySelectColor(string color)
    {
        if (bag.Contains(color) && (color == "Red" || color == "Yellow" || color == "Blue") && mixtureStation != null && mixtureStation.playerNearby)
        {
            if (selected.Count < maxSelectedSize)
            {
                bag.Remove(color);
                selected.Add(color);
                Debug.Log($"{color} moved to selected. Selected: {string.Join(", ", selected)}");
            }
            else
            {
                Debug.Log("Selected list full!");
            }
        }
        else
        {
            bag.Remove(color);
            motes++;
            //SpawnColorPrefab(color, transform.position + Vector3.right * 2f);
        }
    }


    // --- Button click functions ---
    public void OnButton1Click() => TrySelectColor(ColorToName(image1.color));
    public void OnButton2Click() => TrySelectColor(ColorToName(image2.color));
    public void OnButton3Click() => TrySelectColor(ColorToName(image3.color));
    public void OnButton4Click() => TrySelectColor(ColorToName(image4.color));
    public void OnButton5Click() => TrySelectColor(ColorToName(image5.color));
    public void buy1()
    {
        if (motes < 2)
        {
            Debug.Log("Not enough motes to buy color!");
            return;
        }
        bag.Add("Red");
        motes -= 2;
    }
    public void buy2()
    {
        if (motes < 2)
        {
            Debug.Log("Not enough motes to buy color!");
            return;
        }
        bag.Add("Blue");
        motes -= 2;
    }
    public void buy3()
    {
        if (motes < 2)
        {
            Debug.Log("Not enough motes to buy color!");
            return;
        }
        bag.Add("Yellow");
        motes -= 2;
    }
    private string ColorToName(Color color)
    {
        if (color == Color.red) return "Red";
        if (color == Color.yellow) return "Yellow";
        if (color == Color.blue) return "Blue";
        if (color == Color.green) return "Green";
        if (color == new Color(0.5f, 0f, 0.5f)) return "Violet";
        if (color == new Color(1f, 0.65f, 0f)) return "Orange";
        return "Unknown";
    }
// Helper that removes a single instance of color from bag and logs result
    private void RemoveColorFromBag(string color)
    {
        if (bag.Remove(color)) // List.Remove removes the first occurrence and returns true if removed
        {
            Debug.Log($"{color} removed from bag. Bag now: {string.Join(", ", bag)}");
        }
        else
        {
            Debug.Log($"{color} not found in bag.");
        }
    }
    public void RemoveSelectedColor1()
    {
        if (selected.Count > 0)
        {
            string col = selected[0];
            selected.RemoveAt(0);
            if (bag.Count < maxBagSize)
            {
                bag.Add(col);
                Debug.Log($"{col} moved back to bag. Bag: {string.Join(", ", bag)} | Selected: {string.Join(", ", selected)}");
            }
            else
            {
                Debug.Log("Bag is full! Cannot move selected color back.");
                // If bag is full, we can choose to drop the color or keep it in selected.
                // Here we just log and do not add back to bag.
            }
        }
        else
        {
            Debug.Log("No colors to deselect!");
        }

    }
    public void RemoveSelectedColor2()
    {
        if (selected.Count > 1)
        {
            string col = selected[1];
            selected.RemoveAt(1);
            if (bag.Count < maxBagSize)
            {
                bag.Add(col);
                Debug.Log($"{col} moved back to bag. Bag: {string.Join(", ", bag)} | Selected: {string.Join(", ", selected)}");
            }
            else
            {
                Debug.Log("Bag is full! Cannot move selected color back.");
                // If bag is full, we can choose to drop the color or keep it in selected.
                // Here we just log and do not add back to bag.
            }
        }
        else
        {
            Debug.Log("No second color to deselect!");
        }

    }
    // Six color-specific wrappers
    public void RemoveRed() => RemoveColorFromBag("Red");
    public void RemoveBlue()   => RemoveColorFromBag("Blue");
    public void RemoveYellow() => RemoveColorFromBag("Yellow");
    public void RemoveGreen()  => RemoveColorFromBag("Green");
    public void RemoveViolet() => RemoveColorFromBag("Violet");
    public void RemoveOrange() => RemoveColorFromBag("Orange");

    // --- MIX FUNCTION ---
    public void Mix()
    {
        if (selected.Count < 2)
        {
            Debug.Log("Select two colors to mix!");
            return;
        }

        string c1 = selected[0];
        string c2 = selected[1];

        string mixResult = GetMixResult(c1, c2);

        if (mixResult != null)
        {
            // consume selected colors
            selected.Clear();

            if (bag.Count < maxBagSize)
            {
                bag.Add(mixResult);
                Debug.Log($"Mixed {c1} + {c2} = {mixResult}. Added to bag.");
            }
            else
            {
                Debug.Log("Bag is full! Cannot add mixed color.");
            }
        }
        else
        {
            Debug.Log($"Cannot mix {c1} and {c2}. No valid combination found.");
        }
    }

    private string GetMixResult(string color1, string color2)
    {
        List<string> pair = new List<string> { color1, color2 };
        pair.Sort();
        string combo = string.Join("-", pair);

        switch (combo)
        {
            case "Red-Yellow": return "Orange";
            case "Blue-Yellow": return "Green";
            case "Blue-Red": return "Violet";
            default: return null;
        }
    }

    // --- DESELECT FUNCTION ---
    // Moves as many selected items back to the bag as space allows, and removes only those moved from selected.
    public void Deselect()
    {
        if (selected.Count == 0)
        {
            Debug.Log("No colors to deselect!");
            return;
        }

        int space = maxBagSize - bag.Count;
        if (space <= 0)
        {
            Debug.Log("Bag is full! Cannot move selected colors back.");
            return;
        }

        int moved = Mathf.Min(space, selected.Count);

        for (int i = 0; i < moved; i++)
        {
            bag.Add(selected[i]);
            Debug.Log($"{selected[i]} moved back to bag.");
        }

        // Remove the moved items from selected (remove first 'moved' elements)
        if (moved > 0)
            selected.RemoveRange(0, moved);

        Debug.Log($"After deselect: Bag: {string.Join(", ", bag)} | Selected: {string.Join(", ", selected)}");
    }

    // Update the UI bag slots to reflect current bag contents
    private void Update()
    {
        if (moteText != null)
            moteText.text = " x " +  motes.ToString();
        for (int j = 0; j < 2; j++)
        {
            if (j < selected.Count)
            {
                string col = selected[j];
                switch (col)
                {
                    case "Red":
                        selectedImages[j].color = Color.red;
                        break;
                    case "Blue":
                        selectedImages[j].color = Color.blue;
                        break;
                    case "Yellow":
                        selectedImages[j].color = Color.yellow;
                        break;
                    default:
                        selectedImages[j].color = Color.white;
                        break;
                }
            }
            else
            {
                // empty slot
                selectedImages[j].color = Color.white;
            }
        }
        if (selected.Count == 2 && mixButton != null
                && mixtureStation != null && mixtureStation.playerNearby)
            {
                mixButton.SetActive(true);
            }
            else
            {
                mixButton.SetActive(false);
            }

        for (int i = 0; i < bagSlots.Length; i++)
        {
            if (i < bag.Count)
            {
                string col = bag[i];
                switch (col)
                {
                    case "Red":
                        bagSlots[i].color = Color.red;
                        break;
                    case "Blue":
                        bagSlots[i].color = Color.blue;
                        break;
                    case "Yellow":
                        bagSlots[i].color = Color.yellow;
                        break;
                    case "Green":
                        bagSlots[i].color = Color.green;
                        break;
                    case "Orange":
                        bagSlots[i].color = new Color(1f, 0.65f, 0f);
                        break;
                    case "Violet":
                        bagSlots[i].color = new Color(0.5f, 0f, 0.5f);
                        break;
                    default:
                        bagSlots[i].color = Color.white;
                        break;
                }
            }
            else
            {
                // empty slot
                bagSlots[i].color = Color.white;
            }
        }
    }
}