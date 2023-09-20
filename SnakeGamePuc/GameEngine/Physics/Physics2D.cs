using GameEngine.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Physics
{
    public class Physics2D
    {
        //TO-DO use real rects overlaps
        public void CalculateCollisions(Collider[] _colliders)
        {
            for (int i = 0; i < _colliders.Length; i++)
            {
                for (int j = i + 1; j < _colliders.Length; j++)
                {
                    if (_colliders[i].OverlapsOther(_colliders[j]))
                    {
                        _colliders[i].CallCollisionsCB(_colliders[j]);
                        _colliders[j].CallCollisionsCB(_colliders[i]);
                    }
                }
            }
        }
    }
}
