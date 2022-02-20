using UnityEngine;

namespace Scripts.Creatures
{
    public interface IMovable
    {
        void MoveTo(Vector3 dest);
    }
}