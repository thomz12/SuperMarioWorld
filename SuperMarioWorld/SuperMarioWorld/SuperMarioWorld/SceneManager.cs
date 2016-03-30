using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperMarioWorld
{
    public class SceneManager
    {
        private static SceneManager instance;

        public static SceneManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new SceneManager();
                return instance;
            }
        }
        
        public SceneManager()
        {

        }

        public void LoadScene()
        {

        }
    }
}
