using Amino.Interactions.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amino;
using System.Security.Cryptography.X509Certificates;
namespace Amino.Interactions
{
    public class InteractionBase
    {


        public Task<Amino.Objects.Message> Respond(Interaction context, string message, bool asReply = true)
        {
            SubClient subClient = new SubClient(context.AminoClient, context.Message.communityId.ToString());
            var ReturnMessage = subClient.send_message(message, context.InteractionChatId, replyTo: asReply ? context.Message.messageId : null);
            return Task.FromResult(ReturnMessage);
        }

        public Task RespondWithFile(Interaction context, byte[] file, Amino.Types.upload_File_Types type = Types.upload_File_Types.Image)
        {
            SubClient subClient = new SubClient(context.AminoClient, context.Message.communityId.ToString());
            subClient.send_file_message(context.InteractionChatId, file, type);
            return Task.CompletedTask;
        }

        public Task RespondWithSticker(Interaction context, string stickerId)
        {
            SubClient subClient = new SubClient(context.AminoClient, context.Message.communityId.ToString());
            subClient.send_sticker(context.InteractionChatId, stickerId);
            return Task.CompletedTask;
        }

        public Task RespondWithEmbed(Interaction context, string content, string embedId = null, string embedLink = null, string embedTitle = null, string embedContent = null, byte[] embedImage = null)
        {
            SubClient subClient = new SubClient(context.AminoClient, context.Message.communityId.ToString());
            subClient.send_embed(context.InteractionChatId, content, embedId, embedLink, embedTitle, embedContent, embedImage);
            return Task.CompletedTask;
        }

    }
}
