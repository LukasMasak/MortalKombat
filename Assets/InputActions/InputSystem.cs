//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/InputActions/InputSystem.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @InputSystem: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputSystem()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputSystem"",
    ""maps"": [
        {
            ""name"": ""PlayerLeft"",
            ""id"": ""eb79a646-b33e-4b92-9938-c0c8009e9460"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""a55c5fb6-fae8-4f7b-a018-830a6aaf39ce"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""29a84fa4-1161-4095-a0cf-376be205dbbc"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Attack"",
                    ""type"": ""Button"",
                    ""id"": ""80530dc0-12b8-4dc8-8098-f4a97befcd5f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Block"",
                    ""type"": ""Button"",
                    ""id"": ""ff47f8c2-2b40-4c42-9066-c20bf65d3001"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""fefac85b-e377-460d-b744-22d71d8eb5b9"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""dc876a45-c2ff-4def-9726-23f52bb32684"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""185e0530-e64f-4c7d-810f-3764819b0a9f"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""4ef66e69-b1a1-45b9-9f15-10aebd6cb1e3"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""635a0c97-388e-4613-bcf6-cfc358d642a5"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""c4470fd3-1292-4569-9cbb-9a582e63e2b5"",
                    ""path"": ""<Keyboard>/v"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a937d42c-3286-4a8d-9055-cfebcab04a70"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6c732b1d-f1e5-4fc8-bc35-46772dd891ca"",
                    ""path"": ""<Keyboard>/b"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Block"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""PlayerRight"",
            ""id"": ""9ddbdd28-a3fd-4130-911d-63f776cca40f"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""9d218e3b-4582-44f5-b717-b597621c91cd"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""44ee2d01-2056-420d-83b8-5b65ba97f64c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Attack"",
                    ""type"": ""Button"",
                    ""id"": ""d27595f1-1766-44ac-8547-375341ef275a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Block"",
                    ""type"": ""Button"",
                    ""id"": ""254db8be-775c-4429-8a71-6a2fba99d9dc"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""9a73b07b-67ab-4088-8c00-0d3171bbe444"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""d22aa313-7df4-4a8a-9075-041d56bb54a6"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""e89b0247-17e5-497a-93d3-9ff99ab904ee"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""ebdb1eef-a9c6-4cec-b225-4dfde6ec6ba9"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""ad1541ae-a6ea-41f6-876a-d80ffb489845"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""4d4fc5d6-638f-45ff-bdff-75e1ad142096"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""10b0285c-c3f5-4318-aacf-028e919aa374"",
                    ""path"": ""<Keyboard>/k"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6347952e-2ecf-4fd9-b366-b6b64e0d3791"",
                    ""path"": ""<Keyboard>/l"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Block"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // PlayerLeft
        m_PlayerLeft = asset.FindActionMap("PlayerLeft", throwIfNotFound: true);
        m_PlayerLeft_Move = m_PlayerLeft.FindAction("Move", throwIfNotFound: true);
        m_PlayerLeft_Jump = m_PlayerLeft.FindAction("Jump", throwIfNotFound: true);
        m_PlayerLeft_Attack = m_PlayerLeft.FindAction("Attack", throwIfNotFound: true);
        m_PlayerLeft_Block = m_PlayerLeft.FindAction("Block", throwIfNotFound: true);
        // PlayerRight
        m_PlayerRight = asset.FindActionMap("PlayerRight", throwIfNotFound: true);
        m_PlayerRight_Move = m_PlayerRight.FindAction("Move", throwIfNotFound: true);
        m_PlayerRight_Jump = m_PlayerRight.FindAction("Jump", throwIfNotFound: true);
        m_PlayerRight_Attack = m_PlayerRight.FindAction("Attack", throwIfNotFound: true);
        m_PlayerRight_Block = m_PlayerRight.FindAction("Block", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // PlayerLeft
    private readonly InputActionMap m_PlayerLeft;
    private List<IPlayerLeftActions> m_PlayerLeftActionsCallbackInterfaces = new List<IPlayerLeftActions>();
    private readonly InputAction m_PlayerLeft_Move;
    private readonly InputAction m_PlayerLeft_Jump;
    private readonly InputAction m_PlayerLeft_Attack;
    private readonly InputAction m_PlayerLeft_Block;
    public struct PlayerLeftActions
    {
        private @InputSystem m_Wrapper;
        public PlayerLeftActions(@InputSystem wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_PlayerLeft_Move;
        public InputAction @Jump => m_Wrapper.m_PlayerLeft_Jump;
        public InputAction @Attack => m_Wrapper.m_PlayerLeft_Attack;
        public InputAction @Block => m_Wrapper.m_PlayerLeft_Block;
        public InputActionMap Get() { return m_Wrapper.m_PlayerLeft; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerLeftActions set) { return set.Get(); }
        public void AddCallbacks(IPlayerLeftActions instance)
        {
            if (instance == null || m_Wrapper.m_PlayerLeftActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PlayerLeftActionsCallbackInterfaces.Add(instance);
            @Move.started += instance.OnMove;
            @Move.performed += instance.OnMove;
            @Move.canceled += instance.OnMove;
            @Jump.started += instance.OnJump;
            @Jump.performed += instance.OnJump;
            @Jump.canceled += instance.OnJump;
            @Attack.started += instance.OnAttack;
            @Attack.performed += instance.OnAttack;
            @Attack.canceled += instance.OnAttack;
            @Block.started += instance.OnBlock;
            @Block.performed += instance.OnBlock;
            @Block.canceled += instance.OnBlock;
        }

        private void UnregisterCallbacks(IPlayerLeftActions instance)
        {
            @Move.started -= instance.OnMove;
            @Move.performed -= instance.OnMove;
            @Move.canceled -= instance.OnMove;
            @Jump.started -= instance.OnJump;
            @Jump.performed -= instance.OnJump;
            @Jump.canceled -= instance.OnJump;
            @Attack.started -= instance.OnAttack;
            @Attack.performed -= instance.OnAttack;
            @Attack.canceled -= instance.OnAttack;
            @Block.started -= instance.OnBlock;
            @Block.performed -= instance.OnBlock;
            @Block.canceled -= instance.OnBlock;
        }

        public void RemoveCallbacks(IPlayerLeftActions instance)
        {
            if (m_Wrapper.m_PlayerLeftActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPlayerLeftActions instance)
        {
            foreach (var item in m_Wrapper.m_PlayerLeftActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PlayerLeftActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PlayerLeftActions @PlayerLeft => new PlayerLeftActions(this);

    // PlayerRight
    private readonly InputActionMap m_PlayerRight;
    private List<IPlayerRightActions> m_PlayerRightActionsCallbackInterfaces = new List<IPlayerRightActions>();
    private readonly InputAction m_PlayerRight_Move;
    private readonly InputAction m_PlayerRight_Jump;
    private readonly InputAction m_PlayerRight_Attack;
    private readonly InputAction m_PlayerRight_Block;
    public struct PlayerRightActions
    {
        private @InputSystem m_Wrapper;
        public PlayerRightActions(@InputSystem wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_PlayerRight_Move;
        public InputAction @Jump => m_Wrapper.m_PlayerRight_Jump;
        public InputAction @Attack => m_Wrapper.m_PlayerRight_Attack;
        public InputAction @Block => m_Wrapper.m_PlayerRight_Block;
        public InputActionMap Get() { return m_Wrapper.m_PlayerRight; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerRightActions set) { return set.Get(); }
        public void AddCallbacks(IPlayerRightActions instance)
        {
            if (instance == null || m_Wrapper.m_PlayerRightActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PlayerRightActionsCallbackInterfaces.Add(instance);
            @Move.started += instance.OnMove;
            @Move.performed += instance.OnMove;
            @Move.canceled += instance.OnMove;
            @Jump.started += instance.OnJump;
            @Jump.performed += instance.OnJump;
            @Jump.canceled += instance.OnJump;
            @Attack.started += instance.OnAttack;
            @Attack.performed += instance.OnAttack;
            @Attack.canceled += instance.OnAttack;
            @Block.started += instance.OnBlock;
            @Block.performed += instance.OnBlock;
            @Block.canceled += instance.OnBlock;
        }

        private void UnregisterCallbacks(IPlayerRightActions instance)
        {
            @Move.started -= instance.OnMove;
            @Move.performed -= instance.OnMove;
            @Move.canceled -= instance.OnMove;
            @Jump.started -= instance.OnJump;
            @Jump.performed -= instance.OnJump;
            @Jump.canceled -= instance.OnJump;
            @Attack.started -= instance.OnAttack;
            @Attack.performed -= instance.OnAttack;
            @Attack.canceled -= instance.OnAttack;
            @Block.started -= instance.OnBlock;
            @Block.performed -= instance.OnBlock;
            @Block.canceled -= instance.OnBlock;
        }

        public void RemoveCallbacks(IPlayerRightActions instance)
        {
            if (m_Wrapper.m_PlayerRightActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPlayerRightActions instance)
        {
            foreach (var item in m_Wrapper.m_PlayerRightActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PlayerRightActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PlayerRightActions @PlayerRight => new PlayerRightActions(this);
    public interface IPlayerLeftActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnAttack(InputAction.CallbackContext context);
        void OnBlock(InputAction.CallbackContext context);
    }
    public interface IPlayerRightActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnAttack(InputAction.CallbackContext context);
        void OnBlock(InputAction.CallbackContext context);
    }
}
