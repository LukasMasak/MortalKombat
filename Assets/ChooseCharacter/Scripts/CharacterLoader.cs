using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public static class CharacterLoader
{
    // --------------File-Saving-Constants--------------------
    private const string CHARACTER_FOLDER = "/Characters";
    private const string ATTACK_ANIM_FOLDER = "/AttackAnim";
    private const string WALK_ANIM_FOLDER = "/WalkAnim";
    private const string BLOCK_ANIM_FOLDER = "/BlockAnim";
    private const string JUMP_ANIM_FOLDER = "/JumpAnim";
    private const string HURT_ANIM_FOLDER = "/HurtAnim";
    private const string DEATH_ANIM_FOLDER = "/DeathAnim";
    private const string README_FILE = "/README.txt";
    private const string README_CONTENT =
     "Folder anim, yes, dont touch config, no no";
    private const string CONFIG_FILE = "/config.txt";
    private const string CONFIG_HP_NAME = "health";
    private const string CONFIG_DMG_NAME = "damage";
    private const string CONFIG_SPD_NAME = "speed";
    private const string CONFIG_JMP_NAME = "jump";
    private const string CONFIG_ATK_FRAME_NAME = "attackFrame";
    private const string CONFIG_ATK_POINT_NAME = "attackDistance";
    private const string CONFIG_ATK_SIZE_NAME = "attackPointSize";


    // ---------------Animation-Constants----------------------
    private const float FRAME_DELAY = 1f / 24f;

    
    // Loads CharacterData from the character folder using the name of the character
    // When the character is not present, creates a new one by that name
    public static CharacterData LoadFromFile(string characterName)
    {
        CharacterData characterData = new CharacterData{name = characterName};
        string basePath = Application.dataPath + "/" + CHARACTER_FOLDER + "/" + characterName;

        // Check if the character exists and if not create it
        if (!Directory.Exists(basePath)) 
        {
            Debug.Log("Character \"" + characterName + "\" not found, creating a new character...");
            return CreateFreshCharacter(characterName);
        }

        // Load config
        string[] configFileLines = File.ReadAllLines(basePath + CONFIG_FILE);
        
        // TODO call parse and check validity, determine null CharacterData (bool valid in data)

        return characterData;
    }

    // Creates a fresh character from name with default values
    public static CharacterData CreateFreshCharacter(string name)
    {
        CharacterData characterData = GetDefaultCharacterData(name);
        string basePath = Application.dataPath  + CHARACTER_FOLDER + "/" + name;

        // Create all directories
        Directory.CreateDirectory(basePath);
        Directory.CreateDirectory(basePath + ATTACK_ANIM_FOLDER);
        Directory.CreateDirectory(basePath + WALK_ANIM_FOLDER);
        Directory.CreateDirectory(basePath + BLOCK_ANIM_FOLDER);
        Directory.CreateDirectory(basePath + JUMP_ANIM_FOLDER);
        Directory.CreateDirectory(basePath + HURT_ANIM_FOLDER);
        Directory.CreateDirectory(basePath + DEATH_ANIM_FOLDER);

        // Create the README file
        FileStream readmeFile = File.Create(basePath + README_FILE);
        byte[] readmeContentBytes = new UTF8Encoding(true).GetBytes(README_CONTENT);
        readmeFile.Write(readmeContentBytes, 0, readmeContentBytes.Length);

        // Create the Config file
        FileStream configFile = File.Create(basePath + CONFIG_FILE);
        byte[] configContentBytes = new UTF8Encoding(true).GetBytes(GenerateConfigString(characterData));
        configFile.Write(configContentBytes, 0, configContentBytes.Length);

        return characterData;
    }

    // Creates a default instance of CharacterData
    private static CharacterData GetDefaultCharacterData(string characterName)
    {
        return new CharacterData
        {
            name = characterName,
            speed = 5,
            jump = 4,
            health = 100,
            damage = 20,
            attackPointOffset = new Vector2(1,0),
            attackFrameIdx = 0,
            attackSize = 1,
        };
    }

    // Generates the config string from CharacterData
    private static string GenerateConfigString(CharacterData data)
    {
        string configString = "";
        configString += CONFIG_SPD_NAME + "=" + data.speed + "\n";
        configString += CONFIG_JMP_NAME + "=" + data.jump + "\n";
        configString += CONFIG_HP_NAME + "=" + data.health + "\n";
        configString += CONFIG_DMG_NAME + "=" + data.damage + "\n";
        configString += CONFIG_ATK_POINT_NAME + "=" + data.attackPointOffset.x
                                             + "," + data.attackPointOffset.y + "\n";
        configString += CONFIG_ATK_FRAME_NAME + "=" + data.attackFrameIdx + "\n";
        configString += CONFIG_ATK_SIZE_NAME + "=" + data.attackSize + "\n";
        return configString;
    }

    // Parse data from config file
    private static bool ParseLinesFromConfig(ref string[] configLines, ref CharacterData data)
    {
        string errorString = "";

        foreach (string line in configLines)
        {
            bool foundConfig = false;
            if (line.StartsWith(CONFIG_SPD_NAME))
            {
                if (float.TryParse(line.Substring(CONFIG_SPD_NAME.Length), out float readSpeed))
                {

                }
                else
                {
                    errorString = "Could not parse float value of speed from config file!";
                    break;
                }
            }
        }

        if (errorString.Length > 0)
        {
            Debug.LogError("\""+ data.name + "\" character ERROR: " + errorString);
            Debug.LogError("----> Character will not be loaded!");
            return false;
        }

        return true;
    }
}
