// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/Controls/PlayerControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""Combat"",
            ""id"": ""56ae7a05-951a-4f0d-873d-15e807ae7faa"",
            ""actions"": [
                {
                    ""name"": ""LightAttack"",
                    ""type"": ""Button"",
                    ""id"": ""bd86aedf-294f-4851-8c8d-0e2b9697e57c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MediumAttack"",
                    ""type"": ""Button"",
                    ""id"": ""6bf7fcf3-898f-4b64-a759-1e41311635da"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""StrongAttack"",
                    ""type"": ""Button"",
                    ""id"": ""168bda4a-89d5-4ac4-9f0e-2bf79907c755"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""CancelAttack"",
                    ""type"": ""Button"",
                    ""id"": ""d51d3945-3075-4d02-b1c1-e7f9d6028b33"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""1be13354-2b40-4746-b2db-5a339b1a99c3"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LightAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cbea58d9-1c17-41f3-bd02-f4a37948ac16"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MediumAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0bf3c46c-e6eb-42ba-8e5a-e4416f05fbe1"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""StrongAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""098b59ca-8449-47e5-914e-9ea7425cfd18"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CancelAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""CombatUI"",
            ""id"": ""2d8836af-7662-463c-8d34-22ea97e38abb"",
            ""actions"": [
                {
                    ""name"": ""Navigate"",
                    ""type"": ""Value"",
                    ""id"": ""31fa9c89-589f-4d37-b7ce-d86b7c587498"",
                    ""expectedControlType"": ""Dpad"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Submit"",
                    ""type"": ""Button"",
                    ""id"": ""a8125c24-ef61-4869-893a-9d539bc77cd3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Cancel"",
                    ""type"": ""Button"",
                    ""id"": ""2d06f918-b408-4af1-a680-e95a5fcf56fc"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Direction"",
                    ""id"": ""4d5fcfd8-9a62-413b-8204-91b78344c024"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""0e2a96cd-0907-42bd-a7d3-0dc50e1606a0"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""a9197152-2cfc-46b6-9b24-abccad640894"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""d81a1935-e9bc-42cd-9151-7596783b725f"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""b174c9ca-0cab-459e-b938-f387881f609d"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""c84712dd-0529-4bd2-8493-c218ea324672"",
                    ""path"": ""<Gamepad>/dpad"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3987ae99-89d0-471a-882b-9fe44b2708b7"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Submit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b9299cff-706c-4cf8-b26a-60f3f2f0157f"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Combat
        m_Combat = asset.FindActionMap("Combat", throwIfNotFound: true);
        m_Combat_LightAttack = m_Combat.FindAction("LightAttack", throwIfNotFound: true);
        m_Combat_MediumAttack = m_Combat.FindAction("MediumAttack", throwIfNotFound: true);
        m_Combat_StrongAttack = m_Combat.FindAction("StrongAttack", throwIfNotFound: true);
        m_Combat_CancelAttack = m_Combat.FindAction("CancelAttack", throwIfNotFound: true);
        // CombatUI
        m_CombatUI = asset.FindActionMap("CombatUI", throwIfNotFound: true);
        m_CombatUI_Navigate = m_CombatUI.FindAction("Navigate", throwIfNotFound: true);
        m_CombatUI_Submit = m_CombatUI.FindAction("Submit", throwIfNotFound: true);
        m_CombatUI_Cancel = m_CombatUI.FindAction("Cancel", throwIfNotFound: true);
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

    // Combat
    private readonly InputActionMap m_Combat;
    private ICombatActions m_CombatActionsCallbackInterface;
    private readonly InputAction m_Combat_LightAttack;
    private readonly InputAction m_Combat_MediumAttack;
    private readonly InputAction m_Combat_StrongAttack;
    private readonly InputAction m_Combat_CancelAttack;
    public struct CombatActions
    {
        private @PlayerControls m_Wrapper;
        public CombatActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @LightAttack => m_Wrapper.m_Combat_LightAttack;
        public InputAction @MediumAttack => m_Wrapper.m_Combat_MediumAttack;
        public InputAction @StrongAttack => m_Wrapper.m_Combat_StrongAttack;
        public InputAction @CancelAttack => m_Wrapper.m_Combat_CancelAttack;
        public InputActionMap Get() { return m_Wrapper.m_Combat; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CombatActions set) { return set.Get(); }
        public void SetCallbacks(ICombatActions instance)
        {
            if (m_Wrapper.m_CombatActionsCallbackInterface != null)
            {
                @LightAttack.started -= m_Wrapper.m_CombatActionsCallbackInterface.OnLightAttack;
                @LightAttack.performed -= m_Wrapper.m_CombatActionsCallbackInterface.OnLightAttack;
                @LightAttack.canceled -= m_Wrapper.m_CombatActionsCallbackInterface.OnLightAttack;
                @MediumAttack.started -= m_Wrapper.m_CombatActionsCallbackInterface.OnMediumAttack;
                @MediumAttack.performed -= m_Wrapper.m_CombatActionsCallbackInterface.OnMediumAttack;
                @MediumAttack.canceled -= m_Wrapper.m_CombatActionsCallbackInterface.OnMediumAttack;
                @StrongAttack.started -= m_Wrapper.m_CombatActionsCallbackInterface.OnStrongAttack;
                @StrongAttack.performed -= m_Wrapper.m_CombatActionsCallbackInterface.OnStrongAttack;
                @StrongAttack.canceled -= m_Wrapper.m_CombatActionsCallbackInterface.OnStrongAttack;
                @CancelAttack.started -= m_Wrapper.m_CombatActionsCallbackInterface.OnCancelAttack;
                @CancelAttack.performed -= m_Wrapper.m_CombatActionsCallbackInterface.OnCancelAttack;
                @CancelAttack.canceled -= m_Wrapper.m_CombatActionsCallbackInterface.OnCancelAttack;
            }
            m_Wrapper.m_CombatActionsCallbackInterface = instance;
            if (instance != null)
            {
                @LightAttack.started += instance.OnLightAttack;
                @LightAttack.performed += instance.OnLightAttack;
                @LightAttack.canceled += instance.OnLightAttack;
                @MediumAttack.started += instance.OnMediumAttack;
                @MediumAttack.performed += instance.OnMediumAttack;
                @MediumAttack.canceled += instance.OnMediumAttack;
                @StrongAttack.started += instance.OnStrongAttack;
                @StrongAttack.performed += instance.OnStrongAttack;
                @StrongAttack.canceled += instance.OnStrongAttack;
                @CancelAttack.started += instance.OnCancelAttack;
                @CancelAttack.performed += instance.OnCancelAttack;
                @CancelAttack.canceled += instance.OnCancelAttack;
            }
        }
    }
    public CombatActions @Combat => new CombatActions(this);

    // CombatUI
    private readonly InputActionMap m_CombatUI;
    private ICombatUIActions m_CombatUIActionsCallbackInterface;
    private readonly InputAction m_CombatUI_Navigate;
    private readonly InputAction m_CombatUI_Submit;
    private readonly InputAction m_CombatUI_Cancel;
    public struct CombatUIActions
    {
        private @PlayerControls m_Wrapper;
        public CombatUIActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Navigate => m_Wrapper.m_CombatUI_Navigate;
        public InputAction @Submit => m_Wrapper.m_CombatUI_Submit;
        public InputAction @Cancel => m_Wrapper.m_CombatUI_Cancel;
        public InputActionMap Get() { return m_Wrapper.m_CombatUI; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CombatUIActions set) { return set.Get(); }
        public void SetCallbacks(ICombatUIActions instance)
        {
            if (m_Wrapper.m_CombatUIActionsCallbackInterface != null)
            {
                @Navigate.started -= m_Wrapper.m_CombatUIActionsCallbackInterface.OnNavigate;
                @Navigate.performed -= m_Wrapper.m_CombatUIActionsCallbackInterface.OnNavigate;
                @Navigate.canceled -= m_Wrapper.m_CombatUIActionsCallbackInterface.OnNavigate;
                @Submit.started -= m_Wrapper.m_CombatUIActionsCallbackInterface.OnSubmit;
                @Submit.performed -= m_Wrapper.m_CombatUIActionsCallbackInterface.OnSubmit;
                @Submit.canceled -= m_Wrapper.m_CombatUIActionsCallbackInterface.OnSubmit;
                @Cancel.started -= m_Wrapper.m_CombatUIActionsCallbackInterface.OnCancel;
                @Cancel.performed -= m_Wrapper.m_CombatUIActionsCallbackInterface.OnCancel;
                @Cancel.canceled -= m_Wrapper.m_CombatUIActionsCallbackInterface.OnCancel;
            }
            m_Wrapper.m_CombatUIActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Navigate.started += instance.OnNavigate;
                @Navigate.performed += instance.OnNavigate;
                @Navigate.canceled += instance.OnNavigate;
                @Submit.started += instance.OnSubmit;
                @Submit.performed += instance.OnSubmit;
                @Submit.canceled += instance.OnSubmit;
                @Cancel.started += instance.OnCancel;
                @Cancel.performed += instance.OnCancel;
                @Cancel.canceled += instance.OnCancel;
            }
        }
    }
    public CombatUIActions @CombatUI => new CombatUIActions(this);
    public interface ICombatActions
    {
        void OnLightAttack(InputAction.CallbackContext context);
        void OnMediumAttack(InputAction.CallbackContext context);
        void OnStrongAttack(InputAction.CallbackContext context);
        void OnCancelAttack(InputAction.CallbackContext context);
    }
    public interface ICombatUIActions
    {
        void OnNavigate(InputAction.CallbackContext context);
        void OnSubmit(InputAction.CallbackContext context);
        void OnCancel(InputAction.CallbackContext context);
    }
}
