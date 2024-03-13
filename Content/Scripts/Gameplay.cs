using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Design;
using System.Windows.Input;
using Engine;
using Engine.BaseAssets.Components;
using LinearAlgebra;

namespace Test.Content.Scripts
{
    public class Gameplay : BehaviourComponent
    {
        [SerializedField] 
        public double moveSpeed = 0.01;

        public override void Start()
        {
            GameObject.GetComponent<Collider>().OnTriggerEnter += Gameplay_OnTriggerEnter;
        }

        private void Gameplay_OnTriggerEnter(Collider sender, Collider other)
        {
            if (other.GameObject.Name == "Death")
            {
                Logger.Log(LogType.Info, $"fall death");
                GameObject.Transform.Position = new Vector3(0, 0, 2);
                GameObject.GetComponent<Rigidbody>().AddImpulse(new Vector3(0, 0, -7));
            }


        }

        public override void Update()
        {
            var realVector = GameObject.Transform.Position;
            if (Input.IsKeyDown(Key.D))
            {
                realVector = new Vector3(realVector.x + moveSpeed, realVector.y, realVector.z);
            }
            if (Input.IsKeyDown(Key.A))
            {
                realVector = new Vector3(realVector.x - moveSpeed, realVector.y, realVector.z);
            }
            if (Input.IsKeyDown(Key.W))
            {
                realVector = new Vector3(realVector.x, realVector.y + moveSpeed, realVector.z);
            }
            if (Input.IsKeyDown(Key.S))
            {
                realVector = new Vector3(realVector.x, realVector.y - moveSpeed, realVector.z);
            }

            GameObject.Transform.Position = realVector;
        }
    }
}
