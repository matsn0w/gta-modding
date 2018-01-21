using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rage;

[assembly: Rage.Attributes.Plugin("My First LSPDFR Plugin", Description = "This is my first plugin. yay!", Author = "matsn0w")]

namespace My_first_GTA_plugin
{
    public static class EntryPoint
    {
        public static void Main()
        {
            GameFiber.StartNew(delegate
            {
                Game.DisplayHelp("Hello Bart, how are you doing today?");
                Game.DisplayNotification("Hello to the TeamMOH Discord!!!");
            });
        }
    }
}
