using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CommandSystem;
using LabApi.Features.Console;
using LabApi.Features.Wrappers;
using PlayerRoles;
using MEC;

namespace Swapped.Commands;

[CommandHandler(typeof(ClientCommandHandler))]
// ReSharper disable once UnusedType.Global
public class Swap : ICommand
{
    public string Command => "swap";
    public string[] Aliases => [];
    public string Description => "Swap to a different SCP role!";

    public async bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (!Plugin.Instance.SwapEnabled)
        {
            response = "Du kannst nur in den ersten 40 Sekunden der Runde dein Rolle wechseln!";
            return false;
        }

        if (sender == null)
        {
            response = "You must be a player to use this command.";
            return false;
        }

        Player player = Player.Get(sender);

        if (player == null)
        {
            response = "Player not found.";
            return false;
        }

        if (arguments.Array == null)
        {
            response =
                "Willst du mich verarschen? Schreibe z.B.: \".swap 173\" um deine Rolle zu wechseln. Bist du blöd?";
            return false;
        }

        if (!Plugin.Instance.PlayersThatCanUseSwap.Contains(player))
        {
            response = "Du bist kein SCP! lol vollidiot...";
            return false;
        }

        bool success = false;
        if (arguments.Array[1].Contains("173"))
        {
            success = SwapPlayerToScp(player, RoleTypeId.Scp173);
            response = success ? "You will swap to 173" : "You don't have enough ZVC to do that!";
        }
        else if (arguments.Array[1].Contains("939"))
        {
            success = SwapPlayerToScp(player, RoleTypeId.Scp939);
            response = success ? "You will swap to 939" : "You don't have enough ZVC to do that!";
        }
        else if (arguments.Array[1].Contains("079"))
        {
            success = SwapPlayerToScp(player, RoleTypeId.Scp079);
            response = success ? "You will swap to 079" : "You don't have enough ZVC to do that!";
        }
        else if (arguments.Array[1].Contains("049"))
        {
            success = SwapPlayerToScp(player, RoleTypeId.Scp049);
            response = success ? "You will swap to 049" : "You don't have enough ZVC to do that!";
        }
        else if (arguments.Array[1].Contains("096"))
        {
            success = await SwapPlayerToScp(player, RoleTypeId.Scp096);
            response = success ? "You will swap to 096" : "You don't have enough ZVC to do that!";
        }
        else if (arguments.Array[1].Contains("106"))
        {
            success = SwapPlayerToScp(player, RoleTypeId.Scp106);
            response = success ? "You will swap to 106" : "You don't have enough ZVC to do that!";
        }
        else if (arguments.Array[1].Contains("3114"))
        {
            success = SwapPlayerToScp(player, RoleTypeId.Scp3114);
            response = success ? "You will swap to 3114" : "You don't have enough ZVC to do that!";
        }
        else
        {
            response = "You must choose a valid SCP role!";
        }

        
        return true;
    }

    private static async Task<bool> SwapPlayerToScp(Player player, RoleTypeId role)
    {
        try
        {
            string url = $"{Config.EndpointUrl}/swap/?userid={player.UserId}&price={Plugin.Instance.RoleCosts[role]}";
            Logger.Debug($"Sending POST swap request to: {url}");

            using (HttpClient client = new())
            {
                client.DefaultRequestHeaders.Add("Authorization", Config.ApiKey);

                HttpResponseMessage response = await client.PostAsync(url, null);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    Logger.Warn($"Swap request failed with status code: {response.StatusCode}");
                    return false;
                }

                Logger.Info("Swap request successful (200 OK).");
            }
        }
        catch (Exception ex)
        {
            Logger.Warn($"Failed to send swap request: {ex}");
            return false;
        }

        Plugin.Instance.PlayersThatCanUseSwap =
            Plugin.Instance.PlayersThatCanUseSwap.Where(p => p != player).ToArray();

        player.Role = role;
        return true;
    }
}