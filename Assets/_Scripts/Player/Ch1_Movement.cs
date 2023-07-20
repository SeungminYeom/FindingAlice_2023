using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// ===================================================================================================
// 챕터1 Player에 Attach되는 스크립트
//
// Movement 클래스를 상속받는다.
// ===================================================================================================

public class Ch1_Movement : Movement
{
    // ===============================================================================================
    // 점프 입력을 했을 때, 키에 의한 점프이지 않고, 점프할 수 있는 상태라면
    // JumpInput 함수 호출
    // ===============================================================================================
    private void Update()
    {
        // 유니티 에디터에선 wasd(방향키)와 space 입력으로 이동/점프
#if UNITY_EDITOR
        xAxis = Input.GetAxisRaw("Horizontal");
        if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpInput();
        }

        // 안드로이드 빌드 상태에서는 조이스틱 입력값으로 이동
#elif UNITY_ANDROID
        xAxis = joystick.Horizontal;
#endif
    }

    private void FixedUpdate()
    {
        base.Move();
        base.Jump();
    }
}
