using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Restrictions")]
    public float minX = -10f;
    public float maxX = 10f;
    public float minZ = -10f;
    public float maxZ = 10f;

    [Header("Movement")]
    public float moveSpeed = 5f;
    public float borderPercent = 0.2f;

    private float _screenWidth;
    private float _screenHeight;

    void Start()
    {
        _screenWidth = Screen.width;
        _screenHeight = Screen.height;
    }

    void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        float mouseX = Input.mousePosition.x;
        float mouseY = Input.mousePosition.y;
        Vector3 moveDir = Vector3.zero;

        // Горизонтальное движение (влево/вправо по экрану → вправо/влево по карте)
        if (mouseX < _screenWidth * borderPercent)
        {
            float t = 1f - (mouseX / (_screenWidth * borderPercent));
            moveDir -= transform.right * t;
        }
        else if (mouseX > _screenWidth * (1f - borderPercent))
        {
            float t = (mouseX - _screenWidth * (1f - borderPercent)) / (_screenWidth * borderPercent);
            moveDir += transform.right * t;
        }

        // Вертикальное движение (вверх/вниз по экрану → вперёд/назад по карте)
        if (mouseY < _screenHeight * borderPercent)
        {
            float t = 1f - (mouseY / (_screenHeight * borderPercent));
            moveDir -= new Vector3(transform.forward.x, 0, transform.forward.z).normalized * t;
        }
        else if (mouseY > _screenHeight * (1f - borderPercent))
        {
            float t = (mouseY - _screenHeight * (1f - borderPercent)) / (_screenHeight * borderPercent);
            moveDir += new Vector3(transform.forward.x, 0, transform.forward.z).normalized * t;
        }

        // Двигаем камеру
        if (moveDir != Vector3.zero)
        {
            Vector3 pos = transform.position;
            pos += moveDir * moveSpeed * Time.deltaTime;

            // Ограничение по XZ плоскости
            pos.x = Mathf.Clamp(pos.x, minX, maxX);
            pos.z = Mathf.Clamp(pos.z, minZ, maxZ);

            transform.position = pos;
        }
    }
}
