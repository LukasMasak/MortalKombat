using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using System.Linq;
using System;

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
    private const string CONFIG_COL_WIDTH_NAME = "colliderWidth";
    private const string CONFIG_COL_HEIGHT_NAME = "colliderHeight";
    private const string CONFIG_COL_OFFSET_NAME = "colliderOffset";

    private const string BUBBLE_ICON_FILE = "/icon";
    private const string PREVIEW_FILE = "/preview";

    private static readonly string[] SPRITE_FILE_TYPES = {".jpg", ".jpeg", ".png"};

    private static readonly float[] ICON_SPRITE =
    {0,0,0,0, 0,0,0,0, 0,0,0,0, 0,0,0,0,
     0,0,0,0, 0,0,0,0, 0,0,0,0, 0,0,0,0,
     0,0,0,0, 0,0,0,0, 0,0,0,0, 0,0,0,0,
     0,0,0,0, 0,0,0,0, 0,0,0,0, 0,0,0,0,
     
     0,0,0,0, 0,0,0,0, 0,0,0,0, 0,0,0,0,
     1,1,1,0, 1,1,1,0, 1,1,1,0, 1,0,0,1,
     0,1,0,0, 1,0,0,0, 1,0,1,0, 1,0,1,1,
     0,1,0,0, 1,0,0,0, 1,0,1,0, 1,1,0,1,
     
     0,1,0,0, 1,0,0,0, 1,0,1,0, 1,0,0,1,
     1,1,1,0, 1,1,1,0, 1,1,1,0, 1,0,0,1,
     0,0,0,0, 0,0,0,0, 0,0,0,0, 0,0,0,0,
     0,0,0,0, 0,0,0,0, 0,0,0,0, 0,0,0,0,

     0,0,0,0, 0,0,0,0, 0,0,0,0, 0,0,0,0,
     0,0,0,0, 0,0,0,0, 0,0,0,0, 0,0,0,0,
     0,0,0,0, 0,0,0,0, 0,0,0,0, 0,0,0,0,
     0,0,0,0, 0,0,0,0, 0,0,0,0, 0,0,0,0,};

    private static readonly float[] PREVIEW_SPRITE =
    {
     0,0,0,0, 1,1,0,0, 0,1,1,0, 0,0,0,0,
     0,0,0,0, 0,1,0,0, 0,1,0,0, 0,0,0,0,
     0,0,0,0, 0,1,0,0, 0,1,0,0, 0,0,0,0,
     0,0,0,0, 0,0,1,0, 0,1,0,0, 0,0,0,0,
     0,0,0,0, 0,0,1,0, 0,1,0,0, 0,0,0,0,
     0,0,0,0, 0,0,0,1, 1,0,0,0, 0,0,0,0,
     0,0,0,0, 0,0,0,1, 1,0,0,0, 0,0,0,0,
     0,0,0,0, 0,0,0,1, 1,0,0,0, 0,0,0,0,
     0,0,1,1, 1,1,1,1, 1,1,1,1, 1,1,0,0,
     0,0,1,0, 1,1,0,1, 1,0,1,1, 0,0,0,0,
     0,0,1,0, 0,0,0,1, 1,0,0,0, 0,0,0,0,
     0,0,0,1, 0,0,1,1, 1,1,0,0, 0,0,0,0,
     0,0,0,0, 0,1,1,1, 1,1,1,0, 0,0,0,0,
     0,0,0,0, 0,1,1,1, 1,1,1,0, 0,0,0,0,
     0,0,0,0, 0,0,1,1, 1,1,0,0, 0,0,0,0,
     0,0,0,0, 0,0,0,1, 1,0,0,0, 0,0,0,0,
    };


    // ---------------Animation-Constants----------------------
    public const float FRAMERATE = 24f;
    public const float FRAME_DELAY = 1f / FRAMERATE;
    public const string IDLE_ANIM_NAME = "Idle";
    public const string ATTACK_ANIM_NAME = "Attack";
    public const string WALK_ANIM_NAME = "Move";
    public const string BLOCK_ANIM_NAME = "Block";
    public const string JUMP_ANIM_NAME = "Jump";
    public const string HURT_ANIM_NAME = "Hurt";
    public const string DEATH_ANIM_NAME = "Death";


    // Loads all characters in the Characters folder
    public static void LoadAllCharacters(List<CharacterData> characters)
    {
        string basePath = Application.dataPath + CHARACTER_FOLDER;
        string[] allPossibleCharacters = Directory.GetDirectories(basePath);

        GlobalState.AllCharacters.Clear();

        foreach (string folderPath in allPossibleCharacters)
        {
            CharacterData loadedCharacter = LoadFromFolder(folderPath);
            if (loadedCharacter.isValid) 
            {
                Debug.Log("Character " + loadedCharacter.name + " successfully loaded!");
                characters.Add(loadedCharacter);
            }
        }
    }


    // Creates a fresh character from name with default values
    // Return an index in the AllCharacters List in Global State
    public static int CreateFreshCharacter(string name)
    {
        CharacterData characterData = GetDefaultCharacterData(name);
        string basePath = Application.dataPath  + CHARACTER_FOLDER + "/" + name;

        if (Directory.Exists(basePath))
        {
            Debug.LogWarning("Character of name " + name + " already exists!");   
            int idx = GlobalState.GetCharacterIndexFromName(name);
            if (idx == -1) 
            {
                Debug.LogError("Characters folder exists but character is not loaded correctly! -> Wrong character data");
                idx = 0;
            }
            return idx;
        }

        // Create all directories
        try{
            Directory.CreateDirectory(basePath);
        }
        catch (IOException e)
        {
            Debug.LogWarning("Invalid directory name! Use valid characters. Full error: " + e.ToString());
            return 0;
        }
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
        readmeFile.Close();

        // Create the Config file
        FileStream configFile = File.Create(basePath + CONFIG_FILE);
        byte[] configContentBytes = new UTF8Encoding(true).GetBytes(GenerateConfigString(characterData));
        configFile.Write(configContentBytes, 0, configContentBytes.Length);
        configFile.Close();

        // Create a temp icon
        Texture2D tempIcon = new Texture2D(16, 16, TextureFormat.RFloat, false);
        tempIcon.SetPixelData(ICON_SPRITE, 0);
        tempIcon.filterMode = FilterMode.Point;
        SaveTextureAsPNG(tempIcon, basePath + BUBBLE_ICON_FILE + ".png");
        characterData.bubbleIcon = LoadSprite(basePath + BUBBLE_ICON_FILE + ".png");

        // Create a preview icon
        Texture2D tempPreview = new Texture2D(16, 16, TextureFormat.RFloat, false);
        tempPreview.SetPixelData(PREVIEW_SPRITE, 0);
        tempPreview.filterMode = FilterMode.Point;
        SaveTextureAsPNG(tempPreview, basePath + PREVIEW_FILE + ".png");
        characterData.preview = LoadSprite(basePath + PREVIEW_FILE + ".png");

        GlobalState.AllCharacters.Add(characterData);

        return GlobalState.AllCharacters.Count - 1;
    }


    // Completelly deletes a character folder with all files 
    public static void DeleteCharacterFolder(string characterName)
    {
        string path = Application.dataPath  + CHARACTER_FOLDER + "/" + characterName;
        
        // Delete the meta file
        if (File.Exists(path + ".meta")) {
            File.Delete(path + ".meta");
        }
        
        // Delete the folder
        if (Directory.Exists(path)) {
            Directory.Delete(path, true);
        }

        Debug.Log("Deleted character " + characterName +". In path " + path);
    }


    // Saves a config of a character
    public static void SaveConfigOfCharacter(CharacterData data)
    {
        string basePath = Application.dataPath  + CHARACTER_FOLDER + "/" + data.name;

        if (File.Exists(basePath + CONFIG_FILE))
        {
            File.Delete(basePath + CONFIG_FILE);
        }
       
        FileStream configFile = File.Create(basePath + CONFIG_FILE);
        byte[] configContentBytes = new UTF8Encoding(true).GetBytes(GenerateConfigString(data));
        configFile.Write(configContentBytes, 0, configContentBytes.Length);
        configFile.Close();
    }


    // Loads CharacterData from the character folder using the name of the character
    // When the character is not present, creates a new one by that name
    private static CharacterData LoadFromFolder(string characterFolderPath)
    {
        // Find max index of last index of for \ and /
        int backslashIdx = characterFolderPath.LastIndexOf("\\");
        int frontslashIdx = characterFolderPath.LastIndexOf("/");
        int usedIdx = backslashIdx < frontslashIdx ? frontslashIdx : backslashIdx;

        string characterName = characterFolderPath.Substring(usedIdx + 1);
        CharacterData characterData = new CharacterData{name = characterName};

        // Load config
        string[] configFileLines = File.ReadAllLines(characterFolderPath + CONFIG_FILE);

        // Check for corruption in config file
        if (!TryParseLinesFromConfig(ref configFileLines, ref characterData))
        {
            characterData.isValid = false;
            return characterData;
        }
        characterData.isValid = true;
        
        // Check if animation folders exist
        if (!DoAnimationFoldersExist(characterName))
        {
            characterData.isValid = false;
            return characterData;
        }

        // Load sprites for the animations
        LoadAllAnimations(ref characterData);

        // Load Bubble Icon and Preview
        LoadBubbleIconAndPreview(ref characterData);

        return characterData;
    }


    // Saves a given texture a given full path
    private static void SaveTextureAsPNG(Texture2D _texture, string _fullPath)
    {
        byte[] _bytes =_texture.EncodeToPNG();
        File.WriteAllBytes(_fullPath, _bytes);
    }


    // Creates a default instance of CharacterData
    private static CharacterData GetDefaultCharacterData(string characterName)
    {
        return new CharacterData
        {
            name = characterName,
            speed = 100,
            jump = 20,
            health = 100,
            damage = 20,
            attackPointOffset = new Vector2(0.1f,0.1f),
            attackFrameIdx = 0,
            attackSize = 0.1f,
            colliderHeight = 4,
            colliderWidth = 1,
            colliderOffset = new Vector2(0.1f,0.1f),
        };
    }


    // Generates the config string from CharacterData TODOODDOODODODODOODODODO
    private static string GenerateConfigString(CharacterData data)
    {
        string configString = "";
        configString += CONFIG_SPD_NAME + "=" + data.speed + "\n";
        configString += CONFIG_JMP_NAME + "=" + data.jump + "\n";
        configString += CONFIG_HP_NAME + "=" + data.health + "\n";
        configString += CONFIG_DMG_NAME + "=" + data.damage + "\n";
        configString += CONFIG_ATK_POINT_NAME + "=" + data.attackPointOffset.x
                                             + "|" + data.attackPointOffset.y + "\n";
        configString += CONFIG_ATK_FRAME_NAME + "=" + data.attackFrameIdx + "\n";
        configString += CONFIG_ATK_SIZE_NAME + "=" + data.attackSize + "\n";
        configString += CONFIG_COL_OFFSET_NAME + "=" + data.colliderOffset.x
                                             + "|" + data.colliderOffset.y + "\n";
        configString += CONFIG_COL_WIDTH_NAME + "=" + data.colliderWidth + "\n";
        configString += CONFIG_COL_HEIGHT_NAME + "=" + data.colliderHeight + "\n";
        return configString;
    }


    // Parse data from config file and returns if config was valid TODOODDOODODODODOODODODO
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
                if (int.TryParse(valueSubstring, out int value))
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
                if (int.TryParse(valueSubstring, out int value))
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
                int separatorIdx = line.IndexOf('|');
                string valueSubstringX = line.Substring(CONFIG_ATK_POINT_NAME.Length + 1, separatorIdx - CONFIG_ATK_POINT_NAME.Length - 1);
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

                string valueSubstringY = line.Substring(separatorIdx + 1);
                if (float.TryParse(valueSubstringY, out float valueY))
                {
                    y = valueY;
                }
                else
                {
                    errorString = "Could not parse float value of attack point offset Y from config file! Value found= " + valueSubstringY;
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
                    data.attackSize = value;
                }
                else
                {
                    errorString = "Could not parse float value of attack point size from config file! Value found= " + valueSubstring;
                    break;
                }
            }

            // Parse the collider offset
            else if (line.StartsWith(CONFIG_COL_OFFSET_NAME))
            {
                int separatorIdx = line.IndexOf('|');
                string valueSubstringX = line.Substring(CONFIG_COL_OFFSET_NAME.Length + 1, separatorIdx - CONFIG_COL_OFFSET_NAME.Length - 1);
                float x, y;
                if (float.TryParse(valueSubstringX, out float valueX))
                {
                    x = valueX;
                }
                else
                {
                    errorString = "Could not parse float value of collider offset X from config file! Value found= " + valueSubstringX;
                    break;
                }

                string valueSubstringY = line.Substring(separatorIdx + 1);
                if (float.TryParse(valueSubstringY, out float valueY))
                {
                    y = valueY;
                }
                else
                {
                    errorString = "Could not parse float value of collider offset Y from config file! Value found= " + valueSubstringY;
                    break;
                }

                data.colliderOffset = new Vector2(x, y);
            }

            // Parse collider width
            else if (line.StartsWith(CONFIG_COL_WIDTH_NAME))
            {
                string valueSubstring = line.Substring(CONFIG_COL_WIDTH_NAME.Length + 1);
                if (uint.TryParse(valueSubstring, out uint value))
                {
                    data.colliderWidth = value;
                }
                else
                {
                    errorString = "Could not parse uint value of collider width from config file! Value found= " + valueSubstring;
                    break;
                }
            }
            
            // Parse the collider height
            else if (line.StartsWith(CONFIG_COL_HEIGHT_NAME))
            {
                string valueSubstring = line.Substring(CONFIG_COL_HEIGHT_NAME.Length + 1);
                if (float.TryParse(valueSubstring, out float value))
                {
                    data.colliderHeight = value;
                }
                else
                {
                    errorString = "Could not parse float value of collider height size from config file! Value found= " + valueSubstring;
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


    // Check if all folders for a character exist
    private static bool DoAnimationFoldersExist(string characterName)
    {
        string basePath = Application.dataPath + CHARACTER_FOLDER + "/" + characterName;
        string errorString = "";

        if (!Directory.Exists(basePath + IDLE_ANIM_FOLDER))
        {
            errorString = "Folder for Idle Animation does not exist! " + basePath + IDLE_ANIM_FOLDER;
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

        data.idleAnim = LoadAnimationFromPath(basePath + IDLE_ANIM_FOLDER, IDLE_ANIM_NAME);
        data.attackAnim = LoadAnimationFromPath(basePath + ATTACK_ANIM_FOLDER, ATTACK_ANIM_NAME);
        data.walkAnim  = LoadAnimationFromPath(basePath + WALK_ANIM_FOLDER, WALK_ANIM_NAME);
        data.blockAnim = LoadAnimationFromPath(basePath + BLOCK_ANIM_FOLDER, BLOCK_ANIM_NAME);
        data.jumpAnim = LoadAnimationFromPath(basePath + JUMP_ANIM_FOLDER, JUMP_ANIM_NAME);
        data.deathAnim = LoadAnimationFromPath(basePath + DEATH_ANIM_FOLDER, DEATH_ANIM_NAME);
        data.hurtAnim = LoadAnimationFromPath(basePath + HURT_ANIM_FOLDER, HURT_ANIM_NAME);
    }


    // Loads all sprites (png, jpg, jpeg files) in a folder and creates an AnimationClip from them
    private static FajtovAnimationClip LoadAnimationFromPath(string path, string animName)
    {
        //AnimationClip animationClip = new AnimationClip();
        //animationClip.name = animName;
        //animationClip.frameRate = FRAMERATE;

        FajtovAnimationClip fajtovAnimationClip = new FajtovAnimationClip
        {
            name = animName,
            frames = new Sprite[1]
        };

        // Set variables of animations
        if (animName == IDLE_ANIM_NAME ||
            animName == WALK_ANIM_NAME)
        {
            fajtovAnimationClip.isLooping = true;
            fajtovAnimationClip.canBeInterupted = true;
        }

        // Load all found sprites
        List<Sprite> sprites = new List<Sprite>();
        List<string> allFiles = Directory.GetFiles(path).ToList();

        // Sort all frames by the number at the end
        allFiles.Sort((a,b) => {
            string aFileNameOnly = a.Substring(a.LastIndexOf("\\") + 1).Split('.')[0];
            string bFileNameOnly = b.Substring(b.LastIndexOf("\\") + 1).Split('.')[0];

            if (!int.TryParse(aFileNameOnly.Substring(aFileNameOnly.IndexOfAny("0123456789".ToCharArray())), out int aNum))
            {
                Debug.LogError("Could not parse animation frame number of file: " + a + "\n" + "Animation will likely be incorrect!");
            }

            if (!int.TryParse(bFileNameOnly.Substring(aFileNameOnly.IndexOfAny("0123456789".ToCharArray())), out int bNum))
            {
                Debug.LogError("Could not parse animation frame number of file: " + b + "\n" + "Animation will likely be incorrect!");
            }

            return aNum.CompareTo(bNum);
        });
        
        // Check and add all found sprites
        foreach (string file in allFiles)
        {
            // Check if sprite is valid file type
            bool isFileTypeValid = false;
            foreach (string fileType in SPRITE_FILE_TYPES)
            {
                if (file.EndsWith(fileType))
                {
                    isFileTypeValid = true;
                    break;
                }
            }

            // Add sprite to list
            if (isFileTypeValid) 
            {
                sprites.Add(LoadSprite(file));
            }
            else if (!file.EndsWith(".meta"))
            {
                Debug.LogWarning("Unknown sprite file type found " + file + " in directory " + path);
            }
        }

        fajtovAnimationClip.frames = sprites.ToArray();
        
        // // Create an animation curve of the found sprites
        // EditorCurveBinding spriteBinding = new EditorCurveBinding();
        // spriteBinding.type = typeof(SpriteRenderer);
        // spriteBinding.path = "";
        // spriteBinding.propertyName = "m_Sprite"; 

        // // Setup the animation keys from the sprites
        // ObjectReferenceKeyframe[] spriteKeyFrames = new ObjectReferenceKeyframe[sprites.Count];
        // for(int i = 0; i < (sprites.Count); i++) {
        //     spriteKeyFrames[i] = new ObjectReferenceKeyframe();
        //     spriteKeyFrames[i].time = i * FRAME_DELAY;
        //     spriteKeyFrames[i].value = sprites[i];
        // }
        // AnimationUtility.SetObjectReferenceCurve(animationClip, spriteBinding, spriteKeyFrames);

        return fajtovAnimationClip;
    }


    // Loads the bubble icon and preview icon of the character
    private static void LoadBubbleIconAndPreview(ref CharacterData data)
    {
        string basePath = Application.dataPath + CHARACTER_FOLDER + "/" + data.name;

        // Icon loading checks for all accepted filetypes
        bool iconLoaded = false;
        string iconPath = basePath + BUBBLE_ICON_FILE;
        foreach (string fileType in SPRITE_FILE_TYPES)
        {
            if (File.Exists(iconPath + fileType))
            {
                data.bubbleIcon = LoadSprite(iconPath + fileType);
                iconLoaded = true;
                break;
            }
        }
        if (!iconLoaded)
        {
            data.isValid = false;
            Debug.LogError("Character bubble icon could not be loaded! Must have name \"icon\" and filetype " + string.Join(", ", SPRITE_FILE_TYPES));
        }

        // Preview loading checks for all accepted filetypes
        bool previewLoaded = false;
        string previewPath = basePath + PREVIEW_FILE;
        foreach (string fileType in SPRITE_FILE_TYPES)
        {
            if (File.Exists(previewPath + fileType))
            {
                data.preview = LoadSprite(previewPath + fileType);
                previewLoaded = true;
                break;
            }
        }
        if (!previewLoaded)
        {
            data.isValid = false;
            Debug.LogError("Character preview sprite could not be loaded! Must have name \"preview\" and filetype " + string.Join(", ", SPRITE_FILE_TYPES));
        }
    }


    // Load a single sprite from a file (no integrity checks are done)
    private static Sprite LoadSprite(string path)
    {
        byte[] fileData = File.ReadAllBytes(path);
        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(fileData);
        tex.alphaIsTransparency = true;

        // Case of default icon and preview
        if (tex.width == 16)
        {
            tex.filterMode = FilterMode.Point;
        }
        return Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), tex.width);
    }
}
