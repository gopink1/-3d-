using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools;
using UnityEngine.InputSystem;
using System;
/// <summary>
/// 角色输入系统管理器
/// </summary>
public class GameInputManager : Singleton<GameInputManager>
{
    private GameInputAction input;
    // 存储每个按钮的状态
    private Dictionary<string, ButtonState> buttonStates = new Dictionary<string, ButtonState>();

    // 事件定义
    public event Action<string> OnButtonPressed;       // 按钮按下
    public event Action<string> OnButtonReleased;      // 按钮释放(仅触发一次)

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
    // 改进的 SkillQRelease 实现（只在释放瞬间返回一次 true）
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
        // 注册所有需要检测释放的按钮
        RegisterButton("SkillQ", input.GameInput.SKill0);
        RegisterButton("SkillE", input.GameInput.SKill1);
    }

    private void Update()
    {
        // 重置所有按钮的 "释放于当前帧" 标记
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
        // 初始化按钮状态
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
    // 检查某个按钮是否在当前帧释放
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
        // 等待当前帧的Update完成
        yield return null;

        if (buttonStates.TryGetValue(buttonName, out var state))
        {
            // 标记释放发生在下一帧开始前
            state.WasReleasedThisFrame = true;
            OnButtonReleased?.Invoke(buttonName);

            // 在下一帧的Update之后重置标记
            yield return null;
            state.WasReleasedThisFrame = false;
        }
    }

    // 按钮状态类
    private class ButtonState
    {
        public bool IsPressed { get; set; }
        public bool WasReleasedThisFrame { get; set; }
    }

}
