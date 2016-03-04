using System;

namespace SuperMarioWorld
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (SMWGame game = new SMWGame())
            {
                game.Run();
            }
        }
    }
#endif
}

