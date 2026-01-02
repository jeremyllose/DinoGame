using UnityEngine;
using StarterAssets;

public class CharacterSwitcher : MonoBehaviour
{
    [Header("Main Controller")]
    public FirstPersonController playerController;

    [Header("Dino Models")]
    public GameObject velociraptor;
    public GameObject stegosaurus;
    public GameObject pterodactyl;
    public GameObject trex;

    [Header("Mechanics & Abilities")]
    public SurvivalStats thirstSystem; // DRAG PLAYER HERE
    public MonoBehaviour stegoAbility;
    public MonoBehaviour pteroAbility; 
    public MonoBehaviour trexAbility;

    public void SwitchCharacter(int levelNumber)
    {
        Debug.Log($"Switching Character for Level {levelNumber}");

        // 1. Reset Everything (Hide all models AND mechanics)
        velociraptor.SetActive(false);
        stegosaurus.SetActive(false);
        pterodactyl.SetActive(false);
        trex.SetActive(false);

        if (stegoAbility) stegoAbility.enabled = false;
        if (pteroAbility) pteroAbility.enabled = false;
        if (trexAbility) trexAbility.enabled = false;
        
        // DISABLE THIRST by default
        if (thirstSystem != null) thirstSystem.enabled = false;

        // Reset Flight Mode
        if (playerController != null) playerController.IsFlying = false;

        // 2. Activate based on Level Number
        if (levelNumber == 1) // Velociraptor
        {
            velociraptor.SetActive(true);
        }
        else if (levelNumber == 2) // Stego (Desert)
        {
            stegosaurus.SetActive(true);
            
            // Enable Stego Ability
            if (stegoAbility) stegoAbility.enabled = true;

            // ENABLE THIRST (Only for this level)
            if (thirstSystem != null) thirstSystem.enabled = true;
        }
        else if (levelNumber == 3) // Ptero (Sky)
        {
            pterodactyl.SetActive(true);
            if (pteroAbility) pteroAbility.enabled = true;
            if (playerController != null) playerController.IsFlying = true;
        }
        else if (levelNumber == 4) // T-Rex (Lava)
        {
            trex.SetActive(true);
            if (trexAbility) trexAbility.enabled = true;
        }
        else
        {
            velociraptor.SetActive(true);
        }

        // 3. Update Animator Reference
        if (playerController != null)
        {
            playerController.UpdateAnimatorReference();
        }
    }
}