// ©2015 - 2024 Candy Smith
// All rights reserved
// Redistribution of this software is strictly not allowed.
// Copy of this software can be obtained from unity asset store only.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NON-INFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using SweetSugar.Scripts.System; // SweetSugar.Scripts.System 네임스페이스에서 시스템 관련 클래스들을 가져옴
using UnityEngine; // UnityEngine 네임스페이스에서 Unity의 기본 클래스들을 가져옴

namespace SweetSugar.Scripts.Core
{
    // 싱글톤 패턴을 사용한 InputHandler 클래스 정의
    public class InputHandler : Singleton<InputHandler>
    {
        private Vector2 mousePos; // 현재 마우스 위치를 저장하는 변수
        private Vector2 _delta; // 마우스 위치 변화량을 저장하는 변수
        private bool down; // 왼쪽 마우스 버튼이 눌렸는지 여부를 저장하는 플래그
        private Camera _camera; // 메인 카메라를 참조하는 변수

        // 마우스 이벤트 델리게이트 선언
        public delegate void MouseEvents(Vector2 pos);

        // 마우스 이벤트가 발생할 때 호출될 수 있는 이벤트들 정의
        public static event MouseEvents OnDown, OnMove, OnUp, OnDownRight;

        // 시작 시 호출되는 메서드
        private void Start()
        {
            _camera = Camera.main; // 메인 카메라를 초기화
        }

        // 매 프레임마다 호출되는 업데이트 메서드
        void Update()
        {
            // 왼쪽 마우스 버튼이 눌렸을 때
            if (Input.GetMouseButtonDown(0))
            {
                MouseDown(GetMouseWorldPos()); // 마우스 다운 이벤트 호출
                down = true; // 마우스 버튼이 눌렸음을 표시
            }

            // 왼쪽 마우스 버튼이 떼어졌을 때
            if (Input.GetMouseButtonUp(0))
            {
                MouseUp(GetMouseWorldPos()); // 마우스 업 이벤트 호출
                down = false; // 마우스 버튼이 떼어졌음을 표시
            }

            // 오른쪽 마우스 버튼이 눌렸을 때
            if (Input.GetMouseButtonDown(1))
                MouseDownRight(GetMouseWorldPos()); // 오른쪽 마우스 다운 이벤트 호출

            // 왼쪽 마우스 버튼이 눌린 상태에서 마우스가 움직일 때
            if (Input.GetMouseButton(0) && down)
            {
                MouseMove(GetMouseWorldPos()); // 마우스 무브 이벤트 호출
            }
        }

        // 화면 좌표를 월드 좌표로 변환하는 메서드
        private Vector3 GetMouseWorldPos()
        {
            return _camera.ScreenToWorldPoint(Input.mousePosition); // 마우스 위치를 월드 좌표로 변환하여 반환
        }

        // 마우스 버튼이 눌렸을 때 호출되는 메서드
        public void MouseDown(Vector2 pos)
        {
            mousePos = pos; // 현재 마우스 위치를 저장
            OnDown?.Invoke(mousePos); // OnDown 이벤트 호출
        }

        // 마우스 버튼이 떼어졌을 때 호출되는 메서드
        public void MouseUp(Vector2 pos)
        {
            mousePos = pos; // 현재 마우스 위치를 저장
            OnUp?.Invoke(mousePos); // OnUp 이벤트 호출
        }

        // 마우스가 움직일 때 호출되는 메서드
        public void MouseMove(Vector2 pos)
        {
            _delta = mousePos - pos; // 이전 마우스 위치와의 차이를 계산하여 저장
            mousePos = pos; // 현재 마우스 위치를 저장
            OnMove?.Invoke(mousePos); // OnMove 이벤트 호출
        }

        // 오른쪽 마우스 버튼이 눌렸을 때 호출되는 메서드
        public void MouseDownRight(Vector2 pos)
        {
            mousePos = pos; // 현재 마우스 위치를 저장
            OnDownRight?.Invoke(mousePos); // OnDownRight 이벤트 호출
        }

        // 현재 마우스 위치를 반환하는 메서드
        public Vector2 GetMousePosition() => mousePos;

        // 마우스 위치 변화량을 반환하는 메서드
        public Vector2 GetMouseDelta() => _delta;
    }
}
