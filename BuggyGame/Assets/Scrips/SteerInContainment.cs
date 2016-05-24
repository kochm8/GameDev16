using UnityEngine;
using System.Collections;

namespace UnitySteer.Behaviors
{

    public class SteerInContainment : Steering
    {
        public Collider boundary;
        private static int ctr = 0;
        private int idx = -1;

        protected override Vector3 CalculateForce()
        {
            if (idx == -1)
            {
                idx = ctr++;
            }

            Vector3 target;
            Vector3 center = boundary.transform.position;

            bool is_inside = boundary.bounds.Contains(transform.position);

            target = !is_inside ? center : Vehicle.Position;
            
            target = (Vehicle.Position - target) * -1;
            return target;
        }
    }
}