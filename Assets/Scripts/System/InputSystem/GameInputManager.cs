using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools;
using UnityEngine.InputSystem;
using System;
/// <summary>
/// ��ɫ����ϵͳ������
/// </summary>
public class GameInputManager : Singleton<GameInputManager>
{
    private GameInputAction input;
    // �洢ÿ����ť��״̬
    private Dictionary<string, ButtonState> buttonStates = new Dictionary<string, ButtonState>();

    // �¼�����
    public event Action<string> OnButtonPressed;       // ��ť����
    public event Action<string> OnButtonReleased;      // ��ť�ͷ�(������һ��)

    public Vector2 Movement
    {
        get => input.GameInput.Movement.ReadValue<Vector2>();
    }
    public Vector2 CameraView
    {
        get => input.GameInput.CameraView.ReadValue<Vector2>();
    }

    public bool Run
    {
        get => input.GameInput.Run.phase == UnityEngine.InputSystem.InputActionPhase.Performed;
    }
    public bool Dodge
    {
        get => input.GameInput.Dodge.triggered;
    }
    public bool Action
    {
        get => input.GameInput.Interact.triggered;
    }
    public bool Latk
    {
        get => input.GameInput.Latk.triggered;
    }
    public bool Ratk
    {
        get => input.GameInput.Ratk.triggered;
    }

    public bool LockView
    {
        get => input.GameInput.LockView.triggered;
    }
    public bool SkillQ
    {
        get => input.GameInput.SKill0.triggered;
    }
    // �Ľ��� SkillQRelease ʵ�֣�ֻ���ͷ�˲�䷵��һ�� true��
    public bool SkillQRelease => GetButtonReleasedThisFrame("SkillQ");

    public bool SkillE
    {
        get => input.GameInput.SKill1.triggered;
    }
    public bool SkillERelease => GetButtonReleasedThisFrame("SkillE");
    protected override void Awake()
    {
        base.Awake();
        if (input == null)
        {
            input = new GameInputAction();
        }
    }
    private void OnEnable()
    {
        input.Enable();
        // ע��������Ҫ����ͷŵİ�ť
        RegisterButton("SkillQ", input.GameInput.SKill0);
        RegisterButton("SkillE", input.GameInput.SKill1);
    }

    private void Update()
    {
        // �������а�ť�� "�ͷ��ڵ�ǰ֡" ���
        foreach (var state in buttonStates.Values)
        {
            state.WasReleasedThisFrame = false;
        }
    }
    private void OnDisable()
    {
        input.Disable();
    }


    private void RegisterButton(string buttonName, InputAction action)
    {
        // ��ʼ����ť״̬
        buttonStates[buttonName] = new ButtonState();

        action.performed += ctx => OnButtonPerformed(buttonName);
        action.canceled += ctx => OnButtonCanceled(buttonName);
    }

    private void OnButtonPerformed(string buttonName)
    {
        if (buttonStates.TryGetValue(buttonName, out var state))
        {
            state.IsPressed = true;
            state.WasReleasedThisFrame = false;
            OnButtonPressed?.Invoke(buttonName);
        }
    }

    private void OnButtonCanceled(string buttonName)
    {
        if (buttonStates.TryGetValue(buttonName, out var state))
        {
            state.IsPressed = false;
            StartCoroutine(ProcessRelease(buttonName));
        }
    }    
    // ���ĳ����ť�Ƿ��ڵ�ǰ֡�ͷ�
    public bool GetButtonReleasedThisFrame(string buttonName)
    {
        if (buttonStates.TryGetValue(buttonName, out var state))
        {
            //Debug.Log(state.WasReleasedThisFrame);
            return state.WasReleasedThisFrame;
        }
        return false;
    }
    private IEnumerator ProcessRelease(string buttonName)
    {
        // �ȴ���ǰ֡��Update���
        yield return null;

        if (buttonStates.TryGetValue(buttonName, out var state))
        {
            // ����ͷŷ�������һ֡��ʼǰ
            state.WasReleasedThisFrame = true;
            OnButtonReleased?.Invoke(buttonName);

            // ����һ֡��Update֮�����ñ��
            yield return null;
            state.WasReleasedThisFrame = false;
        }
    }

    // ��ť״̬��
    private class ButtonState
    {
        public bool IsPressed { get; set; }
        public bool WasReleasedThisFrame { get; set; }
    }

}
