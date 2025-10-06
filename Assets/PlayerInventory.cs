using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    public List<string> bag = new List<string>();        // Bag (max 5)
    public List<string> selected = new List<string>();   // Selected (max 2)
    public Image[] bagSlots;                              // UI Images for bag slots (size should be >= maxBagSize)
    private int maxBagSize = 5;
    private int maxSelectedSize = 2;
    public ColorMixtureStation mixtureStation; // optional reference to the station (set by station when player enters)

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
        if (IsValidColorTag(tag))
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
        if (bag.Contains(color)&& (color == "Red" || color == "Yellow" || color == "Blue")&& mixtureStation != null && mixtureStation.playerNearby)
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
            Debug.Log($"{color} not in bag.");
        }
    }

    // --- Button click functions ---
    public void OnButton1Click() => TrySelectColor(ColorToName(image1.color));
    public void OnButton2Click() => TrySelectColor(ColorToName(image2.color));
    public void OnButton3Click() => TrySelectColor(ColorToName(image3.color));
    public void OnButton4Click() => TrySelectColor(ColorToName(image4.color));
    public void OnButton5Click() => TrySelectColor(ColorToName(image5.color));

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

    // Six color-specific wrappers
    public void RemoveRed()    => RemoveColorFromBag("Red");
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

//     [Header("Controls")]
//     public Button mixButton;                         // assign Mix button

//     // internal tracking:
//     // which bag-slot index supplied the color for each sprite; -1 = none
//     private int[] spriteOriginIndex = new int[] { -1, -1 };
//     // what color strings the sprites currently hold (null if empty)
//     private string[] spriteColor = new string[] { null, null };
//     // allow the station to set itself on the inventory (optional, used by your station)
//     [HideInInspector] public ColorMixtureStation mixtureStation;

//     // Remove items at the given bag indices (used by ColorMixtureStation)
//     public void RemoveColors(List<int> indices)
//     {
//         if (indices == null || indices.Count == 0) return;

//         // remove from highest index to lowest so indices remain valid
//         indices.Sort((a, b) => b.CompareTo(a));
//         foreach (int idx in indices)
//         {
//             if (idx >= 0 && idx < bag.Count)
//             {
//                 bag.RemoveAt(idx);
//             }
//         }

//         // ensure list size is not less than expected UI slots (optional)
//         while (bag.Count < bagSlots.Length) bag.Add(null);

//         UpdateBagUI();
//     }

//     // Add a color to the bag (used by ColorMixtureStation)
//     public void AddColor(string color)
//     {
//         if (string.IsNullOrEmpty(color)) return;

//         // First try to place into an empty slot (null)
//         for (int i = 0; i < maxItems; i++)
//         {
//             if (i >= bag.Count)
//             {
//                 // extend bag and place
//                 bag.Add(color);
//                 //UpdateBagUI();
//                 return;
//             }
//             else if (bag[i] == null)
//             {
//                 bag[i] = color;
//                 //UpdateBagUI();
//                 return;
//             }
//         }

//         // If no empty slot and bag not full, append
//         if (bag.Count < maxItems)
//         {
//             bag.Add(color);
//             UpdateBagUI();
//             return;
//         }

//         // Bag full: replace first slot (or you can drop it / show message)
//         Debug.Log("AddColor: bag is full. Could not add " + color);
//     }


//     void Start()
//     {
//         // ensure UI initialised
//         UpdateBagUI();
//         ClearSelectionSprite(0);
//         ClearSelectionSprite(1);

//         mixButton.onClick.AddListener(OnMixButtonPressed);
//         mixButton.gameObject.SetActive(false);
//     }

//     // --- UI hookup methods ---
//     // Hook each bag slot button to call this with its index (0..4)
//     public void OnClickBagSlot(int slotIndex)
//     {
//         // ignore invalid clicks
//         if (slotIndex < 0 || slotIndex >= bagSlots.Length) return;

//         // if slotIndex is out of current bag contents (empty) do nothing
//         if (slotIndex >= bag.Count) return;

//         // If this bag slot is already used as origin for a sprite, do nothing.
//         if (spriteOriginIndex[0] == slotIndex || spriteOriginIndex[1] == slotIndex)
//             return;

//         // find first free selection sprite (0 => sprite1, 1 => sprite2)
//         int freeSprite = -1;
//         if (spriteColor[0] == null) freeSprite = 0;
//         else if (spriteColor[1] == null) freeSprite = 1;

//         if (freeSprite == -1)
//         {
//             // both selection sprites occupied — ignore or you can show feedback
//             Debug.Log("Both selection slots are full. Deselect one to select another.");
//             return;
//         }

//         // move color from bag slot to the selection sprite
//         string colorName = bag[slotIndex];
//         spriteColor[freeSprite] = colorName;
//         spriteOriginIndex[freeSprite] = slotIndex;

//         // set sprite image color
//         GetSpriteByIndex(freeSprite).color = GetColorOrWhite(colorName);

//         // set bag slot to white (default) and mark bag entry as "empty" by using a placeholder
//         // We'll keep the bag list length same but mark that slot as empty by replacing with null string sentinel
//         bag[slotIndex] = null;
//         bagSlots[slotIndex].color = Color.white;

//         UpdateMixButtonState();
//     }

//     // Hook selection sprites to call this with 0 for sprite1 and 1 for sprite2 (to deselect)
//     public void OnClickSelectionSprite(int spriteIndex)
//     {
//         if (spriteIndex < 0 || spriteIndex > 1) return;
//         if (spriteColor[spriteIndex] == null) return; // nothing to deselect

//         int origin = spriteOriginIndex[spriteIndex];
//         string col = spriteColor[spriteIndex];

//         // restore color back to the original bag slot if still empty (bag[origin] == null)
//         if (origin >= 0 && origin < bag.Count && bag[origin] == null)
//         {
//             bag[origin] = col;
//             bagSlots[origin].color = GetColorOrWhite(col);
//         }
//         else
//         {
//             // original slot unavailable (rare). fallback: put in first empty bag slot or append if space
//             bool restored = false;
//             for (int i = 0; i < maxItems; i++)
//             {
//                 if (i >= bag.Count)
//                 {
//                     bag.Add(col);
//                     bagSlots[i].color = GetColorOrWhite(col);
//                     restored = true;
//                     break;
//                 }
//                 else if (bag[i] == null)
//                 {
//                     bag[i] = col;
//                     bagSlots[i].color = GetColorOrWhite(col);
//                     restored = true;
//                     break;
//                 }
//             }
//             if (!restored) Debug.Log("No space to restore the item back to bag.");
//         }

//         // clear selection sprite
//         ClearSelectionSprite(spriteIndex);
//         UpdateMixButtonState();
//     }

//     // --- Replace existing OnMixButtonPressed with this ---
// public void OnMixButtonPressed()
// {
//     // require both sprites selected
//     if (spriteColor[0] == null || spriteColor[1] == null) return;

//     string c1 = spriteColor[0];
//     string c2 = spriteColor[1];
//     string result = GetMixedColor(c1, c2);

//     if (string.IsNullOrEmpty(result))
//     {
//         Debug.Log("Invalid mix - no result.");
//         return;
//     }

//     // determine target bag slot BEFORE clearing selection
//     int targetSlot = -1;
//     if (spriteOriginIndex[0] >= 0 && spriteOriginIndex[0] < maxItems)
//         targetSlot = spriteOriginIndex[0];

//     // if origin invalid, find first empty or append spot
//     if (targetSlot == -1 || targetSlot >= bag.Count)
//     {
//         for (int i = 0; i < maxItems; i++)
//         {
//             if (i >= bag.Count) bag.Add(null);
//             if (bag[i] == null)
//             {
//                 targetSlot = i;
//                 break;
//             }
//         }
//     }

//     // start the visual mix animation; after animation we will assign the color to targetSlot
//     StartCoroutine(DoMixAnimation(result, targetSlot));
// }


//     // --- helpers ---
//     private void UpdateMixButtonState()
//     {
//         // show mix button only when both sprites have a valid primary combo
//         if (spriteColor[0] != null && spriteColor[1] != null)
//         {
//             string result = GetMixedColor(spriteColor[0], spriteColor[1]);
//             mixButton.gameObject.SetActive(!string.IsNullOrEmpty(result));
//         }
//         else
//         {
//             mixButton.gameObject.SetActive(false);
//         }
//     }

//     private void ClearSelectionSprite(int index)
//     {
//         if (index < 0 || index > 1) return;
//         spriteColor[index] = null;
//         spriteOriginIndex[index] = -1;
//         GetSpriteByIndex(index).color = Color.white;
//     }

//     private Image GetSpriteByIndex(int i)
//     {
//         return (i == 0) ? sprite1 : sprite2;
//     }

//     // return color by name or white if missing
//     private Color GetColorOrWhite(string name)
//     {
//         if (string.IsNullOrEmpty(name)) return Color.white;
//         if (colorMap.ContainsKey(name)) return colorMap[name];
//         return Color.white;
//     }

//     // standard mixes
//     private string GetMixedColor(string c1, string c2)
//     {
//         // accept nulls
//         if (string.IsNullOrEmpty(c1) || string.IsNullOrEmpty(c2)) return null;

//         if ((c1 == "Red" && c2 == "Blue") || (c1 == "Blue" && c2 == "Red")) return "Violet";
//         if ((c1 == "Red" && c2 == "Yellow") || (c1 == "Yellow" && c2 == "Red")) return "Orange";
//         if ((c1 == "Blue" && c2 == "Yellow") || (c1 == "Yellow" && c2 == "Blue")) return "Green";

//         return null; // not a primary combo
//     }

//     // update bag UI from bag[] contents
//     public void UpdateBagUI()
//     {
//         // make sure bag list fits the bagSlots length
//         while (bag.Count < bagSlots.Length) bag.Add(null);

//         for (int i = 0; i < bagSlots.Length; i++)
//         {
//             string col = (i < bag.Count) ? bag[i] : null;
//             if (!string.IsNullOrEmpty(col))
//                 bagSlots[i].color = GetColorOrWhite(col);
//             else
//                 bagSlots[i].color = Color.white;
//         }
//     }
//     private IEnumerator DoMixAnimation(string resultColor, int targetSlot)
// {
//     // animation parameters (tweak to taste)
//     float slideDuration = 0.20f;
//     float pauseAfterSlide = 0.08f;
//     float popDuration = 0.12f;
//     float slideOffset = 80f; // pixels to slide sprite2 by (positive = right)

//     RectTransform rt1 = sprite1.rectTransform;
//     RectTransform rt2 = sprite2.rectTransform;

//     Vector2 rt2Orig = rt2.anchoredPosition;
//     Vector2 rt2Target = rt2Orig + new Vector2(slideOffset, 0f);

//     // 1) slide sprite2 aside to create space
//     yield return StartCoroutine(MoveRect(rt2, rt2Orig, rt2Target, slideDuration));

//     // small pause
//     yield return new WaitForSeconds(pauseAfterSlide);

//     // 2) show result by changing sprite1 color and give a small pop animation
//     sprite1.color = GetColorOrWhite(resultColor);
//     yield return StartCoroutine(ScaleRect(rt1, Vector3.one, Vector3.one * 1.15f, popDuration)); // pop out
//     yield return StartCoroutine(ScaleRect(rt1, Vector3.one * 1.15f, Vector3.one, popDuration)); // back

//     // 3) slide sprite2 back to its original place
//     yield return StartCoroutine(MoveRect(rt2, rt2Target, rt2Orig, slideDuration));

//     // 4) finalize: clear selection sprites and place resulting color into target bag slot
//     // Clear selection tracking
//     spriteColor[0] = spriteColor[1] = null;
//     int origin0 = spriteOriginIndex[0];
//     int origin1 = spriteOriginIndex[1];
//     spriteOriginIndex[0] = spriteOriginIndex[1] = -1;

//     // set both selection sprites to white
//     sprite1.color = Color.white;
//     sprite2.color = Color.white;

//     // put result into the chosen bag slot (if there is one)
//     if (targetSlot != -1)
//     {
//         bag[targetSlot] = resultColor;
//     }
//     else
//     {
//         Debug.Log("No target slot found to place mixed color.");
//     }

//     // refresh UI
//     UpdateBagUI();

//     // hide mix button
//     mixButton.gameObject.SetActive(false);
// }

// private IEnumerator MoveRect(RectTransform rt, Vector2 from, Vector2 to, float duration)
// {
//     float t = 0f;
//     while (t < duration)
//     {
//         t += Time.deltaTime;
//         float f = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(t / duration));
//         rt.anchoredPosition = Vector2.Lerp(from, to, f);
//         yield return null;
//     }
//     rt.anchoredPosition = to;
// }

// private IEnumerator ScaleRect(RectTransform rt, Vector3 from, Vector3 to, float duration)
// {
//     float t = 0f;
//     while (t < duration)
//     {
//         t += Time.deltaTime;
//         float f = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(t / duration));
//         rt.localScale = Vector3.Lerp(from, to, f);
//         yield return null;
//     }
//     rt.localScale = to;
// }

// }


