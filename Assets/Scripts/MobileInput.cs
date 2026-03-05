using UnityEngine;

public class MobileInput : MonoBehaviour
{
    public static float horizontal; // -1 kiri, 0 diam, 1 kanan
    public static bool jump;
    public static bool attack;

    // KIRI
    public void LeftDown()
    {
        horizontal = -1;
    }

    public void LeftUp()
    {
        if (horizontal == -1)
            horizontal = 0;
    }

    // KANAN
    public void RightDown()
    {
        horizontal = 1;
    }

    public void RightUp()
    {
        if (horizontal == 1)
            horizontal = 0;
    }

    // JUMP
    public void Jump()
    {
        jump = true;
    }

    public void Attack()
    {
        attack = true;
    }
}
