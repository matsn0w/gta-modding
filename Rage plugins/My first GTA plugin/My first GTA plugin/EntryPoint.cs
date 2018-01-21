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
                // Initialize plugin
                Game.DisplayHelp("Hit the G key to start the assasination.");
                Game.DisplayNotification("matsn0w Modding plugin successfully loaded");
                Game.LogTrivial("matsn0w Modding plugin has loaded successfully.");

                // User hits button and a spawned ped drives to the player to kill him.
                while (true)
                {
                    // Wait for the user to hit the G key
                    GameFiber.Yield();
                    if (Game.IsKeyDown(System.Windows.Forms.Keys.G))
                    {
                        // Stop waiting, launch the event
                        break;
                    }

                    Game.LogTrivial("G key pressed!");
                    Game.HideHelp();

                    // Create new ped (the attacker) which is a SWAT guy, 10m in front of the player, heading towards the player
                    Ped attacker_ped = new Ped("s_m_y_swat_01", Game.LocalPlayer.Character.GetOffsetPositionFront(10f), Game.LocalPlayer.Character.Heading + 180f);

                    // Create new vehicle (for the attacker) which is a mesa, 15m in front of the player, heading towards the player
                    Vehicle attacker_veh = new Vehicle("mesa", Game.LocalPlayer.Character.GetOffsetPositionFront(15f), Game.LocalPlayer.Character.Heading + 180f);

                    // Prevent the attacker from being deleted by GTA
                    attacker_ped.BlockPermanentEvents = true;
                    attacker_ped.IsPersistent = true;

                    // Prevent the attacker's vehicle from being deleted by GTa
                    attacker_veh.IsPersistent = true;

                    // Make the attacker get into the vehicle
                    attacker_ped.Tasks.EnterVehicle(attacker_veh, 10000, -1).WaitForCompletion(10000);

                    // Make the attacker drive towards the player
                    attacker_ped.Tasks.DriveToPosition(Game.LocalPlayer.Character.Position, 15f, VehicleDrivingFlags.Emergency).WaitForCompletion(10000);

                    // Make the attacker get out of the vehicle
                    attacker_ped.Tasks.LeaveVehicle(LeaveVehicleFlags.None).WaitForCompletion(6000);

                    // Give the attacker a pistol
                    attacker_ped.Inventory.GiveNewWeapon("WEAPON_PISTOL", -1, true);

                    // Make the attacker fight the player
                    attacker_ped.Tasks.FightAgainst(Game.LocalPlayer.Character);

                    Game.LogTrivial("Let the game begin!");

                    // Check if either the player or the attacker is dead
                    while (true)
                    {
                        GameFiber.Yield();

                        if (Game.LocalPlayer.Character.IsDead || attacker_ped.IsDead)
                        {
                            break;
                        }

                        Game.LogTrivial("Someone's dead!");

                        // Clean up the game
                        if (attacker_ped.Exists())
                        {
                            attacker_ped.Dismiss();
                        }

                        // Stop the GameFiber
                        GameFiber.Hibernate();

                        Game.LogTrivial("Bye!");
                    }
                }
            });
        }
    }
}
