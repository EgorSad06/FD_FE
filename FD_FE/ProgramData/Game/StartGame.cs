using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace FD_FE.Stages
{
    public class StartGame
    {
        public StartGame()
        {
            Game.Cnct = new FD_Tools.Connection.Connection();
        }
        public async Task<bool> Connect(bool is_host, string ip)
        {
            Game.Cnct.is_host = is_host;
            if (Game.Cnct.SetIP(ip))
                return await Game.Cnct.Connect();
            return false;
        }
        public void Disconnect() => Game.Cnct.Disconnect();
        public async Task<bool> TryStart(List<char> frcts)
        {
            if (Game.Cnct.is_host)
            {
                if ((await Game.Cnct.ReceiveDataS(1))[0] != frcts.Count) return false;
                Game.Cnct.SendData(new byte[1] { (byte)frcts.Count });
            }
            else
            {
                Game.Cnct.SendData(new byte[1] { (byte)frcts.Count });
                if ((await Game.Cnct.ReceiveDataS(1))[0] != frcts.Count) return false;
            }

            Game.SetMode((short)frcts.Count);
            Game.battle = Game.p_global_score = Game.o_global_score = 0;
            Game.p_deck = new Deck();
            Game.o_deck = new Deck();
            Game.slct_cards = new Deck();
            Game.start_turn = Game.Cnct.is_host;

            foreach (char f in frcts) Game.slct_cards.deck_cards.AddRange(GameplayData.StartCards[f]);

            if (Game.Cnct.is_host) Game.p_seed = Game.o_seed = BitConverter.ToInt32(await Game.Cnct.ReceiveData(4), 0);
            else Game.Cnct.SendData( BitConverter.GetBytes(Game.p_seed = Game.o_seed = new Random().Next()) );
            return true;
        }
    }
}