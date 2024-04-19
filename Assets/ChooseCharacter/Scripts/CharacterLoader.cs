using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;

public static class CharacterLoader
{
    // --------------File-Saving-Constants--------------------
    private const string CHARACTER_FOLDER = "/Characters";
    private const string IDLE_ANIM_FOLDER = "/IdleAnim";
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
    private const float FRAMERATE = 24f;
    private const float FRAME_DELAY = 1f / FRAMERATE;
    private const string IDLE_ANIM_NAME = "Idle";
    private const string ATTACK_ANIM_NAME = "Attack";
    private const string MOVE_ANIM_NAME = "Move";
    private const string BLOCK_ANIM_NAME = "Block";
    private const string JUMP_ANIM_NAME = "Jump";
    private const string HURT_ANIM_NAME = "Hurt";
    private const string DEATH_ANIM_NAME = "Death";


    
    // Loads CharacterData from the character folder using the name of the character
    // When the character is not present, creates a new one by that name
    public static CharacterData LoadFromFile(string characterName)
    {
        CharacterData characterData = new CharacterData{name = characterName};
        string basePath = Application.dataPath + CHARACTER_FOLDER + "/" + characterName;

        // Check if the character exists and if not create it
        if (!Directory.Exists(basePath)) 
        {
            Debug.Log("Character \"" + characterName + "\" not found, creating a new character...");
            return CreateFreshCharacter(characterName);
        }

        // Load config
        string[] configFileLines = File.ReadAllLines(basePath + CONFIG_FILE);

        // Check for corruprion in config file
        if (!TryParseLinesFromConfig(ref configFileLines, ref characterData))
        {
            characterData.isValid = false;
            return characterData;
        }
        characterData.isValid = true;
        
        // Check if animation folders exist
        if (!CheckAnimationFolders(characterName))
        {
            characterData.isValid = false;
            return characterData;
        }

        // Load sprites for the animations
        LoadAllAnimations(ref characterData);

        return characterData;
    }

    // Creates a fresh character from name with default values
    public static CharacterData CreateFreshCharacter(string name)
    {
        CharacterData characterData = GetDefaultCharacterData(name);
        string basePath = Application.dataPath  + CHARACTER_FOLDER + "/" + name;

        // Create all directories
        Directory.CreateDirectory(basePath);
        Directory.CreateDirectory(basePath + IDLE_ANIM_FOLDER);
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
    // Returns if config was valid
    private static bool TryParseLinesFromConfig(ref string[] configLines, ref CharacterData data)
    {
        string errorString = "";

        foreach (string line in configLines)
        {
            // Parse the speed
            if (line.StartsWith(CONFIG_SPD_NAME))
            {
                string valueSubstring = line.Substring(CONFIG_SPD_NAME.Length + 1);
                if (float.TryParse(valueSubstring, out float value))
                {
                    data.speed = value;
                }
                else
                {
                    errorString = "Could not parse float value of speed from config file! Value found = " + valueSubstring;
                    break;
                }
            }
            
            // Parse the jump
            else if (line.StartsWith(CONFIG_JMP_NAME))
            {
                string valueSubstring = line.Substring(CONFIG_JMP_NAME.Length + 1);
                if (float.TryParse(valueSubstring, out float value))
                {
                    data.jump = value;
                }
                else
                {
                    errorString = "Could not parse float value of jump from config file! Value found = " + valueSubstring;
                    break;
                }
            }

            // Parse the health
            else if (line.StartsWith(CONFIG_HP_NAME))
            {
                string valueSubstring = line.Substring(CONFIG_HP_NAME.Length + 1);
                if (uint.TryParse(valueSubstring, out uint value))
                {
                    data.health = value;
                }
                else
                {
                    errorString = "Could not parse uint value of health from config file! Value found= " + valueSubstring;
                    break;
                }
            }

            // Parse the damage
            else if (line.StartsWith(CONFIG_DMG_NAME))
            {
                string valueSubstring = line.Substring(CONFIG_DMG_NAME.Length + 1);
                if (uint.TryParse(valueSubstring, out uint value))
                {
                    data.damage = value;
                }
                else
                {
                    errorString = "Could not parse uint value of damage from config file! Value found= " + valueSubstring;
                    break;
                }
            }

            // Parse the attack point offset
            else if (line.StartsWith(CONFIG_ATK_POINT_NAME))
            {
                int commaIdx = line.IndexOf(',');
                string valueSubstringX = line.Substring(CONFIG_ATK_POINT_NAME.Length + 1, commaIdx - CONFIG_ATK_POINT_NAME.Length);
                float x, y;
                if (float.TryParse(valueSubstringX, out float valueX))
                {
                    x = valueX;
                }
                else
                {
                    errorString = "Could not parse float value of attack point offset X from config file! Value found= " + valueSubstringX;
                    break;
                }

                string valueSubstringY = line.Substring(commaIdx);
                if (float.TryParse(valueSubstringY, out float valueY))
                {
                    y = valueY;
                }
                else
                {
                    errorString = "Could not parse float value of attack point offset X from config file! Value found= " + valueSubstringY;
                    break;
                }

                data.attackPointOffset = new Vector2(x, y);
            }

            // Parse attack frame
            else if (line.StartsWith(CONFIG_ATK_FRAME_NAME))
            {
                string valueSubstring = line.Substring(CONFIG_ATK_FRAME_NAME.Length + 1);
                if (uint.TryParse(valueSubstring, out uint value))
                {
                    data.attackFrameIdx = value;
                }
                else
                {
                    errorString = "Could not parse uint value of attack frame from config file! Value found= " + valueSubstring;
                    break;
                }
            }
            
            // Parse the attack point size
            else if (line.StartsWith(CONFIG_ATK_SIZE_NAME))
            {
                string valueSubstring = line.Substring(CONFIG_ATK_SIZE_NAME.Length + 1);
                if (float.TryParse(valueSubstring, out float value))
                {
                    data.jump = value;
                }
                else
                {
                    errorString = "Could not parse float value of attack point size from config file! Value found= " + valueSubstring;
                    break;
                }
            }

            // Unknown 
            else 
            {
                errorString = "Unknown config setting! Name found= " + line;
                break;
            }
        }

        if (errorString.Length > 0)
        {
            Debug.LogError("\""+ data.name + "\" character ERROR: " + "\n" + errorString);
            Debug.LogError("----> Character will not be loaded!");
            return false;
        }

        return true;
    }

    // Check if all folders exist
    private static bool CheckAnimationFolders(string characterName)
    {
        string basePath = Application.dataPath + CHARACTER_FOLDER + "/" + characterName;
        string errorString = "";

        if (!Directory.Exists(basePath + IDLE_ANIM_FOLDER))
        {
            errorString = "Folder for Idle Animation does not exist!";
        }
        else if (!Directory.Exists(basePath + ATTACK_ANIM_FOLDER))
        {
            errorString = "Folder for Attack Animation does not exist!";
        }
        else if (!Directory.Exists(basePath + WALK_ANIM_FOLDER))
        {
            errorString = "Folder for Walk Animation does not exist!";
        }
        else if (!Directory.Exists(basePath + BLOCK_ANIM_FOLDER))
        {
            errorString = "Folder for Block Animation does not exist!";
        }
        else if (!Directory.Exists(basePath + JUMP_ANIM_FOLDER))
        {
            errorString = "Folder for Jump Animation does not exist!";
        }
        else if (!Directory.Exists(basePath + HURT_ANIM_FOLDER))
        {
            errorString = "Folder for Hurt Animation does not exist!";
        }
        else if (!Directory.Exists(basePath + DEATH_ANIM_FOLDER))
        {
            errorString = "Folder for Death Animation does not exist!";
        }

        if (errorString.Length > 0)
        {
            Debug.LogError("\""+ characterName + "\" character ERROR: " + "\n" + errorString);
            Debug.LogError("----> Character will not be loaded!");
            return false;
        }

        return true;
    }

    // Loads all animations for the character
    private static void LoadAllAnimations(ref CharacterData data)
    {
        string basePath = Application.dataPath + CHARACTER_FOLDER + "/" + data.name;

        // Idle Animation
        data.idleAnim = LoadAnimationFromPath(basePath + IDLE_ANIM_FOLDER, IDLE_ANIM_NAME);
    }

    private static AnimationClip LoadAnimationFromPath(string path, string animName)
    {
        AnimationClip animationClip = new AnimationClip();
        animationClip.name = animName;
        animationClip.frameRate = FRAMERATE;

        // LOAD ALL SPRITES
        Sprite[] sprites = Resources.LoadAll<Sprite>(path);

        EditorCurveBinding spriteBinding = new EditorCurveBinding();
        spriteBinding.type = typeof(SpriteRenderer);
        spriteBinding.path = "";
        spriteBinding.propertyName = "Sprite"; 

        ObjectReferenceKeyframe[] spriteKeyFrames = new ObjectReferenceKeyframe[sprites.Length];
        for(int i = 0; i < (sprites.Length); i++) {
            spriteKeyFrames[i] = new ObjectReferenceKeyframe();
            spriteKeyFrames[i].time = FRAME_DELAY * i;
            spriteKeyFrames[i].value = sprites[i];
        }
        AnimationUtility.SetObjectReferenceCurve(animationClip, spriteBinding, spriteKeyFrames);

        return animationClip;
    }
}
