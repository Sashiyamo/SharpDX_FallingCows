using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Input;
using Engine;
using Engine.BaseAssets.Components;
using Engine.BaseAssets.Components.Colliders;
using LinearAlgebra;

namespace Test.Content.Scripts
{
    internal class CowController : BehaviourComponent
    {
        [SerializedField] private bool isCowGod = false;
        private Random random = new Random();
        public int lived_cows = 0;
        int cows = 1;
        double spawn_time = 4.0;
        private MainUI main_ui = null;
        private GameObject cow_god = null;

        public override void Start()
        {
            if(isCowGod)
            {
                isCowGod = false;

                GraphicsCore.ViewportPanel.Dispatcher.Invoke(() =>
                {
                    main_ui = new MainUI();
                    GraphicsCore.ViewportPanel.Children.Add(main_ui);
                });

                Coroutine.Start(SpawnCow);

            } else Coroutine.Start(DestoyCow);

            GameObject.GetComponent<Collider>().OnTriggerEnter += Gameplay_OnTriggerEnter;
        }

        public MainUI GetUI()
        {
            return main_ui;
        }

        private void Gameplay_OnTriggerEnter(Collider sender, Collider other)
        {
            if (other.GameObject.Name == "Player")
            {
                Logger.Log(LogType.Info, $"cow death");
                cow_god.GetComponent<CowController>().lived_cows = 0;

                GraphicsCore.ViewportPanel.Dispatcher.Invoke(() =>
                {
                    main_ui.Cows.Text = "0";
                });
            }
            if (other.GameObject.Name == "Death")
            {
                if (GameObject != null) GameObject.Destroy();
            }
        }

        public override void Update()
        {


        }

        private IEnumerator SpawnCow() 
        {
            for (int i = 0; i < 1000; i++)
            {

                if (lived_cows >= 0 && lived_cows < 20)
                {
                    cows = random.Next(1, 2);
                    spawn_time = 4.0;

                } else if (lived_cows >= 20 && lived_cows < 40)
                {
                    cows = random.Next(1, 3);
                    spawn_time = 3.0;

                } else if (lived_cows >= 40 && lived_cows < 100)
                {
                    cows = random.Next(2, 3);
                    spawn_time = 3.0;

                } else if (lived_cows >= 100)
                {
                    cows = random.Next(2, 4);
                    spawn_time = 2.0;
                }


                for (int c = 0; c < cows; c++)
                {
                    Logger.Log(LogType.Info, $"cow: " + lived_cows.ToString());
                    lived_cows++;

                    GraphicsCore.ViewportPanel.Dispatcher.Invoke(() =>
                    {
                        main_ui.Cows.Text = lived_cows.ToString();
                    });

                    var size_rand = random.NextDouble() + 0.4;

                    var new_cow = GameObject.Duplicate();
                    new_cow.Name = "Cow";

                    var cow_scale = new_cow.Transform.LocalScale;
                    new_cow.Transform.LocalScale = new Vector3(cow_scale.x * size_rand, cow_scale.y * size_rand, cow_scale.z * size_rand);

                    var cow_col_scale = new_cow.GetComponent<CubeCollider>().Size;
                    new_cow.GetComponent<CubeCollider>().Size = new Vector3(cow_col_scale.x * size_rand, cow_col_scale.y * size_rand, cow_col_scale.z * size_rand);

                    new_cow.Transform.Position = new Vector3(random.NextDouble() * 18.0 - 10.0, random.NextDouble() * 18.0 - 10.0, 40);
                    new_cow.Transform.Rotation = new Quaternion(random.NextDouble(), random.NextDouble(), random.NextDouble(), random.NextDouble());

                    new_cow.GetComponent<CowController>().main_ui = GameObject.GetComponent<CowController>().GetUI();
                    new_cow.GetComponent<CowController>().cow_god = GameObject;
                }
                

                yield return new WaitForSeconds(spawn_time);
            }
        }

        private IEnumerator DestoyCow()
        {
            yield return new WaitForSeconds(20.0);
            if (GameObject != null) GameObject.Destroy();
        }
    }
}
