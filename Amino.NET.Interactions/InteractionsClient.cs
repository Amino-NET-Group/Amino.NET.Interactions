using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Amino.Interactions
{
    public partial class InteractionsClient
    {

        public Dictionary<string, Objects.InteractionModule> InteractionModules;
        public Queue<Objects.Interaction> InteractionQueue;

        public enum LogLevels
        {
            None,
            Debug,
            Warning,
            Error
        }

        private Amino.Client AminoClient;
        public int InteractionCooldown = 2000;
        public string InteractionPrefix = "/";
        public bool IgnoreSelf = true;
        public LogLevels LogLevel = LogLevels.None;



        public InteractionsClient(Amino.Client client)
        {
            this.AminoClient = client;
            this.InteractionQueue = new Queue<Objects.Interaction>();
            this.InteractionModules = new Dictionary<string, Objects.InteractionModule>();
            _ = Task.Run(async () => { HandleInteractionQueue(); });
        }


        public Task RegisterModule<T>() where T : InteractionBase
        {
            
        }

        public Task RegisterModules(Assembly entrypoint)
        {
            
        }
        
        public bool HandleInteraction(Objects.Interaction interaction)
        {

        }

        private async Task HandleInteractionQueue()
        {
            while(true)
            {
                if(InteractionQueue.Count != 0)
                {
                    HandleInteraction(InteractionQueue.Dequeue());
                }
                await Task.Delay(this.InteractionCooldown);
            }
        }


    }
}
