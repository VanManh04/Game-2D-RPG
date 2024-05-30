using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// PlayerStateMachine quản lý các trạng thái của người chơi
public class PlayerStateMachine : MonoBehaviour
{
    // Biến currentState lưu trữ trạng thái hiện tại của người chơi và chỉ có thể thiết lập nội bộ
    public PlayerState currentState {  get; private set; }

    // Phương thức khởi tạo để thiết lập trạng thái bắt đầu của người chơi
    public void Initialize(PlayerState _startState)
    {
        currentState = _startState; //Thiết lập trạng thái ban đầu
        currentState.Enter(); //Gọi phương thức Enter của trạng thái bắt đầu
    }

    // Phương thức thay đổi trạng thái của người chơi
    public void ChangeState(PlayerState _newState)
    {
        currentState.Exit(); // Gọi phương thức Exit của trạng thái hiện tại
        currentState = _newState; // Cập nhật trạng thái mới
        currentState.Enter(); // Gọi phương thức Enter của trạng thái mới
    }
}
