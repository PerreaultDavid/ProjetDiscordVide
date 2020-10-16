using BotVide.Classes;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotVide.Commands
{
    public class ExempleCommands : BaseCommandModule
    {
        // Système de Save/Load
        saveload sl = new saveload();

        #region Emojis
        public DiscordEmoji[] Emojis(CommandContext ctx)
        {
            DiscordEmoji[] listeemo = new DiscordEmoji[]
            {
                //DiscordEmoji.FromName(ctx.Client, "NOM_EMOJI"), // Entrer un nom d'émoji
                DiscordEmoji.FromName(ctx.Client, ":+1:"), // Oui
                DiscordEmoji.FromName(ctx.Client, ":-1:"), // Non
            };

            return listeemo;
        }
        #endregion

        #region Liste Commandes / Infos
        [Command("infos")]
        [Description("Informations sur le bot / Liste de commandes")]
        [Aliases("info", "aide", "manuel")]
        public async Task Infos(CommandContext ctx)
        {
            if (ctx.Member == null)
                return;

            await InfosApp(ctx, 1);
        }
        [Command("sinfos")]
        [Description("Informations sur le bot / Liste de commandes - Privé")]
        [Aliases("sinfo", "saide", "smanuel")]
        public async Task InfosPrive(CommandContext ctx)
        {
            if (ctx.Member == null)
                return;

            await InfosApp(ctx, 2);
        }
        #endregion

        #region Exemples Fiche Joueur

        #region Fiche
        [Command("fiche")]
        [Description("Fiche de joueur")]
        [Aliases("joueur", "moi")]
        public async Task ExempleFiche(CommandContext ctx)
        {
            if (ctx.Member == null)
                return;

            string idfile = ctx.User.Id.ToString();
            Vide joueur = sl.Load(idfile);

            if (joueur != null)
            {
                // Envoi la liste des emojis
                DiscordEmoji[] listeemo = Emojis(ctx);

                // Création d'un EMBED
                var embed = new DiscordEmbedBuilder
                {
                    Author = new DiscordEmbedBuilder.EmbedAuthor
                    {
                        IconUrl = ctx.User.AvatarUrl,
                        Name = $"Fiche de {ctx.User.Username}"
                    },
                    Title = $"Nom + Titre",
                    Color = new DiscordColor(0xFCDA00), // Yellow
                };
                // SOUS-TITRE, Texte
                //embed.AddField($"",$"");

                await ctx.RespondAsync("", embed: embed);
                // Pour envoyer en version privé (Le Bot envoit seulement pour l'utilisateur)
                //await ctx.Member.SendMessageAsync("", embed: embed);
            }
            else
            {
                // Texte envoyé en cas de fiche non trouvé
                var embed = new DiscordEmbedBuilder
                {
                    Title = $"Fiche Indisponible",
                    Description = $"**{ctx.User.Username}** vous n'avez pas de fiche",
                    Color = DiscordColor.Red
                };
                await ctx.RespondAsync("", embed: embed);
            }
        }
        #endregion

        #region Fiche Autre
        [Command("who")]
        [Description("Affiche les informations d'un autre utilisateur")]
        [Aliases("qui")]
        public async Task AfficherJoueur(CommandContext ctx, [Description("Joueur")] DiscordMember member)
        {
            if (ctx.Member == null)
                return;

            string idfile = member.Id.ToString();
            Vide joueur = sl.Load(idfile);

            if (joueur != null)
            {
                // Envoi la liste des emojis
                DiscordEmoji[] listeemo = Emojis(ctx);

                // Création d'un EMBED
                var embed = new DiscordEmbedBuilder
                {
                    Author = new DiscordEmbedBuilder.EmbedAuthor
                    {
                        IconUrl = member.AvatarUrl,
                        Name = $"Fiche de {member.Username}"
                    },
                    Title = $"Nom + Titre",
                    Color = new DiscordColor(0xFCDA00), // Yellow
                };
                // SOUS-TITRE, Texte
                //embed.AddField($"",$"");

                await ctx.RespondAsync("", embed: embed);
                // Pour envoyer en version privé (Le Bot envoit seulement pour l'utilisateur)
                //await ctx.Member.SendMessageAsync("", embed: embed);
            }
            else
            {
                // Texte envoyé en cas de fiche non trouvé
                var embed = new DiscordEmbedBuilder
                {
                    Title = $"Fiche Indisponible",
                    Description = $"**{member.Username}** n'a pas de fiche",
                    Color = DiscordColor.Red
                };
                await ctx.RespondAsync("", embed: embed);
            }
        }
        #endregion

        #region Création
        [Command("nouveau")]
        [Description("Créer un nouveau profile")]
        public async Task NouveauPirate(CommandContext ctx,
            [Description("Nom")] string nom_pirate)
        {
            if (ctx.Member == null)
                return;

            string idfile = ctx.User.Id.ToString();
            Vide joueur = sl.Load(idfile);

            if (joueur != null)
            {
                var embed = new DiscordEmbedBuilder
                {
                    Title = $"Joueur - Création",
                    Description = $"**{ctx.User.Username}** vous avez déjà un profile!",
                    Color = DiscordColor.Red
                };
                await ctx.RespondAsync("", embed: embed);
            }
            else
            {
                //sl.Save(CLASSE, idfile);
                DiscordEmoji[] listeemo = Emojis(ctx);

                var embed = new DiscordEmbedBuilder
                {
                    Title = $"Joueur - Création",
                    Description = $"**{ctx.User.Username}** votre fiche est créé!",
                    Color = DiscordColor.Green
                };
                await ctx.RespondAsync("", embed: embed);
            }
        }
        #endregion

        #endregion

        #region Méthodes
        public async Task<int> ChoixOuiNon(CommandContext ctx, DiscordEmbedBuilder message)
        {
            DiscordEmoji[] listeemo = Emojis(ctx);

            // Regroupement des émojis pour les réactions
            DiscordEmoji[] options = { listeemo[0], listeemo[1] };

            // Activé l'interactivité
            var interactivity = ctx.Client.GetInteractivity();

            var msg = await ctx.RespondAsync("", embed: message);

            // Ajoute les réactions
            for (var i = 0; i < options.Length; i++)
                await msg.CreateReactionAsync(options[i]);

            // Attend la réponse
            var em1 = await interactivity.WaitForReactionAsync(xe => xe.Emoji == listeemo[0] || xe.Emoji == listeemo[1], ctx.User, TimeSpan.FromSeconds(60));

            // Vérifie le choix de réponse
            if (em1.Result != null)
            {
                if (em1.Result.Emoji == listeemo[0]) // Oui
                    return 1;
                else if (em1.Result.Emoji == listeemo[1]) // Non
                    return 0;
            }
            else
                return 0;

            return 0;
        }
        public async Task InfosApp(CommandContext ctx, int option)
        {
            DiscordEmoji[] listeemo = Emojis(ctx);

            // Les messages
            var embed = new DiscordEmbedBuilder
            {
                Title = $"Bot",
                Description = $"Ajouter du texte sur votre bot",
                Color = DiscordColor.DarkBlue,
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    Text = "Du texte de plus (Version?)"
                }
            };
            // Ajouter plus de embed si besoin

            if (option == 1) // Version publique
            {
                await ctx.RespondAsync("", embed: embed);
                //await ctx.RespondAsync("", embed: embed2);
            }
            else if (option == 2) // Version privé
            {
                await ctx.Member.SendMessageAsync("", embed: embed);
                //await ctx.Member.SendMessageAsync("", embed: embed2);
            }
        }
        #endregion
    }
}